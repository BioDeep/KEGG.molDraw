Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices
#If netcore5 = 0 Then
' 組件的一般資訊是由下列的屬性集控制。
' 變更這些屬性的值即可修改組件的相關
' 資訊。

' 檢閱組件屬性的值

<Assembly: AssemblyTitle("KCF File IO")>
<Assembly: AssemblyDescription("")>
<Assembly: AssemblyCompany("BioNovoGene")>
<Assembly: AssemblyProduct("KCF.IO")>
<Assembly: AssemblyCopyright("Copyright © MIT Licensed. 2019")>
<Assembly: AssemblyTrademark("BioDeep")>

<Assembly: ComVisible(False)>

'下列 GUID 為專案公開 (Expose) 至 COM 時所要使用的 typelib ID
<Assembly: Guid("2dd771e9-7713-4894-a314-1755774d44f9")>

' 組件的版本資訊由下列四個值所組成: 
'
'      主要版本
'      次要版本
'      組建編號
'      修訂編號
'
' 您可以指定所有的值，或將組建編號或修訂編號設為預設值
' 指定為預設值: 
' <Assembly: AssemblyVersion("1.0.*")>

<Assembly: AssemblyVersion("1.0.0.0")>
<Assembly: AssemblyFileVersion("1.0.0.0")>
#end if