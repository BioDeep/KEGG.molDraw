#Region "Microsoft.VisualBasic::fe1274226c4f4295e0ce6db1ba61ba21, KCF\KCF.IO\KCF\Models.vb"

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

    ' Structure Atom
    ' 
    '     Properties: Atom, Atom2D_coordinates, Index, KEGGAtom
    ' 
    '     Function: ToString
    ' 
    ' Structure Bound
    ' 
    '     Properties: [to], bounds, dimentional_levels, from
    ' 
    '     Function: ToString
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Text.Xml.Models

Public Structure Atom

    Public Property Index As Integer
    Public Property KEGGAtom As KegAtomType
    Public Property Atom As String
    ''' <summary>
    ''' 由于这个坐标对象只能存储<see cref="Integer"/>类型，
    ''' 所以KCF之中的坐标值需要乘以100000之后再保存在这个属性值中
    ''' </summary>
    ''' <returns></returns>
    Public Property Atom2D_coordinates As Coordinate

    Public Overrides Function ToString() As String
        Return $"[{Index}] {KEGGAtom} --> {Atom2D_coordinates.ToString}"
    End Function

End Structure

Public Structure Bound

    Public Property from As Integer
    Public Property [to] As Integer
    Public Property bounds As Integer
    ''' <summary>
    ''' 化学键两边的官能团的空间层次
    ''' </summary>
    ''' <returns></returns>
    Public Property dimentional_levels As String

    Public Overrides Function ToString() As String
        Return $"{from} => {[to]}, {bounds} bounds; {dimentional_levels}"
    End Function

End Structure
