#Region "Microsoft.VisualBasic::a9361d167aa523cb01d43726aafd2c95, G:/mzkit/src/visualize/KCF/KEGGdraw//kegDraw/Bracket.vb"

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

    '   Total Lines: 1230
    '    Code Lines: 1147
    ' Comment Lines: 2
    '   Blank Lines: 81
    '     File Size: 52.67 KB


    '     Class Bracket
    ' 
    '         Properties: Bonds, CoveredWholeMol, Label, Limited, LimitedNumber
    '                     Mol, OneHand, Reaction, Recalc, SBL1
    '                     SBL2, SelectAll, SelectSide, Sgroup, SgroupConnectivity
    '                     SimpleRepeat, Size, Type
    ' 
    '         Constructor: (+3 Overloads) Sub New
    ' 
    '         Function: calc_CrossPoint, clone, confirm_Bond, containBracket, contains
    '                   DX, DY, get0point, getBracketLabelMarkArea, getRect
    '                   hasOnlyOneMol, isIntersect, isSameLocatedBonds, nearBracket, nearBracket_1stSide
    '                   nearBracket_2ndSide, nearBracketLabel, size, (+2 Overloads) toKCFStrings, turn
    ' 
    '         Sub: [select], addMol, addSgroup, clearMol, clearSgroup
    '              computeParentMol, flipHorizontal, flipVertical, move, moveInternal
    '              moveInternaldependSelected, putSBL, recalcCoordinate, recalcCoordinateSub, removeMol
    '              reset0point, resize, resizeDependSelected, rotate, rotateDependSelected
    '              selectAll, (+2 Overloads) set0point, setCoordinate, (+2 Overloads) unselect, zoom
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports System.Collections
Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports Microsoft.VisualBasic.Imaging.LayoutModel

Namespace keg.compound

    Public Class Bracket : Inherits ChemObject

        Private m_label As String = "n"
        Private width As Integer
        Private height As Integer
        Private m_reaction As Reaction = Nothing
        Private m_recalc As Boolean = True
        Public b1x1 As Double
        Public b1y1 As Double
        Public b1x2 As Double
        Public b1y2 As Double
        Public b2x1 As Double
        Public b2y1 As Double
        Public b2x2 As Double
        Public b2y2 As Double
        Public lx As Double
        Public ly As Double
        Public setSDI As Integer = 0
        Private c1x1 As Integer
        Private c1y1 As Integer
        Private c1x2 As Integer
        Private c1y2 As Integer
        Private c1x3 As Integer
        Private c1y3 As Integer
        Private c1x4 As Integer
        Private c1y4 As Integer
        Private c2x1 As Integer
        Private c2y1 As Integer
        Private c2x2 As Integer
        Private c2y2 As Integer
        Private c2x3 As Integer
        Private c2y3 As Integer
        Private c2x4 As Integer
        Private c2y4 As Integer
        Private ldx As Integer
        Private ldy As Integer
        Private rect As Rectangle = Nothing
        Public col0 As Color = Nothing
        Public col1 As Color = Nothing
        Public col2 As Color = Nothing
        Friend bonds1 As New ArrayList()
        Friend bonds2 As New ArrayList()
        Friend molecules As New ArrayList()
        Private m_sgroup As New ArrayList()
        Public Const TYPE_MUL As String = "MUL"
        Public Const TYPE_SRU As String = "SRU"
        Public Const TYPE_MON As String = "MON"
        Public Const TYPE_GEN As String = "GEN"
        Private isWholeMol As Boolean = False
        Private m_sbl1 As Integer = 0
        Private m_sbl2 As Integer = 0
        Friend rect2d As RectangleF = New RectangleF()
        Private m_selectSide As Integer = -1
        Private __selectAll As Boolean = True
        Private m_type As String = "SRU"
        Private m_sgroupConnectivity As String = "HT"

        Public Sub New()
        End Sub

        Public Sub New(paramReaction As Reaction, paramVector2D1 As Vector2D, paramVector2D2 As Vector2D, paramVector2D3 As Vector2D)
            Me.m_reaction = paramReaction
            setCoordinate(paramVector2D1.x, paramVector2D2.y, paramVector2D1.x, paramVector2D1.y)
            setCoordinate(paramVector2D2.x, paramVector2D1.y, paramVector2D2.x, paramVector2D2.y)
            Me.lx = paramVector2D3.x
            Me.ly = paramVector2D3.y
        End Sub

        'JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        'ORIGINAL LINE: public Bracket(Reaction paramReaction, String paramString1, String paramString2, String paramString3) throws keg.common.exception.IllegalFormatException
        Public Sub New(paramReaction As Reaction, paramString1 As String, paramString2 As String, paramString3 As String)
            Me.m_reaction = paramReaction
            If paramString1.Length < 13 Then
                Throw New Exception("Format error in BRACKET Block.")
            End If
            Dim localStringTokenizer As New java.util.StringTokenizer(paramString1.Substring(12))
            Dim str1 As String = localStringTokenizer.nextToken()
            If str1.StartsWith("-") Then
                [select]()
            End If
            Dim localDouble As RectangleF
            Dim d3 As Double
            Dim d4 As Double
            Dim d5 As Double
            Dim str2$

            Try
                str2 = localStringTokenizer.nextToken()
                localDouble = New RectangleF(str2.Trim())
                Dim d1 As Double = CDbl(localDouble)
                str2 = localStringTokenizer.nextToken()
                localDouble = New java.awt.geom.Rectangle2D.Double(str2.Trim())
                d3 = CDbl(localDouble)
                str2 = localStringTokenizer.nextToken()
                localDouble = New java.awt.geom.Rectangle2D.Double(str2.Trim())
                d4 = CDbl(localDouble)
                str2 = localStringTokenizer.nextToken()
                localDouble = New java.awt.geom.Rectangle2D.Double(str2.Trim())
                d5 = CDbl(localDouble)
                setCoordinate(d1, d3, d4, d5)
                If localStringTokenizer.hasMoreTokens() Then
                    str2 = localStringTokenizer.nextToken()
                    If str2.StartsWith("#") Then
                        Me.col1 = New Color(CInt(Convert.ToInt32(str2.Substring(1, 6), 16)))
                    End If
                End If
            Catch localNumberFormatException1 As NumberFormatException
                Console.WriteLine(paramString1)
                Throw New keg.common.exception.IllegalFormatException("Format error in BRACKET Block.")
            End Try
            If paramString2.Length < 13 Then
                Throw New keg.common.exception.IllegalFormatException("Format error in BRACKET Block.")
            End If
            localStringTokenizer = New java.util.StringTokenizer(paramString2.Substring(12))
            Dim str2 As String = localStringTokenizer.nextToken()
            If Not str1.Equals(str2) Then
                Throw New keg.common.exception.IllegalFormatException("Format error in BRACKET block.")
            End If
            Try
                str2 = localStringTokenizer.nextToken()
                localDouble = New java.awt.geom.Rectangle2D.Double(str2.Trim())
                Dim d2 As Double = CDbl(localDouble)
                str2 = localStringTokenizer.nextToken()
                localDouble = New java.awt.geom.Rectangle2D.Double(str2.Trim())
                d3 = CDbl(localDouble)
                str2 = localStringTokenizer.nextToken()
                localDouble = New java.awt.geom.Rectangle2D.Double(str2.Trim())
                d4 = CDbl(localDouble)
                str2 = localStringTokenizer.nextToken()
                localDouble = New java.awt.geom.Rectangle2D.Double(str2.Trim())
                d5 = CDbl(localDouble)
                setCoordinate(d2, d3, d4, d5)
                If localStringTokenizer.hasMoreTokens() Then
                    str2 = localStringTokenizer.nextToken()
                    If str2.StartsWith("#") Then
                        Me.col1 = New Color(CInt(Convert.ToInt32(str2.Substring(1, 6), 16)))
                    End If
                End If
            Catch localNumberFormatException2 As NumberFormatException
                Console.WriteLine(paramString2)
                Throw New keg.common.exception.IllegalFormatException("Format error in BRACKET Block.")
            End Try
            If paramString3.Length < 13 Then
                Throw New keg.common.exception.IllegalFormatException("Format error in BRACKET Block.")
            End If
            localStringTokenizer = New java.util.StringTokenizer(paramString3.Substring(12))
            str2 = localStringTokenizer.nextToken()
            If Not str1.Equals(str2) Then
                Throw New keg.common.exception.IllegalFormatException("Format error in BRACKET block.")
            End If
            Try
                str2 = localStringTokenizer.nextToken()
                localDouble = New java.awt.geom.Rectangle2D.Double(str2.Trim())
                Me.lx = CDbl(localDouble)
                str2 = localStringTokenizer.nextToken()
                localDouble = New java.awt.geom.Rectangle2D.Double(str2.Trim())
                Me.ly = CDbl(localDouble)
                If localStringTokenizer.hasMoreTokens() Then
                    str2 = localStringTokenizer.nextToken()
                    If str2.StartsWith("#") Then
                        Me.col0 = New Color(CInt(Convert.ToInt32(str2.Substring(1, 6), 16)))
                    Else
                        Label = str2
                    End If
                End If
            Catch localNumberFormatException3 As NumberFormatException
                Console.WriteLine(paramString3)
                Throw New keg.common.exception.IllegalFormatException("Format error in BRACKET Block.")
            End Try
        End Sub

        Public Overridable Sub setCoordinate(paramDouble1 As Double, paramDouble2 As Double, paramDouble3 As Double, paramDouble4 As Double)
            Me.setSDI += 1
            If (paramDouble2 < 0.0) AndAlso (paramDouble4 < 0.0) Then
                Dim d As Double
                If Me.setSDI = 1 Then
                    If paramDouble2 > paramDouble4 Then
                        d = paramDouble4
                        paramDouble4 = paramDouble2
                        paramDouble2 = d
                    End If
                ElseIf paramDouble2 < paramDouble4 Then
                    d = paramDouble4
                    paramDouble4 = paramDouble2
                    paramDouble2 = d
                End If
            End If
            Select Case Me.setSDI
                Case 1
                    Me.b1x1 = paramDouble1
                    Me.b1x2 = paramDouble3
                    Me.b1y1 = paramDouble2
                    Me.b1y2 = paramDouble4

                Case 2
                    Me.b2x1 = paramDouble1
                    Me.b2y1 = paramDouble2
                    Me.b2x2 = paramDouble3
                    Me.b2y2 = paramDouble4

            End Select
        End Sub

        Public Overridable Property Label() As String
            Get
                Return Me.m_label
            End Get
            Set
                Me.m_label = Value.Trim()
                Try
                    Convert.ToInt32(Label)
                    Type = "MUL"
                Catch localException As Exception
                    If Me.m_label.ToUpper().Equals("MON") Then
                        Me.m_label = "MON".ToLower()
                        Type = "MON"
                    ElseIf Me.m_label.Length = 0 Then
                        Me.m_label = ""
                        Type = "GEN"
                    Else
                        Type = "SRU"
                    End If
                End Try
            End Set
        End Property


        Public Overridable WriteOnly Property Size() As DblRect
            Set
                Me.width = Value.Width
                Me.height = Value.Height
            End Set
        End Property

        Public Overridable Function size() As DblRect
            Return New DblRect(Me.width, Me.height)
        End Function

        Public Overridable Property Reaction() As Reaction
            Get
                Return Me.m_reaction
            End Get
            Set
                Me.m_reaction = Value
            End Set
        End Property


        Public Overridable WriteOnly Property Recalc() As Boolean
            Set
                Me.m_recalc = Value
            End Set
        End Property

        Public Overridable Sub set0point(paramDimension As DblRect)
            Me.displayX = paramDimension.Width
            Me.displayY = paramDimension.Height
        End Sub

        Public Overridable Sub set0point(paramInt1 As Integer, paramInt2 As Integer)
            Me.displayX = paramInt1
            Me.displayY = paramInt2
        End Sub

        Public Overridable Function get0point() As DblRect
            Return New DblRect(Me.displayX, Me.displayY)
        End Function

        Public Overridable Sub reset0point(paramDimension As DblRect, paramDouble As Double)
            moveInternal(Me.displayX - paramDimension.Width, paramDimension.Height - Me.displayY, paramDouble)
            set0point(paramDimension)
        End Sub

        Public Overridable Function getRect(paramDouble As Double) As Rectangle
            If Me.m_recalc Then
                recalcCoordinate(paramDouble)
            End If
            Return Me.rect
        End Function

        Public Overridable Function DX(paramDouble As Double, paramInt1 As Integer, paramInt2 As Integer) As Integer
            recalcCoordinate(paramDouble)
            Select Case paramInt1 * 10 + paramInt2
                Case 0
                    Return Me.ldx
                Case 11
                    Return Me.c1x1
                Case 12
                    Return Me.c1x2
                Case 13
                    Return Me.c1x3
                Case 14
                    Return Me.c1x4
                Case 21
                    Return Me.c2x1
                Case 22
                    Return Me.c2x2
                Case 23
                    Return Me.c2x3
                Case 24
                    Return Me.c2x4
            End Select
            Return 0
        End Function

        Public Overridable Function DY(paramDouble As Double, paramInt1 As Integer, paramInt2 As Integer) As Integer
            recalcCoordinate(paramDouble)
            Select Case paramInt1 * 10 + paramInt2
                Case 0
                    Return Me.ldy
                Case 11
                    Return Me.c1y1
                Case 12
                    Return Me.c1y2
                Case 13
                    Return Me.c1y3
                Case 14
                    Return Me.c1y4
                Case 21
                    Return Me.c2y1
                Case 22
                    Return Me.c2y2
                Case 23
                    Return Me.c2y3
                Case 24
                    Return Me.c2y4
            End Select
            Return 0
        End Function

        Private Sub recalcCoordinateSub(paramDouble As Double)
            Dim d1 As Double = 0.0
            Dim d2 As Double = 0.0
            Dim d3 As Double = 0.0
        End Sub

        Private Sub recalcCoordinate(paramDouble As Double)
            recalcCoordinateSub(paramDouble)
            Dim i As Integer = 0
            Dim j As Integer = 0
            Me.c1x1 = (Me.displayX + CInt(Math.Truncate(Me.b1x1 * paramDouble)))
            Me.c1y1 = (Me.displayY + CInt(Math.Truncate(Me.b1y1 * paramDouble)))
            Me.c1x4 = (Me.displayX + CInt(Math.Truncate(Me.b1x2 * paramDouble)))
            Me.c1y4 = (Me.displayY + CInt(Math.Truncate(Me.b1y2 * paramDouble)))
            Me.c2x1 = (Me.displayX + CInt(Math.Truncate(Me.b2x1 * paramDouble)))
            Me.c2y1 = (Me.displayY + CInt(Math.Truncate(Me.b2y1 * paramDouble)))
            Me.c2x4 = (Me.displayX + CInt(Math.Truncate(Me.b2x2 * paramDouble)))
            Me.c2y4 = (Me.displayY + CInt(Math.Truncate(Me.b2y2 * paramDouble)))
            Dim k As Integer = Me.ldx
            Dim m As Integer = Me.ldy
            If Me.c1x1 < Me.c2x1 Then
                If Me.c2y1 < Me.c2y4 Then
                    k = Me.c2x4
                    m = Me.c2y4
                Else
                    k = Me.c2x1
                    m = Me.c2y1
                End If
                If Me.c2y1 = Me.c2y4 Then
                    Me.ldx = k
                    Me.ldy = (m + 19)
                ElseIf Me.c2x1 = Me.c2x4 Then
                    Me.ldx = (k + 7)
                    Me.ldy = m
                Else
                    Me.ldx = (k + 7)
                    If (Me.c2x2 > Me.c2x1) AndAlso (Me.c2y2 > Me.c2y1) Then
                        Me.ldy = (m + 19)
                    Else
                        Me.ldy = m
                    End If
                End If
            Else
                If Me.c1y1 < Me.c1y4 Then
                    k = Me.c1x4
                    m = Me.c1y4
                Else
                    k = Me.c1x1
                    m = Me.c1y1
                End If
                If Me.c1y1 = Me.c1y4 Then
                    Me.ldx = k
                    Me.ldy = (m + 19)
                ElseIf Me.c1x1 = Me.c1x4 Then
                    Me.ldx = (k + 7)
                    Me.ldy = m
                Else
                    Me.ldx = (k + 7)
                    If (Me.c1x1 > Me.c1x1) AndAlso (Me.c1y1 > Me.c1y1) Then
                        Me.ldy = (m + 19)
                    Else
                        Me.ldy = m
                    End If
                End If
            End If
            Me.lx = ((Me.ldx - Me.displayX) / paramDouble)
            Me.ly = ((Me.ldy - Me.displayY) / paramDouble)
            k = 1
            m = 1
            If Me.c1x1 < Me.c1x4 Then
                k = -1
                m = -1
                If Me.c1y1 > Me.c1y4 Then
                    k = 1
                End If
            ElseIf (Me.c1x1 <= Me.c1x4) AndAlso (Me.c1y1 < Me.c1y4) Then
                k = -1
                m = -1
            End If
            Dim localVector2D As New Vector2D(Me.b1x2 - Me.b1x1, Me.b1y2 - Me.b1y1)
            Dim d As Double = localVector2D.Length()
            If d >= 0.1 Then
                i = CInt(Math.Truncate(Math.Abs((Me.b1y2 - Me.b1y1) / d * 5.0)))
                j = CInt(Math.Truncate(Math.Abs((Me.b1x2 - Me.b1x1) / d * 5.0)))
            End If
            Me.c1x2 = (Me.c1x1 - i * k)
            Me.c1y2 = (Me.c1y1 + j * m)
            Me.c1x3 = (Me.c1x4 - i * k)
            Me.c1y3 = (Me.c1y4 + j * m)
            k = 1
            m = 1
            If Me.c2x1 >= Me.c2x4 Then
                If Me.c2x1 > Me.c2x4 Then
                    k = -1
                    m = -1
                    If Me.c2y1 < Me.c2y4 Then
                        k = 1
                    End If
                ElseIf Me.c2y1 >= Me.c2y4 Then
                    k = -1
                    m = -1
                End If
            End If
            i = 0
            j = 0
            localVector2D = New Vector2D(Me.b2x2 - Me.b2x1, Me.b2y2 - Me.b2y1)
            d = localVector2D.length()
            If d >= 0.1 Then
                i = CInt(Math.Truncate(Math.Abs((Me.b2y2 - Me.b2y1) / d * 5.0)))
                j = CInt(Math.Truncate(Math.Abs((Me.b2x2 - Me.b2x1) / d * 5.0)))
            End If
            Me.c2x2 = (Me.c2x1 + i * k)
            Me.c2y2 = (Me.c2y1 - j * m)
            Me.c2x3 = (Me.c2x4 + i * k)
            Me.c2y3 = (Me.c2y4 - j * m)
            k = Me.c1x1
            m = Me.c1y1
            Dim n As Integer = k
            Dim i1 As Integer = m
            If Me.c1x4 < k Then
                k = Me.c1x4
            End If
            If Me.c1x4 > n Then
                n = Me.c1x4
            End If
            If Me.c1y4 < m Then
                m = Me.c1y4
            End If
            If Me.c1y4 > i1 Then
                i1 = Me.c1y4
            End If
            If Me.c2x1 < k Then
                k = Me.c2x1
            End If
            If Me.c2x1 > n Then
                n = Me.c2x1
            End If
            If Me.c2y1 < m Then
                m = Me.c2y1
            End If
            If Me.c2y1 > i1 Then
                i1 = Me.c2y1
            End If
            If Me.c2x4 < k Then
                k = Me.c2x4
            End If
            If Me.c2x4 > n Then
                n = Me.c2x4
            End If
            If Me.c2y4 < m Then
                m = Me.c2y4
            End If
            If Me.c2y4 > i1 Then
                i1 = Me.c2y4
            End If
            Me.rect = New Rectangle(k, m, n - k, i1 - m)
            Me.m_recalc = False
        End Sub

        Public Overridable Sub moveInternal(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double)
            Dim d As Double = paramInt1 / paramDouble
            Me.b1x1 += d
            Me.b1x2 += d
            Me.b2x1 += d
            Me.b2x2 += d
            d = paramInt2 / paramDouble
            Me.b1y1 += d
            Me.b1y2 += d
            Me.b2y1 += d
            Me.b2y2 += d
            Me.m_recalc = True
        End Sub

        Public Overridable Sub moveInternaldependSelected(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double)
            Dim d1 As Double = paramInt1 / paramDouble
            Dim d2 As Double = paramInt2 / paramDouble
            If (selectAll) OrElse (SelectSide = 1) Then
                Me.b1x1 += d1
                Me.b1x2 += d1
                Me.b1y1 += d2
                Me.b1y2 += d2
            End If
            If (selectAll) OrElse (SelectSide = 2) Then
                Me.b2x1 += d1
                Me.b2x2 += d1
                Me.b2y1 += d2
                Me.b2y2 += d2
                Me.lx += d1
                Me.ly += d2
            End If
            Me.m_recalc = True
        End Sub

        Public Overrides Sub move(paramInt1 As Integer, paramInt2 As Integer)
            Me.c1x1 += paramInt1
            Me.c1x2 += paramInt1
            Me.c1x3 += paramInt1
            Me.c1x4 += paramInt1
            Me.c2x1 += paramInt1
            Me.c2x2 += paramInt1
            Me.c2x3 += paramInt1
            Me.c2x4 += paramInt1
            Me.ldx += paramInt1
            Me.c1y1 -= paramInt2
            Me.c1y2 -= paramInt2
            Me.c1y3 -= paramInt2
            Me.c1y4 -= paramInt2
            Me.c2y1 -= paramInt2
            Me.c2y2 -= paramInt2
            Me.c2y3 -= paramInt2
            Me.c2y4 -= paramInt2
            Me.ldy -= paramInt2
            Me.m_recalc = True
        End Sub

        Public Overridable Sub zoom(paramDouble As Double)
            Me.b1x1 *= paramDouble
            Me.b1y1 *= paramDouble
            Me.b1x2 *= paramDouble
            Me.b1y2 *= paramDouble
            Me.b2x1 *= paramDouble
            Me.b2y1 *= paramDouble
            Me.b2x2 *= paramDouble
            Me.b2y2 *= paramDouble
            Recalc = True
        End Sub

        Public Overridable Overloads Sub rotate(paramInt1 As Integer, paramInt2 As Integer, paramDouble1 As Double, paramDouble2 As Double)
            Dim d5 As Double = (paramInt1 - Me.displayX) / paramDouble2
            Dim d6 As Double = (paramInt2 - Me.displayY) / paramDouble2
            Dim d1 As Double = Math.Cos(paramDouble1)
            Dim d2 As Double = Math.Sin(paramDouble1)
            Dim d3 As Double = Me.b1x1
            Dim d4 As Double = Me.b1y1
            Me.b1x1 = (d1 * (d3 - d5) - d2 * (d4 - d6) + d5)
            Me.b1y1 = (d2 * (d3 - d5) + d1 * (d4 - d6) + d6)
            d3 = Me.b1x2
            d4 = Me.b1y2
            Me.b1x2 = (d1 * (d3 - d5) - d2 * (d4 - d6) + d5)
            Me.b1y2 = (d2 * (d3 - d5) + d1 * (d4 - d6) + d6)
            d3 = Me.b2x1
            d4 = Me.b2y1
            Me.b2x1 = (d1 * (d3 - d5) - d2 * (d4 - d6) + d5)
            Me.b2y1 = (d2 * (d3 - d5) + d1 * (d4 - d6) + d6)
            d3 = Me.b2x2
            d4 = Me.b2y2
            Me.b2x2 = (d1 * (d3 - d5) - d2 * (d4 - d6) + d5)
            Me.b2y2 = (d2 * (d3 - d5) + d1 * (d4 - d6) + d6)
            Me.m_recalc = True
        End Sub

        Public Overridable Sub rotateDependSelected(paramInt1 As Integer, paramInt2 As Integer, paramDouble1 As Double, paramDouble2 As Double)
            Dim d5 As Double = (paramInt1 - Me.displayX) / paramDouble2
            Dim d6 As Double = (paramInt2 - Me.displayY) / paramDouble2
            Dim d1 As Double = Math.Cos(paramDouble1)
            Dim d2 As Double = Math.Sin(paramDouble1)
            Dim d3 As Double
            Dim d4 As Double
            If (selectAll) OrElse (SelectSide = 1) Then
                d3 = Me.b1x1
                d4 = Me.b1y1
                Me.b1x1 = (d1 * (d3 - d5) - d2 * (d4 - d6) + d5)
                Me.b1y1 = (d2 * (d3 - d5) + d1 * (d4 - d6) + d6)
                d3 = Me.b1x2
                d4 = Me.b1y2
                Me.b1x2 = (d1 * (d3 - d5) - d2 * (d4 - d6) + d5)
                Me.b1y2 = (d2 * (d3 - d5) + d1 * (d4 - d6) + d6)
            End If
            If (selectAll) OrElse (SelectSide = 2) Then
                d3 = Me.b2x1
                d4 = Me.b2y1
                Me.b2x1 = (d1 * (d3 - d5) - d2 * (d4 - d6) + d5)
                Me.b2y1 = (d2 * (d3 - d5) + d1 * (d4 - d6) + d6)
                d3 = Me.b2x2
                d4 = Me.b2y2
                Me.b2x2 = (d1 * (d3 - d5) - d2 * (d4 - d6) + d5)
                Me.b2y2 = (d2 * (d3 - d5) + d1 * (d4 - d6) + d6)
            End If
            Me.m_recalc = True
        End Sub

        Public Overridable Function nearBracket(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double, paramInt3 As Integer) As Bracket
            If VecMath2D.isNear(DX(paramDouble, 1, 1), DY(paramDouble, 1, 1), DX(paramDouble, 1, 2), DY(paramDouble, 1, 2), paramInt1, paramInt2,
                paramInt3) Then
                Return Me
            End If
            If VecMath2D.isNear(DX(paramDouble, 1, 2), DY(paramDouble, 1, 2), DX(paramDouble, 1, 3), DY(paramDouble, 1, 3), paramInt1, paramInt2,
                paramInt3) Then
                Return Me
            End If
            If VecMath2D.isNear(DX(paramDouble, 1, 3), DY(paramDouble, 1, 3), DX(paramDouble, 1, 4), DY(paramDouble, 1, 4), paramInt1, paramInt2,
                paramInt3) Then
                Return Me
            End If
            If VecMath2D.isNear(DX(paramDouble, 2, 1), DY(paramDouble, 2, 1), DX(paramDouble, 2, 2), DY(paramDouble, 2, 2), paramInt1, paramInt2,
                paramInt3) Then
                Return Me
            End If
            If VecMath2D.isNear(DX(paramDouble, 2, 2), DY(paramDouble, 2, 2), DX(paramDouble, 2, 3), DY(paramDouble, 2, 3), paramInt1, paramInt2,
                paramInt3) Then
                Return Me
            End If
            If VecMath2D.isNear(DX(paramDouble, 2, 3), DY(paramDouble, 2, 3), DX(paramDouble, 2, 4), DY(paramDouble, 2, 4), paramInt1, paramInt2,
                paramInt3) Then
                Return Me
            End If
            Return Nothing
        End Function

        Public Overridable Function nearBracketLabel(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double, paramInt3 As Integer) As Bracket
            Dim localRectangle As Rectangle = getBracketLabelMarkArea(paramDouble)
            localRectangle.X -= paramInt3
            localRectangle.Y -= paramInt3
            localRectangle.Width += 2 * paramInt3
            localRectangle.Height += 2 * paramInt3
            Return If(localRectangle.Contains(paramInt1, paramInt2) = True, Me, Nothing)
        End Function

        Public Overridable Function getBracketLabelMarkArea(paramDouble As Double) As Rectangle
            Dim i As Integer = If(Me.width > 10, Me.width, 10)
            Dim j As Integer = If(Me.height > 10, Me.height, 10)
            Dim k As Integer = DX(paramDouble, 0, 0)
            Dim m As Integer = DY(paramDouble, 0, 0) - j
            Return New Rectangle(k, m, i, j)
        End Function

        Public Overridable Sub resize(paramInt1 As Integer, paramInt2 As Integer, paramDouble1 As Double, paramDouble2 As Double, paramDouble3 As Double)
            Dim d1 As Double = (paramInt1 - Me.displayX) / paramDouble3
            Dim d2 As Double = (paramInt2 - Me.displayY) / paramDouble3
            Me.b1x1 = ((Me.b1x1 - d1) * paramDouble1 + d1)
            Me.b1y1 = ((Me.b1y1 - d2) * paramDouble2 + d2)
            Me.b1x2 = ((Me.b1x2 - d1) * paramDouble1 + d1)
            Me.b1y2 = ((Me.b1y2 - d2) * paramDouble2 + d2)
            Me.b2x1 = ((Me.b2x1 - d1) * paramDouble1 + d1)
            Me.b2y1 = ((Me.b2y1 - d2) * paramDouble2 + d2)
            Me.b2x2 = ((Me.b2x2 - d1) * paramDouble1 + d1)
            Me.b2y2 = ((Me.b2y2 - d2) * paramDouble2 + d2)
            Me.m_recalc = True
        End Sub

        Public Overridable Sub resizeDependSelected(paramInt1 As Integer, paramInt2 As Integer, paramDouble1 As Double, paramDouble2 As Double, paramDouble3 As Double)
            Dim d1 As Double = (paramInt1 - Me.displayX) / paramDouble3
            Dim d2 As Double = (paramInt2 - Me.displayY) / paramDouble3
            If (selectAll) OrElse (SelectSide = 1) Then
                Me.b1x1 = ((Me.b1x1 - d1) * paramDouble1 + d1)
                Me.b1y1 = ((Me.b1y1 - d2) * paramDouble2 + d2)
                Me.b1x2 = ((Me.b1x2 - d1) * paramDouble1 + d1)
                Me.b1y2 = ((Me.b1y2 - d2) * paramDouble2 + d2)
            End If
            If (selectAll) OrElse (SelectSide = 2) Then
                Me.b2x1 = ((Me.b2x1 - d1) * paramDouble1 + d1)
                Me.b2y1 = ((Me.b2y1 - d2) * paramDouble2 + d2)
                Me.b2x2 = ((Me.b2x2 - d1) * paramDouble1 + d1)
                Me.b2y2 = ((Me.b2y2 - d2) * paramDouble2 + d2)
            End If
            Me.m_recalc = True
        End Sub

        <MethodImpl(MethodImplOptions.Synchronized)>
        Public Overrides Function clone() As Object
            Dim localBracket As New Bracket()
            localBracket.Label = Me.m_label
            localBracket.b1x1 = Me.b1x1
            localBracket.b1y1 = Me.b1y1
            localBracket.b1x2 = Me.b1x2
            localBracket.b1y2 = Me.b1y2
            localBracket.b2x1 = Me.b2x1
            localBracket.b2y1 = Me.b2y1
            localBracket.b2x2 = Me.b2x2
            localBracket.b2y2 = Me.b2y2
            localBracket.displayX = Me.displayX
            localBracket.displayY = Me.displayY
            Return localBracket
        End Function

        Public Overridable Function confirm_Bond(paramBond As Bond) As Boolean
            Dim bool As Boolean = False
            Dim localAtom1 As Atom = paramBond.Atom1
            Dim localAtom2 As Atom = paramBond.Atom2
            If isIntersect(localAtom1.x, localAtom1.y, localAtom2.x, localAtom2.y, Me.b1x1, Me.b1y1,
                Me.b1x2, Me.b1y2) Then
                If Not Me.bonds1.Contains(paramBond) Then
                    Me.bonds1.Add(paramBond)
                End If
                bool = True
            End If
            If (Not bool) AndAlso (Me.bonds1.Contains(paramBond)) Then
                Me.bonds1.Remove(paramBond)
            End If
            Dim i As Integer = 0
            If isIntersect(localAtom1.x, localAtom1.y, localAtom2.x, localAtom2.y, Me.b2x1, Me.b2y1,
                Me.b2x2, Me.b2y2) Then
                If Not Me.bonds2.Contains(paramBond) Then
                    Me.bonds2.Add(paramBond)
                End If
                i = 1
            End If
            If (i = 0) AndAlso (Me.bonds2.Contains(paramBond)) Then
                Me.bonds2.Remove(paramBond)
            End If
            Return bool
        End Function

        Public Overridable Sub computeParentMol()
            Dim i As Integer = 0
            For j As Integer = 0 To Me.bonds1.Count - 1
                Dim localBond1 As Bond = DirectCast(Me.bonds1(j), Bond)
                If Me.molecules.Count = 0 Then
                    addMol(localBond1.Mol)
                    i = 1
                Else
                    If Not Me.molecules.Contains(localBond1.Mol) Then
                        i = 0
                        Exit For
                    End If
                    i = 1
                End If
            Next
            j = 0
            For k As Integer = 0 To Me.bonds2.Count - 1
                Dim localBond2 As Bond = DirectCast(Me.bonds2(k), Bond)
                If Me.molecules.Count = 0 Then
                    addMol(localBond2.Mol)
                    j = 1
                Else
                    If Not Me.molecules.Contains(localBond2.Mol) Then
                        j = 0
                        Exit For
                    End If
                    j = 1
                End If
            Next
            If (i = 0) AndAlso (j = 0) Then
                Me.isWholeMol = True
            Else
                Me.isWholeMol = False
            End If
        End Sub

        Private Shared Function calc_CrossPoint(paramDouble1 As Double, paramDouble2 As Double, paramDouble3 As Double, paramDouble4 As Double, paramDouble5 As Double, paramDouble6 As Double,
            paramDouble7 As Double, paramDouble8 As Double) As Double()
            Dim d3 As Double
            Dim d4 As Double
            Dim d5 As Double
            Dim d6 As Double
            If paramDouble3 - paramDouble1 = 0.0 Then
                d3 = (paramDouble8 - paramDouble6) / (paramDouble7 - paramDouble5)
                d4 = paramDouble8 - d3 * paramDouble7
                d5 = paramDouble1
                d6 = d3 * d5 + d4
            Else
                Dim d1 As Double
                Dim d2 As Double
                If paramDouble7 - paramDouble5 = 0.0 Then
                    d1 = (paramDouble4 - paramDouble2) / (paramDouble3 - paramDouble1)
                    d2 = paramDouble4 - d1 * paramDouble3
                    d5 = paramDouble5
                    d6 = d1 * d5 + d2
                Else
                    d1 = (paramDouble4 - paramDouble2) / (paramDouble3 - paramDouble1)
                    d2 = paramDouble4 - d1 * paramDouble3
                    d3 = (paramDouble8 - paramDouble6) / (paramDouble7 - paramDouble5)
                    d4 = paramDouble8 - d3 * paramDouble7
                    d5 = (d4 - d2) / (d1 - d3)
                    d6 = d1 * d5 + d2
                End If
            End If
            Dim arrayOfDouble As Double() = {d5, d6}
            Return arrayOfDouble
        End Function

        Private Shared Function turn(paramDouble1 As Double, paramDouble2 As Double, paramDouble3 As Double, paramDouble4 As Double, paramDouble5 As Double, paramDouble6 As Double) As Integer
            Dim d1 As Double = paramDouble3 - paramDouble1
            Dim d2 As Double = paramDouble4 - paramDouble2
            Dim d3 As Double = paramDouble5 - paramDouble1
            Dim d4 As Double = paramDouble6 - paramDouble2
            If d1 * d4 > d2 * d3 Then
                Return 1
            End If
            If d1 * d4 < d2 * d3 Then
                Return -1
            End If
            If (d1 * d3 < 0.0) OrElse (d2 * d4 < 0.0) Then
                Return -1
            End If
            If d1 * d1 + d2 * d2 < d3 * d3 + d4 * d4 Then
                Return 1
            End If
            Return 0
        End Function

        Private Shared Function isIntersect(paramDouble1 As Double, paramDouble2 As Double, paramDouble3 As Double, paramDouble4 As Double, paramDouble5 As Double, paramDouble6 As Double,
            paramDouble7 As Double, paramDouble8 As Double) As Boolean
            If ((paramDouble1 = paramDouble5) AndAlso (paramDouble2 = paramDouble6)) OrElse ((paramDouble1 = paramDouble7) AndAlso (paramDouble2 = paramDouble8)) OrElse ((paramDouble3 = paramDouble5) AndAlso (paramDouble4 = paramDouble6)) OrElse ((paramDouble3 = paramDouble7) AndAlso (paramDouble4 = paramDouble8)) Then
                Return False
            End If
            Return (turn(paramDouble1, paramDouble2, paramDouble3, paramDouble4, paramDouble5, paramDouble6) * turn(paramDouble1, paramDouble2, paramDouble3, paramDouble4, paramDouble7, paramDouble8) <= 0) AndAlso (turn(paramDouble5, paramDouble6, paramDouble7, paramDouble8, paramDouble1, paramDouble2) * turn(paramDouble5, paramDouble6, paramDouble7, paramDouble8, paramDouble3, paramDouble4) <= 0)
        End Function

        Public Overridable Function toKCFStrings(paramInt As Integer) As String()
            Return toKCFStrings(paramInt, False, 1)
        End Function

        Public Overridable Function toKCFStrings(paramInt1 As Integer, paramBoolean As Boolean, paramInt2 As Integer) As String()
            Dim arrayOfString As String() = New String(2) {}
            Dim str1 As String = ""
            Dim d1 As Double = 0.0
            Dim d2 As Double = 0.0
            Dim str2 As String = Convert.ToString(paramInt1)
            If Me.col1 Is Nothing Then
                str1 = ""
            Else
                str1 = " #" & keg.common.util.DEBTutil.printf(Integer.toHexString(Me.col1.Red), "2", "0"c) & keg.common.util.DEBTutil.printf(Integer.toHexString(Me.col1.Green), "2", "0"c) & keg.common.util.DEBTutil.printf(Integer.toHexString(Me.col1.Blue), "2", "0"c)
            End If
            If paramInt1 = 1 Then
                arrayOfString(0) = ("BRACKET     " & str2 & " " & keg.common.util.DEBTutil.printf(Me.b1x1 + d1, "4.4") & " " & keg.common.util.DEBTutil.printf(-(Me.b1y1 + d2), "4.4") & " " & keg.common.util.DEBTutil.printf(Me.b1x2 + d1, "4.4") & " " & keg.common.util.DEBTutil.printf(-(Me.b1y2 + d2), "4.4") & str1)
            Else
                arrayOfString(0) = ("            " & str2 & " " & keg.common.util.DEBTutil.printf(Me.b1x1 + d1, "4.4") & " " & keg.common.util.DEBTutil.printf(-(Me.b1y1 + d2), "4.4") & " " & keg.common.util.DEBTutil.printf(Me.b1x2 + d1, "4.4") & " " & keg.common.util.DEBTutil.printf(-(Me.b1y2 + d2), "4.4") & str1)
            End If
            If Me.col2 Is Nothing Then
                str1 = ""
            Else
                str1 = " #" & keg.common.util.DEBTutil.printf(Integer.toHexString(Me.col1.Red), "2", "0"c) & keg.common.util.DEBTutil.printf(Integer.toHexString(Me.col1.Green), "2", "0"c) & keg.common.util.DEBTutil.printf(Integer.toHexString(Me.col1.Blue), "2", "0"c)
            End If
            arrayOfString(1) = ("            " & str2 & " " & keg.common.util.DEBTutil.printf(Me.b2x1 + d1, "4.4") & " " & keg.common.util.DEBTutil.printf(-(Me.b2y1 + d2), "4.4") & " " & keg.common.util.DEBTutil.printf(Me.b2x2 + d1, "4.4") & " " & keg.common.util.DEBTutil.printf(-(Me.b2y2 + d2), "4.4") & str1)
            If Me.col0 Is Nothing Then
                str1 = ""
            Else
                str1 = " #" & keg.common.util.DEBTutil.printf(Integer.toHexString(Me.col1.Red), "2", "0"c) & keg.common.util.DEBTutil.printf(Integer.toHexString(Me.col1.Green), "2", "0"c) & keg.common.util.DEBTutil.printf(Integer.toHexString(Me.col1.Blue), "2", "0"c)
            End If
            arrayOfString(2) = ("            " & str2 & " " & keg.common.util.DEBTutil.printf(Me.lx + d1, "4.4") & " " & keg.common.util.DEBTutil.printf(-(Me.ly + d2), "4.4") & " " & Label & str1)
            Return arrayOfString
        End Function

        Public Overridable Sub addSgroup(paramVector As ArrayList)
            For i As Integer = 0 To paramVector.Count - 1
                Dim localObject As Object = paramVector(i)
                If Not Me.m_sgroup.Contains(localObject) Then
                    Me.m_sgroup.Add(localObject)
                End If
            Next
        End Sub

        Public Overridable ReadOnly Property Sgroup() As ArrayList
            Get
                Return Me.m_sgroup
            End Get
        End Property

        Public Overridable ReadOnly Property Bonds() As ArrayList
            Get
                Dim localVector As New ArrayList()
                For i As Integer = 0 To Me.bonds1.Count - 1
                    localVector.Add(DirectCast(Me.bonds1(i), Bond))
                Next
                For i = 0 To Me.bonds2.Count - 1
                    localVector.Add(DirectCast(Me.bonds2(i), Bond))
                Next
                Return localVector
            End Get
        End Property

        Public Overridable Property SBL1() As Integer
            Get
                Return Me.m_sbl1
            End Get
            Set
                Me.m_sbl1 = Value
            End Set
        End Property

        Public Overridable Property SBL2() As Integer
            Get
                Return Me.m_sbl2
            End Get
            Set
                Me.m_sbl2 = Value
            End Set
        End Property



        Public Overridable ReadOnly Property Mol() As Molecule()
            Get
                Return DirectCast(Me.molecules.ToArray(GetType(Molecule)), Molecule())
            End Get
        End Property

        Public Overridable Function contains(paramAtom As Atom) As Boolean
            Dim d1 As Double = Me.b1x1
            Dim d2 As Double = Me.b1x1
            d1 = Math.Min(d1, Me.b1x2)
            d1 = Math.Min(d1, Me.b2x1)
            d1 = Math.Min(d1, Me.b2x2)
            d2 = Math.Max(d2, Me.b1x2)
            d2 = Math.Max(d2, Me.b2x1)
            d2 = Math.Max(d2, Me.b2x2)
            Dim d3 As Double = Me.b1y1
            Dim d4 As Double = Me.b1y1
            d3 = Math.Min(d3, Me.b1y2)
            d3 = Math.Min(d3, Me.b2y1)
            d3 = Math.Min(d3, Me.b2y2)
            d4 = Math.Max(d4, Me.b1y2)
            d4 = Math.Max(d4, Me.b2y1)
            d4 = Math.Max(d4, Me.b2y2)
            Me.rect2d.setRect(d1, d3, d2 - d1, d4 - d3)
            Return Me.rect2d.Contains(paramAtom.x, paramAtom.y)
        End Function

        Public Overridable Sub addMol(paramMolecule As Molecule)
            If (paramMolecule IsNot Nothing) AndAlso (Not Me.molecules.Contains(paramMolecule)) Then
                Me.molecules.Add(paramMolecule)
                paramMolecule.addBracket(Me)
            End If
        End Sub

        Public Overridable ReadOnly Property CoveredWholeMol() As Boolean
            Get
                Return Me.isWholeMol
            End Get
        End Property

        Public Overridable Sub clearSgroup()
            If Me.m_sgroup IsNot Nothing Then
                Me.m_sgroup.Clear()
            End If
        End Sub

        Public Overridable Sub removeMol(paramMolecule As Molecule)
            Me.molecules.Remove(paramMolecule)
            If Me.m_reaction IsNot Nothing Then
                Me.m_reaction.refineBracket()
            End If
        End Sub

        Public Overridable Sub clearMol()
            Me.molecules.Clear()
        End Sub

        Public Overrides Sub flipHorizontal(paramDouble1 As Double, paramDouble2 As Double)
            If (paramDouble1 - 1.0 < Me.b1x1) AndAlso (Me.b1x1 < paramDouble2 + 1.0) Then
                Me.b1x1 = (paramDouble2 - (Me.b1x1 - paramDouble1))
            End If
            If (paramDouble1 - 1.0 < Me.b1x2) AndAlso (Me.b1x2 < paramDouble2 + 1.0) Then
                Me.b1x2 = (paramDouble2 - (Me.b1x2 - paramDouble1))
            End If
            If (paramDouble1 - 1.0 < Me.b2x1) AndAlso (Me.b2x1 < paramDouble2 + 1.0) Then
                Me.b2x1 = (paramDouble2 - (Me.b2x1 - paramDouble1))
            End If
            If (paramDouble1 - 1.0 < Me.b2x2) AndAlso (Me.b2x2 < paramDouble2 + 1.0) Then
                Me.b2x2 = (paramDouble2 - (Me.b2x2 - paramDouble1))
            End If
            Dim d1 As Double
            If (Me.b1x1 > Me.b2x1) OrElse (Me.b1x2 > Me.b2x2) Then
                d1 = Me.b2x1
                Me.b2x1 = Me.b1x1
                Me.b1x1 = d1
                Dim d2 As Double = Me.b2y1
                Me.b2y1 = Me.b1y1
                Me.b1y1 = d2
                Dim d3 As Double = Me.b2x2
                Me.b2x2 = Me.b1x2
                Me.b1x2 = d3
                Dim d4 As Double = Me.b2y2
                Me.b2y2 = Me.b1y2
                Me.b1y2 = d4
            End If
            If Me.b1y1 < Me.b1y2 Then
                d1 = Me.b1y1
                Me.b1y1 = Me.b1y2
                Me.b1y2 = d1
            End If
            If Me.b2y1 > Me.b2y2 Then
                d1 = Me.b2y1
                Me.b2y1 = Me.b2y2
                Me.b2y2 = d1
            End If
        End Sub

        Public Overrides Sub flipVertical(paramDouble1 As Double, paramDouble2 As Double)
            If (paramDouble1 - 1.0 < Me.b1y1) AndAlso (Me.b1y1 < paramDouble2 + 1.0) Then
                Me.b1y1 = (paramDouble2 - (Me.b1y1 - paramDouble1))
            End If
            If (paramDouble1 - 1.0 < Me.b1y2) AndAlso (Me.b1y2 < paramDouble2 + 1.0) Then
                Me.b1y2 = (paramDouble2 - (Me.b1y2 - paramDouble1))
            End If
            If (paramDouble1 - 1.0 < Me.b2y1) AndAlso (Me.b2y1 < paramDouble2 + 1.0) Then
                Me.b2y1 = (paramDouble2 - (Me.b2y1 - paramDouble1))
            End If
            If (paramDouble1 - 1.0 < Me.b1y1) AndAlso (Me.b1y1 < paramDouble2 + 1.0) Then
                Me.b2y2 = (paramDouble2 - (Me.b2y2 - paramDouble1))
            End If
            Dim d As Double
            If Me.b1y1 < Me.b1y2 Then
                d = Me.b1y1
                Me.b1y1 = Me.b1y2
                Me.b1y2 = d
            End If
            If Me.b2y1 > Me.b2y2 Then
                d = Me.b2y1
                Me.b2y1 = Me.b2y2
                Me.b2y2 = d
            End If
        End Sub

        Public Overridable Function hasOnlyOneMol() As Boolean
            If Me.m_sgroup.Count = 0 Then
                Return False
            End If
            Dim localAtom As Atom = DirectCast(Me.m_sgroup(0), Atom)
            Dim localVector As ArrayList = localAtom.Mol.getAtomsList(localAtom)
            Return localVector.Count = Me.m_sgroup.Count
        End Function

        Public Overridable ReadOnly Property Limited() As Boolean
            Get
                Try
                    CInt(Convert.ToInt32(Label))
                    Return True
                Catch
                End Try
                Return False
            End Get
        End Property

        Public Overridable ReadOnly Property LimitedNumber() As Integer
            Get
                Try
                    Return CInt(Convert.ToInt32(Label))
                Catch
                End Try
                Return -1
            End Get
        End Property

        Public Overridable ReadOnly Property SimpleRepeat() As Boolean
            Get
                Dim i As Integer = If((Me.bonds1.Count < 2) OrElse (isSameLocatedBonds(Me.bonds1)), 1, 0)
                Dim j As Integer = If((Me.bonds2.Count < 2) OrElse (isSameLocatedBonds(Me.bonds2)), 1, 0)
                Return (i <> 0) AndAlso (j <> 0)
            End Get
        End Property

        Private Function isSameLocatedBonds(paramVector As ArrayList) As Boolean
            If paramVector.Count < 2 Then
                Return True
            End If
            Dim bool As Boolean = True
            Dim localBond1 As Bond = DirectCast(paramVector(0), Bond)
            Dim i As Integer = localBond1.Atom1.DX(1.0)
            Dim j As Integer = localBond1.Atom1.DY(1.0)
            Dim k As Integer = localBond1.Atom2.DX(1.0)
            Dim m As Integer = localBond1.Atom2.DY(1.0)
            Dim n As Integer = 1
            While (n < paramVector.Count) AndAlso (bool = True)
                Dim localBond2 As Bond = DirectCast(paramVector(n), Bond)
                If (i <> localBond2.Atom1.DX(1.0)) OrElse (j <> localBond2.Atom1.DY(1.0)) OrElse (k <> localBond2.Atom2.DX(1.0)) OrElse (m <> localBond2.Atom2.DY(1.0)) Then
                    bool = False
                End If
                n += 1
            End While
            Return bool
        End Function

        Public Overridable ReadOnly Property OneHand() As Boolean
            Get
                Dim i As Integer = Me.bonds1.Count
                Dim j As Integer = Me.bonds2.Count
                Return ((i = 0) AndAlso (j > 0)) OrElse ((i > 0) AndAlso (j = 0))
            End Get
        End Property

        Public Overridable ReadOnly Property SelectSide() As Integer
            Get
                Return Me.m_selectSide
            End Get
        End Property

        Public Overridable Overloads Sub [select](paramEditMode As EditMode, paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double, paramInt3 As Integer)
            Me.m_selectSide = -1
            Dim localBracket1 As Bracket = nearBracket_1stSide(paramInt1, paramInt2, paramDouble, paramInt3)
            If localBracket1 IsNot Nothing Then
                Me.m_selectSide = 1
            End If
            Dim localBracket2 As Bracket = nearBracket_2ndSide(paramInt1, paramInt2, paramDouble, paramInt3)
            If localBracket2 IsNot Nothing Then
                Me.m_selectSide = 2
            End If
            Me.__selectAll = False
            [select](paramEditMode)
        End Sub

        Public Overridable Sub selectAll(paramEditMode As EditMode)
            Me.m_selectSide = -1
            Me.__selectAll = True
            [select](paramEditMode)
        End Sub

        Public Overridable Property Type() As String
            Get
                Return Me.m_type
            End Get
            Set
                If "MON".Equals(Value) Then
                    Me.m_label = "MON".ToLower()
                ElseIf "GEN".Equals(Value) Then
                    Me.m_label = ""
                End If
                Me.m_type = Value
            End Set
        End Property

        Private Function nearBracket_2ndSide(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double, paramInt3 As Integer) As Bracket
            If VecMath2D.isNear(DX(paramDouble, 2, 1), DY(paramDouble, 2, 1), DX(paramDouble, 2, 2), DY(paramDouble, 2, 2), paramInt1, paramInt2,
                paramInt3) Then
                Return Me
            End If
            If VecMath2D.isNear(DX(paramDouble, 2, 2), DY(paramDouble, 2, 2), DX(paramDouble, 2, 3), DY(paramDouble, 2, 3), paramInt1, paramInt2,
                paramInt3) Then
                Return Me
            End If
            If VecMath2D.isNear(DX(paramDouble, 2, 3), DY(paramDouble, 2, 3), DX(paramDouble, 2, 4), DY(paramDouble, 2, 4), paramInt1, paramInt2,
                paramInt3) Then
                Return Me
            End If
            Return Nothing
        End Function

        Private Function nearBracket_1stSide(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double, paramInt3 As Integer) As Bracket
            If VecMath2D.isNear(DX(paramDouble, 1, 1), DY(paramDouble, 1, 1), DX(paramDouble, 1, 2), DY(paramDouble, 1, 2), paramInt1, paramInt2,
                paramInt3) Then
                Return Me
            End If
            If VecMath2D.isNear(DX(paramDouble, 1, 2), DY(paramDouble, 1, 2), DX(paramDouble, 1, 3), DY(paramDouble, 1, 3), paramInt1, paramInt2,
                paramInt3) Then
                Return Me
            End If
            If VecMath2D.isNear(DX(paramDouble, 1, 3), DY(paramDouble, 1, 3), DX(paramDouble, 1, 4), DY(paramDouble, 1, 4), paramInt1, paramInt2,
                paramInt3) Then
                Return Me
            End If
            Return Nothing
        End Function

        Public Overridable ReadOnly Property SelectAll() As Boolean
            Get
                Return Me.__selectAll
            End Get
        End Property

        Public Overrides Sub unselect()
            [select](False)
            Me.__selectAll = False
        End Sub

        Public Overrides Sub unselect(paramEditMode As EditMode)
            [select](False, paramEditMode)
            Me.__selectAll = False
        End Sub


        Public Overridable Function containBracket(paramBracket As Bracket) As Boolean
            Dim localRectangle1 As Rectangle = getRect(1.0)
            Dim localRectangle2 As Rectangle = paramBracket.getRect(1.0)
            If localRectangle1.contains(localRectangle2) = True Then
                Return True
            End If
            Return localRectangle2.contains(localRectangle1) = True
        End Function

        Public Overridable Sub putSBL(paramInt As Integer)
            If SBL1 = 0 Then
                SBL1 = paramInt
            ElseIf SBL2 = 0 Then
                SBL2 = paramInt
            Else
                SBL2 = SBL1
                SBL1 = paramInt
            End If
        End Sub

        Public Overridable Property SgroupConnectivity() As String
            Get
                Return Me.m_sgroupConnectivity
            End Get
            Set
                Me.m_sgroupConnectivity = Value
            End Set
        End Property

    End Class
End Namespace
