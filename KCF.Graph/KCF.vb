
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports Microsoft.VisualBasic.Language

''' <summary>
''' The KCF network graph extension
''' </summary>
Public Module KCF

    <Extension>
    Public Function Graph(KCF As Global.KCF.IO.KCF) As NetworkGraph
        Dim g As New NetworkGraph
        Dim node As Node
        Dim point As FDGVector2

        For Each atom In KCF.Atoms
            point = New FDGVector2(atom.Atom2D_coordinates.X, atom.Atom2D_coordinates.Y)
            node = New Node With {
                .ID = "#" & atom.Index,
                .Data = New NodeData With {
                    .label = atom.KEGGAtom,
                    .initialPostion = point
                }
            }

            Call g.AddNode(node)
        Next

        Dim nodes As Dictionary(Of Node) = g.nodes.ToDictionary()
        Dim a, b As String
        Dim edge As Edge

        For Each bound In KCF.Bounds
            a = "#" & bound.from
            b = "#" & bound.to
            edge = New Edge With {
                .Source = nodes(a),
                .Target = nodes(b),
                .ID = $"{a} -> {b}" Or $"{a} -> {b} ({bound.dimentional_levels})".AsDefault(Function() Not bound.dimentional_levels.StringEmpty)
            }

            Call g.AddEdge(edge)
        Next

        Return g
    End Function
End Module
