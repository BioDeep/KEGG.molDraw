Imports Microsoft.VisualBasic.ComponentModel.Algorithm.BinaryTree
Imports SMRUCC.Chemistry
Imports SMRUCC.Chemistry.Model
Imports SMRUCC.Chemistry.Model.Graph

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
