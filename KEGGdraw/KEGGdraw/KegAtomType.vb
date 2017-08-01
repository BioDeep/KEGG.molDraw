Imports Microsoft.VisualBasic.Text

Public Structure KegAtomType

    Dim code$
    Dim formula$
    Dim name$
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

    Public Shared ReadOnly Property KEGGAtomTypes As New Dictionary(Of String, KegAtomType)

    Shared Sub New()
        With KEGGAtomTypes

        End With

    End Sub
End Structure
