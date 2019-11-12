' VisualCodeGrepper - Code security scanner
' Copyright (C) 2012-2019 Nick Dunn and John Murray
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

Public Class PICVar

    '==============================================================================================
    ' COBOL PIC varaibles have their own object type as they have a number of relevant properties.
    ' They can be numeric or alphanumeric, signed or unsigned, and have a length which means
    ' they function alittle like an array.
    ' The sign is important since unsigned PICS are assigned the absolute value if a signed digit
    ' is assigned to them.
    '-----------------------------------------------------------------------------------------------

    Public VarName As String = ""
    Public IsSigned As Boolean = False
    Public IsNumeric As Boolean = False
    Public Length As Integer = 1

End Class
