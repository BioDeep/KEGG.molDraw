Imports System.ComponentModel
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Serialization.JSON
Imports SMRUCC.genomics.Assembly.KEGG.DBGET.bGetObject
Imports cpdBriet = SMRUCC.genomics.Assembly.KEGG.DBGET.BriteHEntry.Compound
Imports kegMap = SMRUCC.genomics.Assembly.KEGG.WebServices.Downloader

Module CLI

    <ExportAPI("/draw.kcf")>
    <Usage("/draw.kcf /in <kcf.txt> [/out <out.png>]")>
    Public Function DrawKCF(args As CommandLine) As Integer
        Dim in$ = args <= "/in"
        Dim out$ = args.GetValue("/out", [in].TrimSuffix & ".png")
        Dim kcf As KCF = [in].LoadKCF

        Return kcf.Draw() _
            .Save(out) _
            .CLICode
    End Function

    <ExportAPI("/draw.kegg")>
    <Description("Drawing query data from KEGG dbget API.")>
    <Usage("/draw.kegg /cpd <kegg_compound_ID> [/out <out.DIR>]")>
    Public Function DrawKEGG(args As CommandLine) As Integer
        Dim cpd$ = args <= "/cpd"
        Dim out$ = args.GetValue("/out", App.CurrentDirectory)
        Dim KCF = Compound.DownloadKCF(cpd, out).LoadKCF

        Return KCF.Draw() _
            .Save(out) _
            .CLICode
    End Function

    <ExportAPI("/dump.kegg.compounds")>
    <Description("Dumping the KEGG compounds database")>
    <Usage("/dump.kegg.compounds [/max.cid <default=25000> /out <save_dir>]")>
    Public Function DumpKEGGCompounds(args As CommandLine) As Integer
        With args.GetValue("/out", App.CurrentDirectory & "/KEGG.compounds/")
            Return cpdBriet.DownloadFromResource(EXPORT:= .ref,
                                                 structInfo:=True,
                                                 maxID:=args.GetValue("/max.cid", 25000)) _
                .GetJson _
                .SaveTo(.ref & "/failures.json") _
                .CLICode
        End With
    End Function

    <ExportAPI("/dump.kegg.maps")>
    <Description("Dumping the KEGG maps database for human species.")>
    <Usage("/dump.kegg.maps [/out <save_dir>]")>
    Public Function DumpKEGGMaps(args As CommandLine) As Integer
        With args.GetValue("/out", App.CurrentDirectory & "/KEGG.pathwayMaps/")
            Dim tmp = App.GetAppSysTempFile(".txt")
            My.Resources.hsa00001.FlushStream(tmp)
            Return kegMap.Downloads(EXPORT:= .ref, briefFile:=tmp) _
                .GetJson _
                .SaveTo(.ref & "/failures.json") _
                .CLICode
        End With
    End Function
End Module
