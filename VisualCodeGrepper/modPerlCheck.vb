Option Explicit On

Imports System.Text.RegularExpressions

Module modPerlCheck

    ' Specific checks for Perl code
    '==============================

    Public Sub CheckPerlCode(ByVal CodeLine As String, ByVal FileName As String)
        ' Carry out any specific checks for the language in question
        '===========================================================


        ' Check for potential SQLi
        ' Identify poor/indiscriminate error handling 

    End Sub

End Module
