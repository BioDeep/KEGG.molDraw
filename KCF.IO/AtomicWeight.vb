Imports System.Runtime.CompilerServices
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.Data.csv
Imports Microsoft.VisualBasic.Data.csv.StorageProvider.Reflection
Imports Microsoft.VisualBasic.Language

Public Class AtomicWeight

    <XmlAttribute> Public Property Symbol As String
    <XmlAttribute> Public Property Name As String

    <Column("Atomic Weight")>
    <XmlAttribute> Public Property AtomicWeight As String

    <Collection("Notes", ", ")>
    <XmlAttribute> Public Property Notes As Integer()

    <Ignored>
    Public ReadOnly Property Mass As Double
        Get
            With AtomicWeight _
                .Replace(" ", "") _
                .GetStackValue("[", "]")

                Return Val(.ByRef)
            End With
        End Get
    End Property

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function GetTable() As Dictionary(Of String, AtomicWeight)
        Return My.Resources _
            .AtomicWeights _
            .lTokens _
            .LoadStream(Of AtomicWeight) _
            .ToDictionary(Function(a)
                              Return a.Symbol
                          End Function)
    End Function

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class
