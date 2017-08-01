
Namespace keg.compound


	Public Class DEBTPreference
		Friend properties As java.util.Properties
		Friend loadIni As Boolean = True

		Public Sub New(paramString1 As String, paramString2 As String, paramBoolean As Boolean)
			Me.loadIni = paramBoolean
			If paramBoolean Then
				Dim localProperties As New java.util.Properties()
				Dim localInputStream As java.io.InputStream = Nothing
				Try
					localInputStream = Me.[GetType]().getResource("/etc/compound/ini/" & paramString1 & ".ini").openStream()
					localProperties.load(localInputStream)
				Catch localException1 As Exception
					Console.WriteLine("Can not read property file in Jar. : " & paramString1)
				End Try
				Me.properties = New java.util.Properties(localProperties)
				If paramString2.Length > 0 Then
					Try
						Dim localFile As New File(paramString2)
						If (localFile.exists()) AndAlso (localFile.canRead()) Then
							Me.properties.load(New java.io.FileInputStream(localFile))
						End If
					Catch localException2 As Exception
					End Try
				End If
			End If
		End Sub

		Public Sub New(paramString1 As String, paramString2 As String)
			Me.New(paramString1, paramString2, True)
		End Sub

		Public Overridable Function [get](paramString As String) As String
			Return Me.properties.getProperty(paramString)
		End Function
	End Class


	' Location:              C:\Users\xieguigang\Downloads\KegDraw-0_1_14Beta\lib\KegDraw.jar!\keg\compound\DEBTPreference.class
'	 * Java compiler version: 6 (50.0)
'	 * JD-Core Version:       0.7.1
'	 

End Namespace
