Imports System.Drawing
Imports Microsoft.VisualBasic.Language.Default

Public Class KCFBrush

    Public Property O As Brush
    Public Property N As Brush
    Public Property P As Brush

    Public Function GetBrush(atom As String) As Brush
        Select Case atom
            Case "O"
                Return O
            Case "N"
                Return N
            Case "P"
                Return P
            Case Else
                Return Brushes.Black
        End Select
    End Function

    Public Shared Function ChEBITheme() As DefaultValue(Of KCFBrush)
        Return New KCFBrush With {
            .N = Brushes.Blue,
            .O = Brushes.Red,
            .P = Brushes.Orange
        }
    End Function

    Public Shared Function MonoColour() As DefaultValue(Of KCFBrush)
        Return New KCFBrush With {
            .N = Brushes.Black,
            .O = Brushes.Black,
            .P = Brushes.Black
        }
    End Function
End Class
