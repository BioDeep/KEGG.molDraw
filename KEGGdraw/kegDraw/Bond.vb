#Region "Microsoft.VisualBasic::3239346520e31de23f484aadc62aa170, KCF\KEGGdraw\kegDraw\Bond.vb"

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

    '     Class Bond
    ' 
    '         Properties: [Select], Atom1, Atom2, Color, ConnectedBond
    '                     HighLight, ID, InRing, Ionic, Mol
    '                     NonGroupedBond, Order, Orientation, Recalc, SideWithMoreConnections
    '                     Stereo
    ' 
    '         Constructor: (+5 Overloads) Sub New
    ' 
    '         Function: (+2 Overloads) calcInside, calcOutside, (+2 Overloads) calcSidePoint, (+3 Overloads) coordinate, hasAtom
    '                   length, nearSide, pairAtom, (+2 Overloads) toKCFString, ToString
    ' 
    '         Sub: (+4 Overloads) [select], breakBond, calcImplicitHydrogen, decisideHydrogenDraw, (+2 Overloads) recalcCoordinate
    '              (+2 Overloads) select_reverse, swapAtom, (+2 Overloads) unselect
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Collections
Imports System.Drawing
Imports System.Text
Imports Microsoft.VisualBasic.Imaging.LayoutModel

Namespace keg.compound

    Public Class Bond

        Public Const STEREO_DOWN_STRING As String = "Down"
        Public Const STEREO_UP_STRING As String = "Up"
        Public Const STEREO_EITHER_STRING As String = "Either"
        Public Const Order_Single As Integer = 1
        Public Const Order_Double As Integer = 2
        Public Const Order_Triple As Integer = 3
        Public Const Order_Aromatic As Integer = 4
        Public Const Order_SorD As Integer = 5
        Public Const Order_SorA As Integer = 6
        Public Const Order_DorA As Integer = 7
        Public Const Order_Any As Integer = 8
        Public Const Order_Coordinate As Integer = -1
        Public Const Order_Dummy As Integer = 0
        Public Const Stereo_None As Integer = 0
        Public Const Stereo_Up As Integer = 1
        Public Const Stereo_Down As Integer = 6
        Public Const Stereo_Either As Integer = 4
        Public Const Stereo_Double As Integer = 3
        Public Const Orient_Undefined As Integer = 0
        Public Const Orient_Right As Integer = 1
        Public Const Orient_Center As Integer = 2
        Public Const Orient_Left As Integer = 3
        Public Const Orient_User As Integer = 10

        Private __select As Boolean = False
        Private m_atom1 As Atom = Nothing
        Private m_atom2 As Atom = Nothing
        Private m_order As Integer = 0
        Private m_stereo As Integer = 0
        Private m_orientation As Integer = 2
        Private m_inRing As Boolean = False
        Private m_mol As Molecule = Nothing
        Public col As Color = Nothing
        Public m_id As String = ""
        Private m_highLight As Integer = 0
        Private m_recalc As Boolean = True
        Private xyPoints As New ArrayList()
        Public Const RIGHT_SIDE As Integer = 1
        Public Const LEFT_SIDE As Integer = 2

        Public Sub New()
        End Sub

        Public Sub New(paramMolecule As Molecule, paramString As String)
            Me.m_mol = paramMolecule
            Try
                Dim str As String = paramString.Substring(0, 3)
                Dim i As Integer = Convert.ToInt32(str.Trim())
                Me.m_atom1 = paramMolecule.getAtom(i)
                str = paramString.Substring(3, 3)
                i = Convert.ToInt32(str.Trim())
                Me.m_atom2 = paramMolecule.getAtom(i)
                str = paramString.Substring(6, 3)
                Dim j As Integer = Convert.ToInt32(str.Trim())
                Order = j
                str = paramString.Substring(9, 3)
                Me.m_stereo = Convert.ToInt32(str.Trim())
            Catch
                Console.WriteLine(paramString)
                Throw New Exception("Format error in Bond Block.")
            End Try
            If Me.m_atom1 Is Me.m_atom2 Then
                Console.WriteLine(paramString)
                Throw New Exception("Format error in Bond Block.")
            End If
            If (Me.m_atom1 Is Nothing) OrElse (Me.m_atom2 Is Nothing) Then
                Console.WriteLine(paramString)
                Throw New Exception("Format error in Bond Block.")
            End If
            Me.m_atom1.makeBond(Me)
            Me.m_atom2.makeBond(Me)
        End Sub

        Public Sub New(paramMolecule As Molecule, paramAtom1 As Atom, paramAtom2 As Atom)
            If paramAtom1 Is paramAtom2 Then
                Throw New Exception("Format error in Bond Block.")
            End If
            Me.m_mol = paramMolecule
            Me.m_atom1 = paramAtom1
            Me.m_atom2 = paramAtom2
            Me.m_atom1.makeBond(Me)
            Me.m_atom2.makeBond(Me)
        End Sub

        Public Sub New(paramMolecule As Molecule, paramAtom1 As Atom, paramAtom2 As Atom, paramInt As Integer)
            Me.New(paramMolecule, paramAtom1, paramAtom2)
            Order = paramInt
            If (paramInt = 2) OrElse (paramInt = 3) OrElse (paramInt = 4) Then
                paramAtom1.setsp3(False)
                paramAtom2.setsp3(False)
            End If
        End Sub

        Public Sub New(paramMolecule As Molecule, paramAtom1 As Atom, paramAtom2 As Atom, paramInt1 As Integer, paramInt2 As Integer, paramInt3 As Integer)
            Me.New(paramMolecule, paramAtom1, paramAtom2, paramInt1)
            Me.m_stereo = paramInt2
            Me.m_orientation = paramInt3
        End Sub

        Public Overridable Property ID() As String
            Get
                Return Me.m_id
            End Get
            Set
                Me.m_id = Value
            End Set
        End Property


        Public Overridable Property Color() As Color
            Get
                Return Me.col
            End Get
            Set
                Me.col = Value
            End Set
        End Property


        Public Overridable Property HighLight() As Integer
            Get
                Return Me.m_highLight
            End Get
            Set
                Me.m_highLight = Value
            End Set
        End Property


        Public Overridable Property Mol() As Molecule
            Get
                Return Me.m_mol
            End Get
            Set
                Me.m_mol = Value
            End Set
        End Property


        Public Overridable WriteOnly Property Recalc() As Boolean
            Set
                Me.m_recalc = Value
            End Set
        End Property

        Public Overridable Sub [select](paramBoolean As Boolean)
            Me.__select = paramBoolean
        End Sub

        Public Overridable Sub [select](paramBoolean As Boolean, paramEditMode As EditMode)
            If paramBoolean Then
                If Not paramEditMode.selected.Contains(Me) Then
                    paramEditMode.selected.Add(Me)
                End If
            ElseIf paramEditMode.selected.Contains(Me) Then
                paramEditMode.selected.Remove(Me)
            End If
            Me.__select = paramBoolean
        End Sub

        Public Overridable Sub [select]()
            [select](True)
        End Sub

        Public Overridable Sub [select](paramEditMode As EditMode)
            [select](True, paramEditMode)
        End Sub

        Public Overridable Sub unselect()
            [select](False)
        End Sub

        Public Overridable Sub unselect(paramEditMode As EditMode)
            [select](False, paramEditMode)
        End Sub

        Public Overridable Sub select_reverse()
            Me.__select = (Not Me.__select)
        End Sub

        Public Overridable Sub select_reverse(paramEditMode As EditMode)
            Me.__select = (Not Me.__select)
            If Me.__select Then
                If Not paramEditMode.selected.Contains(Me) Then
                    paramEditMode.selected.Add(Me)
                End If
            ElseIf paramEditMode.selected.Contains(Me) Then
                paramEditMode.selected.Remove(Me)
            End If
        End Sub

        Public Overridable ReadOnly Property [Select]() As Boolean
            Get
                Return Me.__select
            End Get
        End Property

        Public Overridable Property Atom1() As Atom
            Get
                Return Me.m_atom1
            End Get
            Set
                If Me.m_atom1 IsNot Nothing Then
                    Me.m_atom1.breakBond(Me)
                End If
                Me.m_atom1 = Value
                Value.makeBond(Me)
            End Set
        End Property


        Public Overridable Property Atom2() As Atom
            Get
                Return Me.m_atom2
            End Get
            Set
                If Me.m_atom2 IsNot Nothing Then
                    Me.m_atom2.breakBond(Me)
                End If
                Me.m_atom2 = Value
            End Set
        End Property


        Public Overridable Function hasAtom(paramAtom As Atom) As Boolean
            Return (Me.m_atom1 Is paramAtom) OrElse (Me.m_atom2 Is paramAtom)
        End Function

        Public Overridable Function pairAtom(paramAtom As Atom) As Atom
            If Me.m_atom1 Is paramAtom Then
                Return Me.m_atom2
            End If
            If Me.m_atom2 Is paramAtom Then
                Return Me.m_atom1
            End If
            Return DirectCast(Nothing, Atom)
        End Function

        Public Overridable Sub swapAtom()
            Dim localAtom As Atom = Me.m_atom1
            Me.m_atom1 = Me.m_atom2
            Me.m_atom2 = localAtom
        End Sub

        Public Overridable Property Order() As Integer
            Get
                Return Me.m_order
            End Get
            Set
                If Me.m_order <> Value Then
                    Me.m_order = Value
                    Select Case Me.m_order
                        Case 2
                            Me.m_orientation = 0
                            Me.m_atom1.setsp3(False)
                            Me.m_atom2.setsp3(False)
                        Case 3, 4
                            Me.m_atom1.setsp3(False)
                            Me.m_atom2.setsp3(False)

                        Case 1
                            Me.m_stereo = 0
                            Me.m_atom1.setsp3(True)
                            Me.m_atom2.setsp3(True)
                            For i As Integer = 0 To Me.m_atom1.numBond() - 1
                                If (Me.m_atom1.getBond(i).Order = 2) OrElse (Me.m_atom1.getBond(i).Order = 3) OrElse (Me.m_atom1.getBond(i).Order = 4) Then
                                    Me.m_atom1.setsp3(False)
                                    Exit For
                                End If
                            Next
                            For i = 0 To Me.m_atom2.numBond() - 1
                                If (Me.m_atom2.getBond(i).Order = 2) OrElse (Me.m_atom2.getBond(i).Order = 3) OrElse (Me.m_atom2.getBond(i).Order = 4) Then
                                    Me.m_atom2.setsp3(False)
                                    Exit For
                                End If
                            Next

                    End Select
                    Me.m_recalc = True
                    Me.m_atom1.setNumberOfConnections()
                    Me.m_atom2.setNumberOfConnections()
                End If
            End Set
        End Property


        Public Overridable Property Stereo() As Integer
            Get
                Return Me.m_stereo
            End Get
            Set
                Me.m_stereo = Value
                Me.m_recalc = True
            End Set
        End Property


        Public Overridable Function length() As Double
            Return Math.Sqrt((Me.m_atom1.x - Me.m_atom2.x) * (Me.m_atom1.x - Me.m_atom2.x) + (Me.m_atom1.y - Me.m_atom2.y) * (Me.m_atom1.y - Me.m_atom2.y) + (Me.m_atom1.z - Me.m_atom2.z) * (Me.m_atom1.z - Me.m_atom2.z))
        End Function

        Public Overridable Property Orientation() As Integer
            Get
                Return Me.m_orientation
            End Get
            Set
                Me.m_orientation = Value
                Me.m_recalc = True
            End Set
        End Property


        Public Overridable Property InRing() As Boolean
            Get
                Return Me.m_inRing
            End Get
            Set
                Me.m_inRing = Value
            End Set
        End Property


        Public Overridable Sub calcImplicitHydrogen()
            Me.m_atom1.calcImplicitHydrogen()
            Me.m_atom2.calcImplicitHydrogen()
        End Sub

        Public Overridable Sub decisideHydrogenDraw()
            Me.m_atom1.decisideHydrogenDraw()
            Me.m_atom2.decisideHydrogenDraw()
        End Sub

        Public Overridable Sub breakBond()
        End Sub

        Public Overridable ReadOnly Property SideWithMoreConnections() As Atom
            Get
                Return If(Me.m_atom1.NumberOfConnections > Me.m_atom2.NumberOfConnections, Me.m_atom1, Me.m_atom2)
            End Get
        End Property

        Public Overridable Function nearSide(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double) As Atom
            Dim localAtom As Atom = Nothing
            If Math.Abs(Me.m_atom1.DX(paramDouble) - Me.m_atom2.DX(paramDouble)) > Math.Abs(Me.m_atom1.DY(paramDouble) - Me.m_atom2.DY(paramDouble)) Then
                If Math.Abs(Me.m_atom1.DX(paramDouble) - paramInt1) > Math.Abs(Me.m_atom2.DX(paramDouble) - paramInt1) Then
                    localAtom = Me.m_atom2
                Else
                    localAtom = Me.m_atom1
                End If
            ElseIf Math.Abs(Me.m_atom1.DY(paramDouble) - paramInt2) > Math.Abs(Me.m_atom2.DY(paramDouble) - paramInt2) Then
                localAtom = Me.m_atom2
            Else
                localAtom = Me.m_atom1
            End If
            Return localAtom
        End Function

        Public Overridable Function coordinate(paramDouble As Double, paramInt1 As Integer, paramInt2 As Integer) As ArrayList
            If Me.m_recalc Then
                recalcCoordinate(paramDouble, paramInt1, paramInt2, 1)
            End If
            Return Me.xyPoints
        End Function

        Public Overridable Function coordinate(paramDouble As Double, paramInt1 As Integer, paramInt2 As Integer, paramInt3 As Integer) As ArrayList
            If Me.m_recalc Then
                recalcCoordinate(paramDouble, paramInt1, paramInt2, paramInt3)
            End If
            Return Me.xyPoints
        End Function

        Public Overridable Function coordinate(paramDouble1 As Double, paramInt As Integer, paramDouble2 As Double) As ArrayList
            If Me.m_recalc Then
                recalcCoordinate(paramDouble1, paramInt, paramDouble2)
            End If
            Return Me.xyPoints
        End Function

        Private Sub recalcCoordinate(paramDouble As Double, paramInt1 As Integer, paramInt2 As Integer, paramInt3 As Integer)
            Me.xyPoints.Clear()
            Dim localDimension1 As DblRect = Me.m_atom1.getCoordinate(paramDouble)
            Dim localDimension2 As DblRect = Me.m_atom2.getCoordinate(paramDouble)
            Dim localDimension4 As DblRect = Nothing
            Dim localDimension3 As DblRect = Nothing
            Dim i As Integer
            Dim j As Integer
            Dim localDimension5 As DblRect
            Select Case Me.m_stereo
                Case 4
                    Me.xyPoints.Add(localDimension2)
                    i = CInt(Math.Truncate(Math.Sqrt((localDimension2.Width - localDimension1.Width) * (localDimension2.Width - localDimension1.Width) + (localDimension2.Height - localDimension1.Height) * (localDimension2.Height - localDimension1.Height))))
                    j = 0
                    While j * paramInt2 < i
                        localDimension5 = calcInside(localDimension2, localDimension1, j * paramInt2)
                        Dim k As Integer = localDimension5.Width
                        Dim m As Integer = localDimension5.Height
                        Dim n As Integer = CInt(Math.Truncate(Math.Ceiling(paramInt2 * 1.5 * (1.0 - j * paramInt2 \ i))))
                        If j Mod 2 = 0 Then
                            localDimension3 = calcSidePoint(localDimension5, localDimension1, n, 1)
                            Me.xyPoints.Add(localDimension3)
                        Else
                            localDimension3 = calcSidePoint(localDimension5, localDimension1, n, 2)
                            Me.xyPoints.Add(localDimension3)
                        End If
                        j += 1
                    End While
                    Me.xyPoints.Add(localDimension1)

                Case 0, 3
                    Me.xyPoints.Add(localDimension1)
                    Me.xyPoints.Add(localDimension2)

                Case 1
                    localDimension3 = calcSidePoint(localDimension2, localDimension1, paramInt2, 1)
                    localDimension4 = calcSidePoint(localDimension2, localDimension1, paramInt2, 2)
                    Me.xyPoints.Add(localDimension1)
                    Me.xyPoints.Add(localDimension3)
                    Me.xyPoints.Add(localDimension4)
                    Me.xyPoints.Add(localDimension1)

                Case 6
                    localDimension3 = calcSidePoint(localDimension2, localDimension1, paramInt2, 1)
                    localDimension4 = calcSidePoint(localDimension2, localDimension1, paramInt2, 2)
                    Me.xyPoints.Add(calcSidePoint(localDimension2, localDimension1, paramInt2 + 3, 1))
                    Me.xyPoints.Add(calcSidePoint(localDimension1, localDimension2, paramInt2 + 3, 2))
                    Me.xyPoints.Add(calcSidePoint(localDimension1, localDimension2, 1, 2))
                    Me.xyPoints.Add(calcSidePoint(localDimension2, localDimension1, paramInt2 + 1, 1))
                    Me.xyPoints.Add(calcSidePoint(localDimension2, localDimension1, paramInt2 + 3, 2))
                    Me.xyPoints.Add(calcSidePoint(localDimension1, localDimension2, paramInt2 + 3, 1))
                    Me.xyPoints.Add(calcSidePoint(localDimension1, localDimension2, 1, 1))
                    Me.xyPoints.Add(calcSidePoint(localDimension2, localDimension1, paramInt2 + 1, 2))
                    i = CInt(Math.Truncate(Math.Sqrt((localDimension2.Width - localDimension1.Width) * (localDimension2.Width - localDimension1.Width) + (localDimension2.Height - localDimension1.Height) * (localDimension2.Height - localDimension1.Height))))
                    j = 0
                    While j * paramInt3 < i
                        localDimension5 = calcInside(localDimension2, localDimension1, j * paramInt3)
                        Me.xyPoints.Add(New DblRect(localDimension5.Width - localDimension2.Width + localDimension3.Width, localDimension5.Height - localDimension2.Height + localDimension3.Height))
                        Me.xyPoints.Add(New DblRect(localDimension5.Width - localDimension2.Width + localDimension4.Width, localDimension5.Height - localDimension2.Height + localDimension4.Height))
                        j += 1
                    End While

            End Select
            Me.m_recalc = False
        End Sub

        Private Sub recalcCoordinate(paramDouble1 As Double, paramInt As Integer, paramDouble2 As Double)
            Me.xyPoints.Clear()
            Dim i As Integer = 4
            Dim localDimension1 As DblRect = Me.m_atom1.getCoordinate(paramDouble1)
            Dim localDimension2 As DblRect = Me.m_atom2.getCoordinate(paramDouble1)
            Dim localObject2 As Object = Nothing
            Dim localObject1 As Object = Nothing
            If (Me.m_atom1.DrawLabel.Length > 0) AndAlso (Me.m_stereo <> 0) Then
                localDimension1 = calcInside(localDimension1, localDimension2, paramInt)
            End If
            If (Me.m_atom2.DrawLabel.Length > 0) AndAlso (Me.m_stereo <> 0) Then
                localDimension2 = calcInside(localDimension2, localDimension1, paramInt)
            End If
            Select Case Me.m_order
                Case 2
                    Dim localDimension3 As DblRect
                    Dim localDimension4 As DblRect
                    Select Case Me.m_orientation
                        Case 1, 11
                            localObject1 = calcSidePoint(localDimension1, localDimension2, 1.0, 1)
                            localObject2 = calcSidePoint(localDimension2, localDimension1, 1.0, 2)
                            If (Atom1.numBond() > 1) AndAlso (Atom2.numBond() > 1) Then
                                localDimension3 = New DblRect(DirectCast(localObject1, DblRect))
                                localDimension4 = New DblRect(DirectCast(localObject2, DblRect))
                                If Me.m_atom1.DrawLabel.Length = 0 Then
                                    localDimension3 = calcInside(DirectCast(localObject1, DblRect), DirectCast(localObject2, DblRect), i)
                                End If
                                If Me.m_atom2.DrawLabel.Length = 0 Then
                                    localDimension4 = calcInside(DirectCast(localObject2, DblRect), DirectCast(localObject1, DblRect), i)
                                End If
                                localObject1 = localDimension3
                                localObject2 = localDimension4
                            End If

                        Case 0, 2, 12
                            localObject1 = calcSidePoint(localDimension1, localDimension2, 0.5, 2)
                            localObject2 = calcSidePoint(localDimension2, localDimension1, 0.5, 1)
                            If (DirectCast(localObject1, DblRect).Height = localDimension1.Height) AndAlso (DirectCast(localObject2, DblRect).Height = localDimension2.Height) Then
                                localDimension1.Width -= DirectCast(localObject1, DblRect).Width - localDimension1.Width
                                localDimension2.Width -= DirectCast(localObject2, DblRect).Width - localDimension2.Width
                            Else
                                localDimension1 = calcSidePoint(DirectCast(localObject1, DblRect), DirectCast(localObject2, DblRect), 1.0, 1)
                                localDimension2 = calcSidePoint(DirectCast(localObject2, DblRect), DirectCast(localObject1, DblRect), 1.0, 2)
                            End If

                        Case 3, 13
                            localObject1 = calcSidePoint(localDimension1, localDimension2, 1.0, 2)
                            localObject2 = calcSidePoint(localDimension2, localDimension1, 1.0, 1)
                            If (Atom1.numBond() > 1) AndAlso (Atom2.numBond() > 1) Then
                                localDimension3 = New DblRect(DirectCast(localObject1, DblRect))
                                localDimension4 = New DblRect(DirectCast(localObject2, DblRect))
                                If Me.m_atom1.DrawLabel.Length = 0 Then
                                    localDimension3 = calcInside(DirectCast(localObject1, DblRect), DirectCast(localObject2, DblRect), i)
                                End If
                                If Me.m_atom2.DrawLabel.Length = 0 Then
                                    localDimension4 = calcInside(DirectCast(localObject2, DblRect), DirectCast(localObject1, DblRect), i)
                                End If
                                localObject1 = localDimension3
                                localObject2 = localDimension4
                            End If

                    End Select
                    Me.xyPoints.Add(localDimension1)
                    Me.xyPoints.Add(localDimension2)
                    Me.xyPoints.Add(localObject1)
                    Me.xyPoints.Add(localObject2)

                Case 3
                    Me.xyPoints.Add(localDimension1)
                    Me.xyPoints.Add(localDimension2)
                    localObject1 = calcSidePoint(localDimension1, localDimension2, 0.8, 1)
                    localObject2 = calcSidePoint(localDimension2, localDimension1, 0.8, 2)
                    Me.xyPoints.Add(localObject1)
                    Me.xyPoints.Add(localObject2)
                    localObject1 = calcSidePoint(localDimension1, localDimension2, 0.8, 2)
                    localObject2 = calcSidePoint(localDimension2, localDimension1, 0.8, 1)
                    Me.xyPoints.Add(localObject1)
                    Me.xyPoints.Add(localObject2)

                Case -1, 0, 4
                    Me.xyPoints.Add(localDimension1)
                    Me.xyPoints.Add(localDimension2)

            End Select
            Me.m_recalc = False
        End Sub

        Private Function calcInside(paramDimension1 As DblRect, paramDimension2 As DblRect, paramInt As Integer) As DblRect
            Dim i As Integer = paramDimension1.Width
            Dim j As Integer = paramDimension1.Height
            Dim d As Double = Math.Sqrt((paramDimension2.Width - paramDimension1.Width) * (paramDimension2.Width - paramDimension1.Width) + (paramDimension2.Height - paramDimension1.Height) * (paramDimension2.Height - paramDimension1.Height))
            If d > paramInt Then
                i = CInt(Math.Truncate(Math.Round(paramDimension1.Width + (paramDimension2.Width - paramDimension1.Width) * paramInt / d)))
                j = CInt(Math.Truncate(Math.Round(paramDimension1.Height + (paramDimension2.Height - paramDimension1.Height) * paramInt / d)))
            End If
            Return New DblRect(i, j)
        End Function

        Private Function calcInside(paramDimension1 As DblRect, paramDimension2 As DblRect, paramDouble As Double) As DblRect
            Dim i As Integer = paramDimension1.Width
            Dim j As Integer = paramDimension1.Height
            Dim d As Double = Math.Sqrt((paramDimension2.Width - paramDimension1.Width) * (paramDimension2.Width - paramDimension1.Width) + (paramDimension2.Height - paramDimension1.Height) * (paramDimension2.Height - paramDimension1.Height))
            If d > paramDouble Then
                i = CInt(Math.Truncate(Math.Round(paramDimension1.Width + (paramDimension2.Width - paramDimension1.Width) * paramDouble / d)))
                j = CInt(Math.Truncate(Math.Round(paramDimension1.Height + (paramDimension2.Height - paramDimension1.Height) * paramDouble / d)))
            End If
            Return New DblRect(i, j)
        End Function

        Private Function calcOutside(paramDimension1 As DblRect, paramDimension2 As DblRect, paramInt As Integer) As DblRect
            Dim i As Integer = paramDimension1.Width
            Dim j As Integer = paramDimension1.Height
            Dim k As Integer = CInt(Math.Truncate(Math.Sqrt((paramDimension2.Width - paramDimension1.Width) * (paramDimension2.Width - paramDimension1.Width) + (paramDimension2.Height - paramDimension1.Height) * (paramDimension2.Height - paramDimension1.Height))))
            If k > 0.0 Then
                i = paramDimension1.Width + (paramDimension1.Width - paramDimension2.Width) * paramInt / k
                j = paramDimension1.Height + (paramDimension1.Height - paramDimension2.Height) * paramInt / k
            End If
            Return New DblRect(i, j)
        End Function

        Private Function calcSidePoint(paramDimension1 As DblRect, paramDimension2 As DblRect, paramDouble As Double, paramInt As Integer) As DblRect
            Dim i As Integer = paramDimension1.Width
            Dim j As Integer = paramDimension1.Height
            Dim d1 As Double = 0.0
            Dim d2 As Double = 0.0
            Dim d3 As Double = If(paramDouble = 1.0, 4.5, 2.25)
            d3 = 4.5 * paramDouble
            Dim d4 As Double = Math.Sqrt((paramDimension1.Height - paramDimension2.Height) * (paramDimension1.Height - paramDimension2.Height) + (paramDimension2.Width - paramDimension1.Width) * (paramDimension2.Width - paramDimension1.Width))
            Select Case paramInt
                Case 1
                    d1 = (paramDimension1.Height - paramDimension2.Height) / d4
                    d2 = (paramDimension2.Width - paramDimension1.Width) / d4

                Case 2
                    d1 = (paramDimension2.Height - paramDimension1.Height) / d4
                    d2 = (paramDimension1.Width - paramDimension2.Width) / d4

            End Select
            i = CInt(Math.Truncate(i + Math.Round(d3 * d1)))
            j = CInt(Math.Truncate(j + Math.Round(d3 * d2)))
            Return New DblRect(i, j)
        End Function

        Private Function calcSidePoint(paramDimension1 As DblRect, paramDimension2 As DblRect, paramInt1 As Integer, paramInt2 As Integer) As DblRect
            Dim i As Integer = paramDimension1.Width
            Dim j As Integer = paramDimension1.Height
            Dim d1 As Double = 0.0
            Dim d2 As Double = 0.0
            Dim d3 As Double = paramInt1
            Dim d4 As Double = Math.Sqrt((paramDimension1.Height - paramDimension2.Height) * (paramDimension1.Height - paramDimension2.Height) + (paramDimension2.Width - paramDimension1.Width) * (paramDimension2.Width - paramDimension1.Width))
            Select Case paramInt2
                Case 1
                    d1 = (paramDimension1.Height - paramDimension2.Height) / d4
                    d2 = (paramDimension2.Width - paramDimension1.Width) / d4

                Case 2
                    d1 = (paramDimension2.Height - paramDimension1.Height) / d4
                    d2 = (paramDimension1.Width - paramDimension2.Width) / d4

            End Select
            i = CInt(Math.Truncate(i + Math.Round(d3 * d1)))
            j = CInt(Math.Truncate(j + Math.Round(d3 * d2)))
            Return New DblRect(i, j)
        End Function

        Public Overridable Overloads Function ToString() As String
            Dim i As Integer = Me.m_mol.getAtomNo(Me.m_atom1)
            Dim j As Integer = Me.m_mol.getAtomNo(Me.m_atom2)
            Return keg.common.util.DEBTutil.printf(i, "3") & keg.common.util.DEBTutil.printf(j, "3") & keg.common.util.DEBTutil.printf(Order, "3") & keg.common.util.DEBTutil.printf(Me.m_stereo, "3") & "   " & "  0" & "  0"
        End Function

        Public Overridable Function toKCFString() As String
            Return toKCFString(0)
        End Function

        Public Overridable Function toKCFString(paramInt As Integer) As String
            Dim localStringBuffer As New StringBuilder()
            localStringBuffer.Append(keg.common.util.DEBTutil.printf(Me.m_mol.getAtomNo(Me.m_atom1) + paramInt, "3"))
            localStringBuffer.Append(" ")
            localStringBuffer.Append(keg.common.util.DEBTutil.printf(Me.m_mol.getAtomNo(Me.m_atom2) + paramInt, "3"))
            localStringBuffer.Append(" ")
            localStringBuffer.Append(keg.common.util.DEBTutil.printf(Order, "-1"))
            If (Me.m_stereo <> 3) AndAlso (Me.m_stereo <> 0) Then
                localStringBuffer.Append(" #")
                Select Case Me.m_stereo
                    Case 4
                        localStringBuffer.Append("Either")

                    Case 1
                        localStringBuffer.Append("Up")

                    Case 6
                        localStringBuffer.Append("Down")

                End Select
            End If
            Return localStringBuffer.ToString()
        End Function

        Public Overridable ReadOnly Property Ionic() As Boolean
            Get
                Return (Me.m_atom1.Charge <> 0) AndAlso (Me.m_atom2.Charge <> 0)
            End Get
        End Property

        Public Overridable ReadOnly Property NonGroupedBond() As Boolean
            Get
                Return (Me.m_atom1.NonGroupedAtom) AndAlso (Me.m_atom2.NonGroupedAtom)
            End Get
        End Property

        Public Overridable ReadOnly Property ConnectedBond() As Boolean
            Get
                Return (Me.m_atom1.NonGroupedAtom) OrElse (Me.m_atom2.NonGroupedAtom)
            End Get
        End Property
    End Class
End Namespace

