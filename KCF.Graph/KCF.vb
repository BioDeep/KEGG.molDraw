#Region "Microsoft.VisualBasic::cccfe22e4aea3215afdd476872a49876, KCF\KCF.Graph\KCF.vb"

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

    ' Module KCF
    ' 
    '     Function: Graph
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports Microsoft.VisualBasic.Language

''' <summary>
''' The KCF network graph extension
''' </summary>
''' 
<HideModuleName> Public Module KCF

    ''' <summary>
    ''' Create network graph model from KCF molecule model
    ''' </summary>
    ''' <param name="KCF"></param>
    ''' <returns></returns>
    <Extension>
    Public Function CreateGraph(KCF As Model.KCF) As NetworkGraph
        Dim g As New NetworkGraph
        Dim node As Node
        Dim point As FDGVector2

        For Each atom As Atom In KCF.Atoms
            point = New FDGVector2(atom.Atom2D_coordinates.X, atom.Atom2D_coordinates.Y)
            node = New Node With {
                .ID = atom.Index,
                .Label = $"#{atom.Index}",
                .data = New NodeData With {
                    .label = atom.KEGGAtom.code,
                    .initialPostion = point
                }
            }

            Call g.AddNode(node)
        Next

        ' key by node.label
        Dim nodes As Dictionary(Of Node) = g.nodes.ToDictionary()
        Dim a, b As String
        Dim edge As Edge
        Dim length#
        Dim node1, node2 As Node
        Dim label$

        For Each bound As Bound In KCF.Bounds
            a = "#" & bound.from
            b = "#" & bound.to
            node1 = nodes(a)
            node2 = nodes(b)
            length = Imaging.Math2D.Distance(
                node1.data.initialPostion.Point2D,
                node2.data.initialPostion.Point2D)

            label = $"{a} -> {b}" Or
                    $"{a} -> {b} ({bound.dimentional_levels})".When(Not bound.dimentional_levels.StringEmpty)

            edge = New Edge With {
                .U = node1,
                .V = node2,
                .ID = label,
                .data = New EdgeData With {
                    .weight = bound.bounds,
                    .length = length,
                    .label = label
                }
            }

            Call g.AddEdge(edge)
        Next

        Return g
    End Function
End Module
