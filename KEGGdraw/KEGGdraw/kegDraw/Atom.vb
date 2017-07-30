Imports System.Collections
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Text
Imports Microsoft.VisualBasic.Imaging.LayoutModel
Imports Microsoft.VisualBasic.Imaging.Math2D

Namespace keg.compound


    Public Class Atom
        Inherits ChemObject
        Private __KEGGAtomName As String = ""
        Public Const Chiral_None As Integer = 0
        Public Const Chiral_R As Integer = 1
        Public Const Chiral_S As Integer = 2
        Public Const Chiral_Unknown As Integer = 3
        Public Const Hydrogen_Right As Integer = 1
        Public Const Hydrogen_Below As Integer = 2
        Public Const Hydrogen_Left As Integer = 3
        Public Const Hydrogen_Above As Integer = 4
        Public Const Hydrogen_None As Integer = 0
        Private m_label As String = "C"
        Private m_charge As Integer = 0
        Private m_isotope As Integer = 0
        Private m_chiral As Integer = 0
        Private m_radical As Integer = 0
        Private bonds As New ArrayList()
        Public x As Double = 0.0
        Public y As Double = 0.0
        Public z As Double = 0.0
        Private m_mol As Molecule = Nothing
        Public na As Integer = 0
        Public nb As Integer = 0
        Public nc As Integer = 0
        Private m_recalc As Boolean = True
        Private sp3 As Boolean = True
        Public col As Color = Nothing
        Public Shadows id As String = ""
        Public Shared ReadOnly __ImplicitHydrogen As New Atom(DirectCast(Nothing, Molecule), 0.0, 0.0, 0.0, "H")
        Friend implicit_hydrogen As Integer = 0
        Friend hydrogen_count_offset As Integer = 0
        Friend hydrogen_draw As Integer = 0
        Friend hydrogen_draw_direction_auto As Boolean = True
        Friend m_fontsize As Integer = -1
        Private prevNumberOfConnections As Integer = 0
        Private currNumberOfConnections As Integer = 0
        Friend m_drawLabel As String = "C"
        Friend hydrogen_drawFlag As Boolean = False
        Friend visibility As Boolean = True
        Friend m_express_group As Boolean = False
        Friend groupAtoms As List(Of Atom) = Nothing
        Friend groupPartners As List(Of Atom) = Nothing
        Public Const GROUP_DIRECT_RIGHT As Integer = 0
        Public Const GROUP_DIRECT_LEFT As Integer = 1
        Public Const GROUP_DIRECT_ABOVE As Integer = 2
        Public Const GROUP_DIRECT_BELOW As Integer = 3
        Private m_groupLabelDirection As Integer = 0

        Public Sub New()
        End Sub

        'JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        'ORIGINAL LINE: public Atom(Molecule paramMolecule, String paramString) throws keg.common.exception.IllegalFormatException
        Public Sub New(paramMolecule As Molecule, paramString As String)
            Dim j As Integer = 0
            Dim k As Integer = 0
            Me.m_mol = paramMolecule
            Try
                Dim str As String = paramString.Substring(0, 10)
                Dim localDouble As New System.Nullable(Of Double)(str.Trim())
                Me.x = CDbl(localDouble)
                str = paramString.Substring(10, 10)
                localDouble = New System.Nullable(Of Double)(str.Trim())
                Me.y = CDbl(localDouble)
                str = paramString.Substring(20, 10)
                localDouble = New System.Nullable(Of Double)(str.Trim())
                Me.z = CDbl(localDouble)
                If paramString.Length > 33 Then
                    str = paramString.Substring(31, 3)
                ElseIf paramString.Length > 31 Then
                    str = paramString.Substring(31)
                Else
                    str = ""
                End If
                Label = str.Trim()
                If paramString.Length > 35 Then
                    str = paramString.Substring(34, 2)
                    j = Convert.ToInt32(str.Trim())
                    Me.m_isotope = j
                End If
                If paramString.Length > 38 Then
                    str = paramString.Substring(36, 3)
                    Dim i As Integer = Convert.ToInt32(str.Trim())
                    Me.m_charge = (If(i = 0, 0, 4 - i))
                End If
                If paramString.Length > 41 Then
                    str = paramString.Substring(39, 3)
                    Me.m_chiral = Convert.ToInt32(str.Trim())
                End If
            Catch
                Console.WriteLine(paramString)
                Throw New Exception("Format error in Atom Block.")
            End Try
        End Sub

        Public Sub New(paramMolecule As Molecule, paramDouble1 As Double, paramDouble2 As Double, paramDouble3 As Double)
            Me.m_mol = paramMolecule
            Me.x = paramDouble1
            Me.y = paramDouble2
            Me.z = paramDouble3
        End Sub

        Public Sub New(paramMolecule As Molecule, paramDouble1 As Double, paramDouble2 As Double, paramDouble3 As Double, paramString As String)
            Me.New(paramMolecule, paramDouble1, paramDouble2, paramDouble3)
            Label = paramString
        End Sub

        Public Sub New(paramMolecule As Molecule, paramDouble1 As Double, paramDouble2 As Double, paramDouble3 As Double, paramString As String, paramInt1 As Integer,
            paramInt2 As Integer, paramInt3 As Integer)
            Me.New(paramMolecule, paramDouble1, paramDouble2, paramDouble3, paramString)
            Me.m_charge = paramInt1
            Me.m_isotope = paramInt2
            Me.m_chiral = paramInt3
        End Sub

        Public Overridable Property Label() As String
            Get
                Return Me.m_label
            End Get
            Set
                If Value.Equals("") Then
                    Value = "C"
                End If
                Me.m_label = PeriodicTable.getName(Value)
                Me.m_label = (If(Me.m_label.Equals("r"), "R", Me.m_label))
                If KEGGAtomName.Equals("") Then
                    KEGGAtomName = Value
                End If
                DrawLabel = Me.hydrogen_drawFlag
            End Set
        End Property


        Public Overridable ReadOnly Property AtomicNumber() As Integer
            Get
                Dim i As Integer = 0
                If Label.Length = 0 Then
                    i = 6
                Else
                    For j As Integer = 1 To 102
                        If Label.Equals(DEBT.periodic_label(j)) Then
                            i = j
                            Exit For
                        End If
                    Next
                    If Label.Equals("D") Then
                        i = 1
                    End If
                End If
                Return i
            End Get
        End Property

        Public Overridable Property Charge() As Integer
            Get
                Return Me.m_charge
            End Get
            Set
                Me.m_charge = Value
                Me.m_radical = 0
                For i As Integer = 0 To Me.bonds.Count - 1
                    DirectCast(Me.bonds(i), Bond).Recalc = True
                Next
            End Set
        End Property


        Public Overridable Property Isotope() As Integer
            Get
                Return Me.m_isotope
            End Get
            Set
                Me.m_isotope = Value
                For i As Integer = 0 To Me.bonds.Count - 1
                    DirectCast(Me.bonds(i), Bond).Recalc = True
                Next
            End Set
        End Property


        Public Overridable Property Radical() As Integer
            Get
                Return Me.m_radical
            End Get
            Set
                Me.m_radical = Value
                Me.m_charge = 0
                For i As Integer = 0 To Me.bonds.Count - 1
                    DirectCast(Me.bonds(i), Bond).Recalc = True
                Next
            End Set
        End Property


        Public Overridable Property FullLabel() As String
            Get
                Dim str As String = ""
                If Me.m_isotope > 0 Then
                    str = str & Convert.ToString(Me.m_isotope)
                End If
                If Label.Length > 0 Then
                    str = str & Label
                ElseIf Me.m_isotope > 0 Then
                    str = str & "C"
                End If
                If Math.Abs(Me.m_charge) > 1 Then
                    str = str & Convert.ToString(Math.Abs(Me.m_charge))
                End If
                If Me.m_charge > 0 Then
                    str = str & "+"
                ElseIf Me.m_charge < 0 Then
                    str = str & "-"
                End If
                For i As Integer = 0 To Me.m_radical - 1
                    str = str & "*"
                Next
                Return str
            End Get
            Set
                Dim str1 As String = ""
                Dim str2 As String = ""
                Dim str3 As String = ""
                Dim localException1 As Integer = 0
                Dim localException2 As Integer = Value.Length
                Dim i As Integer = 1
                Label = "C"
                Me.m_isotope = 0
                Me.m_charge = 0
                Me.m_radical = 0
                For localException3 As Integer = localException1 To Value.Length - 1
                    If Not Char.IsDigit(Value(localException3)) Then
                        localException2 = localException3
                        Exit For
                    End If
                Next
                If localException2 > localException1 Then
                    str1 = Value.Substring(localException1, localException2 - localException1)
                    Try
                        Me.m_isotope = Convert.ToInt32(str1)
                    Catch localException4 As Exception
                        Console.WriteLine(localException4)
                    End Try
                End If
                localException1 = localException2
                localException2 = Value.Length
                For localException4 = localException1 To Value.Length - 1
                    If ((Value(localException4) < "a"c) OrElse (Value(localException4) > "z"c)) AndAlso ((Value(localException4) < "A"c) OrElse (Value(localException4) > "Z"c)) AndAlso (Value(localException4) <> "*"c) Then
                        localException2 = localException4
                        Exit For
                    End If
                Next
                If localException2 > localException1 Then
                    str2 = Value.Substring(localException1, localException2 - localException1)
                    If str2.Length > 3 Then
                        Label = str2.Substring(0, 3)
                    Else
                        Label = str2
                    End If
                End If
                localException1 = localException2
                localException2 = Value.Length
                For localException4 = localException1 To Value.Length - 1
                    If Not Char.IsDigit(Value(localException4)) Then
                        localException2 = localException4
                        Exit For
                    End If
                Next
                If localException2 < Value.Length Then
                    If localException2 > localException1 Then
                        str3 = Value.Substring(localException1, localException2 - localException1)
                        Try
                            i = Convert.ToInt32(str3)
                        Catch localException5 As Exception
                            Console.WriteLine(localException5)
                        End Try
                    End If
                    Select Case Value(localException2)
                        Case "*"c
                            Me.m_radical = i
                            
                        Case "+"c
                            Me.m_charge = i
                            
                        Case "-"c
                            Me.m_charge = (-i)
                            
                    End Select
                End If
                If (Me.m_label.Equals("R")) AndAlso (Me.m_radical = 0) AndAlso (Me.m_charge = 0) Then
                    Label = Label & Value.Substring(localException1)
                End If
            End Set
        End Property


        Public Overridable Property Chiral() As Integer
            Get
                Return Me.m_chiral
            End Get
            Set
                Me.m_chiral = Value
            End Set
        End Property


        Public Overridable Sub makeBond(paramBond As Bond)
            If (paramBond.hasAtom(Me)) AndAlso (Not Me.bonds.Contains(paramBond)) Then
                Me.bonds.Add(paramBond)
            End If
        End Sub

        Public Overridable Sub breakBond(paramBond As Bond)
            If paramBond.hasAtom(Me) Then
                Me.bonds.Remove(paramBond)
                setNumberOfConnections()
            End If
        End Sub

        Public Overridable Function numBond() As Integer
            Return Me.bonds.Count
        End Function

        Public Overridable ReadOnly Property PerviuseNumberOfConnections() As Integer
            Get
                Return Me.prevNumberOfConnections
            End Get
        End Property

        Public Overridable ReadOnly Property NumberOfConnections() As Integer
            Get
                Return Me.currNumberOfConnections
            End Get
        End Property

        Public Overridable Sub setNumberOfConnections()
            Me.prevNumberOfConnections = Me.currNumberOfConnections
            Dim i As Integer = 0
            For j As Integer = 0 To Me.bonds.Count - 1
                Dim localBond As Bond = DirectCast(Me.bonds(j), Bond)
                i += (If(localBond.Order = 0, 1, localBond.Order))
            Next
            If Not Me.m_label.Equals("B") Then
                i -= Me.m_charge
            Else
                i += Me.m_charge
            End If
            Me.currNumberOfConnections = i
        End Sub

        Public Overridable Function getBond(paramInt As Integer) As Bond
            Return DirectCast(Me.bonds(paramInt), Bond)
        End Function

        Public Overridable Function getBond(paramAtom As Atom) As Bond
            For i As Integer = 0 To Me.bonds.Count - 1
                If DirectCast(Me.bonds(i), Bond).pairAtom(Me) Is paramAtom Then
                    Return DirectCast(Me.bonds(i), Bond)
                End If
            Next
            Return Nothing
        End Function

        Public Overrides Property Color() As Color
            Get
                Return Me.col
            End Get
            Set
                Me.col = Value
            End Set
        End Property


        Public Overridable Function isConnect(paramAtom As Atom) As Boolean
            For i As Integer = 0 To Me.bonds.Count - 1
                If DirectCast(Me.bonds(i), Bond).pairAtom(Me) Is paramAtom Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Overridable Property InCoordinate() As Vector2D
            Get
                Return New Vector2D(Me.x, Me.y)
            End Get
            Set
                Me.x = Value.x
                Me.y = Value.y
            End Set
        End Property

        Public Overridable Sub zoom(paramDouble As Double)
            Me.x *= paramDouble
            Me.y *= paramDouble
            Recalc = True
        End Sub


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

        Public Overridable Sub setsp3(paramBoolean As Boolean)
            Me.sp3 = paramBoolean
        End Sub

        Public Overridable Function getsp3() As Boolean
            Return Me.sp3
        End Function

        Public Overrides ReadOnly Property Coordinate() As DblRect
            Get
                Return New DblRect(Me.displayX, Me.displayY)
            End Get
        End Property

        Public Overridable Function getCoordinate(paramDouble As Double) As DblRect
            If Me.m_recalc Then
                recalcCoordinate(paramDouble)
            End If
            Return New DblRect(Me.displayX, Me.displayY)
        End Function

        Public Overridable Function DX(paramDouble As Double) As Integer
            If Me.m_recalc Then
                recalcCoordinate(paramDouble)
            End If
            Return Me.displayX
        End Function

        Public Overridable Function DY(paramDouble As Double) As Integer
            If Me.m_recalc Then
                recalcCoordinate(paramDouble)
            End If
            Return Me.displayY
        End Function

        Private Sub recalcCoordinate(paramDouble As Double)
            Dim localDimension As DblRect = Me.m_mol.Coordinate
            Me.displayX = (localDimension.width + CInt(Math.Truncate(Math.Round(Me.x * paramDouble))))
            Me.displayY = (localDimension.height + CInt(Math.Truncate(Math.Round(Me.y * paramDouble))))
            Me.m_recalc = False
        End Sub

        Public Overridable Sub moveInternal(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double)
            Me.x += paramInt1 / paramDouble
            Me.y += paramInt2 / paramDouble
            Me.m_recalc = True
            For i As Integer = 0 To Me.bonds.Count - 1
                DirectCast(Me.bonds(i), Bond).Recalc = True
            Next
        End Sub

        Public Overrides Sub move(paramInt1 As Integer, paramInt2 As Integer)
            Me.displayX += paramInt1
            Me.displayY -= paramInt2
        End Sub

        Public Overridable Overloads Sub rotate(paramDouble1 As Double, paramDouble2 As Double, paramDouble3 As Double)
            Dim d3 As Double = Me.x
            Dim d4 As Double = Me.y
            Dim d1 As Double = Math.Cos(-paramDouble3)
            Dim d2 As Double = Math.Sin(-paramDouble3)
            Me.x = (d1 * (d3 - paramDouble1) - d2 * (d4 - paramDouble2) + paramDouble1)
            Me.y = (d2 * (d3 - paramDouble1) + d1 * (d4 - paramDouble2) + paramDouble2)
            Me.m_recalc = True
            For i As Integer = 0 To Me.bonds.Count - 1
                DirectCast(Me.bonds(i), Bond).Recalc = True
            Next
        End Sub

        Public Overridable Sub resize(paramDouble1 As Double, paramDouble2 As Double, paramDouble3 As Double, paramDouble4 As Double)
            Me.x = ((Me.x - paramDouble1) * paramDouble3 + paramDouble1)
            Me.y = ((Me.y - paramDouble2) * paramDouble4 + paramDouble2)
            Me.m_recalc = True
            For i As Integer = 0 To Me.bonds.Count - 1
                DirectCast(Me.bonds(i), Bond).Recalc = True
            Next
        End Sub

        Public Overrides Sub flipHorizontal(paramDouble1 As Double, paramDouble2 As Double)
            If (paramDouble1 - 1.0 < Me.x) AndAlso (Me.x < paramDouble2 + 1.0) Then
                Me.x = (paramDouble2 - (Me.x - paramDouble1))
            End If
        End Sub

        Public Overrides Sub flipVertical(paramDouble1 As Double, paramDouble2 As Double)
            If (paramDouble1 - 1.0 < Me.y) AndAlso (Me.y < paramDouble2 + 1.0) Then
                Me.y = (paramDouble2 - (Me.y - paramDouble1))
            End If
        End Sub

        Public Overridable Sub checkChiral()
            checkChiral(True)
        End Sub

        Public Overridable Sub checkChiral(paramBoolean As Boolean)
            Me.m_chiral = 0
        End Sub

        Public Overridable Function checkValence(paramBoolean As Boolean) As Boolean
            Return True
        End Function

        Public Overridable WriteOnly Property HydrogenCountOffset() As Integer
            Set
                Me.hydrogen_count_offset = Value
            End Set
        End Property

        Public Overridable Sub calcImplicitHydrogen()
            Dim i As Integer = Me.hydrogen_count_offset
            Dim k As Integer = 0
            Me.implicit_hydrogen = 0
            For m As Integer = 0 To Me.bonds.Count - 1
                Dim localBond As Bond = DirectCast(Me.bonds(m), Bond)
                Dim j As Integer = localBond.Order
                Select Case j
                    Case 1
                        i += 1
                        
                    Case 2
                        i += 2
                        
                    Case 3
                        i += 3
                        
                    Case 4
                        i += 1
                        k = 1
                        
                End Select
            Next
            If k <> 0 Then
                i += 1
            End If
            If Not Me.m_label.Equals("B") Then
                i -= Me.m_charge
            Else
                i += Me.m_charge
            End If
            If (Label.Length = 0) OrElse (Label.Equals("C")) Then
                If i <= 4 Then
                    Me.implicit_hydrogen = (4 - i)
                End If
            ElseIf Me.m_label.Equals("N") Then
                If i <= 3 Then
                    Me.implicit_hydrogen = (3 - i)
                ElseIf i <= 5 Then
                    Me.implicit_hydrogen = (5 - i)
                    Me.implicit_hydrogen = 0
                End If
            ElseIf Me.m_label.Equals("O") Then
                If i <= 2 Then
                    Me.implicit_hydrogen = (2 - i)
                End If
            ElseIf Me.m_label.Equals("S") Then
                If i <= 2 Then
                    Me.implicit_hydrogen = (2 - i)
                ElseIf i <= 4 Then
                    Me.implicit_hydrogen = (4 - i)
                    Me.implicit_hydrogen = 0
                ElseIf i <= 6 Then
                    Me.implicit_hydrogen = (6 - i)
                    Me.implicit_hydrogen = 0
                End If
            ElseIf Me.m_label.Equals("P") Then
                If i <= 3 Then
                    Me.implicit_hydrogen = (3 - i)
                ElseIf i <= 5 Then
                    Me.implicit_hydrogen = (5 - i)
                    Me.implicit_hydrogen = 0
                End If
            ElseIf Me.m_label.Equals("B") Then
                If i <= 3 Then
                    Me.implicit_hydrogen = (3 - i)
                End If
            ElseIf ((Me.m_label.Equals("F")) OrElse (Me.m_label.Equals("Cl")) OrElse (Me.m_label.Equals("Br")) OrElse (Me.m_label.Equals("I"))) AndAlso (i = 0) Then
                Me.implicit_hydrogen = 1
            End If
            If Me.implicit_hydrogen < 0 Then
                Me.implicit_hydrogen = 0
            End If
        End Sub

        Public Overridable ReadOnly Property ImplicitHydrogen() As Integer
            Get
                Return Me.implicit_hydrogen
            End Get
        End Property

        Public Overridable Sub decisideHydrogenDraw()
            If Not Me.hydrogen_draw_direction_auto Then
                Return
            End If
            If Me.implicit_hydrogen = 0 Then
                Me.hydrogen_draw = 0
            ElseIf (Label.Length = 0) AndAlso (Me.m_isotope = 0) AndAlso (Me.m_charge = 0) Then
                Me.hydrogen_draw = 0
            Else
                Dim i As Integer = 0
                Dim j As Integer = 0
                Dim k As Integer = 0
                Dim m As Integer = 0
                For n As Integer = 0 To Me.bonds.Count - 1
                    Dim localAtom As Atom = DirectCast(Me.bonds(n), Bond).pairAtom(Me)
                    If localAtom.x < Me.x Then
                        m += 1
                    End If
                    If localAtom.x > Me.x Then
                        k += 1
                    End If
                    If localAtom.y > Me.y Then
                        j += 1
                    End If
                    If localAtom.y < Me.y Then
                        i += 1
                    End If
                Next
                If (Me.bonds.Count = 0) AndAlso ((Me.m_label.Equals("O")) OrElse (Me.m_label.Equals("F")) OrElse (Me.m_label.Equals("Br")) OrElse (Me.m_label.Equals("Cl")) OrElse (Me.m_label.Equals("I"))) Then
                    Me.hydrogen_draw = 3
                ElseIf k = 0 Then
                    Me.hydrogen_draw = 1
                ElseIf m = 0 Then
                    Me.hydrogen_draw = 3
                ElseIf j = 0 Then
                    Me.hydrogen_draw = 2
                ElseIf i = 0 Then
                    Me.hydrogen_draw = 4
                Else
                    Me.hydrogen_draw = 1
                End If
                If (Label.Equals("C")) AndAlso (numBond() > 1) Then
                    Me.hydrogen_draw = 0
                End If
            End If
        End Sub

        Public Overridable Property DrawLabel() As Boolean
            Get
                DrawLabel = Me.hydrogen_drawFlag
                Return Me.m_drawLabel
            End Get
            Set
                Me.hydrogen_drawFlag = Value
                If Me.m_label.Equals("C") Then
                    If keg.draw.util.PropertiesManager.getProperties("COMPOUND_TYPE_HIDDENHYDOROGEN").Equals("HETERO") Then
                        Me.m_drawLabel = ""
                    ElseIf (Me.hydrogen_drawFlag) AndAlso (numBond() < 2) Then
                        Me.m_drawLabel = Me.m_label
                    Else
                        Me.m_drawLabel = ""
                    End If
                Else
                    Me.m_drawLabel = Me.m_label
                End If
                For i As Integer = 0 To Me.bonds.Count - 1
                    DirectCast(Me.bonds(i), Bond).Recalc = True
                Next
            End Set
        End Property


        Public Overridable Function getDrawLabel(paramBoolean As Boolean) As String
            DrawLabel = paramBoolean
            Return Me.m_drawLabel
        End Function

        Public Overridable Property HydrogenDraw() As Integer
            Get
                Return Me.hydrogen_draw
            End Get
            Set
                HydrogenDrawAuto = False
                Me.hydrogen_draw = Value
            End Set
        End Property


        Public Overridable Property HydrogenDrawAuto() As Boolean
            Get
                Return Me.hydrogen_draw_direction_auto
            End Get
            Set
                Me.hydrogen_draw_direction_auto = Value
                If Me.hydrogen_draw_direction_auto Then
                    decisideHydrogenDraw()
                End If
            End Set
        End Property


        Public Overridable Overloads Function ToString(paramBoolean As Boolean, paramDouble As Double) As String
            Dim j As Integer = -1
            Dim k As Integer = 0
            If Me.m_isotope > 0 Then
                For i As Integer = 1 To 103
                    If Label.Equals(DEBT.periodic_label(i)) Then
                        j = DEBT.periodic_mass(i)
                        Exit For
                    End If
                Next
                If j > 0 Then
                    k = Me.m_isotope - j
                End If
                If k < -3 Then
                    k = 0
                End If
                If k > 4 Then
                    k = 0
                End If
            End If
            Dim str1 As String = Me.m_label
            If (Me.m_label.StartsWith("R")) AndAlso (Me.m_label.Length > 1) Then
                Dim str2 As String = Me.m_label.Substring(1)
                Try
                    Convert.ToInt32(str2.Trim())
                    str1 = "R#"
                Catch
                End Try
            End If
            Dim d1 As Double = Me.x
            Dim d2 As Double = Me.y
            If paramBoolean Then
                d1 += (Mol.Coordinate.Width - Mol.Parent.Coordinate.Width) / paramDouble
                d2 += (Mol.Parent.Coordinate.Height - Mol.Coordinate.Height) / paramDouble
            End If
            Return keg.common.util.DEBTutil.printf(d1, "5.4") & keg.common.util.DEBTutil.printf(-d2, "5.4") & keg.common.util.DEBTutil.printf(0.0, "5.4") & " " & (If(str1.Length = 0, "C  ", keg.common.util.DEBTutil.printf(str1, "-3"))) & keg.common.util.DEBTutil.printf(k, "2") & keg.common.util.DEBTutil.printf(If((Me.m_charge = 0) OrElse (Math.Abs(Me.m_charge) > 3), 0, 4 - Me.m_charge), "3") & keg.common.util.DEBTutil.printf(Me.m_chiral, "3") & "  0" & "  0" & "  0" & "  0" & "  0" & "  0" & "  0" & "  0" & "  0"
        End Function

        Public Overridable Function toTreeString() As String
            Return ""
        End Function

        Public Overridable Function toKCFString() As String
            Dim str1 As String = Nothing
            Dim localStringBuffer As New StringBuilder()
            Dim str2 As String = Label
            str1 = If(KEGGAtomName.Equals(""), str2, KEGGAtomName)
            localStringBuffer.Append(keg.common.util.DEBTutil.printf(If(str1.Length = 0, "C", str1), "-3"))
            localStringBuffer.Append(" ")
            localStringBuffer.Append(keg.common.util.DEBTutil.printf(If(str2.Length = 0, "C", str2), "-2"))
            localStringBuffer.Append(" ")
            localStringBuffer.Append(keg.common.util.DEBTutil.printf(Me.x, "4.4"))
            localStringBuffer.Append(" ")
            localStringBuffer.Append(keg.common.util.DEBTutil.printf(-Me.y, "4.4"))
            If Me.m_charge <> 0 Then
                localStringBuffer.Append(" ")
                localStringBuffer.Append("#")
                If Math.Abs(Me.m_charge) <> 1 Then
                    localStringBuffer.Append(Math.Abs(Me.m_charge))
                End If
                localStringBuffer.Append(If(Me.m_charge > -1, "+", "-"))
            End If
            Return localStringBuffer.ToString()
        End Function

        Public Overridable Property KEGGAtomName() As String
            Get
                Return Me.__KEGGAtomName
            End Get
            Set
                If Value.Equals("") Then
                    Value = "C"
                End If
                Me.__KEGGAtomName = Value
            End Set
        End Property


        Public Overridable Overloads Function ToString() As String
            Return ToString(False, 1.0)
        End Function

        Public Overridable ReadOnly Property LimitNumberOfConnections() As Integer
            Get
                If Me.m_label.Equals("C") Then
                    Return 4
                End If
                If Me.m_label.Equals("N") Then
                    If NumberOfConnections > 4 Then
                        Return 5
                    End If
                    Return 3
                End If
                If Me.m_label.Equals("P") Then
                    If NumberOfConnections > 4 Then
                        Return 5
                    End If
                    Return 3
                End If
                If Me.m_label.Equals("S") Then
                    If NumberOfConnections > 3 Then
                        Return 6
                    End If
                    Return 2
                End If
                If Me.m_label.Equals("O") Then
                    Return 2
                End If
                If Me.m_label.Equals("H") Then
                    Return 1
                End If
                If Me.m_label.Equals("Cl") Then
                    Return 1
                End If
                If Me.m_label.Equals("Br") Then
                    Return 1
                End If
                If Me.m_label.Equals("F") Then
                    Return 1
                End If
                If Me.m_label.Equals("B") Then
                    Return 3
                End If
                Return Integer.MaxValue
            End Get
        End Property

        Public Overridable ReadOnly Property MaxLimitNumberOfConnections() As Integer
            Get
                If Me.m_label.Equals("C") Then
                    Return 4
                End If
                If Me.m_label.Equals("N") Then
                    Return 5
                End If
                If Me.m_label.Equals("P") Then
                    Return 5
                End If
                If Me.m_label.Equals("S") Then
                    Return 6
                End If
                If Me.m_label.Equals("O") Then
                    Return 2
                End If
                If Me.m_label.Equals("H") Then
                    Return 1
                End If
                If Me.m_label.Equals("Cl") Then
                    Return 1
                End If
                If Me.m_label.Equals("Br") Then
                    Return 1
                End If
                If Me.m_label.Equals("F") Then
                    Return 1
                End If
                If Me.m_label.Equals("B") Then
                    Return 3
                End If
                Return Integer.MaxValue
            End Get
        End Property

        Public Overridable Property Fontsize() As Integer
            Get
                Return Me.m_fontsize
            End Get
            Set
                Me.m_fontsize = Value
            End Set
        End Property


        Public Overridable Property NonGroupedAtom() As Boolean
            Get
                Return Me.visibility
            End Get
            Set
                Me.visibility = Value
            End Set
        End Property


        Public Overridable Property Express_group() As Boolean
            Get
                Return Me.m_express_group
            End Get
            Set
                Me.m_express_group = Value
            End Set
        End Property


        Public Overridable Sub addGroupedAtom(paramAtom As Atom)
            If Me.groupAtoms Is Nothing Then
                Me.groupAtoms = New List(Of Atom)
            End If
            Me.groupAtoms.Add(paramAtom)
        End Sub

        Public Overridable Sub removeGroupedAtom(paramAtom As Atom)
            Me.groupAtoms.Remove(paramAtom)
            If Me.groupAtoms.Count = 0 Then
                Me.m_mol.deleteAtom(Me)
            End If
        End Sub

        Public Overridable ReadOnly Property GroupAtomSize() As Integer
            Get
                Return If(Me.groupAtoms Is Nothing, 0, Me.groupAtoms.Count)
            End Get
        End Property

        Public Overridable Function getGroupAtom(paramInt As Integer) As Atom
            Return DirectCast(Me.groupAtoms(paramInt), Atom)
        End Function

        Public Overridable Sub addGroupPartner(paramAtom As Atom)
            If Me.groupPartners Is Nothing Then
                Me.groupPartners = New List(Of Atom)
            End If
            Me.groupPartners.Add(paramAtom)
        End Sub

        Public Overridable Sub removeGroupPartnerAtom(paramAtom As Atom)
            Me.groupPartners.Remove(paramAtom)
        End Sub

        Public Overridable ReadOnly Property GroupPartnerSize() As Integer
            Get
                Return If(Me.groupPartners Is Nothing, 0, Me.groupPartners.Count)
            End Get
        End Property

        Public Overridable Function getGroupPartner(paramInt As Integer) As Atom
            Return DirectCast(Me.groupPartners(paramInt), Atom)
        End Function

        Public Overridable Property GroupLabelDirection() As Integer
            Get
                Return Me.m_groupLabelDirection
            End Get
            Set
                Me.m_groupLabelDirection = Value
            End Set
        End Property


        Public Overridable Function containsInGroupedAtoms(paramAtom As Atom) As Boolean
            Return Me.groupAtoms.Contains(paramAtom)
        End Function
    End Class
End Namespace
