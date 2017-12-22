Imports KEGGdraw
Imports Microsoft.VisualBasic.Serialization.JSON
Imports SMRUCC.Chemistry.Model
Imports SMRUCC.Chemistry.Model.Graph

Module Module1

    Sub Main()

        Call drawTest()

        Dim list = KegAtomType.KEGGAtomTypes.GetJson
        Dim KCF = IO.LoadKCF("G:\MassSpectrum-toolkits\KCF\DATA\NADPH.txt")
        Dim g = KCF.Graph


        Pause()
    End Sub

    Sub drawTest()

        Dim KCF = IO.LoadKCF("../DATA/NADPH.txt")
        Call KCF.Draw().Save("./ddddd.png")

        Pause()
    End Sub
End Module
