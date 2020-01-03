#Region "Microsoft.VisualBasic::fadb24e1524c1fc760e7adb58318a366, src\visualize\KCF\KEGGdraw\kegDraw\EditMode.vb"

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

    '     Class EditMode
    ' 
    '         Properties: ChangeArea, ChangeItems, Clipboard, Scale, ShrinkMode
    '                     UNDO, UndoOperation, UNDOREODMODE, UNDOreserve
    ' 
    '         Function: (+3 Overloads) copy, exportG, getRectangleAboutSelectedArea, hasClipboard, InlineAssignHelper
    '                   isCollision, popREDOUnit, popUNDOUnit, popUnit, redoOK
    '                   topREDOUnit, topUNDOUnit, topUnit, undoOK
    ' 
    '         Sub: clear, clearUNDO, duplicate, getClipboard, pushREDOUnit
    '              pushUNDOUnit, resetArea, sendClipboard
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Collections
Imports System.Collections.Generic

Namespace keg.compound


    Public Class EditMode
        Public atom_label As String = ""
        Public operation As Integer = 1
        Public charge As Integer = 0
        Public old_charge As Integer = 0
        Public category As Integer = 0
        Public draw As Integer = 0
        Public status As Integer = 0
        Public changeAtom As Integer
        Public click As Integer
        Public atom_count As Integer = 0
        Public bracket_count As Integer = 0
        Public save_file As String = ""
        Public flag_edit As Boolean = False
        Public flag_copy As Boolean = False
        Public hydrogen_draw As Boolean = False
        Public keggatomname_mode As Boolean = False
        Public Const UNDO_NONE As Integer = 0
        Public Const UNDO_MOVE As Integer = 1
        Public Const UNDO_RESIZE As Integer = 2
        Friend undo_OK As Boolean = False
        Public undo_action As Integer = 0
        Friend undo_reserve As New ArrayList()
        Public atom As Atom = Nothing
        Public bond As Bond = Nothing
        Public chemobject As ChemObject
        Public bracket As Bracket
        Public selected As New ArrayList()
        Public m_clipboard As New ArrayList()
        Friend clipsize As DblRect
        Public clipReaction As Reaction = Nothing
        Public select_area As New Rectangle()
        Public near_object As Object = Nothing
        Public select_mode As Integer = 0
        Friend dispscale As Double
        Friend diff As Integer = 8
        Public Const charge_PLUS As Integer = 1
        Public Const charge_NONE As Integer = 0
        Public Const charge_MINUS As Integer = -1
        Public Const operation_DRAW As Integer = 1
        Public Const operation_ERASE As Integer = 2
        Public Const operation_SELECT As Integer = 3
        Public Const operation_LABEL As Integer = 4
        Public Const operation_CHARGE As Integer = 5
        Public Const operation_ARROW As Integer = 6
        Public Const operation_BRACKET As Integer = 7
        Public Const operation_ROTATION As Integer = 8
        Public Const category_NONE As Integer = 0
        Public Const category_REACTANT As Integer = 1
        Public Const category_PRODUCT As Integer = 2
        Public Const draw_SINGLE As Integer = 0
        Public Const draw_DOUBLE As Integer = 1
        Public Const draw_TRIPLE As Integer = 2
        Public Const draw_UP As Integer = 3
        Public Const draw_DOWN As Integer = 4
        Public Const draw_EITHER As Integer = 5
        Public Const draw_4RING As Integer = 6
        Public Const draw_5RING As Integer = 7
        Public Const draw_6RING As Integer = 8
        Public Const draw_BENZENE As Integer = 9
        Public Const draw_CHAIN As Integer = 10
        Public Const draw_5RINGB As Integer = 11
        Public Const draw_5RINGBB As Integer = 12
        Public Const status_NORMAL As Integer = 0
        Public Const status_DRAW As Integer = 1
        Public Const status_SELECT As Integer = 2
        Public Const status_MOVE As Integer = 3
        Public Const status_ROTATE As Integer = 4
        Public Const status_RESIZE As Integer = 5
        Public Const status_LABEL As Integer = 6
        Public Const status_EREASE As Integer = 7
        Public Const status_ROTATION As Integer = 8
        Public Const select_LASSO As Integer = 0
        Public Const select_RECT As Integer = 1
        Friend flipflag As Boolean = True
        Private undostack As List(Of UndoUnit) = New ArrayList()
        Private redostack As List(Of UndoUnit) = New ArrayList()
        Private _isShrinkMode As Boolean = False
        Private undoredo_mode As Integer = 0
        Public Const [NOTHING] As Integer = 0
        Public Const UNDOING As Integer = 1
        Public Const REDOING As Integer = 2

        Public Overridable Property Scale() As Double
            Get
                Return Me.dispscale
            End Get
            Set
                Me.dispscale = Value
            End Set
        End Property


        Public Overridable Sub clear()
            While Me.selected.Count > 0
                If (TypeOf Me.selected(0) Is Atom) Then
                    DirectCast(Me.selected(0), Atom).[select](False, Me)
                ElseIf (TypeOf Me.selected(0) Is Bond) Then
                    DirectCast(Me.selected(0), Bond).[select](False, Me)
                Else
                    Me.selected.RemoveAt(0)
                End If
            End While
            Me.selected.Clear()
            Me.select_area.x = -1
            Me.select_area.y = -1
            Me.select_area.width = 0
            Me.select_area.height = 0
            Me.near_object = Nothing
            Me.atom_count = 0
        End Sub

        Public Overridable Sub resetArea()
            Me.atom_count = 0
            Me.bracket_count = 0
            Dim n As Integer = -1
            Me.select_area.x = -1
            Me.select_area.y = -1
            Me.select_area.width = 0
            Me.select_area.height = 0
            If Me.selected.Count > 0 Then
                Dim localObject As Object
                For i1 As Integer = 0 To Me.selected.Count - 1
                    localObject = Me.selected(i1)
                    If (TypeOf localObject Is Atom) Then
                        n = i1
                        Exit For
                    End If
                Next
                Dim localBracket As Bracket
                Dim i As Integer
                Dim j As Integer
                Dim k As Integer
                Dim m As Integer
                Dim i4 As Integer
                Dim localReactionArrow As ReactionArrow
                Dim localAtom As Atom
                If n < 0 Then
                    For i1 = 0 To Me.selected.Count - 1
                        localObject = Me.selected(i1)
                        If (TypeOf localObject Is Bracket) Then
                            localBracket = DirectCast(localObject, Bracket)
                            i = Integer.MaxValue
                            j = Integer.MaxValue
                            k = Integer.MinValue
                            m = Integer.MinValue
                            Dim i2 As Integer
                            If (localBracket.selectAll) OrElse (localBracket.SelectSide = 1) Then
                                i2 = 1
                                For i4 = 1 To 4
                                    If localBracket.DX(Me.dispscale, i2, i4) < i Then
                                        i = localBracket.DX(Me.dispscale, i2, i4)
                                    End If
                                    If localBracket.DX(Me.dispscale, i2, i4) > k Then
                                        k = localBracket.DX(Me.dispscale, i2, i4)
                                    End If
                                    If localBracket.DY(Me.dispscale, i2, i4) < j Then
                                        j = localBracket.DY(Me.dispscale, i2, i4)
                                    End If
                                    If localBracket.DY(Me.dispscale, i2, i4) > m Then
                                        m = localBracket.DY(Me.dispscale, i2, i4)
                                    End If
                                Next
                            End If
                            If (localBracket.selectAll) OrElse (localBracket.SelectSide = 2) Then
                                i2 = 2
                                For i4 = 1 To 4
                                    If localBracket.DX(Me.dispscale, i2, i4) < i Then
                                        i = localBracket.DX(Me.dispscale, i2, i4)
                                    End If
                                    If localBracket.DX(Me.dispscale, i2, i4) > k Then
                                        k = localBracket.DX(Me.dispscale, i2, i4)
                                    End If
                                    If localBracket.DY(Me.dispscale, i2, i4) < j Then
                                        j = localBracket.DY(Me.dispscale, i2, i4)
                                    End If
                                    If localBracket.DY(Me.dispscale, i2, i4) > m Then
                                        m = localBracket.DY(Me.dispscale, i2, i4)
                                    End If
                                Next
                            End If
                            Me.select_area.x = i
                            Me.select_area.y = j
                            Me.select_area.width = (k - Me.select_area.x)
                            Me.select_area.height = (m - Me.select_area.y)
                            n = i1
                            Exit For
                        End If
                        If (TypeOf localObject Is ReactionArrow) Then
                            localReactionArrow = DirectCast(localObject, ReactionArrow)
                            Me.select_area.x = localReactionArrow.DX1()
                            Me.select_area.y = localReactionArrow.DY1()
                            Me.select_area.width = 0
                            Me.select_area.height = 0
                            n = i1
                            Exit For
                        End If
                    Next
                    If n >= 0 Then
                    End If
                Else
                    localAtom = DirectCast(Me.selected(n), Atom)
                    Me.select_area.x = localAtom.DX(Me.dispscale)
                    Me.select_area.y = localAtom.DY(Me.dispscale)
                    Me.select_area.width = 0
                    Me.select_area.height = 0
                End If
                For i1 = 0 To Me.selected.Count - 1
                    If (TypeOf Me.selected(i1) Is Atom) Then
                        Me.atom_count += 1
                        localAtom = DirectCast(Me.selected(i1), Atom)
                        If Me.select_area.x > localAtom.DX(Me.dispscale) Then
                            Me.select_area.width += Me.select_area.x - localAtom.DX(Me.dispscale)
                            Me.select_area.x = localAtom.DX(Me.dispscale)
                        End If
                        If Me.select_area.y > localAtom.DY(Me.dispscale) Then
                            Me.select_area.height += Me.select_area.y - localAtom.DY(Me.dispscale)
                            Me.select_area.y = localAtom.DY(Me.dispscale)
                        End If
                        If Me.select_area.x + Me.select_area.width < localAtom.DX(Me.dispscale) Then
                            Me.select_area.width = (localAtom.DX(Me.dispscale) - Me.select_area.x)
                        End If
                        If Me.select_area.y + Me.select_area.height < localAtom.DY(Me.dispscale) Then
                            Me.select_area.height = (localAtom.DY(Me.dispscale) - Me.select_area.y)
                        End If
                    ElseIf (TypeOf Me.selected(i1) Is Bond) Then
                        Dim localBond As Bond = DirectCast(Me.selected(i1), Bond)
                        For i4 = 0 To 1
                            localAtom = If(i4 = 1, localBond.Atom1, localBond.Atom2)
                            If Me.select_area.x > localAtom.DX(Me.dispscale) Then
                                Me.select_area.width += Me.select_area.x - localAtom.DX(Me.dispscale)
                                Me.select_area.x = localAtom.DX(Me.dispscale)
                            End If
                            If Me.select_area.y > localAtom.DY(Me.dispscale) Then
                                Me.select_area.height += Me.select_area.y - localAtom.DY(Me.dispscale)
                                Me.select_area.y = localAtom.DY(Me.dispscale)
                            End If
                            If Me.select_area.x + Me.select_area.width < localAtom.DX(Me.dispscale) Then
                                Me.select_area.width = (localAtom.DX(Me.dispscale) - Me.select_area.x)
                            End If
                            If Me.select_area.y + Me.select_area.height < localAtom.DY(Me.dispscale) Then
                                Me.select_area.height = (localAtom.DY(Me.dispscale) - Me.select_area.y)
                            End If
                        Next
                    ElseIf (TypeOf Me.selected(i1) Is ReactionArrow) Then
                        localReactionArrow = DirectCast(Me.selected(i1), ReactionArrow)
                        If Me.select_area.x > localReactionArrow.DX1() Then
                            Me.select_area.width += Me.select_area.x - localReactionArrow.DX1()
                            Me.select_area.x = localReactionArrow.DX1()
                        End If
                        If Me.select_area.y > localReactionArrow.DY1() Then
                            Me.select_area.height += Me.select_area.y - localReactionArrow.DY1()
                            Me.select_area.y = localReactionArrow.DY1()
                        End If
                        If Me.select_area.x + Me.select_area.width < localReactionArrow.DX1() Then
                            Me.select_area.width = (localReactionArrow.DX1() - Me.select_area.x)
                        End If
                        If Me.select_area.y + Me.select_area.height < localReactionArrow.DY1() Then
                            Me.select_area.height = (localReactionArrow.DY1() - Me.select_area.y)
                        End If
                        If Me.select_area.x > localReactionArrow.DX2() Then
                            Me.select_area.width += Me.select_area.x - localReactionArrow.DX2()
                            Me.select_area.x = localReactionArrow.DX2()
                        End If
                        If Me.select_area.y > localReactionArrow.DY2() Then
                            Me.select_area.height += Me.select_area.y - localReactionArrow.DY2()
                            Me.select_area.y = localReactionArrow.DY2()
                        End If
                        If Me.select_area.x + Me.select_area.width < localReactionArrow.DX2() Then
                            Me.select_area.width = (localReactionArrow.DX2() - Me.select_area.x)
                        End If
                        If Me.select_area.y + Me.select_area.height < localReactionArrow.DY2() Then
                            Me.select_area.height = (localReactionArrow.DY2() - Me.select_area.y)
                        End If
                    ElseIf (TypeOf Me.selected(i1) Is Bracket) Then
                        Me.bracket_count += 1
                        localBracket = DirectCast(Me.selected(i1), Bracket)
                        i = Integer.MaxValue
                        j = Integer.MaxValue
                        k = Integer.MinValue
                        m = Integer.MinValue
                        Dim i3 As Integer
                        If (localBracket.selectAll) OrElse (localBracket.SelectSide = 1) Then
                            i3 = 1
                            For i4 = 1 To 4
                                If localBracket.DX(Me.dispscale, i3, i4) < i Then
                                    i = localBracket.DX(Me.dispscale, i3, i4)
                                End If
                                If localBracket.DX(Me.dispscale, i3, i4) > k Then
                                    k = localBracket.DX(Me.dispscale, i3, i4)
                                End If
                                If localBracket.DY(Me.dispscale, i3, i4) < j Then
                                    j = localBracket.DY(Me.dispscale, i3, i4)
                                End If
                                If localBracket.DY(Me.dispscale, i3, i4) > m Then
                                    m = localBracket.DY(Me.dispscale, i3, i4)
                                End If
                            Next
                        End If
                        If (localBracket.selectAll) OrElse (localBracket.SelectSide = 2) Then
                            i3 = 2
                            For i4 = 1 To 4
                                If localBracket.DX(Me.dispscale, i3, i4) < i Then
                                    i = localBracket.DX(Me.dispscale, i3, i4)
                                End If
                                If localBracket.DX(Me.dispscale, i3, i4) > k Then
                                    k = localBracket.DX(Me.dispscale, i3, i4)
                                End If
                                If localBracket.DY(Me.dispscale, i3, i4) < j Then
                                    j = localBracket.DY(Me.dispscale, i3, i4)
                                End If
                                If localBracket.DY(Me.dispscale, i3, i4) > m Then
                                    m = localBracket.DY(Me.dispscale, i3, i4)
                                End If
                            Next
                        End If
                        If Me.select_area.x > i Then
                            Me.select_area.width += Me.select_area.x - i
                            Me.select_area.x = i
                        End If
                        If Me.select_area.y > j Then
                            Me.select_area.height += Me.select_area.y - j
                            Me.select_area.y = j
                        End If
                        If k > Me.select_area.x + Me.select_area.width Then
                            Me.select_area.width = (k - Me.select_area.x)
                        End If
                        If m > Me.select_area.y + Me.select_area.height Then
                            Me.select_area.height = (m - Me.select_area.y)
                        End If
                    End If
                Next
            End If
        End Sub

        Public Overridable Function hasClipboard() As Boolean
            Dim localClipboard As java.awt.datatransfer.Clipboard = java.awt.Toolkit.DefaultToolkit.SystemClipboard
            Return ((Me.m_clipboard IsNot Nothing) AndAlso (Me.m_clipboard.Count > 0)) OrElse (localClipboard.isDataFlavorAvailable(keg.compound.gui.CompoundPanel.molFlavor))
        End Function

        Public Overridable Sub sendClipboard()
            If Me.selected.Count > 0 Then
                Dim localArrayList As New ArrayList()
                Dim i As Integer
                Dim localAtom1 As Atom
                Dim j As Integer
                If Not ShrinkMode Then
                    For i = 0 To Me.m_clipboard.Count - 1
                        If (TypeOf Me.m_clipboard(i) Is Atom) Then
                            localAtom1 = DirectCast(Me.m_clipboard(i), Atom)
                            If localAtom1.Express_group Then
                                Me.m_clipboard.Remove(localAtom1)
                                i -= 1
                            End If
                        End If
                    Next
                    For i = 0 To Me.selected.Count - 1
                        If (TypeOf Me.selected(i) Is Atom) Then
                            localAtom1 = DirectCast(Me.selected(i), Atom)
                            If Not localAtom1.NonGroupedAtom Then
                                Dim localAtom2 As Atom = localAtom1.Mol.getExpressionAtomWithGroupedAtom(localAtom1)
                                If (localAtom2 IsNot Nothing) AndAlso (Not localArrayList.Contains(localAtom2)) Then
                                    localArrayList.Add(localAtom2)
                                End If
                            End If
                        End If
                    Next
                    For i = 0 To localArrayList.Count - 1
                        localAtom1 = DirectCast(localArrayList(i), Atom)
                        j = 1
                        For k As Integer = 0 To localAtom1.GroupAtomSize - 1
                            Dim localAtom4 As Atom = localAtom1.getGroupAtom(k)
                            If Not Me.selected.Contains(localAtom4) Then
                                j = 0
                                Exit For
                            End If
                        Next
                        If j = 0 Then
                            localArrayList.Remove(localAtom1)
                            i -= 1
                        ElseIf Not Me.selected.Contains(localAtom1) Then
                            Me.selected.Add(localAtom1)
                        End If
                    Next
                Else
                    For i = Me.selected.Count - 1 To -1 + 1 Step -1
                        If (TypeOf Me.selected(i) Is Atom) Then
                            localAtom1 = DirectCast(Me.selected(i), Atom)
                            If localAtom1.Express_group Then
                                Dim localAtom3 As Atom
                                For j = 0 To localAtom1.GroupAtomSize - 1
                                    localAtom3 = localAtom1.getGroupAtom(j)
                                    Me.selected.Add(localAtom3)
                                    For m As Integer = 0 To localAtom3.numBond() - 1
                                        Dim localBond As Bond = localAtom3.getBond(m)
                                        Dim localAtom5 As Atom = If(localBond.Atom1.Equals(localAtom3), localBond.Atom2, localBond.Atom1)
                                        If Me.selected.Contains(localAtom5) Then
                                            Me.selected.Add(localBond)
                                        End If
                                    Next
                                Next
                                For j = 0 To localAtom1.GroupPartnerSize - 1
                                    localAtom3 = localAtom1.getGroupPartner(j)
                                    If Me.selected.Contains(localAtom3) Then
                                        Me.selected.Add(localAtom3.getBond(localAtom1))
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                copy(Me.selected, Me.m_clipboard)
            Else
                Me.m_clipboard.Clear()
                Me.clipReaction = Nothing
            End If
        End Sub

        Private Function getRectangleAboutSelectedArea(paramVector As ArrayList) As Rectangle
            Dim i As Integer = Integer.MaxValue
            Dim j As Integer = Integer.MinValue
            Dim k As Integer = Integer.MaxValue
            Dim m As Integer = Integer.MinValue
            Dim n As Integer = 0
            For i1 As Integer = 0 To paramVector.Count - 1
                Dim localObject As Object = paramVector(i1)
                If (TypeOf localObject Is Atom) Then
                    Dim localAtom As Atom = DirectCast(localObject, Atom)
                    Dim i2 As Integer = localAtom.DX(Me.dispscale)
                    i = Math.Min(i, i2)
                    j = Math.Max(j, i2)
                    Dim i3 As Integer = localAtom.DY(Me.dispscale)
                    k = Math.Min(k, i3)
                    m = Math.Max(m, i3)
                    n = 1
                End If
            Next
            If n <> 0 Then
                Return New Rectangle(i, k, j - i, m - k)
            End If
            Return New Rectangle()
        End Function

        Friend Overridable Function isCollision(paramReaction1 As Reaction, paramReaction2 As Reaction) As Boolean
            For i As Integer = 0 To paramReaction1.objectNum() - 1
                If (TypeOf paramReaction1.getObject(i) Is Molecule) Then
                    Dim localMolecule As Molecule = DirectCast(paramReaction1.getObject(i), Molecule)
                    For j As Integer = 0 To localMolecule.AtomNum - 1
                        Dim localAtom1 As Atom = localMolecule.getAtom(j + 1)
                        Dim k As Integer = localAtom1.DX(Me.dispscale)
                        Dim m As Integer = localAtom1.DY(Me.dispscale)
                        Dim localAtom2 As Atom = paramReaction2.nearAtom(k, m, Me.dispscale, 6)
                        If localAtom2 IsNot Nothing Then
                            Return True
                        End If
                    Next
                End If
            Next
            Return False
        End Function

        Public Overridable Sub getClipboard(paramDimension As DblRect, paramReaction As Reaction)
            Dim i As Integer = 0
            For j As Integer = 0 To paramReaction.objectNum() - 1
                If (TypeOf paramReaction.getObject(j) Is ReactionArrow) Then
                    i = 1
                    Exit For
                End If
            Next
            If Me.m_clipboard.Count > 0 Then
                For j = 0 To Me.m_clipboard.Count - 1
                    If (TypeOf Me.m_clipboard(j) Is Atom) Then
                        Dim localAtom1 As Atom = DirectCast(Me.m_clipboard(j), Atom)
                        If localAtom1.Express_group Then
                            Me.m_clipboard.Remove(localAtom1)
                            j -= 1
                        End If
                    End If
                Next
                Dim localArrayList As New ArrayList()
                Dim localAtom2 As Atom
                For k As Integer = 0 To Me.m_clipboard.Count - 1
                    If (TypeOf Me.m_clipboard(k) Is Atom) Then
                        localAtom2 = DirectCast(Me.m_clipboard(k), Atom)
                        If Not localAtom2.NonGroupedAtom Then
                            Dim localAtom3 As Atom = localAtom2.Mol.getExpressionAtomWithGroupedAtom(localAtom2)
                            If (localAtom3 IsNot Nothing) AndAlso (Not localArrayList.Contains(localAtom3)) Then
                                localArrayList.Add(localAtom3)
                            End If
                        End If
                    End If
                Next
                Dim localObject As Object
                For k = 0 To localArrayList.Count - 1
                    localAtom2 = DirectCast(localArrayList(k), Atom)
                    m = 1
                    For n = 0 To localAtom2.GroupAtomSize - 1
                        localObject = localAtom2.getGroupAtom(n)
                        If Not Me.m_clipboard.Contains(localObject) Then
                            m = 0
                            Exit For
                        End If
                    Next
                    If m = 0 Then
                        localArrayList.Remove(localAtom2)
                        k -= 1
                    ElseIf Not Me.m_clipboard.Contains(localAtom2) Then
                        Me.m_clipboard.Add(localAtom2)
                    End If
                Next
                copy(Me.m_clipboard, Me.selected, New DblRect(0, 0), paramDimension, True, localArrayList)
                Me.clipReaction.[select](True)
                Dim localRectangle As Rectangle = getRectangleAboutSelectedArea(Me.selected)
                Me.clipReaction.move(paramDimension.width - (localRectangle.x + localRectangle.width / 2), paramDimension.height - (localRectangle.y + localRectangle.height / 2))
                Dim bool As Boolean = isCollision(Me.clipReaction, paramReaction)
                Dim m As Integer = 0
                While bool
                    For n = 0 To Me.clipReaction.objectNum() - 1
                        localObject = Me.clipReaction.getObject(n)
                        If (TypeOf localObject Is Molecule) Then
                            DirectCast(localObject, Molecule).moveInternal(7, 7, Me.dispscale)
                        ElseIf (TypeOf localObject Is Bracket) Then
                            DirectCast(localObject, Bracket).moveInternal(7, 7, Me.dispscale)
                        End If
                    Next
                    bool = isCollision(Me.clipReaction, paramReaction)
                End While
                For n As Integer = 0 To Me.clipReaction.bracketNum() - 1
                    paramReaction.addBracket(Me.clipReaction.getBracket(n))
                Next
                For n = 0 To Me.clipReaction.objectNum() - 1
                    If (TypeOf Me.clipReaction.getObject(n) Is ReactionArrow) Then
                        If i <> 0 Then
                            Me.selected.Remove(Me.clipReaction.getObject(n))
                        Else
                            i = 1
                            paramReaction.addObject(Me.clipReaction.getObject(n), 0)
                        End If
                    Else
                        paramReaction.addObject(Me.clipReaction.getObject(n), 0)
                    End If
                Next
                resetArea()
            End If
        End Sub

        Public Overridable Sub duplicate(paramReaction As Reaction)
            Dim i As Integer = 0
            For j As Integer = 0 To Me.selected.Count - 1
                If (TypeOf Me.selected(j) Is ReactionArrow) Then
                    i = 1
                    Exit For
                End If
            Next
            If Me.selected.Count > 0 Then
                Dim localVector As New ArrayList()
                Me.flipflag = (Not Me.flipflag)
                Dim localObject As Object = copy(Me.selected, localVector, New DblRect(Me.select_area.x, Me.select_area.y), New DblRect(Me.select_area.x + Me.diff, Me.select_area.y + Me.diff))
                Me.flipflag = (Not Me.flipflag)
                clear()
                paramReaction.unselect()
                Me.selected = localVector
                resetArea()
                If localObject IsNot Nothing Then
                    If (TypeOf localObject Is Atom) Then
                        Me.atom = DirectCast(localObject, Atom)
                    ElseIf (TypeOf localObject Is Bond) Then
                        Me.bond = DirectCast(localObject, Bond)
                    End If
                End If
                Me.clipReaction.[select](True)
                paramReaction.add(Me.clipReaction)
            End If
        End Sub

        Public Overridable Function exportG(paramInt1 As Integer, paramInt2 As Integer) As keg.glycan.Molecule
            Dim localMolecule As keg.glycan.Molecule = Nothing
            Dim localVector1 As New ArrayList()
            If Me.selected.Count > 0 Then
                Dim localVector2 As New ArrayList()
                copy(Me.selected, localVector2, New DblRect(Me.select_area.x, Me.select_area.y), New DblRect(Me.select_area.x - paramInt1, Me.select_area.y - paramInt2), False, Nothing)
                For i As Integer = 0 To Me.clipReaction.bracketNum() - 1
                    localVector1.Add(Me.clipReaction.getBracket(i))
                Next
                For i = 0 To Me.clipReaction.objectNum() - 1
                    If (Not (TypeOf Me.clipReaction.getObject(i) Is ReactionArrow)) AndAlso ((TypeOf Me.clipReaction.getObject(i) Is Molecule)) Then
                        Dim localMolecule1 As Molecule = DirectCast(Me.clipReaction.getObject(i), Molecule)
                        Dim arrayOfInt As Integer() = localMolecule1.moleculeRange(Me.dispscale)
                        localMolecule1.setUpperLeft(Me.dispscale, New DblRect(Me.select_area.x - paramInt1, Me.select_area.y - paramInt2))
                        localMolecule1.moveInternal(arrayOfInt(0) - Me.select_area.x, Me.select_area.y - arrayOfInt(1), Me.dispscale)
                        For j As Integer = 0 To localVector1.Count - 1
                            DirectCast(localVector1(j), Bracket).reset0point(New DblRect(Me.select_area.x - paramInt1, Me.select_area.y - paramInt2), Me.dispscale)
                        Next
                        localMolecule = New keg.glycan.Molecule(localMolecule1, localVector1, Me.dispscale, New DblRect(Me.select_area.x - paramInt1, Me.select_area.y - paramInt2))
                        Exit For
                    End If
                Next
            End If
            Return localMolecule
        End Function

        Friend Overridable Function copy(paramVector1 As ArrayList, paramVector2 As ArrayList) As Object
            If paramVector1.Count = 0 Then
                Return Nothing
            End If
            Me.clipsize = New DblRect(Me.select_area.width, Me.select_area.height)
            Return copy(paramVector1, paramVector2, New DblRect(Me.select_area.x + Me.select_area.width / 2, Me.select_area.y + Me.select_area.height / 2), New DblRect(0, 0))
        End Function

        Friend Overridable Function copy(paramVector1 As ArrayList, paramVector2 As ArrayList, paramDimension1 As DblRect, paramDimension2 As DblRect) As Object
            Return copy(paramVector1, paramVector2, paramDimension1, paramDimension2, True, Nothing)
        End Function

        Friend Overridable Function copy(paramVector1 As ArrayList, paramVector2 As ArrayList, paramDimension1 As DblRect, paramDimension2 As DblRect, paramBoolean As Boolean, paramArrayList As List(Of Atom)) As Object
            If paramVector1.Count = 0 Then
                Return Nothing
            End If
            Me.clipReaction = New Reaction()
            Dim localMolecule As New Molecule()
            Dim arrayOfInt As Integer() = New Integer(paramVector1.Count - 1) {}
            localMolecule.set0point(paramDimension2)
            Me.clipReaction.addObject(localMolecule, 0)
            paramVector2.Clear()
            Dim localHashtable As New Hashtable()
            Dim localObject As Object
            Dim localAtom1 As Atom
            For k As Integer = 0 To paramVector1.Count - 1
                localObject = paramVector1(k)
                If (TypeOf localObject Is Atom) Then
                    arrayOfInt(k) = paramVector2.Count
                    localAtom1 = DirectCast(localObject, Atom)
                    If Me.flipflag Then
                        Me.atom = New Atom(localMolecule, (localAtom1.DX(Me.dispscale) - paramDimension1.width) / Me.dispscale, (paramDimension1.height - localAtom1.DY(Me.dispscale)) / Me.dispscale, 0.0, DirectCast(localObject, Atom).Label, DirectCast(localObject, Atom).Charge,
                            DirectCast(localObject, Atom).Isotope, DirectCast(localObject, Atom).Chiral)
                    Else
                        Me.atom = New Atom(localMolecule, (localAtom1.DX(Me.dispscale) - paramDimension1.width) / Me.dispscale, (localAtom1.DY(Me.dispscale) - paramDimension1.height) / Me.dispscale, 0.0, localAtom1.Label, localAtom1.Charge,
                            localAtom1.Isotope, localAtom1.Chiral)
                    End If
                    If DirectCast(localObject, Atom).Label IsNot Nothing Then
                        Me.atom.Label = New String(localAtom1.Label)
                    End If
                    If DirectCast(localObject, Atom).col IsNot Nothing Then
                        Me.atom.col = New Color(localAtom1.col.RGB)
                    End If
                    localHashtable(localAtom1) = Me.atom
                    paramVector2.Add(Me.atom)
                    localMolecule.addAtom(Me.atom)
                    Me.atom.Express_group = localAtom1.Express_group
                    Me.atom.NonGroupedAtom = True
                End If
            Next
            For k = 0 To paramVector1.Count - 1
                localObject = paramVector1(k)
                If (TypeOf localObject Is Atom) Then
                    localAtom1 = DirectCast(localObject, Atom)
                    If localAtom1.Express_group Then
                        Me.atom = DirectCast(localHashtable(DirectCast(localObject, Atom)), Atom)
                        If Me.atom IsNot Nothing Then
                            Dim n As Integer = localAtom1.GroupAtomSize
                            For i1 As Integer = 0 To n - 1
                                Dim localAtom2 As Atom = DirectCast(localHashtable(localAtom1.getGroupAtom(i1)), Atom)
                                If localAtom2 IsNot Nothing Then
                                    Me.atom.addGroupedAtom(localAtom2)
                                    localAtom2.NonGroupedAtom = False
                                End If
                            Next
                            i1 = localAtom1.GroupPartnerSize
                            For i2 As Integer = 0 To i1 - 1
                                Dim localAtom3 As Atom = DirectCast(localHashtable(localAtom1.getGroupPartner(i2)), Atom)
                                If localAtom3 IsNot Nothing Then
                                    Me.atom.addGroupPartner(localAtom3)
                                End If
                            Next
                        End If
                    End If
                End If
            Next
            For k = 0 To paramVector1.Count - 1
                localObject = paramVector1(k)
                If (TypeOf localObject Is Bond) Then
                    Dim j As Integer
                    Dim i As Integer = InlineAssignHelper(j, -1)
                    Me.atom = DirectCast(localObject, Bond).Atom1
                    For m As Integer = 0 To paramVector1.Count - 1
                        If Me.atom Is paramVector1(m) Then
                            i = arrayOfInt(m)
                        End If
                    Next
                    Me.atom = DirectCast(localObject, Bond).Atom2
                    For m = 0 To paramVector1.Count - 1
                        If Me.atom Is paramVector1(m) Then
                            j = arrayOfInt(m)
                        End If
                    Next
                    If (i >= 0) AndAlso (j >= 0) Then
                        arrayOfInt(k) = paramVector2.Count
                        Try
                            Me.bond = New Bond(localMolecule, DirectCast(paramVector2(i), Atom), DirectCast(paramVector2(j), Atom), DirectCast(localObject, Bond).Order, DirectCast(localObject, Bond).Stereo, DirectCast(localObject, Bond).Orientation)
                            Me.bond.col = DirectCast(paramVector1(k), Bond).col
                            paramVector2.Add(Me.bond)
                            localMolecule.addBond(Me.bond)
                        Catch localIllegalFormatException As keg.common.exception.IllegalFormatException
                            Console.Write(localIllegalFormatException)
                        End Try
                    End If
                End If
            Next
            For k = 0 To paramVector1.Count - 1
                localObject = paramVector1(k)
                If (TypeOf localObject Is ReactionArrow) Then
                    Dim localReactionArrow As New ReactionArrow(DirectCast(localObject, ReactionArrow).DX1() - paramDimension1.width + paramDimension2.width, DirectCast(localObject, ReactionArrow).DY1() - paramDimension1.height + paramDimension2.height, DirectCast(localObject, ReactionArrow).Direction, DirectCast(localObject, ReactionArrow).Length)
                    Me.clipReaction.addObject(localReactionArrow, 0)
                    paramVector2.Add(localReactionArrow)
                End If
            Next
            For k = 0 To paramVector1.Count - 1
                localObject = paramVector1(k)
                If (TypeOf localObject Is Bracket) Then
                    Dim localBracket As Bracket = DirectCast(DirectCast(localObject, Bracket).clone(), Bracket)
                    Dim localDimension As DblRect = localBracket.get0point()
                    localBracket.set0point(localDimension.width - paramDimension1.width + paramDimension2.width, localDimension.height - paramDimension1.height + paramDimension2.height)
                    Me.clipReaction.addBracket(localBracket)
                    paramVector2.Add(localBracket)
                End If
            Next
            If paramBoolean Then
                Me.clipReaction.refine()
            End If
            If Me.atom IsNot Nothing Then
                For k = 0 To paramVector1.Count - 1
                    localObject = paramVector1(k)
                    If ((TypeOf localObject Is Atom)) AndAlso (localObject Is Me.atom) Then
                        If arrayOfInt(k) < 0 Then
                            Exit For
                        End If
                        Return paramVector1(arrayOfInt(k))
                    End If
                Next
            End If
            If Me.bond IsNot Nothing Then
                For k = 0 To paramVector1.Count - 1
                    localObject = paramVector1(k)
                    If ((TypeOf localObject Is Bond)) AndAlso (localObject Is Me.bond) Then
                        If arrayOfInt(k) < 0 Then
                            Exit For
                        End If
                        Return paramVector1(arrayOfInt(k))
                    End If
                Next
            End If
            Return Nothing
        End Function

        Public Overridable Sub clearUNDO()
            Me.undo_reserve.Clear()
        End Sub

        Public Overridable Function undoOK() As Boolean
            Return Me.undostack.Count > 0
        End Function

        Public Overridable Function redoOK() As Boolean
            Return Me.redostack.Count > 0
        End Function

        Public Overridable ReadOnly Property ChangeArea() As Boolean
            Get
                Dim localUndoUnit As UndoUnit = topUnit()
                Return (Me.select_area.x <> localUndoUnit.undo_select_area.x) OrElse (Me.select_area.y <> localUndoUnit.undo_select_area.y) OrElse (Me.select_area.width <> localUndoUnit.undo_select_area.width) OrElse (Me.select_area.height <> localUndoUnit.undo_select_area.height)
            End Get
        End Property

        Public Overridable ReadOnly Property ChangeItems() As Boolean
            Get
                Dim localUndoUnit As UndoUnit = topUnit()
                If localUndoUnit Is Nothing Then
                    Return Me.selected.Count > 0
                End If
                If Me.selected.Count <> localUndoUnit.undo_selected.Count Then
                    Return True
                End If
                For i As Integer = 0 To Me.selected.Count - 1
                    If Not localUndoUnit.undo_selected.Contains(Me.selected(i)) Then
                        Return True
                    End If
                Next
                Return False
            End Get
        End Property

        Private Function topUNDOUnit() As UndoUnit
            Return If(Me.undostack.Count > 0, DirectCast(Me.undostack(Me.undostack.Count - 1), UndoUnit), Nothing)
        End Function

        Private Function popUNDOUnit() As UndoUnit
            Dim localUndoUnit As UndoUnit = DirectCast(Me.undostack(Me.undostack.Count - 1), UndoUnit)
            Me.undostack.RemoveAt(Me.undostack.Count - 1)
            Return localUndoUnit
        End Function

        Private Sub pushUNDOUnit(paramUndoUnit As UndoUnit)
            Me.undostack.Add(paramUndoUnit)
            If Me.undostack.Count > 10 Then
                Me.undostack.RemoveAt(0)
            End If
        End Sub

        Private Function topREDOUnit() As UndoUnit
            Return If(Me.redostack.Count > 0, DirectCast(Me.redostack(Me.redostack.Count - 1), UndoUnit), Nothing)
        End Function

        Private Function popREDOUnit() As UndoUnit
            Dim localUndoUnit As UndoUnit = DirectCast(Me.redostack(Me.redostack.Count - 1), UndoUnit)
            Me.redostack.RemoveAt(Me.redostack.Count - 1)
            Return localUndoUnit
        End Function

        Private Sub pushREDOUnit(paramUndoUnit As UndoUnit)
            Me.redostack.Add(paramUndoUnit)
            If Me.redostack.Count > 10 Then
                Me.redostack.RemoveAt(0)
            End If
        End Sub

        Public Overridable Property UNDO() As Reaction
            Get
                Dim localUndoUnit As UndoUnit = topUnit()
                Dim localReaction As New Reaction()
                Dim localMDLSDInterpreter As keg.compound.io.MDLSDInterpreter
                If (Me.undo_reserve.Count > 0) AndAlso ((ChangeArea) OrElse (ChangeItems)) Then
                    Try
                        localMDLSDInterpreter = New keg.compound.io.MDLSDInterpreter(Me.undo_reserve)
                        localReaction = DirectCast(localMDLSDInterpreter.interpret(localReaction), Reaction)
                    Catch localException1 As Exception
                        Console.WriteLine(localException1)
                    End Try
                    localReaction.Coordinate = localUndoUnit.undo_origin
                Else
                    localUndoUnit = popUnit()
                    If localUndoUnit.undo_buffer.Count = 0 Then
                        Return localReaction
                    End If
                    Try
                        localMDLSDInterpreter = New keg.compound.io.MDLSDInterpreter(localUndoUnit.undo_buffer)
                        localReaction = DirectCast(localMDLSDInterpreter.interpret(localReaction), Reaction)
                    Catch localException2 As Exception
                        Console.WriteLine(localException2)
                    End Try
                    localReaction.Coordinate = localUndoUnit.undo_origin
                End If
                If localReaction IsNot Nothing Then
                    For i As Integer = 0 To localReaction.objectNum() - 1
                        If (TypeOf localReaction.getObject(i) Is Molecule) Then
                            Dim localMolecule As Molecule = DirectCast(localReaction.getObject(i), Molecule)
                            For j As Integer = 0 To localMolecule.AtomNum - 1
                                Dim localAtom As Atom = localMolecule.getAtom(j + 1)
                                localAtom.y *= -1.0
                            Next
                            j = localMolecule.BracketNum
                            For k As Integer = 1 To j
                                Dim localBracket As Bracket = localMolecule.getBracket(k)
                                localReaction.addBracket(localBracket)
                            Next
                        End If
                    Next
                    If Me.hydrogen_draw Then
                        localReaction.calcImplicitHydrogen()
                        localReaction.decisideHydrogenDraw()
                    End If
                End If
                localReaction.checkOverlapedBracketWithRefine()
                Return localReaction
            End Get
            Set
                value.OverlapedBracket = Scale
                clearUNDO()
                Dim localUndoUnit As New UndoUnit()
                localUndoUnit.undo_operation = Me.operation
                localUndoUnit.undo_origin.width = value.Coordinate.width
                localUndoUnit.undo_origin.height = value.Coordinate.height
                localUndoUnit.undo_select_area.x = Me.select_area.x
                localUndoUnit.undo_select_area.y = Me.select_area.y
                localUndoUnit.undo_select_area.width = Me.select_area.width
                localUndoUnit.undo_select_area.height = Me.select_area.height
                For i As Integer = 0 To Me.selected.Count - 1
                    localUndoUnit.undo_selected.Add(Me.selected(i))
                Next
                localUndoUnit.undo_buffer.Clear()
                Dim localMDLSDDescriptor As New keg.compound.io.MDLSDDescriptor(localUndoUnit.undo_buffer)
                localMDLSDDescriptor.ModeUndo = True
                localMDLSDDescriptor.Scale = Me.dispscale
                Try
                    localMDLSDDescriptor.describe(value)
                Catch localUnmatchedChemConteiner As keg.common.exception.UnmatchedChemConteiner
                End Try
                Me.undo_OK = True
                value.clearOverlapedBracket()
                If Me.undoredo_mode = 1 Then
                    pushREDOUnit(localUndoUnit)
                ElseIf Me.undoredo_mode = 2 Then
                    pushUNDOUnit(localUndoUnit)
                Else
                    Me.redostack.Clear()
                    pushUNDOUnit(localUndoUnit)
                End If
            End Set
        End Property

        Public Overridable WriteOnly Property UNDOreserve() As Reaction
            Set
                If Me.undo_reserve.Count > 0 Then
                    Return
                End If
                value.OverlapedBracket = Scale
                Dim localMDLSDDescriptor As New keg.compound.io.MDLSDDescriptor(Me.undo_reserve)
                localMDLSDDescriptor.ModeUndo = True
                localMDLSDDescriptor.Scale = Me.dispscale
                Try
                    localMDLSDDescriptor.describe(value)
                Catch localUnmatchedChemConteiner As keg.common.exception.UnmatchedChemConteiner
                End Try
                Me.undo_OK = True
                value.clearOverlapedBracket()
            End Set
        End Property


        Public Overridable WriteOnly Property Clipboard() As ArrayList
            Set
                Me.m_clipboard = value
            End Set
        End Property

        Public Overridable Property ShrinkMode() As Boolean
            Get
                Return Me._isShrinkMode
            End Get
            Set
                Me._isShrinkMode = value
            End Set
        End Property


        Private Function topUnit() As UndoUnit
            Dim localUndoUnit As UndoUnit = If(Me.undoredo_mode = 1, topUNDOUnit(), topREDOUnit())
            Return localUndoUnit
        End Function

        Private Function popUnit() As UndoUnit
            Dim localUndoUnit As UndoUnit = If(Me.undoredo_mode = 1, popUNDOUnit(), popREDOUnit())
            Return localUndoUnit
        End Function

        Public Overridable ReadOnly Property UndoOperation() As Integer
            Get
                Return topUnit().undo_operation
            End Get
        End Property

        Public Overridable WriteOnly Property UNDOREODMODE() As Integer
            Set
                Me.undoredo_mode = value
            End Set
        End Property
        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
    End Class
End Namespace
