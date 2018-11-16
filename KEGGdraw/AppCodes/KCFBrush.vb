Imports System.Drawing
Imports Microsoft.VisualBasic.Language.Default

''' <summary>
''' 原子和化学键的颜色画刷
''' </summary>
Public Class KCFBrush

    Public Property O As Brush
    Public Property N As Brush
    Public Property P As Brush
    Public Property S As Brush

    Public Function GetBrush(atom As String) As Brush
        Select Case atom
            Case "O"
                Return O
            Case "N"
                Return N
            Case "P"
                Return P
            Case "S"
                Return S
            Case Else
                Return Brushes.Black
        End Select
    End Function

    Public Shared Function ChEBITheme() As DefaultValue(Of KCFBrush)
        Return New KCFBrush With {
            .N = Brushes.Blue,
            .O = Brushes.Red,
            .P = Brushes.Orange,
            .S = Brushes.DarkOliveGreen
        }
    End Function

    ''' <summary>
    ''' 所有的原子和化学键都是黑色画刷
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function MonoColour() As DefaultValue(Of KCFBrush)
        Return New KCFBrush With {
            .N = Brushes.Black,
            .O = Brushes.Black,
            .P = Brushes.Black,
            .S = Brushes.Black
        }
    End Function
End Class
