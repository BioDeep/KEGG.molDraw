#Region "Microsoft.VisualBasic::29f67f43db484bb43837cb45e057077f, KCF\KEGGdraw\kegDraw\Reaction.vb"

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

    ' 	Class Reaction
    ' 
    ' 	    Properties: OverlapedBracket, Parent, ReactionArrow, Texts, Title
    ' 
    ' 	    Function: bracketNum, (+2 Overloads) compareBracket, contain, getBound, getBracket
    '                getBracketNo, getCategory, getObjectNo, InlineAssignHelper, isNoCategory
    '                isProduct, isReactant, mergeMol, (+2 Overloads) nearAtom, (+2 Overloads) nearBond
    '                nearBracket, nearBracketLabel, nearChemObject, numNoCategory, numProduct
    '                numReactant, onText, refine, ToString
    ' 
    ' 	    Sub: (+2 Overloads) [select], add, addBracket, addObject, addText
    '           (+2 Overloads) autoSetCategory, calcImplicitHydrogen, checkOverlapedBracket, checkOverlapedBracketWithRefine, clearOverlapedBracket
    '           decisideHydrogenDraw, delAllObject, delBracket, delObject, flipHorizontal
    '           flipHorizontalIfSelected, flipVertical, flipVerticalIfSelected, lockOfCheckRing, move
    '           moveInternal, refineBracket, register, removeText, rescale
    '           reset0point, select_reverse, selectAllItems, (+2 Overloads) selectItems, setAtomsIntoBracketRectangle
    '           (+2 Overloads) setCategory, setDBond, setNoCategoryAll, unlockOfCheckRing, unselect
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Collections

Namespace keg.compound


	Public Class Reaction
		Inherits ChemConteiner
		Public Const __NoCategory As Integer = 0
		Public Const __Reactant As Integer = 1
		Public Const __Product As Integer = 2
		Private position As New ArrayList()
		Private brackets As New ArrayList()
		Private m_parent As ChemConteiner = Nothing
		Private m_title As String = Nothing
		Private _texts As New ArrayList()

		Public Overridable Sub addObject(paramChemObject As ChemObject, paramInt As Integer)
			Me.objects.Add(paramChemObject)
			Me.position.Add(New System.Nullable(Of Integer)(paramInt))
			If (TypeOf paramChemObject Is Molecule) Then
				DirectCast(paramChemObject, Molecule).Parent = Me
			End If
		End Sub

		Public Overridable Sub delObject(paramChemObject As ChemObject)
			Dim i As Integer = getObjectNo(paramChemObject)
			If i > -1 Then
				Me.objects.RemoveAt(i)
				Me.position.RemoveAt(i)
			End If
		End Sub

		Public Overridable Sub addBracket(paramBracket As Bracket)
			If Not Me.brackets.Contains(paramBracket) Then
				Me.brackets.Add(paramBracket)
			End If
		End Sub

		Public Overridable Sub delBracket(paramBracket As Bracket)
			Dim i As Integer = getBracketNo(paramBracket) - 1
			If i < 0 Then
				Return
			End If
			Me.brackets.Remove(paramBracket)
			Dim arrayOfMolecule As Molecule() = paramBracket.Mol
			For j As Integer = 0 To arrayOfMolecule.Length - 1
				arrayOfMolecule(j).removeBracket(paramBracket)
			Next
		End Sub

		Public Overridable Function bracketNum() As Integer
			Return Me.brackets.Count
		End Function

		Public Overridable Function getBracket(paramInt As Integer) As Bracket
			If (paramInt < 0) OrElse (paramInt >= Me.brackets.Count) Then
				Return Nothing
			End If
			Return DirectCast(Me.brackets(paramInt), Bracket)
		End Function

		Public Overridable Property Parent() As ChemConteiner
			Get
				Return Me.m_parent
			End Get
			Set
				Me.m_parent = value
			End Set
		End Property


		Public Overridable Property Title() As String
			Get
				Return Me.m_title
			End Get
			Set
				Me.m_title = value
			End Set
		End Property


		Public Overridable Function numNoCategory() As Integer
			Dim i As Integer = 0
			For j As Integer = 0 To Me.position.Count - 1
				If isNoCategory(j) Then
					i += 1
				End If
			Next
			Return i
		End Function

		Public Overridable Function numReactant() As Integer
			Dim i As Integer = 0
			For j As Integer = 0 To Me.position.Count - 1
				If isReactant(j) Then
					i += 1
				End If
			Next
			Return i
		End Function

		Public Overridable Function numProduct() As Integer
			Dim i As Integer = 0
			For j As Integer = 0 To Me.position.Count - 1
				If isProduct(j) Then
					i += 1
				End If
			Next
			Return i
		End Function

		Public Overridable Function isNoCategory(paramInt As Integer) As Boolean
			Return CInt(CType(Me.position(paramInt), System.Nullable(Of Integer))) = 0
		End Function

		Public Overridable Function isReactant(paramInt As Integer) As Boolean
			Return CInt(CType(Me.position(paramInt), System.Nullable(Of Integer))) = 1
		End Function

		Public Overridable Function isProduct(paramInt As Integer) As Boolean
			Return CInt(CType(Me.position(paramInt), System.Nullable(Of Integer))) = 2
		End Function

		Public Overridable Function getCategory(paramInt As Integer) As Integer
			Return CInt(CType(Me.position(paramInt), System.Nullable(Of Integer)))
		End Function

		Public Overridable Sub setCategory(paramInt1 As Integer, paramInt2 As Integer)
			Me.position(paramInt1) = New System.Nullable(Of Integer)(paramInt2)
		End Sub

		Public Overridable Sub setCategory(paramChemObject As ChemObject, paramInt As Integer)
			setCategory(getObjectNo(paramChemObject), paramInt)
		End Sub

		Public Overridable Sub setNoCategoryAll()
			For i As Integer = 0 To Me.position.Count - 1
				setCategory(i, 0)
			Next
		End Sub

		Public Overridable Function getObjectNo(paramChemObject As ChemObject) As Integer
			Return Me.objects.IndexOf(paramChemObject)
		End Function

		Public Overridable Function getBracketNo(paramBracket As Bracket) As Integer
			Return Me.brackets.IndexOf(paramBracket) + 1
		End Function

		Public Overridable ReadOnly Property ReactionArrow() As ReactionArrow
			Get
				For i As Integer = 0 To Me.objects.Count - 1
					If (TypeOf Me.objects(i) Is ReactionArrow) Then
						Return DirectCast(Me.objects(i), ReactionArrow)
					End If
				Next
				Return Nothing
			End Get
		End Property

		Public Overridable Overloads Function ToString() As String
			Return "undefine"
		End Function

		Public Overrides Sub [select](paramBoolean As Boolean)
			Dim i As Integer = Me.objects.Count
			For j As Integer = 0 To i - 1
				If (TypeOf Me.objects(j) Is ChemConteiner) Then
					DirectCast(Me.objects(j), ChemConteiner).[select](paramBoolean)
				End If
				If (TypeOf Me.objects(j) Is ReactionArrow) Then
					DirectCast(Me.objects(j), ReactionArrow).[select](paramBoolean)
				End If
			Next
			i = Me.brackets.Count
			For j = 0 To i - 1
				DirectCast(Me.brackets(j), Bracket).[select](paramBoolean)
			Next
		End Sub

		Public Overrides Sub [select]()
			[select](True)
		End Sub

		Public Overrides Sub unselect()
			[select](False)
		End Sub

		Public Overrides Sub select_reverse()
			Dim i As Integer = Me.objects.Count
			For j As Integer = 0 To i - 1
				If (TypeOf Me.objects(j) Is ChemConteiner) Then
					DirectCast(Me.objects(j), ChemConteiner).select_reverse()
				End If
				If (TypeOf Me.objects(j) Is ReactionArrow) Then
					DirectCast(Me.objects(j), ReactionArrow).select_reverse()
				End If
			Next
			i = Me.brackets.Count
			For j = 0 To i - 1
				DirectCast(Me.brackets(j), Bracket).select_reverse()
			Next
		End Sub

		Public Overrides Sub register(paramEditMode As EditMode)
			Dim i As Integer = Me.objects.Count
			For j As Integer = 0 To i - 1
				If (TypeOf Me.objects(j) Is ChemConteiner) Then
					DirectCast(Me.objects(j), ChemConteiner).register(paramEditMode)
				End If
			Next
			i = Me.brackets.Count
			For j = 0 To i - 1
				If DirectCast(Me.brackets(j), Bracket).[Select] Then
					paramEditMode.selected.Add(Me.brackets(j))
				End If
			Next
		End Sub

		Public Overrides Sub selectItems(paramInt1 As Integer, paramInt2 As Integer, paramInt3 As Integer, paramInt4 As Integer, paramDouble As Double, paramEditMode As EditMode, _
			paramBoolean1 As Boolean, paramBoolean2 As Boolean)
			Dim localBracket As Bracket = Nothing
			Dim n As Integer = Me.objects.Count
			Dim m As Integer
			Dim k As Integer
			Dim j As Integer
			Dim i As Integer = InlineAssignHelper(j, InlineAssignHelper(k, InlineAssignHelper(m, 0)))
			For i1 As Integer = 0 To n - 1
				If (TypeOf Me.objects(i1) Is ChemConteiner) Then
					DirectCast(Me.objects(i1), ChemConteiner).selectItems(paramInt1, paramInt2, paramInt3, paramInt4, paramDouble, paramEditMode, _
						paramBoolean1, paramBoolean2)
				End If
				If (TypeOf Me.objects(i1) Is ReactionArrow) Then
					DirectCast(Me.objects(i1), ReactionArrow).selectItems(paramInt1, paramInt2, paramInt3, paramInt4, paramEditMode, paramBoolean1)
				End If
			Next
			n = Me.brackets.Count
			For i1 = 0 To n - 1
				localBracket = DirectCast(Me.brackets(i1), Bracket)
				i = localBracket.DX(paramDouble, 0, 0)
				k = localBracket.DX(paramDouble, 0, 0) + localBracket.size().width
				j = localBracket.DY(paramDouble, 0, 0) - localBracket.size().height
				m = localBracket.DY(paramDouble, 0, 0)
				For i2 As Integer = 1 To 2
					For i3 As Integer = 1 To 4
						If localBracket.DX(paramDouble, i2, i3) < i Then
							i = localBracket.DX(paramDouble, i2, i3)
						End If
						If localBracket.DX(paramDouble, i2, i3) > k Then
							k = localBracket.DX(paramDouble, i2, i3)
						End If
						If localBracket.DY(paramDouble, i2, i3) < j Then
							j = localBracket.DY(paramDouble, i2, i3)
						End If
						If localBracket.DY(paramDouble, i2, i3) > m Then
							m = localBracket.DY(paramDouble, i2, i3)
						End If
					Next
				Next
				If (i >= paramInt1) AndAlso (k <= paramInt3) AndAlso (j >= paramInt2) AndAlso (m <= paramInt4) Then
					If paramBoolean1 Then
						DirectCast(Me.brackets(i1), Bracket).select_reverse(paramEditMode)
					Else
						DirectCast(Me.brackets(i1), Bracket).[select](paramEditMode)
					End If
				End If
			Next
		End Sub

		Public Overrides Sub selectItems(paramVector As ArrayList, paramDouble As Double, paramEditMode As EditMode, paramBoolean1 As Boolean, paramBoolean2 As Boolean)
			Dim localBracket As Bracket = Nothing
			Dim i As Integer = Me.objects.Count
			For j As Integer = 0 To i - 1
				If (TypeOf Me.objects(j) Is ChemConteiner) Then
					DirectCast(Me.objects(j), ChemConteiner).selectItems(paramVector, paramDouble, paramEditMode, paramBoolean1, paramBoolean2)
				End If
				If (TypeOf Me.objects(j) Is ReactionArrow) Then
					DirectCast(Me.objects(j), ReactionArrow).selectItems(paramVector, paramEditMode, paramBoolean1)
				End If
			Next
			Dim arrayOfInt1 As Integer() = New Integer(paramVector.Count - 1) {}
			Dim arrayOfInt2 As Integer() = New Integer(paramVector.Count - 1) {}
			For k As Integer = 0 To paramVector.Count - 1
				Dim localDimension As DblRect = DirectCast(paramVector(k), DblRect)
				arrayOfInt1(k) = localDimension.width
				arrayOfInt2(k) = localDimension.height
			Next
			Dim localPolygon As New java.awt.Polygon(arrayOfInt1, arrayOfInt2, paramVector.Count)
			i = Me.brackets.Count
			For m As Integer = 0 To i - 1
				Dim n As Integer = 1
				localBracket = DirectCast(Me.brackets(m), Bracket)
				For i1 As Integer = 1 To 2
					For i2 As Integer = 1 To 4
						If Not localPolygon.inside(localBracket.DX(paramDouble, i1, i2), localBracket.DY(paramDouble, i1, i2)) Then
							n = 0
							GoTo label289
						End If
					Next
				Next
				label289:
				If n <> 0 Then
					If paramBoolean1 Then
						DirectCast(Me.brackets(m), Bracket).select_reverse(paramEditMode)
					Else
						DirectCast(Me.brackets(m), Bracket).[select](paramEditMode)
					End If
				End If
			Next
		End Sub

		Public Overrides Sub selectAllItems(paramEditMode As EditMode, paramBoolean As Boolean)
			Dim i As Integer = Me.objects.Count
			For j As Integer = 0 To i - 1
				If (TypeOf Me.objects(j) Is ChemConteiner) Then
					DirectCast(Me.objects(j), ChemConteiner).selectAllItems(paramEditMode, paramBoolean)
				End If
				If (TypeOf Me.objects(j) Is ReactionArrow) Then
					DirectCast(Me.objects(j), ReactionArrow).[select](paramEditMode)
				End If
			Next
			i = Me.brackets.Count
			For j = 0 To i - 1
				If (TypeOf Me.brackets(j) Is Bracket) Then
					DirectCast(Me.brackets(j), Bracket).selectAll(paramEditMode)
				End If
			Next
		End Sub

		Public Overridable Function mergeMol(paramDouble As Double) As Molecule
			Dim localMolecule As Molecule = Nothing
			For i As Integer = 0 To Me.objects.Count - 1
				If (TypeOf Me.objects(i) Is Molecule) Then
					If localMolecule Is Nothing Then
						localMolecule = DirectCast(Me.objects(i), Molecule).mergeMol(localMolecule, paramDouble)
					Else
						localMolecule = localMolecule.mergeMol(DirectCast(Me.objects(i), Molecule), paramDouble)
					End If
				End If
			Next
			Return localMolecule
		End Function

		Public Overrides Function refine() As Boolean
			Dim i As Integer = 0
			While i < Me.objects.Count
				If (TypeOf Me.objects(i) Is ChemConteiner) Then
					If DirectCast(Me.objects(i), ChemConteiner).refine() Then
						i += 1
					End If
				Else
					i += 1
				End If
			End While
			For i = 0 To objectNum() - 1
				If (TypeOf getObject(i) Is Molecule) Then
					Dim localMolecule As Molecule = DirectCast(getObject(i), Molecule)
					If contain(localMolecule) Then
						delObject(localMolecule)
						i -= 1
					End If
				End If
			Next
			Return True
		End Function

		Public Overrides Sub rescale(paramDouble As Double)
			Dim i As Integer = Me.objects.Count
			For j As Integer = 0 To i - 1
				If (TypeOf Me.objects(j) Is ChemConteiner) Then
					DirectCast(Me.objects(j), ChemConteiner).rescale(paramDouble)
				End If
			Next
		End Sub

		Public Overrides Function nearAtom(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double, paramInt3 As Integer) As Atom
			Return nearAtom(paramInt1, paramInt2, paramDouble, paramInt3, False)
		End Function

		Public Overrides Function nearAtom(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double, paramInt3 As Integer, paramBoolean As Boolean) As Atom
			Dim localAtom As Atom = Nothing
			For i As Integer = 0 To Me.objects.Count - 1
				If (TypeOf Me.objects(i) Is ChemConteiner) Then
					localAtom = DirectCast(Me.objects(i), ChemConteiner).nearAtom(paramInt1, paramInt2, paramDouble, paramInt3, paramBoolean)
					If localAtom IsNot Nothing Then
						Return localAtom
					End If
				End If
			Next
			Return Nothing
		End Function

		Public Overrides Function nearBond(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double, paramInt3 As Integer) As Bond
			Return nearBond(paramInt1, paramInt2, paramDouble, paramInt3, False)
		End Function

		Public Overrides Function nearBond(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double, paramInt3 As Integer, paramBoolean As Boolean) As Bond
			Dim localBond As Bond = Nothing
			For i As Integer = 0 To Me.objects.Count - 1
				If (TypeOf Me.objects(i) Is ChemConteiner) Then
					localBond = DirectCast(Me.objects(i), ChemConteiner).nearBond(paramInt1, paramInt2, paramDouble, paramInt3, paramBoolean)
					If localBond IsNot Nothing Then
						Return localBond
					End If
				End If
			Next
			Return Nothing
		End Function

		Public Overrides Function nearChemObject(paramInt1 As Integer, paramInt2 As Integer, paramInt3 As Integer) As ChemObject
			Dim localReactionArrow As ReactionArrow = Nothing
			For i As Integer = 0 To Me.objects.Count - 1
				If (TypeOf Me.objects(i) Is ReactionArrow) Then
					localReactionArrow = DirectCast(Me.objects(i), ReactionArrow).nearReactionArrow(paramInt1, paramInt2, paramInt3)
				End If
				If localReactionArrow IsNot Nothing Then
					Return localReactionArrow
				End If
			Next
			Return Nothing
		End Function

		Public Overridable Function nearBracket(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double, paramInt3 As Integer) As Bracket
			Dim localBracket As Bracket = Nothing
			Dim i As Integer = Me.brackets.Count
			For j As Integer = 0 To Me.brackets.Count - 1
				If (TypeOf Me.brackets(j) Is Bracket) Then
					localBracket = DirectCast(Me.brackets(j), Bracket).nearBracket(paramInt1, paramInt2, paramDouble, paramInt3)
					If localBracket IsNot Nothing Then
						Return localBracket
					End If
				End If
			Next
			Return Nothing
		End Function

		Public Overridable Function nearBracketLabel(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double, paramInt3 As Integer) As Bracket
			Dim localBracket As Bracket = Nothing
			Dim i As Integer = Me.brackets.Count
			For j As Integer = 0 To Me.brackets.Count - 1
				If (TypeOf Me.brackets(j) Is Bracket) Then
					localBracket = DirectCast(Me.brackets(j), Bracket).nearBracketLabel(paramInt1, paramInt2, paramDouble, paramInt3)
					If localBracket IsNot Nothing Then
						Return localBracket
					End If
				End If
			Next
			Return Nothing
		End Function

		Public Overridable WriteOnly Property OverlapedBracket() As Double
			Set
				Dim i As Integer = Me.objects.Count
				For j As Integer = 0 To i - 1
					If (TypeOf Me.objects(j) Is Molecule) Then
						Dim localMolecule As Molecule = DirectCast(Me.objects(j), Molecule)
					End If
				Next
			End Set
		End Property

		Public Overridable Sub clearOverlapedBracket()
		End Sub

		Public Overridable Sub calcImplicitHydrogen()
			Dim i As Integer = Me.objects.Count
			For j As Integer = 0 To i - 1
				If (TypeOf Me.objects(j) Is Molecule) Then
					Dim localMolecule As Molecule = DirectCast(Me.objects(j), Molecule)
					localMolecule.calcImplicitHydrogen()
				End If
			Next
		End Sub

		Public Overridable Sub decisideHydrogenDraw()
			Dim i As Integer = Me.objects.Count
			For j As Integer = 0 To i - 1
				If (TypeOf Me.objects(j) Is Molecule) Then
					Dim localMolecule As Molecule = DirectCast(Me.objects(j), Molecule)
					localMolecule.decisideHydrogenDraw()
				End If
			Next
		End Sub

		Public Overridable Sub setDBond()
			Dim i As Integer = Me.objects.Count
			For j As Integer = 0 To i - 1
				If (TypeOf Me.objects(j) Is Molecule) Then
					Dim localMolecule As Molecule = DirectCast(Me.objects(j), Molecule)
					localMolecule.setDBond()
				End If
			Next
		End Sub

		Public Overridable Sub autoSetCategory(paramDouble As Double)
			Dim arrayOfInt As Integer() = New Integer(3) {}
			Dim localReactionArrow As ReactionArrow = ReactionArrow
			Dim i3 As Integer
			If localReactionArrow Is Nothing Then
				For i3 = 0 To Me.objects.Count - 1
					setCategory(i3, 0)
				Next
			Else
				Dim k As Integer = localReactionArrow.Direction
				Dim m As Integer = localReactionArrow.DX1()
				Dim n As Integer = localReactionArrow.DY1()
				Dim i1 As Integer = localReactionArrow.DX2()
				Dim i2 As Integer = localReactionArrow.DY2()
				For i3 = 0 To Me.objects.Count - 1
					If (TypeOf Me.objects(i3) Is Molecule) Then
						Dim localMolecule As Molecule = DirectCast(Me.objects(i3), Molecule)
						arrayOfInt = localMolecule.moleculeRange(paramDouble)
						Dim i As Integer = (arrayOfInt(0) + arrayOfInt(2)) \ 2
						Dim j As Integer = (arrayOfInt(1) + arrayOfInt(3)) \ 2
						Select Case k
							Case 0
								If i < m Then
									setCategory(i3, 1)
								ElseIf i > i1 Then
									setCategory(i3, 2)
								Else
									setCategory(i3, 0)
								End If
								
							Case 1
								If j > n Then
									setCategory(i3, 1)
								ElseIf j < i2 Then
									setCategory(i3, 2)
								Else
									setCategory(i3, 0)
								End If
								
							Case 2
								If i > m Then
									setCategory(i3, 1)
								ElseIf i < i1 Then
									setCategory(i3, 2)
								Else
									setCategory(i3, 0)
								End If
								
							Case 3
								If j < n Then
									setCategory(i3, 1)
								ElseIf j > i2 Then
									setCategory(i3, 2)
								Else
									setCategory(i3, 0)
								End If
								
						End Select
					End If
				Next
			End If
		End Sub

		Public Overridable Sub autoSetCategory(paramMolecule As Molecule, paramDouble As Double)
			Dim arrayOfInt As Integer() = New Integer(3) {}
			If getObjectNo(paramMolecule) < 0 Then
				Return
			End If
			Dim localReactionArrow As ReactionArrow = ReactionArrow
			If localReactionArrow Is Nothing Then
				For i3 As Integer = 0 To Me.objects.Count - 1
					setCategory(paramMolecule, 0)
				Next
			Else
				Dim k As Integer = localReactionArrow.Direction
				Dim m As Integer = localReactionArrow.DX1()
				Dim n As Integer = localReactionArrow.DY1()
				Dim i1 As Integer = localReactionArrow.DX2()
				Dim i2 As Integer = localReactionArrow.DY2()
				arrayOfInt = paramMolecule.moleculeRange(paramDouble)
				Dim i As Integer = (arrayOfInt(0) + arrayOfInt(2)) \ 2
				Dim j As Integer = (arrayOfInt(1) + arrayOfInt(3)) \ 2
				Select Case k
					Case 0
						If i < m Then
							setCategory(paramMolecule, 1)
						ElseIf i > i1 Then
							setCategory(paramMolecule, 2)
						Else
							setCategory(paramMolecule, 0)
						End If
						
					Case 1
						If j > n Then
							setCategory(paramMolecule, 1)
						ElseIf j < i1 Then
							setCategory(paramMolecule, 2)
						Else
							setCategory(paramMolecule, 0)
						End If
						
					Case 2
						If i > m Then
							setCategory(paramMolecule, 1)
						ElseIf i < i1 Then
							setCategory(paramMolecule, 2)
						Else
							setCategory(paramMolecule, 0)
						End If
						
					Case 3
						If j < n Then
							setCategory(paramMolecule, 1)
						ElseIf j > i1 Then
							setCategory(paramMolecule, 2)
						Else
							setCategory(paramMolecule, 0)
						End If
						
				End Select
			End If
		End Sub

		Public Overridable Sub reset0point(paramDouble As Double)
			For i As Integer = 0 To objectNum() - 1
				If (TypeOf getObject(i) Is Molecule) Then
					Dim localMolecule As Molecule = DirectCast(getObject(i), Molecule)
					localMolecule.resetOpoint(paramDouble)
				End If
			Next
		End Sub

		Public Overridable Sub add(paramReaction As Reaction)
			For i As Integer = 0 To paramReaction.objectNum() - 1
				If paramReaction.getObject(i) IsNot Nothing Then
					addObject(paramReaction.getObject(i), 0)
				End If
			Next
			For i = 0 To paramReaction.bracketNum() - 1
				If paramReaction.getBracket(i) IsNot Nothing Then
					addBracket(paramReaction.getBracket(i))
				End If
			Next
		End Sub

		Public Overridable Function getBound(paramDouble As Double) As Rectangle
			Dim localRectangle As New Rectangle()
			If (objectNum() = 0) AndAlso (bracketNum() = 0) Then
				Return localRectangle
			End If
			Dim i As Integer = Integer.MaxValue
			Dim j As Integer = Integer.MinValue
			Dim k As Integer = Integer.MaxValue
			Dim m As Integer = Integer.MinValue
			Dim n As Integer = Integer.MaxValue
			Dim i1 As Integer = Integer.MinValue
			Dim i2 As Integer = Integer.MaxValue
			Dim i3 As Integer = Integer.MinValue
			Dim localObject1 As Object
			Dim i6 As Integer
			For i4 As Integer = 0 To objectNum() - 1
				localObject1 = getObject(i4)
				Dim localObject2 As Object
				If (TypeOf localObject1 Is Molecule) Then
					localObject2 = DirectCast(localObject1, Molecule).moleculeRange(paramDouble)
					n = localObject2(0)
					i2 = localObject2(1)
					i1 = localObject2(2)
					i3 = localObject2(3)
				Else
					If (TypeOf localObject1 Is Bracket) Then
						localObject2 = DirectCast(localObject1, Bracket)
						n = DirectCast(localObject2, Bracket).DX(paramDouble, 0, 0)
						i1 = DirectCast(localObject2, Bracket).DX(paramDouble, 0, 0) + DirectCast(localObject2, Bracket).size().width + 3
						i2 = DirectCast(localObject2, Bracket).DY(paramDouble, 0, 0) - DirectCast(localObject2, Bracket).size().height
						i3 = DirectCast(localObject2, Bracket).DY(paramDouble, 0, 0)
						For i6 = 1 To 2
							For i7 As Integer = 1 To 4
								If DirectCast(localObject2, Bracket).DX(paramDouble, i6, i7) < n Then
									n = DirectCast(localObject2, Bracket).DX(paramDouble, i6, i7)
								End If
								If DirectCast(localObject2, Bracket).DX(paramDouble, i6, i7) > i1 Then
									i1 = DirectCast(localObject2, Bracket).DX(paramDouble, i6, i7)
								End If
								If DirectCast(localObject2, Bracket).DY(paramDouble, i6, i7) < i2 Then
									i2 = DirectCast(localObject2, Bracket).DY(paramDouble, i6, i7)
								End If
								If DirectCast(localObject2, Bracket).DY(paramDouble, i6, i7) > i3 Then
									i3 = DirectCast(localObject2, Bracket).DY(paramDouble, i6, i7)
								End If
							Next
						Next
						Exit For
					End If
					If (TypeOf localObject1 Is ReactionArrow) Then
						localObject2 = DirectCast(localObject1, ReactionArrow)
						n = Math.Min(DirectCast(localObject2, ReactionArrow).DX1(), DirectCast(localObject2, ReactionArrow).DX2())
						i1 = Math.Max(DirectCast(localObject2, ReactionArrow).DX1(), DirectCast(localObject2, ReactionArrow).DX2())
						i2 = Math.Min(DirectCast(localObject2, ReactionArrow).DY1(), DirectCast(localObject2, ReactionArrow).DY2())
						i3 = Math.Max(DirectCast(localObject2, ReactionArrow).DY1(), DirectCast(localObject2, ReactionArrow).DY2())
						Exit For
					End If
				End If
				i = Math.Min(n, i)
				k = Math.Min(i2, k)
				j = Math.Max(i1, j)
				m = Math.Max(i3, m)
			Next
			For i4 = 0 To bracketNum() - 1
				localObject1 = getBracket(i4)
				n = DirectCast(localObject1, Bracket).DX(paramDouble, 0, 0)
				i1 = DirectCast(localObject1, Bracket).DX(paramDouble, 0, 0) + DirectCast(localObject1, Bracket).size().width + 3
				i2 = DirectCast(localObject1, Bracket).DY(paramDouble, 0, 0) - DirectCast(localObject1, Bracket).size().height
				i3 = DirectCast(localObject1, Bracket).DY(paramDouble, 0, 0)
				For i5 As Integer = 1 To 2
					For i6 = 1 To 4
						If DirectCast(localObject1, Bracket).DX(paramDouble, i5, i6) < n Then
							n = DirectCast(localObject1, Bracket).DX(paramDouble, i5, i6)
						End If
						If DirectCast(localObject1, Bracket).DX(paramDouble, i5, i6) > i1 Then
							i1 = DirectCast(localObject1, Bracket).DX(paramDouble, i5, i6)
						End If
						If DirectCast(localObject1, Bracket).DY(paramDouble, i5, i6) < i2 Then
							i2 = DirectCast(localObject1, Bracket).DY(paramDouble, i5, i6)
						End If
						If DirectCast(localObject1, Bracket).DY(paramDouble, i5, i6) > i3 Then
							i3 = DirectCast(localObject1, Bracket).DY(paramDouble, i5, i6)
						End If
					Next
				Next
				i = Math.Min(n, i)
				k = Math.Min(i2, k)
				j = Math.Max(i1, j)
				m = Math.Max(i3, m)
			Next
			localRectangle.x = i
			localRectangle.y = k
			localRectangle.width = (j - localRectangle.x)
			localRectangle.height = (m - localRectangle.y)
			Return localRectangle
		End Function

		Public Overrides Sub move(paramInt1 As Integer, paramInt2 As Integer)
			MyBase.move(paramInt1, paramInt2)
			Dim localObject As Object
			For i As Integer = 0 To objectNum() - 1
				localObject = getObject(i)
				DirectCast(localObject, ChemObject).move(paramInt1, paramInt2)
			Next
			For i = 0 To bracketNum() - 1
				localObject = getBracket(i)
				DirectCast(localObject, Bracket).move(paramInt1, paramInt2)
			Next
		End Sub

		Public Overrides Sub flipHorizontal(paramDouble1 As Double, paramDouble2 As Double)
			For i As Integer = 0 To objectNum() - 1
				Dim localChemObject As ChemObject = getObject(i)
				localChemObject.flipHorizontal(paramDouble1, paramDouble2)
			Next
		End Sub

		Public Overridable Sub flipHorizontalIfSelected(paramDouble1 As Double, paramDouble2 As Double)
			Dim localObject As Object
			For i As Integer = 0 To objectNum() - 1
				localObject = getObject(i)
				If (TypeOf localObject Is Molecule) Then
					DirectCast(localObject, Molecule).flipHorizontalIfSelected(paramDouble1, paramDouble2)
				End If
			Next
			For i = 0 To bracketNum() - 1
				localObject = getBracket(i)
				If DirectCast(localObject, Bracket).[Select] Then
					DirectCast(localObject, Bracket).flipHorizontal(paramDouble1, paramDouble2)
				End If
			Next
		End Sub

		Public Overrides Sub flipVertical(paramDouble1 As Double, paramDouble2 As Double)
			Dim localObject As Object
			For i As Integer = 0 To objectNum() - 1
				localObject = getObject(i)
				DirectCast(localObject, ChemObject).flipVertical(paramDouble1, paramDouble2)
			Next
			For i = 0 To bracketNum() - 1
				localObject = getBracket(i)
				DirectCast(localObject, Bracket).flipVertical(paramDouble1, paramDouble2)
			Next
		End Sub

		Public Overridable Sub flipVerticalIfSelected(paramDouble1 As Double, paramDouble2 As Double)
			Dim localObject As Object
			For i As Integer = 0 To objectNum() - 1
				localObject = getObject(i)
				If (TypeOf localObject Is Molecule) Then
					DirectCast(localObject, Molecule).flipVerticalIfSelected(paramDouble1, paramDouble2)
				End If
			Next
			For i = 0 To bracketNum() - 1
				localObject = getBracket(i)
				If DirectCast(localObject, Bracket).[Select] Then
					DirectCast(localObject, Bracket).flipVertical(paramDouble1, paramDouble2)
				End If
			Next
		End Sub

		Public Overridable Sub moveInternal(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double)
			For i As Integer = 0 To objectNum() - 1
				If (TypeOf getObject(i) Is Molecule) Then
					DirectCast(getObject(i), Molecule).moveInternal(paramInt1, paramInt2, paramDouble)
				ElseIf Not (TypeOf getObject(i) Is ReactionArrow) Then
				End If
			Next
			For i = 0 To bracketNum() - 1
				getBracket(i).moveInternal(paramInt1, paramInt2, paramDouble)
			Next
		End Sub

		Public Overridable Sub checkOverlapedBracket()
			For i As Integer = 0 To bracketNum() - 1
				Dim localBracket As Bracket = getBracket(i)
				Dim j As Integer = 1
				For k As Integer = 0 To objectNum() - 1
					If (TypeOf getObject(k) Is Molecule) Then
						Dim localMolecule As Molecule = DirectCast(getObject(k), Molecule)
						If localMolecule.confirmeIntersectionOfBondsAndBracketLine(localBracket) Then
							localMolecule.setSgroup()
							j = 0
						Else
							localMolecule.removeBracket(localBracket)
						End If
					End If
				Next
				If j = 1 Then
					setAtomsIntoBracketRectangle(localBracket, Nothing)
				End If
			Next
		End Sub

		Public Overridable Sub setAtomsIntoBracketRectangle(paramBracket As Bracket, paramMolecule As Molecule)
			For i As Integer = 0 To objectNum() - 1
				If (TypeOf getObject(i) Is Molecule) Then
					Dim localMolecule As Molecule = DirectCast(getObject(i), Molecule)
					For j As Integer = 0 To localMolecule.AtomNum - 1
						Dim localAtom As Atom = localMolecule.getAtom(j + 1)
						If (paramBracket.contains(localAtom) = True) AndAlso ((paramMolecule Is Nothing) OrElse ((paramMolecule IsNot Nothing) AndAlso (Not paramBracket.Sgroup.Contains(localAtom)))) Then
							Dim localVector As ArrayList = localMolecule.getAtomsList(localAtom)
							If localVector.Count > 0 Then
								paramBracket.addMol(localMolecule)
								paramBracket.addSgroup(localVector)
							End If
						End If
					Next
				End If
			Next
		End Sub

		Public Overridable Sub checkOverlapedBracketWithRefine()
			checkOverlapedBracket()
			refine()
		End Sub

		Public Overridable Function contain(paramMolecule As Molecule) As Boolean
			For i As Integer = 0 To objectNum() - 1
				If (TypeOf getObject(i) Is Molecule) Then
					Dim localMolecule As Molecule = DirectCast(getObject(i), Molecule)
					If localMolecule.AtomNum = paramMolecule.AtomNum Then
						Dim localAtom1 As Atom = localMolecule.getAtom(1)
						Dim localAtom2 As Atom = paramMolecule.getAtom(1)
						If (localAtom1.x = localAtom2.x) AndAlso (localAtom1.y = localAtom2.y) AndAlso (Not localMolecule.Equals(paramMolecule)) Then
							Return True
						End If
					End If
				End If
			Next
			Return False
		End Function

		Public Overridable Sub refineBracket()
			Dim localObject As Object
			Dim j As Integer
			Dim localBracket1 As Bracket
			For i As Integer = 0 To Me.brackets.Count - 2
				localObject = DirectCast(Me.brackets(i), Bracket)
				For j = i + 1 To Me.brackets.Count - 1
					localBracket1 = DirectCast(Me.brackets(j), Bracket)
					If compareBracket(DirectCast(localObject, Bracket), localBracket1, True) = True Then
						Dim arrayOfMolecule As Molecule() = localBracket1.Mol
						For m As Integer = 0 To arrayOfMolecule.Length - 1
							arrayOfMolecule(m).addBracket(DirectCast(localObject, Bracket))
						Next
						delBracket(localBracket1)
						j -= 1
					End If
				Next
			Next
			For i = 0 To objectNum() - 1
				If (TypeOf getObject(i) Is Molecule) Then
					localObject = DirectCast(getObject(i), Molecule)
					For j = 0 To DirectCast(localObject, Molecule).BracketNum - 2
						localBracket1 = DirectCast(localObject, Molecule).getBracket(j + 1)
						If Not Me.brackets.Contains(localBracket1) Then
							DirectCast(localObject, Molecule).removeBracket(localBracket1)
						Else
							For k As Integer = j + 1 To DirectCast(localObject, Molecule).BracketNum - 1
								Dim localBracket2 As Bracket = DirectCast(localObject, Molecule).getBracket(k + 1)
								If Not Me.brackets.Contains(localBracket2) Then
									DirectCast(localObject, Molecule).removeBracket(localBracket2)
								ElseIf compareBracket(localBracket1, localBracket2, True) = True Then
									If Not Me.brackets.Contains(localBracket1) Then
										DirectCast(localObject, Molecule).removeBracket(localBracket1)
									ElseIf Not Me.brackets.Contains(localBracket2) Then
										DirectCast(localObject, Molecule).removeBracket(localBracket2)
									Else
										DirectCast(localObject, Molecule).removeBracket(localBracket2)
									End If
								End If
							Next
						End If
					Next
				End If
			Next
		End Sub

		Public Shared Function compareBracket(paramBracket1 As Bracket, paramBracket2 As Bracket) As Boolean
			Return compareBracket(paramBracket1, paramBracket2, False)
		End Function

		Public Shared Function compareBracket(paramBracket1 As Bracket, paramBracket2 As Bracket, paramBoolean As Boolean) As Boolean
			If (Not paramBoolean) AndAlso (paramBracket1.CoveredWholeMol <> paramBracket2.CoveredWholeMol) Then
				Return False
			End If
			Dim localRectangle1 As Rectangle = paramBracket1.getRect(1.0)
			Dim localRectangle2 As Rectangle = paramBracket2.getRect(1.0)
			Return (localRectangle1.x = localRectangle2.x) AndAlso (localRectangle1.y = localRectangle2.y) AndAlso (localRectangle1.width = localRectangle2.width) AndAlso (localRectangle1.height = localRectangle2.height)
		End Function

		Public Overridable Sub lockOfCheckRing()
			For i As Integer = 0 To objectNum() - 1
				If (TypeOf getObject(i) Is Molecule) Then
					Dim localMolecule As Molecule = DirectCast(getObject(i), Molecule)
					localMolecule.lockOfCheckRing()
				End If
			Next
		End Sub

		Public Overridable Sub unlockOfCheckRing()
			For i As Integer = 0 To objectNum() - 1
				If (TypeOf getObject(i) Is Molecule) Then
					Dim localMolecule As Molecule = DirectCast(getObject(i), Molecule)
					localMolecule.unlockOfCheckRing()
				End If
			Next
		End Sub

		Public Overridable Function onText(paramPoint As java.awt.Point) As Text
			For i As Integer = 0 To Me._texts.Count - 1
				Dim localText As Text = DirectCast(Me._texts(i), Text)
				If localText.contains(paramPoint) = True Then
					Return localText
				End If
			Next
			Return Nothing
		End Function

		Public Overridable ReadOnly Property Texts() As ArrayList
			Get
				Return Me._texts
			End Get
		End Property

		Public Overridable Sub addText(paramText As Text)
			Me._texts.Add(paramText)
		End Sub

		Public Overridable Sub removeText(paramText As Text)
			Me._texts.Remove(paramText)
		End Sub

		Public Overridable Sub delAllObject()
			Me.objects.Clear()
			Me.position.Clear()
			Me.brackets.Clear()
		End Sub
		Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
			target = value
			Return value
		End Function
	End Class


	' Location:              C:\Users\xieguigang\Downloads\KegDraw-0_1_14Beta\lib\KegDraw.jar!\keg\compound\Reaction.class
'	 * Java compiler version: 6 (50.0)
'	 * JD-Core Version:       0.7.1
'	 

End Namespace
