Imports Microsoft.VisualBasic.Serialization.JSON

Module Module1

    Sub Main()

        Dim c As New Microsoft.VisualBasic.Imaging.SVG.XML.rect With {.attributes = New Dictionary(Of String, String) From {{"123", "ddd"}}}

        Call c.GetXml.__DEBUG_ECHO

        Pause()


        Dim kcf As KCF = "G:\KEGG.molDraw\NADPH.txt".LoadKCF
        Call kcf.GetJson.__DEBUG_ECHO
        Call kcf.Draw().Save("./test.png")

        Pause()
    End Sub
End Module