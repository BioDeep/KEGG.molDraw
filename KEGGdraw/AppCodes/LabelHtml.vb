Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Text
Imports Microsoft.VisualBasic.Imaging.LayoutModel
Imports Microsoft.VisualBasic.MIME.Markup.HTML
Imports Line2D = Microsoft.VisualBasic.Imaging.Drawing2D.Shapes.Line

Module LabelHtml

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="label"></param>
    ''' <returns>``&lt;= =>``</returns>
    Public Function GetHtmlTuple(label As String) As (left$, right$)
        Select Case label
            Case "NH2"
                Return ("H<sub>2</sub>N", "NH<sub>2</sub>")
            Case "OH"
                Return ("HO", "OH")
            Case Else
                Throw New NotImplementedException(label)
        End Select
    End Function

    ''' <summary>
    ''' 处理比较复杂的原子团标签的绘制布局
    ''' </summary>
    ''' 
    <Extension>
    Public Sub DrawHtmlLabel(g As IGraphics, label$, atomFont As Font, brush As SolidBrush,
                             pt As PointF,
                             conflictions As List(Of Line2D))

        ' 处理比较复杂的原子团标签的绘制布局
        Dim singleCharSize As SizeF = g.MeasureString("A", atomFont)
        Dim html = LabelHtml.GetHtmlTuple(label)
        Dim color As Color = brush.Color
        Dim content As TextString() = TextAPI _
            .TryParse(html.right, atomFont, color) _
            .ToArray
        Dim htmlSize As SizeF = g.MeasureSize(content)
        ' 首先计算从左往右的顺序
        Dim layout As New Rectangle2D(
            pt.X - singleCharSize.Width / 2,
            pt.Y - htmlSize.Height,
            htmlSize.Width,
            htmlSize.Height
        )

        If conflictions _
            .Any(Function(line)
                     Return Not line.GetIntersectLocation(layout) Is Nothing
                 End Function) Then

            ' 存在冲突，则反过来，从右到左
            content = TextAPI _
                .TryParse(html.left, atomFont, color) _
                .ToArray
            layout = New Rectangle2D(
                pt.X - htmlSize.Width + singleCharSize.Width / 2,
                pt.Y - htmlSize.Height,
                htmlSize.Width,
                htmlSize.Height
            )
        End If

        Call HTMLRender.RenderHTML(g, content, layout.Point)
    End Sub
End Module
