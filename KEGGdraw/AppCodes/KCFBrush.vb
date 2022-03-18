#Region "Microsoft.VisualBasic::d1069fca79ae5b7eb0e4f844b9a46c46, mzkit\src\visualize\KCF\KEGGdraw\AppCodes\KCFBrush.vb"

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

    '   Total Lines: 55
    '    Code Lines: 41
    ' Comment Lines: 8
    '   Blank Lines: 6
    '     File Size: 1.93 KB


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
Imports Microsoft.VisualBasic.Language.Default
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.DataFramework
Imports System.Runtime.CompilerServices

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
