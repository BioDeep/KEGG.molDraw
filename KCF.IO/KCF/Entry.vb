#Region "Microsoft.VisualBasic::42580730691973be0621e72014adacb6, mzkit\src\visualize\KCF\KCF.IO\KCF\Entry.vb"

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

    '   Total Lines: 27
    '    Code Lines: 21
    ' Comment Lines: 0
    '   Blank Lines: 6
    '     File Size: 866.00 B


    ' Structure Entry
    ' 
    '     Properties: Id, Type
    ' 
    '     Function: GetSchema, ToString
    ' 
    '     Sub: ReadXml, WriteXml
    ' 
    ' /********************************************************************************/

#End Region

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
