
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph

<HideModuleName>
Public Module Extensions

    ' 可以通过KEGG的原子基团解析出有多少个化学键
    ' 然后通过分析与当前的这个节点相连接的边的化学键的数量
    ' 二者的差值即为当前的这个原子基团可能的电荷值

    ''' <summary>
    ''' Calculate the charge value of ion graph model
    ''' </summary>
    ''' <param name="group"></param>
    ''' <returns></returns>
    <Extension>
    Public Function AtomGroupCharge(group As NetworkGraph) As Double
        Throw New NotImplementedException
    End Function
End Module
