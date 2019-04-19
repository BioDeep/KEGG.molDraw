#Region "Microsoft.VisualBasic::a6d3330d5985e6e9d495c4c64ee4b858, KCF\KEGGdraw.vb"

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

    ' Class KEGGdraw
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: FromEnvironment
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.InteropService
Imports Microsoft.VisualBasic.ApplicationServices

' Microsoft VisualBasic CommandLine Code AutoGenerator
' assembly: ..\app\KEGGdraw.exe

' 
'  // 
'  // 
'  // 
'  // VERSION:   1.0.0.*
'  // COPYRIGHT: Copyright © gg.xie@bionovogene.com 2019
'  // GUID:      7363f587-3c76-4d5e-b95c-36be7d823064
'  // 
' 
' 
'  < KEGGdraw.CLI >
' 
' 
' SYNOPSIS
' KEGGdraw command [/argument argument-value...] [/@set environment-variable=value...]
' 
' All of the command that available in this program has been list below:
' 
' 1. KCF chemical structure tool
' 
' 
'    /draw.kcf:     Draw image from KCF model data file.
' 
' 
' ----------------------------------------------------------------------------------------------------
' 
'    1. You can using "KEGGdraw ??<commandName>" for getting more details command help.
'    2. Using command "KEGGdraw /CLI.dev [---echo]" for CLI pipeline development.

Namespace External


''' <summary>
''' KEGGdraw.CLI
''' </summary>
'''
Public Class KEGGdraw : Inherits InteropService

    Public Const App$ = "KEGGdraw.exe"

    Sub New(App$)
        MyBase._executableAssembly = App$
    End Sub

     <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function FromEnvironment(directory As String) As KEGGdraw
          Return New KEGGdraw(App:=directory & "/" & KEGGdraw.App)
     End Function

''' <summary>
''' ```
''' /draw.kcf /in &lt;kcf.txt/directory> [/transparent /corp /out &lt;out.png/directory>]
''' ```
''' Draw image from KCF model data file.
''' </summary>
'''
Public Function DrawKCF([in] As String, Optional out As String = "", Optional transparent As Boolean = False, Optional corp As Boolean = False) As Integer
    Dim CLI As New StringBuilder("/draw.kcf")
    Call CLI.Append(" ")
    Call CLI.Append("/in " & """" & [in] & """ ")
    If Not out.StringEmpty Then
            Call CLI.Append("/out " & """" & out & """ ")
    End If
    If transparent Then
        Call CLI.Append("/transparent ")
    End If
    If corp Then
        Call CLI.Append("/corp ")
    End If


    Dim proc As IIORedirectAbstract = RunDotNetApp(CLI.ToString())
    Return proc.Run()
End Function
End Class
End Namespace




