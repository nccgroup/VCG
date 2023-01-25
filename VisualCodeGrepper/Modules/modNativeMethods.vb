Imports System.CodeDom
Imports System.Runtime.InteropServices

Module modNativeMethods

    ' COM Function to attach the Windows app to the console
    ' Marked as Private as code below will check if they need to be called
    Private Declare Function AttachConsole Lib "kernel32.dll" (ByVal dwProcessId As Integer) As Boolean

    Private Const ATTACH_PARENT_PROCESS = -1
    Private attachAttempted As Boolean

    ''' <summary>
    ''' Attaches the current process to the Console Window. Since this is compiled as a Windows Application,
    ''' we need to lock the Console, so it doesn't exit early.
    ''' </summary>
    Friend Function AttachProcessToConsole() As Boolean

        ' Only attach once, otherwise there will be COM errors
        If (attachAttempted = False) Then
            AttachConsole(ATTACH_PARENT_PROCESS)
            attachAttempted = True
            LogBlank()
            LogInfo("==============================================")
            LogInfo("==            Visual Code Grepper           ==")
            LogInfo("==              CONSOLE MODE                ==")
            LogInfo("==============================================")
            LogInfo("")
            LogInfo("##############################################")
            LogInfo("Please let VCG finish before using the console")
            LogInfo("##############################################")

        End If
        Return attachAttempted
    End Function

    ''' <summary>
    ''' Detaches the Windows App from the Console. Otherwise, the Windows App will lock the Console upon exiting.
    ''' </summary>
    Friend Sub DetachConsole()
        If (attachAttempted = True) Then
            LogInfo("##############################################")
            LogInfo("   Press <Enter> to get your prompt back      ")
            LogInfo("##############################################")
            attachAttempted = False
        End If
    End Sub
End Module
