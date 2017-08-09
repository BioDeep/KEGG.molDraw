Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.Text.Xml.Models

Public Class KCF

    Public Property Entry As Entry
    Public Property Atoms As Atom()
    Public Property Bounds As Bound()

End Class

Public Structure Entry : Implements IXmlSerializable

    Public Property Id As String
    Public Property Type As String

    Public Sub ReadXml(reader As XmlReader) Implements IXmlSerializable.ReadXml
        Id = reader.GetAttribute("KEGG.id")
        Type = reader.GetAttribute("KEGG.type")
    End Sub

    Public Sub WriteXml(writer As XmlWriter) Implements IXmlSerializable.WriteXml
        Call writer.WriteAttributeString("KEGG.id", Id)
        Call writer.WriteAttributeString("KEGG.type", Type)
    End Sub

    Public Overrides Function ToString() As String
        Return $"Dim {Id} As {Type}"
    End Function

    Public Function GetSchema() As XmlSchema Implements IXmlSerializable.GetSchema
        Return Nothing
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
    ''' <summary>
    ''' 化学键两边的官能团的空间层次
    ''' </summary>
    ''' <returns></returns>
    Public Property dimentional_levels As String

    Public Overrides Function ToString() As String
        Return $"{from} => {[to]}, {bounds} bounds; {dimentional_levels}"
    End Function

End Structure