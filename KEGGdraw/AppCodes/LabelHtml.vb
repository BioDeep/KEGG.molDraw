Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Text
Imports Microsoft.VisualBasic.Imaging.LayoutModel
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports Microsoft.VisualBasic.Language
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

    <Extension>
    Private Function getDrawDirection(pt As PointF, bounds As Line2D()) As QuadrantRegions
        Dim boundList = bounds _
            .Select(Function(l)
                        Dim d# = {
                            l.A.Distance(pt),
                            l.B.Distance(pt)
                        }.Min
                        Return (dist:=d, bound:=l)
                    End Function) _
            .Where(Function(t) t.dist < 10) _
            .Select(Function(d) d.bound) _
            .ToArray
        Dim directions As New List(Of QuadrantRegions)
        Dim o, p As PointF

        For Each line As Line2D In boundList
            If line.A.Distance(pt) < line.B.Distance(pt) Then
                ' A是原子基团的的位置
                o = line.A
                p = line.B
            Else
                o = line.B
                p = line.A
            End If

            directions += o.QuadrantRegion(p)
        Next

        Return directions.TopMostFrequent
    End Function

    ''' <summary>
    ''' 处理比较复杂的原子团标签的绘制布局
    ''' </summary>
    ''' <param name="bounds">
    ''' a,b的位置都是原子基团的绘制位置
    ''' </param>
    <Extension>
    Public Sub DrawHtmlLabel(g As IGraphics, label$, atomFont As Font, brush As SolidBrush,
                             pt As PointF,
                             bounds As Line2D())

        ' 处理比较复杂的原子团标签的绘制布局
        Dim singleCharSize As SizeF = g.MeasureString("A", atomFont)
        Dim html = LabelHtml.GetHtmlTuple(label)
        Dim color As Color = brush.Color
        Dim content As TextString()

        ' pt是原子基团的位置, 相当于坐标轴的原点
        ' 则绘制的时候则是绘制在象限相反的位置
        Select Case pt.getDrawDirection(bounds)

            Case QuadrantRegions.LeftBottom, QuadrantRegions.XLeft, QuadrantRegions.LeftTop
                ' 左下角,左边,左上角,则标签应该绘制在右边
                content = TextAPI _
                    .TryParse(html.right, atomFont, color) _
                    .ToArray
                pt = New PointF With {
                    .X = pt.X + singleCharSize.Width / 2,
                    .Y = pt.Y - singleCharSize.Height / 2
                }
            Case QuadrantRegions.YTop
                ' 垂直上方,则标签应该绘制在下方
                content = TextAPI _
                    .TryParse(html.right, atomFont, color) _
                    .ToArray
                pt = New PointF With {
                    .X = pt.X - singleCharSize.Width / 2,
                    .Y = pt.Y + singleCharSize.Height / 2
                }
            Case QuadrantRegions.YBottom
                ' 垂直下方,则标签应该绘制在上方
                content = TextAPI _
                    .TryParse(html.right, atomFont, color) _
                    .ToArray
                pt = New PointF With {
                    .X = pt.X - singleCharSize.Width / 2,
                    .Y = pt.Y - singleCharSize.Height * 1.25
                }
            Case QuadrantRegions.XRight, QuadrantRegions.RightTop, QuadrantRegions.RightBottom
                ' 右边,右上角和右下角,则标签应该绘制在左边
                content = TextAPI _
                    .TryParse(html.left, atomFont, color) _
                    .ToArray
                Dim htmlSize As SizeF = g.MeasureSize(content)
                pt = New PointF With {
                    .X = pt.X - htmlSize.Width,
                    .Y = pt.Y - singleCharSize.Height / 2
                }

            Case Else
                Throw New NotImplementedException(label)
        End Select

        Call HTMLRender.RenderHTML(g, content, pt.ToPoint)
    End Sub
End Module
