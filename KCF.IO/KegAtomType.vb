#Region "Microsoft.VisualBasic::931464de327439523a705ab5696e4f05, visualize\KCF\KCF.IO\KegAtomType.vb"

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

    '   Total Lines: 118
    '    Code Lines: 77
    ' Comment Lines: 27
    '   Blank Lines: 14
    '     File Size: 3.66 KB


    ' Structure KegAtomType
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: ToString
    '     Enum Types
    ' 
    '         Other
    ' 
    ' 
    ' 
    '  
    ' 
    '     Properties: KEGGAtomTypes
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: GetAtom, parserInternal, parseType
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Public Structure KegAtomType

    ''' <summary>
    ''' KCF文件之中的官能团编码
    ''' </summary>
    Dim code$
    ''' <summary>
    ''' 官能团的化学式
    ''' </summary>
    Dim formula$
    ''' <summary>
    ''' 官能团的名称
    ''' </summary>
    Dim name$
    ''' <summary>
    ''' 在绘图的时候所绘制的标签文本，这个字段可能会为空值
    ''' </summary>
    Dim view$
    ''' <summary>
    ''' 官能团的中心原子的类型
    ''' </summary>
    Dim type As Types

    Sub New(code$, formula$, name$, type As Types)
        Me.code = code
        Me.formula = formula
        Me.name = name
        Me.type = type
    End Sub

    Public Overrides Function ToString() As String
        Return $"{code}    {formula} / {name}"
    End Function

    Public Enum Types As Byte
        Other
        Carbon = 12
        Nitrogen = 14
        Oxygen = 16
        Sulfur = 32
        Phosphorus = 31
        ''' <summary>
        ''' 卤族元素
        ''' </summary>
        Halogen = 255
    End Enum

    ''' <summary>
    ''' <see cref="code"/>是字典的主键
    ''' </summary>
    ''' <returns>在这里值使用一个数组来表示是为了存储``X``分类的卤族元素</returns>
    Public Shared ReadOnly Property KEGGAtomTypes As New Dictionary(Of String, KegAtomType())

    ''' <summary>
    ''' Get atom information details for the atom code that defined in KCF file.
    ''' </summary>
    ''' <param name="code">KCF atom group code like ``C1a``, ``C1b``, etc...</param>
    ''' <returns></returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function GetAtom(code As String) As KegAtomType
        Return KEGGAtomTypes(code)(Scan0)
    End Function

    Shared Sub New()
        With KEGGAtomTypes
            Dim blocks = My.Resources.KEGGAtomTypes _
                .LineTokens _
                .Split(Function(s) s.StringEmpty, includes:=False) _
                .ToArray

            For Each part As String() In blocks
                Dim type As Types = parseType(part(Scan0))
                Dim atoms = part _
                    .Skip(1) _
                    .Select(Function(line)
                                Return parserInternal(line, type)
                            End Function) _
                    .GroupBy(Function(x) x.code) _
                    .ToArray

                For Each atom In atoms
                    Call .Add(atom.Key, atom.ToArray)
                Next
            Next
        End With
    End Sub

    Private Shared Function parserInternal(line$, type As Types) As KegAtomType
        Dim atom As NamedValue(Of String) = line.GetTagValue(" ")
        Dim data = Strings.Split(atom.Value, " / ")
        Dim formula$ = "", name$
        Dim displayLabel$ = Nothing

        If data.Length = 1 Then
            name = data.First
        Else
            formula = data.ElementAtOrDefault(0)
            name = data.ElementAtOrDefault(1)
            displayLabel = data.ElementAtOrDefault(2)
        End If

        Return New KegAtomType With {
            .code = atom.Name,
            .formula = formula,
            .name = name,
            .type = type,
            .view = displayLabel
        }
    End Function

    Private Shared Function parseType(s$) As Types
        s = s.Trim("+"c).Trim.Split.First
        Return [Enum].Parse(GetType(Types), s)
    End Function
End Structure
