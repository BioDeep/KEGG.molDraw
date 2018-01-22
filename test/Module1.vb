Imports System.Drawing
Imports KEGGdraw
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.BitmapImage
Imports Microsoft.VisualBasic.Serialization.JSON
Imports SMRUCC.Chemistry.Model
Imports SMRUCC.Chemistry.Model.Graph

Module Module1

    Sub Main()

        '  Call layerSourceTest()
        Call drawTest()

        Dim list = KegAtomType.KEGGAtomTypes.GetJson
        Dim KCF = IO.LoadKCF("G:\MassSpectrum-toolkits\KCF\DATA\NADPH.txt")
        Dim g = KCF.Graph


        Pause()
    End Sub

    Sub drawTest()

        Dim KCF = IO.LoadKCF("../DATA/NADPH.txt")
        Call KCF.Draw().Save("../DATA/NADPH.png")

        KCF = IO.LoadKCF("../DATA/L-Citrulline.txt")
        Call KCF.Draw().Save("../DATA/L-Citrulline.png")

        KCF = IO.LoadKCF("../DATA/Tetrahydrofolate.txt")
        Call KCF.Draw().Save("../DATA/Tetrahydrofolate.png")

        KCF = IO.LoadKCF("../DATA/CoA.txt")
        Call KCF.Draw().Save("../DATA/CoA.png")

        Pause()
    End Sub

    Sub layerSourceTest()
        Dim KCF = IO.LoadKCF("../DATA/Tetrahydrofolate.txt")
        Dim image = KCF.Draw().AsGDIImage

        Dim tr = image.ColorReplace(Color.White, Color.Transparent)

        Call tr.SaveAs("./ttttt.png")

        Call tr.ResizeUnscaledByHeight(500).SaveAs("./fdffsfsd.png")

        Pause()
    End Sub
End Module
