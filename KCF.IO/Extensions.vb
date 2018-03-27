Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Language.UnixBash

Public Module Extensions

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
