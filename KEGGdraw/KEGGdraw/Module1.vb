Imports Microsoft.VisualBasic.Serialization.JSON

Module Module1

    Sub Main()

        Dim kcf As KCF = "D:\KEGG\NADPH.txt".LoadKCF
        Call kcf.GetJson.__DEBUG_ECHO
        Call kcf.Draw().Save("D:\KEGG\NADPH.png")

        Pause()
    End Sub
End Module