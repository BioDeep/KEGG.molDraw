Imports KCF.Graph
Imports Microsoft.VisualBasic.Serialization.JSON

Module Module1

    Sub Main()

        Dim list = Global.KCF.IO.KegAtomType.KEGGAtomTypes.GetJson
        Dim KCF = Global.KCF.IO.LoadKCF("G:\MassSpectrum-toolkits\KCF\DATA\NADPH.txt")
        Dim g = KCF.Graph


        Pause()
    End Sub
End Module
