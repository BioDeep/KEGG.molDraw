Imports Microsoft.VisualBasic.Serialization.JSON

Module Module1

    Sub Main()

        Dim kcf As KCF = "../../../../NADPH.txt".LoadKCF
        Call kcf.GetJson.__DEBUG_ECHO
        Call kcf.Draw().Save("../../../../NADPH.png")

        Pause()
    End Sub
End Module