Imports Microsoft.VisualBasic.Serialization.JSON

Module Module1

    Sub Main()
        Dim kcf As KCF = "G:\KEGG.molDraw\NADPH.txt".LoadKCF
        Call kcf.GetJson.__DEBUG_ECHO 

        Pause 
    End Sub
End Module
