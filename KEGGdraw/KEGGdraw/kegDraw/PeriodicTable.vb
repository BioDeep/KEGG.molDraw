
Namespace keg.compound


	Public NotInheritable Class PeriodicTable
		Public Shared ReadOnly elements As String() = {"H", "He", "Li", "Be", "B", "C", _
			"N", "O", "F", "Ne", "Na", "Mg", _
			"Al", "Si", "P", "S", "Cl", "Ar", _
			"K", "Ca", "Sc", "Ti", "V", "Cr", _
			"Mn", "Fe", "Co", "Ni", "Cu", "Zn", _
			"Ga", "Ge", "As", "Se", "Br", "Kr", _
			"Rb", "Sr", "Y", "Zr", "Nb", "Mo", _
			"Tc", "Ru", "Rh", "Pd", "Ag", "Cd", _
			"In", "Sn", "Sb", "Te", "I", "Xe", _
			"Cs", "Ba", "La", "Hf", "Ta", "W", _
			"Re", "Os", "Ir", "Pt", "Au", "Hg", _
			"Tl", "Pb", "Bi", "Po", "At", "Rn", _
			"Fr", "Ra", "Ac", "Unq", "Unp", "Unh", _
			"Uns", "Uno", "Une", "Ce", "Pr", "Nd", _
			"Pm", "Sm", "Eu", "Gd", "Tb", "Dy", _
			"Ho", "Er", "Tm", "Yb", "Lu", "Th", _
			"Pa", "U", "Np", "Pu", "Am", "Cm", _
			"Bk", "Cf", "Es", "Fm", "Md", "No", _
			"Lr"}

		Public Shared Function getName(paramString As String) As String
			Dim str As String = paramString.ToLower()
			For i As Integer = 0 To elements.Length - 1
				If elements(i).ToLower().Equals(str) Then
					paramString = New String(elements(i))
					Exit For
				End If
			Next
			Return paramString
		End Function

		Public Shared Sub Main(paramArrayOfString As String())
			Console.[Error].WriteLine(getName("h"))
			Console.[Error].WriteLine(getName("he"))
			Console.[Error].WriteLine(getName("bk"))
			Console.[Error].WriteLine(getName("hoge"))
		End Sub
	End Class


	' Location:              C:\Users\xieguigang\Downloads\KegDraw-0_1_14Beta\lib\KegDraw.jar!\keg\compound\PeriodicTable.class
'	 * Java compiler version: 6 (50.0)
'	 * JD-Core Version:       0.7.1
'	 

End Namespace
