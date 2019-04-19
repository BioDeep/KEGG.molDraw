#Region "Microsoft.VisualBasic::122473cef7fc5a2d89b0e7847454547b, KCF\KEGGdraw\kegDraw\DEBT.vb"

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

    ' 	Class DEBT
    ' 
    ' 	    Sub: (+2 Overloads) init
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace keg.compound

	Public Class DEBT
		Public Const PRODUCT_NAME As String = "KEGDRAW/COMPOUND"
		Public Const PRODUCT_VERSION As String = "1.4.9"
		Public Const PRODUCT_EXTENSION As String = ""
		Public Const [ON] As String = "1"
		Public Const OFF As String = "0"
		Public Shared ReadOnly FONT_NAMES As String() = {"Courier", "Helvetica", "TimesRoman"}
		Public Const VERTICAL As String = "1"
		Public Const HORIZONTAL As String = "2"
		Public Const FIXED_LENGTH As String = "FIXED_LENGTH"
		Public Const BOLD_WIDTH As String = "BOLD_WIDTH"
		Public Const MARGIN_WIDTH As String = "MARGIN_WIDTH"
		Public Const TOLERANCE As String = "TOLERANCE"
		Public Const HASH_SPACING As String = "HASH_SPACING"
		Public Const BOND_SPACING As String = "BOND_SPACING"
		Public Const LABEL_FONT As String = "LABEL_FONT"
		Public Const LABEL_SIZE As String = "LABEL_SIZE"
		Public Const LABEL_STYLE As String = "LABEL_STYLE"
		Public Const LABEL1_FONT As String = "LABEL1_FONT"
		Public Const LABEL1_SIZE As String = "LABEL1_SIZE"
		Public Const LABEL1_STYLE As String = "LABEL1_STYLE"
		Public Const LABEL2_FONT As String = "LABEL2_FONT"
		Public Const LABEL2_SIZE As String = "LABEL2_SIZE"
		Public Const LABEL2_STYLE As String = "LABEL2_STYLE"
		Public Const LABEL_COLOR_R As String = "LABEL_COLOR_R"
		Public Const LABEL_COLOR_G As String = "LABEL_COLOR_G"
		Public Const LABEL_COLOR_B As String = "LABEL_COLOR_B"
		Public Const LABEL1_COLOR_R As String = "LABEL1_COLOR_R"
		Public Const LABEL1_COLOR_G As String = "LABEL1_COLOR_G"
		Public Const LABEL1_COLOR_B As String = "LABEL1_COLOR_B"
		Public Const LABEL2_COLOR_R As String = "LABEL2_COLOR_R"
		Public Const LABEL2_COLOR_G As String = "LABEL2_COLOR_G"
		Public Const LABEL2_COLOR_B As String = "LABEL2_COLOR_B"
		Public Const FORE_COLOR_R As String = "FORE_COLOR_R"
		Public Const FORE_COLOR_G As String = "FORE_COLOR_G"
		Public Const FORE_COLOR_B As String = "FORE_COLOR_B"
		Public Const BACK_COLOR_R As String = "BACK_COLOR_R"
		Public Const BACK_COLOR_G As String = "BACK_COLOR_G"
		Public Const BACK_COLOR_B As String = "BACK_COLOR_B"
		Public Const REACTANT_COLOR_R As String = "REACTANT_COLOR_R"
		Public Const REACTANT_COLOR_G As String = "REACTANT_COLOR_G"
		Public Const REACTANT_COLOR_B As String = "REACTANT_COLOR_B"
		Public Const PRODUCT_COLOR_R As String = "PRODUCT_COLOR_R"
		Public Const PRODUCT_COLOR_G As String = "PRODUCT_COLOR_G"
		Public Const PRODUCT_COLOR_B As String = "PRODUCT_COLOR_B"
		Public Const HIGHLIGHT_R As String = "HIGHLIGHT_R"
		Public Const HIGHLIGHT_G As String = "HIGHLIGHT_G"
		Public Const HIGHLIGHT_B As String = "HIGHLIGHT_B"
		Public Const SELECT_FRAME_R As String = "SELECT_FRAME_R"
		Public Const SELECT_FRAME_G As String = "SELECT_FRAME_G"
		Public Const SELECT_FRAME_B As String = "SELECT_FRAME_B"
		Public Const COORDINATE_LENGTH As String = "COORDINATE_LENGTH"
		Public Const BRACKET_FONT As String = "BRACKET_FONT"
		Public Const BRACKET_SIZE As String = "BRACKET_SIZE"
		Public Const BRACKET_STYLE As String = "BRACKET_STYLE"
		Public Const BRACKET_COLOR_R As String = "BRACKET_COLOR_R"
		Public Const BRACKET_COLOR_G As String = "BRACKET_COLOR_G"
		Public Const BRACKET_COLOR_B As String = "BRACKET_COLOR_B"
		Public Const LAYOUT_REACTANT As String = "LAYOUT_REACTANT"
		Public Const LAYOUT_PRODUCT As String = "LAYOUT_PRODUCT"
		Public Const LAYOUT_NO_CATEGORY As String = "LAYOUT_NO_CATEGORY"
		Public Const MARGIN_FRAME As String = "MARGIN_FRAME"
		Public Const MARGIN_IN_REACTANT As String = "MARGIN_IN_REACTANT"
		Public Const MARGIN_IN_PRODUCT As String = "MARGIN_IN_PRODUCT"
		Public Const MARGIN_IN_NO_CATEGORY As String = "MARGIN_IN_NO_CATEGORY"
		Public Const MARGIN_R_TO_P As String = "MARGIN_R_TO_P"
		Public Const MARGIN_RP_TO_N As String = "MARGIN_RP_TO_N"
		Public Const ZOOM_LIMIT As String = "ZOOM_LIMIT"
		Public Const MENU_NOCHANGE As String = " "
		Public Const POPUP_LIMIT As Integer = 20
		Public Const projectName As String = "KEG"
		Public Shared pref As DEBTPreference
		Public Shared ReadOnly periodic_label As String() = {"", "H", "He", "Li", "Be", "B", _
			"C", "N", "O", "F", "Ne", "Na", _
			"Mg", "Al", "Si", "P", "S", "Cl", _
			"Ar", "K", "Ca", "Sc", "Ti", "V", _
			"Cr", "Mn", "Fe", "Co", "Ni", "Cu", _
			"Zn", "Ga", "Ge", "As", "Se", "Br", _
			"Kr", "Rb", "Sr", "Y", "Zr", "Nb", _
			"Mo", "Tc", "Ru", "Rh", "Pd", "Ag", _
			"Cd", "In", "Sn", "Sb", "Te", "I", _
			"Xe", "Cs", "Ba", "La", "Ce", "Pr", _
			"Nd", "Pm", "Sm", "Eu", "Gd", "Tb", _
			"Dy", "Ho", "Er", "Tm", "Yb", "Lu", _
			"Hf", "Ta", "W", "Re", "Os", "Ir", _
			"Pt", "Au", "Hg", "Tl", "Pb", "Bi", _
			"Po", "At", "Rn", "Fr", "Ra", "Ac", _
			"Th", "Pa", "U", "Np", "Pu", "Am", _
			"Cm", "Bk", "Cf", "Es", "Fm", "Md", _
			"Mo", "Lr", "", "", "D", "Du", _
			"Lp"}
		Public Shared ReadOnly periodic_mass As Integer() = {-1, 1, 4, 7, 9, 11, _
			12, 14, 16, 19, 20, 23, _
			24, 27, 28, 31, 32, 35, _
			40, 39, 40, 45, 48, 51, _
			52, 55, 56, 59, 58, 63, _
			64, 69, 74, 75, 80, 79, _
			84, 85, 88, 89, 90, 93, _
			98, 99, 102, 103, 106, 107, _
			114, 115, 120, 121, 130, 127, _
			132, 133, 138, 139, 133, 141, _
			142, 145, 152, 153, 158, 159, _
			164, 165, 166, 169, 174, 175, _
			180, 181, 184, 187, 192, 193, _
			195, 197, 202, 205, 208, 209, _
			209, 210, 222, 223, 226, 227, _
			232, 231, 238, 237, 244, 243, _
			247, 247, 251, 252, 257, 258, _
			259, 260, -1, -1, 2, 0, _
			0}
		Public Shared ReadOnly atomic_wight As Double() = {-1.0, 1.0079, 4.0026, 6.941, 9.01218, 10.81, _
			12.011, 14.0067, 15.9994, 18.998403, 20.179, 22.98977, _
			24.305, 26.98154, 28.0855, 30.97376, 32.06, 35.456, _
			39.948, 39.0983, 40.08, 44.9559, 47.88, 50.9415, _
			51.996, 54.938, 55.847, 58.9332, 58.69, 63.546, _
			65.38, 69.72, 72.59, 74.9216, 78.96, 79.904, _
			83.8, 85.4678, 87.62, 88.9059, 91.22, 92.9064, _
			95.94, 98.0, 101.07, 102.9055, 106.42, 107.868, _
			112.41, 114.82, 118.69, 121.75, 127.6, 126.9045, _
			131.29, 132.9054, 137.33, 138.9055, 140.12, 140.9077, _
			144.24, 145.0, 150.36, 151.96, 157.25, 158.9254, _
			162.5, 164.9304, 167.26, 168.9342, 173.04, 174.967, _
			178.49, 180.9479, 183.85, 186.207, 190.2, 192.22, _
			195.08, 196.9665, 200.59, 204.383, 207.2, 208.9804, _
			209.0, 210.0, 222.0, 223.0, 226.0254, 227.0278, _
			232.0381, 231.0359, 238.0289, 237.0482, 244.0, 243.0, _
			247.0, 247.0, 251.0, 252.0, 257.0, 258.0, _
			259.0, 260.0, -1.0, -1.0, 2.0, 0.0, _
			0.0}
		Public Shared ReadOnly symbol_order As Integer() = {2, 89, 47, 13, 95, 18, _
			33, 85, 79, 6, 56, 5, _
			83, 97, 35, 20, 48, 58, _
			98, 17, 96, 27, 24, 55, _
			29, 66, 68, 99, 63, 9, _
			26, 100, 87, 31, 64, 32, _
			1, 3, 72, 80, 67, 53, _
			49, 77, 19, 36, 57, 4, _
			103, 71, 101, 12, 25, 42, _
			102, 7, 11, 41, 60, 10, _
			28, 93, 8, 76, 15, 91, _
			82, 46, 61, 84, 59, 78, _
			94, 88, 37, 75, 45, 86, _
			44, 16, 51, 21, 34, 14, _
			62, 50, 38, 73, 65, 43, _
			52, 90, 22, 81, 69, 92, _
			23, 74, 54, 39, 70, 30, _
			40}

		Public Shared Sub init(paramString1 As String, paramString2 As String)
			pref = New DEBTPreference(paramString1, paramString2)
		End Sub

		Public Shared Sub init(paramString1 As String, paramString2 As String, paramBoolean As Boolean)
			pref = New DEBTPreference(paramString1, paramString2, paramBoolean)
		End Sub
	End Class


	' Location:              C:\Users\xieguigang\Downloads\KegDraw-0_1_14Beta\lib\KegDraw.jar!\keg\compound\DEBT.class
'	 * Java compiler version: 6 (50.0)
'	 * JD-Core Version:       0.7.1
'	 

End Namespace
