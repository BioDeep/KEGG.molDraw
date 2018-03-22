Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Language.UnixBash

Public Module Extensions

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function ScanDirectory(directory As String) As IEnumerable(Of KCF)
        Return (ls - l - r - "*.KCF" <= directory).Select(AddressOf LoadKCF)
    End Function
End Module
