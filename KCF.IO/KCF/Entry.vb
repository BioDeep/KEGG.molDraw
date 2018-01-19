Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.Serialization

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

    Public Function GetSchema() As XMLSchema Implements IXmlSerializable.GetSchema
        Return Nothing
    End Function
End Structure