#Region "Microsoft.VisualBasic::30725c120a2365a02909ebf00684614b, visualize\KCF\KCF.IO\AtomicWeight.vb"

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

    '   Total Lines: 53
    '    Code Lines: 45
    ' Comment Lines: 0
    '   Blank Lines: 8
    '     File Size: 1.77 KB


    ' Class AtomicWeight
    ' 
    '     Properties: AtomicWeight, Mass, Name, Notes, Symbol
    ' 
    '     Function: GetTable, tableLoader, ToString
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.Language

Public Class AtomicWeight

    <XmlAttribute> Public Property Symbol As String
    <XmlAttribute> Public Property Name As String
    <XmlAttribute> Public Property AtomicWeight As String
    <XmlAttribute> Public Property Notes As Integer()

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
        Return tableLoader _
            .ToDictionary(Function(a)
                              Return a.Symbol
                          End Function)
    End Function

    Private Shared Iterator Function tableLoader() As IEnumerable(Of AtomicWeight)
        For Each line As String In My.Resources.AtomicWeights.LineTokens.Skip(1)
            Dim tokens As String() = line.Split(","c)
            Dim weight As New AtomicWeight With {
                .Symbol = tokens(1),
                .Name = tokens(2),
                .AtomicWeight = tokens(3),
                .Notes = tokens _
                    .Skip(4) _
                    .Select(Function(t)
                                Return t.Trim(""""c, " "c).ParseInteger
                            End Function) _
                    .ToArray
            }

            Yield weight
        Next
    End Function

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class
