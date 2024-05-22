#Region "Microsoft.VisualBasic::b5bc9fc5df393ca8c9a0a5b9e9c6a455, visualize\KCF\KCF.IO\KCF\KCF.vb"

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

    '   Total Lines: 18
    '    Code Lines: 8 (44.44%)
    ' Comment Lines: 8 (44.44%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 2 (11.11%)
    '     File Size: 460 B


    ' Class KCF
    ' 
    '     Properties: Atoms, Bounds, Entry
    ' 
    '     Function: ToString
    ' 
    ' /********************************************************************************/

#End Region

Public Class KCF

    Public Property Entry As Entry
    ''' <summary>
    ''' 原子基团列表
    ''' </summary>
    ''' <returns></returns>
    Public Property Atoms As Atom()
    ''' <summary>
    ''' 原子基团之间相互连接的化学键
    ''' </summary>
    ''' <returns></returns>
    Public Property Bounds As Bound()

    Public Overrides Function ToString() As String
        Return Entry.ToString
    End Function
End Class
