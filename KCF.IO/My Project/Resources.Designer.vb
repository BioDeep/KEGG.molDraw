﻿#Region "Microsoft.VisualBasic::6dcfff8f8c72bcef7976b4cf4f4ed0ca, mzkit\src\visualize\KCF\KCF.IO\My Project\Resources.Designer.vb"

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


    ' Code Statistics:

    '   Total Lines: 112
    '    Code Lines: 42
    ' Comment Lines: 60
    '   Blank Lines: 10
    '     File Size: 4.82 KB


    '     Module Resources
    ' 
    '         Properties: AtomicWeights, Culture, KEGGAtomTypes, ResourceManager
    ' 
    ' 
    ' /********************************************************************************/

#End Region

'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("BioNovoGene.BioDeep.Chemistry.Model.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to #NO,Symbol,Name,Atomic Weight,Notes
        '''1,H,Hydrogen,1.008,&quot;3, 5&quot;
        '''2,He,Helium,4.002 602(2),&quot;1, 2&quot;
        '''3,Li,Lithium,6.94,&quot;3, 5&quot;
        '''4,Be,Beryllium,9.012 1831(5),
        '''5,B,Boron,10.81,&quot;3, 5&quot;
        '''6,C,Carbon,12.011,5
        '''7,N,Nitrogen,14.007,5
        '''8,O,Oxygen,15.999,5
        '''9,F,Fluorine,18.998 403 163(6),
        '''10,Ne,Neon,20.1797(6),&quot;1, 3&quot;
        '''11,Na,Sodium,22.989 769 28(2),
        '''12,Mg,Magnesium,24.305,5
        '''13,Al,Aluminium,26.981 5385(7),
        '''14,Si,Silicon,28.085,5
        '''15,P,Phosphorus,30.973 761 998(5),
        '''16,S,Sulfur,32.06,5
        '''17,Cl,Chlorine,35.45,&quot;3, 5&quot;
        '''18,Ar [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property AtomicWeights() As String
            Get
                Return ResourceManager.GetString("AtomicWeights", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to +   Carbon atoms
        '''C1a R-CH3 / methyl / CH3
        '''C1b R-CH2-R / methylene
        '''C1c R-CH(-R)-R / tertiary carbon
        '''C1d R-C(-R)2-R / quaternary carbon
        '''C1x ring-CH2-ring / methylene in ring
        '''C1y ring-CH(-R)-ring / tertiary carbon in ring
        '''C1z ring-C(-R)2-ring / quaternary carbon in ring
        '''C2a R=CH2 / alkenyl terminus carbon
        '''C2b R=CH-R / alkenyl secondary carbon
        '''C2c R=C(-R)2 / alkenyl tertiary carbon
        '''C2x ring-CH=ring / alkenyl secondary carbon in ring
        '''C2y ring-C(-R)=ring / alkenyl tertiary carbon in ring
        '''C2y ring-C(= [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property KEGGAtomTypes() As String
            Get
                Return ResourceManager.GetString("KEGGAtomTypes", resourceCulture)
            End Get
        End Property
    End Module
End Namespace

