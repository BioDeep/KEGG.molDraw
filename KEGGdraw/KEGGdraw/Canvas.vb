Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.MIME.Markup.HTML.CSS
Imports Microsoft.VisualBasic.Scripting

Public Module Canvas

    <Extension>
    Public Function Draw(kcf As KCF, Optional size$ = "800,800", Optional padding$ = g.DefaultPadding, Optional bg$ = "white", Optional font$ = CSSFont.Win7Normal) As GraphicsData
        Dim plotInternal =
            Sub(ByRef g As IGraphics, region As GraphicsRegion)

            End Sub

        Return g.GraphicsPlots(size.SizeParser, padding, bg, plotInternal)
    End Function
End Module
