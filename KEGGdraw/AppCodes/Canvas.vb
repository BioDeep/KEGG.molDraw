#Region "Microsoft.VisualBasic::ef0aeaf377d0541271b6c413c7dcc615, KCF\KEGGdraw\AppCodes\Canvas.vb"

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

    ' Module Canvas
    ' 
    '     Function: DownArrow, Draw, GetLabel, getPolygon, UpArrow
    ' 
    '     Sub: drawParallelLines
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Shapes
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports Microsoft.VisualBasic.MIME.Markup.HTML.CSS
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports Microsoft.VisualBasic.Serialization.JSON
Imports SMRUCC.Chemistry.Model
Imports Line2D = Microsoft.VisualBasic.Imaging.Drawing2D.Shapes.Line

''' <summary>
''' 绘制分子结构图的画布对象
''' </summary>
Public Module Canvas

    <Extension>
    Private Function getPolygon(atoms As (pt As PointF, atom As Atom)(),
                                scaleFactor#,
                                region As GraphicsRegion,
                                bounds As RectangleF) As PointF()

        If bounds.Width > bounds.Height Then
            Return atoms.Select(Function(a) a.pt).Enlarge(scaleFactor * region.Size.Width / bounds.Width)
        Else
            Return atoms.Select(Function(a) a.pt).Enlarge(scaleFactor * region.Size.Height / bounds.Height)
        End If
    End Function

    ''' <summary>
    ''' Draw the 2D chemical structure
    ''' </summary>
    ''' <param name="kcf"></param>
    ''' <param name="size$"></param>
    ''' <param name="padding$"></param>
    ''' <param name="bg$"></param>
    ''' <param name="font$"></param>
    ''' <param name="monoColour">禁用所有的颜色设置，出图只会有黑白两种颜色</param>
    ''' <param name="theme">Default color theme is <see cref="KCFBrush.ChEBITheme()"/></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 在2D平面上面表现出空间层次：在KCF文件之中，使用下面的标记信息来标识空间的层次信息
    ''' 
    ''' + ``#Up``：实箭头
    ''' + ``#Down``：虚箭头
    ''' 
    ''' 对于这两种类型的空间层次，都是箭头指向碳原子，即箭头的尖头的部分是指向碳原子的
    ''' </remarks>
    <Extension>
    Public Function Draw(kcf As KCF,
                         Optional size$ = "2000,2000",
                         Optional padding$ = g.DefaultPadding,
                         Optional bg$ = "white",
                         Optional font$ = "font-style: strong; font-size: 72; font-family: " & FontFace.MicrosoftYaHei & ";",
                         Optional scaleFactor# = 0.85,
                         Optional boundStroke$ = "stroke: black; stroke-width: 15px; stroke-dash: solid;",
                         Optional monoColour As Boolean = False,
                         Optional theme As KCFBrush = Nothing) As GraphicsData

        Dim background As Brush = bg.GetBrush
        Dim atomFont As Font = CSSFont.TryParse(font).GDIObject
        Dim dot = Brushes.Gray
        Dim boundsPen As Pen = Stroke.TryParse(boundStroke).GDIObject
        Dim labelSize!
        Dim layoutPoint As PointF
        Dim layoutSize As SizeF
        Dim atomLabelLayout As RectangleF
        Dim atoms As (pt As PointF, atom As Atom)() =
            kcf _
            .Atoms _
            .Select(Function(a)
                        With a.Atom2D_coordinates
                            Dim pt As New PointF(.X, .Y * -1)
                            Return (pt:=pt, Atom:=a)
                        End With
                    End Function) _
            .ToArray Or die("No atom elements to plot!")

        theme = (theme Or KCFBrush.ChEBITheme) Or KCFBrush.MonoColour.When(monoColour)

        Dim plotInternal =
            Sub(ByRef g As IGraphics, region As GraphicsRegion)
                Dim bounds = atoms.Select(Function(a) a.pt).GetBounds
                Dim polygon As PointF() = atoms.getPolygon(scaleFactor, region, bounds)

                atoms = atoms _
                    .Select(Function(a, i)
                                Return (pt:=polygon(i), Atom:=a.atom)
                            End Function) _
                    .ToArray

                Dim centra As PointF = atoms _
                    .Select(Function(a) a.pt) _
                    .CentralOffset(region.Size)

                For Each bound As Bound In kcf.Bounds
                    Dim U = atoms(bound.from - 1)
                    Dim V = atoms(bound.to - 1)
                    Dim a = U.pt.OffSet2D(centra)
                    Dim b = V.pt.OffSet2D(centra)
                    Dim penColor As Brush

                    If U.atom.Atom <> "C" Then
                        penColor = theme.GetBrush(U.atom.Atom)
                    ElseIf V.atom.Atom <> "C" Then
                        penColor = theme.GetBrush(V.atom.Atom)
                    Else
                        penColor = Brushes.Black
                    End If

                    boundsPen.Brush = penColor

                    If bound.dimentional_levels.StringEmpty Then
                        Dim line As New Line2D(a, b) With {
                            .Stroke = boundsPen
                        }

                        If bound.bounds > 1 Then
                            Call g.drawParallelLines(bound, line)
                        Else
                            Call line.Draw(g)
                        End If
                    Else
                        If bound.dimentional_levels = "#Up" Then
                            Call UpArrow(a, b, boundsPen.Width * 2)(g, penColor)
                        ElseIf bound.dimentional_levels = "#Down" Then
                            Call DownArrow(a, b, boundsPen.Width * 2)(g, boundsPen)
                        Else
                            Dim ex As New NotImplementedException(bound.GetJson)
                            ex = New NotImplementedException(kcf.Entry.Id, ex)
                            Throw ex
                        End If
                    End If
                Next

                Dim left, top As Single

                For Each atom As (pt As PointF, atom As Atom) In atoms
                    With atom
                        ' 只显示出非碳原子的标签
                        If .atom.Atom.TextEquals("C") Then
                            Continue For
                        End If

                        Dim pt As PointF = .pt.OffSet2D(centra)
                        Dim label$ = .atom.GetLabel
                        Dim brush = theme.GetBrush(.atom.Atom)

                        With g.MeasureString(label, atomFont)
                            pt = New PointF(pt.X - .Width / 2, pt.Y - .Height / 2)
                            labelSize = Math.Min(.Width, .Height)
                            left = pt.X + (.Width - labelSize) / 2
                            top = pt.Y + (.Height - labelSize) / 2
                            layoutPoint = New PointF(left, top)
                            layoutSize = New SizeF(labelSize / 2, labelSize / 2)
                            atomLabelLayout = New RectangleF(layoutPoint, layoutSize)

                            g.FillEllipse(background, atomLabelLayout)
                            g.DrawString(label, atomFont, brush, pt)
                        End With
                    End With
                Next
            End Sub

        Return g.GraphicsPlots(
            size.SizeParser, padding,
            bg,
            plotInternal)
    End Function

    ''' <summary>
    ''' 绘制双键或者三键
    ''' </summary>
    ''' <param name="g"></param>
    ''' <param name="bound"></param>
    ''' <param name="line"></param>
    <Extension>
    Private Sub drawParallelLines(g As IGraphics, bound As Bound, line As Line2D)
        Dim lineWidth! = line.Stroke.Width / bound.bounds
        Dim newPen As New Pen(line.Stroke.Brush, lineWidth)

        line = New Line2D(line.A, line.B) With {.Stroke = newPen}
        line = line.ParallelShift(lineWidth * (-bound.bounds + 1))

        For i As Integer = 1 To bound.bounds
            Call line.Draw(g)
            line = line.ParallelShift(lineWidth * 2)
        Next
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Private Function GetLabel(atom As Atom) As String
        Return atom.KEGGAtom.view Or atom.Atom.AsDefault
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>
    ''' <param name="c%">
    ''' 碳原子的位置，碳原子为箭头的顶点：
    ''' 
    ''' 1 - <paramref name="a"/>
    ''' 2 - <paramref name="b"/>
    ''' </param>
    ''' <returns></returns>
    Public Function UpArrow(a As PointF, b As PointF, c%) As Action(Of IGraphics, Brush)
        Dim l As New Line2D(a, b)  ' 线段长度

        With New Path2D
            Dim vetx As PointF() = Arrow _
                .ArrowHead(c, l.Length) _
                .Rotate(-l.Alpha - Math.PI) _
                .MoveTo(l.Center, MoveTypes.PolygonCentre)

            Call .MoveTo(vetx(Scan0))
            Call .LineTo(vetx(1))
            Call .LineTo(vetx(2))
            Call .CloseAllFigures()

            Return Sub(g, br)
                       Call g.FillPath(br, .Path)
                   End Sub
        End With
    End Function

    ''' <summary>
    ''' ``#Down``
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>
    ''' <param name="c%"></param>
    ''' <returns></returns>
    Public Function DownArrow(a As PointF, b As PointF, c%) As Action(Of IGraphics, Pen)
        Dim l As New Line2D(a, b)  ' 线段长度
        Dim vetx As PointF() = Arrow.ArrowHead(c, l.Length) ' 顶点， 底端1， 底端2

        ' vetx 为基本模型
        '
        ' | 底端1
        ' |_________________ 顶点
        ' |
        ' | 底端2

        ' 先构建出两条边的线段函数
        ' y = ax + b
        Dim line1 As fx = PrimitiveLinearEquation(vetx(0), vetx(1))
        Dim line2 As fx = PrimitiveLinearEquation(vetx(0), vetx(2))
        Dim lines As New List(Of PointF)
        Dim len% = l.Length

        For x As Double = 0 To len Step (len / 5)
            Dim a1 As New PointF(x, line1(x))
            Dim b1 As New PointF(x, line2(x))

            lines += {a1, b1}
        Next

        ' 对所构成的新的shape进行旋转和位移即可
        Return Sub(g, pen)
                   For Each line As PointF() In lines _
                       .Rotate(-l.Alpha - Math.PI) _
                       .MoveTo(l.Center, MoveTypes.PolygonCentre) _
                       .Split(2)

                       Call g.DrawLine(pen, line(0), line(1))
                   Next
               End Sub
    End Function
End Module

