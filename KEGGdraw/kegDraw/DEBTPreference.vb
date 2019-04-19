#Region "Microsoft.VisualBasic::8c3947ada120eb15c0926b1797328178, KCF\KEGGdraw\kegDraw\DEBTPreference.vb"

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

    ' 	Class DEBTPreference
    ' 
    ' 	    Constructor: (+2 Overloads) Sub New
    ' 	    Function: [get]
    ' 
    ' 
    ' /********************************************************************************/

#End Region

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
