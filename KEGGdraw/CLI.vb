Imports System.ComponentModel
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.InteropService.SharedORM
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language.UnixBash
Imports SMRUCC.Chemistry.Model

<CLI> Module CLI

    <ExportAPI("/draw.kcf")>
    <Usage("/draw.kcf /in <kcf.txt> [/out <out.png>]")>
    <Description("Draw image from KCF model data file.")>
    <Group(Groups.KCF_tools)>
    Public Function DrawKCF(args As CommandLine) As Integer
        Dim in$ = args <= "/in"

        If [in].DirectoryExists Then
            Dim EXPORT$ = args("/out") Or "./"

            For Each file As String In ls - l - r - {"*.txt", "*.kcf"} <= [in]
                Dim kcf As KCF = [in].LoadKCF
                Dim out$ = EXPORT & "/" & file.BaseName & ".png"

                Call kcf.Draw() _
                    .Save(out) _
                    .CLICode
            Next

            Return 0
        Else
            Dim out$ = args("/out") Or $"{[in].TrimSuffix}.png"
            Dim kcf As KCF = [in].LoadKCF

            Return kcf.Draw() _
                .Save(out) _
                .CLICode
        End If
    End Function

    '<ExportAPI("/draw.kegg")>
    '<Description("Drawing query data from KEGG dbget API.")>
    '<Usage("/draw.kegg /cpd <kegg_compound_ID> [/out <out.DIR>]")>
    '<Group(Groups.KCF_tools)>
    'Public Function DrawKEGG(args As CommandLine) As Integer
    '    Dim cpd$ = args <= "/cpd"
    '    Dim out$ = args.GetValue("/out", App.CurrentDirectory)
    '    Dim KCF = Compound.DownloadKCF(cpd, out).LoadKCF

    '    Return KCF.Draw() _
    '        .Save(out) _
    '        .CLICode
    'End Function

    '<ExportAPI("/dump.kegg.compounds")>
    '<Description("Dumping the KEGG compounds database")>
    '<Usage("/dump.kegg.compounds [/max.cid <default=25000> /out <save_dir>]")>
    '<Group(Groups.KegDatabase)>
    'Public Function DumpKEGGCompounds(args As CommandLine) As Integer
    '    With args.GetValue("/out", App.CurrentDirectory & "/KEGG.compounds/")
    '        Call cpdBriet.WorkspaceCleanup(DIR:= .ref)
    '        Return cpdBriet.DownloadFromResource(EXPORT:= .ref,
    '                                             structInfo:=True,
    '                                             maxID:=args.GetValue("/max.cid", 25000)) _
    '            .GetJson _
    '            .SaveTo(.ref & "/failures.json") _
    '            .CLICode
    '    End With
    'End Function

    '<ExportAPI("/dump.kegg.maps")>
    '<Description("Dumping the KEGG maps database for human species.")>
    '<Usage("/dump.kegg.maps [/out <save_dir>]")>
    '<Group(Groups.KegDatabase)>
    'Public Function DumpKEGGMaps(args As CommandLine) As Integer
    '    With args.GetValue("/out", App.CurrentDirectory & "/KEGG.pathwayMaps/")
    '        Dim tmp = App.GetAppSysTempFile(".txt")
    '        Call My.Resources.hsa00001.FlushStream(tmp)
    '        Return kegMap.Downloads(EXPORT:= .ref, briefFile:=tmp) _
    '            .GetJson _
    '            .SaveTo(.ref & "/failures.json") _
    '            .CLICode
    '    End With
    'End Function
End Module
