#Region "Microsoft.VisualBasic::e5924c4cf78e725d89522a9ad67b7982, E:/mzkit/src/visualize/KCF/KEGGdraw//CLI.vb"

    ' Author:
    ' 
    '       xieguigang (gg.xie@bionovogene.com, BioNovoGene Co., LTD.)
    ' 
    ' Copyright (c) 2018 gg.xie@bionovogene.com, BioNovoGene Co., LTD.
    ' 
    ' 
    ' MIT License
    ' 
    ' 
    ' Permission is hereby granted, free of charge, to any person obtaining a copy
    ' of this software and associated documentation files (the "Software"), to deal
    ' in the Software without restriction, including without limitation the rights
    ' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    ' copies of the Software, and to permit persons to whom the Software is
    ' furnished to do so, subject to the following conditions:
    ' 
    ' The above copyright notice and this permission notice shall be included in all
    ' copies or substantial portions of the Software.
    ' 
    ' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    ' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    ' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    ' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    ' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    ' SOFTWARE.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 79
    '    Code Lines: 67
    ' Comment Lines: 1
    '   Blank Lines: 11
    '     File Size: 3.07 KB


    ' Module CLI
    ' 
    '     Function: DrawKCF, TransCode
    ' 
    ' /********************************************************************************/

#End Region

Imports System.ComponentModel
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports BioNovoGene.BioDeep.Chemistry.Model
Imports BioNovoGene.BioDeep.Chemistry.Model.Drawing
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.InteropService.SharedORM
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.BitmapImage
Imports Microsoft.VisualBasic.Language.UnixBash

<CLI> Module CLI

    <ExportAPI("/draw.kcf")>
    <Usage("/draw.kcf /in <kcf.txt/directory> [/transparent /corp /out <out.png/directory>]")>
    <Description("Draw image from KCF model data file.")>
    <Argument("/in", False, CLITypes.File, PipelineTypes.std_in,
              AcceptTypes:={GetType(String)},
              Extensions:="*.txt",
              Description:="If this parameter is missing, then this drawer program will accept data from standard input.")>
    <Argument("/out", True, CLITypes.File,
              Extensions:="*.png",
              Description:="The output file location, or a directory path if in batch mode.")>
    <Group(Groups.KCF_tools)>
    Public Function DrawKCF(args As CommandLine) As Integer
        Dim in$ = args <= "/in"
        Dim isTransparent As Boolean = args("/transparent")
        Dim corpBlank As Boolean = args("/corp")

        If [in].StringEmpty Then
            ' 没有/in参数, 则从标准输入之中得到绘图数据
            Dim KCF = args.ReadInput("/in").LoadKCF(throwEx:=False)

            If KCF Is Nothing Then
                Return -1
            Else
                Dim out$ = $"./{KCF.Entry.Id}.png"
                Return KCF _
                    .TransCode(out, isTransparent, corpBlank) _
                    .CLICode
            End If
        ElseIf [in].DirectoryExists Then
            Dim EXPORT$ = args("/out") Or $"{[in].TrimDIR}.images/"

            For Each file As String In ls - l - r - {"*.txt", "*.kcf"} <= [in]
                Dim out$ = EXPORT & "/" & file.BaseName & ".png"

                Call file.LoadKCF.TransCode(out, isTransparent, corpBlank)
                Call out.GetFullPath.ToFileURL.__DEBUG_ECHO
            Next

            Return 0
        Else
            Dim out$ = args("/out") Or $"{[in].TrimSuffix}.png"

            Return [in] _
                .LoadKCF _
                .TransCode(out, isTransparent, corpBlank) _
                .CLICode
        End If
    End Function

    <Extension>
    Public Function TransCode(kcf As KCF, out$, isTransparent As Boolean, corpBlank As Boolean) As Boolean
        Dim image As Image = kcf.Draw.AsGDIImage

        If corpBlank Then
            image = image.CorpBlank(blankColor:=Color.White)
        End If
        If isTransparent Then
            image = image.ColorReplace(Color.White, Color.Transparent)
        End If

        Return image _
            .SaveAs(out, ImageFormats.Png) _
            .CLICode
    End Function
End Module
