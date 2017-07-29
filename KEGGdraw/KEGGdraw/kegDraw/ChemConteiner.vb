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
