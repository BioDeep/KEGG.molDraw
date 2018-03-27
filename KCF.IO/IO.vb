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
    <Extension> Public Function LoadKCF(stream$) As KCF
        If stream.FileExists Then
            stream = stream.ReadAllText Or die("No content data!")
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
                        Return New Atom With {
                            .Index = Val(t(0)),
                            .KEGGAtom = t(1),
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
