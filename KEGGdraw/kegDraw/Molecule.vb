#Region "Microsoft.VisualBasic::3cd8befd34b741ff4cb65d2522608c57, mzkit\src\visualize\KCF\KEGGdraw\kegDraw\Molecule.vb"

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

    '   Total Lines: 1818
    '    Code Lines: 1696
    ' Comment Lines: 4
    '   Blank Lines: 118
    '     File Size: 86.42 KB


    '     Class Molecule
    ' 
    '         Properties: AtomNum, BondNum, BracketNum, LockOfCheckRing, OverlapedBracket
    '                     Parent, Scale, Title, XpointTemp, YpointTemp
    ' 
    '         Function: confirmeIntersectionOfBondsAndBracketLine, (+2 Overloads) convertInternal, get0point, getAtom, getAtomNo
    '                   getAtomsInBracket, getAtomsList, getBond, getBondNo, getBracket
    '                   getExpressionAtomWithGroupedAtom, hasTheBracket, InlineAssignHelper, internalX, internalY
    '                   isCarbon, mergeMol, (+3 Overloads) moleculeRange, moleculeRangeSub, (+2 Overloads) nearAtom
    '                   (+2 Overloads) nearBond, refine, serchRing, ToString, whichSide
    ' 
    '         Sub: (+4 Overloads) [select], addAtom, addBond, addBracket, addBracketStrings
    '              autoRotate, calcImplicitHydrogen, checkRing, (+3 Overloads) combineMol, decisideHydrogenDraw
    '              deleteAtom, deleteBond, doFixedLength, fixLength, flipHorizontal
    '              flipHorizontalIfSelected, flipStereoTypeofBonds, flipVertical, flipVerticalIfSelected, getAtomsList
    '              lockOfCheckRing, moveInternal, register, removeBracket, rescale
    '              resetKEGGAtomName, resetOpoint, resetSelect, searchConnection, searchConnection_For_Bracket_Sub
    '              (+2 Overloads) select_reverse, selectAllItems, (+2 Overloads) selectItems, set0point, set0pointTemp
    '              setBracketCoordinate, setDBond, setMiddleLeft, setSgroup, setUpperCenter
    '              setUpperLeft, terminalCheck, unlockOfCheckRing, (+2 Overloads) unselect
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports System.Collections
Imports Microsoft.VisualBasic.Imaging.LayoutModel

Namespace keg.compound


    Public Class Molecule
        Inherits ChemConteiner
        Private atoms As ArrayList = Me.objects
        Private bonds As New ArrayList()
        Private m_parent As ChemConteiner = Nothing
        Private m_title As String = Nothing
        Private dispscale As Double
        Private brackets As New ArrayList()
        Private atomflag As Integer()
        Private bondflag As Integer()
        Private workflag As Integer()
        Private cx As Double
        Private cy As Double
        Private m_scale As Double = 1.0
        Private __lockOfCheckRing As Boolean = False

        Public Overridable Sub set0point(paramDimension As DblRect)
            Me.displayX = paramDimension.Width
            Me.displayY = paramDimension.Height
            For i As Integer = 0 To Me.atoms.Count - 1
                DirectCast(Me.atoms(i), Atom).Recalc = True
            Next
            For i = 0 To Me.bonds.Count - 1
                DirectCast(Me.bonds(i), Bond).Recalc = True
            Next
        End Sub

        Public Overridable Function get0point() As DblRect
            Return New DblRect(Me.displayX, Me.displayY)
        End Function

        Public Overridable Sub set0pointTemp(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double)
            Me.cx = ((paramInt1 - Me.displayX) / paramDouble)
            Me.cy = ((Me.displayY - paramInt2) / paramDouble)
        End Sub

        Public Overridable ReadOnly Property XpointTemp() As Double
            Get
                Return Me.cx
            End Get
        End Property

        Public Overridable ReadOnly Property YpointTemp() As Double
            Get
                Return Me.cy
            End Get
        End Property

        Public Overridable Sub setUpperLeft(paramDouble As Double, paramDimension As DblRect)
            Dim d1 As Double = getAtom(1).x
            Dim d2 As Double = getAtom(1).y
            For i As Integer = 2 To Me.atoms.Count
                If d1 > getAtom(i).x Then
                    d1 = getAtom(i).x
                End If
                If d2 < getAtom(i).y Then
                    d2 = getAtom(i).y
                End If
            Next
            Me.displayX = (paramDimension.Width - CInt(Math.Truncate(d1 * paramDouble)))
            Me.displayY = (paramDimension.Height + CInt(Math.Truncate(d2 * paramDouble)))
            For i = 0 To Me.brackets.Count - 1
                getBracket(i + 1).set0point(Me.displayX, Me.displayY)
            Next
        End Sub

        Public Overridable Sub setUpperCenter(paramDouble As Double, paramDimension As DblRect)
            Dim d2 As Double = getAtom(1).x
            Dim d1 As Double = getAtom(1).x
            Dim d4 As Double = getAtom(1).y
            For i As Integer = 2 To Me.atoms.Count
                If d2 > getAtom(i).x Then
                    d2 = getAtom(i).x
                End If
                If d1 < getAtom(i).x Then
                    d1 = getAtom(i).x
                End If
                If d4 < getAtom(i).y Then
                    d4 = getAtom(i).y
                End If
            Next
            Dim d3 As Double = (d1 + d2) / 2.0
            Me.displayX = (paramDimension.Width - CInt(Math.Truncate(d3 * paramDouble)))
            Me.displayY = (paramDimension.Height + CInt(Math.Truncate(d4 * paramDouble)))
            For i = 0 To Me.brackets.Count - 1
                getBracket(i + 1).set0point(Me.displayX, Me.displayY)
            Next
        End Sub

        Public Overridable Sub setMiddleLeft(paramDouble As Double, paramDimension As DblRect)
            Dim d1 As Double = getAtom(1).x
            Dim d2 As Double = getAtom(1).y
            Dim d3 As Double = getAtom(1).y
            For i As Integer = 2 To Me.atoms.Count
                If d1 > getAtom(i).x Then
                    d1 = getAtom(i).x
                End If
                If d2 < getAtom(i).y Then
                    d2 = getAtom(i).y
                End If
                If d3 > getAtom(i).y Then
                    d3 = getAtom(i).y
                End If
            Next
            Dim d4 As Double = (d2 + d3) / 2.0
            Me.displayX = (paramDimension.Width - CInt(Math.Truncate(d1 * paramDouble)))
            Me.displayY = (paramDimension.Height + CInt(Math.Truncate(d4 * paramDouble)))
            For i = 0 To Me.brackets.Count - 1
                getBracket(i + 1).set0point(Me.displayX, Me.displayY)
            Next
        End Sub

        Public Overridable Property Parent() As ChemConteiner
            Get
                Return Me.m_parent
            End Get
            Set
                Me.m_parent = Value
            End Set
        End Property


        Public Overridable Property Title() As String
            Get
                Return Me.m_title
            End Get
            Set
                Me.m_title = Value
            End Set
        End Property


        Public Overridable Sub addAtom(paramAtom As Atom)
            If Not Me.atoms.Contains(paramAtom) Then
                Me.atoms.Add(paramAtom)
            End If
        End Sub

        Public Overridable Function getAtom(paramInt As Integer) As Atom
            If paramInt < 1 Then
                Return DirectCast(Nothing, Atom)
            End If
            If paramInt > Me.atoms.Count Then
                Return DirectCast(Nothing, Atom)
            End If
            Return DirectCast(Me.atoms(paramInt - 1), Atom)
        End Function

        Public Overridable Function getAtomNo(paramAtom As Atom) As Integer
            Return Me.atoms.IndexOf(paramAtom) + 1
        End Function

        Public Overridable ReadOnly Property AtomNum() As Integer
            Get
                Return Me.atoms.Count
            End Get
        End Property

        Public Overridable Sub addBond(paramBond As Bond)
            If Not Me.bonds.Contains(paramBond) Then
                Me.bonds.Add(paramBond)
            End If
        End Sub

        Public Overridable Function getBond(paramInt As Integer) As Bond
            If paramInt < 1 Then
                Return DirectCast(Nothing, Bond)
            End If
            If paramInt > Me.bonds.Count Then
                Return DirectCast(Nothing, Bond)
            End If
            Return DirectCast(Me.bonds(paramInt - 1), Bond)
        End Function

        Public Overridable Function getBondNo(paramBond As Bond) As Integer
            Return Me.bonds.IndexOf(paramBond) + 1
        End Function

        Public Overridable ReadOnly Property BondNum() As Integer
            Get
                Return Me.bonds.Count
            End Get
        End Property

        Public Overrides Sub register(paramEditMode As EditMode)
            For i As Integer = 0 To Me.atoms.Count - 1
                If DirectCast(Me.atoms(i), Atom).[select] Then
                    paramEditMode.selected.Add(Me.atoms(i))
                End If
            Next
            For i = 0 To Me.bonds.Count - 1
                If DirectCast(Me.bonds(i), Bond).[select] Then
                    paramEditMode.selected.Add(Me.bonds(i))
                End If
            Next
        End Sub

        Public Overrides Sub [select](paramBoolean As Boolean)
            MyBase.[select](paramBoolean)
            For i As Integer = 0 To Me.atoms.Count - 1
                DirectCast(Me.atoms(i), Atom).[select](paramBoolean)
            Next
            For i = 0 To Me.bonds.Count - 1
                DirectCast(Me.bonds(i), Bond).[select](paramBoolean)
            Next
        End Sub

        Public Overrides Sub [select](paramBoolean As Boolean, paramEditMode As EditMode)
            MyBase.[select](paramBoolean)
            For i As Integer = 0 To Me.atoms.Count - 1
                DirectCast(Me.atoms(i), Atom).[select](paramBoolean, paramEditMode)
            Next
            For i = 0 To Me.bonds.Count - 1
                DirectCast(Me.bonds(i), Bond).[select](paramBoolean, paramEditMode)
            Next
        End Sub

        Public Overrides Sub [select]()
            MyBase.[select](True)
            [select](True)
        End Sub

        Public Overrides Sub [select](paramEditMode As EditMode)
            MyBase.[select](True)
            [select](True, paramEditMode)
            resetSelect(paramEditMode)
        End Sub

        Public Overrides Sub unselect()
            [select](False)
        End Sub

        Public Overrides Sub unselect(paramEditMode As EditMode)
            [select](False, paramEditMode)
        End Sub

        Public Overrides Sub select_reverse()
            MyBase.select_reverse()
            For i As Integer = 0 To Me.atoms.Count - 1
                DirectCast(Me.atoms(i), Atom).select_reverse()
            Next
            For i = 0 To Me.bonds.Count - 1
                DirectCast(Me.bonds(i), Bond).select_reverse()
            Next
        End Sub

        Public Overrides Sub select_reverse(paramEditMode As EditMode)
            MyBase.select_reverse(paramEditMode)
            For i As Integer = 0 To Me.atoms.Count - 1
                DirectCast(Me.atoms(i), Atom).select_reverse(paramEditMode)
            Next
            resetSelect(paramEditMode)
        End Sub

        Friend Overridable Sub resetSelect(paramEditMode As EditMode)
            Dim localBond As Bond
            For i As Integer = paramEditMode.selected.Count - 1 To 0 Step -1
                Dim localObject As Object = paramEditMode.selected(i)
                If (TypeOf localObject Is Bond) Then
                    localBond = DirectCast(localObject, Bond)
                    If localBond.Mol Is Me Then
                        paramEditMode.selected.Remove(localBond)
                        localBond.[select](False)
                    End If
                End If
            Next
            For i = 0 To Me.bonds.Count - 1
                localBond = DirectCast(Me.bonds(i), Bond)
                Dim localAtom1 As Atom = localBond.Atom1
                Dim localAtom2 As Atom = localBond.Atom2
                If (localAtom1.[select]) AndAlso (localAtom2.[select]) Then
                    localBond.[select]()
                    paramEditMode.selected.Add(localBond)
                End If
            Next
            paramEditMode.resetArea()
        End Sub

        Public Overrides Sub selectItems(paramInt1 As Integer, paramInt2 As Integer, paramInt3 As Integer, paramInt4 As Integer, paramDouble As Double, paramEditMode As EditMode,
            paramBoolean1 As Boolean, paramBoolean2 As Boolean)
            Dim i As Integer = -1
            Dim localBond As Bond
            For j As Integer = paramEditMode.selected.Count - 1 To 0 Step -1
                Dim localObject As Object = paramEditMode.selected(j)
                If (TypeOf localObject Is Bond) Then
                    localBond = DirectCast(localObject, Bond)
                    If localBond.Mol Is Me Then
                        paramEditMode.selected.Remove(localBond)
                        localBond.[select](False)
                    End If
                End If
            Next
            Dim localAtom1 As Atom
            For j = 0 To Me.atoms.Count - 1
                localAtom1 = DirectCast(Me.atoms(j), Atom)
                If ((localAtom1.NonGroupedAtom) OrElse (Not paramBoolean2)) AndAlso ((localAtom1.Express_group <> True) OrElse (paramBoolean2)) AndAlso (localAtom1.DX(paramDouble) >= paramInt1) AndAlso (localAtom1.DX(paramDouble) <= paramInt3) AndAlso (localAtom1.DY(paramDouble) >= paramInt2) AndAlso (localAtom1.DY(paramDouble) <= paramInt4) Then
                    If (paramBoolean1) OrElse ((paramEditMode.ShrinkMode) AndAlso (Not localAtom1.NonGroupedAtom)) OrElse ((Not paramEditMode.ShrinkMode) AndAlso (localAtom1.Express_group)) Then
                        localAtom1.select_reverse(paramEditMode)
                    Else
                        localAtom1.[select](paramEditMode)
                    End If
                End If
            Next
            For j = 0 To Me.bonds.Count - 1
                localBond = DirectCast(Me.bonds(j), Bond)
                localAtom1 = localBond.Atom1
                Dim localAtom2 As Atom = localBond.Atom2
                If (localAtom1.[select]) AndAlso (localAtom2.[select]) Then
                    localBond.[select]()
                    paramEditMode.selected.Add(localBond)
                End If
            Next
        End Sub

        Public Overrides Sub selectItems(paramVector As ArrayList, paramDouble As Double, paramEditMode As EditMode, paramBoolean1 As Boolean, paramBoolean2 As Boolean)
            Dim i As Integer = -1
            Dim arrayOfInt1 As Integer() = New Integer(paramVector.Count - 1) {}
            Dim arrayOfInt2 As Integer() = New Integer(paramVector.Count - 1) {}
            For j As Integer = 0 To paramVector.Count - 1
                Dim localDimension As DblRect = DirectCast(paramVector(j), DblRect)
                arrayOfInt1(j) = localDimension.Width
                arrayOfInt2(j) = localDimension.Height
            Next
            Dim localPolygon As New java.awt.Polygon(arrayOfInt1, arrayOfInt2, paramVector.Count)
            Dim localBond As Bond
            For j = paramEditMode.selected.Count - 1 To 0 Step -1
                Dim localObject As Object = paramEditMode.selected(j)
                If (TypeOf localObject Is Bond) Then
                    localBond = DirectCast(localObject, Bond)
                    If localBond.Mol Is Me Then
                        paramEditMode.selected.Remove(localBond)
                        localBond.[select](False)
                    End If
                End If
            Next
            Dim localAtom1 As Atom
            For j = 0 To Me.atoms.Count - 1
                localAtom1 = DirectCast(Me.atoms(j), Atom)
                If ((localAtom1.NonGroupedAtom) OrElse (Not paramBoolean2)) AndAlso ((localAtom1.Express_group <> True) OrElse (paramBoolean2)) AndAlso (localPolygon.inside(localAtom1.DX(paramDouble), localAtom1.DY(paramDouble))) Then
                    If (paramBoolean1) OrElse ((paramEditMode.ShrinkMode) AndAlso (Not localAtom1.NonGroupedAtom)) OrElse ((Not paramEditMode.ShrinkMode) AndAlso (localAtom1.Express_group)) Then
                        localAtom1.select_reverse()
                    Else
                        localAtom1.[select]()
                    End If
                    If localAtom1.[select] Then
                        paramEditMode.selected.Add(localAtom1)
                    Else
                        paramEditMode.selected.Remove(localAtom1)
                    End If
                End If
            Next
            For j = 0 To Me.bonds.Count - 1
                localBond = DirectCast(Me.bonds(j), Bond)
                localAtom1 = localBond.Atom1
                Dim localAtom2 As Atom = localBond.Atom2
                If (localAtom1.[select]) AndAlso (localAtom2.[select]) Then
                    localBond.[select]()
                    paramEditMode.selected.Add(localBond)
                End If
            Next
        End Sub

        Public Overrides Sub selectAllItems(paramEditMode As EditMode, paramBoolean As Boolean)
            [select](True, paramEditMode)
            Dim localObject As Object
            For i As Integer = 0 To Me.atoms.Count - 1
                localObject = DirectCast(Me.atoms(i), Atom)
                If ((DirectCast(localObject, Atom).Express_group) AndAlso (Not paramBoolean)) OrElse ((Not DirectCast(localObject, Atom).NonGroupedAtom) AndAlso (paramBoolean)) Then
                    DirectCast(localObject, Atom).unselect(paramEditMode)
                End If
            Next
            For i = 0 To Me.bonds.Count - 1
                localObject = DirectCast(Me.bonds(i), Bond)
                If (Not DirectCast(localObject, Bond).NonGroupedBond) AndAlso (paramBoolean) Then
                    DirectCast(localObject, Bond).unselect(paramEditMode)
                End If
            Next
            paramEditMode.resetArea()
        End Sub

        Friend Overridable Function internalX(paramInt As Integer) As Double
            Return (paramInt - Me.displayX) / Me.dispscale
        End Function

        Friend Overridable Function internalY(paramInt As Integer) As Double
            Return (Me.displayY - paramInt) / Me.dispscale
        End Function

        Public Overridable Property BracketNum() As Integer
            Get
                Return Me.brackets.Count
            End Get
            Set
                For i As Integer = 0 To Value - 1
                    Dim localBracket As New Bracket()
                    localBracket.set0point(get0point())
                    localBracket.b2x1 = ((New Random(1)).NextDouble() * 1000.0)
                    addBracket(localBracket)
                Next
            End Set
        End Property

        Public Overridable ReadOnly Property OverlapedBracket() As ArrayList
            Get
                Return Me.brackets
            End Get
        End Property

        Public Overridable Function getBracket(paramInt As Integer) As Bracket
            If Me.brackets.Count < paramInt Then
                Return Nothing
            End If
            Return DirectCast(Me.brackets(paramInt - 1), Bracket)
        End Function


        Public Overridable Sub addBracketStrings(paramVector As ArrayList, paramHashtable As Hashtable, paramBoolean As Boolean)
            Dim localVector1 As New ArrayList()
            Dim i As Integer = 0
            Dim str As String = ""
            Dim localBracket As Bracket
            For j As Integer = 0 To Me.brackets.Count - 1
                localBracket = DirectCast(Me.brackets(j), Bracket)
                If localVector1.Contains(localBracket) <> True Then
                    localVector1.Add(localBracket)
                    str = str & keg.common.util.DEBTutil.printf(Convert.ToString(j + 1), "4") & keg.common.util.DEBTutil.printf(localBracket.Type, "4")
                    i += 1
                    If i = 8 Then
                        paramVector.Add("M  STY" & keg.common.util.DEBTutil.printf(Convert.ToString(i), "3") & str)
                        str = ""
                        i = 0
                    End If
                End If
            Next
            If i > 0 Then
                paramVector.Add("M  STY" & keg.common.util.DEBTutil.printf(Convert.ToString(i), "3") & str)
            End If
            str = ""
            i = 0
            localVector1.Clear()
            For j = 0 To Me.brackets.Count - 1
                localBracket = DirectCast(Me.brackets(j), Bracket)
                If localVector1.Contains(localBracket) <> True Then
                    localVector1.Add(localBracket)
                    str = str & keg.common.util.DEBTutil.printf(Convert.ToString(j + 1), "4") & keg.common.util.DEBTutil.printf(Convert.ToString(j + 1), "4")
                    i += 1
                    If i = 8 Then
                        paramVector.Add("M  SLB" & keg.common.util.DEBTutil.printf(Convert.ToString(i), "3") & str)
                        str = ""
                        i = 0
                    End If
                End If
            Next
            If i > 0 Then
                paramVector.Add("M  SLB" & keg.common.util.DEBTutil.printf(Convert.ToString(i), "3") & str)
            End If
            str = ""
            i = 0
            localVector1.Clear()
            For j = 0 To Me.brackets.Count - 1
                localBracket = DirectCast(Me.brackets(j), Bracket)
                If localBracket.Type.Equals("MON") Then
                    localVector1.Add(localBracket)
                ElseIf localVector1.Contains(localBracket) <> True Then
                    localVector1.Add(localBracket)
                    str = str & keg.common.util.DEBTutil.printf(Convert.ToString(j + 1), "4") & " " & localBracket.SgroupConnectivity & " "
                    i += 1
                    If i = 8 Then
                        paramVector.Add("M  SCN" & keg.common.util.DEBTutil.printf(Convert.ToString(i), "3") & str)
                        str = ""
                        i = 0
                    End If
                End If
            Next
            If i > 0 Then
                paramVector.Add("M  SCN" & keg.common.util.DEBTutil.printf(Convert.ToString(i), "3") & str)
            End If
            localVector1.Clear()
            For j = 0 To Me.brackets.Count - 1
                localBracket = DirectCast(Me.brackets(j), Bracket)
                If localVector1.Contains(localBracket) <> True Then
                    localVector1.Add(localBracket)
                    i = 0
                    str = ""
                    If localBracket.Sgroup IsNot Nothing Then
                        Dim localVector2 As New ArrayList()
                        Dim localObject1 As Object = paramHashtable.Keys.GetEnumerator()
                        While DirectCast(localObject1, System.Collections.IEnumerator).hasMoreElements()
                            localObject2 = DirectCast(DirectCast(localObject1, System.Collections.IEnumerator).nextElement(), String)
                            If DirectCast(localObject2, String).StartsWith(localBracket.GetHashCode() & ":") Then
                                localVector2.Add(CType(paramHashtable(localObject2), System.Nullable(Of Integer)))
                            End If
                        End While
                        java.util.Collections.sort(localVector2)
                        localObject1 = New ArrayList()
                        Dim localObject2 As Object = localBracket.Sgroup
                        For k As Integer = 0 To DirectCast(localObject2, ArrayList).Count - 1
                            Dim localObject3 As Object = DirectCast(localObject2, ArrayList)(k)
                            Dim n As Integer = getAtomNo(DirectCast(localObject3, Atom))
                            If localVector2.Contains(New System.Nullable(Of Integer)(n)) = True Then
                                DirectCast(localObject1, ArrayList).Add(localObject3)
                            End If
                        Next
                        Dim m As Integer
                        For k = 0 To localVector2.Count - 1
                            m = CInt(CType(localVector2(k), System.Nullable(Of Integer)))
                            If m <> 0 Then
                                str = str & keg.common.util.DEBTutil.printf(m, "4")
                            End If
                            i += 1
                            If i = 15 Then
                                paramVector.Add("M  SAL" & keg.common.util.DEBTutil.printf(Convert.ToString(j + 1), "4") & keg.common.util.DEBTutil.printf(Convert.ToString(i), "3") & str)
                                str = ""
                                i = 0
                            End If
                        Next
                        If i > 0 Then
                            paramVector.Add("M  SAL" & keg.common.util.DEBTutil.printf(Convert.ToString(j + 1), "4") & keg.common.util.DEBTutil.printf(Convert.ToString(i), "3") & str)
                        End If
                        If (localBracket.SBL1 <> 0) AndAlso (localBracket.SBL2 <> 0) Then
                            paramVector.Add("M  SBL" & keg.common.util.DEBTutil.printf(Convert.ToString(j + 1), "4") & keg.common.util.DEBTutil.printf(Convert.ToString(2), "3") & keg.common.util.DEBTutil.printf(Convert.ToString(localBracket.SBL1), "4") & keg.common.util.DEBTutil.printf(Convert.ToString(localBracket.SBL2), "4"))
                        End If
                        If DirectCast(localObject1, ArrayList).Count < localVector2.Count Then
                            str = ""
                            i = 0
                            For k = 0 To DirectCast(localObject1, ArrayList).Count - 1
                                m = getAtomNo(DirectCast(DirectCast(localObject1, ArrayList)(k), Atom))
                                If m <> 0 Then
                                    str = str & keg.common.util.DEBTutil.printf(m, "4")
                                End If
                                i += 1
                                If i = 15 Then
                                    paramVector.Add("M  SPA" & keg.common.util.DEBTutil.printf(Convert.ToString(j + 1), "4") & keg.common.util.DEBTutil.printf(Convert.ToString(i), "3") & str)
                                    str = ""
                                    i = 0
                                End If
                            Next
                            If i > 0 Then
                                paramVector.Add("M  SPA" & keg.common.util.DEBTutil.printf(Convert.ToString(j + 1), "4") & keg.common.util.DEBTutil.printf(Convert.ToString(i), "3") & str)
                            End If
                        End If
                        Dim d1 As Double = 1.0
                        Dim d2 As Double = 1.0
                        If paramBoolean Then
                            d2 = -1.0
                            paramVector.Add("M  SDI" & keg.common.util.DEBTutil.printf(Convert.ToString(j + 1), "4") & "  4" & keg.common.util.DEBTutil.printf(d1 * internalX(localBracket.DX(Me.dispscale, 1, 1)), "5.4") & keg.common.util.DEBTutil.printf(d2 * internalY(localBracket.DY(Me.dispscale, 1, 1)), "5.4") & keg.common.util.DEBTutil.printf(d1 * internalX(localBracket.DX(Me.dispscale, 1, 4)), "5.4") & keg.common.util.DEBTutil.printf(d2 * internalY(localBracket.DY(Me.dispscale, 1, 4)), "5.4"))
                            paramVector.Add("M  SDI" & keg.common.util.DEBTutil.printf(Convert.ToString(j + 1), "4") & "  4" & keg.common.util.DEBTutil.printf(d1 * internalX(localBracket.DX(Me.dispscale, 2, 1)), "5.4") & keg.common.util.DEBTutil.printf(d2 * internalY(localBracket.DY(Me.dispscale, 2, 1)), "5.4") & keg.common.util.DEBTutil.printf(d1 * internalX(localBracket.DX(Me.dispscale, 2, 4)), "5.4") & keg.common.util.DEBTutil.printf(d2 * internalY(localBracket.DY(Me.dispscale, 2, 4)), "5.4"))
                        Else
                            Dim i1 As Integer = 1
                            Dim i2 As Integer = 4
                            paramVector.Add("M  SDI" & keg.common.util.DEBTutil.printf(Convert.ToString(j + 1), "4") & "  4" & keg.common.util.DEBTutil.printf(d1 * internalX(localBracket.DX(Me.dispscale, 1, i1)), "5.4") & keg.common.util.DEBTutil.printf(d2 * internalY(localBracket.DY(Me.dispscale, 1, i1)), "5.4") & keg.common.util.DEBTutil.printf(d1 * internalX(localBracket.DX(Me.dispscale, 1, i2)), "5.4") & keg.common.util.DEBTutil.printf(d2 * internalY(localBracket.DY(Me.dispscale, 1, i2)), "5.4"))
                            i1 = 1
                            i2 = 4
                            paramVector.Add("M  SDI" & keg.common.util.DEBTutil.printf(Convert.ToString(j + 1), "4") & "  4" & keg.common.util.DEBTutil.printf(d1 * internalX(localBracket.DX(Me.dispscale, 2, i1)), "5.4") & keg.common.util.DEBTutil.printf(d2 * internalY(localBracket.DY(Me.dispscale, 2, i1)), "5.4") & keg.common.util.DEBTutil.printf(d1 * internalX(localBracket.DX(Me.dispscale, 2, i2)), "5.4") & keg.common.util.DEBTutil.printf(d2 * internalY(localBracket.DY(Me.dispscale, 2, i2)), "5.4"))
                        End If
                        str = localBracket.Label
                        If (str.Length > 0) AndAlso (Not "MON".Equals(localBracket.Type)) Then
                            paramVector.Add("M  SMT" & keg.common.util.DEBTutil.printf(Convert.ToString(j + 1), "4") & " " & str)
                        End If
                    End If
                End If
            Next
        End Sub

        Public Overridable Sub calcImplicitHydrogen()
            For i As Integer = 0 To Me.atoms.Count - 1
                Dim localAtom As Atom = DirectCast(Me.atoms(i), Atom)
                localAtom.calcImplicitHydrogen()
            Next
        End Sub

        Public Overridable Sub decisideHydrogenDraw()
            For i As Integer = 0 To Me.atoms.Count - 1
                Dim localAtom As Atom = DirectCast(Me.atoms(i), Atom)
                localAtom.decisideHydrogenDraw()
            Next
        End Sub

        Public Overridable Function mergeMol(paramMolecule As Molecule, paramDouble As Double) As Molecule
            Dim localMolecule As New Molecule()
            Dim i As Integer = 0
            localMolecule.set0point(get0point())
            Dim localAtom2 As Atom
            Dim localAtom1 As Atom
            For m As Integer = 0 To Me.atoms.Count - 1
                localAtom2 = DirectCast(Me.atoms(m), Atom)
                localAtom1 = New Atom(localMolecule, localAtom2.x, localAtom2.y, localAtom2.z, localAtom2.Label, localAtom2.Charge,
                    localAtom2.Isotope, localAtom2.Chiral)
                localAtom1.col = localAtom2.col
                localMolecule.addAtom(localAtom1)
            Next
            i = Me.atoms.Count
            Dim localBond2 As Bond
            Dim j As Integer
            Dim k As Integer
            Dim localBond1 As Bond
            For m = 0 To Me.bonds.Count - 1
                localBond2 = DirectCast(Me.bonds(m), Bond)
                localAtom1 = localBond2.Atom1
                j = getAtomNo(localAtom1)
                localAtom1 = localBond2.Atom2
                k = getAtomNo(localAtom1)
                Try
                    localBond1 = New Bond(localMolecule, localMolecule.getAtom(j), localMolecule.getAtom(k), localBond2.Order, localBond2.Stereo, localBond2.Orientation)
                    localBond1.col = localBond2.col
                    localMolecule.addBond(localBond1)
                Catch localIllegalFormatException1 As keg.common.exception.IllegalFormatException
                    Console.WriteLine(localIllegalFormatException1)
                End Try
            Next
            If paramMolecule IsNot Nothing Then
                Dim localDimension1 As DblRect = paramMolecule.get0point()
                Dim localDimension2 As New DblRect(localDimension1.Width - Me.displayX, Me.displayY - localDimension1.Height)
                For m = 0 To paramMolecule.atoms.Count - 1
                    localAtom2 = DirectCast(paramMolecule.atoms(m), Atom)
                    localAtom1 = New Atom(localMolecule, localAtom2.x + localDimension2.Width / paramDouble, localAtom2.y + localDimension2.Height / paramDouble, localAtom2.z, localAtom2.Label, localAtom2.Charge,
                        localAtom2.Isotope, localAtom2.Chiral)
                    localAtom1.col = localAtom2.col
                    localMolecule.addAtom(localAtom1)
                Next
                For m = 0 To paramMolecule.bonds.Count - 1
                    localBond2 = DirectCast(paramMolecule.bonds(m), Bond)
                    localAtom1 = localBond2.Atom1
                    j = paramMolecule.getAtomNo(localAtom1) + i
                    localAtom1 = localBond2.Atom2
                    k = paramMolecule.getAtomNo(localAtom1) + i
                    Try
                        localBond1 = New Bond(localMolecule, localMolecule.getAtom(j), localMolecule.getAtom(k), localBond2.Order, localBond2.Stereo, localBond2.Orientation)
                        localBond1.col = localBond2.col
                        localMolecule.addBond(localBond1)
                    Catch localIllegalFormatException2 As keg.common.exception.IllegalFormatException
                        Console.WriteLine(localIllegalFormatException2)
                    End Try
                Next
            End If
            Return localMolecule
        End Function

        Public Overridable Sub combineMol(paramAtom1 As Atom, paramMolecule As Molecule, paramAtom2 As Atom, paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double)
            combineMol(paramAtom1, paramMolecule, paramAtom2, paramInt1, paramInt2, paramDouble,
                True)
        End Sub

        Public Overridable Sub combineMol(paramAtom1 As Atom, paramMolecule As Molecule, paramAtom2 As Atom, paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double,
            paramBoolean As Boolean)
            Dim i As Integer
            If Me Is paramMolecule Then
                i = 0
                Dim localBond As Bond = Nothing
                For j As Integer = 0 To paramAtom1.numBond() - 1
                    localBond = paramAtom1.getBond(j)
                    If localBond.pairAtom(paramAtom1) Is paramAtom2 Then
                        i = 1
                        Exit For
                    End If
                Next
                If (i <> 0) AndAlso (paramBoolean) Then
                    Select Case paramInt1
                        Case 1
                            Select Case localBond.Order
                                Case 1
                                    localBond.Order = 2

                                Case 2
                                    localBond.Order = 3

                                Case Else
                                    localBond.Order = paramInt1
                                    localBond.Stereo = paramInt2

                            End Select

                        Case 2
                            localBond.Order = paramInt1

                        Case 3
                            localBond.Order = paramInt1

                        Case 4
                            localBond.Order = paramInt1

                        Case -1
                            localBond.Order = paramInt1

                        Case 0
                            localBond.Order = paramInt1

                    End Select
                ElseIf i = 0 Then
                    Try
                        addBond(New Bond(Me, paramAtom1, paramAtom2, paramInt1, paramInt2, 0))
                    Catch localIllegalFormatException2 As keg.common.exception.IllegalFormatException
                        Console.WriteLine(localIllegalFormatException2)
                    End Try
                End If
            Else
                paramMolecule.moveInternal(paramMolecule.displayX - Me.displayX, Me.displayY - paramMolecule.displayY, paramDouble)
                For i = 0 To paramMolecule.atoms.Count - 1
                    DirectCast(paramMolecule.atoms(i), Atom).Mol = Me
                Next
                For i = 0 To paramMolecule.bonds.Count - 1
                    DirectCast(paramMolecule.bonds(i), Bond).Mol = Me
                Next
                Me.atoms.AddRange(paramMolecule.atoms)
                Me.bonds.AddRange(paramMolecule.bonds)
                Try
                    addBond(New Bond(Me, paramAtom1, paramAtom2, paramInt1, paramInt2, 0))
                Catch localIllegalFormatException1 As keg.common.exception.IllegalFormatException
                    Console.WriteLine(localIllegalFormatException1)
                End Try
                If (TypeOf Me.m_parent Is Reaction) Then
                    DirectCast(Me.m_parent, Reaction).delObject(paramMolecule)
                End If
            End If
        End Sub

        Public Overridable Sub combineMol(paramAtom1 As Atom, paramAtom2 As Atom, paramDouble As Double)
            If paramAtom1.Mol IsNot Me Then
                Return
            End If
            Dim localMolecule1 As Molecule = paramAtom2.Mol
            localMolecule1.moveInternal(localMolecule1.displayX - Me.displayX, Me.displayY - localMolecule1.displayY, paramDouble)
            resetOpoint(paramDouble)
            localMolecule1.resetOpoint(paramDouble)
            Dim localMolecule2 As Molecule = paramAtom1.Mol
            Dim localMolecule3 As Molecule = paramAtom2.Mol
            Dim localArrayList1 As New ArrayList()
            Dim localArrayList2 As New ArrayList()
            Dim localObject As Object
            For i As Integer = 0 To localMolecule3.AtomNum - 1
                localObject = localMolecule3.getAtom(i + 1)
                Dim localAtom1 As Atom = localMolecule2.nearAtom(DirectCast(localObject, Atom).DX(paramDouble), DirectCast(localObject, Atom).DY(paramDouble), paramDouble, 3)
                If localAtom1 IsNot Nothing Then
                    localArrayList1.Add(localAtom1)
                    localArrayList2.Add(localObject)
                ElseIf localMolecule2 IsNot localMolecule3 Then
                    DirectCast(localObject, Atom).Mol = localMolecule2
                    localMolecule2.addAtom(DirectCast(localObject, Atom))
                End If
            Next
            If localArrayList1.Count < 1 Then
                Return
            End If
            For i = 0 To localMolecule3.BondNum - 1
                localObject = localMolecule3.getBond(i + 1)
                If localMolecule2 IsNot localMolecule3 Then
                    If (localArrayList2.Contains(DirectCast(localObject, Bond).Atom1)) AndAlso (localArrayList2.Contains(DirectCast(localObject, Bond).Atom2)) Then
                        DirectCast(localObject, Bond).Atom1.breakBond(DirectCast(localObject, Bond))
                        DirectCast(localObject, Bond).Atom2.breakBond(DirectCast(localObject, Bond))
                        localMolecule3.deleteBond(DirectCast(localObject, Bond))
                        i -= 1
                        DirectCast(localObject, Bond).breakBond()
                    Else
                        DirectCast(localObject, Bond).Mol = localMolecule2
                    End If
                Else
                    If (localArrayList2.Contains(DirectCast(localObject, Bond).Atom1)) OrElse (localArrayList2.Contains(DirectCast(localObject, Bond).Atom2)) Then
                        For j As Integer = 0 To localArrayList2.Count - 1
                            Dim localAtom2 As Atom = DirectCast(localArrayList1(j), Atom)
                            Dim localAtom3 As Atom = DirectCast(localArrayList2(j), Atom)
                            Dim localAtom4 As Atom
                            If localAtom3.Equals(DirectCast(localObject, Bond).Atom1) Then
                                localAtom4 = DirectCast(localObject, Bond).Atom1
                                DirectCast(localObject, Bond).Atom1 = localAtom2
                                DirectCast(localObject, Bond).Mol = localMolecule2
                                localAtom2.makeBond(DirectCast(localObject, Bond))
                                If localAtom4 Is paramAtom2 Then
                                    localMolecule3.deleteAtom(localAtom4)
                                End If
                            ElseIf localAtom3.Equals(DirectCast(localObject, Bond).Atom2) Then
                                localAtom4 = DirectCast(localObject, Bond).Atom2
                                DirectCast(localObject, Bond).Atom2 = localAtom2
                                DirectCast(localObject, Bond).Mol = localMolecule2
                                localAtom2.makeBond(DirectCast(localObject, Bond))
                                If localAtom4 Is paramAtom2 Then
                                    localMolecule3.deleteAtom(localAtom4)
                                End If
                            End If
                        Next
                    End If
                    localMolecule2.addBond(DirectCast(localObject, Bond))
                End If
            Next
            If ((TypeOf Me.m_parent Is Reaction)) AndAlso (Not localMolecule2.Equals(localMolecule1)) Then
                DirectCast(Me.m_parent, Reaction).delObject(localMolecule1)
            End If
        End Sub

        Public Overridable Overloads Function ToString() As String
            Return "atoms = " & Me.atoms.Count & " : bonds = " & Me.bonds.Count
        End Function

        Public Overridable Sub deleteAtom(paramAtom As Atom)
            For i As Integer = Me.bonds.Count - 1 To 0 Step -1
                Dim localBond As Bond = DirectCast(Me.bonds(i), Bond)
                If localBond.hasAtom(paramAtom) Then
                    Me.bonds.RemoveAt(i)
                    Dim localAtom1 As Atom = localBond.pairAtom(paramAtom)
                    localAtom1.breakBond(localBond)
                    If (localAtom1.numBond() = 0) AndAlso (isCarbon(localAtom1)) Then
                        Me.atoms.Remove(localAtom1)
                    End If
                End If
            Next
            For i = 0 To Me.atoms.Count - 1
                Dim localAtom2 As Atom = DirectCast(Me.atoms(i), Atom)
                If localAtom2.Express_group Then
                    For j As Integer = 0 To localAtom2.GroupAtomSize - 1
                        Dim localAtom3 As Atom = localAtom2.getGroupAtom(j)
                        If paramAtom Is localAtom3 Then
                            localAtom2.removeGroupedAtom(localAtom3)
                        End If
                    Next
                End If
            Next
            Me.atoms.Remove(paramAtom)
        End Sub

        Private Function isCarbon(paramAtom As Atom) As Boolean
            If paramAtom.Label.Length = 0 Then
                Return True
            End If
            Return paramAtom.Label.Equals("C")
        End Function

        Public Overridable Sub deleteBond(paramBond As Bond)
            Dim localAtom1 As Atom = paramBond.Atom1
            Dim localAtom2 As Atom = paramBond.Atom2
            localAtom1.breakBond(paramBond)
            localAtom2.breakBond(paramBond)
            If localAtom1.numBond() = 0 Then
                Me.atoms.Remove(localAtom1)
            End If
            If localAtom2.numBond() = 0 Then
                Me.atoms.Remove(localAtom2)
            End If
            Dim i As Integer
            Dim localAtom3 As Atom
            Dim j As Integer
            Dim localAtom4 As Atom
            If (Not localAtom1.NonGroupedAtom) AndAlso (localAtom2.NonGroupedAtom) Then
                For i = 0 To Me.atoms.Count - 1
                    localAtom3 = DirectCast(Me.atoms(i), Atom)
                    If localAtom3.Express_group Then
                        For j = 0 To localAtom3.GroupPartnerSize - 1
                            localAtom4 = localAtom3.getGroupPartner(j)
                            If localAtom2 Is localAtom4 Then
                                localAtom3.removeGroupPartnerAtom(localAtom4)
                            End If
                        Next
                    End If
                Next
            ElseIf (Not localAtom2.NonGroupedAtom) AndAlso (localAtom1.NonGroupedAtom) Then
                For i = 0 To Me.atoms.Count - 1
                    localAtom3 = DirectCast(Me.atoms(i), Atom)
                    If localAtom3.Express_group Then
                        For j = 0 To localAtom3.GroupPartnerSize - 1
                            localAtom4 = localAtom3.getGroupPartner(j)
                            If localAtom1 Is localAtom4 Then
                                localAtom3.removeGroupPartnerAtom(localAtom4)
                            End If
                        Next
                    End If
                Next
            End If
            Me.bonds.Remove(paramBond)
        End Sub

        Public Overridable Sub moveInternal(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double)
            For i As Integer = 0 To Me.atoms.Count - 1
                DirectCast(Me.atoms(i), Atom).moveInternal(paramInt1, paramInt2, paramDouble)
            Next
        End Sub

        Public Overridable Function convertInternal(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double) As Vector2D
            Dim localVector2D As New Vector2D()
            localVector2D.x = ((paramInt1 - Me.displayX) * paramDouble)
            localVector2D.x = ((Me.displayY - paramInt2) * paramDouble)
            Return localVector2D
        End Function

        Public Overridable Function convertInternal(paramDimension As DblRect, paramDouble As Double) As Vector2D
            Return convertInternal(paramDimension.Width, paramDimension.Height, paramDouble)
        End Function

        Public Overridable Sub autoRotate()
            Dim arrayOfDouble As Double() = moleculeRange()
            Dim d1 As Double = arrayOfDouble(3) - arrayOfDouble(0)
            Dim d2 As Double = arrayOfDouble(4) - arrayOfDouble(1)
            Dim d3 As Double = arrayOfDouble(5) - arrayOfDouble(2)
            Dim i As Integer
            Dim d4 As Double
            If (d1 > d2) AndAlso (d1 > d3) Then
                If d2 <= d3 Then
                    For i = 1 To Me.atoms.Count
                        d4 = getAtom(i).y
                        getAtom(i).y = getAtom(i).z
                        getAtom(i).z = (-d4)
                    Next
                End If
            ElseIf (d2 > d1) AndAlso (d2 > d3) Then
                If d1 > d3 Then
                    For i = 1 To Me.atoms.Count
                        d4 = getAtom(i).x
                        getAtom(i).x = getAtom(i).y
                        getAtom(i).y = (-d4)
                    Next
                Else
                    For i = 1 To Me.atoms.Count
                        d4 = getAtom(i).x
                        getAtom(i).x = getAtom(i).y
                        getAtom(i).y = getAtom(i).z
                        getAtom(i).z = d4
                    Next
                End If
            ElseIf d1 > d2 Then
                For i = 1 To Me.atoms.Count
                    d4 = getAtom(i).x
                    getAtom(i).x = getAtom(i).z
                    getAtom(i).z = getAtom(i).y
                    getAtom(i).y = d4
                Next
            Else
                For i = 1 To Me.atoms.Count
                    d4 = getAtom(i).x
                    getAtom(i).x = getAtom(i).z
                    getAtom(i).z = (-d4)
                Next
            End If
        End Sub

        Public Overrides Function refine() As Boolean
            Dim i As Integer = 0
            Dim j As Integer = 1
            If Me.atoms.Count = 0 Then
                DirectCast(Me.m_parent, Reaction).delObject(Me)
                Return False
            End If
            Me.atomflag = New Integer(Me.atoms.Count) {}
            For m As Integer = 0 To Me.atoms.Count
                Me.atomflag(m) = 0
            Next
            Me.bondflag = New Integer(Me.bonds.Count) {}
            For m = 0 To Me.bonds.Count
                Me.bondflag(m) = 0
            Next
            While j > 0
                i += 1
                Me.atomflag(j) = i
                searchConnection(j, i)
                j = 0
                For m = 1 To Me.atoms.Count
                    If Me.atomflag(m) = 0 Then
                        j = m
                        Exit For
                    End If
                Next
            End While
            Dim k As Integer = DirectCast(Me.m_parent, Reaction).getCategory(DirectCast(Me.m_parent, Reaction).getObjectNo(Me))
            Dim localVector As New ArrayList()
            Dim localObject As Object
            For n As Integer = 2 To i
                localObject = New Molecule()
                DirectCast(localObject, Molecule).set0point(get0point())
                DirectCast(localObject, Molecule).Parent = Parent
                For i1 As Integer = 1 To Me.atoms.Count
                    If Me.atomflag(i1) = n Then
                        Dim localAtom1 As Atom = getAtom(i1)
                        If localAtom1.Express_group Then
                            localVector.Add(localAtom1)
                        Else
                            DirectCast(localObject, Molecule).addAtom(localAtom1)
                            localAtom1.Mol = DirectCast(localObject, Molecule)
                        End If
                    End If
                Next
                For i1 = 1 To Me.bonds.Count
                    If Me.bondflag(i1) = n Then
                        DirectCast(localObject, Molecule).addBond(getBond(i1))
                        getBond(i1).Mol = DirectCast(localObject, Molecule)
                    End If
                Next
                DirectCast(localObject, Molecule).brackets.Clear()
                For i1 = 0 To Me.brackets.Count - 1
                    DirectCast(localObject, Molecule).brackets.Add(getBracket(i1 + 1))
                Next
                If ((TypeOf Me.m_parent Is Reaction)) AndAlso (DirectCast(localObject, Molecule).atoms.Count > 0) Then
                    DirectCast(Me.m_parent, Reaction).addObject(DirectCast(localObject, ChemObject), k)
                End If
            Next
            For n = Me.bonds.Count To 1 Step -1
                If Me.bondflag(n) <> 1 Then
                    Me.bonds.Remove(getBond(n))
                End If
            Next
            For n = Me.atoms.Count To 1 Step -1
                If Me.atomflag(n) <> 1 Then
                    localObject = getAtom(n)
                    If Not DirectCast(localObject, Atom).Mol.Equals(Me) Then
                        Me.atoms.Remove(localObject)
                    End If
                End If
            Next
            If Me.brackets IsNot Nothing Then
                n = 0
                While n > Me.brackets.Count
                    localObject = getBracket(n)
                    DirectCast(Me.m_parent, Reaction).addBracket(DirectCast(localObject, Bracket))
                    n += 1
                End While
            End If
            For n = 0 To localVector.Count - 1
                localObject = DirectCast(localVector(n), Atom)
                If DirectCast(localObject, Atom).GroupAtomSize <> 0 Then
                    Me.atoms.Remove(localObject)
                    Dim localMolecule1 As Molecule = DirectCast(localObject, Atom).getGroupAtom(0).Mol
                    If DirectCast(localObject, Atom).GroupPartnerSize > 0 Then
                        localMolecule1 = DirectCast(localObject, Atom).getGroupPartner(0).Mol
                    End If
                    Dim localAtom2 As Atom
                    For i2 As Integer = 0 To DirectCast(localObject, Atom).GroupAtomSize - 1
                        localAtom2 = DirectCast(localObject, Atom).getGroupAtom(i2)
                        Dim localMolecule2 As Molecule = localAtom2.Mol
                        If Not localMolecule2.Equals(localMolecule1) Then
                            DirectCast(localObject, Atom).removeGroupedAtom(localAtom2)
                            localAtom2.NonGroupedAtom = True
                            i2 -= 1
                        End If
                    Next
                    For i2 = 0 To DirectCast(localObject, Atom).GroupPartnerSize - 1
                        localAtom2 = DirectCast(localObject, Atom).getGroupPartner(i2)
                        If Not localAtom2.Mol.Equals(localMolecule1) Then
                            DirectCast(localObject, Atom).removeGroupPartnerAtom(localAtom2)
                            i2 -= 1
                        End If
                    Next
                    If DirectCast(localObject, Atom).GroupAtomSize <> 0 Then
                        localMolecule1.addAtom(DirectCast(localObject, Atom))
                        DirectCast(localObject, Atom).Mol = localMolecule1
                    End If
                End If
            Next
            setDBond()
            Return True
        End Function

        Friend Overridable Sub fixLength()
            Dim d1 As Double = 1.4
            Try
                Dim localDouble As New System.Nullable(Of Double)(DEBT.pref.[get]("COORDINATE_LENGTH"))
                d1 = CDbl(localDouble)
            Catch localNumberFormatException As NumberFormatException
            End Try
            Dim arrayOfDouble As Double() = New Double(Me.bonds.Count - 1) {}
            Dim d3 As Double = 0.0
            Dim d4 As Double = 0.0
            For i As Integer = 0 To Me.bonds.Count - 1
                Dim localBond As Bond = getBond(i + 1)
                arrayOfDouble(i) = VecMath2D.length(VecMath2D.subtract(localBond.Atom1.InCoordinate, localBond.Atom2.InCoordinate))
                d3 += arrayOfDouble(i)
                d4 += arrayOfDouble(i) * arrayOfDouble(i)
            Next
            d3 /= Me.bonds.Count
            d4 /= Me.bonds.Count
            Dim d5 As Double = d4 - d3 * d3
            Dim d6 As Double = 0.0
            Dim j As Integer = 0
            For k As Integer = 0 To arrayOfDouble.Length - 1
                d6 += arrayOfDouble(k)
                j += 1
            Next
            Dim d2 As Double
            If (d6 = 0.0) OrElse (d5 > 0.1) Then
                d2 = 1.0
            Else
                d2 = d1 / d6 * j
            End If
            Dim localObject As Object
            For k = 1 To Me.atoms.Count
                localObject = getAtom(k)
                DirectCast(localObject, Atom).zoom(d2)
            Next
            For k = 0 To Me.brackets.Count - 1
                localObject = DirectCast(Me.brackets(k), Bracket)
                DirectCast(localObject, Bracket).zoom(d2)
            Next
        End Sub

        Public Shared Function whichSide(paramBond As Bond) As Boolean
            Dim localAtom1 As Atom = paramBond.Atom1
            Dim localAtom2 As Atom = paramBond.Atom2
            Dim i As Integer = localAtom1.numBond()
            Dim j As Integer = localAtom2.numBond()
            Dim bool As Boolean = True
            Dim m As Integer
            Dim k As Integer = InlineAssignHelper(m, 0)
            Dim localVector2D1 As New Vector2D(localAtom2.x - localAtom1.x, localAtom2.y - localAtom1.y)
            Dim localBond As Bond
            Dim localAtom3 As Atom
            Dim localVector2D2 As Vector2D
            Dim d As Double
            For n As Integer = 0 To i - 1
                localBond = localAtom1.getBond(n)
                If localBond IsNot paramBond Then
                    localAtom3 = localBond.pairAtom(localAtom1)
                    localVector2D2 = New Vector2D(localAtom3.x - localAtom1.x, localAtom3.y - localAtom1.y)
                    d = VecMath2D.angle(localVector2D1, localVector2D2)
                    If (Math.Abs(d) >= 1.0) AndAlso (Math.Abs(d) <= 179.0) Then
                        If d > 0.0 Then
                            If (k = 0) OrElse (k = 1) Then
                                k += 2
                            End If
                        ElseIf (k = 0) OrElse (k = 2) Then
                            k += 1
                        End If
                    End If
                End If
            Next
            localVector2D1 = New Vector2D(localAtom1.x - localAtom2.x, localAtom1.y - localAtom2.y)
            For n = 0 To j - 1
                localBond = localAtom2.getBond(n)
                If localBond IsNot paramBond Then
                    localAtom3 = localBond.pairAtom(localAtom2)
                    localVector2D2 = New Vector2D(localAtom3.x - localAtom2.x, localAtom3.y - localAtom2.y)
                    d = VecMath2D.angle(localVector2D1, localVector2D2)
                    If (Math.Abs(d) >= 1.0) AndAlso (Math.Abs(d) <= 179.0) Then
                        If d > 0.0 Then
                            If (m = 0) OrElse (m = 2) Then
                                m += 1
                            End If
                        ElseIf (m = 0) OrElse (m = 1) Then
                            m += 2
                        End If
                    End If
                End If
            Next
            If (k = 1) OrElse (m = 1) Then
                bool = True
            End If
            If (k = 2) OrElse (m = 2) Then
                bool = False
            End If
            Return bool
        End Function

        Public Overridable Sub setDBond()
            If Not lockOfCheckRing Then
                checkRing()
            End If
            For n As Integer = 1 To Me.atoms.Count
                getAtom(n).setsp3(True)
            Next
            Dim localBond1 As Bond
            For n = 1 To Me.bonds.Count
                localBond1 = getBond(n)
                If (localBond1.Order = 2) OrElse (localBond1.Order = 3) OrElse (localBond1.Order = 4) Then
                    localBond1.Atom1.setsp3(False)
                    localBond1.Atom2.setsp3(False)
                End If
            Next
            For n = 1 To Me.bonds.Count
                localBond1 = getBond(n)
                If ((localBond1.Order = 2) OrElse (localBond1.Order = 4)) AndAlso (localBond1.Orientation < 10) Then
                    Dim localAtom1 As Atom = localBond1.Atom1
                    Dim localAtom2 As Atom = localBond1.Atom2
                    Dim i As Integer = localAtom1.numBond()
                    Dim j As Integer = localAtom2.numBond()
                    If (i = 1) OrElse (j = 1) Then
                        localBond1.Orientation = 2
                    Else
                        Dim m As Integer
                        Dim k As Integer
                        Dim localVector2D1 As Vector2D
                        Dim i1 As Integer
                        Dim localBond2 As Bond
                        Dim localAtom3 As Atom
                        Dim localVector2D2 As Vector2D
                        If localBond1.InRing Then
                            k = InlineAssignHelper(m, 0)
                            localVector2D1 = New Vector2D(localAtom2.x - localAtom1.x, localAtom2.y - localAtom1.y)
                            For i1 = 0 To i - 1
                                localBond2 = localAtom1.getBond(i1)
                                If (localBond2 IsNot localBond1) AndAlso (localBond2.InRing) Then
                                    localAtom3 = localBond2.pairAtom(localAtom1)
                                    localVector2D2 = New Vector2D(localAtom3.x - localAtom1.x, localAtom3.y - localAtom1.y)
                                    If VecMath2D.angle(localVector2D1, localVector2D2) > 0.0 Then
                                        If (k = 0) OrElse (k = 1) Then
                                            k += 2
                                        End If
                                    ElseIf (k = 0) OrElse (k = 2) Then
                                        k += 1
                                    End If
                                End If
                            Next
                            localVector2D1 = New Vector2D(localAtom1.x - localAtom2.x, localAtom1.y - localAtom2.y)
                            For i1 = 0 To j - 1
                                localBond2 = localAtom2.getBond(i1)
                                If (localBond2 IsNot localBond1) AndAlso (localBond2.InRing) Then
                                    localAtom3 = localBond2.pairAtom(localAtom2)
                                    localVector2D2 = New Vector2D(localAtom3.x - localAtom2.x, localAtom3.y - localAtom2.y)
                                    If VecMath2D.angle(localVector2D1, localVector2D2) > 0.0 Then
                                        If (m = 0) OrElse (m = 2) Then
                                            m += 1
                                        End If
                                    ElseIf (m = 0) OrElse (m = 1) Then
                                        m += 2
                                    End If
                                End If
                            Next
                            Select Case k And m
                                Case 0
                                    localBond1.Orientation = 1

                                Case 1
                                    localBond1.Orientation = 1

                                Case 2
                                    localBond1.Orientation = 3

                                Case 3
                                    localBond1.Orientation = 1

                            End Select
                        Else
                            k = InlineAssignHelper(m, 0)
                            localVector2D1 = New Vector2D(localAtom2.x - localAtom1.x, localAtom2.y - localAtom1.y)
                            For i1 = 0 To i - 1
                                localBond2 = localAtom1.getBond(i1)
                                If localBond2 IsNot localBond1 Then
                                    localAtom3 = localBond2.pairAtom(localAtom1)
                                    localVector2D2 = New Vector2D(localAtom3.x - localAtom1.x, localAtom3.y - localAtom1.y)
                                    If VecMath2D.angle(localVector2D1, localVector2D2) > 0.0 Then
                                        If (k = 0) OrElse (k = 1) Then
                                            k += 2
                                        End If
                                    ElseIf (k = 0) OrElse (k = 2) Then
                                        k += 1
                                    End If
                                End If
                            Next
                            localVector2D1 = New Vector2D(localAtom1.x - localAtom2.x, localAtom1.y - localAtom2.y)
                            For i1 = 0 To j - 1
                                localBond2 = localAtom2.getBond(i1)
                                If localBond2 IsNot localBond1 Then
                                    localAtom3 = localBond2.pairAtom(localAtom2)
                                    localVector2D2 = New Vector2D(localAtom3.x - localAtom2.x, localAtom3.y - localAtom2.y)
                                    If VecMath2D.angle(localVector2D1, localVector2D2) > 0.0 Then
                                        If (m = 0) OrElse (m = 2) Then
                                            m += 1
                                        End If
                                    ElseIf (m = 0) OrElse (m = 1) Then
                                        m += 2
                                    End If
                                End If
                            Next
                            Select Case k And m
                                Case 0
                                    localBond1.Orientation = 1

                                Case 1
                                    localBond1.Orientation = 1

                                Case 2
                                    localBond1.Orientation = 3

                                Case 3
                                    localBond1.Orientation = 1

                            End Select
                        End If
                    End If
                End If
            Next
        End Sub

        Public Overridable Sub lockOfCheckRing()
            lockOfCheckRing = True
        End Sub

        Public Overridable Sub unlockOfCheckRing()
            lockOfCheckRing = False
        End Sub

        Private Property LockOfCheckRing() As Boolean
            Get
                Return Me.__lockOfCheckRing
            End Get
            Set
                Me.__lockOfCheckRing = Value
            End Set
        End Property


        Private Sub checkRing()
            Dim i As Integer = -1
            Me.workflag = New Integer(Me.bonds.Count) {}
            Me.bondflag = New Integer(Me.bonds.Count) {}
            For j As Integer = 0 To Me.bonds.Count
                Me.bondflag(j) = 1
            Next
            For j = 1 To Me.bonds.Count
                If Me.bondflag(j) = 1 Then
                    terminalCheck(j)
                End If
            Next
            For j = 1 To Me.bonds.Count
                If (Me.bondflag(j) = 1) AndAlso (Not getBond(j).InRing) Then
                    i = j
                End If
            Next
            While i > 0
                For j = 1 To Me.bonds.Count
                    Me.workflag(j) = 0
                Next
                Me.workflag(i) = 1
                If Not serchRing(getBond(i).Atom1, getBond(i).Atom2, 2) Then
                    Me.bondflag(i) = 0
                    terminalCheck(i)
                End If
                i = -1
                For j = 1 To Me.bonds.Count
                    If (Me.bondflag(j) = 1) AndAlso (Not getBond(j).InRing) Then
                        i = j
                    End If
                Next
            End While
        End Sub

        Private Function serchRing(paramAtom1 As Atom, paramAtom2 As Atom, paramInt As Integer) As Boolean
            If paramInt > 8 Then
                Return False
            End If
            For i As Integer = 1 To Me.bonds.Count
                If (Me.bondflag(i) <> 0) AndAlso (Me.workflag(i) = 0) AndAlso (getBond(i).hasAtom(paramAtom2)) Then
                    If getBond(i).hasAtom(paramAtom1) Then
                        For j As Integer = 1 To Me.bonds.Count
                            If Me.workflag(j) <> 0 Then
                                getBond(j).InRing = True
                            End If
                        Next
                        Return True
                    End If
                    Me.workflag(i) = paramInt
                    If serchRing(paramAtom1, getBond(i).pairAtom(paramAtom2), paramInt + 1) Then
                        Return True
                    End If
                    Me.workflag(i) = 0
                End If
            Next
            Return False
        End Function

        Private Sub terminalCheck(paramInt As Integer)
            Dim i As Integer = InlineAssignHelper(j, 0)
            Dim localBond As Bond = getBond(paramInt)
            Dim localAtom1 As Atom = localBond.Atom1
            Dim localAtom2 As Atom = localBond.Atom2
            i = localAtom1.numBond()
            Dim j As Integer = localAtom2.numBond()
            If (i = 1) AndAlso (j = 1) Then
                Me.bondflag(paramInt) = 0
                Return
            End If
            If i = 1 Then
                Me.bondflag(paramInt) = 0
                If j = 2 Then
                    If localAtom2.getBond(0).pairAtom(localAtom2) Is localAtom1 Then
                        If Me.bondflag(getBondNo(localAtom2.getBond(1))) = 1 Then
                            terminalCheck(getBondNo(localAtom2.getBond(1)))
                        End If
                    ElseIf Me.bondflag(getBondNo(localAtom2.getBond(0))) = 1 Then
                        terminalCheck(getBondNo(localAtom2.getBond(0)))
                    End If
                End If
            End If
            If j = 1 Then
                Me.bondflag(paramInt) = 0
                If i = 2 Then
                    If localAtom1.getBond(0).pairAtom(localAtom1) Is localAtom2 Then
                        If Me.bondflag(getBondNo(localAtom1.getBond(1))) = 1 Then
                            terminalCheck(getBondNo(localAtom1.getBond(1)))
                        End If
                    ElseIf Me.bondflag(getBondNo(localAtom1.getBond(0))) = 1 Then
                        terminalCheck(getBondNo(localAtom1.getBond(0)))
                    End If
                End If
            End If
        End Sub

        Private Sub searchConnection(paramInt1 As Integer, paramInt2 As Integer)
            Dim localAtom As Atom = getAtom(paramInt1)
            If localAtom Is Nothing Then
                Return
            End If
            Dim i As Integer = localAtom.numBond()
            For k As Integer = 0 To i - 1
                Dim localBond As Bond = localAtom.getBond(k)
                Me.bondflag(getBondNo(localBond)) = paramInt2
                Dim j As Integer = getAtomNo(localBond.pairAtom(localAtom))
                If Me.atomflag(j) = 0 Then
                    Me.atomflag(j) = paramInt2
                    searchConnection(j, paramInt2)
                End If
            Next
        End Sub

        Public Overrides Sub rescale(paramDouble As Double)
            Dim localVector As New ArrayList()
            For i As Integer = 1 To Me.bonds.Count
                Dim localBond As Bond = getBond(i)
                localVector.Add(New System.Nullable(Of Double)(localBond.length()))
            Next
            Dim d As Double = keg.common.util.DEBTutil.availableMean(localVector)
            If d = 0.0 Then
                Return
            End If
            For j As Integer = 1 To Me.atoms.Count
                Dim localAtom As Atom = getAtom(j)
                localAtom.x *= paramDouble / d
                localAtom.y *= paramDouble / d
                localAtom.z *= paramDouble / d
            Next
        End Sub

        Public Overrides Function nearAtom(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double, paramInt3 As Integer) As Atom
            Return nearAtom(paramInt1, paramInt2, paramDouble, paramInt3, False)
        End Function

        Public Overrides Function nearAtom(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double, paramInt3 As Integer, paramBoolean As Boolean) As Atom
            Dim localAtom As Atom = Nothing
            For i As Integer = 1 To Me.atoms.Count
                localAtom = getAtom(i)
                If If(paramBoolean, localAtom.NonGroupedAtom, Not localAtom.Express_group) Then
                    Dim j As Integer = If(localAtom.Express_group, 2, 1)
                    If (Math.Abs(localAtom.DX(paramDouble) - paramInt1) <= paramInt3 * j) AndAlso (Math.Abs(localAtom.DY(paramDouble) - paramInt2) <= paramInt3) Then
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
            For i As Integer = 1 To Me.bonds.Count
                localBond = getBond(i)
                If (Not paramBoolean) OrElse (localBond.NonGroupedBond) Then
                    Dim localAtom1 As Atom = localBond.Atom1
                    Dim localAtom2 As Atom = localBond.Atom2
                    If VecMath2D.isNear(localAtom1.DX(paramDouble), localAtom1.DY(paramDouble), localAtom2.DX(paramDouble), localAtom2.DY(paramDouble), paramInt1, paramInt2,
                        paramInt3) Then
                        Return localBond
                    End If
                End If
            Next
            Return Nothing
        End Function

        Public Overrides Function moleculeRange() As Double()
            Dim arrayOfDouble As Double() = New Double(5) {}
            arrayOfDouble(0) = getAtom(1).x
            arrayOfDouble(1) = getAtom(1).y
            arrayOfDouble(2) = getAtom(1).z
            arrayOfDouble(3) = getAtom(1).x
            arrayOfDouble(4) = getAtom(1).y
            arrayOfDouble(5) = getAtom(1).z
            For i As Integer = 2 To Me.atoms.Count
                If arrayOfDouble(0) > getAtom(i).x Then
                    arrayOfDouble(0) = getAtom(i).x
                End If
                If arrayOfDouble(1) > getAtom(i).y Then
                    arrayOfDouble(1) = getAtom(i).y
                End If
                If arrayOfDouble(2) > getAtom(i).z Then
                    arrayOfDouble(2) = getAtom(i).z
                End If
                If arrayOfDouble(3) < getAtom(i).x Then
                    arrayOfDouble(3) = getAtom(i).x
                End If
                If arrayOfDouble(4) < getAtom(i).y Then
                    arrayOfDouble(4) = getAtom(i).y
                End If
                If arrayOfDouble(5) < getAtom(i).z Then
                    arrayOfDouble(5) = getAtom(i).z
                End If
            Next
            Return arrayOfDouble
        End Function

        Public Overridable Overloads Function moleculeRange(paramDouble As Double) As Integer()
            Dim arrayOfInt As Integer() = {0, 0, 0, 0}
            If AtomNum > 0 Then
                arrayOfInt(0) = getAtom(1).DX(paramDouble)
                arrayOfInt(1) = getAtom(1).DY(paramDouble)
                arrayOfInt(2) = getAtom(1).DX(paramDouble)
                arrayOfInt(3) = getAtom(1).DY(paramDouble)
                For i As Integer = 2 To AtomNum
                    If arrayOfInt(0) > getAtom(i).DX(paramDouble) Then
                        arrayOfInt(0) = getAtom(i).DX(paramDouble)
                    End If
                    If arrayOfInt(1) > getAtom(i).DY(paramDouble) Then
                        arrayOfInt(1) = getAtom(i).DY(paramDouble)
                    End If
                    If arrayOfInt(2) < getAtom(i).DX(paramDouble) Then
                        arrayOfInt(2) = getAtom(i).DX(paramDouble)
                    End If
                    If arrayOfInt(3) < getAtom(i).DY(paramDouble) Then
                        arrayOfInt(3) = getAtom(i).DY(paramDouble)
                    End If
                Next
            End If
            Return arrayOfInt
        End Function

        Private Function moleculeRangeSub(paramInt As Integer, paramFontMetrics As java.awt.FontMetrics, paramDouble As Double) As Integer()
            Dim i As Integer = paramFontMetrics.Height - 2
            Dim localDimension As DblRect = Nothing
            Dim arrayOfInt As Integer() = {0, 0, 0, 0}
            Dim localAtom As Atom = getAtom(paramInt)
            Dim str As String = localAtom.Label
            If (str.Length = 0) OrElse (str.Equals("C")) Then
                localDimension = New DblRect(0, 0)
            Else
                localDimension = New DblRect(paramFontMetrics.stringWidth(str), i)
            End If
            arrayOfInt(0) = (getAtom(paramInt).DX(paramDouble) - localDimension.Width / 2)
            arrayOfInt(1) = (getAtom(paramInt).DY(paramDouble) - localDimension.Height / 2)
            arrayOfInt(2) = (getAtom(paramInt).DX(paramDouble) + localDimension.Width / 2)
            arrayOfInt(3) = (getAtom(paramInt).DY(paramDouble) + localDimension.Height / 2)
            Return arrayOfInt
        End Function

        Public Overridable Overloads Function moleculeRange(paramFontMetrics As java.awt.FontMetrics, paramDouble As Double) As Integer()
            Dim arrayOfInt1 As Integer() = {0, 0, 0, 0}
            If AtomNum > 0 Then
                Dim arrayOfInt2 As Integer() = moleculeRangeSub(1, paramFontMetrics, paramDouble)
                arrayOfInt1(0) = arrayOfInt2(0)
                arrayOfInt1(1) = arrayOfInt2(1)
                arrayOfInt1(2) = arrayOfInt2(2)
                arrayOfInt1(3) = arrayOfInt2(3)
                For i As Integer = 2 To AtomNum
                    arrayOfInt2 = moleculeRangeSub(i, paramFontMetrics, paramDouble)
                    If arrayOfInt1(0) > arrayOfInt2(0) Then
                        arrayOfInt1(0) = arrayOfInt2(0)
                    End If
                    If arrayOfInt1(1) > arrayOfInt2(1) Then
                        arrayOfInt1(1) = arrayOfInt2(1)
                    End If
                    If arrayOfInt1(2) < arrayOfInt2(2) Then
                        arrayOfInt1(2) = arrayOfInt2(2)
                    End If
                    If arrayOfInt1(3) < arrayOfInt2(3) Then
                        arrayOfInt1(3) = arrayOfInt2(3)
                    End If
                Next
            End If
            Return arrayOfInt1
        End Function

        Public Overridable Sub resetKEGGAtomName()
            For i As Integer = 1 To Me.atoms.Count
                Dim localAtom As Atom = getAtom(i)
                Dim str As String = localAtom.Label
                If str.Length = 0 Then
                    str = "C"
                End If
                localAtom.KEGGAtomName = str
            Next
        End Sub

        Public Overridable WriteOnly Property Scale() As Double
            Set
                Me.m_scale = Value
                Me.dispscale = Value
            End Set
        End Property

        Public Overridable Sub resetOpoint(paramDouble As Double)
            Scale = paramDouble
            For i As Integer = 0 To AtomNum - 1
                Dim localAtom As Atom = getAtom(i + 1)
                localAtom.Recalc = True
                Dim localDimension As DblRect = localAtom.getCoordinate(paramDouble)
                localAtom.x = (localDimension.Width / paramDouble)
                localAtom.y = (localDimension.Height / paramDouble)
            Next
            set0point(New DblRect(0, 0))
        End Sub

        Public Overrides Sub flipHorizontal(paramDouble1 As Double, paramDouble2 As Double)
            For i As Integer = 0 To AtomNum - 1
                Dim localAtom As Atom = getAtom(i + 1)
                localAtom.flipHorizontal(paramDouble1, paramDouble2)
            Next
        End Sub

        Public Overridable Sub flipHorizontalIfSelected(paramDouble1 As Double, paramDouble2 As Double)
            For i As Integer = 0 To AtomNum - 1
                Dim localAtom As Atom = getAtom(i + 1)
                If localAtom.[select] Then
                    localAtom.flipHorizontal(paramDouble1, paramDouble2)
                End If
            Next
            flipStereoTypeofBonds()
        End Sub

        Public Overrides Sub flipVertical(paramDouble1 As Double, paramDouble2 As Double)
            For i As Integer = 0 To AtomNum - 1
                Dim localAtom As Atom = getAtom(i + 1)
                localAtom.flipVertical(paramDouble1, paramDouble2)
            Next
        End Sub

        Public Overridable Sub flipVerticalIfSelected(paramDouble1 As Double, paramDouble2 As Double)
            For i As Integer = 0 To AtomNum - 1
                Dim localAtom As Atom = getAtom(i + 1)
                If localAtom.[select] Then
                    localAtom.flipVertical(paramDouble1, paramDouble2)
                End If
            Next
            flipStereoTypeofBonds()
        End Sub

        Private Sub flipStereoTypeofBonds()
            For i As Integer = 0 To BondNum - 1
                Dim localBond As Bond = getBond(i + 1)
                If localBond.[select] Then
                    If localBond.Stereo = 1 Then
                        localBond.Stereo = 6
                    ElseIf localBond.Stereo = 6 Then
                        localBond.Stereo = 1
                    End If
                End If
            Next
        End Sub

        Public Overridable Sub doFixedLength()
            fixLength()
        End Sub

        Public Overridable Sub removeBracket(paramBracket As Bracket)
            Me.brackets.Remove(paramBracket)
            paramBracket.removeMol(Me)
        End Sub

        Public Overridable Sub setBracketCoordinate(paramInt As Integer, paramDouble1 As Double, paramDouble2 As Double, paramDouble3 As Double, paramDouble4 As Double)
            Dim localBracket As Bracket = getBracket(paramInt)
            If localBracket IsNot Nothing Then
                localBracket.setCoordinate(paramDouble1, paramDouble2, paramDouble3, paramDouble4)
                If localBracket.setSDI > 1 Then
                    confirmeIntersectionOfBondsAndBracketLine(localBracket)
                End If
            End If
        End Sub

        Protected Friend Overridable Function confirmeIntersectionOfBondsAndBracketLine(paramBracket As Bracket) As Boolean
            Dim bool1 As Boolean = False
            For i As Integer = 0 To BondNum - 1
                Dim localBond As Bond = getBond(i + 1)
                Dim bool2 As Boolean = paramBracket.confirm_Bond(localBond)
                If bool2 = True Then
                    addBracket(paramBracket)
                    bool1 = True
                End If
            Next
            paramBracket.computeParentMol()
            Return bool1
        End Function

        Public Overridable Sub addBracket(paramBracket As Bracket)
            If Not Me.brackets.Contains(paramBracket) Then
                For i As Integer = 0 To Me.brackets.Count - 1
                    Dim localBracket As Bracket = DirectCast(Me.brackets(i), Bracket)
                    If Reaction.compareBracket(localBracket, paramBracket) = True Then
                        removeBracket(localBracket)
                    End If
                Next
                Me.brackets.Add(paramBracket)
                paramBracket.addMol(Me)
                DirectCast(Me.m_parent, Reaction).addBracket(paramBracket)
            End If
        End Sub

        Public Overridable Function hasTheBracket(paramBracket As Bracket) As Boolean
            Return Me.brackets.Contains(paramBracket)
        End Function

        <MethodImpl(MethodImplOptions.Synchronized)>
        Private Function getAtomsInBracket(paramBracket As Bracket) As ArrayList
            If AtomNum = 0 Then
                Return Nothing
            End If
            Dim arrayOfInt As Integer() = New Integer(AtomNum - 1) {}
            For i As Integer = 0 To AtomNum - 1
                arrayOfInt(i) = -1
            Next
            If (paramBracket.bonds1 IsNot Nothing) AndAlso (paramBracket.bonds1.Count > 0) Then
                localBond = DirectCast(paramBracket.bonds1(0), Bond)
                paramBracket.SBL1 = getBondNo(localBond)
            End If
            If (paramBracket.bonds2 IsNot Nothing) AndAlso (paramBracket.bonds2.Count > 0) Then
                localBond = DirectCast(paramBracket.bonds2(0), Bond)
                paramBracket.SBL2 = getBondNo(localBond)
            End If
            Dim localBond As Bond = Nothing
            Dim localAtom As Atom = Nothing
            If (paramBracket.bonds1 IsNot Nothing) AndAlso (paramBracket.bonds1.Count > 0) Then
                localBond = DirectCast(paramBracket.bonds1(0), Bond)
                localAtom = If(paramBracket.contains(localBond.Atom1), localBond.Atom1, localBond.Atom2)
            ElseIf (paramBracket.bonds2 IsNot Nothing) AndAlso (paramBracket.bonds2.Count > 0) Then
                localBond = DirectCast(paramBracket.bonds2(0), Bond)
                localAtom = If(paramBracket.contains(localBond.Atom1), localBond.Atom1, localBond.Atom2)
            End If
            Dim localVector As New ArrayList()
            Dim j As Integer
            If (localAtom IsNot Nothing) AndAlso (getAtomNo(localAtom) > 0) Then
                arrayOfInt((getAtomNo(localAtom) - 1)) = 1
                searchConnection_For_Bracket_Sub(paramBracket, localAtom, arrayOfInt)
                For j = 0 To AtomNum - 1
                    If arrayOfInt(j) = 1 Then
                        localVector.Add(getAtom(j + 1))
                    End If
                Next
            Else
                For j = 0 To AtomNum - 1
                    localVector.Add(getAtom(j + 1))
                Next
            End If
            Return localVector
        End Function

        <MethodImpl(MethodImplOptions.Synchronized)>
        Private Sub searchConnection_For_Bracket_Sub(paramBracket As Bracket, paramAtom As Atom, paramArrayOfInt As Integer())
            Dim i As Integer = paramArrayOfInt((getAtomNo(paramAtom) - 1))
            Dim localVector As ArrayList = paramBracket.Bonds
            For j As Integer = 0 To paramAtom.numBond() - 1
                Dim k As Integer = i
                Dim localBond As Bond = paramAtom.getBond(j)
                If localVector.Contains(localBond) Then
                    k = If(k = 0, 1, 0)
                End If
                Dim localAtom As Atom = localBond.pairAtom(paramAtom)
                If paramArrayOfInt((getAtomNo(localAtom) - 1)) = -1 Then
                    paramArrayOfInt((getAtomNo(localAtom) - 1)) = k
                    searchConnection_For_Bracket_Sub(paramBracket, localAtom, paramArrayOfInt)
                End If
            Next
        End Sub

        Public Overridable Sub setSgroup()
            For i As Integer = 0 To Me.brackets.Count - 1
                Dim localBracket As Bracket = DirectCast(Me.brackets(i), Bracket)
                localBracket.SBL1 = 0
                localBracket.SBL2 = 0
                If Not localBracket.CoveredWholeMol Then
                    localBracket.clearSgroup()
                    Dim localVector As ArrayList = getAtomsInBracket(localBracket)
                    localBracket.addSgroup(localVector)
                End If
                If (TypeOf Me.m_parent Is Reaction) Then
                    DirectCast(Me.m_parent, Reaction).setAtomsIntoBracketRectangle(localBracket, Me)
                End If
            Next
        End Sub

        Friend Overridable Function getAtomsList(paramAtom As Atom) As ArrayList
            Dim localVector As New ArrayList()
            localVector.Add(paramAtom)
            getAtomsList(paramAtom, localVector)
            Return localVector
        End Function

        Private Sub getAtomsList(paramAtom As Atom, paramVector As ArrayList)
            For i As Integer = 0 To paramAtom.numBond() - 1
                Dim localAtom As Atom = paramAtom.getBond(i).pairAtom(paramAtom)
                If Not paramVector.Contains(localAtom) Then
                    paramVector.Add(localAtom)
                    getAtomsList(localAtom, paramVector)
                End If
            Next
        End Sub

        Public Overridable Function getExpressionAtomWithGroupedAtom(paramAtom As Atom) As Atom
            If Not paramAtom.Mol.Equals(Me) Then
                Return Nothing
            End If
            For i As Integer = 0 To AtomNum - 1
                Dim localAtom As Atom = getAtom(i + 1)
                If (localAtom.Express_group) AndAlso (localAtom.containsInGroupedAtoms(paramAtom)) Then
                    Return localAtom
                End If
            Next
            Return Nothing
        End Function
        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
    End Class


    ' Location:              C:\Users\xieguigang\Downloads\KegDraw-0_1_14Beta\lib\KegDraw.jar!\keg\compound\Molecule.class
    '	 * Java compiler version: 6 (50.0)
    '	 * JD-Core Version:       0.7.1
    '	 

End Namespace
