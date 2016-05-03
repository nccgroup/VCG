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

Imports System.Text.RegularExpressions

Public Class CodeTracker
    ' Stores details around the current code block in order to 
    ' facilitate the location of mismatched 'malloc'-'dealloc' and
    ' other issues which require checks from multiple lines 
    ' throughout the file.
    '===============================================================


    '==================================================
    ' Constants to identify usages of sizeof in strncpy
    '--------------------------------------------------
    Private Const MISCALC_SIZE_OF As Integer = -1
    Private Const OFF_BY_ONE_SIZE_OF As Integer = -2
    Private Const SOURCE_SIZE_OF As Integer = -3
    Private Const DEST_SIZE_OF As Integer = -4
    '==================================================


    '========================================================================
    ' Constants to identify types of buffer comparison for overflow detection
    '------------------------------------------------------------------------
    Private Const NO_DANGER As Integer = 0
    Private Const POINTER_INTO_ARRAY As Integer = 1
    Private Const CMDLINE_INTO_ARRAY As Integer = 2
    Private Const SOURCE_LARGER_THAN_DEST As Integer = 3
    Private Const WRONG_LIMIT_NO_DANGER As Integer = 4
    Private Const OFF_BY_ONE As Integer = 5
    Private Const SOURCE_LIMIT As Integer = 6
    Private Const STRNCAT_MISUSE As Integer = 7
    '========================================================================


    '========================================
    ' C++ Details
    ' Used for tracking details of the C code
    '----------------------------------------
    Public IsDestructor As Boolean = False
    Public IsLstat As Boolean = False

    Public DestructorBraces As Integer = 0
    Public TocTouLineCount As Integer = 0

    Public DestinationBuffer As String = ""                     ' Keep track of this to determnine correct error checking for realloc
    Public SourceBuffer As String = ""                          ' Keep track of this to determnine correct error checking for realloc

    Public UserVariables As New ArrayList                       ' List of user-controlled varaibles from commandline

    Private dicMemAssign As New Dictionary(Of String, String)   ' Dictionary of instances of new/malloc
    Private dicBuffer As New Dictionary(Of String, String)      ' Dictionary of fixed buffers
    Private dicInteger As New Dictionary(Of String, String)     ' Dictionary of integer assignments to help identify any buffer sizes
    Private dicUnsigned As New Dictionary(Of String, String)    ' Dictionary of any unsigned integers to help identify signed/unsigned comparisons
    '=======================================


    '===========================================
    ' Java details
    ' Used for tracking details of the Java code
    '-------------------------------------------
    Public IsServlet As Boolean = False
    Public IsRuntime As Boolean = False
    Public ImplementsClone As Boolean = False
    Public IsSerialize As Boolean = False
    Public IsDeserialize As Boolean = False
    Public HasValidator As Boolean = False
    Public HasVulnSQLString As Boolean = False
    Public HasHttpRequestData As Boolean = False
    Public IsInsideClass As Boolean = False
    Public HasGetVariables As Boolean = False
    Public IsSynchronized As Boolean = False
    Public IsInsideMethod As Boolean = False
    Public IsPrivileged As Boolean = False
    Public HasXXEEnabled As Boolean = False
    Public HasPrimitives As Boolean = False
    Public IsFileOpen As Boolean = False
    Public HasTry As Boolean = False
    Public HasResourceRelease As Boolean = True
    Public HasIntent As Boolean = False

    Public SerializeBraces As Integer = 0
    Public DeserializeBraces As Integer = 0
    Public ClassBraces As Integer = 0
    Public SyncBraces As Integer = 0
    Public MethodBraces As Integer = 0
    Public PrivBraces As Integer = 0
    Public SyncLineCount As Integer = 0
    Public SyncIndex As Integer = 0
    Public PrivLineCount As Integer = 0
    Public FileOpenLine As Integer = 0

    Public HttpRequestVar As String = ""
    Public ServletName As String = ""

    Public GetVariables As New ArrayList
    Public SQLStatements As New ArrayList
    Public PrivateInstanceVars As New ArrayList
    Public ServletNames As New ArrayList        ' The list of servlet class names
    Public SyncBlockObjects As New ArrayList    ' List of locked object names and any inner locked objects in nested synchronized blocks
    Public AndroidIntents As New ArrayList      ' Android intents, stored to determine whether used explicitly or implicitly

    Public ServletInstances As New Dictionary(Of String, String)    ' Maps each Servlet object onto its class name
    Private dicStatic As New Dictionary(Of String, String)  ' Instances of static variables to look for non-threadsafe operations
    '===========================================


    '==============================================
    ' PL/SQL details
    ' Used for tracking details of the PL/SQL code
    '----------------------------------------------
    Public IsOracleEncrypt As Boolean = False
    Public IsAutonomousProcedure As Boolean = False
    Public IsView As Boolean = False
    Public IsNewPackage As Boolean = False
    Public IsAuth As Boolean = False
    Public IsInsideSQLVarDec As Boolean = False
    Public IsInsidePlSqlExecuteStmt As Boolean = False
    Public IsInsideProcDec As Boolean = False

    Public CurrentVar As String = ""
    '==============================================


    '=========================================
    ' C# details
    ' Used for tracking details of the C# code
    '-----------------------------------------
    Public HasSeed As Boolean = False
    Public IsSamlFunction As Boolean = False
    Public IsSerializable As Boolean = False
    Public IsSerializableClass As Boolean = False
    Public IsUnsafe As Boolean = False

    Public UnsafeBraces As Integer = 0

    Public InputVars As New ArrayList
    Public CookieVals As New ArrayList
    Public AspLabels As New ArrayList
    '=========================================


    '=========================================
    ' PHP details
    ' Used for tracking details of the PHP code
    '-----------------------------------------
    Public IsRegisterGlobals As Boolean = False
    Public IsArrayMerge As Boolean = False
    Public GlobalArrayName As String = ""
    Public HasDisableFunctions As Boolean = False
    '=========================================

    '============================================
    ' COBOL details
    ' Used for tracking details of the COBOL code
    '--------------------------------------------
    Public ProgramId As String = ""
    '============================================

    '===================================================
    ' Used in C++, C# and Java scans to handle thread issues
    '---------------------------------------------------
    Public LockedObject As String = ""
    Public GlobalVars As New Dictionary(Of String, String)
    Public InstanceVars As New Dictionary(Of String, String)
    Public GetSetMethods As New Dictionary(Of String, String)
    '===================================================


    'RegEx object for parsing the more complex C expressions, etc.
    '=============================================================
    Dim reRegex As Regex


    Public Sub New()
        'Initialise variables
        '====================

        Reset()
        ResetCDictionaries()

    End Sub

    Public Sub Reset()
        ' Reset/empty all variables in preparation for scanning a code module
        '====================================================================

        ' Java details
        IsServlet = False
        ImplementsClone = False
        IsRuntime = False
        IsSerialize = False
        IsDeserialize = False
        HasValidator = False
        HasVulnSQLString = False
        HasHttpRequestData = False
        IsInsideClass = False
        HasGetVariables = False
        IsSynchronized = False
        IsInsideMethod = False
        IsPrivileged = False
        HasXXEEnabled = False
        HasPrimitives = False
        IsFileOpen = False
        HasTry = False
        HasResourceRelease = True
        HasIntent = False

        HttpRequestVar = ""
        ServletName = ""
        LockedObject = ""

        SerializeBraces = 0
        DeserializeBraces = 0
        ClassBraces = 0
        SyncBraces = 0
        MethodBraces = 0
        PrivBraces = 0
        SyncLineCount = 0
        SyncIndex = 0
        PrivLineCount = 0
        FileOpenLine = 0

        GetVariables.Clear()
        SQLStatements.Clear()
        dicStatic.Clear()
        PrivateInstanceVars.Clear()
        SyncBlockObjects.Clear()
        AndroidIntents.Clear()

        ' PL/SQL details
        IsOracleEncrypt = False
        IsAutonomousProcedure = False
        IsView = False
        IsNewPackage = False
        IsAuth = False
        IsInsideSQLVarDec = False
        IsInsidePlSqlExecuteStmt = False
        IsInsideProcDec = False

        CurrentVar = ""


        ' C++ Details
        IsDestructor = False
        IsLstat = False

        DestructorBraces = 0
        TocTouLineCount = 0

        DestinationBuffer = ""
        SourceBuffer = ""

        dicMemAssign.Clear()
        dicUnsigned.Clear()


        ' C# Details
        HasSeed = False
        IsSamlFunction = False
        IsSerializable = False
        IsSerializableClass = False
        IsUnsafe = False

        InputVars.Clear()
        CookieVals.Clear()
        AspLabels.Clear()


        ' PHP Details
        IsRegisterGlobals = False
        IsArrayMerge = False

        GlobalArrayName = ""


        ' COBOL details
        ProgramId = ""

    End Sub

    Public Sub ResetCDictionaries()
        ' Reset the C++ Dictionaries which have a project-wide scope, not file-wide
        '==========================================================================

        dicBuffer.Clear()
        dicInteger.Clear()
        UserVariables.Clear()

        ' Used for tracking thread issues in C++ and Java
        ServletNames.Clear()
        GlobalVars.Clear()
        GetSetMethods.Clear()

    End Sub

    Public Sub AddMalloc(ByVal CodeLine As String, ByVal FileName As String)
        ' Take the variable name and its details and add them to the list
        '================================================================
        Dim strVarName As String = ""
        Dim strDescription As String = ""
        Dim strTemp As String = ""

        Dim arrStatements As String()
        Dim arrFragments As String()
        Dim arrVarNames As String()


        '== Split line into statements to account for any instances of multiple statements on same line ==
        arrStatements = CodeLine.Trim().Split(";")
        For Each strStatement In arrStatements

            ' The split means that we will get a final empty statement
            If strStatement = "" Then Exit For

            '== Identify occurences of malloc ==
            If strStatement.Contains("=") And strStatement.Contains("malloc") Then
                arrFragments = strStatement.Trim.Split("=")

                '== Use of malloc in C++ is discouraged in favour of delete ==
                If arrFragments(1).Contains("malloc") And FileName.EndsWith(".cpp") Then
                    strDescription = "malloc without free. The use of malloc() and free() functions in C++ code is not recommended and can result in errors that would otherwise have been avoided with new and delete.|Line: " & rtResultsTracker.LineCount & " FileName: " & FileName
                ElseIf arrFragments(1).Contains("malloc") Then
                    strDescription = "malloc without free.|Line: " & rtResultsTracker.LineCount & " FileName: " & FileName
                End If

                If strDescription <> "" Then

                    '== If we have a result add it to the dictionary ==
                    arrVarNames = arrFragments.First.Trim.Split()
                    strVarName = arrVarNames.Last.Trim()
                    strVarName = strVarName.Replace("*", "").Trim()

                    ' This varname may be in the dictionary already (e.g. different functions using same local variable names)
                    If strVarName <> "" Then
                        If dicMemAssign.ContainsKey(strVarName) Then
                            dicMemAssign.Item(strVarName) = strDescription
                        Else
                            dicMemAssign.Add(strVarName, strDescription)
                        End If
                    End If

                End If

            End If
        Next

    End Sub

    Public Sub AddFree(ByVal CodeLine As String, ByVal FileName As String)
        ' Take the variable name and its details and delete them from the list
        '=====================================================================
        Dim strVarName As String = ""
        Dim strDescription As String = ""
        Dim strNewDescription As String = ""

        Dim arrStatements As String()
        Dim arrFragments As String()
        Dim arrReportFragments As String()

        Dim intNumFrees As Integer = 0


        '== Split line into statements to account for any instances of multiple statements on same line ==
        arrStatements = CodeLine.Trim().Split(";")
        For Each strStatement In arrStatements

            ' The split means that we will get a final empty statement
            If strStatement = "" Then Exit For

            '== Check statement contains a 'free' and extract varname from brackets ==
            If InStr(strStatement, "free") < InStr(strStatement, "(") < InStr(strStatement, ")") Then
                arrFragments = strStatement.Split("(")
                strVarName = arrFragments.Last.Trim()
                arrFragments = strVarName.Split(")")
                strVarName = arrFragments.First
                strVarName = strVarName.Replace("*", "").Trim()
            End If


            '== If we've managed to extract a variable name then assign a description ==
            If strVarName <> "" Then

                If dicMemAssign.ContainsKey(strVarName) Then strDescription = dicMemAssign.Item(strVarName)

                If strDescription.Trim() = "" Then
                    ' Variable not in dictionary - list the error
                    strNewDescription = "1 free |Potential memory leak/system crash - free without malloc."
                ElseIf (strDescription.Contains("new ") And strStatement.Contains(" without delete")) Then
                    ' Mismatched new and free
                    strNewDescription = "1 free |Potential memory leak or heap corruption - inappropriate use of new and free. Mixing of new and free operators can result in a failure to de-allocate memory as this behavior is compiler dependent and technically undefined."
                ElseIf strDescription.Contains(" free ") And Not strDescription.Contains("without free ") Then
                    ' More than one free for this variable (it has already been processed and had its description changed)
                    arrReportFragments = strDescription.Split("|") ' Break the description up - it has the format: "n delete| [description |] Line: n Filename: filename"
                    arrFragments = arrReportFragments(0).Split()
                    intNumFrees = CInt(arrFragments(0).Trim())
                    intNumFrees += 1
                    strNewDescription = CStr(intNumFrees) & " free |Multiple frees detected. Check code paths manually to ensure that variables cannot be freed more than once."

                    ' Add any extra description for previously identified error
                    If arrReportFragments.Length > 1 Then strDescription = strDescription & "|" & arrReportFragments(1)
                Else
                    strNewDescription = "1 free "
                End If
            End If

            '== Locate variable in dictionary and modify its description ==
            If strNewDescription <> "" And strVarName <> "" And FileName.EndsWith(".cpp") Then
                dicMemAssign.Item(strVarName) = strNewDescription & "|The use of malloc() and free() functions in C++ code is not recommended and can result in errors that would otherwise have been avoided with new and delete.|Line: " & rtResultsTracker.LineCount & " FileName: " & FileName
            ElseIf strNewDescription <> "" And strVarName <> "" Then
                dicMemAssign.Item(strVarName) = strNewDescription & "|Line: " & rtResultsTracker.LineCount & " FileName: " & FileName
            End If

        Next

    End Sub

    Public Sub AddNew(ByVal CodeLine As String, ByVal FileName As String)
        ' Take the variable name and its details and add them to the list
        '================================================================
        Dim strVarName As String = ""
        Dim strDescription As String = ""

        Dim arrStatements As String()
        Dim arrFragments As String()
        Dim arrVarNames As String()


        '== Split line into statements to account for any instances of multiple statements on same line ==
        arrStatements = CodeLine.Trim().Split(";")
        For Each strStatement In arrStatements

            ' The split means that we will get a final empty statement
            If strStatement = "" Then Exit For

            If strStatement.Contains("=") And strStatement.Contains("new") Then
                arrFragments = strStatement.Trim.Split("=")
                If arrFragments(1).Contains("new") And arrFragments(1).Contains("[") Then
                    strDescription = "new [] without delete.|Line: " & rtResultsTracker.LineCount & " FileName: " & FileName
                ElseIf arrFragments(1).Contains("new") Then
                    strDescription = "new without delete.|Line: " & rtResultsTracker.LineCount & " FileName: " & FileName
                End If

                If strDescription <> "" Then
                    '== If we have a result add it to the dictionary ==
                    arrVarNames = arrFragments.First.Trim.Split()
                    strVarName = arrVarNames.Last.Trim()
                    strVarName = strVarName.Replace("*", "").Trim()

                    ' This varname may be in the dictionary already (e.g. different functions using same local variable names)
                    If strVarName <> "" Then
                        If dicMemAssign.ContainsKey(strVarName) Then
                            dicMemAssign.Item(strVarName) = strDescription
                        Else
                            dicMemAssign.Add(strVarName, strDescription)
                        End If
                    End If

                End If
            End If
        Next

    End Sub

    Public Sub AddDelete(ByVal CodeLine As String, ByVal FileName As String)
        ' Take the variable name and its details and modify the dictionary accordingly
        '=============================================================================
        Dim strVarName As String = ""
        Dim strDescription As String = ""
        Dim strNewDescription As String = ""

        Dim intNumDeletes As Integer

        Dim arrStatements As String()
        Dim arrFragments As String()
        Dim arrReportFragments As String()


        '== Split line into statements to account for any instances of multiple statements on same line ==
        arrStatements = CodeLine.Trim().Split(";")
        For Each strStatement In arrStatements

            ' The split means that we will get a final empty statement
            If strStatement = "" Then Exit For

            '== Get type of delete (vector or normal) and then extract variable name ==
            If strStatement.Contains("delete") And strStatement.Contains("[") Then
                arrFragments = strStatement.Split("[")
                strVarName = arrFragments.Last.Trim()
                strVarName = strVarName.Replace("*", "").Trim()
            ElseIf strStatement.Contains("delete ") Then
                arrFragments = strStatement.Split(" ")
                strVarName = arrFragments.Last.Trim()
                strVarName = strVarName.Replace("*", "").Trim()
            End If


            '== If we've managed to extract a variable name then assign a description ==
            If strVarName <> "" Then

                '== Check if the item is in the dictionary ==
                If dicMemAssign.ContainsKey(strVarName) Then strDescription = dicMemAssign.Item(strVarName)

                If strDescription.Trim() = "" Then
                    ' Variable not in dictionary - list the error
                    strNewDescription = "1 delete |Potential memory leak/system crash - delete without new."
                ElseIf (strDescription.Contains("new [") And Not strStatement.Contains("[")) Or (Not strDescription.Contains("new [") And strStatement.Contains("[")) Then
                    ' Mismatched new and delete, one vector, the other scalar
                    strNewDescription = "1 delete |Potential memory leak or heap corruption - mismatched new and delete. Mixing of scalar and vector operators can result in too much or too little memory being deallocated."
                ElseIf strDescription.Contains("malloc") And Not strDescription.StartsWith("1 delete ") Then
                    ' Mismatched new and delete, one vector, the other scalar
                    strNewDescription = "1 delete |Potential memory leak or heap corruption - malloc should not be used in conjunction with delete. This behavior is compiler dependent and technically undefined, with no guarantee that delete will internally use free."
                ElseIf strDescription.Contains(" delete ") And Not strDescription.Contains("without delete ") Then
                    ' More than one delete for this variable (it has already been processed and had its description changed
                    arrReportFragments = strDescription.Split("|") ' Break the description up - it has the format: "n delete| [description |] Line: n Filename: filename"
                    arrFragments = arrReportFragments(0).Split()
                    intNumDeletes = CInt(arrFragments(0).Trim())
                    intNumDeletes += 1
                    strNewDescription = CStr(intNumDeletes) & " delete |Multiple deletes detected. Check code paths manually to ensure that variables cannot be deleted more than once."

                    ' Add any extra description for previously identified error
                    If arrReportFragments.Length > 1 Then strDescription = strDescription & "|" & arrReportFragments(1)
                Else
                    strNewDescription = "1 delete "
                End If
            End If

            '== Locate variable in dictionary and modify its description ==
            If strNewDescription <> "" And strVarName <> "" Then
                dicMemAssign.Item(strVarName) = strNewDescription & "|Line: " & rtResultsTracker.LineCount & " FileName: " & FileName
            End If

        Next

    End Sub

    Public Sub AddBuffer(ByVal CodeLine As String, Optional ByVal BuffType As String = "(char|TCHAR|BYTE)")
        ' Take the variable name and its details and add them to the list
        '================================================================
        Dim strVarName As String = ""
        Dim strDescription As String = ""

        Dim blnIsMultDecs = False

        Dim arrStatements As String()
        Dim arrParams As String()
        Dim arrFragments As String()
        Dim arrVarNames As String()
        Dim arrAllocations As String()


        '== Split line into statements to account for any instances of multiple statements on same line ==
        arrStatements = CodeLine.Trim().Split(";")
        For Each strStatement In arrStatements

            ' The split means that we will get a final empty statement
            If strStatement.Trim() = "" Then Exit For

            ' Do we have multiple char decalarations on the same line
            If Regex.IsMatch(strStatement, "\b" & BuffType & "\b\s+\w+\s*\[\s*[a-z,A-Z,0-9,_]\s*\]\s*\,\s*(\w+\s*\[|\*\s*\w+)") Or _
                Regex.IsMatch(strStatement, "\b" & BuffType & "\b\s*\*\s*\w+\s*\,\s*(\w+\s*\[|\*\s*\w+)") Then
                blnIsMultDecs = True
            Else
                blnIsMultDecs = False
            End If

            ' Split on commas to take account of paramaters within variable declarations
            ' or multiple declarations on same line
            arrParams = strStatement.Trim.Split(",")
            For Each strParameter In arrParams

                strVarName = ""
                strDescription = "*"

                If strParameter.Trim() <> "" Then

                    '== Check statement contains a char array and extract length from brackets ==
                    If (Not strParameter.Contains("*") And (InStr(strParameter, "[") < InStr(strParameter, "]"))) Then
                        arrFragments = strParameter.Split("[")

                        strDescription = arrFragments.Last.Trim()
                        arrAllocations = strDescription.Split("]")
                        strDescription = arrAllocations.First.Trim()

                        '== Split 'char' from varname if required ==
                        strVarName = arrFragments.First.Trim()
                        arrVarNames = strVarName.Split()
                        If Regex.IsMatch(strVarName, "\b" & BuffType & "\b") Then
                            strVarName = arrVarNames.Last.Trim()
                        Else
                            strVarName = arrVarNames.First.Trim()
                        End If

                    End If

                    '== This varname may be in the dictionary already (e.g. different functions using same local variable names) ==
                    If strVarName <> "" And strDescription <> "" Then
                        If dicBuffer.ContainsKey(strVarName) Then
                            dicBuffer.Item(strVarName) = strDescription
                        Else
                            dicBuffer.Add(strVarName, strDescription)
                        End If
                    End If
                End If
            Next
        Next

    End Sub

    Public Sub AddCharStar(ByVal CodeLine As String, Optional ByVal BuffType As String = "(char|TCHAR|BYTE)")
        ' Take the variable name and its details and add them to the list
        '================================================================
        Dim strVarName As String = ""
        Dim strDescription As String = "*"
        Dim strTemp As String = ""

        Dim arrStatements As String()
        Dim arrParams As String()
        Dim arrFragments As String()


        '== Split line into statements to account for any instances of multiple statements on same line ==
        arrStatements = CodeLine.Trim().Split(";")
        For Each strStatement In arrStatements

            ' The split means that we will get a final empty statement
            If strStatement.Trim() = "" Then Exit For

            If Regex.IsMatch(strStatement, "\b" & BuffType & "\b\s+\w+\s*\[\s*[a-z,A-Z,0-9,_]\s*\]\s*\,\s*(\w+\s*\[|\*\s*\w+)") Or _
            Regex.IsMatch(strStatement, "\b" & BuffType & "\b\s*\*\s*\w+\s*\,\s*(\w+\s*\[|\*\s*\w+)") Then

                '== Remove 'char' from start ==
                arrFragments = Regex.Split(strStatement, "\b" & BuffType & "\b\s*")
                strVarName = arrFragments.Last.Trim()
                '== Extract varnames from any potential assignment statement ==
                arrFragments = strVarName.Split("=")
                strVarName = arrFragments.First.Trim()
                ' If this is a list of parameters in function declaration we need to split on commas
                arrParams = strStatement.Trim.Split(",")
                For Each strParameter In arrParams

                    If Regex.IsMatch(strParameter, "\*\s*\w+") And Not strParameter.Contains("[") Then
                        arrFragments = strParameter.Split("*")
                        strVarName = arrFragments.Last().Trim()

                        '== Trim any extraneous braces ==
                        If Regex.IsMatch(strVarName, "\w+\s*\)") Then
                            arrFragments = strVarName.Split(")")
                            strVarName = arrFragments.First.Trim()
                        End If

                        If dicBuffer.ContainsKey(strVarName) Then
                            dicBuffer.Item(strVarName) = strDescription
                        Else
                            dicBuffer.Add(strVarName, strDescription)
                        End If
                    End If

                    strVarName = ""
                Next
            ElseIf Regex.IsMatch(strStatement, "\b" & BuffType & "\b\s*\*\s*\w+") Then
                arrParams = strStatement.Trim.Split(",")
                For Each strParameter In arrParams

                    If Not strParameter.Contains("[") Then

                        '== Extract varnames from any potential assignment statement ==
                        arrFragments = strVarName.Split("=")
                        strVarName = arrFragments.First.Trim()
                        '== Remove 'char *' from start ==
                        arrFragments = strParameter.Split("*")
                        strVarName = arrFragments.Last.Trim()

                        '== Trim any extraneous braces ==
                        If Regex.IsMatch(strVarName, "\w+\s*\)") Then
                            arrFragments = strVarName.Split(")")
                            strVarName = arrFragments.First.Trim()
                        End If

                        If strParameter.Trim() <> "" Then
                            If dicBuffer.ContainsKey(strVarName) Then
                                dicBuffer.Item(strVarName) = strDescription
                            Else
                                dicBuffer.Add(strVarName, strDescription)
                            End If
                        End If

                        strVarName = ""
                    End If
                Next
            End If
        Next

    End Sub

    Public Sub AddPointer(ByVal CodeLine As String, Optional ByVal BuffType As String = "POINTER")
        ' Take the variable name and its details and add them to the list
        ' N.B. - This function deals with COBOL pointers
        '================================================================

    End Sub

    Public Sub AddInteger(ByVal CodeLine As String)
        ' Take the variable name and its details and add them to the list
        '================================================================
        Dim strDescription As String = ""
        Dim strTemp As String = ""  ' used as placeholder

        Dim arrStatements As String()
        Dim arrFragments As String()
        Dim arrPlaceHolders As String()
        Dim arlVarNames As New ArrayList()

        '== Split line into statements to account for any instances of multiple statements on same line ==
        arrStatements = CodeLine.Trim().Split(";")

        For Each strStatement In arrStatements
            '== If it's a pointer or an array then don't bother ==
            If strStatement.Contains("=") And (Not strStatement.Contains("[")) And (Not strStatement.Contains("*")) Then

                arrFragments = strStatement.Trim().Split("=")
                strDescription = arrFragments.Last.Trim()

                '== Split the statement on comma if multiple variables are defined ==
                If arrFragments.First.Contains(",") Then
                    arrPlaceHolders = arrFragments.First.Split(",")
                    For Each strVarName In arrPlaceHolders
                        If strVarName.Contains(" ") Then strVarName = GetLastItem(strVarName) ' This will deal with the first item which will be "int varname"

                        '== Be careful of anything which may break the regex ==
                        strVarName = strVarName.TrimStart("(").Trim()
                        strVarName = strVarName.TrimEnd(")").Trim()

                        arlVarNames.Add(strVarName)
                    Next
                Else
                    ' Otherwise split on spaces and take last item as varname 
                    strTemp = GetLastItem(arrFragments.First)

                    '== Be careful of anything which may break the regex ==
                    strTemp = strTemp.TrimStart("(").Trim()
                    strTemp = strTemp.TrimEnd(")").Trim()

                    arlVarNames.Add(strTemp)
                End If

                '== The varnames may be in the dictionary already (e.g. different functions using same local variable names) ==
                For Each strVarName In arlVarNames
                    If strVarName <> "" Then

                        If dicInteger.ContainsKey(strVarName) Then
                            dicInteger.Item(strVarName) = strDescription
                        Else
                            dicInteger.Add(strVarName, strDescription)
                        End If
                        '== If this is a new signed integer remove any with a duplicate name from the unsigned dictionary ==
                        If ((Not strStatement.Contains("unsigned ")) And (dicUnsigned.ContainsKey(strVarName))) Then dicUnsigned.Remove(strVarName)
                    End If
                Next

            End If
        Next

    End Sub

    Public Sub AddUnsigned(ByVal CodeLine As String)
        ' Take the variable name and its details and add them to the list
        '================================================================
        Dim strDescription As String = ""
        Dim strVarType As String = "unsigned"
        Dim strTemp As String = ""  ' used as placeholder

        Dim arrStatements As String()
        Dim arrFragments As String()
        Dim arrPlaceHolders As String()
        Dim arlVarNames As New ArrayList()

        '== Split line into statements to account for any instances of multiple statements on same line ==
        arrStatements = CodeLine.Trim().Split(";")



        '== Extract variable names ==
        For Each strStatement In arrStatements

            '== RegEx should trap function input parameters, declarations of multiple unsigned vars on same line, etc. ==
            If Regex.IsMatch(strStatement, "\b(unsigned|UNSIGNED|size_t|uint16|uint32|UINT)\b\s+\w+\s*(\,|$|=|\))") _
                Or Regex.IsMatch(strStatement, "\b(unsigned|UNSIGNED)\b\s+\b(short|int|long|INT|LONG)\b\s+\w+\s*(\,|$|=|\))") _
                Or Regex.IsMatch(strStatement, "\b(short|long|LONG|unsigned|UNSIGNED)\b\s+\b(short|long|LONG|unsigned|UNSIGNED)\b\s+\b(int|INT)\b\s+\w+\s*(\,|$|=|\))") _
                Or Regex.IsMatch(strStatement, "\b(unsigned|UNSIGNED)\b\s+\b(short|int|long|INT|LONG)\b\s+\w+(\,|$|=|\))") _
                Or Regex.IsMatch(strStatement, "\b(short|int|long|INT|LONG)\b\s+\b(unsigned|UNSIGNED)\b\s+\w+(\,|$|=|\))") Then

                '== Strip off anything which follows the equals sign (if present) as we won't need it ==
                arrFragments = strStatement.Trim().Split("=")
                strDescription = arrFragments.First.Trim()

                '== Multiple declarations, comma separated ==


                '== Obtain each comma-separated statement ==
                arrPlaceHolders = strDescription.Split(",")
                For Each strVarName In arrPlaceHolders
                    If Regex.IsMatch(strVarName, "\b(unsigned|UNSIGNED|size_t|uint16|uint32|UINT)\b") Then
                        strVarName = GetLastItem(strVarName)
                    End If

                    '== Be careful of anything which may break the regex ==
                    strVarName = strVarName.TrimStart("(").Trim()
                    strVarName = strVarName.TrimEnd(")").Trim()
                    arlVarNames.Add(strVarName)
                Next

            End If

            '== The varnames may be in the dictionary already (e.g. different functions using same local variable names) ==
            For Each strVarName In arlVarNames
                If strVarName <> "" Then
                    If dicUnsigned.ContainsKey(strVarName) Then
                        dicUnsigned.Item(strVarName) = strVarType
                    Else
                        dicUnsigned.Add(strVarName, strVarType)
                    End If
                End If
            Next

        Next

    End Sub

    Public Sub CheckOverflow(ByVal CodeLine As String, ByVal FileName As String)
        ' Check to see if the strcpy is copying within limits of buffer sizes
        '====================================================================
        Dim strSource As String = ""
        Dim strDestination As String = ""
        Dim strExpression As String = ""
        Dim strParameter As String

        Dim arrStatements As String()
        Dim arrFragments As String()

        Dim intLimit As Integer = 0
        Dim intOverflowType As Integer = NO_DANGER

        Dim blnIsStrN As Boolean = False
        Dim blnIsCat As Boolean = False


        '== Split line into statements to account for any instances of multiple statements on same line ==
        arrStatements = CodeLine.Trim().Split(";")
        For Each strStatement In arrStatements

            On Error GoTo NextLoop

            blnIsCat = False
            blnIsStrN = False
            intOverflowType = NO_DANGER

            ' Remove all spaces to simplify parsing
            'strStatement = strStatement.Replace(" ", "")

            If strStatement = "" Then GoTo NextLoop

            ' Determine type of expression
            If strStatement.Contains("strcpy") Then
                strExpression = "strcpy"
            ElseIf strStatement.Contains("strlcpy") Then
                strExpression = "strlcpy"
            ElseIf strStatement.Contains("strcat") Then
                blnIsCat = True ' Set this boolean var to avoid repeated calls to .contains()
                strExpression = "strcat"
            ElseIf strStatement.Contains("strlcat") Then
                blnIsCat = True ' Set this boolean var to avoid repeated calls to .contains()
                strExpression = "strlcat"
            ElseIf strStatement.Contains("strncpy") Then
                blnIsStrN = True ' Set this boolean var to avoid repeated calls to .contains()
                strExpression = "strncpy"
            ElseIf strStatement.Contains("strncat") Then
                blnIsCat = True ' Set this boolean var to avoid repeated calls to .contains()
                blnIsStrN = True ' Set this boolean var to avoid repeated calls to .contains()
                strExpression = "strncat"
            ElseIf strStatement.Contains("sprintf") Then
                strExpression = "sprintf"
            ElseIf strStatement.Contains("memcpy") Then
                blnIsStrN = True ' Set this boolean var as memcpy has char limit
                strExpression = "memcpy"
            ElseIf strStatement.Contains("memmove") Then
                blnIsStrN = True ' Set this boolean var as memcpy has char limit
                strExpression = "memmove"
            Else
                GoTo NextLoop
            End If

            arrFragments = Regex.Split(strStatement, "\b" & strExpression & "\b")


            '== Check statement contains a workable expression and extract varnames from brackets ==
            If (InStr(strStatement, strExpression) < InStr(strStatement, "(") < InStr(strStatement, ")")) Then

                strParameter = arrFragments.Last

                ' Locate size/type of source and destination buffers
                arrFragments = strParameter.Split(",")
                ' Remove spaces from fragments and remove any leading/trailing braces
                strDestination = arrFragments(0).Trim.TrimStart("(").Trim

                ' Annoyingly sprintf has parameters in a different order from the others...
                If strExpression = "sprintf" Then
                    strSource = arrFragments.Last.Trim.TrimEnd(")").Trim
                Else
                    strSource = arrFragments(1).Trim.TrimEnd(")").Trim
                    If blnIsStrN Then intLimit = GetStrncpyLimit(arrFragments(2).Trim.TrimEnd(")"), strSource, strDestination, (blnIsCat And blnIsStrN))
                End If


                '== Compare buffer sizes and types to determine if there is any possibility of overflow ==
                '== In the case of strncpy check for number or variable as length argument and check for any sizeof() buffoonery ==
                intOverflowType = CompareBufferLengths(strSource, strDestination, blnIsStrN, intLimit, blnIsCat)

            End If


            If blnIsStrN = False And intOverflowType <> NO_DANGER Then

                Select Case intOverflowType
                    Case CMDLINE_INTO_ARRAY
                        ' Is strcpy copying a commandline argument to char[]?
                        frmMain.ListCodeIssue("Unsafe Use of " & strExpression & " Allows Buffer Overflow", "A user-supplied string from the commandline is being copied to a fixed length destination buffer and could allow a buffer overflow to take place.", FileName, CodeIssue.CRITICAL, CodeLine)
                    Case POINTER_INTO_ARRAY
                        ' Is strcpy copying a char* to char[]?
                        frmMain.ListCodeIssue("Unsafe Use of " & strExpression & " Allows Buffer Overflow", "A char* is being copied to a fixed length destination buffer and could allow a buffer overflow to take place.", FileName, CodeIssue.CRITICAL, CodeLine)
                    Case SOURCE_LARGER_THAN_DEST
                        ' Mismatched array sizes
                        frmMain.ListCodeIssue("Unsafe Use of " & strExpression & " Allows Buffer Overflow", "The source buffer is larger than the destination buffer and could allow a buffer overflow to take place.", FileName, CodeIssue.CRITICAL, CodeLine)
                End Select

            ElseIf blnIsStrN = True And intOverflowType <> NO_DANGER Then

                Select Case intOverflowType
                    Case CMDLINE_INTO_ARRAY
                        ' Is strcpy copying a commandline argument to char[]?
                        frmMain.ListCodeIssue("Unsafe Use of " & strExpression & " Allows Buffer Overflow", "The size limit is larger than the destination buffer, while the source is a user-supplied string from the commandline, and so could allow a buffer overflow to take place.", FileName, CodeIssue.CRITICAL, CodeLine)
                    Case POINTER_INTO_ARRAY
                        ' Is strcpy copying a char* to char[]?
                        frmMain.ListCodeIssue("Unsafe Use of " & strExpression & " Allows Buffer Overflow", "The size limit is larger than the destination buffer, while the source is a char* and so, could allow a buffer overflow to take place.", FileName, CodeIssue.CRITICAL, CodeLine)
                    Case SOURCE_LARGER_THAN_DEST
                        ' Mismatched array sizes
                        frmMain.ListCodeIssue("Unsafe Use of " & strExpression & " Allows Buffer Overflow", "The source buffer and size limit are BOTH larger than the destination buffer and could allow a buffer overflow to take place.", FileName, CodeIssue.CRITICAL, CodeLine)
                    Case WRONG_LIMIT_NO_DANGER
                        ' No immediate danger but wrong size limit in place
                        frmMain.ListCodeIssue("Unsafe Use of " & strExpression & ".", "Although the source buffer is not large enough to deliver a buffer overflow to the destination buffer, the size limit used by strncpy is larger then the destination and would theoretically such an attack if the source buffer were modified in the future.", FileName, CodeIssue.STANDARD, CodeLine)
                    Case OFF_BY_ONE
                        ' Incorrect use of sizeof()
                        frmMain.ListCodeIssue("Off-by-One Error - Unsafe Use of " & strExpression & " Allows Buffer Overflow", "The size limit is one byte larger than the destination buffer due to incorrect use of sizeof( ) and so could allow a buffer overflow to take place.", FileName, CodeIssue.HIGH, CodeLine)
                    Case SOURCE_LIMIT
                        ' Incorrect use of sizeof()
                        frmMain.ListCodeIssue("Programmer Error - Unsafe Use of " & strExpression & " Allows Buffer Overflow", "The size limit is set to the size of the source buffer, rather than the destination buffer due to mistaken use of sizeof( ) and so could allow a buffer overflow to take place.", FileName, CodeIssue.CRITICAL, CodeLine)
                    Case STRNCAT_MISUSE
                        ' Incorrect use of strncat (uses size of source or destination as limit)
                        frmMain.ListCodeIssue("Programmer Error - Unsafe Use of " & strExpression & " Allows Buffer Overflow", "The size limit is set to the size of the source or destination buffer. For safe usage the limit should be set to the allowed size of the destination buffer minus the current size of the destination buffer.", FileName, CodeIssue.CRITICAL, CodeLine)
                End Select

            End If
NextLoop:
            strSource = ""
            strDestination = ""
            strExpression = ""
            intLimit = 0
            blnIsStrN = False
        Next

    End Sub

    Public Function TrackBraces(ByVal CodeLine As String, ByRef BraceCount As Integer) As Boolean
        ' Track matching open/close braces to determine whether still inside function, loop, etc.
        '========================================================================================
        Dim blnRetVal As Boolean = True

        ' We need this to cover the special case of blank lines before the braces/routine 
        ' causing the function to return 'false' before any of the routine has been scanned
        If CodeLine.Contains("{") Or CodeLine.Contains("}") Then

            ' It's necessary to count every single brace in a line
            For Each chrChar As Char In CodeLine
                If chrChar = "{" Then BraceCount += 1
            Next
            For Each chrChar As Char In CodeLine
                If chrChar = "}" Then BraceCount -= 1
            Next

            If BraceCount < 1 Then blnRetVal = False

        End If

        Return blnRetVal

    End Function

    Public Function GetMemAssign() As Dictionary(Of String, String)
        'Return dictionary of any potentially problematic memory assignments
        '===================================================================
        Dim dicReturnDic As New Dictionary(Of String, String)

        ' Build new dictionary, ignoring all instances of variables that are correctly allocated and deleted
        For Each kyKey In dicMemAssign.Keys
            If Not ((dicMemAssign(kyKey).StartsWith("1 delete ") Or dicMemAssign(kyKey).StartsWith("1 free "))) Then dicReturnDic.Add(kyKey, dicMemAssign.Item(kyKey))
        Next

        ' Return the remaining list of faulty allocations
        Return dicReturnDic

    End Function

    Private Function GetStrncpyLimit(ByVal BufferSize As String, ByVal SourceName As String, ByVal DestName As String, ByVal IsStrncat As Boolean) As Integer
        ' Get content of square braces and check if numeric
        ' If non-numeric check for sizeof() issues
        ' and try and get value for variable used for buffer size
        '========================================================
        Dim intRetVal As Integer = 0
        Dim strTemp As String


        If BufferSize = "" Then
            ' This shouldn't happen, but you never know...
            intRetVal = 0
        ElseIf IsNumeric(BufferSize) Then
            ' Simplest case - hardcoded numeric buffer
            intRetVal = CInt(BufferSize)
        ElseIf BufferSize.Contains("sizeof") Then
            ' Check that sizeof has been correctly used
            If SourceName <> "" And BufferSize.Contains(SourceName) Then
                intRetVal = SOURCE_SIZE_OF
            ElseIf DestName <> "" And BufferSize.Contains(DestName) And IsStrncat Then
                intRetVal = DEST_SIZE_OF
            ElseIf DestName <> "" And Regex.IsMatch(BufferSize, "\bsizeof\b\s*\(\s*\b" & DestName & "\b\s*\-") Then
                intRetVal = MISCALC_SIZE_OF
            ElseIf DestName <> "" And Regex.IsMatch(BufferSize, "\bsizeof\b\s*\(\s*\b" & DestName & "\b") Then
                intRetVal = OFF_BY_ONE_SIZE_OF
            End If
        Else
            ' Check against dictionary of variable names
            strTemp = dicInteger.Item(BufferSize)
            If IsNumeric(strTemp) Then intRetVal = CInt(strTemp)
        End If

        Return intRetVal

    End Function

    Private Function CompareBufferLengths(ByVal SourceBuffer As String, ByVal DestinationBuffer As String, Optional ByVal IsStrN As Boolean = False, Optional ByVal SizeLimit As Integer = 0, Optional ByVal IsCat As Boolean = False) As Integer
        ' Take two buffer names and return true if source is larger than destination
        '===========================================================================
        Dim intRetVal As Integer = NO_DANGER


        If SourceBuffer = "" Or DestinationBuffer = "" Then
            ' Parsing has failed to identify buffers
            intRetVal = NO_DANGER
        ElseIf dicBuffer.Item(DestinationBuffer) = "*" Then
            ' Copying to a destination with no fixed limit
            intRetVal = NO_DANGER
        ElseIf Regex.IsMatch(SourceBuffer, "\bargv\b") Then
            ' Copying an unlimited length string from cmdline into fixed length buffer
            intRetVal = CMDLINE_INTO_ARRAY
        ElseIf dicBuffer.Item(SourceBuffer) = "*" Then
            ' Copying an unlimited length string into fixed length buffer
            intRetVal = POINTER_INTO_ARRAY
        ElseIf IsStrN = False And (GetBufferLength(SourceBuffer) > GetBufferLength(DestinationBuffer)) Then
            ' Classic overflow - source buffer larger than destination buffer
            intRetVal = SOURCE_LARGER_THAN_DEST
        ElseIf IsStrN = False And IsCat = True And ((GetBufferLength(SourceBuffer) + GetBufferLength(DestinationBuffer)) > GetBufferLength(DestinationBuffer)) Then
            ' Overflow from unsafe use of strcat
            intRetVal = SOURCE_LARGER_THAN_DEST
        ElseIf IsStrN = True And (GetBufferLength(SourceBuffer) > GetBufferLength(DestinationBuffer)) And (SizeLimit >= DestinationBuffer) Then
            ' Overflow from unsafe use of strncpy/strncat
            intRetVal = SOURCE_LARGER_THAN_DEST
        ElseIf IsStrN = True And (GetBufferLength(SourceBuffer) <= GetBufferLength(DestinationBuffer)) And (SizeLimit > DestinationBuffer) Then
            ' Overflow from unsafe use of strncpy/strncat
            intRetVal = WRONG_LIMIT_NO_DANGER
        ElseIf IsStrN = True And (GetBufferLength(SourceBuffer) > GetBufferLength(DestinationBuffer)) And SizeLimit = OFF_BY_ONE_SIZE_OF Then
            ' Overflow from unsafe use of strncpy/strncat
            intRetVal = OFF_BY_ONE
        ElseIf IsStrN = True And (GetBufferLength(SourceBuffer) > GetBufferLength(DestinationBuffer)) And SizeLimit = SOURCE_SIZE_OF Then
            ' Overflow from unsafe use of strncpy
            intRetVal = SOURCE_LIMIT
        ElseIf IsStrN = True And IsCat = True And ((GetBufferLength(SourceBuffer) + GetBufferLength(DestinationBuffer)) >= SizeLimit) Then
            ' Overflow from unsafe use of strncat
            intRetVal = SOURCE_LARGER_THAN_DEST
        ElseIf IsStrN = True And IsCat = True And (SizeLimit = DEST_SIZE_OF Or SizeLimit = SOURCE_SIZE_OF Or SizeLimit = MISCALC_SIZE_OF Or SizeLimit = OFF_BY_ONE_SIZE_OF) Then
            ' Use of strncat without safe limit
            intRetVal = STRNCAT_MISUSE
        End If

        Return intRetVal

    End Function

    Private Function GetBufferLength(ByVal BufferName As String) As Integer
        ' Take name of buffer and attempt to find its length
        '===================================================
        Dim intRetVal As Integer = 0
        Dim strLength As String = ""
        Dim strVarVal As String = ""


        strLength = dicBuffer.Item(BufferName)

        If strLength = "" Then
            intRetVal = 0
        ElseIf IsNumeric(strLength) Then
            intRetVal = CInt(strLength)
        Else
            ' If we have a non-numeric string we need to see if we have an 
            ' identifiable variable with a value
            intRetVal = GetInteger(strLength)
        End If

        Return intRetVal

    End Function

    Private Function GetInteger(ByVal VarName As String) As Integer
        'Get value of numeric variable - check recursively for variables that
        ' are assigned the value of another variable
        '====================================================================
        Dim intRetVal As Integer = 0
        Dim strResult As String = ""

        If Not dicInteger.ContainsKey(VarName) Then Return 0

        strResult = dicInteger.Item(VarName)

        If strResult <> "" Then
            If IsNumeric(strResult) Then
                intRetVal = CInt(strResult)
            Else
                intRetVal = GetInteger(strResult)
            End If
        End If

        Return intRetVal

    End Function

    Public Function GetIntegers() As Dictionary(Of String, String)
        'Get dictionary of numeric variables (used primarily to identify buffer sizes)
        '=============================================================================

        Return dicInteger

    End Function

    Public Function CheckSignedComp(ByVal CodeLine As String) As Integer
        ' Get status of numeric variables - signed or unsigned
        ' Return true if signed is compared with unsigned, otherwise return false
        '========================================================================

        Dim strLeftSide As String = ""
        Dim strRightSide As String = ""
        Dim strOperator As String = ""

        Dim blnIsSizeOfR As Boolean = False
        Dim blnIsSizeOfL As Boolean = False

        Dim arrStatements As String()
        Dim arrFragments As String()


        '== Split line into statements to account for any instances of multiple statements ==
        '== on same line or comparisons inside for loops ==
        arrStatements = CodeLine.Trim().Split(";")

        For Each strStatement In arrStatements

            '== If it's an empty string then don't bother ==
            If strStatement.Trim <> "" Then

                '== Get the comparison operator ==
                If strStatement.Contains("==") Then
                    strOperator = "=="
                ElseIf strStatement.Contains("!=") Then
                    strOperator = "!="
                ElseIf strStatement.Contains("<=") Then
                    strOperator = "<="
                ElseIf strStatement.Contains(">=") Then
                    strOperator = ">="
                ElseIf strStatement.Contains("<") And Not CodeLine.Contains("<<") Then
                    strOperator = "<"
                ElseIf strStatement.Contains(">") And Not (CodeLine.Contains(">>") Or CodeLine.Contains("->")) Then
                    strOperator = ">"
                Else
                    strOperator = ""
                End If


                '== If comparison is taking place continue with check ==
                If strOperator <> "" Then

                    '== Break down code to get the operand either side of the comparison ==
                    arrFragments = Regex.Split(strStatement, strOperator)
                    If arrFragments.Count < 2 Then Return False

                    strLeftSide = arrFragments.First.Trim()
                    strRightSide = arrFragments.ElementAt(1).Trim()

                    '== The sizeof operator returns a signed integer ==
                    If Regex.IsMatch(strRightSide, "\bsizeof\b\s*\(") Then blnIsSizeOfR = True
                    If Regex.IsMatch(strLeftSide, "\bsizeof\b\s*\(") Then blnIsSizeOfL = True

                    '== Get the items immediately adjacent to the comparison operator and trim any spaces/braces from the edges ==
                    arrFragments = Regex.Split(strLeftSide, "(\(|\s+)")
                    strLeftSide = GetLastItem(arrFragments.Last)
                    strLeftSide = strLeftSide.Trim("(")
                    strLeftSide = strLeftSide.Trim(")")

                    arrFragments = Regex.Split(strRightSide, "(\)|\s+)")
                    strRightSide = GetFirstItem(arrFragments.First)
                    strRightSide = strRightSide.Trim("(")
                    strRightSide = strRightSide.Trim(")")

                    '== Remove any increment/decrement operators (++/--) ==
                    strRightSide = TrimOperators(strRightSide)
                    strLeftSide = TrimOperators(strLeftSide)

                    '== Exit if we have no expression, string expression, etc. ==
                    If (strLeftSide = "" Or strRightSide = "") Then Return False
                    If (strLeftSide.Contains("""") Or strRightSide.Contains("""")) Then Return False
                    If (strLeftSide.Contains("'") Or strRightSide.Contains("'")) Then Return False

                    '== If either side of the comparison is NULL then we don't need to proceed any further ==
                    If strLeftSide = "NULL" Or strRightSide = "NULL" Then Return False

                    '== If we find an unsigned comparison anywhere then just exit function and return 'true' ==
                    If IsNumeric(strLeftSide) And IsNumeric(strRightSide) Then
                        Return False
                    ElseIf strLeftSide.StartsWith("0x") Or strRightSide.StartsWith("0x") Then
                        Return False
                    ElseIf IsNumeric(strLeftSide) And Not IsNumeric(strRightSide) Then
                        If Regex.IsMatch(strLeftSide, "\-\d+") And dicUnsigned.ContainsKey(strRightSide) Then
                            Return True
                        Else
                            Return False
                        End If

                    ElseIf IsNumeric(strRightSide) And Not IsNumeric(strLeftSide) Then
                        If Regex.IsMatch(strRightSide, "\-\d+") And dicUnsigned.ContainsKey(strLeftSide) Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        '== Both sides are variable names so check in dictionary ==
                        If (dicUnsigned.ContainsKey(strLeftSide)) And (((Not dicUnsigned.ContainsKey(strRightSide)) And dicInteger.ContainsKey(strRightSide)) And (Not blnIsSizeOfR)) _
                            Or (((Not dicUnsigned.ContainsKey(strLeftSide)) And dicInteger.ContainsKey(strLeftSide)) And (Not blnIsSizeOfL)) And (dicUnsigned.ContainsKey(strRightSide)) Then
                            Return True
                        End If
                    End If

                End If
            End If
        Next

        '== If we have reached this point then no signed/unsigned comparison has been encountered ==
        Return False

    End Function

    Private Function TrimOperators(ByVal VarName As String) As String
        ' Trim any increment and decrement operators from variable names
        '===============================================================
        Dim strRetVal As String = VarName

        If VarName.StartsWith("++") Or VarName.EndsWith("++") Then strRetVal = VarName.Trim("+")
        If VarName.StartsWith("--") Or VarName.EndsWith("--") Then strRetVal = VarName.Trim("-")

        Return strRetVal.Trim()

    End Function

End Class
