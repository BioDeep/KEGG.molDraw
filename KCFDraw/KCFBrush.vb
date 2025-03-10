﻿#Region "Microsoft.VisualBasic::20af03cef39858b494c6df2d4cc1eabc, visualize\KCF\KCFDraw\KCFBrush.vb"

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

    '   Total Lines: 81
    '    Code Lines: 66 (81.48%)
    ' Comment Lines: 8 (9.88%)
    '    - Xml Docs: 87.50%
    ' 
    '   Blank Lines: 7 (8.64%)
    '     File Size: 3.06 KB


    ' Class KCFBrush
    ' 
    '     Properties: Br, Cl, N, O, P
    '                 S
    ' 
    '     Function: ChEBITheme, GetBrush, MonoColour
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.DataFramework
Imports Microsoft.VisualBasic.Language.Default

#If NET48 Then
Imports Pen = System.Drawing.Pen
Imports Pens = System.Drawing.Pens
Imports Brush = System.Drawing.Brush
Imports Font = System.Drawing.Font
Imports Brushes = System.Drawing.Brushes
Imports SolidBrush = System.Drawing.SolidBrush
Imports DashStyle = System.Drawing.Drawing2D.DashStyle
Imports Image = System.Drawing.Image
Imports Bitmap = System.Drawing.Bitmap
Imports GraphicsPath = System.Drawing.Drawing2D.GraphicsPath
Imports FontStyle = System.Drawing.FontStyle
#Else
Imports Pen = Microsoft.VisualBasic.Imaging.Pen
Imports Pens = Microsoft.VisualBasic.Imaging.Pens
Imports Brush = Microsoft.VisualBasic.Imaging.Brush
Imports Font = Microsoft.VisualBasic.Imaging.Font
Imports Brushes = Microsoft.VisualBasic.Imaging.Brushes
Imports SolidBrush = Microsoft.VisualBasic.Imaging.SolidBrush
Imports DashStyle = Microsoft.VisualBasic.Imaging.DashStyle
Imports Image = Microsoft.VisualBasic.Imaging.Image
Imports Bitmap = Microsoft.VisualBasic.Imaging.Bitmap
Imports GraphicsPath = Microsoft.VisualBasic.Imaging.GraphicsPath
Imports FontStyle = Microsoft.VisualBasic.Imaging.FontStyle
#End If

''' <summary>
''' 原子和化学键的颜色画刷
''' </summary>
Public Class KCFBrush

    Public Property O As SolidBrush
    Public Property N As SolidBrush
    Public Property P As SolidBrush
    Public Property S As SolidBrush
    Public Property Cl As SolidBrush
    Public Property Br As SolidBrush

    Shared ReadOnly getAtomBrush As Dictionary(Of String, Func(Of KCFBrush, SolidBrush)) =
        GetType(KCFBrush) _
        .GetProperties(PublicProperty) _
        .ToDictionary(Function(atom) atom.Name,
                      Function(read) As Func(Of KCFBrush, SolidBrush)
                          Return Function(colors) DirectCast(read.GetValue(colors), SolidBrush)
                      End Function)
    Shared ReadOnly Black As [Default](Of SolidBrush) = Brushes.Black

    Public Function GetBrush(atom As String) As SolidBrush
        If getAtomBrush.ContainsKey(atom) Then
            Return getAtomBrush(atom)(Me) Or Black
        Else
            Return Brushes.Black
        End If
    End Function

    Public Shared Function ChEBITheme() As [Default](Of KCFBrush)
        Return New KCFBrush With {
            .N = New SolidBrush(Color.FromArgb(51, 51, 153)),
            .O = Brushes.Red,
            .P = Brushes.Orange,
            .S = Brushes.DarkOliveGreen,
            .Cl = Brushes.Green,
            .Br = Brushes.Orange
        }
    End Function

    ''' <summary>
    ''' 所有的原子和化学键都是黑色画刷
    ''' </summary>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function MonoColour() As [Default](Of KCFBrush)
        Return New KCFBrush
    End Function
End Class
