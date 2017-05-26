Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Text.Xml.Models

Public Module IO

    <Extension> Public Function LoadKCF(stream$) As KCF
        If stream.FileExists Then
            stream = stream.ReadAllText Or die("No content data!", Function(s) DirectCast(s, String).StringEmpty)
        End If

        Dim lines$() = stream.lTokens
        Dim sections = lines _
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

    <Extension> Private Function parseAtoms(atoms$()) As Atom()
        Return atoms _
            .Select(AddressOf Trim) _
            .Select(Function(s) s.StringSplit("\s+")) _
            .Select(Function(t)
                        Return New Atom With {
                            .Index = Val(t(0)),
                            .KEGGAtom = t(1),
                            .Atom = t(2),
                            .Atom2D_coordinates = Coordinate.FromPointF(
                                Val(t(3)), Val(t(4)), 100000
                            )
                        }
                    End Function) _
            .ToArray
    End Function

    <Extension> Private Function parseBounds(bounds$()) As Bound()
        Return bounds _
            .Select(AddressOf Trim) _
            .Select(Function(s) s.StringSplit("\s+")) _
            .Select(Function(t)
                        Return New Bound With {
                            .from = Val(t(1)),
                            .to = Val(t(2)),
                            .bounds = Val(t(3)),
                            .direction = t.ElementAtOrDefault(4)
                        }
                    End Function) _
            .ToArray
    End Function
End Module
