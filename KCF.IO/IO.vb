#Region "Microsoft.VisualBasic::99f30242a93c229736dc74f2ea7111bc, src\visualize\KCF\KCF.IO\IO.vb"

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

    ' Module IO
    ' 
    '     Function: LoadKCF, parseAtoms, parseBounds, parserInternal
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Text.Xml.Models

''' <summary>
''' KCF molecular structure file IO provider
''' </summary>
Public Module IO

    ''' <summary>
    ''' Load a text file and creates a KCF model from this data
    ''' </summary>
    ''' <param name="stream$">Can be text data or file path</param>
    ''' <returns></returns>
    <Extension> Public Function LoadKCF(stream$, Optional throwEmpty As Boolean = True) As KCF
        If stream.StringEmpty Then
            If throwEmpty Then
                Throw New NoNullAllowedException("Data input can not be empty!")
            Else
                Return Nothing
            End If
        End If

        If stream.FileExists Then
            stream = stream.ReadAllText Or die("No content data!")
        ElseIf Not stream.Match("http(s)?[:]//", RegexICSng).StringEmpty Then
            stream = stream.GET Or die("No content data!")
        End If

        Return parserInternal(stream)
    End Function

    <Extension> Private Function parserInternal(stream$) As KCF
        Dim sections = stream _
            .LineTokens _
            .Split(Function(s) Not InStr(s, "    ") = 1, DelimiterLocation.NextFirst) _
            .Skip(1) _
            .ToArray
        Dim t$() = sections(Scan0) _
            .First _
            .StringSplit("\s+") _
            .Where(Function(s) Not s.StringEmpty) _
            .ToArray
        Dim entry As New Entry With {
            .Id = t(1),
            .Type = t(2)
        }
        Dim atoms = sections(1) _
            .Skip(1) _
            .ToArray _
            .parseAtoms
        Dim bounds = sections(2) _
            .Skip(1) _
            .ToArray _
            .parseBounds

        Return New KCF With {
            .Entry = entry,
            .Atoms = atoms,
            .Bounds = bounds
        }
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension> Private Function parseAtoms(atoms$()) As Atom()
        Return atoms _
            .Select(AddressOf Trim) _
            .Select(Function(s) s.StringSplit("\s+")) _
            .Select(Function(t)
                        Dim type As KegAtomType = KegAtomType.GetAtom(t(1))
                        Return New Atom With {
                            .Index = Val(t(0)),
                            .KEGGAtom = type,
                            .Atom = t(2),
                            .Atom2D_coordinates = New Coordinate With {
                                .X = Val(t(3)),
                                .Y = Val(t(4))
                            }
                        }
                    End Function) _
            .ToArray
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension> Private Function parseBounds(bounds$()) As Bound()
        Return bounds _
            .Select(AddressOf Trim) _
            .Select(Function(s) s.StringSplit("\s+")) _
            .Select(Function(t)
                        Return New Bound With {
                            .from = Val(t(1)),
                            .to = Val(t(2)),
                            .bounds = Val(t(3)),
                            .dimentional_levels = t.ElementAtOrDefault(4)
                        }
                    End Function) _
            .ToArray
    End Function
End Module
