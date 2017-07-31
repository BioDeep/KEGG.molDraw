Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Vector.Shapes
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.MIME.Markup.HTML.CSS
Imports Microsoft.VisualBasic.Scripting.Runtime

Public Module Canvas

    ''' <summary>
    ''' Draw the 2D chemical structure
    ''' </summary>
    ''' <param name="kcf"></param>
    ''' <param name="size$"></param>
    ''' <param name="padding$"></param>
    ''' <param name="bg$"></param>
    ''' <param name="font$"></param>
    ''' <returns></returns>
    <Extension>
    Public Function Draw(kcf As KCF,
                         Optional size$ = "800,800",
                         Optional padding$ = g.DefaultPadding,
                         Optional bg$ = "white",
                         Optional font$ = CSSFont.Win7Normal) As GraphicsData

        Dim dot = Brushes.Gray
        Dim atoms As (pt As PointF, atom$)() =
            kcf _
            .Atoms _
            .Select(Function(a)
                        With a.Atom2D_coordinates
                            Dim pt As New PointF(.X, .Y * -1)
                            Return (pt:=pt, Atom:=a.KEGGAtom)
                        End With
                    End Function) _
            .ToArray Or die("No atom elements to plot!", Function(l) DirectCast(l, Array).Length = 0)

        Dim plotInternal =
            Sub(ByRef g As IGraphics, region As GraphicsRegion)
                Dim bounds = atoms.Select(Function(a) a.pt).GetBounds
                Dim scale As New SizeF With {
                    .Width = region.Size.Width / bounds.Width,
                    .Height = region.Size.Height / bounds.Height
                }

                atoms = atoms _
                    .Select(Function(a)
                                Dim pt As New PointF(a.pt.X * scale.Width, a.pt.Y * scale.Height)
                                Return (pt, a.atom)
                            End Function) _
                    .ToArray

                Dim centra As PointF = atoms _
                    .Select(Function(a) a.pt) _
                    .CentralOffset(region.Size)

                For Each atom In atoms
                    Dim pt = atom.pt.OffSet2D(centra)
                    Call g.FillPie(dot, pt.X, pt.Y, 5, 5, 0, 360)
                Next

                For Each bound As Bound In kcf.Bounds
                    Dim a = atoms(bound.from - 1).pt.OffSet2D(centra)
                    Dim b = atoms(bound.to - 1).pt.OffSet2D(centra)
                    Dim line As New Line(a, b)

                    For i As Integer = 1 To bound.bounds
                        Call line.Draw(g)
                        line = line.ParallelShift(10)
                    Next
                Next
            End Sub

        Return g.GraphicsPlots(size.SizeParser, padding, bg, plotInternal)
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
    Public Function UpArrow(a As PointF, b As PointF, c%) As PointF()

    End Function

    Public Function DownArrow(a As PointF, b As PointF, c%) As PointF()
        Dim l# = Math.Sqrt((a.X - b.X) ^ 2 + (a.Y - b.Y) ^ 2)  ' 线段长度
        Dim vetx As PointF() ' 顶点， 底端1， 底端2

        ' vetx 为基本模型
        '
        ' | 底端1
        ' |_________________ 顶点
        ' |
        ' | 底端2

        ' 先构建出两条边的线段函数
        ' y = ax + b
        Dim line1 As Func(Of Double, Double)
        Dim line2 As Func(Of Double, Double)
        Dim lines As New List(Of Line)

        For x As Double = 0 To l Step (l / 5)
            Dim a1 As New PointF(x, line1(x))
            Dim b1 As New PointF(x, line2(x))
            lines.Add(New Line(a1, b1))
        Next

        ' 对所构成的新的shape进行旋转和唯一即可
    End Function
End Module
