Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.MIME.Markup.HTML.CSS
Imports Microsoft.VisualBasic.Scripting

Public Module Canvas

    <Extension>
    Public Function Draw(kcf As KCF, Optional size$ = "800,800", Optional padding$ = g.DefaultPadding, Optional bg$ = "white", Optional font$ = CSSFont.Win7Normal) As GraphicsData
        Dim atoms = kcf.Atoms _
            .Select(Function(a)
                        Return (pt:=a.Atom2D_coordinates.ToPointF(100000), Atom:=a.KEGGAtom)
                    End Function) _
            .ToArray
        Dim dot = Brushes.Gray
        Dim plotInternal =
            Sub(ByRef g As IGraphics, region As GraphicsRegion)

                For Each atom In atoms
                    Dim pt As New PointF(atom.pt.X, atom.pt.Y * -1)
                    Call g.FillPie(dot, pt.X * 10, pt.Y * 10, 5, 5, 0, 360)
                Next

            End Sub

        Return g.GraphicsPlots(size.SizeParser, padding, bg, plotInternal)
    End Function
End Module
