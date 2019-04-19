#Region "Microsoft.VisualBasic::f432cda0f29c6a5d5b1cae12fbfa117c, KCF\KEGGdraw\kegDraw\PeriodicTable.vb"

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

    ' 	Class PeriodicTable
    ' 
    ' 	    Function: getName
    ' 
    ' 	    Sub: Main
    ' 
    ' 
    ' /********************************************************************************/

#End Region

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
