Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Serialization.JSON
Imports SMRUCC.genomics.Assembly.KEGG.WebServices

Module Program

    Public Function Main() As Integer
        Return GetType(CLI).RunCLI(App.CommandLine)
    End Function

    Sub New()
#If DEBUG Then
        Call test()
#End If
    End Sub

    Sub test()


        Dim reder As LocalRender = LocalRender.FromRepository("G:\KEGG.molDraw\App\KEGG.pathwayMaps")

        Call reder.Rendering("http://www.genome.jp/kegg-bin/show_pathway?hsa05034/hsa:3014%09red/hsa:8970%09red/hsa:3845%09red/hsa:3013%09red").SaveAs("G:\KEGG\App\test.png")
        Call reder.Rendering("http://www.genome.jp/kegg-bin/show_pathway?hsa00010/C00103%09red/C00267%09red/C00197%09red/C00022%09red").SaveAs("G:\KEGG\App\test2.png")

        Pause()
        Dim kcf As KCF = "../DATA/NADPH.txt".LoadKCF
        Call kcf.GetJson.__DEBUG_ECHO
        Call kcf.Draw().Save("../DATA/NADPH.png")



        Pause()
    End Sub
End Module