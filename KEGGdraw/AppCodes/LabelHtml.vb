#Region "Microsoft.VisualBasic::bb2356dd025c1a6c6503a857d846a13a, mzkit\src\visualize\KCF\KEGGdraw\AppCodes\LabelHtml.vb"

    ' Author:
    ' 
    '       xieguigang (gg.xie@bionovogene.com, BioNovoGene Co., LTD.)
    ' 
    ' Copyright (c) 2018 gg.xie@bionovogene.com, BioNovoGene Co., LTD.
    ' 
    ' 
    ' MIT License
    ' 
    ' 
    ' Permission is hereby granted, free of charge, to any person obtaining a copy
    ' of this software and associated documentation files (the "Software"), to deal
    ' in the Software without restriction, including without limitation the rights
    ' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    ' copies of the Software, and to permit persons to whom the Software is
    ' furnished to do so, subject to the following conditions:
    ' 
    ' The above copyright notice and this permission notice shall be included in all
    ' copies or substantial portions of the Software.
    ' 
    ' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    ' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    ' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    ' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    ' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    ' SOFTWARE.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 174
    '    Code Lines: 131
    ' Comment Lines: 24
    '   Blank Lines: 19
    '     File Size: 6.44 KB


    ' Module LabelHtml
    ' 
    '     Function: getDrawDirection, GetHtmlTuple, IsAtomLabel
    ' 
    '     Sub: DrawHtmlLabel
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Text
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.MIME.Html.Render
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
            Case "C1a"
                Return ("H<sub>3</sub>C", "CH<sub>3</sub>")
            Case Else
                If label.IsAtomLabel Then
                    Return (label, label)
                Else
                    Throw New NotImplementedException($"Please add label for: [{label}]")
                End If
        End Select
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function IsAtomLabel(label As String) As Boolean
        Return Strings.Len(label) = 2 AndAlso Char.IsUpper(label(0)) AndAlso Char.IsLower(label(1))
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
            .Where(Function(t) t.dist < 5) _
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
                             bounds As Line2D(),
                             charSize As SizeF)

        ' 处理比较复杂的原子团标签的绘制布局
        Dim html = LabelHtml.GetHtmlTuple(label)
        Dim color As Color = brush.Color
        Dim content As TextString()
        Dim left As TextString() = TextAPI _
            .TryParse(html.right, atomFont, color) _
            .ToArray
        Dim right As TextString() = TextAPI _
            .TryParse(html.left, atomFont, color) _
            .ToArray
        Dim dir As QuadrantRegions = pt.getDrawDirection(bounds)

        ' pt是原子基团的位置, 相当于坐标轴的原点
        ' 则绘制的时候则是绘制在象限相反的位置
        Select Case dir

            Case QuadrantRegions.LeftBottom
                ' 左下角, 则标签应该绘制在右边 
                content = left
                pt = New PointF With {
                    .X = pt.X - charSize.Width / 2,
                    .Y = pt.Y
                }

            Case QuadrantRegions.LeftTop
                content = left
                pt = New PointF With {
                    .X = pt.X - charSize.Width / 3,
                    .Y = pt.Y
                }

            Case QuadrantRegions.XLeft
                ' 左边,左上角,则标签应该绘制在右边
                content = TextAPI _
                    .TryParse(html.right, atomFont, color) _
                    .ToArray
                pt = New PointF With {
                    .X = pt.X,
                    .Y = pt.Y - charSize.Height / 2
                }
            Case QuadrantRegions.YTop
                ' 垂直上方,则标签应该绘制在下方
                content = TextAPI _
                    .TryParse(html.right, atomFont, color) _
                    .ToArray
                pt = New PointF With {
                    .X = pt.X - charSize.Width / 2,
                    .Y = pt.Y
                }
            Case QuadrantRegions.YBottom
                ' 垂直下方,则标签应该绘制在上方
                content = left
                pt = New PointF With {
                    .X = pt.X - charSize.Width / 2,
                    .Y = pt.Y - charSize.Height
                }
            Case QuadrantRegions.XRight, QuadrantRegions.RightBottom
                ' 右边,和右下角,则标签应该绘制在左边
                Dim htmlSize As SizeF = g.MeasureSize(right)

                pt = New PointF With {
                    .X = pt.X - htmlSize.Width,
                    .Y = pt.Y - charSize.Height / 2
                }
                content = right

            Case QuadrantRegions.RightTop
                ' 右上角
                Dim htmlSize As SizeF = g.MeasureSize(right)

                pt = New PointF With {
                    .X = pt.X - htmlSize.Width + charSize.Width / 3,
                    .Y = pt.Y
                }
                content = right

            Case Else

                ' 对于金属单质或者离子之类的,只有一个原子标签
                ' 这个时候就直接绘制吧
                ' 例如 C19157 物质的KCF数据就只有一个原子的标签
                If label.IsAtomLabel AndAlso dir = QuadrantRegions.Origin Then
                    content = left
                Else
                    Throw New NotImplementedException($"[{dir.Description}] {label}")
                End If
        End Select

        Call HTMLRender.RenderHTML(g, content, pt.ToPoint)
    End Sub
End Module
