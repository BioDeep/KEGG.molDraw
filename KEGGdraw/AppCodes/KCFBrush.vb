#Region "Microsoft.VisualBasic::a26ac443733ed2bff7b077033b4cda93, KCF\KEGGdraw\AppCodes\KCFBrush.vb"

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

    ' Class KCFBrush
    ' 
    '     Properties: N, O, P, S
    ' 
    '     Function: ChEBITheme, GetBrush, MonoColour
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports Microsoft.VisualBasic.Language.Default

''' <summary>
''' 原子和化学键的颜色画刷
''' </summary>
Public Class KCFBrush

    Public Property O As Brush
    Public Property N As Brush
    Public Property P As Brush
    Public Property S As Brush

    Public Function GetBrush(atom As String) As Brush
        Select Case atom
            Case "O"
                Return O
            Case "N"
                Return N
            Case "P"
                Return P
            Case "S"
                Return S
            Case Else
                Return Brushes.Black
        End Select
    End Function

    Public Shared Function ChEBITheme() As DefaultValue(Of KCFBrush)
        Return New KCFBrush With {
            .N = Brushes.Blue,
            .O = Brushes.Red,
            .P = Brushes.Orange,
            .S = Brushes.DarkOliveGreen
        }
    End Function

    ''' <summary>
    ''' 所有的原子和化学键都是黑色画刷
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function MonoColour() As DefaultValue(Of KCFBrush)
        Return New KCFBrush With {
            .N = Brushes.Black,
            .O = Brushes.Black,
            .P = Brushes.Black,
            .S = Brushes.Black
        }
    End Function
End Class

