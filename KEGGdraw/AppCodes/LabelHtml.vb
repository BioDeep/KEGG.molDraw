Module LabelHtml

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="label"></param>
    ''' <returns>``&lt;= =>``</returns>
    Public Function GetHtmlTuple(label As String) As (left$, right$)
        Select Case label
            Case "NH2"
                Return ("H<sub>2</sub>N", "NH<sub>2</sub>")
            Case "OH"
                Return ("HO", "OH")
            Case Else
                Throw New NotImplementedException(label)
        End Select
    End Function
End Module
