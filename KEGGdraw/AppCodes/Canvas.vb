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
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Text.ASCIIArt
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
        Dim factor#
        Dim canvas = region.PlotRegion

        If bounds.Width > bounds.Height Then
            factor = canvas.Width / bounds.Width
        Else
            factor = canvas.Height / bounds.Height
        End If

        Return atoms _
            .Select(Function(a) a.pt) _
            .Enlarge(scaleFactor * factor)
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
                         Optional size$ = "2700,2300",
                         Optional padding$ = g.DefaultUltraLargePadding,
                         Optional bg$ = "white",
                         Optional font$ = "font-style: normal; font-size: 90; font-family: " & FontFace.SegoeUI & ";",
                         Optional scaleFactor# = 0.85,
                         Optional boundStroke$ = "stroke: black; stroke-width: 15px; stroke-dash: solid;",
                         Optional monoColour As Boolean = False,
                         Optional theme As KCFBrush = Nothing) As GraphicsData

        Dim background As Brush = bg.GetBrush
        Dim atomFont As Font = CSSFont.TryParse(font).GDIObject
        Dim dot = Brushes.Gray
        Dim boundsPen As Pen = Stroke.TryParse(boundStroke).GDIObject
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
                Dim charSize As SizeF = g.GetGeneralSize("ABCDEFGHIJKLMNOPQRSTUVWXYZ", atomFont)

                atoms = atoms _
                    .Select(Function(a, i)
                                Return (pt:=polygon(i), Atom:=a.atom)
                            End Function) _
                    .ToArray

                Dim centra As PointF = atoms _
                    .Select(Function(a) a.pt) _
                    .CentralOffset(region.Size)
                Dim layoutElements As Line2D() = kcf _
                    .drawBoundsConnection(atoms, centra, theme, boundsPen, g, charSize) _
                    .ToArray

                For Each atom As (pt As PointF, atom As Atom) In atoms
                    With atom
                        ' 只显示出非碳原子的标签
                        If .atom.Atom.TextEquals("C") Then
                            Continue For
                        Else
                            Call g.drawLabel(
                                .pt, .atom, centra,
                                theme,
                                atomFont,
                                background,
                                bounds:=layoutElements,
                                charSize:=charSize
                            )
                        End If
                    End With
                Next
            End Sub

        Return g.GraphicsPlots(
            size.SizeParser, padding,
            bg,
            plotInternal
        )
    End Function

    ''' <summary>
    ''' 首先绘制原子基团之间的边链接
    ''' </summary>
    ''' <param name="kcf"></param>
    ''' <param name="atoms"></param>
    ''' <param name="centra"></param>
    ''' <returns></returns>
    <Extension>
    Private Iterator Function drawBoundsConnection(kcf As KCF,
                                                   atoms As (pt As PointF, atom As Atom)(),
                                                   centra As PointF,
                                                   theme As KCFBrush,
                                                   boundsPen As Pen,
                                                   g As IGraphics,
                                                   charSize As SizeF) As IEnumerable(Of Line2D)

        Dim maxSize! = {charSize.Width, charSize.Height}.Max
        Dim dashPen As New Pen(boundsPen.Color, boundsPen.Width / 2)

        For Each bound As Bound In kcf.Bounds
            Dim U = atoms(bound.from - 1)
            Dim V = atoms(bound.to - 1)
            Dim a = U.pt.OffSet2D(centra)
            Dim b = V.pt.OffSet2D(centra)
            Dim la As PointF = a
            Dim lb As PointF = b
            Dim penColor As Brush

            If U.atom.Atom <> "C" Then
                penColor = theme.GetBrush(U.atom.Atom)
            ElseIf V.atom.Atom <> "C" Then
                penColor = theme.GetBrush(V.atom.Atom)
            Else
                penColor = Brushes.Black
            End If

            Dim t = 1

            If U.atom.Atom <> "C" Then
                la = New Line2D(la, lb).LengthVariationFromPointA(-maxSize / 3).A
                t = 1.5
            End If
            If V.atom.Atom <> "C" Then
                If t > 1 Then
                    lb = New Line2D(la, lb).LengthVariationFromPointB(-maxSize / 8).B
                Else
                    lb = New Line2D(la, lb).LengthVariationFromPointB(-maxSize / 3).B
                End If
            End If

            boundsPen.Brush = penColor

            If bound.dimentional_levels.StringEmpty Then
                Dim line As New Line2D(la, lb) With {
                    .Stroke = boundsPen
                }

                If bound.bounds > 1 Then
                    Call g.drawParallelLines(bound, line)
                Else
                    Call line.Draw(g)
                End If

                Yield New Line2D(a, b)
            Else
                If bound.dimentional_levels = "#Up" Then
                    Call UpArrow(la, lb, boundsPen.Width * 2)(g, penColor)
                ElseIf bound.dimentional_levels = "#Down" Then
                    Call DownArrow(la, lb, boundsPen.Width * 1.25)(g, dashPen)
                ElseIf bound.dimentional_levels = "#Either" Then
                    Call ZigzagArrow(la, lb, boundsPen.Width * 2.5)(g, dashPen)
                Else
                    Call throwHelper(kcf.Entry.Id, bound)
                End If

                Yield New Line2D(a, b)
            End If
        Next
    End Function

    Private Sub throwHelper(entryID$, bound As Bound)
        Dim ex As New NotImplementedException(bound.GetJson)
        Throw New NotImplementedException(entryID, ex)
    End Sub

    <Extension> Private Sub drawLabel(g As IGraphics,
                                      atomPt As PointF,
                                      atom As Atom,
                                      centra As PointF,
                                      theme As KCFBrush,
                                      atomFont As Font,
                                      background As Brush,
                                      bounds As Line2D(),
                                      charSize As SizeF)

        Dim pt As PointF = atomPt.OffSet2D(centra)
        Dim label$ = atom.GetLabel
        Dim brush As SolidBrush = theme.GetBrush(atom.Atom)
        Dim labelSize!
        Dim layoutSize As SizeF
        Dim atomLabelLayout As RectangleF

        If label.Length > 1 Then
            Call g.DrawHtmlLabel(label, atomFont, brush, pt, bounds, charSize)
        Else
            With g.MeasureString(label, atomFont)

                ' Call $"Draw single: {label}=[{ .Width},{ .Height}]".__DEBUG_ECHO

                ' 只有一个原子标签的情况
                ' 在该原子的位置上面居中显示
                pt = New PointF(pt.X - .Width / 2, pt.Y - .Height / 2)
                labelSize = Math.Max(.Width, .Height)
                layoutSize = New SizeF(.Width, .Height)
                atomLabelLayout = New RectangleF With {
                    .Location = New PointF With {
                        .X = pt.X + layoutSize.Width / 2,
                        .Y = pt.Y + layoutSize.Height / 2
                    },
                    .Size = New SizeF(labelSize / 2, labelSize / 2)
                }
            End With

            Call g.DrawString(label, atomFont, brush, pt)
        End If
    End Sub

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
    ''' 大箭头
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
        ' 线段长度
        Dim l As New Line2D(a, b)

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
    ''' 折线箭头
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>
    ''' <param name="c%"></param>
    ''' <returns></returns>
    Public Function ZigzagArrow(a As PointF, b As PointF, c%) As Action(Of IGraphics, Pen)
        ' 线段长度
        Dim l As New Line2D(a, b)
        ' 顶点， 底端1， 底端2
        Dim vetx As PointF() = Arrow.ArrowHead(c, l.Length)
        Dim line1 As fx = PrimitiveLinearEquation(vetx(0), vetx(1))
        Dim line2 As fx = PrimitiveLinearEquation(vetx(0), vetx(2))
        Dim lines As New List(Of PointF)
        Dim len% = l.Length
        Dim previous As New PointF(0, line1(0))
        Dim t As Boolean = True

        ' 在这里构建Z字型的折线链接
        For x As Double = 0 To len Step (len / 15)
            Dim a1 As New PointF(x, line1(x))
            Dim b1 As New PointF(x, line2(x))

            If t Then
                lines += {previous, a1}
                previous = a1
            Else
                lines += {previous, b1}
                previous = b1
            End If

            t = Not t
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

    ''' <summary>
    ''' ``#Down`` 虚线箭头
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>
    ''' <param name="c%"></param>
    ''' <returns></returns>
    Public Function DownArrow(a As PointF, b As PointF, c%) As Action(Of IGraphics, Pen)
        ' 线段长度
        Dim l As New Line2D(a, b)
        ' 顶点， 底端1， 底端2
        Dim vetx As PointF() = Arrow.ArrowHead(c, l.Length)

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

        For x As Double = 0 To len Step (len / 8)
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

