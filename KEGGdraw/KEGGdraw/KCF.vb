Imports Microsoft.VisualBasic.Text.Xml.Models

Public Class KCF

    Public Property Entry As Entry
    Public Property Atoms As Atom()
    Public Property Bounds As Bound()

End Class

Public Structure Entry

    Public Property Id As String
    Public Property Type As String

    Public Overrides Function ToString() As String
        Return $"Dim {Id} As {Type}"
    End Function
End Structure

Public Structure Atom

    Public Property Index As Integer
    Public Property KEGGAtom As String
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
    Public Property direction As String

    Public Overrides Function ToString() As String
        Return $"{from} => {[to]}, {bounds} bounds; {direction}"
    End Function

End Structure