#Region "Microsoft.VisualBasic::a77bac65a465104828b17d34aa7bc2a3, visualize\KCF\KCF.Graph\KCF.vb"

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

    '   Total Lines: 135
    '    Code Lines: 99 (73.33%)
    ' Comment Lines: 18 (13.33%)
    '    - Xml Docs: 72.22%
    ' 
    '   Blank Lines: 18 (13.33%)
    '     File Size: 4.62 KB


    ' Module KCFGraph
    ' 
    '     Function: (+2 Overloads) atomMass, CreateGraph
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Data.visualize.Network.FileStream.Generic
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Language
Imports r = System.Text.RegularExpressions.Regex

''' <summary>
''' The KCF network graph extension
''' </summary>
''' 
<HideModuleName> Public Module KCFGraph

    <Extension>
    Private Function atomMass(atom As Atom) As Double
        Dim formula = atom.KEGGAtom.formula

        formula = formula.Replace("ring-", "").Replace("-ring", "")
        formula = formula.Replace("R-", "").Replace("-R", "")
        formula = formula.Replace("()", "")
        formula = formula.Trim("-"c)

        Dim atoms = r.Matches(formula, "[A-Z][a-z]*\d*", RegexOptions.Singleline).ToArray
        Dim mass As Double = atoms.Select(AddressOf atomMass).Sum

        Return mass
    End Function

    ReadOnly atomWeights As Dictionary(Of String, AtomicWeight) = AtomicWeight.GetTable

    Private Function atomMass(atoms As String) As Double
        Dim n As Integer = Val(r.Match(atoms, "\d+").Value)

        If n > 0 Then
            atoms = atoms.Replace(n.ToString, "")
        Else
            ' 当n=1的时候是被忽略掉的
            n = 1
        End If

        If Not atomWeights.ContainsKey(atoms) Then
            Return -n
        Else
            Return atomWeights(atoms).Mass * n
        End If
    End Function

    ''' <summary>
    ''' Create network graph model from KCF molecule model
    ''' </summary>
    ''' <param name="KCF"></param>
    ''' <returns>
    ''' Some important data in this generated network graph model:
    ''' 
    ''' + bounds: edge.data.weight
    ''' + atom code: node.data.label
    ''' + bound length: edge.data.length
    ''' 
    ''' </returns>
    <Extension>
    Public Function CreateGraph(KCF As Model.KCF) As NetworkGraph
        Dim g As New NetworkGraph
        Dim node As Node
        Dim point As FDGVector2

        For Each atom As Atom In KCF.Atoms
            point = New FDGVector2(atom.Atom2D_coordinates.X, atom.Atom2D_coordinates.Y)
            node = New Node With {
                .ID = atom.Index,
                .label = $"#{atom.Index}",
                .data = New NodeData With {
                    .label = atom.KEGGAtom.code,
                    .initialPostion = point,
                    .Properties = New Dictionary(Of String, String) From {
                        {"charge", 0},
                        {"atom", atom.Atom},
                        {"atom_type", atom.KEGGAtom.type.Description},
                        {NamesOf.REFLECTION_ID_MAPPING_NODETYPE, atom.KEGGAtom.code}
                    },
                    .mass = atomMass(atom.Atom)
                }
            }

            Call g.AddNode(node, assignId:=False)
        Next

#If DEBUG Then
        Call $"Total mass of {KCF.Entry} is {Aggregate atom In g.vertex Into Sum(atom.data.mass)}".debug
#End If

        ' key by node.label
        Dim nodes As Dictionary(Of Node) = g.vertex.ToDictionary()
        Dim a, b As String
        Dim edge As Edge
        Dim length#
        Dim node1, node2 As Node
        Dim label$
        Dim x, y As PointF
        Dim hasLevel As Boolean

        For Each bound As Bound In KCF.Bounds
            a = "#" & bound.from
            b = "#" & bound.to
            node1 = nodes(a)
            node2 = nodes(b)
            x = node1.data.initialPostion.Point2D
            y = node2.data.initialPostion.Point2D
            length = Math2D.Distance(x, y)
            hasLevel = Not bound.dimentional_levels.StringEmpty
            label = $"{a} -> {b}" Or $"{a} -> {b} ({bound.dimentional_levels})".When(hasLevel)

            edge = New Edge With {
                .U = node1,
                .V = node2,
                .ID = label,
                .weight = bound.bounds,
                .data = New EdgeData With {
                    .length = length,
                    .label = label,
                    .Properties = New Dictionary(Of String, String) From {
                        {"bounds", bound.bounds}
                    }
                }
            }

            Call g.AddEdge(edge)
        Next

        Return g
    End Function
End Module
