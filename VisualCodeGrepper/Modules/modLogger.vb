Module modLogger
    ''' <summary>
    ''' Writes a log message in the Information category.
    ''' </summary>
    ''' <param name="text">The text to render.</param>
    ''' <param name="args">Formatting parameters associated with the text.</param>
    Public Sub LogInfo(text As String, ParamArray args() As Object)
        WriteLine("[+] " & text, args)
    End Sub
    ''' <summary>
    ''' Writes a log message in the Verbose category.
    ''' </summary>
    ''' <param name="text">The text to render.</param>
    ''' <param name="args">Formatting parameters associated with the text.</param>
    Public Sub LogVerbose(text As String, ParamArray args() As Object)
        ' Only write to .Out, as .Error will not generate output
        If asAppSettings.IsVerbose = True Then
            WriteLine("[*] " & text, args)
        End If
    End Sub

    ''' <summary>
    ''' Provides logging but with no prefixes on any outputs.
    ''' </summary>
    ''' <param name="text">The text to render.</param>
    ''' <param name="args">Formatting parameters associated with the text.</param>
    Public Sub LogBlank(text As String, ParamArray args() As Object)
        WriteLine(text, args)
    End Sub

    ''' <summary>
    ''' Provides a blank line logging feature.
    ''' </summary>
    Public Sub LogBlank()
        WriteLine("")
    End Sub
    ''' <summary>
    ''' Writes a log message in the Error category.
    ''' </summary>
    ''' <param name="text">The text to render.</param>
    ''' <param name="args">Formatting parameters associated with the text.</param>
    Public Sub LogError(text As String, ParamArray args() As Object)
        ' Show console error for incorrect command line options
        '======================================================
        WriteLine("[!] " & text, args)
    End Sub

    ''' <summary>
    ''' Writes a log message with a carriage return/line feed.
    ''' </summary>
    ''' <param name="text">The text to render.</param>
    ''' <param name="args">Formatting parameters associated with the text.</param>
    Private Sub WriteLine(text As String, ParamArray args() As Object)
        If asAppSettings.IsConsole = True Then
            Console.Out.WriteLine(text, args)
        End If
    End Sub
    ''' <summary>
    ''' Writes a message to the logger, but no carriage-return. This allows console logging to use '\r', but won't be compatible on something like file logging.
    ''' </summary>
    ''' <param name="text">The text to render.</param>
    ''' <param name="args">Formatting parameters associated with the text.</param>
    Private Sub Write(text As String, ParamArray args() As Object)
        If asAppSettings.IsConsole = True Then
            Console.Out.Write(text, args)
        End If
    End Sub
End Module
