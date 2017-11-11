Imports Microsoft.VisualBasic.Serialization.JSON
Imports SMRUCC.Chemistry.Model
Imports SMRUCC.Chemistry.Model.Graph

Module Module1

    Sub Main()

        Dim list = KegAtomType.KEGGAtomTypes.GetJson
        Dim KCF = IO.LoadKCF("G:\MassSpectrum-toolkits\KCF\DATA\NADPH.txt")
        Dim g = KCF.Graph


        Pause()
    End Sub
End Module
