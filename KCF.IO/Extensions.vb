#Region "Microsoft.VisualBasic::25657af6ba802a1df799299a9d97a003, mzkit\src\visualize\KCF\KCF.IO\Extensions.vb"

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

    '   Total Lines: 58
    '    Code Lines: 35
    ' Comment Lines: 15
    '   Blank Lines: 8
    '     File Size: 1.84 KB


    ' Module Extensions
    ' 
    '     Properties: atoms
    ' 
    '     Function: KCFComposition, ScanDirectory
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Language.UnixBash

<HideModuleName>
Public Module Extensions

    Public ReadOnly Property atoms As String()
        Get
            Return KegAtomType.KEGGAtomTypes.Keys.ToArray
        End Get
    End Property

    ''' <summary>
    ''' KEGG KCF molecular strucutre model to regression model factors.
    ''' </summary>
    ''' <param name="KCF"></param>
    ''' <returns>Regression model factors</returns>
    <Extension>
    Public Function KCFComposition(KCF As KCF) As Dictionary(Of String, Double)
        Dim table As Dictionary(Of String, Double) = KCF.Atoms _
            .GroupBy(Function(a)
                         Return a.KEGGAtom.code
                     End Function) _
            .ToDictionary(Function(a) a.Key,
                          Function(a) CDbl(a.Count))

        Static allAtoms$() = atoms

        For Each code As String In allAtoms
            If Not table.ContainsKey(code) Then
                table(code) = 0
            End If
        Next

        Return table
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="directory"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 可能会出现一类抽象代谢物，诸如：``Generic compound in reaction hierarchy``
    ''' 则这个时候KCF数据是空的
    ''' 函数会自动跳过这些空的模型数据文件的加载
    ''' </remarks>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function ScanDirectory(directory As String) As IEnumerable(Of KCF)
        Return (ls - l - r - "*.KCF" <= directory) _
            .Where(Function(file)
                       Return file.FileLength > 0
                   End Function) _
            .Select(AddressOf LoadKCF)
    End Function

End Module
