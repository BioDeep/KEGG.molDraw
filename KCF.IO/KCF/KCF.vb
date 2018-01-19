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