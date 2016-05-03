' VisualCodeGrepper - Code security scanner
' Copyright (C) 2012-2014 Nick Dunn and John Murray
'
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with this program.  If not, see <http://www.gnu.org/licenses/>.

Option Explicit On

Imports System.Text.RegularExpressions

Module modJavaCheck

    ' Specific checks for Java code
    '==============================

    Public Sub CheckJavaCode(ByVal CodeLine As String, ByVal FileName As String)
        ' Carry out any specific checks for the language in question
        '===========================================================

        ' Is there a struts validator or similar framework in place?
        If ctCodeTracker.HasValidator = False And FileName.ToLower.EndsWith(".xml") And CodeLine.ToLower.Contains("<plug-in") And CodeLine.ToLower.Contains(".validator") Then ctCodeTracker.HasValidator = True

        CheckServlet(CodeLine, FileName)            ' Check for any issues specific to servlets
        CheckSQLiValidation(CodeLine, FileName)     ' Check for potential SQLi
        CheckXSSValidation(CodeLine, FileName)      ' Check for potential XSS
        CheckRunTime(CodeLine, FileName)            ' Check use of java.lang.Runtime.exec
        CheckIsHttps(CodeLine, FileName)            ' Check any URLs being used are not http
        CheckClone(CodeLine, FileName)              ' Check for unsafe cloning implementation
        CheckSerialize(CodeLine, FileName)          ' Check for unsafe serialization implementation
        IdentifyServlets(CodeLine)                  ' List any servlet instantiations for the thread checks
        CheckModifiers(CodeLine, FileName)          ' Check for public variables in classes
        CheckThreadIssues(CodeLine, FileName)       ' Check for good/bad thread management
        CheckUnsafeTempFiles(CodeLine, FileName)    ' Check for temp files with obvious names
        CheckPrivileged(CodeLine, FileName)         ' Check for potential user access to code with system privileges
        CheckRequestDispatcher(CodeLine, FileName)  ' Check for user control of request dispatcher
        CheckXXEExpansion(CodeLine, FileName)       ' Check for safe/unsafe XML expansion
        CheckOverflow(CodeLine, FileName)           ' Check use of primitive types and any operations upon them
        CheckResourceRelease(CodeLine, FileName)    ' Are file resources safely handled in try ... catch blocks

        ' Identify any nested classes (if required by user)
        If asAppSettings.IsInnerClassCheck Then CheckInnerClasses(CodeLine, FileName)

        ' Carry out any Android-specific checks
        If asAppSettings.IsAndroid = True Then
            CheckAndroidStaticCrypto(CodeLine, FileName)
            CheckAndroidIntent(CodeLine, FileName)
        End If

    End Sub

    Private Sub CheckServlet(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether or not the module contains code for a servlet
        ' Check for any problems specific to servlets if necessary
        '================================================================
        Dim arrFragments As String()
        Dim strServletName As String = ""


        '== Determine whether this is a servlet in order to check for servlet-specific problems ==
        If ctCodeTracker.IsServlet = False And (CodeLine.Contains("extends HttpServlet") Or CodeLine.Contains("implements Servlet")) Then
            ctCodeTracker.IsServlet = True

            '== Store servlet name for thread-saftey checks ==
            arrFragments = Regex.Split(CodeLine, "(extends\s+HttpServlet|implements\s+Servlet)")
            strServletName = GetLastItem(arrFragments.First)

            If strServletName = "" Then Exit Sub

            ctCodeTracker.ServletName = strServletName
            If (Not ctCodeTracker.ServletNames.Contains(strServletName)) Then ctCodeTracker.ServletNames.Add(strServletName)

        End If

        '== Check for Thread.sleep() in servlet ==
        If ctCodeTracker.IsServlet = True And CodeLine.Contains("Thread.sleep") Then
            frmMain.ListCodeIssue("Use of Thread.sleep() within a Java servlet", "The use of Thread.sleep() within Java servlets is discouraged", FileName)
        End If

        '== List any getter and setter methods for the servlet's instance variables so that we can check these are handled in a thread-safe manner ==
        If ctCodeTracker.IsServlet = True Then IdentifyGetAndSet(CodeLine)

    End Sub

    Private Sub CheckSQLiValidation(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for any SQL injection problems 
        '=====================================
        Dim strVarName As String = ""   ' Holds the variable name for the dynamic SQL statement


        '== Only check unvalidated code ==
        If ctCodeTracker.HasValidator = True Then Exit Sub


        '== Is unsanitised dynamic SQL statement prepared beforehand? ==
        If CodeLine.Contains("=") And (CodeLine.ToLower.Contains("sql") Or CodeLine.ToLower.Contains("query")) And (CodeLine.Contains("""") And CodeLine.Contains("+")) Then
            '== Extract variable name from assignment statement ==
            strVarName = GetVarName(CodeLine)
            ctCodeTracker.HasVulnSQLString = True
            If Not ctCodeTracker.SQLStatements.Contains(strVarName) And _
                (Not (strVarName.Contains("(") Or strVarName.Contains(")") Or strVarName.Contains("[") Or strVarName.Contains("]") Or strVarName.Contains(" ") Or strVarName.Contains("+") Or strVarName.Contains("*"))) Then ctCodeTracker.SQLStatements.Add(strVarName)
        End If


        If Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise") Then

            '== Remove any variables which have been sanitised from the list of vulnerable variables ==  
            If ctCodeTracker.SQLStatements.Count > 0 Then
                For Each strVar In ctCodeTracker.SQLStatements

                    If Regex.IsMatch(CodeLine, strVar & "\s*\=\s*\S*validate|encode|sanitize|sanitise\S*\(" & strVar) Then
                        ctCodeTracker.SQLStatements.Remove(strVar)
                        Exit For
                    End If
                Next
            End If
        ElseIf Regex.IsMatch(CodeLine, "\S*\.(prepareStatement|executeQuery|query|queryForObject|queryForList|queryForInt|queryForMap|update|getQueryString|executeQuery|createNativeQuery|createQuery)\s*\(") Then

            '== Check usage of java.sql.Statement.executeQuery, etc. ==
            If CodeLine.Contains("""") And CodeLine.Contains("+") Then
                '== Dynamic SQL built into connection/update ==
                frmMain.ListCodeIssue("Potential SQL Injection", "The application appears to allow SQL injection via dynamic SQL statements. No validator plug-ins were located in the application's XML files.", FileName, CodeIssue.CRITICAL, CodeLine)
            ElseIf ctCodeTracker.HasVulnSQLString = True Then
                '== Otherwise check for use of pre-prepared statements ==
                For Each strVar In ctCodeTracker.SQLStatements
                    If CodeLine.Contains(strVar) Then
                        frmMain.ListCodeIssue("Potential SQL Injection", "The application appears to allow SQL injection via a pre-prepared dynamic SQL statement. No validator plug-ins were located in the application's XML files.", FileName, CodeIssue.CRITICAL, CodeLine)
                        Exit For
                    End If
                Next
            End If
        End If

    End Sub

    Private Sub CheckXSSValidation(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for any XSS problems 
        '===========================

        Dim arrFragments As String()    ' Holds the parts of any line containing HttpRequest var assignments
        Dim strVarName As String = ""   ' Holds the variable name assigned to any HttpRequest data


        '== Only check unvalidated code ==
        If ctCodeTracker.HasValidator = True Then Exit Sub


        '== Identify any HttpRequest variables in servlets ==
        If CodeLine.Contains("HttpServletRequest ") And Not CodeLine.Contains("import ") Then

            arrFragments = Regex.Split(CodeLine, "HttpServletRequest ")
            strVarName = arrFragments.Last.Trim

            If strVarName <> "" Then
                '== Variable name should be immediately followed by either whitespace, a comma, an equals sign or a semi-colon
                arrFragments = Regex.Split(strVarName, "[,;=\s+]")
                strVarName = arrFragments.First.Trim
                ctCodeTracker.HasHttpRequestData = True
                ctCodeTracker.HttpRequestVar = strVarName
            End If

        ElseIf FileName.ToLower.EndsWith(".jsp") And Regex.IsMatch(CodeLine, "\s*\S*\s*={1}?\s*\S*\s*\brequest\b\.\bgetParameter\b") Then

            '== Identify any GET parameters assigned to variables ==
            strVarName = GetVarName(CodeLine, True)

            ' Add variable to dictionary if it's alphanumeric (we have not accidentally caught an expression)
            ctCodeTracker.HasGetVariables = True
            If Regex.IsMatch(strVarName, "^[a-zA-Z0-9_]*$") And Not ctCodeTracker.SQLStatements.Contains(strVarName) Then ctCodeTracker.GetVariables.Add(strVarName)

        ElseIf FileName.ToLower.EndsWith(".jsp") And Regex.IsMatch(CodeLine, "\<\%\=\s*\w+\.getParameter\s*\(") Then
            frmMain.ListCodeIssue("Potential XSS", "The application appears to reflect a HTTP request parameter to the screen with no apparent validation or sanitisation.", FileName, CodeIssue.HIGH, CodeLine)

        ElseIf FileName.ToLower.EndsWith(".jsp") And ctCodeTracker.GetVariables.Count > 0 And (CodeLine.ToLower.Contains("validate") Or CodeLine.ToLower.Contains("encode") Or CodeLine.ToLower.Contains("sanitize") Or CodeLine.ToLower.Contains("sanitise")) Then

            '== Check for sanitisation of any GET/POST params and remove from dictionary ==
            If ctCodeTracker.GetVariables.Count > 0 Then
                For Each strVar In ctCodeTracker.GetVariables
                    If Not (strVar.contains("(") Or strVar.contains(")") Or strVar.contains("[") Or strVar.contains("]") Or strVar.contains(" ") Or strVar.contains("+") Or strVar.contains("*")) Then
                        If Regex.IsMatch(CodeLine, strVar & "\s*\=\s*\S*validate|encode|sanitize|sanitise\S*\(" & strVar) Then
                            ctCodeTracker.GetVariables.Remove(strVar)
                            Exit For
                        End If
                    End If
                Next
            End If
            
        ElseIf ctCodeTracker.HasHttpRequestData = True And CodeLine.Contains(ctCodeTracker.HttpRequestVar) Then

            '== If sanitisation takes place reset all HttpRequest variables ==
            If CodeLine.ToLower.Contains("validate") Or CodeLine.ToLower.Contains("encode") Or CodeLine.ToLower.Contains("sanitize") Or CodeLine.ToLower.Contains("sanitise") Then
                ctCodeTracker.HasHttpRequestData = False
                ctCodeTracker.HttpRequestVar = ""
            ElseIf (CodeLine.Contains("getCookies") Or CodeLine.Contains("getHeader") Or CodeLine.Contains("getPart") Or CodeLine.Contains("getQueryString") Or CodeLine.Contains("getParameter") Or CodeLine.Contains("getRequestUR")) Then

                '== If this point has been reached then variables probably used without sanitisation ==
                If FileName.ToLower.EndsWith(".jsp") Then
                    frmMain.ListCodeIssue("Potential XSS", "The application appears to use data contained in the HttpServletRequest without validation or sanitisation. No validator plug-ins were located in the application's XML files.", FileName, CodeIssue.HIGH, CodeLine)
                Else
                    frmMain.ListCodeIssue("Poor Input Validation", "The application appears to use data contained in the HttpServletRequest without validation or sanitisation. No validator plug-ins were located in the application's XML files.", FileName, CodeIssue.HIGH, CodeLine)
                End If
                
            End If

        ElseIf (FileName.ToLower.EndsWith(".jsp") And Regex.IsMatch(CodeLine, "<%=\s*\S*\bsession\b\.\bgetAttribute\b\s*\(")) Then
            '== Check JSPs for session variables being reflected back to page ==
            frmMain.ListCodeIssue("Potential XSS", "The JSP displays a session variable directly to the screen. No validator plug-ins were located in the application's XML files.", FileName, CodeIssue.HIGH, CodeLine)

        ElseIf FileName.ToLower.EndsWith(".jsp") And ctCodeTracker.GetVariables.Count > 0 And CodeLine.Contains("<%=") Then

            '== Check for get params being reflected to page without sanitisation ==
            For Each strVar In ctCodeTracker.GetVariables
                If Not (strVar.contains("(") Or strVar.contains(")") Or strVar.contains("[") Or strVar.contains("]") Or strVar.contains(" ") Or strVar.contains("+") Or strVar.contains("*")) Then
                    If Regex.IsMatch(CodeLine, "<%=\s*\S*\s*\b" & strVar & "\b") Then
                        frmMain.ListCodeIssue("Potential XSS", "The JSP displays directly a user-supplied parameter directly to the screen. No validator plug-ins were located in the application's XML files.", FileName, CodeIssue.HIGH, CodeLine)
                    End If
                End If
            Next

        ElseIf FileName.ToLower.EndsWith(".jsp") And Regex.IsMatch(CodeLine, "<c:\bout\b\s*\S*\s*=\s*['""]\s*\$\{\s*\S*\}\s*['""]\s*\bescapeXML\b\s*=\s*['""]\bfalse\b['""]\s*/>") Then
            '== Check JSPs for variables being reflected back to page without XML encoding ==
            frmMain.ListCodeIssue("Potential XSS", "The JSP displays application data without applying XML encoding. No validator plug-ins were located in the application's XML files.", FileName, CodeIssue.HIGH, CodeLine)
        End If

    End Sub

    Private Sub CheckRunTime(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether or not the module uses java.lang.Runtime.exec
        ' Check for any any unsafe usage if necessary
        '================================================================

        '== Check for use of java.lang.Runtime ==
        If CodeLine.Contains("Runtime.") Then ctCodeTracker.IsRuntime = True

        '== Check for unsafe use of java.lang.Runtime.exec ==
        If ctCodeTracker.IsRuntime And (CodeLine.Contains(".exec ") Or CodeLine.Contains(".exec(")) And (Not CodeLine.Contains("""")) Then
            frmMain.ListCodeIssue("java.lang.Runtime.exec Gets Path from Variable", "The pathname used in the call appears to be loaded from a variable. Check the code manually to ensure that malicious filenames cannot be submitted by an attacker.", FileName, CodeIssue.HIGH, CodeLine)
        End If

    End Sub

    Private Sub CheckIsHttps(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether or not the code connects to external URLs
        ' Check for any any unsafe usage if necessary
        '============================================================

        '== Check for secure HTTP usage ==
        If CodeLine.Contains("URLConnection") And CodeLine.Contains("HTTP:") Then
            frmMain.ListCodeIssue("URL request sent over HTTP:", "The URL used in the HTTP request appears to be unencrypted. Check the code manually to ensure that sensitive data is not being submitted.", FileName, CodeIssue.STANDARD, CodeLine)
        ElseIf (CodeLine.Contains("URLConnection(") Or CodeLine.Contains("URLConnection (")) And (Not CodeLine.Contains("""")) Then
            frmMain.ListCodeIssue("URL Request Gets Path from Variable", "The URL used in the HTTP request appears to be loaded from a variable. Check the code manually to ensure that malicious URLs cannot be submitted by an attacker.", FileName, CodeIssue.STANDARD, CodeLine)
        End If

    End Sub

    Private Sub CheckClone(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether or not the code implements cloning
        ' Check for any any unsafe usage if necessary
        '=====================================================

        '== Check for safe or unsafe implementation of cloning ==
        If Regex.IsMatch(CodeLine, "\bpublic\b\s+\w+\s+\bclone\b\s*\(") Then
            ctCodeTracker.ImplementsClone = True
        End If
        If ctCodeTracker.ImplementsClone = True And CodeLine.Contains("throw new java.lang.CloneNotSupportedException") Then
            ctCodeTracker.ImplementsClone = False
        End If

    End Sub

    Private Sub CheckSerialize(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether or not the code implements serialization
        ' Check for any any unsafe usage if necessary
        '===========================================================

        '== Check for safe or unsafe implementation of serialization/deserialization ==
        If CodeLine.Contains(" writeObject") Then ctCodeTracker.IsSerialize = True
        If CodeLine.Contains(" readObject") Then ctCodeTracker.IsDeserialize = True

        If ctCodeTracker.IsSerialize = True And ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.SerializeBraces) And CodeLine.Contains("throw new java.io.IOException") Then
            ctCodeTracker.IsSerialize = False
        End If
        If ctCodeTracker.IsDeserialize = True And ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.DeserializeBraces) And CodeLine.Contains("throw new java.io.IOException") Then
            ctCodeTracker.IsDeserialize = False
        End If

    End Sub

    Private Sub CheckModifiers(ByVal CodeLine As String, ByVal FileName As String)
        ' Identify any public variables in classes
        '=========================================
        Dim strVarName As String = ""


        '== Check for public variables and non-final classes (finalize check is optional) ==
        If CodeLine.Contains("public ") And CodeLine.Contains(";") And Not (CodeLine.Contains("{") Or CodeLine.Contains("abstract ") Or CodeLine.Contains("class ") Or CodeLine.Contains("static ")) Then

            strVarName = GetVarName(CodeLine)
            If Regex.IsMatch(strVarName, "^[a-zA-Z0-9_]*$") Then
                frmMain.ListCodeIssue("Class Contains Public Variable: " & strVarName, "The class variable may be accessed and modified by other classes without the use of getter/setter methods. It is considered unsafe to have public fields or methods in a class unless required as any method, field, or class that is not private is a potential avenue of attack. It is safer to provide accessor methods to variables in order to limit their accessibility.", FileName, CodeIssue.STANDARD, CodeLine)
            End If

            '== Store public variable name for any thread safety checks if this is a servlet ==
            If ctCodeTracker.IsServlet Then
                If Not ctCodeTracker.GlobalVars.ContainsKey(strVarName) Then ctCodeTracker.GlobalVars.Add(strVarName, ctCodeTracker.ServletName)
            End If

        ElseIf asAppSettings.IsFinalizeCheck And (CodeLine.Contains("public ") And CodeLine.Contains("class ")) And Not CodeLine.Contains("final ") Then
            frmMain.ListCodeIssue("Public Class Not Declared as Final", "The class is not declared as final as per OWASP recommendations. It is considered best practice to make classes final where possible and practical (i.e. It has no classes which inherit from it). Non-Final classes can allow an attacker to extend a class in a malicious manner. Manually inspect the code to determine whether or not it is practical to make this class final.", FileName, CodeIssue.POSSIBLY_SAFE, CodeLine)
        End If

    End Sub

    Private Sub CheckInnerClasses(ByVal CodeLine As String, ByVal FileName As String)
        ' Identify any inner classes within classes
        '==========================================

        '== Check for entry into class ==
        If Not ctCodeTracker.IsInsideClass And Regex.IsMatch(CodeLine, "\bpublic\b\s*\bclass\b") Then
            If CodeLine.Contains("{") Then
                ctCodeTracker.IsInsideClass = ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.ClassBraces)
            Else
                ctCodeTracker.IsInsideClass = True
            End If
        ElseIf ctCodeTracker.IsInsideClass Then
            If CodeLine.Contains("private ") And CodeLine.Contains("class ") Then
                frmMain.ListCodeIssue("Class Contains Inner Class", "When translated into bytecode, any inner classes are rebuilt within the JVM as external classes within the same package. As a result, any class in the package can access these inner classes. The enclosing class's private fields become protected fields, accessible by the now external 'inner class'.", FileName, CodeIssue.STANDARD, CodeLine)
            End If
            ctCodeTracker.IsInsideClass = ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.ClassBraces)
        End If

    End Sub

    Private Sub CheckThreadIssues(ByVal CodeLine As String, ByVal FileName As String)
        ' Identify potential for race conditions and deadlocking
        '=======================================================
        Dim blnIsRace As Boolean = False
        Dim strSyncObject As String = ""



        '== Identify object locked for use in synchronized block ==
        If ctCodeTracker.IsSynchronized = False And Regex.IsMatch(CodeLine, "\bsynchronized\b\s*\(\s*\w+\s*\)") Then
            strSyncObject = GetSyncObject(CodeLine)
            ctCodeTracker.LockedObject = strSyncObject
            ctCodeTracker.SyncIndex += 1
        End If



        '== Identify entry into a synchronized block ==
        '== The synchronized may be followed by method type and name for a synchronized method, or by braces for a synchronized block ==
        If ctCodeTracker.IsSynchronized = False And Regex.IsMatch(CodeLine, "\bsynchronized\b\s*\S*\s*\S*\s*\(") Then
            If CodeLine.Contains("{") Then
                ctCodeTracker.IsSynchronized = ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.SyncBraces)
            Else
                ctCodeTracker.IsSynchronized = True
            End If

        ElseIf ctCodeTracker.IsSynchronized = False And ctCodeTracker.IsServlet = True Then

            '== Check for any unsafe modifications to instance variables == 
            If ctCodeTracker.GlobalVars.Count > 0 Then

                For Each itmItem In ctCodeTracker.GlobalVars
                    If CodeLine.Contains(itmItem.Key) Then
                        frmMain.ListCodeIssue("Possible Race Condition", "A HttpServlet instance variable is being used/modified without a synchronize block: " & itmItem.Key & vbNewLine & "Check if any code which calls this code is thread-safe.", FileName, CodeIssue.MEDIUM)
                        Exit For
                    End If
                Next
            End If

        ElseIf ctCodeTracker.IsSynchronized = False Then

            '== Check for any unsafe modifications to instance variables == 
            If ctCodeTracker.GlobalVars.Count > 0 Then
                For Each itmItem In ctCodeTracker.GlobalVars
                    blnIsRace = CheckRaceCond(CodeLine, FileName, itmItem)
                    If blnIsRace Then Exit For
                Next
            End If

            If blnIsRace = False And ctCodeTracker.GetSetMethods.Count > 0 Then
                For Each itmItem In ctCodeTracker.GetSetMethods
                    blnIsRace = CheckRaceCond(CodeLine, FileName, itmItem)
                    If blnIsRace Then Exit For
                Next
            End If

        ElseIf ctCodeTracker.IsSynchronized Then
            '== Track the amount of code that is inside the lock - resources may be locked unnecessarily ==
            If (CodeLine.Trim <> "{" And CodeLine.Trim <> "}") Then ctCodeTracker.SyncLineCount += 1

            '== Check whether still inside synchronized code ==
            ctCodeTracker.IsSynchronized = ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.SyncBraces)

            '== Check for large areas of locked code and potential deadlock ==
            CheckSyncIssues(CodeLine, FileName)
        End If

    End Sub

    Private Sub IdentifyServlets(ByVal CodeLine As String)
        ' Identify any instantiation of Servlet Classes and store the object names for thread safety checks
        '==================================================================================================
        Dim strVarName As String = ""


        If (CodeLine.Contains("public ") Or CodeLine.Contains("private ") Or CodeLine.Contains("protected ")) And CodeLine.Contains(";") And Not CodeLine.Contains("{") And Not CodeLine.Contains("abstract ") Then
            For Each strName In ctCodeTracker.ServletNames
                If CodeLine.Contains(strName) Then
                    strVarName = GetVarName(CodeLine)
                    If Not ctCodeTracker.ServletInstances.ContainsKey(strVarName) Then ctCodeTracker.ServletInstances.Add(strVarName, strName)
                    Exit For
                End If
            Next
        End If

    End Sub

    Private Sub IdentifyGetAndSet(ByVal CodeLine As String)
        ' Identify any getter and setter methods within Servlet Classes and store the object names for thread safety checks
        '==================================================================================================================
        Dim strMethodName As String = ""
        Dim arrFragments As String()


        '== Do we have a susceptible method? ==
        If Regex.IsMatch(CodeLine, "\s*\bpublic\b\s+\S*\s+(g|s)et\S+\s*\(") Then

            '== Extract method name ==
            arrFragments = CodeLine.Split("(")
            strMethodName = arrFragments.First
            strMethodName = GetLastItem(strMethodName)

            If Not ctCodeTracker.GetSetMethods.ContainsKey(strMethodName) Then ctCodeTracker.GetSetMethods.Add(strMethodName, ctCodeTracker.ServletName)

        End If

    End Sub

    Private Function CheckRaceCond(ByVal CodeLine As String, ByVal FileName As String, ByVal DictionaryItem As KeyValuePair(Of String, String)) As Boolean
        ' Check if line contains any references to public variables of servlets or to getter/setter methods of servlets
        '==============================================================================================================
        Dim strServletName As String = ""
        Dim arrFragments As String()
        Dim blnRetVal As Boolean = False


        If CodeLine.Contains("." & DictionaryItem.Key) Then
            arrFragments = Regex.Split(CodeLine, "." & DictionaryItem.Key)
            strServletName = GetLastItem(arrFragments.First)
            If ctCodeTracker.ServletInstances.Count > 0 And ctCodeTracker.ServletInstances.ContainsKey(strServletName) Then
                If DictionaryItem.Value = ctCodeTracker.ServletInstances.Item(strServletName) Then
                    frmMain.ListCodeIssue("Possible Race Condition", "A HttpServlet instance variable is being used/modified without a synchronize block.", FileName, CodeIssue.HIGH)
                    blnRetVal = True
                End If
            End If
        End If

        Return blnRetVal

    End Function

    Private Sub CheckUnsafeTempFiles(ByVal CodeLine As String, ByVal FileName As String)
        ' Identify any creation of temp files with static names
        '======================================================

        If Regex.IsMatch(CodeLine, "\bnew\b\s+File\s*\(\s*\""*\S*(temp|tmp)\S*\""\s*\)") Then
            frmMain.ListCodeIssue("Unsafe Temporary File Allocation", "The application appears to create a temporary file with a static, hard-coded name. This causes security issues in the form of a classic race condition (an attacker creates a file with the same name between the application's creation and attempted usage) or a symbolic linbk attack where an attacker creates a symbolic link at the temporary file location.", FileName, CodeIssue.MEDIUM, CodeLine)
        End If

    End Sub

    Private Function GetSyncObject(ByVal CodeLine As String) As String
        ' Extract the name of a synchronized object from a line of code
        '==============================================================
        Dim strSyncObject As String = ""
        Dim strFragments As String()


        strFragments = Regex.Split(CodeLine, "\bsynchronized\b\s*\(")
        strSyncObject = GetFirstItem(strFragments.Last, ")")
        If strSyncObject <> "" Then ctCodeTracker.LockedObject = strSyncObject

        Return strSyncObject

    End Function

    Private Sub CheckSyncIssues(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for, and report on, any issues located inside the synchronized block or when leaving the block
        '=====================================================================================================
        Dim intSeverity As Integer = 0
        Dim intIndex As Integer = 0
        Dim strSyncObject As String = ""
        Dim strOuterSyncObject As String = ""


        '== Report potentially excessive locking when leaving the code block ==
        If ctCodeTracker.IsSynchronized = False Then

            If ctCodeTracker.SyncLineCount > 14 Then
                intSeverity = CodeIssue.MEDIUM
            ElseIf ctCodeTracker.SyncLineCount > 10 Then
                intSeverity = CodeIssue.STANDARD
            ElseIf ctCodeTracker.SyncLineCount > 6 Then
                intSeverity = CodeIssue.LOW
            End If

            If ctCodeTracker.SyncLineCount > 6 Then
                frmMain.ListCodeIssue("Synchronized Code - Possible Performance Impact", "There are " & ctCodeTracker.SyncLineCount & " lines of code in the synchronized block. Manually check the code to ensure any shared resources are not being locked unnecessarily.", FileName, intSeverity)
            End If

            ctCodeTracker.SyncLineCount = 0

        ElseIf ctCodeTracker.LockedObject <> "" And Regex.IsMatch(CodeLine, "\bsynchronized\b\s*\(\s*\w+\s*\)") Then
            '== Build dictionary for potential deadlocks by tracking synchronized blocks inside synchronized blocks ==
            strOuterSyncObject = ctCodeTracker.LockedObject
            strSyncObject = GetSyncObject(CodeLine)

            If strSyncObject <> "" Then
                '== Check if this sync block already exists ==
                For Each itmItem In ctCodeTracker.SyncBlockObjects
                    If itmItem.BlockIndex = ctCodeTracker.SyncIndex Then
                        intIndex = itmItem.BlockIndex
                        '== Add to existing block ==
                        If Not itmItem.InnerObjects.Contains(strSyncObject) Then itmItem.InnerObjects.Add(strSyncObject)
                        Exit For
                    End If
                Next

                '== Create new sync block an add inner object name ==
                If intIndex = 0 Then AddNewSyncBlock(strOuterSyncObject, strSyncObject)

                CheckDeadlock(strOuterSyncObject, strSyncObject, FileName)

            End If
        End If

    End Sub

    Public Sub AddNewSyncBlock(ByVal OuterObject As String, ByVal InnerObject As String)
        ' Initialise a new syncblock container to hold details of locked items
        '=====================================================================
        Dim sbSyncBlock As New SyncBlock

        sbSyncBlock.BlockIndex = ctCodeTracker.SyncIndex
        sbSyncBlock.OuterObject = OuterObject
        sbSyncBlock.InnerObjects.Add(InnerObject)

        ctCodeTracker.SyncBlockObjects.Add(sbSyncBlock)

    End Sub

    Public Sub CheckDeadlock(ByVal OuterObject As String, ByVal InnerObject As String, ByVal FileName As String)
        ' Check whether the locked object combination has a reverse block where the inner item and outer item swap places
        '================================================================================================================

        For Each itmItem In ctCodeTracker.SyncBlockObjects
            If itmItem.OuterObject = InnerObject And itmItem.InnerObjects.Contains(OuterObject) Then
                frmMain.ListCodeIssue("Synchronized Code May Result in DeadLock", "The objects " & OuterObject & " and " & InnerObject & " lock each other in such a way that they may become deadlocked.", FileName, CodeIssue.MEDIUM)
                Exit For
            End If
        Next

    End Sub

    Private Sub CheckPrivileged(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for unsafe use of privileged blocks
        '==========================================
        Dim intSeverity As Integer = 0


        '== The IsInsideClass variable tracks whether we are inside a public class and can be re-used here ==
        If ctCodeTracker.IsInsideClass = True Then

            '== Check for public method ==
            If ctCodeTracker.IsInsideMethod = False And Regex.IsMatch(CodeLine, "\bpublic\b\s+\w+\s+\w+\s*\w*\s*\(") Then

                If CodeLine.Contains("{") Then
                    ctCodeTracker.IsInsideMethod = ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.MethodBraces)
                Else
                    ctCodeTracker.IsInsideMethod = True
                End If

            ElseIf ctCodeTracker.IsInsideMethod = True And Regex.IsMatch(CodeLine, "\bAccessController\b\.\bdoPrivileged\b") Then

                If CodeLine.Contains("{") Then
                    ctCodeTracker.IsPrivileged = ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.PrivBraces)
                Else
                    ctCodeTracker.IsPrivileged = True
                End If
                frmMain.ListCodeIssue("Use of AccessController.doPrivileged() in Public Method of Public Class", "The code will execute with system privileges and should be manually checked with great care to ensure no vulnerabilities are present.", FileName, CodeIssue.MEDIUM, CodeLine)

            ElseIf ctCodeTracker.IsPrivileged = False And Regex.IsMatch(CodeLine, "\bAccessController\b\.\bdoPrivileged\b") Then

                If CodeLine.Contains("{") Then
                    ctCodeTracker.IsPrivileged = ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.PrivBraces)
                Else
                    ctCodeTracker.IsPrivileged = True
                End If

            ElseIf ctCodeTracker.IsPrivileged = True Then

                '== Track the amount of code that is inside the lock - resources may be locked unnecessarily ==
                If (CodeLine.Trim <> "{" And CodeLine.Trim <> "}") Then ctCodeTracker.PrivLineCount += 1
                ctCodeTracker.IsPrivileged = ctCodeTracker.TrackBraces(CodeLine, ctCodeTracker.PrivBraces)

                '== If we've exited then give notice of excessively large privileged blocks ==
                If ctCodeTracker.IsInsideMethod = True And ctCodeTracker.IsPrivileged = False Then

                    If ctCodeTracker.PrivLineCount > 20 Then
                        intSeverity = CodeIssue.MEDIUM
                    ElseIf ctCodeTracker.PrivLineCount > 15 Then
                        intSeverity = CodeIssue.STANDARD
                    ElseIf ctCodeTracker.PrivLineCount > 10 Then
                        intSeverity = CodeIssue.LOW
                    End If

                    If ctCodeTracker.PrivLineCount > 10 Then
                        frmMain.ListCodeIssue("Privileged Code - Possible Risks", "There are " & ctCodeTracker.PrivLineCount & " lines of code in the privileged block. Manually check the code to ensure no unnecessary code is included.", FileName, intSeverity)
                    End If
                    ctCodeTracker.PrivLineCount = 0
                ElseIf ctCodeTracker.IsPrivileged = True Then
                    '== Check for use of user-controlled variables within privileged code ==
                    For Each strVar In ctCodeTracker.GetVariables
                        If Not (strVar.contains("(") Or strVar.contains(")") Or strVar.contains("[") Or strVar.contains("]") Or strVar.contains(" ") Or strVar.contains("+") Or strVar.contains("*")) Then
                            If Regex.IsMatch(CodeLine, "\b" & strVar & "\b") Then
                                frmMain.ListCodeIssue("Use of User-Controlled Variable Within Privileged Code", "The code will execute with system privileges and the usage of the variable should be manually checked with great care.", FileName, CodeIssue.HIGH, CodeLine)
                                Exit For
                            End If
                        End If
                    Next
                End If
            End If

        End If

    End Sub

    Private Sub CheckRequestDispatcher(ByVal CodeLine As String, ByVal FileName As String)
        ' Check for unsafe use of RequestDispatcher
        '==========================================

        If Regex.IsMatch(CodeLine, "\.\bgetRequestDispatcher\b\s*\(") Then
            '== Check for use of user-controlled variable within privileged code ==
            For Each strVar In ctCodeTracker.GetVariables
                If Not (strVar.contains("(") Or strVar.contains(")") Or strVar.contains("[") Or strVar.contains("]") Or strVar.contains(" ") Or strVar.contains("+") Or strVar.contains("*")) Then
                    If Regex.IsMatch(CodeLine, "\bgetRequestDispatcher\b\s*\(\s*\S*\s*\S*\s*\b" & strVar & "\b") Then
                        frmMain.ListCodeIssue("Use of RequestDispatcher in Combination with User-Controlled Variable", "The code appears to use a user-controlled variable in a RequestDispatcher method which can allow horizontal directory traversal, allowing an attacker to download system files.", FileName, CodeIssue.HIGH, CodeLine)
                        Exit For
                    End If
                End If
            Next
        End If

    End Sub

    Private Sub CheckXXEExpansion(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether XML expansion is possible and check feasibility of XML-Bomb delivery
        '=======================================================================================

        '== Check for use of XXE parser ==
        If ctCodeTracker.HasXXEEnabled = False And Regex.IsMatch(CodeLine, "import\s+javax\.xml\.bind\.JAXB\s*\;") Then ctCodeTracker.HasXXEEnabled = True

        If ctCodeTracker.HasXXEEnabled = True And Regex.IsMatch(CodeLine, "\(\s*(XMLConstants\.FEATURE_SECURE_PROCESSING|XMLInputFactory.SUPPORT_DTD)\s*\,\s*false\s*\)") Then
            '== Deliberate setting of entity expansion ==
            frmMain.ListCodeIssue("XML Entity Expansion Enabled", "The FEATURE_SECURE_PROCESSING attribute is set to false which can render the application vulnerable to the use of XML bombs. Check the necessity of enabling this feature and check for validation of incoming data.", FileName, CodeIssue.HIGH, CodeLine)
        ElseIf ctCodeTracker.HasXXEEnabled = True And Regex.IsMatch(CodeLine, "\(\s*(XMLConstants\.FEATURE_SECURE_PROCESSING|XMLInputFactory.SUPPORT_DTD)\s*\,\s*true\s*\)") Then
            '== Security settings have been applied ==
            ctCodeTracker.HasXXEEnabled = False
        ElseIf ctCodeTracker.HasXXEEnabled = True Then

        End If

    End Sub

    Private Sub CheckOverflow(ByVal CodeLine As String, ByVal FileName As String)
        ' Identify occurences of primitive types and warn for any potential overflows
        '============================================================================

        '== Identify any primitives and add to dictionary ==
        If Regex.IsMatch(CodeLine, "\b(short|int|long)\b\s+\w+\s*(\=|\;)") Then
            ctCodeTracker.HasPrimitives = True
            ctCodeTracker.AddInteger(CodeLine)
        End If

        '== Warn of any mathematical operations on integers and possible overflows ==
        If ctCodeTracker.HasPrimitives = True And (CodeLine.Contains("+") Or CodeLine.Contains("-") Or CodeLine.Contains("*")) Then
            For Each itmIntItem In ctCodeTracker.GetIntegers
                If Not (itmIntItem.Key.Contains("(") Or itmIntItem.Key.Contains(")") Or itmIntItem.Key.Contains("[") Or itmIntItem.Key.Contains("]") Or itmIntItem.Key.Contains(" ") Or itmIntItem.Key.Contains("+") Or itmIntItem.Key.Contains("*")) Then
                    If Regex.IsMatch(CodeLine, "\b" & itmIntItem.Key & "\b") Then
                        For Each itmVarItem In ctCodeTracker.GetVariables
                            frmMain.ListCodeIssue("Operation on Primitive Data Type", "The code appears to be carrying out a mathematical operation involving a primitive data type and a user-supplied variable. This may result in an overflow and unexpected behaviour. Check the code manually to determine the risk.", FileName, CodeIssue.HIGH, CodeLine)
                            Exit Sub
                        Next
                        frmMain.ListCodeIssue("Operation on Primitive Data Type", "The code appears to be carrying out a mathematical operation on a primitive data type. In some circumstances this can result in an overflow and unexpected behaviour. Check the code manually to determine the risk.", FileName, CodeIssue.LOW, CodeLine)
                        Exit Sub
                    End If
                End If
            Next
        End If

    End Sub

    Private Sub CheckResourceRelease(ByVal CodeLine As String, ByVal FileName As String)
        ' Check that try ... catch blocks are being used to release resources and avoid DoS
        '==================================================================================

        '== Record any instances of filestreams being created ==
        If ctCodeTracker.IsFileOpen = False And Regex.IsMatch(CodeLine, "\bnew\b\s+\bFileOutputStream\b\s*\(") Then
            ctCodeTracker.IsFileOpen = True
            ctCodeTracker.HasResourceRelease = False
            ctCodeTracker.FileOpenLine = rtResultsTracker.LineCount
        End If


        '== Check for safe release of resources in all cases ==
        If ctCodeTracker.IsFileOpen = True And Regex.IsMatch(CodeLine, "\btry\b") Then
            ctCodeTracker.HasTry = True
        ElseIf ctCodeTracker.IsFileOpen = True And Regex.IsMatch(CodeLine, "\bfinally\b") Then
            ctCodeTracker.IsFileOpen = False
            If Regex.IsMatch(CodeLine, "\.\bclose\b\s*\(") Then
                ctCodeTracker.HasResourceRelease = True
            End If
        End If

    End Sub

    Private Sub CheckAndroidStaticCrypto(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether static crypto is being used for Android apps
        '===============================================================

        '== Check for use of static string in crypto command ==
        If Regex.IsMatch(CodeLine, "CryptoAPI\.(encrypt|decrypt)\s*\(\""\w+\""\s*\,") Then
            frmMain.ListCodeIssue("Static Crypto Keys in Use", "The application appears to be using static crypto keys. The absence of secure key storage may allow unauthorised decryption of data.", FileName, CodeIssue.HIGH, CodeLine)
        End If

    End Sub

    Private Sub CheckAndroidIntent(ByVal CodeLine As String, ByVal FileName As String)
        ' Determine whether implicit intents are being used for Android apps
        '===================================================================
        Dim strFragments As String()
        Dim strIntent As String = ""


        '== Check for creation of blank intent ==
        If ctCodeTracker.HasIntent = False And Regex.IsMatch(CodeLine, "\bIntent\b\s+\w+\s*\=\s*new\s+Intent\s*\(\s*\)") Then

            ' Sey boolean to show that an intent exists
            ctCodeTracker.HasIntent = True

            ' Store the name of the intent
            strFragments = Regex.Split(CodeLine, "\=\s*new\s+Intent\s*\(\s*\)")
            strIntent = GetLastItem(strFragments.First())
            If strIntent <> "" And Not ctCodeTracker.AndroidIntents.Contains(strIntent) Then ctCodeTracker.AndroidIntents.Add(strIntent)

        ElseIf ctCodeTracker.HasIntent = True And Regex.IsMatch(CodeLine, "\.setClass\(") Then

            ' Remove any explicit intents from the dictionary
            strFragments = Regex.Split(CodeLine, "\.setClass\(")

            If strFragments.Count > 1 Then
                strIntent = GetFirstItem(strFragments.ElementAt(1), ")")

                If strIntent <> "" And ctCodeTracker.AndroidIntents.Contains(strIntent) Then ctCodeTracker.AndroidIntents.Remove(strIntent)
                If ctCodeTracker.AndroidIntents.Count = 0 Then ctCodeTracker.HasIntent = False
            End If

        ElseIf ctCodeTracker.HasIntent = True And Regex.IsMatch(CodeLine, "\bstartActivity\b\s*\(") Then

            ' Remove any explicit intents from the dictionary
            strFragments = Regex.Split(CodeLine, "\bstartActivity\b\s*\(")

            If strFragments.Count > 1 Then
                strIntent = GetFirstItem(strFragments.ElementAt(1), ")")
                If strIntent <> "" And ctCodeTracker.AndroidIntents.Contains(strIntent) Then
                    ctCodeTracker.AndroidIntents.Remove(strIntent)
                    If ctCodeTracker.AndroidIntents.Count = 0 Then ctCodeTracker.HasIntent = False
                    frmMain.ListCodeIssue("Implicit Intents in Use", "The application appears to be using implicit intents which could be intercepted by rogue applications. Intent name: " & strIntent, FileName, CodeIssue.MEDIUM, CodeLine)
                End If
            End If
        End If

    End Sub

End Module
