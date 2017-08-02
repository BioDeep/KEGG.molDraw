Imports Microsoft.VisualBasic.Serialization.JSON

Module Module1

    Sub Main()

        Dim kcf As KCF = "../DATA/NADPH.txt".LoadKCF
        Call kcf.GetJson.__DEBUG_ECHO
        Call kcf.Draw().Save("../DATA/NADPH.png")

        Pause()
    End Sub
End Module