Imports Microsoft.VisualBasic.Serialization.JSON

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

        Dim kcf As KCF = "../../../../NADPH.txt".LoadKCF
        Call kcf.GetJson.__DEBUG_ECHO
        Call kcf.Draw().Save("../../../../NADPH.png")



        Pause()
    End Sub
End Module