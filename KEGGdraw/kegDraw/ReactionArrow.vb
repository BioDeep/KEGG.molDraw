#Region "Microsoft.VisualBasic::9e9bfd06902adfe3137cb94471456b7d, src\visualize\KCF\KEGGdraw\kegDraw\ReactionArrow.vb"

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

    ' 	Class ReactionArrow
    ' 
    ' 	    Properties: Color, Direction, Length
    ' 
    ' 	    Constructor: (+2 Overloads) Sub New
    ' 
    ' 	    Function: arrowHead, DX1, DX2, DY1, DY2
    '                nearReactionArrow
    ' 
    ' 	    Sub: resize, rotate, (+2 Overloads) selectItems
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Collections

Namespace keg.compound


	Public Class ReactionArrow
		Inherits ChemObject
		Public Const RIGHT As Integer = 0
		Public Const UP As Integer = 1
		Public Const LEFT As Integer = 2
		Public Const DOWN As Integer = 3
		Friend m_direction As Integer
		Friend m_length As Integer
		Friend Const HEAD_WIDTH_N As Integer = 5
		Friend Const HEAD_LENGTH_N As Integer = 15
		Friend Const HEAD_INSIDE_N As Integer = 11
		Friend Const HEAD_WIDTH_S As Integer = 4
		Friend Const HEAD_LENGTH_S As Integer = 4
		Friend Const HEAD_INSIDE_S As Integer = 4
		Friend head_width As Integer = 5
		Friend head_length As Integer = 15
		Friend head_inside As Integer = 11
		Public col As Color = Nothing
		Public Shadows id As String = ""

		Public Sub New(paramInt1 As Integer, paramInt2 As Integer, paramInt3 As Integer, paramInt4 As Integer)
			Me.displayX = paramInt1
			Me.displayY = paramInt2
			Me.m_direction = paramInt3
			Me.m_length = paramInt4
		End Sub

		Public Sub New(paramInt1 As Integer, paramInt2 As Integer, paramInt3 As Integer, paramInt4 As Integer, paramBoolean As Boolean)
			Me.displayX = paramInt1
			Me.displayY = paramInt2
			Me.m_direction = paramInt3
			Me.m_length = paramInt4
			If paramBoolean Then
				Me.head_width = 4
				Me.head_length = 4
				Me.head_inside = 4
			End If
		End Sub

		Public Overridable ReadOnly Property Direction() As Integer
			Get
				Return Me.m_direction
			End Get
		End Property

		Public Overridable ReadOnly Property Length() As Integer
			Get
				Return Me.m_length
			End Get
		End Property

		Public Overridable Function DX1() As Integer
			Return Me.displayX
		End Function

		Public Overridable Function DY1() As Integer
			Return Me.displayY
		End Function

		Public Overridable Function DX2() As Integer
			Dim i As Integer = 0
			Select Case Me.m_direction
				Case 0
					i = Me.displayX + Me.m_length
					
				Case 2
					i = Me.displayX - Me.m_length
					
				Case 1, 3
					i = Me.displayX
					
			End Select
			Return i
		End Function

		Public Overridable Function DY2() As Integer
			Dim i As Integer = 0
			Select Case Me.m_direction
				Case 0, 2
					i = Me.displayY
					
				Case 1
					i = Me.displayY - Me.m_length
					
				Case 3
					i = Me.displayY + Me.m_length
					
			End Select
			Return i
		End Function

		Public Overridable Function arrowHead() As ArrayList
			Dim localVector As New ArrayList()
			localVector.Add(New DblRect(DX2(), DY2()))
			Select Case Me.m_direction
				Case 0
					localVector.Add(New DblRect(DX2() - Me.head_length, DY2() - Me.head_width))
					localVector.Add(New DblRect(DX2() - Me.head_inside, DY2()))
					localVector.Add(New DblRect(DX2() - Me.head_length, DY2() + Me.head_width))
					
				Case 2
					localVector.Add(New DblRect(DX2() + Me.head_length, DY2() + Me.head_width))
					localVector.Add(New DblRect(DX2() + Me.head_inside, DY2()))
					localVector.Add(New DblRect(DX2() + Me.head_length, DY2() - Me.head_width))
					
				Case 1
					localVector.Add(New DblRect(DX2() - Me.head_width, DY2() + Me.head_length))
					localVector.Add(New DblRect(DX2(), DY2() + Me.head_inside))
					localVector.Add(New DblRect(DX2() + Me.head_width, DY2() + Me.head_length))
					
				Case 3
					localVector.Add(New DblRect(DX2() + Me.head_width, DY2() - Me.head_length))
					localVector.Add(New DblRect(DX2(), DY2() - Me.head_inside))
					localVector.Add(New DblRect(DX2() - Me.head_width, DY2() - Me.head_length))
					
			End Select
			localVector.Add(New DblRect(DX2(), DY2()))
			Return localVector
		End Function

		Public Overrides Property Color() As Color
			Get
				Return Me.col
			End Get
			Set
				Me.col = value
			End Set
		End Property


		Public Overrides Sub rotate(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double)
			Dim i As Integer = 0
			Dim j As Integer = 0
			Dim d1 As Double = Math.Cos(paramDouble)
			Dim d2 As Double = Math.Sin(paramDouble)
			Select Case Me.m_direction
				Case 0
					i = Me.displayX + Me.m_length \ 2
					j = Me.displayY
					
				Case 1
					i = Me.displayX
					j = Me.displayY - Me.m_length \ 2
					
				Case 2
					i = Me.displayX - Me.m_length \ 2
					j = Me.displayY
					
				Case 3
					i = Me.displayX
					j = Me.displayY + Me.m_length \ 2
					
			End Select
			Me.displayX = (CInt(Math.Truncate(d1 * (i - paramInt1) - d2 * (j - paramInt2))) + paramInt1)
			Me.displayY = (CInt(Math.Truncate(d2 * (i - paramInt1) + d1 * (j - paramInt2))) + paramInt2)
			If paramDouble > 0.0 Then
				Me.m_direction += 4 - CInt(Math.Truncate((paramDouble + 0.785398163397448) * 2.0 / 3.14159265358979))
			Else
				Me.m_direction += 4 - CInt(Math.Truncate((paramDouble - 0.785398163397448) * 2.0 / 3.14159265358979))
			End If
			Me.m_direction = Me.m_direction Mod 4
			Select Case Me.m_direction
				Case 0
					Me.displayX -= Me.m_length \ 2
					
				Case 1
					Me.displayY += Me.m_length \ 2
					
				Case 2
					Me.displayX += Me.m_length \ 2
					
				Case 3
					Me.displayY -= Me.m_length \ 2
					
			End Select
		End Sub

		Public Overridable Sub resize(paramInt1 As Integer, paramInt2 As Integer, paramDouble1 As Double, paramDouble2 As Double)
			Me.displayX = (CInt(Math.Truncate((Me.displayX - paramInt1) * paramDouble1)) + paramInt1)
			Me.displayY = (CInt(Math.Truncate((Me.displayY - paramInt2) * paramDouble2)) + paramInt2)
			Select Case Me.m_direction
				Case 0, 2
					Me.m_length = CInt(Math.Truncate(Me.m_length * paramDouble1))
					
				Case 1, 3
					Me.m_length = CInt(Math.Truncate(Me.m_length * paramDouble2))
					
			End Select
		End Sub

		Public Overridable Sub selectItems(paramInt1 As Integer, paramInt2 As Integer, paramInt3 As Integer, paramInt4 As Integer, paramEditMode As EditMode, paramBoolean As Boolean)
			Dim i As Integer = DX1()
			Dim j As Integer = DY1()
			Dim k As Integer = DX2()
			Dim m As Integer = DY2()
			If (i >= paramInt1) AndAlso (i <= paramInt3) AndAlso (j >= paramInt2) AndAlso (j <= paramInt4) AndAlso (k >= paramInt1) AndAlso (k <= paramInt3) AndAlso (m >= paramInt2) AndAlso (m <= paramInt4) Then
				If paramBoolean Then
					select_reverse(paramEditMode)
				Else
					[select](paramEditMode)
				End If
			End If
		End Sub

		Public Overridable Sub selectItems(paramVector As ArrayList, paramEditMode As EditMode, paramBoolean As Boolean)
			Dim i As Integer = DX1()
			Dim j As Integer = DY1()
			Dim k As Integer = DX2()
			Dim m As Integer = DY2()
			Dim arrayOfInt1 As Integer() = New Integer(paramVector.Count - 1) {}
			Dim arrayOfInt2 As Integer() = New Integer(paramVector.Count - 1) {}
			For n As Integer = 0 To paramVector.Count - 1
				Dim localDimension As DblRect = DirectCast(paramVector(n), DblRect)
				arrayOfInt1(n) = localDimension.width
				arrayOfInt2(n) = localDimension.height
			Next
			Dim localPolygon As New java.awt.Polygon(arrayOfInt1, arrayOfInt2, paramVector.Count)
			If (localPolygon.inside(i, j)) AndAlso (localPolygon.inside(k, m)) Then
				If paramBoolean Then
					select_reverse(paramEditMode)
				Else
					[select](paramEditMode)
				End If
			End If
		End Sub

		Public Overridable Function nearReactionArrow(paramInt1 As Integer, paramInt2 As Integer, paramInt3 As Integer) As ReactionArrow
			If VecMath2D.isNear(DX1(), DY1(), DX2(), DY2(), paramInt1, paramInt2, _
				paramInt3) Then
				Return Me
			End If
			Return Nothing
		End Function
	End Class


	' Location:              C:\Users\xieguigang\Downloads\KegDraw-0_1_14Beta\lib\KegDraw.jar!\keg\compound\ReactionArrow.class
'	 * Java compiler version: 6 (50.0)
'	 * JD-Core Version:       0.7.1
'	 

End Namespace
