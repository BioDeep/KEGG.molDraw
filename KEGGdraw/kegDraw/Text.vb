#Region "Microsoft.VisualBasic::78d873fea33955f7fac2438738272f13, visualize\KCF\KEGGdraw\kegDraw\Text.vb"

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

    '   Total Lines: 143
    '    Code Lines: 112
    ' Comment Lines: 4
    '   Blank Lines: 27
    '     File Size: 2.96 KB


    ' 	Class Text
    ' 
    ' 	    Properties: Bounds, Color, Font, FontFamily, FontSize
    '                  FontStyle, Text, X, Y
    ' 
    ' 	    Constructor: (+2 Overloads) Sub New
    ' 
    ' 	    Function: contains
    ' 
    ' 	    Sub: moveBy, moveTo, refrectParamFromFont
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace keg.compound


	Public Class Text
		Private ReadOnly DEFAULT_FONT As New java.awt.Font("Courier", 0, 14)
		Private ReadOnly DEFAULT_COLOR As Color = Color.black
		Private _font As java.awt.Font = Me.DEFAULT_FONT
		Private _text As String = ""
		Private _x As Integer
		Private _y As Integer
		Private _fontFamily As String
		Private _fontStyle As Integer
		Private _fontSize As Integer
		Private _color As Color = Me.DEFAULT_COLOR
		Private _bounds As Rectangle

		Public Sub New()
			refrectParamFromFont()
			Me._x = 0
			Me._y = (0 - -Me._fontSize)
		End Sub

		Public Sub New(paramString As String)
			Me.New()
			Me._text = paramString
		End Sub

		Private Sub refrectParamFromFont()
			If Me._font IsNot Nothing Then
				Me._fontFamily = Me._font.Family
				Me._fontStyle = Me._font.Style
				Me._fontSize = Me._font.Size
			End If
		End Sub

		Public Overridable Property Font() As java.awt.Font
			Get
				Return Me._font
			End Get
			Set
				Me._font = value
				refrectParamFromFont()
			End Set
		End Property


		Public Overridable ReadOnly Property X() As Integer
			Get
				Return Me._x
			End Get
		End Property

		Public Overridable ReadOnly Property Y() As Integer
			Get
				Return Me._y
			End Get
		End Property

		Public Overridable Property Text() As String
			Get
				Return Me._text
			End Get
			Set
				Me._text = value
			End Set
		End Property


		Public Overridable Property FontFamily() As String
			Get
				Return Me._fontFamily
			End Get
			Set
				Me._fontFamily = value
			End Set
		End Property


		Public Overridable Property FontStyle() As Integer
			Get
				Return Me._fontStyle
			End Get
			Set
				Me._fontStyle = value
			End Set
		End Property


		Public Overridable Property FontSize() As Integer
			Get
				Return Me._fontSize
			End Get
			Set
				Me._fontSize = value
			End Set
		End Property


		Public Overridable Property Color() As Color
			Get
				Return Me._color
			End Get
			Set
				Me._color = value
			End Set
		End Property


		Public Overridable Property Bounds() As Rectangle
			Get
				Return Me._bounds
			End Get
			Set
				Me._bounds = value
			End Set
		End Property


		Public Overridable Function contains(paramPoint As java.awt.Point) As Boolean
			If Me._bounds Is Nothing Then
				Return False
			End If
			Return Me._bounds.contains(paramPoint)
		End Function

		Public Overridable Sub moveTo(paramPoint As java.awt.Point)
			Me._x = paramPoint.x
			Me._y = paramPoint.y
		End Sub

		Public Overridable Sub moveBy(paramInt1 As Integer, paramInt2 As Integer)
			Me._x += paramInt1
			Me._y += paramInt2
		End Sub
	End Class


	' Location:              C:\Users\xieguigang\Downloads\KegDraw-0_1_14Beta\lib\KegDraw.jar!\keg\compound\Text.class
'	 * Java compiler version: 6 (50.0)
'	 * JD-Core Version:       0.7.1
'	 

End Namespace
