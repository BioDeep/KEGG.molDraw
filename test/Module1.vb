#Region "Microsoft.VisualBasic::33b73d505c54cdcbe4117d6c94c2335b, KCF\test\Module1.vb"

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

    ' Module Module1
    ' 
    '     Sub: drawTest, layerSourceTest, Main
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports KEGGdraw
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.BitmapImage
Imports Microsoft.VisualBasic.Serialization.JSON
Imports SMRUCC.Chemistry.Model
Imports SMRUCC.Chemistry.Model.Graph

Module Module1

    Sub Main()

        '  Call layerSourceTest()
        Call drawTest()

        Dim list = KegAtomType.KEGGAtomTypes.GetJson
        Dim KCF = IO.LoadKCF("G:\MassSpectrum-toolkits\KCF\DATA\NADPH.txt")
        Dim g = KCF.Graph


        Pause()
    End Sub

    Sub drawTest()

        Dim KCF = IO.LoadKCF("../DATA/NADPH.txt")
        Call KCF.Draw().Save("../DATA/NADPH.png")

        Pause()


        KCF = IO.LoadKCF("../DATA/L-Citrulline.txt")
        Call KCF.Draw().Save("../DATA/L-Citrulline.png")

        KCF = IO.LoadKCF("../DATA/Tetrahydrofolate.txt")
        Call KCF.Draw().Save("../DATA/Tetrahydrofolate.png")

        KCF = IO.LoadKCF("../DATA/CoA.txt")
        Call KCF.Draw().Save("../DATA/CoA.png")

        Pause()
    End Sub

    Sub layerSourceTest()
        Dim KCF = IO.LoadKCF("../DATA/Tetrahydrofolate.txt")
        Dim image = KCF.Draw().AsGDIImage

        Dim tr = image.ColorReplace(Color.White, Color.Transparent)

        Call tr.SaveAs("./ttttt.png")

        Call tr.ResizeUnscaledByHeight(500).SaveAs("./fdffsfsd.png")

        Pause()
    End Sub
End Module

