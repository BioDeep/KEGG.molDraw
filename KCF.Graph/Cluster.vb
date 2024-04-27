#Region "Microsoft.VisualBasic::c219fda0c04346f04317f7e2b08db4c6, G:/mzkit/src/visualize/KCF/KCF.Graph//Cluster.vb"

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

    '   Total Lines: 75
    '    Code Lines: 55
    ' Comment Lines: 8
    '   Blank Lines: 12
    '     File Size: 2.80 KB


    ' Module Cluster
    ' 
    '     Function: BinaryTree, createVectorTemplate, KCFAtomVector
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.Algorithm.BinaryTree
Imports Microsoft.VisualBasic.ComponentModel.Algorithm.DynamicProgramming
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Language.Default
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.LinearAlgebra

''' <summary>
''' Cluster of the chemical compounds based on the binary tree graph
''' </summary>
Public Module Cluster

    ReadOnly atoms$() = KegAtomType.KEGGAtomTypes _
        .Values _
        .IteratesALL _
        .Select(Function(a) a.code) _
        .Distinct _
        .ToArray

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Private Function createVectorTemplate() As Dictionary(Of String, Counter)
        Return atoms.ToDictionary(Function(k) k, Function() New Counter)
    End Function

    <Extension> Public Function KCFAtomVector(KCF As Model.KCF) As Vector
        Dim template = createVectorTemplate()

        For Each atom In KCF.Atoms
            Call template(atom.KEGGAtom.code).Hit()
        Next

        Return template.AsNumeric.Takes(atoms).AsVector
    End Function

    ReadOnly KCFAtomVectorCos As New [Default](Of ISimilarity(Of Vector))(Function(a, b) SSM(a, b))
    ReadOnly GetKCFAtomVector As New [Default](Of Func(Of Model.KCF, Vector))(AddressOf KCFAtomVector)

    ''' <summary>
    ''' 利用二叉树对KCF模型之间的相似度进行聚类操作
    ''' </summary>
    ''' <param name="compounds"></param>
    ''' <returns></returns>
    <Extension>
    Public Function BinaryTree(compounds As IEnumerable(Of Model.KCF),
                               Optional asVector As Func(Of Model.KCF, Vector) = Nothing,
                               Optional similarity As ISimilarity(Of Vector) = Nothing) As BinaryTree(Of Vector, Model.KCF)

        With similarity Or KCFAtomVectorCos
            Dim tree As New AVLTree(Of Vector, Model.KCF)(
                Function(a, b)
                    Dim cos# = .ByRef(a, b)

                    If cos >= 0.95 Then
                        Return 0
                    ElseIf cos < 0.8 Then
                        Return 1
                    Else
                        Return -1
                    End If
                End Function)

            With asVector Or GetKCFAtomVector
                For Each compound As Model.KCF In compounds
                    Call tree.Add(.ByRef(compound), compound, valueReplace:=False)
                Next
            End With

            Return tree.root
        End With
    End Function
End Module
