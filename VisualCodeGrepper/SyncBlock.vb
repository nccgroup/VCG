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

Public Class SyncBlock

    Public BlockIndex = 0
    Public OuterObject As String = ""
    Public InnerObjects As New ArrayList

    Public Function IsLockedBy(ByVal InnerObject As String) As Boolean
        ' Checks if an object name is in the list of locked objects
        '==========================================================
        Dim blnRetVal As Boolean = False

        If InnerObjects.Contains(InnerObject) Then blnRetVal = True

        Return blnRetVal

    End Function

End Class
