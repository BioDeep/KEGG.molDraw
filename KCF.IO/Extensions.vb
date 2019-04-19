#Region "Microsoft.VisualBasic::1f57e5ba281e45e5a59e7a4e401125f1, KCF\KCF.IO\Extensions.vb"

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

    ' Module Extensions
    ' 
    '     Function: KCFComposition, ScanDirectory
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Language.UnixBash

Public Module Extensions

    ReadOnly atoms$() = KegAtomType.KEGGAtomTypes.Keys.ToArray

    ''' <summary>
    ''' KEGG KCF molecular strucutre model to regression model factors.
    ''' </summary>
    ''' <param name="KCF"></param>
    ''' <returns>Regression model factors</returns>
    <Extension>
    Public Function KCFComposition(KCF As KCF) As Dictionary(Of String, Double)
        Dim table = KCF.Atoms _
            .GroupBy(Function(a)
                         Return a.KEGGAtom.code
                     End Function) _
            .ToDictionary(Function(a) a.Key,
                          Function(a) CDbl(a.Count))

        For Each code As String In atoms
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
    ''' <param name="skipEmpty">
    ''' 可能会出现一类抽象代谢物，诸如：``Generic compound in reaction hierarchy``
    ''' 则这个时候KCF数据是空的
    ''' </param>
    ''' <returns></returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function ScanDirectory(directory As String, Optional skipEmpty As Boolean = True) As IEnumerable(Of KCF)
        Return (ls - l - r - "*.KCF" <= directory) _
            .Where(Function(file) file.FileLength > 0) _
            .Select(AddressOf LoadKCF)
    End Function


End Module
