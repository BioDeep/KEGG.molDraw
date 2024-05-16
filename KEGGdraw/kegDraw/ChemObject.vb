#Region "Microsoft.VisualBasic::759d177301af0e9749a11219c1aa05b8, visualize\KCF\KEGGdraw\kegDraw\ChemObject.vb"

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

    '   Total Lines: 137
    '    Code Lines: 111
    ' Comment Lines: 0
    '   Blank Lines: 26
    '     File Size: 4.36 KB


    '     Class ChemObject
    ' 
    '         Properties: [Select], Color, Coordinate, HighLight, ID
    ' 
    '         Function: ICloneable_Clone, inside
    ' 
    '         Sub: (+4 Overloads) [select], flipHorizontal, flipVertical, move, rotate
    '              (+2 Overloads) select_reverse, (+2 Overloads) unselect
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging.LayoutModel
Imports System.Drawing.Drawing2D

Namespace keg.compound

    Public Class ChemObject : Implements ICloneable

        Public m_id As String
        Friend displayX As Integer
        Friend displayY As Integer
        Private select_flag As Boolean
        Private m_color As Color
        Private m_highLight As Integer = 0

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
                Return Me.m_color
            End Get
            Set
                Me.m_color = Value
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


        Public Overridable Property Coordinate() As DblRect
            Get
                Return New DblRect(Me.displayX, Me.displayY)
            End Get
            Set
                Me.displayX = Value.Width
                Me.displayY = Value.Height
            End Set
        End Property


        Public Overridable Sub move(paramInt1 As Integer, paramInt2 As Integer)
            Me.displayX += paramInt1
            Me.displayY += paramInt2
        End Sub

        Public Overridable Sub rotate(paramInt1 As Integer, paramInt2 As Integer, paramDouble As Double)
        End Sub

        Public Overridable Sub flipHorizontal(paramDouble1 As Double, paramDouble2 As Double)
        End Sub

        Public Overridable Sub flipVertical(paramDouble1 As Double, paramDouble2 As Double)
        End Sub

        Public Overridable Function inside(paramPolygon As GraphicsPath) As ChemObject
            If paramPolygon.IsVisible(Me.displayX, Me.displayY) Then
                Return Me
            End If
            Return DirectCast(Nothing, ChemObject)
        End Function

        Public Overridable Sub [select](paramBoolean As Boolean)
            Me.select_flag = paramBoolean
        End Sub

        Public Overridable Sub [select](paramBoolean As Boolean, paramEditMode As EditMode)
            If paramBoolean Then
                If Not paramEditMode.selected.Contains(Me) Then
                    paramEditMode.selected.Add(Me)
                End If
            ElseIf paramEditMode.selected.Contains(Me) Then
                paramEditMode.selected.Remove(Me)
            End If
            Me.select_flag = paramBoolean
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
            Me.select_flag = (Not Me.select_flag)
        End Sub

        Public Overridable Sub select_reverse(paramEditMode As EditMode)
            Me.select_flag = (Not Me.select_flag)
            If Me.select_flag Then
                If Not paramEditMode.selected.Contains(Me) Then
                    paramEditMode.selected.Add(Me)
                End If
            ElseIf paramEditMode.selected.Contains(Me) Then
                paramEditMode.selected.Remove(Me)
            End If
        End Sub

        Public Overridable ReadOnly Property [Select]() As Boolean
            Get
                Return Me.select_flag
            End Get
        End Property

        <MethodImpl(MethodImplOptions.Synchronized)>
        Private Function ICloneable_Clone() As Object Implements ICloneable.Clone
            Dim localChemObject As ChemObject = DirectCast(MyBase.MemberwiseClone, ChemObject)
            Return localChemObject
        End Function
    End Class
End Namespace
