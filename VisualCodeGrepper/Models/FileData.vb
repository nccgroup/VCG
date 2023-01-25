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

Public Class FileData
    ' Used to store the data for each file scanned. This is used to populate the  
    ' charts and visual breakdown without re-scanning.
    '===============================================================================

    Public ShortName As String = ""
    Public FileName As String = ""

    Public LineCount As Long = 0
    Public CodeCount As Long = 0
    Public CommentCount As Long = 0
    Public WhitespaceCount As Long = 0
    Public FixMeCount As Long = 0
    Public BadFuncCount As Long = 0

End Class
