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

Public Class CodeIssue
    ' Used to store the name of a dangerous function along with any associated 
    ' description and rationale that will be provided to the user
    '
    ' N.B. - a dictionary was not feasible as the description will not always be present
    '       and this more flexible approach allows for severity levels, etc.
    '===================================================================================

    '=================================================
    '== Constants to mark the severity of the issue ==
    '-------------------------------------------------
    Public Const CRITICAL = 1
    Public Const HIGH = 2
    Public Const MEDIUM = 3
    Public Const STANDARD = 4
    Public Const LOW = 5
    Public Const INFO = 6
    Public Const POSSIBLY_SAFE = 7
    '=================================================

    Public FunctionName As String
    Public Description As String

    Public Severity As Integer = STANDARD

End Class
