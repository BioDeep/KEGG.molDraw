#Region "Microsoft.VisualBasic::dc16eeea172ae28f6ddab11a55d71a69, KCF\KEGGdraw\CLI.vb"

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

    ' Module CLI
    ' 
    '     Function: DrawKCF, TransCode
    ' 
    ' /********************************************************************************/

#End Region

Imports System.ComponentModel
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.InteropService.SharedORM
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.BitmapImage
Imports Microsoft.VisualBasic.Language.UnixBash
Imports SMRUCC.Chemistry.Model

<CLI> Module CLI

    <ExportAPI("/draw.kcf")>
    <Usage("/draw.kcf /in <kcf.txt> [/transparent /corp /out <out.png>]")>
    <Description("Draw image from KCF model data file.")>
    <Group(Groups.KCF_tools)>
    Public Function DrawKCF(args As CommandLine) As Integer
        Dim in$ = args <= "/in"
        Dim isTransparent As Boolean = args("/transparent")
        Dim corpBlank As Boolean = args("/corp")

        If [in].DirectoryExists Then
            Dim EXPORT$ = args("/out") Or $"{[in].TrimDIR}.images/"

            For Each file As String In ls - l - r - {"*.txt", "*.kcf"} <= [in]
                Dim out$ = EXPORT & "/" & file.BaseName & ".png"

                Call file.TransCode(out, isTransparent, corpBlank)
                Call out.GetFullPath.ToFileURL.__DEBUG_ECHO
            Next

            Return 0
        Else
            Dim out$ = args("/out") Or $"{[in].TrimSuffix}.png"
            Return [in].TransCode(out, isTransparent, corpBlank)
        End If
    End Function

    <Extension>
    Public Function TransCode(in$, out$, isTransparent As Boolean, corpBlank As Boolean) As Boolean
        Dim kcf As KCF = [in].LoadKCF
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

