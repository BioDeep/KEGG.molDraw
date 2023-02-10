#Region "Microsoft.VisualBasic::dd6196caa24f7d8a0e729bd094b15052, mzkit\src\visualize\KCF\KEGGdraw\kegDraw\ChemConteiner.vb"

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

    '   Total Lines: 72
    '    Code Lines: 52
    ' Comment Lines: 0
    '   Blank Lines: 20
    '     File Size: 3.13 KB


    '     Class ChemConteiner
    ' 
    '         Function: getObject, moleculeRange, (+2 Overloads) nearAtom, (+2 Overloads) nearBond, nearChemObject
    '                   objectNum, refine
    ' 
    '         Sub: register, rescale, selectAllItems, (+4 Overloads) selectItems
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Collections

Namespace keg.compound


    Public Class ChemConteiner : Inherits ChemObject

        Friend objects As New ArrayList()

        Public Overridable Function objectNum() As Integer
            Return Me.objects.Count
        End Function

        Public Overridable Function getObject(paramInt As Integer) As ChemObject
            Return DirectCast(Me.objects(paramInt), ChemObject)
        End Function

        Public Overridable Sub register(paramEditMode As EditMode)
        End Sub

        Public Overridable Sub selectItems(paramInt1 As Integer, paramInt2 As Integer, paramInt3 As Integer, paramInt4 As Integer, paramDouble As Double, paramEditMode As EditMode,
            paramBoolean As Boolean)
            selectItems(paramInt1, paramInt2, paramInt3, paramInt4, paramDouble, paramEditMode,
                paramBoolean, False)
        End Sub

        Public Overridable Sub selectItems(paramInt1 As Integer, paramInt2 As Integer, paramInt3 As Integer, paramInt4 As Integer, paramDouble As Double, paramEditMode As EditMode,
            paramBoolean1 As Boolean, paramBoolean2 As Boolean)
        End Sub

        Public Overridable Sub selectItems(paramVector As ArrayList, paramDouble As Double, paramEditMode As EditMode, paramBoolean As Boolean)
            selectItems(paramVector, paramDouble, paramEditMode, paramBoolean, False)
        End Sub

        Public Overridable Sub selectItems(paramVector As ArrayList, paramDouble As Double, paramEditMode As EditMode, paramBoolean1 As Boolean, paramBoolean2 As Boolean)
        End Sub

        Public Overridable Sub selectAllItems(paramEditMode As EditMode, paramBoolean As Boolean)
        End Sub

        Public Overridable Function refine() As Boolean
            Return True
        End Function

        Public Overridable Sub rescale(paramDouble As Double)
        End Sub

        Public Overridable Function nearAtom(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double, paramInt3 As Integer) As Atom
            Return DirectCast(Nothing, Atom)
        End Function

        Public Overridable Function nearAtom(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double, paramInt3 As Integer, paramBoolean As Boolean) As Atom
            Return DirectCast(Nothing, Atom)
        End Function

        Public Overridable Function nearBond(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double, paramInt3 As Integer) As Bond
            Return DirectCast(Nothing, Bond)
        End Function

        Public Overridable Function nearBond(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double, paramInt3 As Integer, paramBoolean As Boolean) As Bond
            Return DirectCast(Nothing, Bond)
        End Function

        Public Overridable Function nearChemObject(paramInt1 As Integer, paramInt2 As Integer, paramInt3 As Integer) As ChemObject
            Return DirectCast(Nothing, ChemObject)
        End Function

        Public Overridable Function moleculeRange() As Double()
            Return DirectCast(Nothing, Double())
        End Function
    End Class
End Namespace
