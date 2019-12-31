#Region "Microsoft.VisualBasic::0ab0d4b36ad8e97bb95af4c2e8b31223, visual\KCF\KCF.Graph\test\Module1.vb"

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
'     Sub: Main, moleculeWeightTest
' 
' /********************************************************************************/

#End Region

Imports BioNovoGene.BioDeep.Chemistry
Imports BioNovoGene.BioDeep.Chemistry.Model

Module Module1

    Sub Main()

        Call moleculeWeightTest()

        Dim tree = "D:\KEGG-compounds\OtherUnknowns".ScanDirectory _
                                                    .BinaryTree _
                                                    .PopulateNodes _
                                                    .OrderByDescending(Function(cluster) DirectCast(cluster!values, List(Of Model.KCF)).Count) _
                                                    .ToArray

        For Each cluster In tree
            Dim members = DirectCast(cluster!values, List(Of Model.KCF))

            Call members.GetXml.SaveTo($"./{cluster.Value.Entry.Id}.Xml")

            Dim dir = $"./{cluster.Value.Entry.Id}/"

            For Each member In members
                Call $"D:\KEGG-compounds\OtherUnknowns\{member.Entry.Id}.gif".FileCopy(dir & $"/{member.Entry.Id}.gif")
            Next
        Next

        Pause()
    End Sub

    Sub moleculeWeightTest()
        Dim data = AtomicWeight.GetTable

        Pause()
    End Sub
End Module
