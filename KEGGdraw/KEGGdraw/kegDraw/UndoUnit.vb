Imports System.Collections

Namespace keg.compound


	Public Class UndoUnit
		Public undo_operation As Integer = 0
		Public undo_origin As New DblRect(0, 0)
		Public undo_select_area As New Rectangle(0, 0, 0, 0)
		Public undo_selected As New ArrayList()
		Public undo_buffer As New ArrayList()
		Public undo_action As Integer = 0
	End Class


	' Location:              C:\Users\xieguigang\Downloads\KegDraw-0_1_14Beta\lib\KegDraw.jar!\keg\compound\UndoUnit.class
'	 * Java compiler version: 6 (50.0)
'	 * JD-Core Version:       0.7.1
'	 

End Namespace
