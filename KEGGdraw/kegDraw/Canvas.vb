#Region "Microsoft.VisualBasic::a2459c3068d02a4775d5b4ee3beab3cd, visual\KCF\KEGGdraw\kegDraw\Canvas.vb"

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

    '     Class CompoundCanvas
    ' 
    '         Properties: AllAtoms, Conteiner, Dispscale, DispScale, EnabledShrikPartMarker
    '                     Hoffset, OnlyOneBracketSelected, PointOfCanvasCenter, ShrinkMode, VirtualScreen
    '                     Voffset
    ' 	  Class RFR
    ' 
    ' 	      Properties: ConnectionPoint, Selected
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

'Error: Converting Casting from C# to VB.Net 
'Error: Converting Case Blocks 
'Error: Converting Methods, Functions and Constructors 
'Error: Converting If-Else-End If Blocks 

Imports System
Imports System.Runtime.CompilerServices
Imports System.Collections
Imports System.Windows.Forms
 
Namespace keg.compound.gui


    Public Class CompoundCanvas
        Inherits java.awt.Panel
        Inherits java.awt.event.MouseListener
        Inherits java.awt.event.MouseMotionListener
        Inherits java.awt.event.KeyListener
        Inherits java.awt.event.ActionListener
        Inherits java.awt.event.WindowListener
        Private Shared MAC_OS_X As Boolean = False
        Private Shared ReadOnly SIN_30DEG As Double = Math.sin(0.5235987755982988D)
        Private Shared ReadOnly COS_30DEG As Double = Math.cos(0.5235987755982988D)
        Private Shared ReadOnly TAN_60DEG As Double = Math.tan(1.0471975511965976D)
        Friend parentK As CompoundPanel = Nothing
        Private conteiner As keg.compound.ChemConteiner
        Public foreC As Color = Color.black
        Public labelC As Color = Color.black
        Public label1C As Color = Color.black
        Public label2C As Color = Color.black
        Public reactantC As Color = Color.blue
        Public productC As Color = Color.red
        Public backC As Color = Color.white
        Private shrinkBackC As Color = New Color(245, 250, 255)
        Private grounedAtomColor As Color = New Color(240, 245, 250)
        Public highC As Color = Color.magenta
        Public select_frameC As Color = Color.darkGray
        Public lFont As java.awt.Font
        Public lFontSUP As java.awt.Font
        Public lFont1 As java.awt.Font
        Public lFont1SUP As java.awt.Font
        Public lFont2 As java.awt.Font
        Public lFont2SUP As java.awt.Font
        Public font_margin As Integer
        Public bFont As java.awt.Font
        Public bC As Color = Color.black
        Public blC As Color = Color.black
        Friend vHeight As Integer
        Friend vWidth As Integer
        Friend h0 As Integer = 0
        Friend v0 As Integer = 0
        Public fixed_length As Integer
        Public bold_width As Integer
        Public margin_width As Integer
        Public hash_spacing As Integer = 3
        Public bond_spacing As Double
        Private dispscale As Double
        Public tolerance As Integer = 3
        Public editmode As keg.compound.EditMode = Nothing
        Friend eventmode As Boolean = True
        Friend r As Integer
        Friend g As Integer
        Friend b As Integer
        Friend fName As String
        Friend fSize As Integer
        Friend fStyle As Integer
        Friend fm As java.awt.FontMetrics
        Friend fHeight As Integer
        Friend fWidth As Integer
        Friend fHalfW As Integer
        Public fHeight_discount As Integer = 2
        Friend xPoints() As Integer = New Integer(5) {}
        Friend yPoints() As Integer = New Integer(5) {}
        Public Const Integer RIGHT_SIDE = 1
	  Public Const Integer LEFT_SIDE = 2
	  Public element_text As java.awt.TextField = Nothing
        Public element_choice As java.awt.PopupMenu
        Public element_choice1 As java.awt.Menu
        Public element_choice2 As java.awt.Menu
        Public element_words() As java.awt.MenuItem
        Public element_words1() As java.awt.MenuItem
        Public element_words2() As java.awt.MenuItem
        Friend menuX As Integer
        Friend menuY As Integer
        Public bracket_text As java.awt.TextField
        Friend handlesize As Integer = 5
        Friend selectRegion As Rectangle = New Rectangle(-1, -1, 0, 0)
        Friend vec0 As Vector2D
        Friend cx As Integer
        Friend cy As Integer
        Friend rx1 As Integer
        Friend ry1 As Integer
        Friend rx2 As Integer
        Friend ry2 As Integer
        Friend rx3 As Integer
        Friend ry3 As Integer
        Friend rx4 As Integer
        Friend ry4 As Integer
        Friend lasso_points As ArrayList = New ArrayList()
        Friend markFlag_BracketLabel As keg.compound.Bracket
        Private downP As java.awt.Point = New java.awt.Point()
        Private upP As java.awt.Point = New java.awt.Point()
        Private moveP As java.awt.Point = New java.awt.Point()
        Private dragP As java.awt.Point = New java.awt.Point()
        Private prevDragP As java.awt.Point = New java.awt.Point()
        Private draggedFlag As Boolean = False
        Private mouseDragDraw2_n As Integer
        Private mouseDragDraw2_prevTh As Double
        Private selectFlagWhenChangeFromDownToDrag As Boolean
        Private selectFlagWhenChangeFromDownToDrag2 As Boolean
        Friend rfr As RFR = New RFR(Me)
        Private markFlag_Atom As keg.compound.Atom
        Private markFlag_Bond As keg.compound.Bond
        Private markFlag_Bracket As keg.compound.Bracket
        Private markFlag_ReactionArrow As keg.compound.ReactionArrow
        Friend imageForMove As java.awt.Image
        Friend graphicsForMove As java.awt.Graphics
        Friend ht As Hashtable = New Hashtable()
        Friend startSideFlag As Boolean = False
        Friend sideFlag As Boolean = False
        Friend mouseDragDraw2_dragP As java.awt.Point = New java.awt.Point()
        Friend mouseDragDraw2_tth As Double
        Friend mouseDragDraw2_depend As Boolean
        Friend mouseDragDraw2_draw As Boolean
        Private rtrTolerance As Integer = 5
        Friend selectedText As keg.compound.Text = Nothing
        Private _prevDraggedTextBorder As Rectangle = Nothing
        Friend textDialog As TextPreferenceDialog
        Friend lfontSUPHt As Hashtable = New Hashtable()
        Friend lfontHt As Hashtable = New Hashtable()
        Private grp_jd_ok As javax.swing.JButton
        Private grp_jd_cancel As javax.swing.JButton
        Private grp_jd_field As javax.swing.JTextField
        Private grp_jd_combo As javax.swing.JComboBox
        Private grp_jd As javax.swing.JDialog
        Private clickTimer As javax.swing.Timer = New javax.swing.Timer(300, Me)
        Private timer As javax.swing.Timer = New javax.swing.Timer(600, Me)
        Private pressEvent As java.awt.event.MouseEvent
        Private activeFlag As Boolean = False
        Friend clipboard As java.awt.datatransfer.Clipboard
        Private isEnabledShrinkPartMarker As Boolean = True

        Public void New() 
	  {
		Me.editmode = New keg.compound.EditMode()
		initParameter()
	  }
 
	  Public void New(CompoundPanel paramCompoundPanel, keg.compound.EditMode paramEditMode) 
	  {
		Me.editmode = paramEditMode
        Me.parentK = paramCompoundPanel
		Layout = (java.awt.LayoutManager)Nothing
		initParameter()
		initTextFileds()
		addMouseListener(Me)
		addMouseMotionListener(Me)
		addKeyListener(Me)
		paramCompoundPanel.kegF.addWindowListener(Me)
		refreshAtomsDrawLabel()
		Visible = True
	  }
 
	  Private void initParameter() 
	  {
		Try
        Me.r = Convert.ToInt32(keg.compound.DEBT.p.get("FORE_COLOR_R"))
        Me.g = Convert.ToInt32(keg.compound.DEBT.p.get("FORE_COLOR_G"))
        Me.b = Convert.ToInt32(keg.compound.DEBT.p.get("FORE_COLOR_B"))
		  normalizeRGB()
		  Me.foreC = New Color(Me.r, Me.g, Me.b)
        Catch localException1 As Exception
		  Console.WriteLine("Can not set parameter 'FORE_COLOR'.")
		  Console.WriteLine(localException1)
		End Try
        Try
        Me.r = Convert.ToInt32(keg.compound.DEBT.p.get("LABEL_COLOR_R"))
        Me.g = Convert.ToInt32(keg.compound.DEBT.p.get("LABEL_COLOR_G"))
        Me.b = Convert.ToInt32(keg.compound.DEBT.p.get("LABEL_COLOR_B"))
		  normalizeRGB()
		  Me.labelC = New Color(Me.r, Me.g, Me.b)
        Catch localException2 As Exception
		  Console.WriteLine("Can not set parameter 'LABEL_COLOR'.")
		  Console.WriteLine(localException2)
		End Try
        Try
        Me.r = Convert.ToInt32(keg.compound.DEBT.p.get("REACTANT_COLOR_R"))
        Me.g = Convert.ToInt32(keg.compound.DEBT.p.get("REACTANT_COLOR_G"))
        Me.b = Convert.ToInt32(keg.compound.DEBT.p.get("REACTANT_COLOR_B"))
		  normalizeRGB()
		  Me.reactantC = New Color(Me.r, Me.g, Me.b)
        Catch localException3 As Exception
		  Console.WriteLine("Can not set parameter 'REACTANT_COLOR'.")
		  Console.WriteLine(localException3)
		End Try
        Try
        Me.r = Convert.ToInt32(keg.compound.DEBT.p.get("PRODUCT_COLOR_R"))
        Me.g = Convert.ToInt32(keg.compound.DEBT.p.get("PRODUCT_COLOR_G"))
        Me.b = Convert.ToInt32(keg.compound.DEBT.p.get("PRODUCT_COLOR_B"))
		  normalizeRGB()
		  Me.productC = New Color(Me.r, Me.g, Me.b)
        Catch localException4 As Exception
		  Console.WriteLine("Can not set parameter 'PRODUCT_COLOR'.")
		  Console.WriteLine(localException4)
		End Try
        Try
        Me.r = Convert.ToInt32(keg.compound.DEBT.p.get("BRACKET_COLOR_R"))
        Me.g = Convert.ToInt32(keg.compound.DEBT.p.get("BRACKET_COLOR_G"))
        Me.b = Convert.ToInt32(keg.compound.DEBT.p.get("BRACKET_COLOR_B"))
		  normalizeRGB()
		  Me.bC = New Color(Me.r, Me.g, Me.b)
        Me.blC = Me.bC
        Catch localException5 As Exception
		  Console.WriteLine(localException5)
		End Try
        Try
        Me.r = Convert.ToInt32(keg.compound.DEBT.p.get("BACK_COLOR_R"))
        Me.g = Convert.ToInt32(keg.compound.DEBT.p.get("BACK_COLOR_G"))
        Me.b = Convert.ToInt32(keg.compound.DEBT.p.get("BACK_COLOR_B"))
		  normalizeRGB()
		  Me.backC = New Color(Me.r, Me.g, Me.b)
        Catch localException6 As Exception
		  Console.WriteLine("Can not set parameter 'BACK_COLOR'.")
		  Console.WriteLine(localException6)
		End Try
        Try
        Me.r = Convert.ToInt32(keg.compound.DEBT.p.get("HIGHLIGHT_R"))
        Me.g = Convert.ToInt32(keg.compound.DEBT.p.get("HIGHLIGHT_G"))
        Me.b = Convert.ToInt32(keg.compound.DEBT.p.get("HIGHLIGHT_B"))
		  normalizeRGB()
		  Me.highC = New Color(Me.r, Me.g, Me.b)
        Catch localException7 As Exception
		  Console.WriteLine("Can not set parameter 'HIGHLIGHT'.")
		  Console.WriteLine(localException7)
		End Try
        Try
        Me.r = Convert.ToInt32(keg.compound.DEBT.p.get("SELECT_FRAME_R"))
        Me.g = Convert.ToInt32(keg.compound.DEBT.p.get("SELECT_FRAME_G"))
        Me.b = Convert.ToInt32(keg.compound.DEBT.p.get("SELECT_FRAME_B"))
		  normalizeRGB()
		  Me.select_frameC = New Color(Me.r, Me.g, Me.b)
        Catch localException8 As Exception
		  Console.WriteLine("Can not set parameter 'SELECT_FRAME'.")
		  Console.WriteLine(localException8)
		End Try
        Try
        Me.fName = keg.compound.DEBT.FONT_NAMES(Convert.ToInt32(keg.compound.DEBT.p.get("LABEL_FONT")))
        Me.fSize = Convert.ToInt32(keg.compound.DEBT.p.get("LABEL_SIZE"))
        Me.fStyle = Convert.ToInt32(keg.compound.DEBT.p.get("LABEL_STYLE"))
        Me.lFont = New java.awt.Font(Me.fName, Me.fStyle, Me.fSize)
        Me.lFontSUP = New java.awt.Font(Me.fName, Me.fStyle, Me.fSize * 5 / 6)
        Me.font_margin = (Me.fSize * 2 / 3)
        Catch localException9 As Exception
		  Console.WriteLine("Can not set parameter 'LABEL_FONT'.")
		  Console.WriteLine(localException9)
		End Try
        Me.lFont1 = Me.lFont
        Me.lFont2 = Me.lFont
        Me.lFont1SUP = Me.lFontSUP
        Me.lFont2SUP = Me.lFontSUP
        Try
        Me.fName = keg.compound.DEBT.FONT_NAMES(Convert.ToInt32(keg.compound.DEBT.p.get("BRACKET_FONT")))
        Me.fSize = Convert.ToInt32(keg.compound.DEBT.p.get("BRACKET_SIZE"))
        Me.fStyle = Convert.ToInt32(keg.compound.DEBT.p.get("BRACKET_STYLE"))
        Me.bFont = New java.awt.Font(Me.fName, Me.fStyle, Me.fSize)
        Catch localException10 As Exception
		  Console.WriteLine(localException10)
		End Try
        Try
        Me.fixed_length = Convert.ToInt32(keg.compound.DEBT.p.get("FIXED_LENGTH"))
        Catch localException11 As Exception
		  Console.WriteLine("Can not set parameter 'FIXED_LENGTH'.")
		  Console.WriteLine(localException11)
		End Try
        Try
        Me.bold_width = Convert.ToInt32(keg.compound.DEBT.p.get("BOLD_WIDTH"))
        Catch localException12 As Exception
		  Console.WriteLine("Can not set parameter 'BOLD_WIDTH'.")
		  Console.WriteLine(localException12)
		End Try
        Try
        Me.margin_width = Convert.ToInt32(keg.compound.DEBT.p.get("MARGIN_WIDTH"))
        Catch localException13 As Exception
		  Console.WriteLine("Can not set parameter 'MARGIN_WIDTH'.")
		  Console.WriteLine(localException13)
		End Try
        Try
        Me.hash_spacing = Convert.ToInt32(keg.compound.DEBT.p.get("HASH_SPACING"))
        Catch localException14 As Exception
		  Console.WriteLine("Can not set parameter 'HASH_SPACING'.")
		  Console.WriteLine(localException14)
		End Try
        Try
        Me.bond_spacing = (Convert.ToInt32(keg.compound.DEBT.p.get("BOND_SPACING")) / 100D)
        Catch localException15 As Exception
		  Console.WriteLine("Can not set parameter 'BOND_SPACING'.")
		  Console.WriteLine(localException15)
		End Try
        Try
        Me.tolerance = Convert.ToInt32(keg.compound.DEBT.p.get("TOLERANCE"))
        Catch localException16 As Exception
		  Console.WriteLine("Can not set parameter 'TOLERANCE'.")
		  Console.WriteLine(localException16)
		End Try
        Dim localDouble1 As Double? = New Double?(keg.compound.DEBT.p.get("FIXED_LENGTH"))

        Dim localDouble2 As Double? = New Double?(keg.compound.DEBT.p.get("COORDINATE_LENGTH"))

		DispScale = (Double)localDouble1 / (Double)localDouble2
	  }
 
	  Private void initTextFileds() 
	  {
		Me.element_text = New java.awt.TextField(8)
        Me.element_text.Background = Color.white
        Me.element_text.Foreground = Color.black
		add(Me.element_text)
		Me.element_text.hide()
        Me.element_text.resize(0, 0)
        Me.element_choice = New java.awt.PopupMenu()
        Me.element_choice.Font = Me.lFont
        Dim localVector As ArrayList = Me.parentK.AtomTemplate
        Me.element_words = New java.awt.MenuItem(localVector.Count - 2) {}
        Me.element_words(0) = New java.awt.MenuItem((String)localVector(0))
		Me.element_choice.add(Me.element_words(0))
        Me.element_choice1 = New java.awt.Menu((String)localVector(localVector.Count - 2))
		Me.element_choice1.Font = Me.lFont1
        Me.element_choice2 = New java.awt.Menu((String)localVector(localVector.Count - 1))
		Me.element_choice2.Font = Me.lFont2
        Dim i As Integer
        For i = 1 To localVector.Count - 2 - 1 Step i + 1
        Me.element_words(i) = New java.awt.MenuItem((String)localVector(i))
		  Me.element_words(i).Font = Me.lFont
        Me.element_choice.add(Me.element_words(i))
        Next
        Me.element_words((localVector.Count - 3)).Font = New java.awt.Font(Me.lFont.Name, 2, Me.lFont.Size)
        Me.element_choice.add(Me.element_choice1)
        Me.element_choice.add(Me.element_choice2)
		localVector = Me.parentK.AtomTemplate1
		Me.element_words1 = New java.awt.MenuItem(localVector.Count) {}
        For i = 0 To localVector.Count - 1 Step i + 1
        Me.element_words1(i) = New java.awt.MenuItem((String)localVector(i))
		  Me.element_words1(i).Font = Me.lFont1
        Me.element_choice1.add(Me.element_words1(i))
        Next
		localVector = Me.parentK.AtomTemplate2
		Me.element_words2 = New java.awt.MenuItem(localVector.Count) {}
        For i = 0 To localVector.Count - 1 Step i + 1
        Me.element_words2(i) = New java.awt.MenuItem((String)localVector(i))
		  Me.element_words2(i).Font = Me.lFont2
        Me.element_choice2.add(Me.element_words2(i))
        Next
		add(Me.element_choice)
		Me.bracket_text = New java.awt.TextField(8)
        Me.bracket_text.Background = Color.white
        Me.bracket_text.Foreground = Color.black
		add("Center", Me.bracket_text)
		Me.bracket_text.hide()
        Me.bracket_text.resize(0, 0)
        Me.bracket_text.addActionListener(Me)
	  }
 
	  Public virtual void resetAtomTemplate() 
	  {
		Me.element_choice1.removeAll()
        Me.element_choice2.removeAll()
        Me.element_choice.removeAll()
        Me.element_choice.Font = Me.lFont
        Dim localVector As ArrayList = Me.parentK.AtomTemplate
        Me.element_words = New java.awt.MenuItem(localVector.Count - 2) {}
        Me.element_words(0) = New java.awt.MenuItem((String)localVector(0))
		Me.element_choice.add(Me.element_words(0))
        Me.element_choice1 = New java.awt.Menu((String)localVector(localVector.Count - 2))
		Me.element_choice1.Font = Me.lFont1
        Me.element_choice2 = New java.awt.Menu((String)localVector(localVector.Count - 1))
		Me.element_choice2.Font = Me.lFont2
        Dim i As Integer
        For i = 1 To localVector.Count - 2 - 1 Step i + 1
        Me.element_words(i) = New java.awt.MenuItem((String)localVector(i))
		  Me.element_words(i).Font = Me.lFont
        Me.element_choice.add(Me.element_words(i))
        Next
        Me.element_words((localVector.Count - 3)).Font = New java.awt.Font(Me.lFont.Name, 2, Me.lFont.Size)
        Me.element_choice.add(Me.element_choice1)
        Me.element_choice.add(Me.element_choice2)
		localVector = Me.parentK.AtomTemplate1
		Me.element_words1 = New java.awt.MenuItem(localVector.Count) {}
        For i = 0 To localVector.Count - 1 Step i + 1
        Me.element_words1(i) = New java.awt.MenuItem((String)localVector(i))
		  Me.element_words1(i).Font = Me.lFont1
        Me.element_choice1.add(Me.element_words1(i))
        Next
		localVector = Me.parentK.AtomTemplate2
		Me.element_words2 = New java.awt.MenuItem(localVector.Count) {}
        For i = 0 To localVector.Count - 1 Step i + 1
        Me.element_words2(i) = New java.awt.MenuItem((String)localVector(i))
		  Me.element_words2(i).Font = Me.lFont2
        Me.element_choice2.add(Me.element_words2(i))
        Next
	  }
 
	  Public Overridable WriteOnly Property DispScale() As Double
            Set(ByVal Value As Double)
                Me.dispscale = value
                If (Me.editmode <> Nothing) Then
			{
			  Me.editmode.Scale = Me.dispscale
			}
        End Set
        End Property

        Public Overridable Property VirtualScreen() As DblRect
            Get
                Return New DblRect(Me.vWidth, Me.vHeight)
            End Get
            Set(ByVal Value As DblRect)
                setVirtualScreen(value.width, value.height)
            End Set
        End Property

        Public virtual void setVirtualScreen(Integer paramInt1, Integer paramInt2) 
	  {
		Me.vWidth = paramInt1
        Me.vHeight = paramInt2
	  }
 
 
	  Public Overridable Property Hoffset() As Integer
            Get
                Return Me.h0
            End Get
            Set(ByVal Value As Integer)
                Me.h0 = value
            End Set
        End Property

        Public Overridable Property Voffset() As Integer
            Get
                Return Me.v0
            End Get
            Set(ByVal Value As Integer)
                Me.v0 = value
            End Set
        End Property



        Private void normalizeRGB() 
	  {
		While Me.r > 255
        Dim - As Me.r =  256 
		End While
        While Me.r < 0
        Me.r += 256
        End While
        While Me.g > 255
        Dim - As Me.g =  256 
		End While
        While Me.g < 0
        Me.g += 256
        End While
        While Me.b > 255
        Dim - As Me.b =  256 
		End While
        While Me.b < 0
        Me.b += 256
        End While
	  }
 
	  Public Overridable WriteOnly Property Conteiner() As keg.compound.ChemConteiner
            Set(ByVal Value As keg.compound.ChemConteiner)
                Me.conteiner = value
                refreshAtomsDrawLabel()
            End Set
        End Property

        Public virtual Boolean mouseDown(java.awt.Event.MouseEvent paramMouseEvent, Integer paramInt1, Integer paramInt2) 
	  {
		Me.downP.x = (paramInt1 + Hoffset)
        Me.downP.y = (paramInt2 + Voffset)
        Me.prevDragP.x = Me.downP.x
        Me.prevDragP.y = Me.downP.y
        If (Me.element_text.Visible)
		{
		  cancelAtomLabel()
		}
		If (Not Me.eventmode)
		{
		  stopTimer()
		  Return False
		}
		If (Me.editmode.near_object <> Nothing)
		{
		  If ((Me.editmode.near_object Is keg.compound.Atom))
		  {
			paintAtomMark(Nothing, (keg.compound.Atom)Me.editmode.near_object)
		  }
 ElseIf ((Me.editmode.near_object Is keg.compound.Bond))
		  {
			paintBondMark(Nothing, (keg.compound.Bond)Me.editmode.near_object)
		  }
		}
		If (Me.editmode.operation = 3)
		{
		  Me.selectedText = onText(Me.downP)
		}
		Me.editmode.near_object = Nothing
        Select Case Me.editmode.operation
        Case 1

        Return mouseDownDraw(paramInt1, paramInt2)
        Case 2

        Case 3

        Case 8 : 

		  stopTimer()
		  Return mouseDownSelect(paramMouseEvent)
        Case 4 : 

		  stopTimer()
		  Return mouseDownLabel(paramInt1, paramInt2, paramMouseEvent.ShiftDown, paramMouseEvent.ControlDown)
        Case 7 : 

		  stopTimer()
		  Return mouseDownBracket(paramInt1, paramInt2)
        End Select
		stopTimer()
		Return False
	  }
 
	  Public Static void printReaction(String paramString, keg.compound.Reaction paramReaction) 
	  {
		Console.Error.WriteLine(paramString)
		printReaction(paramReaction)
	  }
 
	  Public Static void printReaction(keg.compound.Reaction paramReaction) 
	  {
		Console.Error.WriteLine("---- Reaction viewer ----" + paramReaction.objectNum())
		Dim localObject1 As Object
        Dim localObject2 As Object
        Dim i As Integer
        For i = 0 To paramReaction.objectNum() - 1 Step i + 1
		  localObject1 = paramReaction.getObject(i)
		  If ((localObject1 Is keg.compound.Molecule))
		  {
			localObject2 = (keg.compound.Molecule)localObject1
			printMol((keg.compound.Molecule)localObject2)
		  }
 ElseIf ((localObject1 Is keg.compound.ReactionArrow))
		  {
			localObject2 = (keg.compound.ReactionArrow)localObject1
			Console.Error.WriteLine("ReactionArrow " +((keg.compound.ReactionArrow)localObject2).id)
		  }
		Next
        For i = 0 To paramReaction.bracketNum() - 1 Step i + 1
		  localObject1 = paramReaction.getBracket(i)
		  Console.Error.WriteLine("Bracket\t" + localObject1.GetHashCode() + "\t[ ((" + ((keg.compound.Bracket)localObject1).b1x1 + "," + ((keg.compound.Bracket)localObject1).b1y1 + "),(" + ((keg.compound.Bracket)localObject1).b1x2 + "," + ((keg.compound.Bracket)localObject1).b1y2 + ")),((" + ((keg.compound.Bracket)localObject1).b2x1 + "," + ((keg.compound.Bracket)localObject1).b2y1 + "),(" + ((keg.compound.Bracket)localObject1).b2x2 + "," + ((keg.compound.Bracket)localObject1).b2y2 + "))]")
		  localObject2 = ((keg.compound.Bracket)localObject1).CoveredWholeMol = True ? "FOR_ATOMS" : "FOR_REPEAT"

		  Dim j As Integer = ((keg.compound.Bracket)localObject1).Sgroup <> IIf(  Nothing , ((keg.compound.Bracket)localObject1).Sgroup.Count ,  0 )

		  Console.Error.WriteLine("       \t" + localObject1.GetHashCode() + "\t[ " + (String)localObject2 + ":" + j + (((keg.compound.Bracket)localObject1).CoveredWholeMol = True ? "FOR_ATOMS"  "FOR_REPEAT"))

		Next
		Console.Error.WriteLine("---- Reaction viewer end ----")
	  }
 
	  Public Static void printMol(String paramString, keg.compound.Molecule paramMolecule) 
	  {
		Console.Error.WriteLine(paramString)
		printMol(paramMolecule)
	  }
 
	  Public Static void printMol(keg.compound.Molecule paramMolecule) 
	  {
		Console.Error.WriteLine("Molecule\t" + paramMolecule.GetHashCode() + "\t[" + paramMolecule.AtomNum + " " + paramMolecule.BondNum + " " + paramMolecule.BracketNum)
		Dim localObject As Object
        Dim i As Integer
        For i = 0 To paramMolecule.AtomNum - 1 Step i + 1
		  localObject = paramMolecule.getAtom(i + 1)
		  Console.Error.WriteLine(" +Atom\t\t" + localObject.GetHashCode() + "\t[" + paramMolecule.getAtomNo((keg.compound.Atom)localObject) + ": " + ((keg.compound.Atom)localObject).x + "," + ((keg.compound.Atom)localObject).y + "]")
		Next
        For i = 0 To paramMolecule.BondNum - 1 Step i + 1
		  localObject = paramMolecule.getBond(i + 1)
		  Console.Error.WriteLine(" +Bond\t\t" + localObject.GetHashCode() + "\t[" + paramMolecule.getAtomNo(((keg.compound.Bond)localObject).Atom1) + ", " + paramMolecule.getAtomNo(((keg.compound.Bond)localObject).Atom2))
		Next
        For i = 0 To paramMolecule.BracketNum - 1 Step i + 1
		  localObject = paramMolecule.getBracket(i + 1)
		  Console.Error.WriteLine(" +Bracket\t" + localObject.GetHashCode() + "\t[" + ((keg.compound.Bracket)localObject).CoveredWholeMol + "\t" + ((keg.compound.Bracket)localObject).getRect(1.0D))
		Next
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        Public virtual void unselect() 
	  {
		((keg.compound.Reaction)Me.conteiner).unselect()
		Me.rfr.unselected()
        Me.editmode.clear()
        Me.editmode.resetArea()
	  }
 
	  Public virtual void refreshAtomsDrawLabel() 
	  {
		If (Me.conteiner = Nothing)
		{
		  Return
		}
		Dim i As Integer
        For i = 0 To ((keg.compound.Reaction)Me.conteiner).objectNum()- 1  Step i + 1
        Dim localChemObject As keg.compound.ChemObject = ((keg.compound.Reaction)Me.conteiner).getObject(i) 
		  If ((localChemObject Is keg.compound.Molecule))
		  {
			Dim j As Integer
        For j = 0 To ((keg.compound.Molecule)localChemObject).AtomNum- 1  Step j + 1
        Dim localAtom As keg.compound.Atom = ((keg.compound.Molecule)localChemObject).getAtom(j) 
			  If (localAtom <> Nothing)
			  {
				localAtom.DrawLabel = Me.editmode.hydrogen_draw
			  }
			Next
		  }
		Next
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseDownDraw(Integer paramInt1, Integer paramInt2) 
	  {
		unselect()
		Dim localAtom As keg.compound.Atom = Me.conteiner.nearAtom(Me.downP.x, Me.downP.y, Me.dispscale, Me.tolerance)
        Me.editmode.status = 0
        Me.editmode.atom = ((keg.compound.Atom)Nothing)
		Me.editmode.bond = ((keg.compound.Bond)Nothing)
		Me.editmode.status = 1
        If (localAtom <> Nothing)
		{
		  Me.downP.x = localAtom.DX(Me.dispscale)
        Me.downP.y = localAtom.DY(Me.dispscale)
        Me.editmode.atom = localAtom
		}
 Else
		{
		  Dim localBond As keg.compound.Bond = Me.conteiner.nearBond(Me.downP.x, Me.downP.y, Me.dispscale, Me.tolerance)
        Me.editmode.bond = localBond
		}
		Me.editmode.UNDO = (keg.compound.Reaction)Me.conteiner
		Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseDownErase(Integer paramInt1, Integer paramInt2) 
	  {
		Dim localAtom As keg.compound.Atom = Me.conteiner.nearAtom(Me.downP.x, Me.downP.y, Me.dispscale, Me.tolerance)
        Me.editmode.status = 0
        Me.editmode.atom = ((keg.compound.Atom)Nothing)
		Me.editmode.bond = ((keg.compound.Bond)Nothing)
		Me.editmode.status = 7
        If (localAtom <> Nothing)
		{
		  Me.downP.x = localAtom.DX(Me.dispscale)
        Me.downP.y = localAtom.DY(Me.dispscale)
        Me.editmode.atom = localAtom
		}
 Else
		{
		  Dim localBond As keg.compound.Bond = Me.conteiner.nearBond(Me.downP.x, Me.downP.y, Me.dispscale, Me.tolerance)
        Me.editmode.bond = localBond
		}
		Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseDownSelect(java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		Dim bool1 As Boolean = paramMouseEvent.ShiftDown
        Dim bool2 As Boolean = paramMouseEvent.ControlDown
        Dim bool3 As Boolean = paramMouseEvent.MetaDown
        Me.selectFlagWhenChangeFromDownToDrag = True
        Me.selectFlagWhenChangeFromDownToDrag2 = True
        Me.vec0 = New Vector2D(Me.downP.x - Me.cx, Me.downP.y - Me.cy)
        Me.rx1 = (Me.rx2 = Me.rx3 = Me.rx4 = 0)
        Me.ry1 = (Me.ry2 = Me.ry3 = Me.ry4 = 0)
        If ((Me.editmode.operation = 8) And (Me.rfr.contains(Me.downP.x, Me.downP.y)))
		{
		  Me.rfr.selected()
		  repaint()
		  Me.editmode.status = 4
        Return False
		}
		Me.rfr.unselected()
        If (((Me.editmode.selected.Count > 1) Or ((Me.editmode.selected.Count = 1) And (Not (Me.editmode.selected(0) Is keg.compound.Atom)))) And (Me.downP.x > Me.editmode.select_area.x + Me.editmode.select_area.width + Me.tolerance) And (Me.downP.x < Me.editmode.select_area.x + Me.editmode.select_area.width + Me.tolerance + 2 + Me.handlesize))
		{
		  If ((Me.downP.y > Me.editmode.select_area.y - Me.tolerance) And (Me.downP.y < Me.editmode.select_area.y - Me.tolerance + 2 + Me.handlesize))
		  {
			Me.editmode.status = 4
        If (Me.editmode.operation = 3)
			{
			  Me.cx = (Me.editmode.select_area.x - Me.tolerance + (Me.editmode.select_area.width + Me.tolerance * 2 + 1 + Me.handlesize) / 2)
        Me.cy = (Me.editmode.select_area.y - Me.tolerance + (Me.editmode.select_area.height + Me.tolerance * 2) / 2)
        Me.vec0 = New Vector2D(Me.downP.x - Me.cx, Me.downP.y - Me.cy)
        Me.rfr.x = Me.cx
        Me.rfr.y = Me.cy
			}
			Return False
		  }
		  If ((Me.downP.y > Me.editmode.select_area.y + Me.editmode.select_area.height + Me.tolerance - 1 - Me.handlesize) And (Me.downP.y < Me.editmode.select_area.y + Me.editmode.select_area.height + Me.tolerance))
		  {
			Me.editmode.status = 5
        Return False
		  }
		}
		Dim localAtom As keg.compound.Atom = Me.conteiner.nearAtom(Me.downP.x, Me.downP.y, Me.dispscale, Me.tolerance, ShrinkMode)
        Dim localBond As keg.compound.Bond = Me.conteiner.nearBond(Me.downP.x, Me.downP.y, Me.dispscale, Me.tolerance, ShrinkMode)
        Me.editmode.status = 0
        Me.editmode.atom = ((keg.compound.Atom)Nothing)
		Me.editmode.bond = ((keg.compound.Bond)Nothing)
		Me.editmode.bracket = ((keg.compound.Bracket)Nothing)
		Me.editmode.chemobject = ((keg.compound.ChemObject)Nothing)
		If (bool1)
		{
		  Me.editmode.status = 2
        Me.editmode.atom = localAtom
        Me.editmode.bond = Me.conteiner.nearBond(Me.downP.x, Me.downP.y, Me.dispscale, Me.tolerance)
        If ((Me.editmode.atom = Nothing) And (Me.editmode.bond = Nothing))
		  {
			Me.editmode.chemobject = ((keg.compound.Reaction)Me.conteiner).nearBracket(Me.downP.x, Me.downP.y, Me.dispscale, Me.tolerance)
			If (Me.editmode.chemobject = Nothing)
			{
			  Me.editmode.bracket = ((keg.compound.Reaction)Me.conteiner).nearBracketLabel(Me.downP.x, Me.downP.y, Me.dispscale, 1)
			}
		  }
 Else
		  {
			Me.editmode.chemobject = ((keg.compound.ChemObject)Nothing)
		  }
		  If ((Me.editmode.select_mode = 0) Or (Me.editmode.operation = 8))
		  {
			Me.lasso_points.Clear()
        Me.lasso_points.Add(New DblRect(Me.downP.x, Me.downP.y))
		  }
		}
 ElseIf (bool3)
		{
		  If (localAtom <> Nothing)
		  {
			Me.editmode.atom = localAtom
        Me.editmode.status = 2
			localAtom.Mol.selectAllItems(Me.editmode, ShrinkMode)
			Me.editmode.resetArea()
			repaint()
		  }
 ElseIf (localBond <> Nothing)
		  {
			Me.editmode.bond = localBond
        Me.editmode.status = 2
			localBond.Mol.selectAllItems(Me.editmode, ShrinkMode)
			Me.editmode.resetArea()
			repaint()
		  }
 Else
		  {
			Me.editmode.status = 2
		  }
		}
 ElseIf (localAtom <> Nothing)
		{
		  Me.editmode.atom = localAtom
        If (localAtom.Select)
		  {
			Me.editmode.status = 3
        If (Me.editmode.operation = 8)
			{
			  Me.editmode.status = 4
			}
		  }
 Else
		  {
			Me.editmode.status = 2
        If ((Me.editmode.select_mode = 0) Or (Me.editmode.operation = 8))
			{
			  Me.lasso_points.Clear()
        Me.lasso_points.Add(New DblRect(Me.downP.x, Me.downP.y))
			}
			Me.editmode.atom = localAtom
			hideAtomTextFiled()
			hideBracketTextFiled()
			Me.editmode.clear()
        Me.conteiner.unselect()
			repaint()
		  }
		}
 ElseIf (localBond <> Nothing)
		{
		  Me.editmode.bond = localBond
        If (localBond.Select)
		  {
			Me.editmode.status = 3
        If (Me.editmode.operation = 8)
			{
			  Me.editmode.status = 4
			}
		  }
 Else
		  {
			Me.editmode.status = 2
		  }
		  hideAtomTextFiled()
		  hideBracketTextFiled()
		}
 Else
		{
		  Dim localBracket As keg.compound.Bracket = ((keg.compound.Reaction)Me.conteiner).nearBracket(Me.downP.x, Me.downP.y, Me.dispscale, Me.tolerance) 
		  If (localBracket = Nothing)
		  {
			localBracket = ((keg.compound.Reaction)Me.conteiner).nearBracketLabel(Me.downP.x, Me.downP.y, Me.dispscale, 1)
			Me.editmode.bracket = ((keg.compound.Bracket)localBracket)
		  }
 Else
		  {
			Me.editmode.chemobject = localBracket
		  }
		  If ((localBracket <> Nothing) And (localBracket.Select))
		  {
			Me.editmode.status = 3
        If (Me.editmode.operation = 8)
			{
			  Me.editmode.status = 4
			}
		  }
 ElseIf (Me.editmode.select_area.contains(Me.downP.x, Me.downP.y))
		  {
			Me.editmode.status = 3
        If (Me.editmode.operation = 8)
			{
			  Me.editmode.status = 4
			}
		  }
 Else
		  {
			Me.editmode.status = 2
        If ((Me.editmode.select_mode = 0) Or (Me.editmode.operation = 8))
			{
			  Me.lasso_points.Clear()
        Me.lasso_points.Add(New DblRect(Me.downP.x, Me.downP.y))
			}
			Me.editmode.clear()
			hideAtomTextFiled()
			hideBracketTextFiled()
			Me.conteiner.unselect()
			repaint()
		  }
		}
		Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseDownLabel(Integer paramInt1, Integer paramInt2, Boolean paramBoolean1, Boolean paramBoolean2) 
	  {
		unselect()
		Me.editmode.status = 6
        Me.editmode.atom = Me.conteiner.nearAtom(Me.downP.x, Me.downP.y, Me.dispscale, Me.tolerance)
        If (paramBoolean1)
		{
		  If (Me.editmode.atom <> Nothing)
		  {
			Me.editmode.atom.FullLabel = Me.editmode.atom_label
        Me.editmode.atom.calcImplicitHydrogen()
        Me.editmode.atom.decisideHydrogenDraw()
        Me.editmode.flag_edit = True
        If (Me.parentK <> Nothing)
			{
			  Me.parentK.checkButton()
			}
		  }
		}
 ElseIf (Not paramBoolean2)
		{
		  If (Me.editmode.atom <> Nothing)
		  {
			Me.element_text.Text = Me.editmode.atom.FullLabel
		  }
 Else
		  {
			Me.element_text.Text = ""
		  }
		  Me.element_text.resize(Me.element_text.preferredSize())
        Dim localDimension2 As DblRect = size()
        Dim localDimension1 As DblRect = Me.element_text.size()
        Dim i As Integer = paramInt1 - 4
        Dim j As Integer = paramInt2 - localDimension1.height / 2
        If (i < 0)
		  {
			i = 0
		  }
		  If (j < 0)
		  {
			j = 0
		  }
		  If (i + localDimension1.width > localDimension2.width)
		  {
			i = localDimension2.width - localDimension1.width
		  }
		  If (j + localDimension1.height > localDimension2.height)
		  {
			j = localDimension2.height - localDimension1.height
		  }
		  Me.element_text.move(i, j)
        Me.element_text.show()
        Me.element_text.requestFocus()
		}
 Else
		{
		  Me.menuX = paramInt1
        Me.menuY = paramInt2
        Me.element_choice.show(this, paramInt1, paramInt2)
		}
		repaint()
		Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseDownArrow(Integer paramInt1, Integer paramInt2) 
	  {
		Me.editmode.status = 1
        Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseDownBracket(Integer paramInt1, Integer paramInt2) 
	  {
		Me.editmode.atom = ((keg.compound.Atom)Nothing)
		Me.editmode.bond = ((keg.compound.Bond)Nothing)
		Me.editmode.bracket = ((keg.compound.Reaction)Me.conteiner).nearBracketLabel(Me.downP.x, Me.downP.y, Me.dispscale, 1)
		cancelAtomLabel()
		If (Me.editmode.bracket <> Nothing)
		{
		  showBracketTextFiled(paramInt1, paramInt2)
		}
 Else
		{
		  cancelAtomLabel()
		  Me.editmode.status = 1
		  repaint()
		}
		Return False
	  }
 
	  Private void showBracketTextFiled(Integer paramInt1, Integer paramInt2) 
	  {
		Me.editmode.status = 6
        If (Me.editmode.bracket <> Nothing)
		{
		  Me.bracket_text.Text = Me.editmode.bracket.Label
		}
		Me.bracket_text.resize(Me.bracket_text.preferredSize())
        Dim localDimension1 As DblRect = size()
        Dim localDimension2 As DblRect = Me.bracket_text.size()
        Dim i As Integer = paramInt1 - 4
        Dim j As Integer = paramInt2 - localDimension2.height / 2
        If (i < 0)
		{
		  i = 0
		}
		If (j < 0)
		{
		  j = 0
		}
		If (i + localDimension2.width > localDimension1.width)
		{
		  i = localDimension1.width - localDimension2.width
		}
		If (j + localDimension2.height > localDimension1.height)
		{
		  j = localDimension1.height - localDimension2.height
		}
		Me.bracket_text.move(i, j)
        Me.bracket_text.show()
        Me.bracket_text.requestFocus()
	  }
 
	  Private void hideBracketTextFiled() 
	  {
		Me.bracket_text.hide()
        Me.bracket_text.resize(0, 0)
	  }
 
	  Private void hideAtomTextFiled() 
	  {
		Me.element_text.hide()
        Me.element_text.resize(0, 0)
	  }
 
	  Public virtual Boolean mouseMove(java.awt.Event.MouseEvent paramMouseEvent, Integer paramInt1, Integer paramInt2) 
	  {
		Me.moveP.x = (paramInt1 + Hoffset)
        Me.moveP.y = (paramInt2 + Voffset)
        If (Not Me.eventmode)
		{
		  Return False
		}
		Dim i As Integer = 0
        Me.markFlag_Atom = Nothing
        Me.markFlag_Bond = Nothing
        Me.markFlag_Bracket = Nothing
        Me.markFlag_BracketLabel = Nothing
        Dim localObject As Object = Me.conteiner.nearAtom(Me.moveP.x, Me.moveP.y, Me.dispscale, Me.tolerance, ShrinkMode)
        If ((localObject = Nothing) And ((Me.editmode.operation = 2) Or (Me.editmode.operation = 4) Or (Me.editmode.operation = 3) Or (Me.editmode.operation = 1)))
		{
		  localObject = Me.conteiner.nearBond(Me.moveP.x, Me.moveP.y, Me.dispscale, Me.tolerance, ShrinkMode)
		}
		If ((localObject = Nothing) And ((Me.editmode.operation = 2) Or (Me.editmode.operation = 3)))
		{
		  localObject = ((keg.compound.Reaction)Me.conteiner).nearBracket(Me.moveP.x, Me.moveP.y, Me.dispscale, Me.tolerance)
		}
		If ((localObject = Nothing) And ((Me.editmode.operation = 2) Or (Me.editmode.operation = 3)))
		{
		  localObject = Me.conteiner.nearChemObject(Me.moveP.x, Me.moveP.y, Me.tolerance)
		}
		If ((localObject = Nothing) And ((Me.editmode.operation = 7) Or (Me.editmode.operation = 3)))
		{
		  localObject = ((keg.compound.Reaction)Me.conteiner).nearBracketLabel(Me.moveP.x, Me.moveP.y, Me.dispscale, 1)
		  If (localObject <> Nothing)
		  {
			i = 1
		  }
		}
		If (localObject <> Me.editmode.near_object)
		{
		  If (Me.editmode.near_object <> Nothing)
		  {
			If ((Me.editmode.near_object Is keg.compound.Atom))
			{
			  Me.markFlag_Atom = ((keg.compound.Atom)Me.editmode.near_object)
			  If ((ShrinkMode) And (Me.markFlag_Atom.Express_group) And (Me.editmode.operation = 1))
			  {
				Me.markFlag_Atom = Nothing
			  }
			}
 ElseIf ((Me.editmode.near_object Is keg.compound.Bond))
			{
			  Me.markFlag_Bond = ((keg.compound.Bond)Me.editmode.near_object)
			}
 ElseIf ((Me.editmode.near_object Is keg.compound.Bracket))
			{
			  Me.markFlag_Bracket = ((keg.compound.Bracket)Me.editmode.near_object)
			}
 ElseIf ((Me.editmode.near_object Is keg.compound.ReactionArrow))
			{
			  Me.markFlag_ReactionArrow = ((keg.compound.ReactionArrow)Me.editmode.near_object)
			}
		  }
 ElseIf (localObject <> Nothing)
		  {
			If ((localObject Is keg.compound.Atom))
			{
			  Me.markFlag_Atom = ((keg.compound.Atom)localObject)
			}
 ElseIf ((localObject Is keg.compound.Bond))
			{
			  Me.markFlag_Bond = ((keg.compound.Bond)localObject)
			}
 ElseIf ((localObject Is keg.compound.Bracket))
			{
			  If (i = 0)
			  {
				Me.markFlag_Bracket = ((keg.compound.Bracket)localObject)
			  }
 Else
			  {
				Me.markFlag_BracketLabel = ((keg.compound.Bracket)localObject)
			  }
			}
 ElseIf ((localObject Is keg.compound.ReactionArrow))
			{
			  Me.markFlag_ReactionArrow = ((keg.compound.ReactionArrow)localObject)
			}
		  }
		}
		repaint()
		Return False
	  }
 
	  Public virtual Boolean mouseDrag(java.awt.Event.MouseEvent paramMouseEvent, Integer paramInt1, Integer paramInt2) 
	  {
		Dim @bool As Boolean =  False 
		Me.dragP.x = (paramInt1 + Hoffset)
        Me.dragP.y = (paramInt2 + Voffset)
        If (Not Me.eventmode)
		{
		  Return @bool
		}
		If (Me.editmode.status = 0)
		{
		  Return @bool
		}
		If ((Me.editmode.operation = 3) And (Me.selectedText <> Nothing))
		{
		  Dim i As Integer = Me.dragP.x - Me.downP.x
        Dim j As Integer = Me.dragP.y - Me.downP.y
		  repaint()
		  Dim localGraphics As java.awt.Graphics = Graphics
		  localGraphics.XORMode = Me.backC
		  Dim localRectangle1 As Rectangle = Me._prevDraggedTextBorder
        Dim localRectangle2 As Rectangle = Me.selectedText.Bounds
        If (localRectangle1 <> Nothing)
		  {
			localGraphics.drawRect(localRectangle1.x, localRectangle1.y, localRectangle1.width, localRectangle1.height)
		  }
		  localGraphics.drawRect(localRectangle2.x + i, localRectangle2.y + j, localRectangle2.width, localRectangle2.height)
		  Me._prevDraggedTextBorder = New Rectangle(localRectangle2.x + i, localRectangle2.y + j, localRectangle2.width, localRectangle2.height)
        Return @bool
		}
		Select Case Me.editmode.operation
        Case 1

        If (paramMouseEvent.MetaDown)
		  {
			Return @bool
		  }
		  If (Me.editmode.draw = 10)
		  {
			@bool = mouseDragDraw_CarbonChain(paramMouseEvent)
		  }
 ElseIf (paramMouseEvent.AltDown)
		  {
			Select Case Me.editmode.draw
        Case 0

        Case 1

        Case 2

        Case 3

        Case 4

        Case 5

        Case 10 : 

			  @bool = mouseDragDraw_CarbonChain(paramMouseEvent)
		  Exit For
        End Select
			@bool = False
		  }
 Else
		  {
			@bool = mouseDragDraw(paramMouseEvent)
		  }
		  Exit For
        Case 2

        Case 3

        Case 8

        If ((Me.selectFlagWhenChangeFromDownToDrag) And ((Me.editmode.status = 3) Or (Me.editmode.status = 4) Or (Me.editmode.status = 5)))
		  {
			Me.editmode.UNDO = (keg.compound.Reaction)Me.conteiner
			Me.selectFlagWhenChangeFromDownToDrag = False
		  }
		  Select Case Me.editmode.status
        Case 3 : 

			@bool = mouseDragMove()
			Exit For
        Case 2 : 

			@bool = mouseDragSelect()
			Exit For
        Case 4 : 

			@bool = mouseDragRotate()
			Exit For
        Case 5 : 

			@bool = mouseDragResize()
		Exit For
        End Select
        Exit For
        Case 6 : 

		  @bool = mouseDragArrow()
		  Exit For
        Case 7 : 

		  @bool = mouseDragBracket()
	  Exit For
        End Select
        Me.prevDragP.x = Me.dragP.x
        Me.prevDragP.y = Me.dragP.y
        Return @bool
	  }
 
	  Private Boolean mouseDragDraw_CarbonChain(java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		Dim arrayOfDouble() As Double = atan2to2Points(New java.awt.Point(Me.dragP.x, Me.dragP.y), Me.downP)
        If (Me.draggedFlag)
		{
		  localPoint1 = getRotateGrid(Me.dragP, Me.downP, paramMouseEvent.ShiftDown)
		  Me.dragP.x = localPoint1.x
        Me.dragP.y = localPoint1.y
		}
		Dim localPoint1 As java.awt.Point = New java.awt.Point(Me.downP.x, Me.downP.y)
        Dim localPoint2 As java.awt.Point = New java.awt.Point(Me.dragP.x, Me.dragP.y)
        Dim d1 As Double = Math.Sqrt((localPoint2.x - Me.downP.x) * (localPoint2.x - Me.downP.x) + (localPoint2.y - Me.downP.y) * (localPoint2.y - Me.downP.y))
        Dim i As Integer = (Integer)Math.Round(d1 /(Me.fixed_length * COS_30DEG)) 
		Dim d2 As Double = 0.5235987755982988D
        If (arrayOfDouble(1) > Me.mouseDragDraw2_prevTh)
		{
		  Me.sideFlag = True
		}
 Else
		{
		  d2 = -d2
		  Me.sideFlag = False
		}
		Me.startSideFlag = Me.sideFlag
        Me.mouseDragDraw2_n = i
        Me.mouseDragDraw2_dragP.x = Me.dragP.x
        Me.mouseDragDraw2_dragP.y = Me.dragP.y
        Me.mouseDragDraw2_tth = d2
        Me.mouseDragDraw2_depend = False
        Me.mouseDragDraw2_draw = True
		repaint()
		Me.mouseDragDraw2_prevTh = arrayOfDouble(1)
        Return True
	  }
 
	  Private Boolean mouseUpDraw_CarbonChain(java.awt.Event.MouseEvent paramMouseEvent, Integer paramInt1, Integer paramInt2) 
	  {
		If (Me.draggedFlag)
		{
		  localPoint1 = getRotateGrid(Me.upP, Me.downP, paramMouseEvent.ShiftDown)
		  Me.upP.x = localPoint1.x
        Me.upP.y = localPoint1.y
		}
		Dim localPoint1 As java.awt.Point = New java.awt.Point(Me.downP.x, Me.downP.y)
        Dim localPoint2 As java.awt.Point = New java.awt.Point(Me.upP.x, Me.upP.y)
        Dim d1 As Double = Math.Sqrt((localPoint2.x - Me.downP.x) * (localPoint2.x - Me.downP.x) + (localPoint2.y - Me.downP.y) * (localPoint2.y - Me.downP.y))
        Dim i As Integer = (Integer)Math.Round(d1 /(Me.fixed_length * COS_30DEG)) 
		Dim d2 As Double = 0.5235987755982988D
        If (Me.startSideFlag)
		{
		  Me.sideFlag = True
		}
 Else
		{
		  d2 = -d2
		  Me.sideFlag = False
		}
		mouseUpDraw2(i, paramInt1, paramInt2, d2, False)
		((keg.compound.Reaction)Me.conteiner).checkOverlapedBracketWithRefine()
		repaint()
		Me.editmode.flag_edit = True
        If (Me.parentK <> Nothing)
		{
		  Me.parentK.checkButton()
		}
		Me.editmode.status = 0
        Return True
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseUpDraw2(Integer paramInt1, Integer paramInt2, Integer paramInt3, Double paramDouble, Boolean paramBoolean) 
	  {
		Dim i As Integer = 1
        Dim j As Integer = 0
        Dim k As Integer = -1
        Dim localMolecule1 As keg.compound.Molecule = Nothing
        Dim d1 As Double = Me.downP.x
        Dim d2 As Double = Me.downP.y
        Dim d3 As Double = d1
        Dim d4 As Double = d2
        Dim d5 As Double = Me.upP.x
        Dim d6 As Double = Me.upP.y
        Dim m As Integer
        For m = 0 To paramInt1 - 1 Step m + 1
        If (m > 0)
		  {
			paramDouble = 0.0D
			paramBoolean = True
		  }
		  If (m > 0)
		  {
			Select Case Me.editmode.draw
        Case 0

        Case 1

        Case 2

        Case 3

        Case 4

        Case 5

        Case 10

        Dim n As Integer = 1
        If ((n > 1) And (Not paramBoolean))
			  {
				n = 1
			  }
			  Select Case n
        Case 1

        Dim arrayOfDouble2() As Double = New Double(2) {}
        Dim arrayOfDouble3() As Double = New Double(2) {}
				arrayOfDouble2(0) = d5
				arrayOfDouble2(1) = d6
				arrayOfDouble3(0) = d3
				arrayOfDouble3(1) = d4
				Dim d8 As Double = arrayOfDouble3(0) - arrayOfDouble2(0)
        Dim d9 As Double = arrayOfDouble3(1) - arrayOfDouble2(1)
        Dim arrayOfDouble4() As Double = New Double(2) {}
        Dim arrayOfDouble5() As Double = New Double(2) {}
        Dim d10 As Double = Math.Sqrt(d8 * d8 + d9 * d9)
				arrayOfDouble2(0) += (-SIN_30DEG * d8 - COS_30DEG * d9) * Me.fixed_length / d10
				arrayOfDouble2(1) += (COS_30DEG * d8 - SIN_30DEG * d9) * Me.fixed_length / d10
				arrayOfDouble2(0) += (-SIN_30DEG * d8 + COS_30DEG * d9) * Me.fixed_length / d10
				arrayOfDouble2(1) += (-COS_30DEG * d8 - SIN_30DEG * d9) * Me.fixed_length / d10
				If (Me.sideFlag)
				{
				  d5 = arrayOfDouble4(0)
				  d6 = arrayOfDouble4(1)
				}
 Else
				{
				  d5 = arrayOfDouble5(0)
				  d6 = arrayOfDouble5(1)
				}
				Me.sideFlag = (Not Me.sideFlag)
        Exit For
        End Select
        Exit For
        Case 6

        Case 7

        Case 8

        Case 9

        Case 11

        Case 12

        Return False
        End Select
		  }
		  Dim localAtom1 As keg.compound.Atom = Me.editmode.atom
        If (localAtom1 = Nothing)
		  {
			localMolecule1 = New keg.compound.Molecule()
			localAtom1 = New keg.compound.Atom(localMolecule1, Me.downP.x / Me.dispscale, Me.downP.y / Me.dispscale, 0.0D, "")
			localMolecule1.addAtom(localAtom1)
			localMolecule1.set0point(New DblRect(0,0))
			((keg.compound.Reaction)Me.conteiner).addObject(localMolecule1, 0)
			localAtom1.select(Me.editmode)
		  }
 Else
		  {
			localMolecule1 = localAtom1.Mol
			k = ((keg.compound.Reaction)Me.conteiner).getCategory(((keg.compound.Reaction)Me.conteiner).getObjectNo(localMolecule1))
		  }
		  Dim localObject As Object
        If (Me.fixed_length > 0)
		  {
			Dim d7 As Double = Math.Sqrt((d1 - d5) * (d1 - d5) + (d2 - d6) * (d2 - d6))
			localObject = New Double() 
			{
				(d5 - d1) * Me.fixed_length / d7, (d6 - d2) * Me.fixed_length / d7
			}

			Dim arrayOfDouble1() As Double = rotete(paramDouble, (Double())localObject)
			d5 = d1 + arrayOfDouble1(0)
			d6 = d2 + arrayOfDouble1(1)
			paramInt2 = (Integer)Math.Round(d5)
			paramInt3 = (Integer)Math.Round(d6)
		  }
		  Dim localMolecule2 As keg.compound.Molecule = New keg.compound.Molecule()
        Dim localAtom2 As keg.compound.Atom = New keg.compound.Atom(localMolecule2, d5 / Me.dispscale, d6 / Me.dispscale, 0.0D, "")
		  localMolecule2.set0point(New DblRect(0,0))
		  localMolecule2.addAtom(localAtom2)
		  ((keg.compound.Reaction)Me.conteiner).addObject(localMolecule2, 0)
		  localAtom2.select(Me.editmode)
		  If (k < 0)
		  {
			k = Me.editmode.category
		  }
		  Select Case Me.editmode.draw
        Case 1 : 

			i = 2
			Exit For
        Case 2 : 

			i = 3
			Exit For
        Case 3 : 

			j = 1
			Exit For
        Case 4 : 

			j = 6
			Exit For
        Case 5 : 

			j = 4
		Exit For
        End Select
        If (localAtom1 <> localAtom2)
		  {
			localMolecule1.combineMol(localAtom1, localMolecule2, localAtom2, i, j, Me.dispscale, Me.editmode.draw < 6)
			localObject = localAtom1.getBond(localAtom2)
			If (localObject <> Nothing)
			{
			  ((keg.compound.Bond)localObject).select(Me.editmode)
			  Dim @bool As Boolean =  checkBondsNumber((keg.compound.Bond)localObject) 
			  If (Not @bool) 
			  {
				Me.parentK.doUndo()
        Return False
			  }
			}
			localAtom1.calcImplicitHydrogen()
			localAtom1.decisideHydrogenDraw()
			localAtom2.calcImplicitHydrogen()
			localAtom2.decisideHydrogenDraw()
		  }
		  d3 = d1
		  d4 = d2
		  d1 = d5
		  d2 = d6
		  Me.editmode.atom = localAtom2
        Next
        If (localMolecule1 <> Nothing)
		{
		  ((keg.compound.Reaction)Me.conteiner).setCategory(localMolecule1, k)
		  localMolecule1.setDBond()
		  ((keg.compound.Reaction)Me.conteiner).autoSetCategory(Me.dispscale)
		}
		Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseDragDraw(java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		Dim d As Double = 0.0D
        Dim localGraphics As java.awt.Graphics = Graphics
		localGraphics.Color = Me.foreC
		localGraphics.XORMode = Me.backC
		If ((Math.Abs(Me.dragP.x - Me.downP.x) < Me.tolerance) And (Math.Abs(Me.dragP.y - Me.downP.y) < Me.tolerance))
		{
		  Return False
		}
		Dim localAtom As keg.compound.Atom = Me.conteiner.nearAtom(Me.dragP.x, Me.dragP.y, Me.dispscale, Me.tolerance)
        If ((localAtom <> Nothing) And (ShrinkMode) And ((localAtom.Express_group) Or (Not localAtom.NonGroupedAtom)))
		{
		  localAtom = Nothing
		}
		If ((localAtom <> Me.editmode.atom) And (localAtom <> Me.editmode.near_object))
		{
		  If ((Me.editmode.near_object <> Nothing) And ((Me.editmode.near_object Is keg.compound.Atom)))
		  {
			paintAtomMark(Nothing, (keg.compound.Atom)Me.editmode.near_object)
		  }
		  If (localAtom <> Nothing)
		  {
			paintAtomMark(Nothing, localAtom)
		  }
		  Me.editmode.near_object = localAtom
		}
		If ((localAtom <> Nothing) And (localAtom <> Me.editmode.atom))
		{
		  Me.dragP.x = localAtom.DX(Me.dispscale)
        Me.dragP.y = localAtom.DY(Me.dispscale)
		}
 Else
		{
		  Dim localPoint As java.awt.Point = getRotateGrid(Me.dragP, Me.downP, paramMouseEvent.ShiftDown, True)
        Me.dragP.x = localPoInteger.x
        Me.dragP.y = localPoInteger.y
		}
		drawLine(localGraphics, Me.downP.x, Me.downP.y, Me.prevDragP.x, Me.prevDragP.y)
		drawLine(localGraphics, Me.downP.x, Me.downP.y, Me.dragP.x, Me.dragP.y)
		Dim i As Integer
        Dim j As Integer
        Select Case Me.editmode.draw
        Case 6 : 

		  i = Me.prevDragP.y - Me.downP.y + Me.prevDragP.x
		  j = Me.downP.x - Me.prevDragP.x + Me.prevDragP.y
		  fillRect(localGraphics, i - 2, j - 2, 5, 5)
		  i = Me.prevDragP.y - Me.downP.y + Me.downP.x
		  j = Me.downP.x - Me.prevDragP.x + Me.downP.y
		  fillRect(localGraphics, i - 2, j - 2, 5, 5)
		  i = Me.dragP.y - Me.downP.y + Me.dragP.x
		  j = Me.downP.x - Me.dragP.x + Me.dragP.y
		  fillRect(localGraphics, i - 2, j - 2, 5, 5)
		  i = Me.dragP.y - Me.downP.y + Me.downP.x
		  j = Me.downP.x - Me.dragP.x + Me.downP.y
		  fillRect(localGraphics, i - 2, j - 2, 5, 5)
		  Exit For
        Case 7 : 

		  i = (Integer)(-0.309017D * (Me.downP.x - Me.prevDragP.x) - 0.951065D * (Me.downP.y - Me.prevDragP.y)) + Me.prevDragP.x
		  j = (Integer)(0.951065D * (Me.downP.x - Me.prevDragP.x) + -0.309017D * (Me.downP.y - Me.prevDragP.y)) + Me.prevDragP.y
		  fillRect(localGraphics, i - 2, j - 2, 5, 5)
		  i = (Integer)((0.309017D * (Me.downP.x - Me.prevDragP.x) - 0.951065D * (Me.downP.y - Me.prevDragP.y)) * 1.618034D) + Me.prevDragP.x
		  j = (Integer)((0.951065D * (Me.downP.x - Me.prevDragP.x) + 0.309017D * (Me.downP.y - Me.prevDragP.y)) * 1.618034D) + Me.prevDragP.y
		  fillRect(localGraphics, i - 2, j - 2, 5, 5)
		  i = (Integer)(-0.309017D * (Me.prevDragP.x - Me.downP.x) - -0.951065D * (Me.prevDragP.y - Me.downP.y)) + Me.downP.x
		  j = (Integer)(-0.951065D * (Me.prevDragP.x - Me.downP.x) + -0.309017D * (Me.prevDragP.y - Me.downP.y)) + Me.downP.y
		  fillRect(localGraphics, i - 2, j - 2, 5, 5)
		  i = (Integer)(-0.309017D * (Me.downP.x - Me.dragP.x) - 0.951065D * (Me.downP.y - Me.dragP.y)) + Me.dragP.x
		  j = (Integer)(0.951065D * (Me.downP.x - Me.dragP.x) + -0.309017D * (Me.downP.y - Me.dragP.y)) + Me.dragP.y
		  fillRect(localGraphics, i - 2, j - 2, 5, 5)
		  i = (Integer)((0.309017D * (Me.downP.x - Me.dragP.x) - 0.951065D * (Me.downP.y - Me.dragP.y)) * 1.618034D) + Me.dragP.x
		  j = (Integer)((0.951065D * (Me.downP.x - Me.dragP.x) + 0.309017D * (Me.downP.y - Me.dragP.y)) * 1.618034D) + Me.dragP.y
		  fillRect(localGraphics, i - 2, j - 2, 5, 5)
		  i = (Integer)(-0.309017D * (Me.dragP.x - Me.downP.x) - -0.951065D * (Me.dragP.y - Me.downP.y)) + Me.downP.x
		  j = (Integer)(-0.951065D * (Me.dragP.x - Me.downP.x) + -0.309017D * (Me.dragP.y - Me.downP.y)) + Me.downP.y
		  fillRect(localGraphics, i - 2, j - 2, 5, 5)
		  Exit For
        Case 8

        Case 9

        Case 11

        Case 12 : 

		  i = (Integer)(-0.5D * (Me.downP.x - Me.prevDragP.x) - 0.866025D * (Me.downP.y - Me.prevDragP.y)) + Me.prevDragP.x
		  j = (Integer)(0.866025D * (Me.downP.x - Me.prevDragP.x) + -0.5D * (Me.downP.y - Me.prevDragP.y)) + Me.prevDragP.y
		  fillRect(localGraphics, i - 2, j - 2, 5, 5)
		  i = (Integer)(1.732051D * (Me.prevDragP.y - Me.downP.y)) + Me.prevDragP.x
		  j = (Integer)(1.732051D * (Me.downP.x - Me.prevDragP.x)) + Me.prevDragP.y
		  If ((Me.editmode.draw <> 11) And (Me.editmode.draw <> 12))
		  {
			fillRect(localGraphics, i - 2, j - 2, 5, 5)
		  }
		  i = (Integer)(1.732051D * (Me.prevDragP.y - Me.downP.y)) + Me.downP.x
		  j = (Integer)(1.732051D * (Me.downP.x - Me.prevDragP.x)) + Me.downP.y
		  fillRect(localGraphics, i - 2, j - 2, 5, 5)
		  i = (Integer)(-0.5D * (Me.prevDragP.x - Me.downP.x) - -0.866025D * (Me.prevDragP.y - Me.downP.y)) + Me.downP.x
		  j = (Integer)(-0.866025D * (Me.prevDragP.x - Me.downP.x) + -0.5D * (Me.prevDragP.y - Me.downP.y)) + Me.downP.y
		  fillRect(localGraphics, i - 2, j - 2, 5, 5)
		  i = (Integer)(-0.5D * (Me.downP.x - Me.dragP.x) - 0.866025D * (Me.downP.y - Me.dragP.y)) + Me.dragP.x
		  j = (Integer)(0.866025D * (Me.downP.x - Me.dragP.x) + -0.5D * (Me.downP.y - Me.dragP.y)) + Me.dragP.y
		  fillRect(localGraphics, i - 2, j - 2, 5, 5)
		  i = (Integer)(1.732051D * (Me.dragP.y - Me.downP.y)) + Me.dragP.x
		  j = (Integer)(1.732051D * (Me.downP.x - Me.dragP.x)) + Me.dragP.y
		  If ((Me.editmode.draw <> 11) And (Me.editmode.draw <> 12))
		  {
			fillRect(localGraphics, i - 2, j - 2, 5, 5)
		  }
		  i = (Integer)(1.732051D * (Me.dragP.y - Me.downP.y)) + Me.downP.x
		  j = (Integer)(1.732051D * (Me.downP.x - Me.dragP.x)) + Me.downP.y
		  fillRect(localGraphics, i - 2, j - 2, 5, 5)
		  i = (Integer)(-0.5D * (Me.dragP.x - Me.downP.x) - -0.866025D * (Me.dragP.y - Me.downP.y)) + Me.downP.x
		  j = (Integer)(-0.866025D * (Me.dragP.x - Me.downP.x) + -0.5D * (Me.dragP.y - Me.downP.y)) + Me.downP.y
		  fillRect(localGraphics, i - 2, j - 2, 5, 5)
	  Exit For
        End Select
        Return False
	  }
 
	  Private Static Double() atan2to2Points(Double paramDouble1, Double paramDouble2, Double paramDouble3, Double paramDouble4) 
	  {
		Dim arrayOfDouble() As Double = New Double(2) {}
        Dim d1 As Double = paramDouble1 - paramDouble3
        Dim d2 As Double = paramDouble2 - paramDouble4
		arrayOfDouble(0) = Math.Sqrt(d1 * d1 + d2 * d2)
		arrayOfDouble(1) = Math.Atan2(d2, d1)
		Return arrayOfDouble
	  }
 
	  Private Static Double() atan2to2Points(java.awt.Point paramPoint1, java.awt.Point paramPoint2) 
	  {
		Return atan2to2Points(paramPoint1.x, paramPoint1.y, paramPoint2.x, paramPoint2.y)
	  }
 
	  Private java.awt.Point getRotateGrid(java.awt.Point paramPoint1, java.awt.Point paramPoint2, Boolean paramBoolean) 
	  {
		Return getRotateGrid(paramPoint1, paramPoint2, paramBoolean, False)
	  }
 
	  Private java.awt.Point getRotateGrid(java.awt.Point paramPoint1, java.awt.Point paramPoint2, Boolean paramBoolean1, Boolean paramBoolean2) 
	  {
		Dim arrayOfDouble() As Double = getRotateGrid_Inner(paramPoint1, paramPoint2, paramBoolean1, paramBoolean2)
        Dim i As Integer = (Integer)Math.Round(arrayOfDouble(0) * Dispscale) 
		Dim j As Integer = (Integer)Math.Round(arrayOfDouble(1) * Dispscale) 
		Return New java.awt.Point(i, j)
	  }
 
	  Private Double() getRotateGrid_Inner(java.awt.Point paramPoint1, java.awt.Point paramPoint2, Boolean paramBoolean1, Boolean paramBoolean2) 
	  {
		Dim arrayOfDouble1() As Double = New Double(2) {}
        Dim arrayOfDouble2() As Double = New Double(2) {}
        Dim localAtom As keg.compound.Atom = ((keg.compound.Reaction)Me.conteiner).nearAtom(paramPoint2.x, paramPoint2.y,Dispscale,Me.tolerance) 
		If (localAtom <> Nothing)
		{
		  arrayOfDouble1(0) = localAtom.x
		  arrayOfDouble1(1) = localAtom.y
		}
 Else
		{
		  arrayOfDouble1(0) = (paramPoint2.x / Dispscale)
		  arrayOfDouble1(1) = (paramPoint2.y / Dispscale)
		}
		arrayOfDouble2(0) = (paramPoint1.x / Dispscale)
		arrayOfDouble2(1) = (paramPoint1.y / Dispscale)
		Dim d1 As Double = IIf(paramBoolean1, 4D, 24D)

        Dim arrayOfDouble3() As Double = atan2to2Points(arrayOfDouble2(0), arrayOfDouble2(1), arrayOfDouble1(0), arrayOfDouble1(1))
        Dim d2 As Double = arrayOfDouble3(1)
        Dim d3 As Double = 0.0D
        Dim d4 As Double = 6.283185307179586D / d1
        Dim i As Integer = 1
        If (i = 1)
		{
		  d5 = -3.141592653589793D - d4 / 2.0D
		  Dim j As Integer
        For j = 0 To d1 + 1.0D- 1  Step j + 1
        If ((d5 + d4 * j < d2) And (d2 < d5 + d4 * (j + 1)))
			{
			  d3 = -3.141592653589793D + d4 * j
			  Exit For
			}
		  Next
		}
		Dim d5 As Double = paramBoolean2 ? Me.fixed_length / Dispscale : arrayOfDouble3(0) 

		Dim d6 As Double = d5 * Math.Cos(d3)
        Dim d7 As Double = d5 * Math.Sin(d3)
		d6 = (0.0D < d6) And (d6 < 1.0E-10D) ? 0.0D : d6

		d6 = (-1.0E-10D < d6) And (d6 < 0.0D) ? 0.0D  d6

		d7 = (0.0D < d7) And (d7 < 1.0E-10D) ? 0.0D  d7

		d7 = (-1.0E-10D < d7) And (d7 < 0.0D) ? 0.0D  d7

		Return New Double()
		{
			d6 + arrayOfDouble1(0), d7 + arrayOfDouble1(1)
		}

	  }
 
	  Private Double normalizeTheta(Double paramDouble) 
	  {
		paramDouble = paramDouble > 3.141592653589793D ? paramDouble - 6.283185307179586D : paramDouble

		paramDouble = paramDouble < -3.141592653589793D ? paramDouble + 6.283185307179586D  paramDouble

		Return paramDouble
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseDragBracket() 
	  {
		Dim localGraphics As java.awt.Graphics = Graphics
		localGraphics.Color = Me.bC
		localGraphics.XORMode = Me.backC
		Dim i As Integer = Math.Min(Me.downP.x, Me.prevDragP.x)
        Dim j As Integer = Math.Min(Me.downP.y, Me.prevDragP.y)
        Dim k As Integer = Math.Abs(Me.prevDragP.x - Me.downP.x)
        Dim m As Integer = Math.Abs(Me.prevDragP.y - Me.downP.y)
		drawRect(localGraphics, i, j, k, m)
		i = Math.Min(Me.downP.x, Me.dragP.x)
		j = Math.Min(Me.downP.y, Me.dragP.y)
		k = Math.Abs(Me.dragP.x - Me.downP.x)
		m = Math.Abs(Me.dragP.y - Me.downP.y)
		drawRect(localGraphics, i, j, k, m)
		Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseDragErase() 
	  {
		Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseDragMove() 
	  {
		If (Me.selectFlagWhenChangeFromDownToDrag2)
		{
		  Me.editmode.UNDO = (keg.compound.Reaction)Me.conteiner
		  Me.selectFlagWhenChangeFromDownToDrag2 = False
		  localObject1 = Size
		  Me.graphicsForMove = Nothing
        Me.imageForMove = Nothing
		  System.gc()
		  Me.imageForMove = createImage(((DblRect)localObject1).width, ((DblRect)localObject1).height)
		  Me.graphicsForMove = Me.imageForMove.Graphics
		  paint(Me.graphicsForMove)
		}
		Dim localObject1 As Object = Graphics
        If (Me.imageForMove <> Nothing)
		{
		  ((java.awt.Graphics)localObject1).drawImage(Me.imageForMove, 0, 0, Color.white, Me)
		}
 Else
		{
		  Console.Error.WriteLine("imageForMove = null")
		}
		Dim localColor As Color = Me.foreC
        Dim j As Integer = Me.dragP.x - Me.downP.x
        Dim k As Integer = Me.dragP.y - Me.downP.y
		((java.awt.Graphics)localObject1).Color = Me.select_frameC
		((java.awt.Graphics)localObject1).XORMode = Me.backC
		drawRect((java.awt.Graphics)localObject1, Me.editmode.select_area.x - Me.tolerance + j, Me.editmode.select_area.y - Me.tolerance + k, Me.editmode.select_area.width + Me.tolerance * 2 + 1 + Me.handlesize, Me.editmode.select_area.height + Me.tolerance * 2)
		fillRect((java.awt.Graphics)localObject1, Me.editmode.select_area.x + Me.editmode.select_area.width + Me.tolerance + j + 1, Me.editmode.select_area.y - Me.tolerance + k + 1, Me.handlesize, Me.handlesize)
		fillRect((java.awt.Graphics)localObject1, Me.editmode.select_area.x + Me.editmode.select_area.width + Me.tolerance + j + 1, Me.editmode.select_area.y + Me.editmode.select_area.height + Me.tolerance + k - Me.handlesize, Me.handlesize, Me.handlesize)
		Dim m As Integer
        For m = 0 To Me.editmode.selected.Count - 1 Step m + 1
        Dim localObject2 As Object = Me.editmode.selected(m)
        Dim localAtom1 As keg.compound.Atom
        If ((localObject2 Is keg.compound.Atom))
		  {
			((java.awt.Graphics)localObject1).XORMode = Me.backC
			Dim localAtom4 As keg.compound.Atom
        Dim n As Integer
        For n = 0 To ((keg.compound.Atom)localObject2).numBond()- 1  Step n + 1
        Dim localBond As keg.compound.Bond = ((keg.compound.Atom)localObject2).getBond(n) 
			  If (Not localBond.Select)
			  {
				((java.awt.Graphics)localObject1).XORMode = Me.backC
				Dim i As Integer = ((keg.compound.Reaction)Me.conteiner).getObjectNo(((keg.compound.Atom)localObject2).Mol) 
				If (((keg.compound.Reaction)Me.conteiner).isReactant(i)) 
				{
				  localColor = Me.reactantC
				}
 ElseIf (((keg.compound.Reaction)Me.conteiner).isProduct(i)) 
				{
				  localColor = Me.productC
				}
 Else
				{
				  localColor = Me.foreC
				}
				Dim localAtom2 As keg.compound.Atom = (keg.compound.Atom)localObject2 
				localAtom4 = localBond.pairAtom(localAtom2)
				If ((localAtom2.NonGroupedAtom) And (localAtom4.NonGroupedAtom))
				{
				  drawLine((java.awt.Graphics)localObject1, localAtom2.DX(Me.dispscale) + j, localAtom2.DY(Me.dispscale) + k, localAtom4.DX(Me.dispscale), localAtom4.DY(Me.dispscale))
				}
			  }
			Next
			localAtom1 = (keg.compound.Atom)localObject2
			If (localAtom1.Express_group)
			{
			  Dim i1 As Integer
        For i1 = 0 To localAtom1.GroupPartnerSize - 1 Step i1 + 1
				localAtom4 = localAtom1.getGroupPartner(i1)
				If (Not localAtom4.Select)
				{
				  drawLine((java.awt.Graphics)localObject1, localAtom1.DX(Me.dispscale) + j, localAtom1.DY(Me.dispscale) + k, localAtom4.DX(Me.dispscale), localAtom4.DY(Me.dispscale))
				}
			  Next
			}
		  }
 ElseIf ((localObject2 Is keg.compound.Bond))
		  {
			((java.awt.Graphics)localObject1).XORMode = Me.backC
			((java.awt.Graphics)localObject1).Color = Me.highC
			localAtom1 = ((keg.compound.Bond)localObject2).Atom1
			Dim localAtom3 As keg.compound.Atom = ((keg.compound.Bond)localObject2).Atom2 
			drawLine((java.awt.Graphics)localObject1, localAtom1.DX(Me.dispscale) + j, localAtom1.DY(Me.dispscale) + k, localAtom3.DX(Me.dispscale) + j, localAtom3.DY(Me.dispscale) + k)
		  }
 ElseIf ((localObject2 Is keg.compound.Bracket))
		  {
			((java.awt.Graphics)localObject1).XORMode = Me.backC
			paintBracket((java.awt.Graphics)localObject1, (keg.compound.Bracket)localObject2, j, k)
		  }
		Next
        Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseDragSelect() 
	  {
		Dim localGraphics As java.awt.Graphics = Graphics
		localGraphics.Color = Me.select_frameC
		localGraphics.setPaintMode()
		If ((Me.editmode.select_mode = 0) Or (Me.editmode.operation = 8))
		{
		  Dim localDimension As DblRect = (DblRect)Me.lasso_points(Me.lasso_points.Count - 1) 
		  drawLine(localGraphics, localDimension.width, localDimension.height, Me.dragP.x, Me.dragP.y)
		  Me.lasso_points.Add(New DblRect(Me.dragP.x, Me.dragP.y))
		}
 Else
		{
		  localGraphics.XORMode = Me.backC
		  drawRect(localGraphics, Math.Min(Me.downP.x, Me.prevDragP.x), Math.Min(Me.downP.y, Me.prevDragP.y), Math.Abs(Me.downP.x - Me.prevDragP.x), Math.Abs(Me.downP.y - Me.prevDragP.y))
		  drawRect(localGraphics, Math.Min(Me.downP.x, Me.dragP.x), Math.Min(Me.downP.y, Me.dragP.y), Math.Abs(Me.dragP.x - Me.downP.x), Math.Abs(Me.dragP.y - Me.downP.y))
		}
		Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseDragRotate() 
	  {
		If ((Me.editmode.operation = 8) And (Me.rfr.Selected))
		{
		  Me.cx = Me.dragP.x
        Me.cy = Me.dragP.y
        Me.rfr.x = Me.cx
        Me.rfr.y = Me.cy
		  repaint()
		  Return False
		}
		Me.rfr.unselected()
        Dim localGraphics As java.awt.Graphics = Graphics
		localGraphics.Color = Me.select_frameC
		localGraphics.XORMode = Me.backC
		If ((Me.rx1 = 0) And (Me.ry1 = 0))
		{
		  drawRect(localGraphics, Me.editmode.select_area.x - Me.tolerance, Me.editmode.select_area.y - Me.tolerance, Me.editmode.select_area.width + Me.tolerance * 2 + 1 + Me.handlesize, Me.editmode.select_area.height + Me.tolerance * 2)
		  If (Me.editmode.operation <> 8)
		  {
			fillRect(localGraphics, Me.editmode.select_area.x + Me.editmode.select_area.width + Me.tolerance + 1, Me.editmode.select_area.y - Me.tolerance + 1, Me.handlesize, Me.handlesize)
			fillRect(localGraphics, Me.editmode.select_area.x + Me.editmode.select_area.width + Me.tolerance + 1, Me.editmode.select_area.y + Me.editmode.select_area.height + Me.tolerance - Me.handlesize, Me.handlesize, Me.handlesize)
		  }
		}
 Else
		{
		  drawLine(localGraphics, Me.rx1, Me.ry1, Me.rx2, Me.ry2)
		  drawLine(localGraphics, Me.rx2, Me.ry2, Me.rx3, Me.ry3)
		  drawLine(localGraphics, Me.rx3, Me.ry3, Me.rx4, Me.ry4)
		  drawLine(localGraphics, Me.rx4, Me.ry4, Me.rx1, Me.ry1)
		}
		Dim localVector2D As Vector2D = New Vector2D(Me.dragP.x - Me.cx, Me.dragP.y - Me.cy)
        Dim d1 As Double = VecMath2D.angle(Me.vec0, localVector2D)
        Dim * As d1 =  -1.0D 
		Dim d2 As Double = Math.Cos(d1)
        Dim d3 As Double = Math.Sin(d1)
        Dim d4 As Double = Me.editmode.select_area.x - Me.tolerance - Me.cx
        Dim d5 As Double = Me.editmode.select_area.y - Me.tolerance - Me.cy
        Me.rx1 = ((Integer)(d2 * d4 - d3 * d5) + Me.cx)
        Me.ry1 = ((Integer)(d3 * d4 + d2 * d5) + Me.cy)
		d4 = Me.editmode.select_area.x - Me.tolerance - Me.cx
		d4 += Me.editmode.select_area.width + Me.tolerance * 2 + 1 + Me.handlesize
		d5 = Me.editmode.select_area.y - Me.tolerance - Me.cy
		Me.rx2 = ((Integer)(d2 * d4 - d3 * d5) + Me.cx)
        Me.ry2 = ((Integer)(d3 * d4 + d2 * d5) + Me.cy)
		d4 = Me.editmode.select_area.x - Me.tolerance - Me.cx
		d4 += Me.editmode.select_area.width + Me.tolerance * 2 + 1 + Me.handlesize
		d5 = Me.editmode.select_area.y - Me.tolerance - Me.cy
		d5 += Me.editmode.select_area.height + Me.tolerance * 2
		Me.rx3 = ((Integer)(d2 * d4 - d3 * d5) + Me.cx)
        Me.ry3 = ((Integer)(d3 * d4 + d2 * d5) + Me.cy)
		d4 = Me.editmode.select_area.x - Me.tolerance - Me.cx
		d5 = Me.editmode.select_area.y - Me.tolerance - Me.cy
		d5 += Me.editmode.select_area.height + Me.tolerance * 2
		Me.rx4 = ((Integer)(d2 * d4 - d3 * d5) + Me.cx)
        Me.ry4 = ((Integer)(d3 * d4 + d2 * d5) + Me.cy)
		drawLine(localGraphics, Me.rx1, Me.ry1, Me.rx2, Me.ry2)
		drawLine(localGraphics, Me.rx2, Me.ry2, Me.rx3, Me.ry3)
		drawLine(localGraphics, Me.rx3, Me.ry3, Me.rx4, Me.ry4)
		drawLine(localGraphics, Me.rx4, Me.ry4, Me.rx1, Me.ry1)
		Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseDragResize() 
	  {
		Dim localGraphics As java.awt.Graphics = Graphics
		localGraphics.Color = Me.select_frameC
		localGraphics.XORMode = Me.backC
		Dim i As Integer = Me.editmode.select_area.width + Me.tolerance * 2 + (Me.prevDragP.x - Me.downP.x) + 1 + Me.handlesize
        Dim j As Integer = Me.editmode.select_area.height + Me.tolerance * 2 + (Me.prevDragP.y - Me.downP.y)
        If (i < 1 + Me.handlesize)
		{
		  i = 1 + Me.handlesize
		}
		If (j < 1 + Me.handlesize * 2)
		{
		  j = 1 + Me.handlesize * 2
		}
		drawRect(localGraphics, Me.editmode.select_area.x - Me.tolerance, Me.editmode.select_area.y - Me.tolerance, i, j)
		fillRect(localGraphics, Me.editmode.select_area.x + i - Me.tolerance - Me.handlesize, Me.editmode.select_area.y - Me.tolerance + 1, Me.handlesize, Me.handlesize)
		fillRect(localGraphics, Me.editmode.select_area.x + i - Me.tolerance - Me.handlesize, Me.editmode.select_area.y + j - Me.tolerance - Me.handlesize, Me.handlesize, Me.handlesize)
		i = Me.editmode.select_area.width + Me.tolerance * 2 + (Me.dragP.x - Me.downP.x) + 1 + Me.handlesize
		j = Me.editmode.select_area.height + Me.tolerance * 2 + (Me.dragP.y - Me.downP.y)
		If (i < 1 + Me.handlesize)
		{
		  i = 1 + Me.handlesize
		}
		If (j < 1 + Me.handlesize * 2)
		{
		  j = 1 + Me.handlesize * 2
		}
		drawRect(localGraphics, Me.editmode.select_area.x - Me.tolerance, Me.editmode.select_area.y - Me.tolerance, i, j)
		fillRect(localGraphics, Me.editmode.select_area.x + i - Me.tolerance - Me.handlesize, Me.editmode.select_area.y - Me.tolerance + 1, Me.handlesize, Me.handlesize)
		fillRect(localGraphics, Me.editmode.select_area.x + i - Me.tolerance - Me.handlesize, Me.editmode.select_area.y + j - Me.tolerance - Me.handlesize, Me.handlesize, Me.handlesize)
		Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseDragArrow() 
	  {
		Dim localGraphics As java.awt.Graphics = Graphics
		localGraphics.Color = Me.foreC
		localGraphics.XORMode = Me.backC
		Dim i As Integer
        Dim j As Integer
        If (Math.Abs(Me.prevDragP.x - Me.downP.x) < Math.Abs(Me.prevDragP.y - Me.downP.y))
		{
		  i = Me.downP.x
		  j = Me.prevDragP.y
		}
 Else
		{
		  i = Me.prevDragP.x
		  j = Me.downP.y
		}
		drawLine(localGraphics, Me.downP.x, Me.downP.y, i, j)
		If (Math.Abs(Me.dragP.x - Me.downP.x) < Math.Abs(Me.dragP.y - Me.downP.y))
		{
		  i = Me.downP.x
		  j = Me.dragP.y
		}
 Else
		{
		  i = Me.dragP.x
		  j = Me.downP.y
		}
		drawLine(localGraphics, Me.downP.x, Me.downP.y, i, j)
		Return False
	  }
 
	  Public virtual Boolean mouseExit(java.awt.Event.MouseEvent paramMouseEvent, Integer paramInt1, Integer paramInt2) 
	  {
		Return mouseUp(paramMouseEvent, paramInt1, paramInt2)
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        Private Boolean draw_SingleBond(keg.compound.EditMode paramEditMode) 
	  {
		paramEditMode.bond.Order = 1
		paramEditMode.bond.Stereo = 0
		paramEditMode.bond.calcImplicitHydrogen()
		paramEditMode.bond.decisideHydrogenDraw()
		Return True
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        Private Boolean draw_DoubleBond(keg.compound.EditMode paramEditMode) 
	  {
		If (paramEditMode.bond.Order = 2)
		{
		  Select Case paramEditMode.bond.Orientation
        Case 1

        Case 11 : 

			paramEditMode.bond.Orientation = 12
			Exit For
        Case 2

        Case 12 : 

			paramEditMode.bond.Orientation = 13
			Exit For
        Case 3

        Case 13 : 

			paramEditMode.bond.Orientation = 11
		Exit For
        End Select
		}
 Else
		{
		  paramEditMode.bond.Order = 2
		  paramEditMode.bond.Stereo = 0
		  paramEditMode.bond.Mol.setDBond()
		}
		paramEditMode.bond.calcImplicitHydrogen()
		paramEditMode.bond.decisideHydrogenDraw()
		Return True
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        Private Boolean draw_TripleBond(keg.compound.EditMode paramEditMode) 
	  {
		paramEditMode.bond.Order = 3
		paramEditMode.bond.Stereo = 0
		paramEditMode.bond.calcImplicitHydrogen()
		paramEditMode.bond.decisideHydrogenDraw()
		Return True
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseUpDraw(java.awt.Event.MouseEvent paramMouseEvent, Integer paramInt1, Integer paramInt2) 
	  {
		Dim localAtom2 As keg.compound.Atom = Nothing
        Dim i As Integer = 1
        Dim j As Integer = 1
        Dim k As Integer = 0
        Dim m As Integer = -1
        Dim str As String = Nothing
        If (((Math.Abs(Me.upP.x - Me.downP.x) < Me.tolerance) And (Math.Abs(Me.upP.y - Me.downP.y) < Me.tolerance)) Or ((Math.Abs(Me.upP.x - Me.downP.x) < Me.tolerance * 2) And (Math.Abs(Me.upP.y - Me.downP.y) < Me.tolerance * 2) And (Me.conteiner.nearAtom(Me.upP.x, Me.upP.y, Me.dispscale, Me.tolerance) <> Nothing)))
		{
		  If (Me.draggedFlag)
		  {
			Dim localPoint As java.awt.Point = getRotateGrid(Me.upP, Me.downP, paramMouseEvent.ShiftDown, True)
        Me.upP.x = localPoInteger.x
        Me.upP.y = localPoInteger.y
		  }
		  Return mouseClickDraw(paramMouseEvent)
		}
		Dim n As Integer = 2
        Dim d9 As Double = Me.fixed_length / Me.dispscale
        Dim localMolecule3 As keg.compound.Molecule
        If (Me.editmode.bond <> Nothing)
		{
		  Me.editmode.bond.Recalc = True
        Select Case Me.editmode.draw
        Case 0 : 

			draw_SingleBond(Me.editmode)
			repaint()
			Return True
        Case 1 : 

			draw_DoubleBond(Me.editmode)
			repaint()
			Return True
        Case 2 : 

			draw_TripleBond(Me.editmode)
			repaint()
			Return True
        Case 3

        Me.editmode.bond.Order = 1
        Me.editmode.bond.Stereo = 1
			localObject1 = Me.editmode.bond.nearSide(Me.upP.x, Me.upP.y, Me.dispscale)
			Me.editmode.bond.swapAtom()
        Me.editmode.bond.calcImplicitHydrogen()
        Me.editmode.bond.decisideHydrogenDraw()
			repaint()
			Return True
        Case 4

        Me.editmode.bond.Order = 1
        Me.editmode.bond.Stereo = 6
			localObject1 = Me.editmode.bond.nearSide(Me.upP.x, Me.upP.y, Me.dispscale)
			Me.editmode.bond.swapAtom()
        Me.editmode.bond.calcImplicitHydrogen()
        Me.editmode.bond.decisideHydrogenDraw()
			repaint()
			Return True
        Case 5

        Me.editmode.bond.Order = 1
        Me.editmode.bond.Stereo = 4
        Me.editmode.bond.swapAtom()
        Me.editmode.bond.calcImplicitHydrogen()
        Me.editmode.bond.decisideHydrogenDraw()
			repaint()
			Return True
        Case 6

        Case 7

        Case 8

        Case 9

        Case 11

        Case 12

        If (keg.compound.Molecule.whichSide(Me.editmode.bond))
			{
			  Me.editmode.atom = Me.editmode.bond.Atom1
			  localAtom2 = Me.editmode.bond.Atom2
			}
 Else
			{
			  Me.editmode.atom = Me.editmode.bond.Atom2
			  localAtom2 = Me.editmode.bond.Atom1
			}
			If ((Me.editmode.draw = 11) Or (Me.editmode.draw = 12))
			{
			  n = getLocationOn5BaseRing(Me.editmode.bond, Me.editmode.draw)
			  If (n = 5)
			  {
				If (Not keg.compound.Molecule.whichSide(Me.editmode.bond))
				{
				  Me.editmode.atom = Me.editmode.bond.Atom1
				  localAtom2 = Me.editmode.bond.Atom2
				}
 Else
				{
				  Me.editmode.atom = Me.editmode.bond.Atom2
				  localAtom2 = Me.editmode.bond.Atom1
				}
				d5 = Me.editmode.atom.x
				d6 = Me.editmode.atom.y
				d7 = localAtom2.x
				d8 = localAtom2.y
				Dim d10 As Double = Math.Atan2(d8 - d6, d7 - d5)
				d10 += 1.5707963267948966D
				d3 = d9 * Math.Cos(d10) + d5
				d4 = d9 * Math.Sin(d10) + d6
				localMolecule3 = New keg.compound.Molecule()
				localAtom2 = New keg.compound.Atom(localMolecule3, d3, d4, 0.0D, "")
				localMolecule3.set0point(New DblRect(0,0))
				localMolecule3.addAtom(localAtom2)
				((keg.compound.Reaction)Me.conteiner).addObject(localMolecule3, 0)
				localAtom2.select(Me.editmode)
				n = 4
			  }
			}
			Exit For
        End Select
		}
		str = ""
		Dim localAtom1 As keg.compound.Atom = Me.editmode.atom
        Dim localMolecule2 As keg.compound.Molecule
        If (localAtom1 = Nothing)
		{
		  localMolecule2 = New keg.compound.Molecule()
		  localAtom1 = New keg.compound.Atom(localMolecule2, Me.downP.x / Me.dispscale, Me.downP.y / Me.dispscale, 0.0D, str)
		  localMolecule2.addAtom(localAtom1)
		  localMolecule2.set0point(New DblRect(0,0))
		  ((keg.compound.Reaction)Me.conteiner).addObject(localMolecule2, 0)
		  localAtom1.select(Me.editmode)
		}
 Else
		{
		  localAtom1.unselect(Me.editmode)
		  localMolecule2 = localAtom1.Mol
		  m = ((keg.compound.Reaction)Me.conteiner).getCategory(((keg.compound.Reaction)Me.conteiner).getObjectNo(localMolecule2))
		}
		If (localAtom2 = Nothing)
		{
		  localAtom2 = Me.conteiner.nearAtom(Me.upP.x, Me.upP.y, Me.dispscale, Me.tolerance)
		}
		Dim localObject2 As Object
        If (localAtom2 = Nothing)
		{
		  localObject2 = Nothing
		  If (Me.draggedFlag)
		  {
			Dim localAtom3 As keg.compound.Atom = ((keg.compound.Reaction)Me.conteiner).nearAtom(Me.downP.x, Me.downP.y, Dispscale, Me.tolerance) 
			If (localAtom3 <> Nothing)
			{
			  Me.downP.x = localAtom3.DX(Dispscale)
        Me.downP.y = localAtom3.DY(Dispscale)
			}
			localObject2 = getRotateGrid_Inner(Me.upP, Me.downP, paramMouseEvent.ShiftDown, True)
			Me.upP.x = ((Integer)Math.Round(localObject2(0) * Dispscale))
			Me.upP.y = ((Integer)Math.Round(localObject2(1) * Dispscale))
		  }
 ElseIf (Me.fixed_length > 0)
		  {
			Dim d11 As Double = Math.Sqrt((Me.downP.x - Me.upP.x) * (Me.downP.x - Me.upP.x) + (Me.downP.y - Me.upP.y) * (Me.downP.y - Me.upP.y))
        Me.upP.x = (Me.downP.x + (Integer)Math.Round((Me.upP.x - Me.downP.x) * Me.fixed_length / d11))
			Me.upP.y = (Me.downP.y + (Integer)Math.Round((Me.upP.y - Me.downP.y) * Me.fixed_length / d11))
			localObject2 = New Double(2) {}
			localObject2(0) = (Me.upP.x / Dispscale)
			localObject2(1) = (Me.upP.y / Dispscale)
		  }
		  localAtom2 = Me.conteiner.nearAtom(Me.upP.x, Me.upP.y, Me.dispscale, Me.tolerance)
		  If (localAtom2 = Nothing)
		  {
			localMolecule3 = New keg.compound.Molecule()
			localAtom2 = New keg.compound.Atom(localMolecule3, localObject2(0), localObject2(1), 0.0D, str)
			localMolecule3.addAtom(localAtom2)
			localMolecule3.set0point(New DblRect(0,0))
			((keg.compound.Reaction)Me.conteiner).addObject(localMolecule3, 0)
			localAtom2.select(Me.editmode)
		  }
 Else
		  {
			localMolecule3 = localAtom2.Mol
		  }
		}
 Else
		{
		  localMolecule3 = localAtom2.Mol
		  If (m < 0)
		  {
			m = ((keg.compound.Reaction)Me.conteiner).getCategory(((keg.compound.Reaction)Me.conteiner).getObjectNo(localMolecule3))
		  }
		}
		If ((localAtom1.Express_group) Or (localAtom2.Express_group))
		{
		  Return False
		}
		If (m < 0)
		{
		  m = Me.editmode.category
		}
		Select Case Me.editmode.draw
        Case 1 : 

		  j = 2
		  Exit For
        Case 2 : 

		  j = 3
		  Exit For
        Case 3 : 

		  k = 1
		  Exit For
        Case 4 : 

		  k = 6
		  Exit For
        Case 5 : 

		  k = 4
		  Exit For
        Case 6 : 

		  i = 4
		  Exit For
        Case 7 : 

		  i = 5
		  Exit For
        Case 8

        Case 11

        Case 12 : 

		  i = 6
		  Exit For
        Case 9

        If ((localAtom1.getsp3()) And (localAtom2.getsp3()))
		  {
			j = 2
		  }
		  i = 6
	  Exit For
        End Select
        If (localAtom1 <> localAtom2)
		{
		  If (((localAtom1.NonGroupedAtom) And (Not localAtom2.NonGroupedAtom)) Or ((Not localAtom1.NonGroupedAtom) And (localAtom2.NonGroupedAtom)))
		  {
			localObject2 = IIf( localAtom1.NonGroupedAtom ,  localAtom2 ,  localAtom1)

			Dim localAtom4 As keg.compound.Atom = localObject2 = IIf(localAtom1, localAtom2, localAtom1)

        Dim localMolecule4 As keg.compound.Molecule = ((keg.compound.Atom)localObject2).Mol 
			Dim i2 As Integer
        For i2 = 0 To localMolecule4.AtomNum - 1 Step i2 + 1
        Dim localAtom5 As keg.compound.Atom = localMolecule4.getAtom(i2 + 1)
        If (localAtom5.Express_group)
			  {
				localAtom5.addGroupPartner(localAtom4)
				Exit For
			  }
			Next
		  }
		  localMolecule2.combineMol(localAtom1, localMolecule3, localAtom2, j, k, Me.dispscale, Me.editmode.draw < 6)
		  localObject2 = localAtom1.getBond(localAtom2)
		  If (localObject2 <> Nothing)
		  {
			((keg.compound.Bond)localObject2).select(Me.editmode)
			Dim bool1 As Boolean = checkBondsNumber((keg.compound.Bond)localObject2)
        If (Not bool1)
			{
			  Me.parentK.doUndo()
        Return False
			}
		  }
		  localAtom1.calcImplicitHydrogen()
		  localAtom1.decisideHydrogenDraw()
		  localAtom2.calcImplicitHydrogen()
		  localAtom2.decisideHydrogenDraw()
		}
		((keg.compound.Reaction)Me.conteiner).setCategory(localMolecule2, m)
		Dim d5 As Double = localAtom1.x
        Dim d6 As Double = localAtom1.y
        Dim d7 As Double = localAtom2.x
        Dim d8 As Double = localAtom2.y
        Dim d1 As Double = d7
        Dim d2 As Double = d8
        Dim d3 As Double = d7
        Dim d4 As Double = d8
        Dim localMolecule1 As keg.compound.Molecule = localMolecule2
        Dim localObject1 As Object = localAtom1
        If ((n = 4) Or (n = 1))
		{
		  If ((Me.editmode.draw = 11) And (d6 < d8))
		  {
			n = 4
		  }
		  If ((Me.editmode.draw = 11) And (d6 > d8))
		  {
			n = 1
		  }
		  If ((Me.editmode.draw = 12) And (d6 < d8))
		  {
			n = 1
		  }
		  If ((Me.editmode.draw = 12) And (d6 > d8))
		  {
			n = 4
		  }
		}
		Dim i1 As Integer
        For i1 = 1 To i - 1 - 1 Step i1 + 1
        If ((Me.editmode.draw = 11) Or (Me.editmode.draw = 12))
		  {
			i1 = i1 = n ? i1 + 1 : i1

			If (i1 >= i - 1)
			{
			}
		  }
 Else
		  {
			localMolecule2 = localMolecule3
			localAtom1 = localAtom2
			Select Case Me.editmode.draw
        Case 6

        Select Case i1
        Case 1 : 

				d3 = d8 - d6 + d7
				d4 = d5 - d7 + d8
				Exit For
        Case 2 : 

				d3 = d8 - d6 + d5
				d4 = d5 - d7 + d6
			Exit For
        End Select
        Exit For
        Case 7

        Select Case i1
        Case 1 : 

				d3 = -0.309017D * (d5 - d7) - 0.951065D * (d6 - d8) + d7
				d4 = 0.951065D * (d5 - d7) + -0.309017D * (d6 - d8) + d8
				Exit For
        Case 2 : 

				d3 = (0.309017D * (d5 - d7) - 0.951065D * (d6 - d8)) * 1.618034D + d7
				d4 = (0.951065D * (d5 - d7) + 0.309017D * (d6 - d8)) * 1.618034D + d8
				Exit For
        Case 3 : 

				d3 = -0.309017D * (d7 - d5) - -0.951065D * (d8 - d6) + d5
				d4 = -0.951065D * (d7 - d5) + -0.309017D * (d8 - d6) + d6
			Exit For
        End Select
        Exit For
        Case 8

        Case 9

        Case 11

        Case 12

        Dim d12 As Double = d5 - d7
        Dim d13 As Double = d6 - d8
        Dim d14 As Double = Math.Atan2(d13, d12)
			  d12 = d9 * Math.Cos(d14)
			  d13 = d9 * Math.Sin(d14)
			  Select Case i1
        Case 1 : 

				d3 = -SIN_30DEG * d12 - COS_30DEG * d13 + d7
				d4 = COS_30DEG * d12 + -SIN_30DEG * d13 + d8
				j = 1
				Exit For
        Case 2 : 

				d3 = TAN_60DEG * -d13 + d7
				d4 = TAN_60DEG * d12 + d8
				j = 1
				Exit For
        Case 3 : 

				d3 = TAN_60DEG * -d13 + d5
				d4 = TAN_60DEG * d12 + d6
				j = 1
				Exit For
        Case 4 : 

				d3 = -SIN_30DEG * -d12 - -COS_30DEG * -d13 + d5
				d4 = -COS_30DEG * -d12 + -SIN_30DEG * -d13 + d6
				j = 1
			Exit For
        End Select
        Exit For
        End Select
			localAtom2 = Me.conteiner.nearAtom((Integer)Math.Round(d3 * Me.dispscale), (Integer)Math.Round(d4 * Me.dispscale), Me.dispscale, Me.tolerance)
			If (localAtom2 = Nothing)
			{
			  localMolecule3 = New keg.compound.Molecule()
			  localAtom2 = New keg.compound.Atom(localMolecule3, d3, d4, 0.0D, str)
			  localMolecule3.set0point(New DblRect(0,0))
			  localMolecule3.addAtom(localAtom2)
			  ((keg.compound.Reaction)Me.conteiner).addObject(localMolecule3, 0)
			  localAtom2.select(Me.editmode)
			}
 Else
			{
			  localMolecule3 = localAtom2.Mol
			}
			If ((Me.editmode.draw = 9) And (localAtom1.getsp3()) And (localAtom2.getsp3()))
			{
			  j = 2
			}
			If (localAtom1 <> localAtom2)
			{
			  localMolecule1.combineMol(localAtom1, localMolecule3, localAtom2, j, k, Me.dispscale, Me.editmode.draw < 6)
			  Dim localBond2 As keg.compound.Bond = localAtom1.getBond(localAtom2)
        If (localBond2 <> Nothing)
			  {
				localBond2.select(Me.editmode)
				checkBondsNumber(localBond2)
			  }
			  localAtom1.calcImplicitHydrogen()
			  localAtom1.decisideHydrogenDraw()
			  localAtom2.calcImplicitHydrogen()
			  localAtom2.decisideHydrogenDraw()
			}
			d1 = d3
			d2 = d4
		  }
		Next
        Select Case Me.editmode.draw
        Case 6

        Case 7

        Case 8

        Case 9

        Case 11

        Case 12

        If ((Me.editmode.draw = 9) And (((keg.compound.Atom)localObject1).getsp3()) And (localAtom2.getsp3())) 
		  {
			j = 2
		  }
 Else
		  {
			j = 1
		  }
		  If (localObject1 <> localAtom2)
		  {
			localMolecule1.combineMol((keg.compound.Atom)localObject1, localMolecule1, localAtom2, j, k, Me.dispscale, Me.editmode.draw < 6)
			Dim localBond1 As keg.compound.Bond = ((keg.compound.Atom)localObject1).getBond(localAtom2) 
			If (localBond1 <> Nothing)
			{
			  localBond1.select(Me.editmode)
			  Dim bool2 As Boolean = checkBondsNumber(localBond1)
        If (Not bool2)
			  {
				Me.parentK.doUndo()
        Return False
			  }
			}
			((keg.compound.Atom)localObject1).calcImplicitHydrogen()
			((keg.compound.Atom)localObject1).decisideHydrogenDraw()
			localAtom2.calcImplicitHydrogen()
			localAtom2.decisideHydrogenDraw()
		  }
		  Exit For
        End Select
		localMolecule1.setDBond()
		((keg.compound.Reaction)Me.conteiner).checkOverlapedBracketWithRefine()
		((keg.compound.Reaction)Me.conteiner).autoSetCategory(Me.dispscale)
		repaint()
		Me.editmode.flag_edit = True
        If (Me.parentK <> Nothing)
		{
		  Me.parentK.checkButton()
		}
		Me.editmode.status = 0
        Return True
	  }
 
	  Public Static Integer getLocationOn5BaseRing(keg.compound.Bond paramBond, Integer paramInt) 
	  {
		Dim localObject1 As Object = Nothing
        Dim localAtom1 As keg.compound.Atom = paramBond.Atom1
        Dim localAtom2 As keg.compound.Atom = paramBond.Atom2
        Dim localObject2 As Object = Nothing
        Dim i As Integer = localAtom1.numBond()
        Dim j As Integer = localAtom2.numBond()
        Dim k As Integer = 0
        Dim d1 As Double = Double.MaxValue
        Dim d2 As Double = Double.MaxValue
        Dim d3 As Double = Double.MaxValue
        Dim d4 As Double = Double.MaxValue
        Dim d5 As Double = Double.MaxValue
        Dim localVector2D1 As Vector2D = New Vector2D(localAtom2.x - localAtom1.x, localAtom2.y - localAtom1.y)
        Dim localBond As keg.compound.Bond
        Dim localAtom3 As keg.compound.Atom
        Dim localVector2D2 As Vector2D
        Dim d8 As Double
        Dim m As Integer
        For m = 0 To localAtom1.numBond() - 1 Step m + 1
		  localBond = localAtom1.getBond(m)
		  If (localBond <> paramBond)
		  {
			localAtom3 = localBond.pairAtom(localAtom1)
			localVector2D2 = New Vector2D(localAtom3.x - localAtom1.x, localAtom3.y - localAtom1.y)
			d8 = VecMath2D.angle(localVector2D1, localVector2D2)
			If (Math.Abs(d1) > Math.Abs(d8))
			{
			  d1 = d8
			  localObject1 = localBond
			  localObject2 = localAtom3
			}
		  }
		Next
		localVector2D1 = New Vector2D(localAtom1.x - ((keg.compound.Atom)localObject2).x, localAtom1.y - ((keg.compound.Atom)localObject2).y)
		For m = 0 To ((keg.compound.Atom)localObject2).numBond()- 1  Step m + 1
		  localBond = ((keg.compound.Atom)localObject2).getBond(m)
		  If (localBond <> localObject1)
		  {
			localAtom3 = localBond.pairAtom((keg.compound.Atom)localObject2)
			localVector2D2 = New Vector2D(localAtom3.x - ((keg.compound.Atom)localObject2).x, localAtom3.y - ((keg.compound.Atom)localObject2).y)
			d8 = VecMath2D.angle(localVector2D1, localVector2D2)
			If (Math.Abs(d2) > Math.Abs(d8))
			{
			  d2 = d8
			}
		  }
		Next
		localVector2D1 = New Vector2D(localAtom1.x - localAtom2.x, localAtom1.y - localAtom2.y)
		For m = 0 To localAtom2.numBond() - 1 Step m + 1
		  localBond = localAtom2.getBond(m)
		  If (localBond <> paramBond)
		  {
			localAtom3 = localBond.pairAtom(localAtom2)
			localVector2D2 = New Vector2D(localAtom3.x - localAtom2.x, localAtom3.y - localAtom2.y)
			d8 = VecMath2D.angle(localVector2D1, localVector2D2)
			If (Math.Abs(d3) > Math.Abs(d8))
			{
			  d3 = d8
			  localObject1 = localBond
			  localObject2 = ((keg.compound.Bond)localObject1).pairAtom(localAtom2)
			}
		  }
		Next
		localVector2D1 = New Vector2D(localAtom2.x - ((keg.compound.Atom)localObject2).x, localAtom2.y - ((keg.compound.Atom)localObject2).y)
		For m = 0 To ((keg.compound.Atom)localObject2).numBond()- 1  Step m + 1
		  localBond = ((keg.compound.Atom)localObject2).getBond(m)
		  If (localBond <> localObject1)
		  {
			localAtom3 = localBond.pairAtom((keg.compound.Atom)localObject2)
			localVector2D2 = New Vector2D(localAtom3.x - ((keg.compound.Atom)localObject2).x, localAtom3.y - ((keg.compound.Atom)localObject2).y)
			d8 = VecMath2D.angle(localVector2D1, localVector2D2)
			If (Math.Abs(d4) > Math.Abs(d8))
			{
			  d4 = d8
			}
		  }
		Next
        Dim d6 As Double = 1.5707963267948966D
        Dim d7 As Double = 2.0943951023931953D
        If ((near(d3, d6)) And (near(d1, d6)))
		{
		  Return 5
		}
		If (near(d2, d6))
		{
		  If (near(d1, d6))
		  {
			Return 1
		  }
		  Return 3
		}
		If (near(d4, d6))
		{
		  If (near(d3, d6))
		  {
			Return 4
		  }
		  Return 2
		}
		If ((near(d1, d7)) And (near(d2, d7)) And (near(d3, d7)) And (near(d4, d7)))
		{
		  If (paramInt = 11)
		  {
			Return 4
		  }
		  Return 1
		}
		If (paramInt = 11)
		{
		  Return 4
		}
		Return 1
	  }
 
	  Public Static Boolean near(Double paramDouble1, Double paramDouble2) 
	  {
		paramDouble1 = Math.Abs(paramDouble1)
		Dim d As Double = 0.05D
        If ((paramDouble1 + d > paramDouble2) And (paramDouble1 - d < paramDouble2))
		{
		  Return True
		}
		Return (paramDouble2 + d > paramDouble1) And (paramDouble2 - d < paramDouble1)
	  }
 
	  Private Point2D createPointAuto(Point2D paramPoint2D1, Point2D paramPoint2D2, Double paramDouble) 
	  {
		Dim localPoint2D As Point2D = New Point2D(Me)
        Dim d1 As Double = paramPoint2D2.x - paramPoint2D1.x
        Dim d2 As Double = paramPoint2D2.y - paramPoint2D1.y
        Dim d5 As Double = Math.Sqrt(d1 * d1 + d2 * d2)
        Dim d3 As Double = COS_30DEG * d1 - SIN_30DEG * d2
        Dim d4 As Double = -COS_30DEG * d1 - SIN_30DEG * d2
        If (Math.Abs(d3) < Math.Abs(d4))
		{
		  paramPoint2D1.x += (-SIN_30DEG * d1 - COS_30DEG * d2) * paramDouble / d5
		  paramPoint2D1.y += d3 * paramDouble / d5
		}
 Else
		{
		  paramPoint2D1.x += (-SIN_30DEG * d1 + COS_30DEG * d2) * paramDouble / d5
		  paramPoint2D1.y += d4 * paramDouble / d5
		}
		Return localPoint2D
	  }
 
	  Private Point2D createPointAuto(Point2D paramPoint2D1, Point2D paramPoint2D2, Point2D paramPoint2D3, Double paramDouble) 
	  {
		Dim localPoint2D As Point2D = New Point2D(Me)
        Dim localVector2D2 As Vector2D = New Vector2D(paramPoint2D2.x - paramPoint2D1.x, paramPoint2D2.y - paramPoint2D1.y)
        Dim localVector2D3 As Vector2D = New Vector2D(paramPoint2D3.x - paramPoint2D1.x, paramPoint2D3.y - paramPoint2D1.y)
		localVector2D2 = localVector2D2.multiple(1.0D / localVector2D2.length())
		localVector2D3 = localVector2D3.multiple(1.0D / localVector2D3.length())
		Dim localVector2D1 As Vector2D = VecMath2D.add(localVector2D2, localVector2D3)
        If ((localVector2D1.x = 0.0D) And (localVector2D1.y = 0.0D))
		{
		  Dim arrayOfDouble1() As Double = {paramPoint2D2.x - paramPoint2D1.x, paramPoint2D2.y - paramPoint2D1.y}

        Dim arrayOfDouble2() As Double = rotete(1.5707963267948966D, arrayOfDouble1)
        Dim arrayOfDouble3() As Double = rotete(-1.5707963267948966D, arrayOfDouble1)
        If (paramPoint2D1.y - arrayOfDouble2(1) < paramPoint2D1.y - arrayOfDouble3(1))
		  {
			localVector2D1.x = arrayOfDouble2(0)
			localVector2D1.y = arrayOfDouble2(1)
		  }
 Else
		  {
			localVector2D1.x = arrayOfDouble3(0)
			localVector2D1.y = arrayOfDouble3(1)
		  }
		}
		localVector2D1 = localVector2D1.multiple(paramDouble / localVector2D1.length())
		Dim - As paramPoint2D1.x =  localVector2D1.x 
		Dim - As paramPoint2D1.y =  localVector2D1.y 
		Return localPoint2D
	  }
 
	  Private Point2D createPointAuto(Point2D paramPoint2D, ArrayList paramVector, Double paramDouble) 
	  {
		Dim localPoint2D As Point2D = New Point2D(Me)
        Dim d1 As Double = 360D
        Dim d2 As Double = 360D
        Dim d3 As Double = 0.0D
        Dim i As Integer = paramVector.Count
        Dim j As Integer = 0
        Dim k As Integer = 1
        Dim m As Integer = -1
        Dim n As Integer = -1
        Dim i1 As Integer = -1
        Dim arrayOfBoolean() As Boolean = New Boolean(paramVector.Count) {}
        Dim i2 As Integer
        For i2 = 0 To paramVector.Count - 1 Step i2 + 1
		  arrayOfBoolean(i2) = False
		Next
        While i > 0
		  localVector2D3 = New Vector2D(((Point2D)paramVector(j)).x - paramPoint2D.x, ((Point2D)paramVector(j)).y - paramPoint2D.y)
		  d2 = 360.0D
		  m = -1
		  For k = 0 To paramVector.Count - 1 Step k + 1
        If ((arrayOfBoolean(k) = 0) And (j <> k))
			{
			  localVector2D2 = New Vector2D(((Point2D)paramVector(k)).x - paramPoint2D.x, ((Point2D)paramVector(k)).y - paramPoint2D.y)
			  d1 = VecMath2D.angleDeg(localVector2D3, localVector2D2)
			  If (d1 < 0.0D)
			  {
				d1 += 360.0D
			  }
			  If (d1 < d2)
			  {
				m = k
				d2 = d1
			  }
			}
		  Next
        If (d3 < d2)
		  {
			n = j
			i1 = m
			d3 = d2
		  }
		  j = m
		  arrayOfBoolean(m) = True
		  i = i - 1
		End While
        Dim localVector2D3 As Vector2D = New Vector2D(((Point2D)paramVector(n)).x - paramPoint2D.x,((Point2D)paramVector(n)).y - paramPoint2D.y) 
		Dim localVector2D2 As Vector2D = New Vector2D(((Point2D)paramVector(i1)).x - paramPoint2D.x,((Point2D)paramVector(i1)).y - paramPoint2D.y) 
		localVector2D3 = localVector2D3.multiple(1.0D / localVector2D3.length())
		localVector2D2 = localVector2D2.multiple(1.0D / localVector2D2.length())
		Dim localVector2D1 As Vector2D = VecMath2D.add(localVector2D3, localVector2D2)
        If ((localVector2D1.x = 0.0D) And (localVector2D1.y = 0.0D))
		{
		  Dim arrayOfDouble1() As Double = {localVector2D3.x, localVector2D3.y}

        Dim arrayOfDouble2() As Double = rotete(1.5707963267948966D, arrayOfDouble1)
        Dim arrayOfDouble3() As Double = rotete(-1.5707963267948966D, arrayOfDouble1)
        If (paramPoint2D.y - arrayOfDouble2(1) < paramPoint2D.y - arrayOfDouble3(1))
		  {
			localVector2D1.x = arrayOfDouble2(0)
			localVector2D1.y = arrayOfDouble2(1)
		  }
 Else
		  {
			localVector2D1.x = arrayOfDouble3(0)
			localVector2D1.y = arrayOfDouble3(1)
		  }
		}
		localVector2D1 = localVector2D1.multiple(paramDouble / localVector2D1.length())
		paramPoint2D.x += localVector2D1.x
		paramPoint2D.y += localVector2D1.y
		Return localPoint2D
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseClickDraw(java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		If ((Me.editmode.draw = 8) Or (Me.editmode.draw = 9))
		{
		  Return mouseClickDraw_6Ring_OR_BENZEN(paramMouseEvent)
		}
		Dim localAtom2 As keg.compound.Atom = Nothing
        Dim i As Integer = 1
        Dim j As Integer = 1
        Dim k As Integer = 0
        Dim m As Integer = -1
        Dim str As String = Nothing
        Dim d9 As Double = Me.fixed_length / Me.dispscale
        Dim n As Integer = 2
        Dim d10 As Double = Me.downP.x / Me.dispscale
        Dim d11 As Double = Me.downP.y / Me.dispscale
        Dim d12 As Double = Me.downP.x / Me.dispscale
        Dim d13 As Double = Me.downP.x / Me.dispscale
        If ((Me.editmode.atom = Nothing) And (Me.editmode.bond = Nothing))
		{
		  Select Case Me.editmode.draw
        Case 0

        Case 1

        Case 2

        Case 3

        Case 4

        Case 5 : 

			d12 = d10 + d9 * COS_30DEG
			d13 = d11 + d9 * SIN_30DEG
			Exit For
        Case 6

        Case 7

        Case 8

        Case 9

        Case 11

        Case 12 : 

			d13 = d11 + d9
			If (Me.editmode.draw = 11)
			{
			  n = 4
			}
 ElseIf (Me.editmode.draw = 12)
			{
			  n = 1
			}
			Exit For
        End Select
		}
		If ((Me.editmode.atom <> Nothing) And (Not Me.editmode.atom.Express_group))
		{
		  Dim localPoint2D1 As Point2D
        Dim localObject2 As Object
        Dim localPoint2D2 As Point2D
        Dim localPoint2D4 As Point2D
        Dim localPoint2D3 As Point2D
        Select Case Me.editmode.draw
        Case 0

        Case 1

        Case 2

        Case 3

        Case 4

        Case 5

        Select Case Me.editmode.atom.numBond()
        Case 0 : 

			  d12 = Me.editmode.atom.x + d9 * COS_30DEG
			  d13 = Me.editmode.atom.y + d9 * SIN_30DEG
			  Exit For
        Case 1 : 

			  localPoint2D1 = New Point2D(this, Me.editmode.atom.x, Me.editmode.atom.y)
			  localObject2 = Me.editmode.atom.getBond(0).pairAtom(Me.editmode.atom)
			  localPoint2D2 = New Point2D(this, ((keg.compound.Atom)localObject2).x, ((keg.compound.Atom)localObject2).y)
			  localPoint2D4 = createPointAuto(localPoint2D1, localPoint2D2, d9)
			  d12 = localPoint2D4.x
			  d13 = localPoint2D4.y
			  Exit For
        Case 2 : 

			  localPoint2D1 = New Point2D(this, Me.editmode.atom.x, Me.editmode.atom.y)
			  localObject2 = Me.editmode.atom.getBond(0).pairAtom(Me.editmode.atom)
			  localPoint2D2 = New Point2D(this, ((keg.compound.Atom)localObject2).x, ((keg.compound.Atom)localObject2).y)
			  localObject2 = Me.editmode.atom.getBond(1).pairAtom(Me.editmode.atom)
			  localPoint2D3 = New Point2D(this, ((keg.compound.Atom)localObject2).x, ((keg.compound.Atom)localObject2).y)
			  localPoint2D4 = createPointAuto(localPoint2D1, localPoint2D2, localPoint2D3, d9)
			  d12 = localPoint2D4.x
			  d13 = localPoint2D4.y
			  Exit For
        Case Else : 

			  localPoint2D1 = New Point2D(this, Me.editmode.atom.x, Me.editmode.atom.y)
			  localObject2 = New ArrayList()
			  Dim i6 As Integer
        For i6 = 0 To Me.editmode.atom.numBond() - 1 Step i6 + 1
        Dim localAtom3 As keg.compound.Atom = Me.editmode.atom.getBond(i6).pairAtom(Me.editmode.atom)
				((ArrayList)localObject2).Add(New Point2D(this,localAtom3.x,localAtom3.y))
			  Next
			  localPoint2D4 = createPointAuto(localPoint2D1, (ArrayList)localObject2, d9)
			  d12 = localPoint2D4.x
			  d13 = localPoint2D4.y
		  Exit For
        End Select
        Exit For
        Case 6

        Case 7

        Case 8

        Case 9

        Case 11

        Case 12

        Dim d14 As Double
        Dim d17 As Double
        Dim d19 As Double
        Dim d21 As Double
        Select Case Me.editmode.atom.numBond()
        Case 0 : 

			  d12 = Me.editmode.atom.x
			  d13 = Me.editmode.atom.y + d9
			  Exit For
        Case 1 : 

			  d14 = 1.0D
			  d17 = 0.0D
			  Select Case Me.editmode.draw
        Case 6 : 

				d14 = -0.70710678D
				d17 = -0.70710678D
				Exit For
        Case 7 : 

				d14 = -0.58778525D
				d17 = -0.80901699D
				Exit For
        Case 8

        Case 9

        Case 11

        Case 12 : 

				d14 = -SIN_30DEG
				d17 = -COS_30DEG
			Exit For
        End Select
			  d19 = Me.editmode.atom.getBond(0).pairAtom(Me.editmode.atom).x - Me.editmode.atom.x
			  d21 = Me.editmode.atom.getBond(0).pairAtom(Me.editmode.atom).y - Me.editmode.atom.y
			  Dim d22 As Double = Math.Sqrt(d19 * d19 + d21 * d21)
			  d19 = d19 / d22 * d9
			  d21 = d21 / d22 * d9
			  d12 = d14 * d19 - d17 * d21 + Me.editmode.atom.x
			  d13 = d17 * d19 + d14 * d21 + Me.editmode.atom.y
			  Exit For
        Case 2 : 

			  localPoint2D1 = New Point2D(this, Me.editmode.atom.x, Me.editmode.atom.y)
			  localObject2 = Me.editmode.atom.getBond(0).pairAtom(Me.editmode.atom)
			  localPoint2D2 = New Point2D(this, ((keg.compound.Atom)localObject2).x, ((keg.compound.Atom)localObject2).y)
			  localObject2 = Me.editmode.atom.getBond(1).pairAtom(Me.editmode.atom)
			  localPoint2D3 = New Point2D(this, ((keg.compound.Atom)localObject2).x, ((keg.compound.Atom)localObject2).y)
			  localPoint2D4 = createPointAuto(localPoint2D1, localPoint2D2, localPoint2D3, d9)
			  d14 = 1.0D
			  d17 = 0.0D
			  Select Case Me.editmode.draw
        Case 6 : 

				d14 = 0.70710678D
				d17 = 0.70710678D
				Exit For
        Case 7 : 

				d14 = 0.58778525D
				d17 = 0.80901699D
				Exit For
        Case 8

        Case 9

        Case 11

        Case 12 : 

				d14 = SIN_30DEG
				d17 = COS_30DEG
			Exit For
        End Select
			  d19 = localPoint2D4.x - Me.editmode.atom.x
			  d21 = localPoint2D4.y - Me.editmode.atom.y
			  d12 = d14 * d19 - d17 * d21 + Me.editmode.atom.x
			  d13 = d17 * d19 + d14 * d21 + Me.editmode.atom.y
			  Exit For
        Case Else : 

			  d12 = Me.editmode.atom.getBond(0).pairAtom(Me.editmode.atom).x
			  d13 = Me.editmode.atom.getBond(0).pairAtom(Me.editmode.atom).y
		  Exit For
        End Select
        Exit For
        End Select
		}
		Dim d15 As Double
        Dim localMolecule3 As keg.compound.Molecule
        If (Me.editmode.bond <> Nothing)
		{
		  Me.editmode.bond.Recalc = True
        Select Case Me.editmode.draw
        Case 0

        If (Me.editmode.bond.Stereo = 0)
			{
			  If (Me.editmode.bond.Order = 1)
			  {
				draw_DoubleBond(Me.editmode)
				If (isTooMuchBondsNumber(Me.editmode.bond))
				{
				  Dim i1 As Integer = Me.editmode.bond.Atom1.LimitNumberOfConnections
        Dim i3 As Integer = Me.editmode.bond.Atom1.MaxLimitNumberOfConnections
        Dim i4 As Integer = Me.editmode.bond.Atom2.LimitNumberOfConnections
        Dim i5 As Integer = Me.editmode.bond.Atom2.MaxLimitNumberOfConnections
        If ((i1 = i3) And (i4 = i5) And (Not checkBondsNumber(Me.editmode.bond)))
				  {
					Me.parentK.doUndo()
        Return False
				  }
				}
 ElseIf (Not checkBondsNumber(Me.editmode.bond))
				{
				  Me.parentK.doUndo()
        Return False
				}
			  }
 ElseIf (Me.editmode.bond.Order = 2)
			  {
				draw_TripleBond(Me.editmode)
				If (isTooMuchBondsNumber(Me.editmode.bond))
				{
				  draw_SingleBond(Me.editmode)
				}
 ElseIf (Not checkBondsNumber(Me.editmode.bond))
				{
				  Me.parentK.doUndo()
        Return False
				}
			  }
 ElseIf (Me.editmode.bond.Order = 3)
			  {
				draw_SingleBond(Me.editmode)
				If (Not checkBondsNumber(Me.editmode.bond))
				{
				  Me.parentK.doUndo()
        Return False
				}
			  }
 Else
			  {
				draw_SingleBond(Me.editmode)
				If (Not checkBondsNumber(Me.editmode.bond))
				{
				  Me.parentK.doUndo()
        Return False
				}
			  }
			}
 Else
			{
			  draw_SingleBond(Me.editmode)
			  If (Not checkBondsNumber(Me.editmode.bond))
			  {
				Me.parentK.doUndo()
        Return False
			  }
			}
			Me.editmode.bond.Mol.setDBond()
			repaint()
			Return True
        Case 1 : 

			draw_DoubleBond(Me.editmode)
			If (Not checkBondsNumber(Me.editmode.bond))
			{
			  Me.parentK.doUndo()
        Return False
			}
			Me.editmode.bond.Mol.setDBond()
			repaint()
			Return True
        Case 2 : 

			draw_TripleBond(Me.editmode)
			If (Not checkBondsNumber(Me.editmode.bond))
			{
			  Me.parentK.doUndo()
        Return False
			}
			Me.editmode.bond.Mol.setDBond()
			repaint()
			Return True
        Case 3

        If (Me.editmode.bond.Stereo <> 1)
			{
			  If (Me.editmode.bond.Atom2.Equals(Me.editmode.bond.SideWithMoreConnections))
			  {
				Me.editmode.bond.swapAtom()
			  }
			}
 Else
			{
			  Me.editmode.bond.swapAtom()
			}
			Me.editmode.bond.Order = 1
        Me.editmode.bond.Stereo = 1
			localObject1 = Me.editmode.bond.nearSide(Me.upP.x, Me.upP.y, Me.dispscale)
			Me.editmode.bond.calcImplicitHydrogen()
        Me.editmode.bond.decisideHydrogenDraw()
        If (Not checkBondsNumber(Me.editmode.bond))
			{
			  Me.parentK.doUndo()
        Return False
			}
			Me.editmode.bond.Mol.setDBond()
			repaint()
			Return True
        Case 4

        If (Me.editmode.bond.Stereo <> 6)
			{
			  If (Me.editmode.bond.Atom2.Equals(Me.editmode.bond.SideWithMoreConnections))
			  {
				Me.editmode.bond.swapAtom()
			  }
			}
 Else
			{
			  Me.editmode.bond.swapAtom()
			}
			Me.editmode.bond.Order = 1
        Me.editmode.bond.Stereo = 6
			localObject1 = Me.editmode.bond.nearSide(Me.upP.x, Me.upP.y, Me.dispscale)
			Me.editmode.bond.calcImplicitHydrogen()
        Me.editmode.bond.decisideHydrogenDraw()
        If (Not checkBondsNumber(Me.editmode.bond))
			{
			  Me.parentK.doUndo()
        Return False
			}
			Me.editmode.bond.Mol.setDBond()
			repaint()
			Return True
        Case 5

        If (Me.editmode.bond.Stereo <> 4)
			{
			  If (Me.editmode.bond.Atom2.Equals(Me.editmode.bond.SideWithMoreConnections))
			  {
				Me.editmode.bond.swapAtom()
			  }
			}
 Else
			{
			  Me.editmode.bond.swapAtom()
			}
			Me.editmode.bond.Order = 1
        Me.editmode.bond.Stereo = 4
			localObject1 = Me.editmode.bond.nearSide(Me.upP.x, Me.upP.y, Me.dispscale)
			Me.editmode.bond.calcImplicitHydrogen()
        Me.editmode.bond.decisideHydrogenDraw()
        If (Not checkBondsNumber(Me.editmode.bond))
			{
			  Me.parentK.doUndo()
        Return False
			}
			Me.editmode.bond.Mol.setDBond()
			repaint()
			Return True
        Case 6

        Case 7

        Case 8

        Case 9

        Case 11

        Case 12

        If (keg.compound.Molecule.whichSide(Me.editmode.bond))
			{
			  Me.editmode.atom = Me.editmode.bond.Atom1
			  localAtom2 = Me.editmode.bond.Atom2
			}
 Else
			{
			  Me.editmode.atom = Me.editmode.bond.Atom2
			  localAtom2 = Me.editmode.bond.Atom1
			}
			If ((Me.editmode.draw = 11) Or (Me.editmode.draw = 12))
			{
			  n = getLocationOn5BaseRing(Me.editmode.bond, Me.editmode.draw)
			  If (n = 5)
			  {
				If (Not keg.compound.Molecule.whichSide(Me.editmode.bond))
				{
				  Me.editmode.atom = Me.editmode.bond.Atom1
				  localAtom2 = Me.editmode.bond.Atom2
				}
 Else
				{
				  Me.editmode.atom = Me.editmode.bond.Atom2
				  localAtom2 = Me.editmode.bond.Atom1
				}
				d5 = Me.editmode.atom.x
				d6 = Me.editmode.atom.y
				d7 = localAtom2.x
				d8 = localAtom2.y
				d15 = Math.Atan2(d8 - d6, d7 - d5)
				d15 += 1.5707963267948966D
				d3 = d9 * Math.Cos(d15) + d5
				d4 = d9 * Math.Sin(d15) + d6
				localMolecule3 = New keg.compound.Molecule()
				localAtom2 = New keg.compound.Atom(localMolecule3, d3, d4, 0.0D, "")
				localMolecule3.set0point(New DblRect(0,0))
				localMolecule3.addAtom(localAtom2)
				((keg.compound.Reaction)Me.conteiner).addObject(localMolecule3, 0)
				localAtom2.select(Me.editmode)
				n = 4
			  }
			}
			Exit For
        End Select
		}
		str = ""
		Dim localAtom1 As keg.compound.Atom = Me.editmode.atom
        Dim localMolecule2 As keg.compound.Molecule
        If (localAtom1 = Nothing)
		{
		  localMolecule2 = New keg.compound.Molecule()
		  localAtom1 = New keg.compound.Atom(localMolecule2, d10, d11, 0.0D, str)
		  localMolecule2.addAtom(localAtom1)
		  ((keg.compound.Reaction)Me.conteiner).addObject(localMolecule2, 0)
		  localAtom1.select(Me.editmode)
		}
 Else
		{
		  localAtom1.unselect(Me.editmode)
		  localMolecule2 = localAtom1.Mol
		  m = ((keg.compound.Reaction)Me.conteiner).getCategory(((keg.compound.Reaction)Me.conteiner).getObjectNo(localMolecule2))
		}
		If (localAtom2 = Nothing)
		{
		  localAtom2 = Me.conteiner.nearAtom((Integer)Math.Round(d12 * Me.dispscale), (Integer)Math.Round(-d13 * Me.dispscale), Me.dispscale, Me.tolerance)
		}
		If (localAtom2 = Nothing)
		{
		  If (d9 > 0.0D)
		  {
			d15 = Math.Sqrt((d10 - d12) * (d10 - d12) + (d11 - d13) * (d11 - d13))
			d12 = d10 + (d12 - d10) * d9 / d15
			d13 = d11 + (d13 - d11) * d9 / d15
		  }
		  localAtom2 = Me.conteiner.nearAtom((Integer)Math.Round(d12 * Me.dispscale), (Integer)Math.Round(d13 * Me.dispscale), Me.dispscale, Me.tolerance)
		  If (localAtom2 = Nothing)
		  {
			localMolecule3 = New keg.compound.Molecule()
			localAtom2 = New keg.compound.Atom(localMolecule3, d12, d13, 0.0D, str)
			localMolecule3.addAtom(localAtom2)
			((keg.compound.Reaction)Me.conteiner).addObject(localMolecule3, 0)
			localAtom2.select(Me.editmode)
		  }
 Else
		  {
			localMolecule3 = localAtom2.Mol
		  }
		}
 Else
		{
		  localMolecule3 = localAtom2.Mol
		  If (m < 0)
		  {
			m = ((keg.compound.Reaction)Me.conteiner).getCategory(((keg.compound.Reaction)Me.conteiner).getObjectNo(localMolecule3))
		  }
		}
		If (m < 0)
		{
		  m = Me.editmode.category
		}
		Select Case Me.editmode.draw
        Case 1 : 

		  j = 2
		  Exit For
        Case 2 : 

		  j = 3
		  Exit For
        Case 3 : 

		  k = 1
		  Exit For
        Case 4 : 

		  k = 6
		  Exit For
        Case 5 : 

		  k = 4
		  Exit For
        Case 6 : 

		  i = 4
		  Exit For
        Case 7 : 

		  i = 5
		  Exit For
        Case 8

        Case 11

        Case 12 : 

		  i = 6
		  Exit For
        Case 9

        If ((localAtom1.getsp3()) And (localAtom2.getsp3()))
		  {
			j = 2
		  }
		  i = 6
	  Exit For
        End Select
        If (localAtom1 <> localAtom2)
		{
		  localMolecule2.combineMol(localAtom1, localMolecule3, localAtom2, j, k, Me.dispscale, Me.editmode.draw < 6)
		  Dim localBond1 As keg.compound.Bond = localAtom1.getBond(localAtom2)
        If (localBond1 <> Nothing)
		  {
			localBond1.select(Me.editmode)
			If (Not checkBondsNumber(localBond1))
			{
			  Me.parentK.doUndo()
        Return False
			}
		  }
		  localAtom1.calcImplicitHydrogen()
		  localAtom1.decisideHydrogenDraw()
		  localAtom2.calcImplicitHydrogen()
		  localAtom2.decisideHydrogenDraw()
		}
		((keg.compound.Reaction)Me.conteiner).setCategory(localMolecule2, m)
		Dim d5 As Double = localAtom1.x
        Dim d6 As Double = localAtom1.y
        Dim d7 As Double = localAtom2.x
        Dim d8 As Double = localAtom2.y
        Dim d1 As Double = d7
        Dim d2 As Double = d8
        Dim d3 As Double = d7
        Dim d4 As Double = d8
        Dim localMolecule1 As keg.compound.Molecule = localMolecule2
        Dim localObject1 As Object = localAtom1
        If ((n = 4) Or (n = 1))
		{
		  If ((Me.editmode.draw = 11) And (d6 < d8))
		  {
			n = 4
		  }
		  If ((Me.editmode.draw = 11) And (d6 > d8))
		  {
			n = 1
		  }
		  If ((Me.editmode.draw = 12) And (d6 < d8))
		  {
			n = 1
		  }
		  If ((Me.editmode.draw = 12) And (d6 > d8))
		  {
			n = 4
		  }
		}
		Dim i2 As Integer
        For i2 = 1 To i - 1 - 1 Step i2 + 1
        If ((Me.editmode.draw = 11) Or (Me.editmode.draw = 12))
		  {
			i2 = i2 = n ? i2 + 1 : i2

			If (i2 >= i - 1)
			{
			}
		  }
 Else
		  {
			localMolecule2 = localMolecule3
			localAtom1 = localAtom2
			Select Case Me.editmode.draw
        Case 6

        Select Case i2
        Case 1 : 

				d3 = d8 - d6 + d7
				d4 = d5 - d7 + d8
				Exit For
        Case 2 : 

				d3 = d8 - d6 + d5
				d4 = d5 - d7 + d6
			Exit For
        End Select
        Exit For
        Case 7

        Select Case i2
        Case 1 : 

				d3 = -0.309017D * (d5 - d7) - 0.951065D * (d6 - d8) + d7
				d4 = 0.951065D * (d5 - d7) + -0.309017D * (d6 - d8) + d8
				Exit For
        Case 2 : 

				d3 = (0.309017D * (d5 - d7) - 0.951065D * (d6 - d8)) * 1.618034D + d7
				d4 = (0.951065D * (d5 - d7) + 0.309017D * (d6 - d8)) * 1.618034D + d8
				Exit For
        Case 3 : 

				d3 = -0.309017D * (d7 - d5) - -0.951065D * (d8 - d6) + d5
				d4 = -0.951065D * (d7 - d5) + -0.309017D * (d8 - d6) + d6
			Exit For
        End Select
        Exit For
        Case 8

        Case 9

        Case 11

        Case 12

        Dim d16 As Double = d5 - d7
        Dim d18 As Double = d6 - d8
        Dim d20 As Double = Math.Atan2(d18, d16)
			  d16 = d9 * Math.Cos(d20)
			  d18 = d9 * Math.Sin(d20)
			  Select Case i2
        Case 1 : 

				d3 = -SIN_30DEG * d16 - COS_30DEG * d18 + d7
				d4 = COS_30DEG * d16 + -SIN_30DEG * d18 + d8
				j = 1
				Exit For
        Case 2 : 

				d3 = TAN_60DEG * -d18 + d7
				d4 = TAN_60DEG * d16 + d8
				j = 1
				Exit For
        Case 3 : 

				d3 = TAN_60DEG * -d18 + d5
				d4 = TAN_60DEG * d16 + d6
				j = 1
				Exit For
        Case 4 : 

				d3 = -SIN_30DEG * -d16 - -COS_30DEG * -d18 + d5
				d4 = -COS_30DEG * -d16 + -SIN_30DEG * -d18 + d6
				j = 1
			Exit For
        End Select
        Exit For
        End Select
			localAtom2 = Me.conteiner.nearAtom((Integer)Math.Round(d3 * Me.dispscale), (Integer)Math.Round(d4 * Me.dispscale), Me.dispscale, Me.tolerance)
			If (localAtom2 = Nothing)
			{
			  localMolecule3 = New keg.compound.Molecule()
			  localAtom2 = New keg.compound.Atom(localMolecule3, d3, d4, 0.0D, str)
			  localMolecule3.addAtom(localAtom2)
			  ((keg.compound.Reaction)Me.conteiner).addObject(localMolecule3, 0)
			  localAtom2.select(Me.editmode)
			}
 Else
			{
			  localMolecule3 = localAtom2.Mol
			}
			If ((Me.editmode.draw = 9) And (localAtom1.getsp3()) And (localAtom2.getsp3()))
			{
			  j = 2
			}
			If (localAtom1 <> localAtom2)
			{
			  localMolecule1.combineMol(localAtom1, localMolecule3, localAtom2, j, k, Me.dispscale, Me.editmode.draw < 6)
			  Dim localBond3 As keg.compound.Bond = localAtom1.getBond(localAtom2)
        If (localBond3 <> Nothing)
			  {
				localBond3.select(Me.editmode)
				Dim bool2 As Boolean = checkBondsNumber(localBond3)
        If (Not bool2)
				{
				  Me.parentK.doUndo()
        Return False
				}
			  }
			  localAtom1.calcImplicitHydrogen()
			  localAtom1.decisideHydrogenDraw()
			  localAtom2.calcImplicitHydrogen()
			  localAtom2.decisideHydrogenDraw()
			}
			d1 = d3
			d2 = d4
		  }
		Next
        Select Case Me.editmode.draw
        Case 6

        Case 7

        Case 8

        Case 9

        Case 11

        Case 12

        If ((Me.editmode.draw = 9) And (((keg.compound.Atom)localObject1).getsp3()) And (localAtom2.getsp3())) 
		  {
			j = 2
		  }
 Else
		  {
			j = 1
		  }
		  If (localObject1 <> localAtom2)
		  {
			localMolecule1.combineMol((keg.compound.Atom)localObject1, localMolecule1, localAtom2, j, k, Me.dispscale, Me.editmode.draw < 6)
			Dim localBond2 As keg.compound.Bond = ((keg.compound.Atom)localObject1).getBond(localAtom2) 
			If (localBond2 <> Nothing)
			{
			  localBond2.select(Me.editmode)
			  Dim bool1 As Boolean = checkBondsNumber(localBond2)
        If (Not bool1)
			  {
				Me.parentK.doUndo()
        Return False
			  }
			}
			((keg.compound.Atom)localObject1).calcImplicitHydrogen()
			((keg.compound.Atom)localObject1).decisideHydrogenDraw()
			localAtom2.calcImplicitHydrogen()
			localAtom2.decisideHydrogenDraw()
		  }
		  Exit For
        End Select
		localMolecule1.setDBond()
		((keg.compound.Reaction)Me.conteiner).checkOverlapedBracketWithRefine()
		((keg.compound.Reaction)Me.conteiner).autoSetCategory(Me.dispscale)
		repaint()
		Me.editmode.flag_edit = True
        If (Me.parentK <> Nothing)
		{
		  Me.parentK.checkButton()
		}
		Me.editmode.status = 0
        Me.editmode.atom = localAtom2
        Return True
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseClickDraw_6Ring_OR_BENZEN(java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		If ((Me.editmode.draw <> 8) And (Me.editmode.draw <> 9))
		{
		  Return False
		}
		Dim localAtom1 As keg.compound.Atom = Nothing
        Dim d1 As Double = Me.fixed_length / Me.dispscale
        Dim d2 As Double = Me.downP.x / Me.dispscale
        Dim d3 As Double = Me.downP.y / Me.dispscale
        Dim d4 As Double = Me.downP.x / Me.dispscale
        Dim d5 As Double = Me.downP.x / Me.dispscale
        Dim localObject1 As Object
        If ((Me.editmode.atom = Nothing) And (Me.editmode.bond = Nothing))
		{
		  d5 = d3 + d1
		}
 Else
		{
		  If ((Me.editmode.atom <> Nothing) And (Not Me.editmode.atom.Express_group))
		  {
		  }
		  Dim d11 As Double
        Dim d13 As Double
        Select Case Me.editmode.atom.numBond()
        Case 0 : 

			d4 = Me.editmode.atom.x
			d5 = Me.editmode.atom.y + d1
			Exit For
        Case 1

        Dim d6 As Double = -SIN_30DEG
        Dim d7 As Double = -COS_30DEG
        Dim d8 As Double = Me.editmode.atom.getBond(0).pairAtom(Me.editmode.atom).x - Me.editmode.atom.x
			d11 = Me.editmode.atom.getBond(0).pairAtom(Me.editmode.atom).y - Me.editmode.atom.y
			d13 = Math.Sqrt(d8 * d8 + d11 * d11)
			d8 = d8 / d13 * d1
			d11 = d11 / d13 * d1
			d4 = d6 * d8 - d7 * d11 + Me.editmode.atom.x
			d5 = d7 * d8 + d6 * d11 + Me.editmode.atom.y
			Exit For
        Case 2 : 

			localObject1 = New Point2D(this, Me.editmode.atom.x, Me.editmode.atom.y)
			Dim localAtom2 As keg.compound.Atom = Me.editmode.atom.getBond(0).pairAtom(Me.editmode.atom)
			localObject2 = New Point2D(this, localAtom2.x, localAtom2.y)
			localAtom2 = Me.editmode.atom.getBond(1).pairAtom(Me.editmode.atom)
			Dim localPoint2D As Point2D = New Point2D(this, localAtom2.x, localAtom2.y)
			localObject3 = createPointAuto((Point2D)localObject1, (Point2D)localObject2, localPoint2D, d1)
			Dim d9 As Double = SIN_30DEG
			d11 = COS_30DEG
			d13 = ((Point2D)localObject3).x - Me.editmode.atom.x
			Dim d15 As Double = ((Point2D)localObject3).y - Me.editmode.atom.y 
			d4 = d9 * d13 - d11 * d15 + Me.editmode.atom.x
			d5 = d11 * d13 + d9 * d15 + Me.editmode.atom.y
			Exit For
        Case Else : 

			d4 = Me.editmode.atom.getBond(0).pairAtom(Me.editmode.atom).x
			d5 = Me.editmode.atom.getBond(0).pairAtom(Me.editmode.atom).y
			Exit For
        If (Me.editmode.bond <> Nothing)
			{
			  Me.editmode.bond.Recalc = True
        If (keg.compound.Molecule.whichSide(Me.editmode.bond))
			  {
				Me.editmode.atom = Me.editmode.bond.Atom1
				localAtom1 = Me.editmode.bond.Atom2
			  }
 Else
			  {
				Me.editmode.atom = Me.editmode.bond.Atom2
				localAtom1 = Me.editmode.bond.Atom1
			  }
			}
			Exit For
        End Select
		}
		Dim localObject2 As Object = Me.editmode.atom
        Dim i As Integer = -1
        Dim localObject3 As Object = ""
        If (localObject2 = Nothing)
		{
		  localObject1 = New keg.compound.Molecule()
		  localObject2 = New keg.compound.Atom((keg.compound.Molecule)localObject1, d2, d3, 0.0D, (String)localObject3)
		  ((keg.compound.Molecule)localObject1).addAtom((keg.compound.Atom)localObject2)
		  ((keg.compound.Reaction)Me.conteiner).addObject((keg.compound.ChemObject)localObject1, 0)
		  ((keg.compound.Atom)localObject2).select(Me.editmode)
		}
 Else
		{
		  ((keg.compound.Atom)localObject2).unselect(Me.editmode)
		  localObject1 = ((keg.compound.Atom)localObject2).Mol
		  i = ((keg.compound.Reaction)Me.conteiner).getCategory(((keg.compound.Reaction)Me.conteiner).getObjectNo((keg.compound.ChemObject)localObject1))
		}
		If (localAtom1 = Nothing)
		{
		  localAtom1 = Me.conteiner.nearAtom((Integer)Math.Round(d4 * Me.dispscale), (Integer)Math.Round(-d5 * Me.dispscale), Me.dispscale, Me.tolerance)
		}
		Dim localMolecule As keg.compound.Molecule
        If (localAtom1 = Nothing)
		{
		  If (d1 > 0.0D)
		  {
			Dim d10 As Double = Math.Sqrt((d2 - d4) * (d2 - d4) + (d3 - d5) * (d3 - d5))
			d4 = d2 + (d4 - d2) * d1 / d10
			d5 = d3 + (d5 - d3) * d1 / d10
		  }
		  localAtom1 = Me.conteiner.nearAtom((Integer)Math.Round(d4 * Me.dispscale), (Integer)Math.Round(d5 * Me.dispscale), Me.dispscale, Me.tolerance)
		  If (localAtom1 = Nothing)
		  {
			localMolecule = New keg.compound.Molecule()
			localAtom1 = New keg.compound.Atom(localMolecule, d4, d5, 0.0D, (String)localObject3)
			localMolecule.addAtom(localAtom1)
			((keg.compound.Reaction)Me.conteiner).addObject(localMolecule, 0)
			localAtom1.select(Me.editmode)
		  }
 Else
		  {
			localMolecule = localAtom1.Mol
		  }
		}
 Else
		{
		  localMolecule = localAtom1.Mol
		  If (i < 0)
		  {
			i = ((keg.compound.Reaction)Me.conteiner).getCategory(((keg.compound.Reaction)Me.conteiner).getObjectNo(localMolecule))
		  }
		}
		Dim j As Integer = 1
        Dim k As Integer = 0
        If (i < 0)
		{
		  i = Me.editmode.category
		}
		If ((Me.editmode.draw = 9) And (((keg.compound.Atom)localObject2).getsp3()) And (localAtom1.getsp3())) 
		{
		  j = 2
		}
		If (localObject2 <> localAtom1)
		{
		  ((keg.compound.Molecule)localObject1).combineMol((keg.compound.Atom)localObject2, localMolecule, localAtom1, j, k, Me.dispscale, Me.editmode.draw < 6)
		  Dim localBond1 As keg.compound.Bond = ((keg.compound.Atom)localObject2).getBond(localAtom1) 
		  If (localBond1 <> Nothing)
		  {
			localBond1.select(Me.editmode)
			If (Not checkBondsNumber(localBond1))
			{
			  Me.parentK.doUndo()
        Return False
			}
		  }
		  ((keg.compound.Atom)localObject2).calcImplicitHydrogen()
		  ((keg.compound.Atom)localObject2).decisideHydrogenDraw()
		  localAtom1.calcImplicitHydrogen()
		  localAtom1.decisideHydrogenDraw()
		}
		((keg.compound.Reaction)Me.conteiner).setCategory((keg.compound.ChemObject)localObject1, i)
		Dim d12 As Double = ((keg.compound.Atom)localObject2).x 
		Dim d14 As Double = ((keg.compound.Atom)localObject2).y 
		Dim d16 As Double = localAtom1.x
        Dim d17 As Double = localAtom1.y
        Dim d18 As Double = d16
        Dim d19 As Double = d17
        Dim localObject4 As Object = localObject1
        Dim localObject5 As Object = localObject2
        Dim m As Integer = 6
        Dim n As Integer
        For n = 1 To m - 1 - 1 Step n + 1
		  localObject1 = localMolecule
		  localObject2 = localAtom1
		  Dim d20 As Double = d12 - d16
        Dim d21 As Double = d14 - d17
        Dim d22 As Double = Math.Atan2(d21, d20)
		  d20 = d1 * Math.Cos(d22)
		  d21 = d1 * Math.Sin(d22)
		  Select Case n
        Case 1 : 

			d18 = -SIN_30DEG * d20 - COS_30DEG * d21 + d16
			d19 = COS_30DEG * d20 + -SIN_30DEG * d21 + d17
			j = 1
			Exit For
        Case 2 : 

			d18 = TAN_60DEG * -d21 + d16
			d19 = TAN_60DEG * d20 + d17
			j = 1
			Exit For
        Case 3 : 

			d18 = TAN_60DEG * -d21 + d12
			d19 = TAN_60DEG * d20 + d14
			j = 1
			Exit For
        Case 4 : 

			d18 = -SIN_30DEG * -d20 - -COS_30DEG * -d21 + d12
			d19 = -COS_30DEG * -d20 + -SIN_30DEG * -d21 + d14
			j = 1
		Exit For
        End Select
		  localAtom1 = Me.conteiner.nearAtom((Integer)Math.Round(d18 * Me.dispscale), (Integer)Math.Round(d19 * Me.dispscale), Me.dispscale, Me.tolerance)
		  If (localAtom1 = Nothing)
		  {
			localMolecule = New keg.compound.Molecule()
			localAtom1 = New keg.compound.Atom(localMolecule, d18, d19, 0.0D, (String)localObject3)
			localMolecule.addAtom(localAtom1)
			((keg.compound.Reaction)Me.conteiner).addObject(localMolecule, 0)
			localAtom1.select(Me.editmode)
		  }
 Else
		  {
			localMolecule = localAtom1.Mol
		  }
		  If ((Me.editmode.draw = 9) And (((keg.compound.Atom)localObject2).getsp3()) And (localAtom1.getsp3())) 
		  {
			j = 2
		  }
		  If (localObject2 <> localAtom1)
		  {
			((keg.compound.Molecule)localObject4).combineMol((keg.compound.Atom)localObject2, localMolecule, localAtom1, j, k, Me.dispscale, Me.editmode.draw < 6)
			Dim localBond3 As keg.compound.Bond = ((keg.compound.Atom)localObject2).getBond(localAtom1) 
			If (localBond3 <> Nothing)
			{
			  localBond3.select(Me.editmode)
			  Dim bool2 As Boolean = checkBondsNumber(localBond3)
        If (Not bool2)
			  {
				Me.parentK.doUndo()
        Return False
			  }
			}
			((keg.compound.Atom)localObject2).calcImplicitHydrogen()
			((keg.compound.Atom)localObject2).decisideHydrogenDraw()
			localAtom1.calcImplicitHydrogen()
			localAtom1.decisideHydrogenDraw()
		  }
		Next
        If ((Me.editmode.draw = 9) And (((keg.compound.Atom)localObject5).getsp3()) And (localAtom1.getsp3())) 
		{
		  j = 2
		}
 Else
		{
		  j = 1
		}
		If (localObject5 <> localAtom1)
		{
		  ((keg.compound.Molecule)localObject4).combineMol((keg.compound.Atom)localObject5, (keg.compound.Molecule)localObject4, localAtom1, j, k, Me.dispscale, Me.editmode.draw < 6)
		  Dim localBond2 As keg.compound.Bond = ((keg.compound.Atom)localObject5).getBond(localAtom1) 
		  If (localBond2 <> Nothing)
		  {
			localBond2.select(Me.editmode)
			Dim bool1 As Boolean = checkBondsNumber(localBond2)
        If (Not bool1)
			{
			  Me.parentK.doUndo()
        Return False
			}
		  }
		  ((keg.compound.Atom)localObject5).calcImplicitHydrogen()
		  ((keg.compound.Atom)localObject5).decisideHydrogenDraw()
		  localAtom1.calcImplicitHydrogen()
		  localAtom1.decisideHydrogenDraw()
		}
		((keg.compound.Molecule)localObject4).setDBond()
		((keg.compound.Reaction)Me.conteiner).checkOverlapedBracketWithRefine()
		((keg.compound.Reaction)Me.conteiner).autoSetCategory(Me.dispscale)
		repaint()
		Me.editmode.flag_edit = True
        If (Me.parentK <> Nothing)
		{
		  Me.parentK.checkButton()
		}
		Me.editmode.status = 0
        Me.editmode.atom = localAtom1
        Return True
	  }
 
	  Private Boolean containsRingPoligon(java.awt.Point paramPoint, Boolean paramBoolean) 
	  {
		Dim localVector As ArrayList = Nothing
        Dim localAtom1 As keg.compound.Atom = Nothing
        Dim localAtom2 As keg.compound.Atom = Nothing
        Me.ht.Clear()
		localVector = AllAtoms
		localAtom1 = nearAtom(paramPoint, localVector)
		If (localAtom1 = Nothing)
		{
		  Return False
		}
		Me.ht(localAtom1) = True
		localAtom2 = localAtom1
		Dim i As Integer = 0
        Dim j As Integer = 100
        Dim localPolygon As java.awt.Polygon = New java.awt.Polygon()
		localPolygon.addPoint(localAtom1.DX(Me.dispscale), localAtom1.DY(Me.dispscale))
		Dim localObject1 As Object = Nothing
        Dim localAtom3 As keg.compound.Atom
        For localAtom3 = localAtom1 To 8 - 1 Step localAtom3 = localAtom1
		  localVector = getPairAtoms(localAtom3, (keg.compound.Atom)localObject1)
		  localAtom1 = nearAtom(paramPoint, localVector)
		  If (localAtom1 = Nothing)
		  {
			Return False
		  }
		  If (localAtom1.Equals(localAtom2))
		  {
			i = j
		  }
 Else
		  {
			localPolygon.addPoint(localAtom1.DX(Me.dispscale), localAtom1.DY(Me.dispscale))
			i = i + 1
		  }
		  Me.ht(localAtom1.getBond(localAtom3)) = True
        Me.ht(localAtom1) = True
		  localObject1 = localAtom3
		Next
        If ((i = j) And (localPolygon.contains(paramPoInteger.x, paramPoInteger.y)))
		{
		  unselect()
		  Dim localEnumeration As System.Collections.IEnumerator = Me.ht.Keys.GetEnumerator()
        Dim localObject2 As Object
        If (paramBoolean)
		  {
			If (localEnumeration.hasMoreElements())
			{
			  localObject2 = localEnumeration.nextElement()
			  If ((localObject2 Is keg.compound.Atom))
			  {
				((keg.compound.Atom)localObject2).Mol.selectAllItems(Me.editmode, ShrinkMode)
			  }
 ElseIf ((localObject2 Is keg.compound.Bond))
			  {
				((keg.compound.Bond)localObject2).Mol.selectAllItems(Me.editmode, ShrinkMode)
			  }
 Else
			  {
				Console.Error.WriteLine(localObject2.ToString())
			  }
			  repaint()
			}
		  }
 Else
		  {
			While localEnumeration.hasMoreElements()
			  localObject2 = localEnumeration.nextElement()
			  If ((localObject2 Is keg.compound.Atom))
			  {
				((keg.compound.Atom)localObject2).select(Me.editmode)
			  }
 ElseIf ((localObject2 Is keg.compound.Bond))
			  {
				((keg.compound.Bond)localObject2).select(Me.editmode)
			  }
			End While
			repaint()
		  }
		}
		Return False
	  }
 
	  Private ArrayList getPairAtoms(keg.compound.Atom paramAtom1, keg.compound.Atom paramAtom2) 
	  {
		Dim localVector As ArrayList = New ArrayList()
        Dim i As Integer
        For i = 0 To paramAtom1.numBond() - 1 Step i + 1
        Dim localAtom As keg.compound.Atom = paramAtom1.getBond(i).pairAtom(paramAtom1)
        If (Not localAtom.Equals(paramAtom2))
		  {
			localVector.Add(localAtom)
		  }
		Next
        Return localVector
	  }
 
	  Private keg.compound.Atom nearAtom(java.awt.Point paramPoint, ArrayList paramVector) 
	  {
		Dim i As Integer = Integer.MaxValue
        Dim localObject As Object = Nothing
        Dim j As Integer
        For j = 0 To paramVector.Count - 1 Step j + 1
        Dim localAtom As keg.compound.Atom = (keg.compound.Atom)paramVector(j) 
		  If (Not localAtom.Express_group)
		  {
			Dim k As Integer = (localAtom.DX(Me.dispscale) - paramPoInteger.x) * (localAtom.DX(Me.dispscale) - paramPoInteger.x) + (localAtom.DY(Me.dispscale) - paramPoInteger.y) * (localAtom.DY(Me.dispscale) - paramPoInteger.y)
        If (k < i)
			{
			  i = k
			  localObject = localAtom
			}
		  }
		Next
        Return (keg.compound.Atom)localObject
	  }
 
	  Private ReadOnly Property AllAtoms() As ArrayList
            Get
                Dim localVector As ArrayList = New ArrayList()
                Dim i As Integer
                For i = 0 To Me.conteiner.objectNum() - 1 Step i + 1
                    If ((Me.conteiner.getObject(i) Is keg.compound.Molecule)) Then
			  {
				Dim localMolecule As keg.compound.Molecule = (keg.compound.Molecule)Me.conteiner.getObject(i) 
				Dim j As Integer
                        For j = 0 To localMolecule.AtomNum - 1 Step j + 1
                            localVector.Add(localMolecule.getAtom(j + 1))
                        Next
			  }
            Next
                Return localVector
            End Get
        End Property

        Private Double() rotete(Double paramDouble, Double() paramArrayOfDouble) 
	  {
		Dim d1 As Double = Math.Cos(paramDouble)
        Dim d2 As Double = Math.Sin(paramDouble)
        Dim arrayOfDouble() As Double = New Double(2) {}
		arrayOfDouble(0) = (d1 * paramArrayOfDouble(0) - d2 * paramArrayOfDouble(1))
		arrayOfDouble(1) = (d2 * paramArrayOfDouble(0) + d1 * paramArrayOfDouble(1))
		Return arrayOfDouble
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseDragDraw2(java.awt.Graphics paramGraphics) 
	  {
		Dim d1 As Double = Me.mouseDragDraw2_dragP.x
        Dim d2 As Double = Me.mouseDragDraw2_dragP.y
        Dim d3 As Double = Me.mouseDragDraw2_tth
        Dim @bool As Boolean =  Me.mouseDragDraw2_depend 
		Dim d4 As Double = Me.downP.x
        Dim d5 As Double = Me.downP.y
        Dim d6 As Double = d4
        Dim d7 As Double = d5
        Dim i As Integer
        For i = 0 To Me.mouseDragDraw2_n - 1 Step i + 1
        If (i > 0)
		  {
			d3 = 0.0D
			@bool = True
		  }
		  If (i > 0)
		  {
			Select Case Me.editmode.draw
        Case 0

        Case 1

        Case 2

        Case 3

        Case 4

        Case 5

        Case 10

        Dim j As Integer = 1
        If ((j > 1) And (Not @bool)) 
			  {
				j = 1
			  }
			  Select Case j
        Case 1

        Dim arrayOfDouble3() As Double = New Double(2) {}
        Dim arrayOfDouble4() As Double = New Double(2) {}
				arrayOfDouble3(0) = d1
				arrayOfDouble3(1) = d2
				arrayOfDouble4(0) = d6
				arrayOfDouble4(1) = d7
				Dim d9 As Double = arrayOfDouble4(0) - arrayOfDouble3(0)
        Dim d10 As Double = arrayOfDouble4(1) - arrayOfDouble3(1)
        Dim arrayOfDouble5() As Double = New Double(2) {}
        Dim arrayOfDouble6() As Double = New Double(2) {}
        Dim d11 As Double = Math.Sqrt(d9 * d9 + d10 * d10)
				arrayOfDouble3(0) += (-SIN_30DEG * d9 - COS_30DEG * d10) * Me.fixed_length / d11
				arrayOfDouble3(1) += (COS_30DEG * d9 - SIN_30DEG * d10) * Me.fixed_length / d11
				arrayOfDouble3(0) += (-SIN_30DEG * d9 + COS_30DEG * d10) * Me.fixed_length / d11
				arrayOfDouble3(1) += (-COS_30DEG * d9 - SIN_30DEG * d10) * Me.fixed_length / d11
				If (Me.sideFlag)
				{
				  d1 = arrayOfDouble5(0)
				  d2 = arrayOfDouble5(1)
				}
 Else
				{
				  d1 = arrayOfDouble6(0)
				  d2 = arrayOfDouble6(1)
				}
				Me.sideFlag = (Not Me.sideFlag)
        Exit For
        End Select
        Exit For
        Case 6

        Case 7

        Case 8

        Case 9

        Case 11

        Case 12

        Return False
        End Select
		  }
		  If (Me.fixed_length > 0)
		  {
			Dim d8 As Double = Math.Sqrt((d4 - d1) * (d4 - d1) + (d5 - d2) * (d5 - d2))
        Dim arrayOfDouble1() As Double = {(d1 - d4) * Me.fixed_length / d8, (d2 - d5) * Me.fixed_length / d8}

        Dim arrayOfDouble2() As Double = rotete(d3, arrayOfDouble1)
			d1 = d4 + arrayOfDouble2(0)
			d2 = d5 + arrayOfDouble2(1)
		  }
		  paramGraphics.Color = Color.gray
		  drawLine(paramGraphics, (Integer)d1, (Integer)d2, (Integer)d4, (Integer)d5)
		  d6 = d4
		  d7 = d5
		  d4 = d1
		  d5 = d2
		Next
		drawString(paramGraphics, Convert.ToString(Me.mouseDragDraw2_n + 1), (Integer)d4 + 3, (Integer)d5 + 3)
		Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseUpErase(Integer paramInt1, Integer paramInt2) 
	  {
		Dim localAtom As keg.compound.Atom = Me.conteiner.nearAtom(Me.upP.x, Me.upP.y, Me.dispscale, Me.tolerance)
        Me.editmode.status = 0
        Me.editmode.atom = ((keg.compound.Atom)Nothing)
		Me.editmode.bond = ((keg.compound.Bond)Nothing)
		Me.editmode.chemobject = ((keg.compound.ChemObject)Nothing)
		Dim localMolecule As keg.compound.Molecule
        If (localAtom <> Nothing)
		{
		  Me.editmode.UNDO = (keg.compound.Reaction)Me.conteiner
		  localMolecule = localAtom.Mol
		  localMolecule.deleteAtom(localAtom)
		  localMolecule.refine()
		  ((keg.compound.Reaction)Me.conteiner).calcImplicitHydrogen()
		  ((keg.compound.Reaction)Me.conteiner).decisideHydrogenDraw()
		  Me.editmode.flag_edit = True
		}
 Else
		{
		  Dim localBond As keg.compound.Bond = Me.conteiner.nearBond(Me.upP.x, Me.upP.y, Me.dispscale, Me.tolerance)
        If (localBond <> Nothing)
		  {
			Me.editmode.UNDO = (keg.compound.Reaction)Me.conteiner
			Select Case localBond.Order
        Case -1

        Case 0

        Case 1

        Case 4 : 

			  localMolecule = localBond.Mol
			  localMolecule.deleteBond(localBond)
			  localMolecule.refine()
			  Exit For
        Case 2 : 

			  localBond.Order = 1
			  Exit For
        Case 3 : 

			  localBond.Order = 2
		  Exit For
        End Select
			((keg.compound.Reaction)Me.conteiner).calcImplicitHydrogen()
			((keg.compound.Reaction)Me.conteiner).decisideHydrogenDraw()
			Me.editmode.flag_edit = True
		  }
 Else
		  {
			Dim localBracket As keg.compound.Bracket = ((keg.compound.Reaction)Me.conteiner).nearBracket(Me.upP.x, Me.upP.y, Me.dispscale, Me.tolerance) 
			If (localBracket <> Nothing)
			{
			  Me.editmode.UNDO = (keg.compound.Reaction)Me.conteiner
			  ((keg.compound.Reaction)Me.conteiner).delBracket(localBracket)
			}
 Else
			{
			  localBracket = ((keg.compound.Reaction)Me.conteiner).nearBracketLabel(Me.upP.x, Me.upP.y, Me.dispscale, 1)
			  If (localBracket <> Nothing)
			  {
				Me.editmode.UNDO = (keg.compound.Reaction)Me.conteiner
				((keg.compound.Reaction)Me.conteiner).delBracket(localBracket)
			  }
			}
		  }
		}
		((keg.compound.Reaction)Me.conteiner).checkOverlapedBracketWithRefine()
		((keg.compound.Reaction)Me.conteiner).autoSetCategory(Me.dispscale)
		repaint()
		If (Me.parentK <> Nothing)
		{
		  Me.parentK.checkButton()
		}
		Me.editmode.status = 0
        Return True
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseUpMove(Integer paramInt1, Integer paramInt2, java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		Dim localObject1 As Object = Nothing
        Dim localObject2 As Object = Nothing
        Dim localArrayList As ArrayList = New ArrayList()
        Dim localObject3 As Object
        Dim i As Integer
        For i = 0 To Me.editmode.selected.Count - 1 Step i + 1
		  localObject3 = Me.editmode.selected(i)
		  If ((localObject3 Is keg.compound.Atom))
		  {
			Dim localAtom1 As keg.compound.Atom = (keg.compound.Atom)localObject3 
			localAtom1.moveInternal(Me.upP.x - Me.downP.x, Me.upP.y - Me.downP.y, Me.dispscale)
			If (ShrinkMode)
			{
			  If ((localAtom1.Express_group) And (Not paramMouseEvent.AltDown))
			  {
				Dim j As Integer
        For j = 0 To localAtom1.GroupAtomSize - 1 Step j + 1
        Dim localAtom3 As keg.compound.Atom = localAtom1.getGroupAtom(j)
				  localAtom3.moveInternal(Me.upP.x - Me.downP.x, Me.upP.y - Me.downP.y, Me.dispscale)
				Next
			  }
			}
 ElseIf ((Not paramMouseEvent.AltDown) And (localAtom1.Mol.getExpressionAtomWithGroupedAtom(localAtom1) <> Nothing))
			{
			  localAtom2 = localAtom1.Mol.getExpressionAtomWithGroupedAtom(localAtom1)
			  If (Not localArrayList.Contains(localAtom2))
			  {
				localArrayList.Add(localAtom2)
			  }
			}
			Dim localAtom2 As keg.compound.Atom = getTheConnectionNeededAtom(localAtom1)
        If (localAtom2 <> Nothing)
			{
			  If (((localAtom2.Express_group) And (Not ShrinkMode)) Or ((Not localAtom2.NonGroupedAtom) And (ShrinkMode)))
			  {
				Continue Do
			  }
			  localObject1 = localAtom2
			  localObject2 = localAtom1
			}
		  }
 ElseIf ((localObject3 Is keg.compound.ReactionArrow))
		  {
			((keg.compound.ReactionArrow)localObject3).move(Me.upP.x - Me.downP.x, Me.upP.y - Me.downP.y)
		  }
 ElseIf ((localObject3 Is keg.compound.Bracket))
		  {
			((keg.compound.Bracket)localObject3).moveInternaldependSelected(Me.upP.x - Me.downP.x, Me.upP.y - Me.downP.y, Me.dispscale)
		  }
		Next
        For i = 0 To localArrayList.Count - 1 Step i + 1
		  localObject3 = (keg.compound.Atom)localArrayList(i)
		  ((keg.compound.Atom)localObject3).moveInternal(Me.upP.x - Me.downP.x, Me.upP.y - Me.downP.y, Me.dispscale)
		Next
        If ((localObject1 <> Nothing) And (localObject2 <> Nothing))
		{
		  Me.upP.x = (((keg.compound.Atom)localObject1).DX(Me.dispscale) - ((keg.compound.Atom)localObject2).DX(Me.dispscale) + Me.downP.x)
		  Me.upP.y = (((keg.compound.Atom)localObject1).DY(Me.dispscale) - ((keg.compound.Atom)localObject2).DY(Me.dispscale) + Me.downP.y)
		  For i = 0 To Me.editmode.selected.Count - 1 Step i + 1
			localObject3 = Me.editmode.selected(i)
			If ((localObject3 Is keg.compound.Atom))
			{
			  ((keg.compound.Atom)localObject3).moveInternal(Me.upP.x - Me.downP.x, Me.upP.y - Me.downP.y, Me.dispscale)
			}
 ElseIf ((localObject3 Is keg.compound.ReactionArrow))
			{
			  ((keg.compound.ReactionArrow)localObject3).move(Me.upP.x - Me.downP.x, Me.upP.y - Me.downP.y)
			}
 ElseIf ((localObject3 Is keg.compound.Bracket))
			{
			  ((keg.compound.Bracket)localObject3).moveInternal(Me.upP.x - Me.downP.x, Me.upP.y - Me.downP.y, Me.dispscale)
			}
		  Next
		  ((keg.compound.Atom)localObject1).Mol.combineMol((keg.compound.Atom)localObject1, (keg.compound.Atom)localObject2, Me.dispscale)
		  Dim @bool As Boolean =  checkBondsNumber((keg.compound.Atom)localObject1) 
		  If (Not @bool) 
		  {
			Me.parentK.doUndo()
        Me.imageForMove = Nothing
        Return False
		  }
		  ((keg.compound.Atom)localObject1).Mol.resetOpoint(Me.dispscale)
		}
		((keg.compound.Reaction)Me.conteiner).checkOverlapedBracketWithRefine()
		Me.editmode.resetArea()
		((keg.compound.Reaction)Me.conteiner).autoSetCategory(Me.dispscale)
		repaint()
		Me.editmode.flag_edit = True
        If (Me.parentK <> Nothing)
		{
		  Me.parentK.checkButton()
		}
		Me.editmode.status = 0
        Me.imageForMove = Nothing
        Return False
	  }
 
	  Private keg.compound.Atom getTheConnectionNeededAtom(keg.compound.Atom paramAtom) 
	  {
		Dim localAtom As keg.compound.Atom = Nothing
        Dim i As Integer
        For i = 0 To Me.conteiner.objectNum() - 1 Step i + 1
        If ((Me.conteiner.getObject(i) Is keg.compound.ChemConteiner))
		  {
			localAtom = ((keg.compound.ChemConteiner)Me.conteiner.getObject(i)).nearAtom(paramAtom.DX(Me.dispscale), paramAtom.DY(Me.dispscale), Me.dispscale, Me.tolerance)
			If ((localAtom <> Nothing) And (Not localAtom.Equals(paramAtom)))
			{
			  Return localAtom
			}
		  }
		Next
        Return Nothing
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseUpSelect(Integer paramInt1, Integer paramInt2, java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		Dim @bool As Boolean =  paramMouseEvent.ShiftDown 
		Dim localGraphics As java.awt.Graphics = Graphics
        If ((Math.Abs(Me.upP.x - Me.downP.x) < Me.tolerance) And (Math.Abs(Me.upP.y - Me.downP.y) < Me.tolerance))
		{
		  mouseClickSelect(paramMouseEvent)
		}
 ElseIf ((Me.editmode.select_mode = 0) Or (Me.editmode.operation = 8))
		{
		  localGraphics.setPaintMode()
		  localGraphics.Color = Me.select_frameC
		  drawLine(localGraphics, Me.upP.x, Me.upP.y, Me.prevDragP.x, Me.prevDragP.y)
		  Me.lasso_points.Add(New DblRect(Me.upP.x, Me.upP.y))
        Me.conteiner.selectItems(Me.lasso_points, Me.dispscale, Me.editmode, @bool, ShrinkMode)
        Me.lasso_points.Clear()
		}
 Else
		{
		  Me.conteiner.selectItems(Math.Min(Me.downP.x, Me.upP.x), Math.Min(Me.downP.y, Me.upP.y), Math.Max(Me.downP.x, Me.upP.x), Math.Max(Me.downP.y, Me.upP.y), Me.dispscale, Me.editmode, @bool, ShrinkMode)
		}
		((keg.compound.Reaction)Me.conteiner).checkOverlapedBracketWithRefine()
		Me.editmode.resetArea()
		repaint()
		If (Me.parentK <> Nothing)
		{
		  Me.parentK.checkButton()
		}
		Me.editmode.status = 0
        Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseClickSelect(java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		Dim bool1 As Boolean = paramMouseEvent.ShiftDown
        Dim bool2 As Boolean = paramMouseEvent.MetaDown
        If (Not paramMouseEvent.MetaDown)
		{
		  If (Me.editmode.atom <> Nothing)
		  {
			If (bool1)
			{
			  Me.editmode.atom.select_reverse(Me.editmode)
			}
 Else
			{
			  unselect()
			  Me.editmode.atom.select(Me.editmode)
			}
		  }
 ElseIf (Me.editmode.bond <> Nothing)
		  {
			If (bool1)
			{
			  Me.editmode.bond.select_reverse(Me.editmode)
			}
 Else
			{
			  unselect()
			  Me.editmode.bond.select(Me.editmode)
			}
		  }
 ElseIf (Me.editmode.chemobject <> Nothing)
		  {
			If (bool1)
			{
			  Me.editmode.chemObject.select_reverse(Me.editmode)
			}
 ElseIf ((Me.editmode.chemobject Is keg.compound.Bracket))
			{
			  ((keg.compound.Bracket)Me.editmode.chemobject).select(Me.editmode, Me.upP.x, Me.upP.y, Me.dispscale, Me.tolerance)
			}
 Else
			{
			  Me.editmode.chemObject.select(Me.editmode)
			}
		  }
 Else
		  {
			containsRingPoligon(New java.awt.Point(Me.upP.x,Me.upP.y),bool2)
		  }
		}
 Else
		{
		  containsRingPoligon(New java.awt.Point(Me.upP.x,Me.upP.y),bool2)
		}
		Me.editmode.resetArea()
		repaint()
		If (Me.parentK <> Nothing)
		{
		  Me.parentK.checkButton()
		}
		Me.editmode.status = 0
        Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseUpBracket(Integer paramInt1, Integer paramInt2) 
	  {
		unselect()
		If (Me.editmode.status = 6)
		{
		  Return False
		}
		Dim localGraphics As java.awt.Graphics = Graphics
        Me.editmode.UNDO = (keg.compound.Reaction)Me.conteiner
		Dim localDimension As DblRect = ((keg.compound.Reaction)Me.conteiner).Coordinate 
		If (Math.Abs(Me.upP.x - Me.downP.x) < Me.tolerance)
		{
		  Return False
		}
		If (Math.Abs(Me.upP.x - Me.downP.x) < Me.dispscale)
		{
		  Return False
		}
		If (Math.Abs(Me.upP.y - Me.downP.y) < Me.tolerance)
		{
		  Return False
		}
		If (Math.Abs(Me.upP.y - Me.downP.y) < Me.dispscale)
		{
		  Return False
		}
		Dim i As Integer = Math.Min(Me.downP.x, Me.upP.x)
        Dim j As Integer = Math.Max(Me.downP.x, Me.upP.x)
        Dim k As Integer = Math.Min(Me.downP.y, Me.upP.y)
        Dim m As Integer = Math.Max(Me.downP.y, Me.upP.y)
        Dim localVector2D1 As Vector2D = New Vector2D((i - localDimension.width) / Me.dispscale, (k - localDimension.height) / Me.dispscale)
        Dim localVector2D2 As Vector2D = New Vector2D((j - localDimension.width) / Me.dispscale, (m - localDimension.height) / Me.dispscale)
        Dim localVector2D3 As Vector2D = New Vector2D((j - localDimension.width + 8D) / Me.dispscale, (m - localDimension.height - 8D) / Me.dispscale)
        Dim localBracket As keg.compound.Bracket = New keg.compound.Bracket((keg.compound.Reaction)Me.conteiner, localVector2D1, localVector2D2, localVector2D3)
		localBracket.select(Me.editmode)
		localBracket.set0point(localDimension)
		localGraphics.Font = Me.bFont
		Dim localFontMetrics As java.awt.FontMetrics = localGraphics.FontMetrics
		localBracket.Size = New DblRect(localFontMetrics.stringWidth(localBracket.Label), localFontMetrics.Height - Me.fHeight_discount)
		((keg.compound.Reaction)Me.conteiner).addBracket(localBracket)
		((keg.compound.Reaction)Me.conteiner).checkOverlapedBracketWithRefine()
		Me.editmode.resetArea()
		repaint()
		Me.editmode.status = 0
        Me.editmode.flag_edit = True
        If (Me.parentK <> Nothing)
		{
		  Me.parentK.checkButton()
		}
		Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseUpRotate(Integer paramInt1, Integer paramInt2) 
	  {
		Dim localObject As Object
        If ((Me.editmode.operation = 8) And (Me.rfr.Selected))
		{
		  localObject = Me.conteiner.nearAtom(Me.upP.x, Me.upP.y, Me.dispscale, Me.rtrTolerance)
		  If (localObject <> Nothing)
		  {
			Me.cx = ((keg.compound.Atom)localObject).DX(Me.dispscale)
			Me.cy = ((keg.compound.Atom)localObject).DY(Me.dispscale)
		  }
 Else
		  {
			Me.cx = Me.upP.x
        Me.cy = Me.upP.y
		  }
		  Me.rfr.x = Me.cx
        Me.rfr.y = Me.cy
		  repaint()
		  Return False
		}
		Dim localMolecule As keg.compound.Molecule = Nothing
        Dim localVector2D As Vector2D = New Vector2D(Me.upP.x - Me.cx, Me.upP.y - Me.cy)
        Dim d As Double = VecMath2D.angle(Me.vec0, localVector2D)
        Dim * As d =  -1.0D 
		Dim i As Integer
        For i = 0 To Me.editmode.selected.Count - 1 Step i + 1
		  localObject = Me.editmode.selected(i)
		  If ((localObject Is keg.compound.Atom))
		  {
			If (localMolecule <> ((keg.compound.Atom)localObject).Mol) 
			{
			  localMolecule = ((keg.compound.Atom)localObject).Mol
			  localMolecule.set0pointTemp(Me.cx, -Me.cy, Me.dispscale)
			}
			((keg.compound.Atom)localObject).rotate(localMolecule.XpointTemp, localMolecule.YpointTemp, -d)
		  }
		  If ((localObject Is keg.compound.ReactionArrow))
		  {
			((keg.compound.ReactionArrow)localObject).rotate(Me.cx, Me.cy, d)
		  }
		  If ((localObject Is keg.compound.Bracket))
		  {
			((keg.compound.Bracket)localObject).rotateDependSelected(Me.cx + Me.h0, Me.cy + Me.v0, d, Me.dispscale)
		  }
		Next
		((keg.compound.Reaction)Me.conteiner).checkOverlapedBracketWithRefine()
		Me.editmode.resetArea()
		((keg.compound.Reaction)Me.conteiner).autoSetCategory(Me.dispscale)
		((keg.compound.Reaction)Me.conteiner).calcImplicitHydrogen()
		((keg.compound.Reaction)Me.conteiner).decisideHydrogenDraw()
		repaint()
		Me.editmode.flag_edit = True
        If (Me.parentK <> Nothing)
		{
		  Me.parentK.checkButton()
		}
		Me.editmode.status = 0
        Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseUpResize(Integer paramInt1, Integer paramInt2) 
	  {
		Dim localMolecule As keg.compound.Molecule = Nothing
        Dim i As Integer = Me.editmode.select_area.width + Me.tolerance * 2 + 1 + Me.handlesize
        Dim j As Integer = Me.editmode.select_area.height + Me.tolerance * 2
        Dim k As Integer = Me.editmode.select_area.width + Me.tolerance * 2 + (Me.upP.x - Me.downP.x) + 1 + Me.handlesize
        Dim m As Integer = Me.editmode.select_area.height + Me.tolerance * 2 + (Me.upP.y - Me.downP.y)
        If (k < 1 + Me.handlesize)
		{
		  k = 1 + Me.handlesize
		}
		If (m < 1 + Me.handlesize * 2)
		{
		  m = 1 + Me.handlesize * 2
		}
		Dim d1 As Double = k / i
        Dim d2 As Double = m / j
        Dim n As Integer
        For n = 0 To Me.editmode.selected.Count - 1 Step n + 1
        Dim localObject As Object = Me.editmode.selected(n)
        If ((localObject Is keg.compound.Atom))
		  {
			If (localMolecule <> ((keg.compound.Atom)localObject).Mol) 
			{
			  localMolecule = ((keg.compound.Atom)localObject).Mol
			  localMolecule.set0pointTemp(Me.editmode.select_area.x - Me.tolerance, -(Me.editmode.select_area.y - Me.tolerance), Me.dispscale)
			}
			((keg.compound.Atom)localObject).resize(localMolecule.XpointTemp, localMolecule.YpointTemp, d1, d2)
		  }
		  If ((localObject Is keg.compound.ReactionArrow))
		  {
			((keg.compound.ReactionArrow)localObject).resize(Me.editmode.select_area.x - Me.tolerance, Me.editmode.select_area.y - Me.tolerance, d1, d2)
		  }
		  If ((localObject Is keg.compound.Bracket))
		  {
			((keg.compound.Bracket)localObject).resizeDependSelected(Me.editmode.select_area.x - Me.tolerance, Me.editmode.select_area.y - Me.tolerance, d1, d2, Me.dispscale)
		  }
		Next
		((keg.compound.Reaction)Me.conteiner).checkOverlapedBracketWithRefine()
		Me.editmode.resetArea()
		((keg.compound.Reaction)Me.conteiner).autoSetCategory(Me.dispscale)
		repaint()
		Me.editmode.flag_edit = True
        Me.editmode.operation = 3
        If (Me.parentK <> Nothing)
		{
		  Me.parentK.checkButton()
		}
		Me.editmode.status = 0
        Return False
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        internal virtual Boolean mouseUpArrow(Integer paramInt1, Integer paramInt2) 
	  {
		unselect()
		Dim i As Integer
        Dim j As Integer
        If (Math.Abs(Me.upP.x - Me.downP.x) < Math.Abs(Me.upP.y - Me.downP.y))
		{
		  Me.upP.x = Me.downP.x
        If (Me.upP.y > Me.downP.y)
		  {
			i = 3
			j = Me.upP.y - Me.downP.y
		  }
 Else
		  {
			i = 1
			j = Me.downP.y - Me.upP.y
		  }
		}
 Else
		{
		  Me.upP.y = Me.downP.y
        If (Me.upP.x > Me.downP.x)
		  {
			i = 0
			j = Me.upP.x - Me.downP.x
		  }
 Else
		  {
			i = 2
			j = Me.downP.x - Me.upP.x
		  }
		}
		Dim localReactionArrow As keg.compound.ReactionArrow = New keg.compound.ReactionArrow(Me.downP.x, Me.downP.y, i, j)
		localReactionArrow.select(Me.editmode)
		((keg.compound.Reaction)Me.conteiner).addObject(localReactionArrow, 0)
		((keg.compound.Reaction)Me.conteiner).autoSetCategory(Me.dispscale)
		repaint()
		Me.editmode.flag_edit = True
        Me.editmode.operation = 3
        If (Me.parentK <> Nothing)
		{
		  Me.parentK.checkButton()
		}
		Me.editmode.status = 0
        Return False
	  }
 
	  Public virtual Boolean action(java.awt.Event paramEvent, Object paramObject) 
	  {
		Dim localGraphics As java.awt.Graphics = Graphics
        If (paramEvent.target = Me.element_text)
		{
		  setAtomLabel(Me.element_text.Text, Me.lFont, Me.foreC)
		  Me.editmode.flag_edit = True
        Me.element_text.hide()
        Me.element_text.resize(0, 0)
        Me.parentK.canvas.requestFocus()
		  repaint()
		  If (Me.parentK <> Nothing)
		  {
			Me.parentK.checkButton()
		  }
		}
 ElseIf ((paramEvent.target Is java.awt.MenuItem))
		{
		  Dim i As Integer
        For i = 0 To Me.element_words.Length - 1 Step i + 1
        If (paramEvent.target = Me.element_words(i))
			{
			  Me.editmode.status = 0
        If (Me.element_words(i).Label.Equals("other"))
			  {
				If (Me.editmode.atom <> Nothing)
				{
				  Me.element_text.Text = Me.editmode.atom.Label
				}
 Else
				{
				  Me.element_text.Text = ""
				}
				Me.element_text.resize(Me.element_text.preferredSize())
        Me.element_text.move(Me.menuX, Me.menuY)
        Me.element_text.show()
        Me.element_text.requestFocus()
			  }
 Else
			  {
				setAtomLabel(Me.element_words(i).Label, Me.lFont, Me.foreC)
				Me.editmode.flag_edit = True
				repaint()
			  }
			  Me.editmode.flag_edit = True
        If (Me.parentK <> Nothing)
			  {
				Me.parentK.checkButton()
			  }
			  Return False
			}
		  Next
        For i = 0 To Me.element_words1.Length - 1 Step i + 1
        If (paramEvent.target = Me.element_words1(i))
			{
			  Me.editmode.status = 0
			  setAtomLabel(Me.element_words1(i).Label, Me.lFont, Me.foreC)
			  repaint()
			  Me.editmode.flag_edit = True
        If (Me.parentK <> Nothing)
			  {
				Me.parentK.checkButton()
			  }
			  Return False
			}
		  Next
        For i = 0 To Me.element_words2.Length - 1 Step i + 1
        If (paramEvent.target = Me.element_words2(i))
			{
			  Me.editmode.status = 0
			  setAtomLabel(Me.element_words2(i).Label, Me.lFont, Me.foreC)
			  repaint()
			  Me.editmode.flag_edit = True
        If (Me.parentK <> Nothing)
			  {
				Me.parentK.checkButton()
			  }
			  Return False
			}
		  Next
		}
		If (paramEvent.target = Me.bracket_text)
		{
		  Me.editmode.status = 0
        Dim str As String = Me.bracket_text.Text
        If ((Me.editmode.bracket Is keg.compound.Bracket))
		  {
			Me.editmode.UNDO = (keg.compound.Reaction)Me.conteiner
			Dim localBracket As keg.compound.Bracket = Me.editmode.bracket
			localBracket.Label = str
			localGraphics.Font = Me.bFont
			Dim localFontMetrics As java.awt.FontMetrics = localGraphics.FontMetrics
			localBracket.Size = New DblRect(localFontMetrics.stringWidth(str), localFontMetrics.Height - Me.fHeight_discount)
		  }
		  Me.bracket_text.hide()
        Me.bracket_text.resize(0, 0)
		  repaint()
		}
		Return False
	  }
 
	  Public virtual void cancelAtomLabel() 
	  {
		Me.editmode.status = 0
        Me.element_text.hide()
        Me.element_text.resize(0, 0)
        Me.bracket_text.hide()
        Me.bracket_text.resize(0, 0)
	  }
 
	  internal virtual void setAtomLabel(keg.compound.Atom paramAtom, String paramString) 
	  {
		setAtomLabel(paramAtom, paramString, Me.lFont, Me.foreC)
		Me.editmode.flag_edit = True
        If (paramAtom <> Nothing)
		{
		  Dim @bool As Boolean =  checkBondsNumber(paramAtom) 
		  If (Not @bool) 
		  {
			Me.parentK.doUndo()
		  }
		}
	  }
 
	  internal virtual void setAtomLabel(String paramString, java.awt.Font paramFont, Color paramColor) 
	  {
		setAtomLabel(Me.editmode.atom, paramString, paramFont, paramColor)
	  }
 
	  Private void setAtomLabel(keg.compound.Atom paramAtom, String paramString, java.awt.Font paramFont, Color paramColor) 
	  {
		Me.editmode.status = 0
        Me.editmode.UNDO = (keg.compound.Reaction)Me.conteiner
		If (paramString.Equals(" "))
		{
		  Return
		}
		Me.editmode.atom_label = paramString
        If (paramAtom <> Nothing)
		{
		  paramAtom.FullLabel = paramString
		  paramAtom.calcImplicitHydrogen()
		  paramAtom.decisideHydrogenDraw()
		  paramAtom.Mol.resetKEGGAtomName()
		  paramAtom.select(Me.editmode)
		  Me.editmode.selected.Add(Me.editmode.atom)
		}
 Else
		{
		  Dim localMolecule As keg.compound.Molecule = New keg.compound.Molecule()
        Dim localAtom As keg.compound.Atom = New keg.compound.Atom(localMolecule, 0.0D, 0.0D, 0.0D, "")
		  localMolecule.addAtom(localAtom)
		  localMolecule.set0point(New DblRect(Me.downP.x,Me.downP.y))
		  localAtom.FullLabel = Me.editmode.atom_label
		  localAtom.calcImplicitHydrogen()
		  localAtom.decisideHydrogenDraw()
		  ((keg.compound.Reaction)Me.conteiner).addObject(localMolecule, 0)
		  localAtom.select(Me.editmode)
		  Me.editmode.selected.Add(localAtom)
		}
		CompoundPanel.setAbsoluteCoord((keg.compound.Reaction)Me.conteiner, True, Dispscale)
	  }
 
	  Public virtual void setCategory() 
	  {
		If (Me.editmode.operation = 3)
		{
		  Dim i As Integer
        For i = 0 To Me.editmode.selected.Count - 1 Step i + 1
        Dim localObject As Object = Me.editmode.selected(i)
        If ((localObject Is keg.compound.Atom))
			{
			  ((keg.compound.Reaction)Me.conteiner).setCategory(((keg.compound.Atom)localObject).Mol, Me.editmode.category)
			}
		  Next
		  repaint()
		}
	  }
 
	  Public virtual void showTextPreferenceDialog(keg.compound.Text paramText, java.awt.Point paramPoint) 
	  {
		If (Me.textDialog = Nothing)
		{
		  Me.textDialog = New TextPreferenceDialog(Me)
		}
		Me.textDialog.show(paramText, paramPoint)
	  }
 
	  Private keg.compound.Text onText(java.awt.Point paramPoint) 
	  {
		Return ((keg.compound.Reaction)Me.conteiner).onText(paramPoint)
	  }
 
	  Private void paintTexts(java.awt.Graphics paramGraphics, ArrayList paramVector) 
	  {
		Dim localFont As java.awt.Font = paramGraphics.Font
        Dim i As Integer
        For i = 0 To paramVector.Count - 1 Step i + 1
        Dim localText As keg.compound.Text = (keg.compound.Text)paramVector(i) 
		  If ((localText.Text <> Nothing) And (localText.Text.Length <> 0))
		  {
			If (localText.Font = Nothing)
			{
			  localText.Font = getFont(localText)
			}
			paramGraphics.Font = localText.Font
			paramGraphics.Color = localText.Color
			paramGraphics.drawString(localText.Text, localText.X, localText.Y)
			paramGraphics.Color = Color.gray
			Dim localFontMetrics As java.awt.FontMetrics = paramGraphics.FontMetrics
        Dim localRectangle As Rectangle = localFontMetrics.getStringBounds(localText.Text, paramGraphics).Bounds
			localRectangle.x = localText.X
			localRectangle.y = localText.Y
			Dim - As localRectangle.y =  localRectangle.height 
			localText.Bounds = localRectangle
			If ((Me.selectedText <> Nothing) And (Me.selectedText.Equals(localText)))
			{
			  paramGraphics.drawRect(localRectangle.x, localRectangle.y, localRectangle.width, localRectangle.height)
			}
		  }
		Next
		paramGraphics.Font = localFont
	  }
 
	  Public virtual void print(java.awt.Graphics paramGraphics) 
	  {
		Foreground = Me.foreC
		Background = Me.backC
		paramGraphics.setPaintMode()
		Dim i As Integer = Voffset
        Dim j As Integer = Hoffset
        If ((Me.conteiner Is keg.compound.Reaction))
		{
		  paintReaction(paramGraphics, (keg.compound.Reaction)Me.conteiner)
		}
		Voffset = i
		Hoffset = j
	  }
 
	  Public virtual void paint(java.awt.Graphics paramGraphics) 
	  {
		update(paramGraphics)
	  }
 
	  Public virtual void update(java.awt.Graphics paramGraphics) 
	  {
		Dim localDimension As DblRect = Size
        Dim localRectangle1 As Rectangle = ((keg.compound.Reaction)Me.conteiner).getBound(Me.dispscale) 
		If (localRectangle1.width > localDimension.width)
		{
		  localDimension.width = (localRectangle1.width + 10)
		}
		If (localRectangle1.height > localDimension.height)
		{
		  localDimension.height = (localRectangle1.height + 10)
		}
		Dim localGraphics As java.awt.Graphics = Nothing
		Foreground = Me.foreC
		Background = Me.backC
		Dim localImage As java.awt.Image = createImage(localDimension.width, localDimension.height)
        If (localImage <> Nothing)
		{
		  localGraphics = localImage.Graphics
		}
		If (localGraphics = Nothing)
		{
		  localGraphics = paramGraphics
		  localImage = Nothing
		}
		localGraphics.Color = IIf( ShrinkMode ,  Me.shrinkBackC ,  Me.backC)

		localGraphics.fillRect(0, 0, localDimension.width, localDimension.height)
		localGraphics.setPaintMode()
		If ((Me.conteiner Is keg.compound.Reaction))
		{
		  paintReaction(localGraphics, (keg.compound.Reaction)Me.conteiner)
		  Dim localRectangle2 As Rectangle = ((keg.compound.Reaction)Me.conteiner).getBound(Me.dispscale) 
		  paintTexts(localGraphics, ((keg.compound.Reaction)Me.conteiner).Texts)
		}
		If (Me.mouseDragDraw2_draw)
		{
		  mouseDragDraw2(localGraphics)
		  Me.mouseDragDraw2_draw = False
		}
		If (Me.eventmode)
		{
		  If (((Me.editmode.operation = 3) Or (Me.editmode.operation = 8)) And (Me.editmode.selected.Count > 0))
		  {
			localGraphics.Color = Me.select_frameC
			localGraphics.XORMode = Me.backC
			If ((Me.editmode.select_area.width <> 0) Or (Me.editmode.select_area.height <> 0))
			{
			  drawRect(localGraphics, Me.editmode.select_area.x - Me.tolerance, Me.editmode.select_area.y - Me.tolerance, Me.editmode.select_area.width + Me.tolerance * 2 + 1 + Me.handlesize, Me.editmode.select_area.height + Me.tolerance * 2)
			  If (Me.editmode.operation <> 8)
			  {
				fillRect(localGraphics, Me.editmode.select_area.x + Me.editmode.select_area.width + Me.tolerance + 1, Me.editmode.select_area.y - Me.tolerance + 1, Me.handlesize, Me.handlesize)
				fillRect(localGraphics, Me.editmode.select_area.x + Me.editmode.select_area.width + Me.tolerance + 1, Me.editmode.select_area.y + Me.editmode.select_area.height + Me.tolerance - Me.handlesize, Me.handlesize, Me.handlesize)
			  }
			}
		  }
		  If (Me.editmode.operation = 8)
		  {
			If (Me.rfr.Selected)
			{
			  localGraphics.Color = Me.highC
			  localGraphics.XORMode = Me.backC
			}
 Else
			{
			  localGraphics.Color = Me.foreC
			  localGraphics.setPaintMode()
			}
			drawOval(localGraphics, Me.rfr.x - 5, Me.rfr.y - 5, 10, 10)
			drawLine(localGraphics, Me.rfr.x, Me.rfr.y - 4, Me.rfr.x, Me.rfr.y + 4)
			drawLine(localGraphics, Me.rfr.x - 4, Me.rfr.y, Me.rfr.x + 4, Me.rfr.y)
		  }
		}
		If (localImage <> Nothing)
		{
		  paramGraphics.drawImage(localImage, 0, 0, Me)
		}
		If (Me.markFlag_Atom <> Nothing)
		{
		  paintAtomMark(paramGraphics, Me.markFlag_Atom)
		  Me.markFlag_Atom = Nothing
		}
		If (Me.markFlag_Bond <> Nothing)
		{
		  paintBondMark(paramGraphics, Me.markFlag_Bond)
		  Me.markFlag_Bond = Nothing
		}
		If (Me.markFlag_Bracket <> Nothing)
		{
		  paintBracketMark(paramGraphics, Me.markFlag_Bracket)
		  Me.markFlag_Bracket = Nothing
		}
		If (Me.markFlag_BracketLabel <> Nothing)
		{
		  paintBracketLabelMark(paramGraphics, Me.markFlag_BracketLabel)
		  Me.markFlag_BracketLabel = Nothing
		}
		If (Me.markFlag_ReactionArrow <> Nothing)
		{
		  paintReactionArrowMark(paramGraphics, Me.markFlag_ReactionArrow)
		  Me.markFlag_ReactionArrow = Nothing
		}
		If (Me.parentK <> Nothing)
		{
		  localDimension = Size
		  Dim i As Integer = localDimension.width * 2
        If (localRectangle1.x + localRectangle1.width / 2 > i)
		  {
			i = localRectangle1.x + localRectangle1.width + 20
		  }
		  If (i > 1920)
		  {
			i = 1920
		  }
		  Dim j As Integer = localDimension.height * 2
        If (localRectangle1.y + localRectangle1.height > j)
		  {
			j = localRectangle1.y + localRectangle1.height + 20
		  }
		  If (j > 1200)
		  {
			j = 1200
		  }
		  setVirtualScreen(i, j)
		  Me.parentK.hscroll.setValues(Me.h0, localDimension.width, 0, i)
        Me.parentK.vscroll.setValues(Me.v0, localDimension.height, 0, j)
		}
	  }
 
	  Private void drawGrid(java.awt.Graphics paramGraphics, DblRect paramDimension) 
	  {
		Dim localColor1 As Color = Color.gray
        Dim localColor2 As Color = Color.gray
        Dim n As Integer = 0
        Dim i1 As Integer = 0
        Dim i2 As Integer = 10
		paramGraphics.Color = localColor1
		Dim j As Integer
        Dim m As Integer
        Dim i As Integer
        Dim k As Integer
        Dim i3 As Integer
        Select Case n
        Case 0 : 

		  j = 0
		  m = paramDimension.height
		  i = Me.h0 / i2 * i2 - Me.h0
		  While i < Me.h0 + paramDimension.width
			paramGraphics.drawLine(i, j, i, m)
			i += i2
		  End While
		  i = 0
		  k = paramDimension.width
		  j = Me.v0 / i2 * i2 - Me.v0
	  GoTo case 1
		Case 1

        Case 2

        While j < Me.v0 + paramDimension.height
			paramGraphics.drawLine(i, j, k, j)
			j += i2
			Continue Do
			i = Me.h0 / i2 * i2 - Me.h0
			While i < Me.h0 + paramDimension.width
			  j = Me.v0 / i2 * i2 - Me.v0
			  While j < Me.v0 + paramDimension.height
				paramGraphics.drawLine(i, j, i, j)
				j += i2
			  End While
			  i += i2
			  Continue Do
			  i3 = (Integer)(Me.dispscale / 4.0D)
			  i = Me.h0 / i2 * i2 - Me.h0
			  While i < Me.h0 + paramDimension.width
				j = Me.v0 / i2 * i2 - Me.v0
				While j < Me.v0 + paramDimension.height
				  paramGraphics.drawLine(i - i3, j, i + i3, j)
				  paramGraphics.drawLine(i, j - i3, i, j + i3)
				  j += i2
				End While
				i += i2
			  End While
        End While
        End While
        Exit While
        End Select
		paramGraphics.Color = localColor2
		Select Case i1
        Case 0 : 

		  j = 0
		  m = paramDimension.height
		  i = Me.h0 / i2 / Me.fixed_length * i2 * Me.fixed_length - Me.h0
		  While i < Me.h0 + paramDimension.width
			paramGraphics.drawLine(i, j, i, m)
			i += i2 * Me.fixed_length
		  End While
		  i = 0
		  k = paramDimension.width
		  j = Me.v0 / i2 / Me.fixed_length * i2 * Me.fixed_length - Me.v0
	  GoTo case 1
		Case 1

        Case 2

        While j < Me.v0 + paramDimension.height
			paramGraphics.drawLine(i, j, k, j)
			j += i2 * Me.fixed_length
			Continue Do
			i = Me.h0 / i2 * i2 - Me.h0
			While i < Me.h0 + paramDimension.width
			  j = Me.v0 / i2 * i2 - Me.v0
			  While j < Me.v0 + paramDimension.height
				paramGraphics.drawLine(i, j, i, j)
				j += i2 * Me.fixed_length
			  End While
			  i += i2 * Me.fixed_length
			  Continue Do
			  i3 = (Integer)(Me.dispscale / 4.0D)
			  i = Me.h0 / i2 * i2 - Me.h0
			  While i < Me.h0 + paramDimension.width
				j = Me.v0 / i2 * i2 - Me.v0
				While j < Me.v0 + paramDimension.height
				  paramGraphics.drawLine(i - i3, j, i + i3, j)
				  paramGraphics.drawLine(i, j - i3, i, j + i3)
				  j += i2 * Me.fixed_length
				End While
				i += i2 * Me.fixed_length
			  End While
        End While
        End While
        Exit While
        End Select
	  }
 
	  Public virtual void paintReaction(java.awt.Graphics paramGraphics, keg.compound.Reaction paramReaction) 
	  {
		If ((paramGraphics Is java.awt.Graphics2D))
		{
		  ((java.awt.Graphics2D)paramGraphics).setRenderingHint(java.awt.RenderingHints.KEY_ANTIALIASING, java.awt.RenderingHints.VALUE_ANTIALIAS_ON)
		}
		Dim localRectangle As Rectangle = ((keg.compound.Reaction)Me.conteiner).getBound(Me.dispscale) 
		Dim j As Integer = localRectangle.x
        Dim k As Integer = localRectangle.x + localRectangle.width
        Dim m As Integer = localRectangle.y
        Dim n As Integer = localRectangle.y + localRectangle.height
        Dim i1 As Integer = 0
        Dim i2 As Integer = 0
        Me.fm = paramGraphics.FontMetrics
        Me.fHeight = Me.fm.Ascent
        Me.fWidth = Me.fm.stringWidth("H")
        Dim i3 As Integer = (Integer)Math.Round(Me.fWidth * 2.5D) 
		If (j < i3)
		{
		  Dim - As i1 =  j - i3 
		}
		i3 = (Integer)Math.Round(Me.fHeight * 2.5D)
		If (m < i3)
		{
		  i2 += m - i3
		}
		If ((i1 <> 0) Or (i2 <> 0))
		{
		  paramReaction.moveInternal(i1, -i2, Dispscale)
		  Me.editmode.resetArea()
		}
		Dim i As Integer = paramReaction.objectNum()
        Dim localColor1 As Color = Me.labelC
        Dim localColor2 As Color = Me.foreC
        For n = 0 To i - 1 Step n + 1
        Dim localChemObject As keg.compound.ChemObject = paramReaction.getObject(n)
        If (paramReaction.isReactant(n))
		  {
			localColor1 = Me.reactantC
			localColor2 = Me.reactantC
		  }
 ElseIf (paramReaction.isProduct(n))
		  {
			localColor1 = Me.productC
			localColor2 = Me.productC
		  }
 Else
		  {
			localColor1 = Me.labelC
			localColor2 = Me.foreC
		  }
		  If ((localChemObject Is keg.compound.Molecule))
		  {
			paintMolecule(paramGraphics, (keg.compound.Molecule)localChemObject, localColor1, localColor2)
		  }
 ElseIf ((localChemObject Is keg.compound.ReactionArrow))
		  {
			If (((keg.compound.ReactionArrow)localChemObject).Select) 
			{
			  paintReactionArrow(paramGraphics, (keg.compound.ReactionArrow)localChemObject, Me.highC)
			}
 Else
			{
			  paintReactionArrow(paramGraphics, (keg.compound.ReactionArrow)localChemObject, Me.foreC)
			}
		  }
		Next
		i = paramReaction.bracketNum()
		For n = 0 To i - 1 Step n + 1
        Dim localBracket As keg.compound.Bracket = paramReaction.getBracket(n)
		  paintBracket(paramGraphics, localBracket)
		Next
	  }
 
	  Public virtual void paintReactionArrow(java.awt.Graphics paramGraphics, keg.compound.ReactionArrow paramReactionArrow, Color paramColor) 
	  {
		paramGraphics.Color = paramColor
		Dim i As Integer = paramReactionArrow.DX1()
        Dim j As Integer = paramReactionArrow.DY1()
        Dim k As Integer = paramReactionArrow.DX2()
        Dim m As Integer = paramReactionArrow.DY2()
		paramGraphics.drawLine(i - Me.h0, j - Me.h0, k - Me.h0, m - Me.h0)
		Dim localVector As ArrayList = paramReactionArrow.arrowHead()
        Dim n As Integer
        For n = 0 To 5 - 1 Step n + 1
        Me.xPoints(n) = (((DblRect)localVector(n)).width - Me.h0)
		  Me.yPoints(n) = (((DblRect)localVector(n)).height - Me.v0)
		Next
		paramGraphics.fillPolygon(Me.xPoints, Me.yPoints, 5)
	  }
 
	  Public virtual void paintReactionArrow(java.awt.Graphics paramGraphics, keg.compound.ReactionArrow paramReactionArrow, Color paramColor, Integer paramInt1, Integer paramInt2) 
	  {
		paramGraphics.Color = paramColor
		paramGraphics.drawLine(paramReactionArrow.DX1() + paramInt1 - Me.h0, paramReactionArrow.DY1() + paramInt2 - Me.v0, paramReactionArrow.DX2() + paramInt1 - Me.h0, paramReactionArrow.DY2() + paramInt2 - Me.v0)
		Dim localVector As ArrayList = paramReactionArrow.arrowHead()
        Dim i As Integer
        For i = 0 To 5 - 1 Step i + 1
        Me.xPoints(i) = (((DblRect)localVector(i)).width + paramInt1 - Me.h0)
		  Me.yPoints(i) = (((DblRect)localVector(i)).height + paramInt2 - Me.v0)
		Next
		paramGraphics.fillPolygon(Me.xPoints, Me.yPoints, 5)
	  }
 
	  Public virtual void paintMolecule(java.awt.Graphics paramGraphics, keg.compound.Molecule paramMolecule, Color paramColor1, Color paramColor2) 
	  {
		Dim localObject As Object
        Dim localAtom2 As keg.compound.Atom
        If ((Not ShrinkMode) And (Me.isEnabledShrinkPartMarker))
		{
		  paramGraphics.Color = Me.grounedAtomColor
		  For i = 1 To paramMolecule.BondNum Step i + 1
			localObject = paramMolecule.getBond(i)
			If ((((keg.compound.Bond)localObject).NonGroupedBond <> True) And (((keg.compound.Bond)localObject).ConnectedBond <> True)) 
			{
			  Dim localAtom1 As keg.compound.Atom = ((keg.compound.Bond)localObject).Atom1 
			  localAtom2 = ((keg.compound.Bond)localObject).Atom2
			  Dim k As Integer = localAtom1.DX(Me.dispscale) - Me.h0 - 10
        Dim m As Integer = localAtom1.DY(Me.dispscale) - Me.v0 - 10
        Dim n As Integer = localAtom2.DX(Me.dispscale) - Me.h0 - 10
        Dim i1 As Integer = localAtom2.DY(Me.dispscale) - Me.v0 - 10
        Dim d1 As Double = Math.Ceiling(Math.Sqrt((n - k) * (n - k) + (i1 - m) * (i1 - m)))
        Dim d2 As Double = (n - k) / d1
        Dim d3 As Double = (i1 - m) / d1
			  paramGraphics.fillOval((Integer)(d2 + k), (Integer)(d3 + m), 20, 20)
			  Dim i2 As Integer
        For i2 = 1 To d1 + 1.0D- 1  Step i2 + 1
				paramGraphics.fillOval((Integer)(d2 * i2 + k), (Integer)(d3 * i2 + m), 20, 20)
			  Next
			}
		  Next
		}
		Dim i As Integer
        For i = 1 To paramMolecule.BondNum Step i + 1
        If ((paramMolecule.getBond(i).Order = 1) And (paramMolecule.getBond(i).Stereo = 6))
		  {
			localObject = paramMolecule.getBond(i)
			If ((((keg.compound.Bond)localObject).NonGroupedBond) Or (Not ShrinkMode)) 
			{
			  If (((keg.compound.Bond)localObject).Select) 
			  {
				paintBond(paramGraphics, (keg.compound.Bond)localObject, Me.highC)
			  }
 ElseIf (((keg.compound.Bond)localObject).col <> Nothing) 
			  {
				paintBond(paramGraphics, (keg.compound.Bond)localObject, paramMolecule.getBond(i).col)
			  }
 Else
			  {
				paintBond(paramGraphics, (keg.compound.Bond)localObject, paramColor2)
			  }
			}
		  }
		Next
        If (ShrinkMode)
		{
		  For i = 1 To paramMolecule.AtomNum Step i + 1
			localObject = paramMolecule.getAtom(i)
			If (((keg.compound.Atom)localObject).Express_group) 
			{
			  Dim j As Integer
        For j = 0 To ((keg.compound.Atom)localObject).GroupPartnerSize- 1  Step j + 1
				localAtom2 = ((keg.compound.Atom)localObject).getGroupPartner(j)
				paramGraphics.Color = paramColor2
				paramGraphics.drawLine(localAtom2.DX(Me.dispscale) - Me.h0, localAtom2.DY(Me.dispscale) - Me.v0, ((keg.compound.Atom)localObject).DX(Me.dispscale) - Me.h0, ((keg.compound.Atom)localObject).DY(Me.dispscale) - Me.v0)
			  Next
			}
		  Next
		}
		For i = 1 To paramMolecule.BondNum Step i + 1
        If ((paramMolecule.getBond(i).Order <> 1) Or (paramMolecule.getBond(i).Stereo <> 6))
		  {
			localObject = paramMolecule.getBond(i)
			If ((((keg.compound.Bond)localObject).NonGroupedBond) Or (Not ShrinkMode)) 
			{
			  If (((keg.compound.Bond)localObject).Select) 
			  {
				paintBond(paramGraphics, (keg.compound.Bond)localObject, Me.highC)
			  }
 ElseIf (((keg.compound.Bond)localObject).col <> Nothing) 
			  {
				paintBond(paramGraphics, (keg.compound.Bond)localObject, paramMolecule.getBond(i).col)
			  }
 Else
			  {
				paintBond(paramGraphics, (keg.compound.Bond)localObject, paramColor2)
			  }
			}
		  }
		Next
        For i = 1 To paramMolecule.AtomNum Step i + 1
		  localObject = paramMolecule.getAtom(i)
		  If (((((keg.compound.Atom)localObject).NonGroupedAtom) Or (Not ShrinkMode)) And ((ShrinkMode) Or (Not ((keg.compound.Atom)localObject).Express_group))) 
		  {
			If (((keg.compound.Atom)localObject).Select) 
			{
			  paintAtom(paramGraphics, (keg.compound.Atom)localObject, Me.highC)
			}
 ElseIf (((keg.compound.Atom)localObject).col <> Nothing) 
			{
			  paintAtom(paramGraphics, (keg.compound.Atom)localObject, paramMolecule.getAtom(i).col)
			}
 Else
			{
			  paintAtom(paramGraphics, (keg.compound.Atom)localObject, paramColor1)
			}
		  }
		Next
        For i = 0 To paramMolecule.BracketNum - 1 Step i + 1
		  paintBracket(paramGraphics, paramMolecule.getBracket(i + 1))
		Next
	  }
 
	  Public Overridable Property ShrinkMode() As Boolean
            Get
                Return Me.editmode.ShrinkMode
            End Get
            Set(ByVal Value As Boolean)
                Me.editmode.ShrinkMode = value
                repaint()
            End Set
        End Property


        Public virtual void paintMoleculeOutside(java.awt.Graphics paramGraphics, keg.compound.Molecule paramMolecule, Color paramColor1, Color paramColor2, Integer paramInt1, Integer paramInt2) 
	  {
		Dim i As Integer
        For i = 1 To paramMolecule.BondNum Step i + 1
        If ((paramMolecule.getBond(i).Order = 1) And (paramMolecule.getBond(i).Stereo = 6))
		  {
			If (paramMolecule.getBond(i).Select)
			{
			  paintBond(paramGraphics,paramMolecule.getBond(i),paramColor1,paramColor2,New DblRect(paramInt1,paramInt2))
			}
 ElseIf (paramMolecule.getBond(i).col <> Nothing)
			{
			  paintBond(paramGraphics,paramMolecule.getBond(i),paramMolecule.getBond(i).col,paramColor2,New DblRect(paramInt1,paramInt2))
			}
 Else
			{
			  paintBond(paramGraphics,paramMolecule.getBond(i),Me.foreC,paramColor2,New DblRect(paramInt1,paramInt2))
			}
		  }
		Next
        For i = 1 To paramMolecule.BondNum Step i + 1
        If ((paramMolecule.getBond(i).Order <> 1) Or (paramMolecule.getBond(i).Stereo <> 6))
		  {
			If (paramMolecule.getBond(i).Select)
			{
			  paintBond(paramGraphics,paramMolecule.getBond(i),paramColor1,paramColor2,New DblRect(paramInt1,paramInt2))
			}
 ElseIf (paramMolecule.getBond(i).col <> Nothing)
			{
			  paintBond(paramGraphics,paramMolecule.getBond(i),paramMolecule.getBond(i).col,paramColor2,New DblRect(paramInt1,paramInt2))
			}
 Else
			{
			  paintBond(paramGraphics,paramMolecule.getBond(i),Me.foreC,paramColor2,New DblRect(paramInt1,paramInt2))
			}
		  }
		Next
        For i = 1 To paramMolecule.AtomNum Step i + 1
        If (paramMolecule.getAtom(i).Select)
		  {
			paintAtom(paramGraphics,paramMolecule.getAtom(i),paramColor1,paramColor2,New DblRect(paramInt1,paramInt2))
		  }
 ElseIf (paramMolecule.getAtom(i).col <> Nothing)
		  {
			paintAtom(paramGraphics,paramMolecule.getAtom(i),paramMolecule.getAtom(i).col,paramColor2,New DblRect(paramInt1,paramInt2))
		  }
 Else
		  {
			paintAtom(paramGraphics,paramMolecule.getAtom(i),Me.labelC,paramColor2,New DblRect(paramInt1,paramInt2))
		  }
		Next
	  }
 
	  Public virtual void paintAtom(java.awt.Graphics paramGraphics, keg.compound.Atom paramAtom, Color paramColor) 
	  {
		paintAtom(paramGraphics,paramAtom,paramColor,Me.backC,New DblRect(Me.h0,Me.v0))
	  }
 
	  Public virtual void paintAtom(java.awt.Graphics paramGraphics, keg.compound.Atom paramAtom, Color paramColor, DblRect paramDimension) 
	  {
		paintAtom(paramGraphics, paramAtom, paramColor, Me.backC, paramDimension)
	  }
 
	  Public virtual java.awt.Font getlFontSup(keg.compound.Atom paramAtom) 
	  {
		Dim localFont As java.awt.Font = Me.lFontSUP
        If (paramAtom.Fontsize <> -1)
		{
		  Dim localInteger As int? = New int?(paramAtom.Fontsize)

		  localFont = (java.awt.Font)Me.lfontSUPHt(localInteger)
		  If (localFont = Nothing)
		  {
			Dim i As Integer = Math.Round(Me.lFontSUP.Size2D / Me.lFont.Size2D * (Integer)localInteger)
			localFont = New java.awt.Font(Me.lFontSUP.Family, Me.lFontSUP.Style, i)
			Me.lfontSUPHt(localInteger) = localFont
		  }
		}
		Return localFont
	  }
 
	  Private java.awt.Font getFont(keg.compound.Text paramText) 
	  {
		Dim str As String = paramText.FontFamily + "_" + paramText.FontStyle + "_" + paramText.FontSize
        Dim localFont As java.awt.Font = (java.awt.Font)Me.lfontHt(str) 
		If (localFont = Nothing)
		{
		  localFont = New java.awt.Font(paramText.FontFamily, paramText.FontStyle, paramText.FontSize)
		  Me.lfontHt(str) = localFont
		}
		Return localFont
	  }
 
	  Public virtual java.awt.Font getlFont(keg.compound.Atom paramAtom) 
	  {
		Dim localFont As java.awt.Font = Me.lFont
        If (paramAtom.Fontsize <> -1)
		{
		  Dim localInteger As int? = New int?(paramAtom.Fontsize)

		  localFont = (java.awt.Font)Me.lfontHt(localInteger)
		  If (localFont = Nothing)
		  {
			localFont = New java.awt.Font(Me.lFont.Family, Me.lFont.Style, paramAtom.Fontsize)
			Me.lfontHt(localInteger) = localFont
		  }
		}
		Return localFont
	  }
 
	  Public virtual void paintAtom(java.awt.Graphics paramGraphics, keg.compound.Atom paramAtom, Color paramColor1, Color paramColor2, DblRect paramDimension) 
	  {
		Dim str As String = Me.editmode.keggatomname_mode ? paramAtom.KEGGAtomName : paramAtom.getDrawLabel(Me.editmode.hydrogen_draw) 

		paramGraphics.Font = getlFont(paramAtom)
		Dim localFontMetrics As java.awt.FontMetrics = paramGraphics.FontMetrics
        Me.fHeight = (localFontMetrics.Ascent + localFontMetrics.Descent - 1)
        Dim i1 As Integer = localFontMetrics.stringWidth("+")
        Dim i As Integer = paramAtom.DX(Me.dispscale)
        Dim j As Integer = paramAtom.DY(Me.dispscale)
        Dim i2 As Integer
        If (paramAtom.Express_group)
		{
		  i2 = localFontMetrics.stringWidth(str)
		  If (paramAtom.GroupLabelDirection = 1)
		  {
			Dim - As i =  i2 - 3 
		  }
 ElseIf (paramAtom.GroupLabelDirection = 3)
		  {
			Dim - As i =  i2 / 2 
			j += Me.fHeight / 2
		  }
 ElseIf (paramAtom.GroupLabelDirection = 2)
		  {
			Dim - As i =  i2 / 2 
			Dim - As j =  Me.fHeight / 2 
		  }
		}
		Dim n As Integer
        Dim k As Integer = n = i
        Dim m As Integer = j
        If ((str.Length = 0) And (paramAtom.Isotope = 0))
		{
		  If (paramAtom.Select)
		  {
			paramGraphics.Color = paramColor1
			paramGraphics.fillRect(i - 2 - paramDimension.width, j - 2 - paramDimension.height, 5, 5)
		  }
 ElseIf (paramAtom.numBond() = 0)
		  {
			paramGraphics.Color = paramAtom.col = IIf( Nothing ,  Me.labelC ,  paramAtom.col)

			paramGraphics.fillRect(i - 2 - paramDimension.width, j - 2 - paramDimension.height, 5, 5)
		  }
		}
 Else
		{
		  If (str.Length = 0)
		  {
			str = "C"
		  }
		  Me.fHalfW = (localFontMetrics.stringWidth(str.Substring(0, 1)) / 2)
        Me.fWidth = localFontMetrics.stringWidth(str)
        Dim - As i =  Me.fHalfW 
		  j += Me.fHeight / 2
		  k = i
		  n = i
		  m = j - Me.fHeight + 2
		  paramGraphics.Color = paramColor2
		  paramGraphics.fillRect(i - paramDimension.width - 1, j - Me.fHeight - paramDimension.height, Me.fWidth + 2, Me.fHeight)
		  Dim - As j =  localFontMetrics.Descent - 2 
		  paramGraphics.drawString(str, i - paramDimension.width - 1, j - paramDimension.height)
		  paramGraphics.drawString(str, i - paramDimension.width, j - paramDimension.height - 1)
		  paramGraphics.drawString(str, i - paramDimension.width + 1, j - paramDimension.height)
		  paramGraphics.drawString(str, i - paramDimension.width, j - paramDimension.height + 1)
		  paramGraphics.Color = paramColor1
		  paramGraphics.drawString(str, i - paramDimension.width, j - paramDimension.height)
		  i += Me.fWidth
		}
		If (paramAtom.Isotope <> 0)
		{
		  str = Convert.ToString(paramAtom.Isotope)
		  paramGraphics.Font = getlFontSup(paramAtom)
		  localFontMetrics = paramGraphics.FontMetrics
		  Me.fHeight = localFontMetrics.Height
        Me.fWidth = localFontMetrics.stringWidth(str)
        Dim - As k =  Me.fWidth + 1 
		  i2 = Me.fHeight / 2 + 2
		  paramGraphics.Color = paramColor2
		  paramGraphics.drawString(str, k - paramDimension.width - 1, m + Me.fHeight - paramDimension.height - i2)
		  paramGraphics.drawString(str, k - paramDimension.width, m + Me.fHeight - paramDimension.height - 1 - i2)
		  paramGraphics.drawString(str, k - paramDimension.width + 1, m + Me.fHeight - paramDimension.height - i2)
		  paramGraphics.drawString(str, k - paramDimension.width, m + Me.fHeight - paramDimension.height + 1 - i2)
		  paramGraphics.Color = paramColor1
		  paramGraphics.drawString(str, k - paramDimension.width, m + Me.fHeight - paramDimension.height - i2)
		  k = k - 1
		}
		If ((paramAtom.Charge <> 0) And (paramAtom.HydrogenDraw <> 1))
		{
		  Dim arrayOfInt1() As Integer = {i, k, n, j, m}

		  arrayOfInt1 = paintAtom_Charge(paramGraphics, paramAtom, arrayOfInt1, paramDimension, paramColor1)
		  i = arrayOfInt1(0)
		  k = arrayOfInt1(1)
		  n = arrayOfInt1(2)
		  j = arrayOfInt1(3)
		  m = arrayOfInt1(4)
		  If (paramAtom.HydrogenDraw = 4)
		  {
			Dim - As m =  Me.fHeight / 2 
		  }
		}
 ElseIf (paramAtom.Radical <> 0)
		{
		  str = ""
		  Dim i3 As Integer
        For i3 = 0 To paramAtom.Radical - 1 Step i3 + 1
			str = str + "*"
		  Next
		  paramGraphics.Font = getlFontSup(paramAtom)
		  localFontMetrics = paramGraphics.FontMetrics
		  Me.fHeight = localFontMetrics.Height
		  paramGraphics.Color = paramColor1
		  paramGraphics.drawString(str, i - paramDimension.width, m + Me.fHeight - paramDimension.height)
		}
		Dim arrayOfInt2() As Integer
        If ((Me.editmode.hydrogen_draw) And (paramAtom.HydrogenDraw <> 0))
		{
		  arrayOfInt2 = New Integer() 
		  {
		  	i, k, n, j, m
		  }

		  arrayOfInt2 = paint_ImpliciteHydrogen(paramGraphics, paramAtom, str, arrayOfInt2, paramDimension, paramColor1)
		  i = arrayOfInt2(0)
		  k = arrayOfInt2(1)
		  n = arrayOfInt2(2)
		  j = arrayOfInt2(3)
		  m = arrayOfInt2(4)
		}
		If ((paramAtom.Charge <> 0) And (paramAtom.HydrogenDraw = 1))
		{
		  arrayOfInt2 = New Integer() 
		  {
		  	i, k, n, j, m
		  }

		  paintAtom_Charge(paramGraphics, paramAtom, arrayOfInt2, paramDimension, paramColor1)
		}
	  }
 
	  Private Integer() paintAtom_Charge(java.awt.Graphics paramGraphics, keg.compound.Atom paramAtom, Integer() paramArrayOfInt, DblRect paramDimension, Color paramColor) 
	  {
		Dim i As Integer = paramArrayOfInt(0)
        Dim j As Integer = paramArrayOfInt(1)
        Dim k As Integer = paramArrayOfInt(2)
        Dim m As Integer = paramArrayOfInt(3)
        Dim n As Integer = paramArrayOfInt(4)
        Dim str As String = ""
        If (paramAtom.Charge < -1)
		{
		  str = Convert.ToString(-paramAtom.Charge) + "-"
		}
 ElseIf (paramAtom.Charge = -1)
		{
		  str = "-"
		}
 ElseIf (paramAtom.Charge = 1)
		{
		  str = "+"
		}
 ElseIf (paramAtom.Charge > 1)
		{
		  str = Convert.ToString(paramAtom.Charge) + "+"
		}
		paramGraphics.Font = getlFontSup(paramAtom)
		Me.fm = paramGraphics.FontMetrics
        Me.fHeight = (Me.fm.Ascent + Me.fm.Descent)
		paramGraphics.Color = Me.backC
		paramGraphics.drawString(str, i - paramDimension.width - 1, n + Me.fHeight - paramDimension.height - Me.font_margin)
		paramGraphics.drawString(str, i - paramDimension.width + 1, n + Me.fHeight - paramDimension.height - Me.font_margin)
		paramGraphics.drawString(str, i - paramDimension.width, n + Me.fHeight - paramDimension.height - Me.font_margin - 1)
		paramGraphics.drawString(str, i - paramDimension.width, n + Me.fHeight - paramDimension.height - Me.font_margin + 1)
		paramGraphics.drawString(str, i - paramDimension.width - 2, n + Me.fHeight - paramDimension.height - Me.font_margin)
		paramGraphics.drawString(str, i - paramDimension.width + 2, n + Me.fHeight - paramDimension.height - Me.font_margin)
		paramGraphics.drawString(str, i - paramDimension.width, n + Me.fHeight - paramDimension.height - Me.font_margin - 2)
		paramGraphics.drawString(str, i - paramDimension.width, n + Me.fHeight - paramDimension.height - Me.font_margin + 2)
		paramGraphics.Color = paramColor
		paramGraphics.drawString(str, i - paramDimension.width, n + Me.fHeight - paramDimension.height - Me.font_margin)
		Return New Integer()
		{
			i, j, k, m, n
		}

	  }
 
	  Private Integer() paint_ImpliciteHydrogen(java.awt.Graphics paramGraphics, keg.compound.Atom paramAtom, String paramString, Integer() paramArrayOfInt, DblRect paramDimension, Color paramColor) 
	  {
		Dim i As Integer = paramArrayOfInt(0)
        Dim j As Integer = paramArrayOfInt(1)
        Dim k As Integer = paramArrayOfInt(2)
        Dim m As Integer = paramArrayOfInt(3)
        Dim n As Integer = paramArrayOfInt(4)
        If (paramString.Length = 0)
		{
		  Return paramArrayOfInt
		}
		Dim i1 As Integer = System.getProperty("os.name").ToLower().IndexOf("mac os") > -1 ? 1 : 0 

		Dim i2 As Integer
        Select Case paramAtom.HydrogenDraw
        Case 1 : 

		  paramString = "H"
		  paramGraphics.Font = getlFont(paramAtom)
		  Me.fm = paramGraphics.FontMetrics
        Me.fWidth = Me.fm.stringWidth(paramString)
		  paramGraphics.Color = Me.backC
		  paramGraphics.drawString(paramString, i - paramDimension.width - 1, m - paramDimension.height)
		  paramGraphics.drawString(paramString, i - paramDimension.width, m - paramDimension.height - 1)
		  paramGraphics.drawString(paramString, i - paramDimension.width + 1, m - paramDimension.height)
		  paramGraphics.drawString(paramString, i - paramDimension.width, m - paramDimension.height + 1)
		  paramGraphics.Color = paramColor
		  paramGraphics.drawString(paramString, i - paramDimension.width, m - paramDimension.height)
		  i += Me.fWidth
		  If (paramAtom.ImplicitHydrogen > 1)
		  {
			paramString = Convert.ToString(paramAtom.ImplicitHydrogen)
			paramGraphics.Font = getlFontSup(paramAtom)
			Me.fm = paramGraphics.FontMetrics
        Me.fWidth = Me.fm.stringWidth(paramString)
			i = i + 1
			paramGraphics.Color = Me.backC
			paramGraphics.drawString(paramString, i - paramDimension.width - 1, m + 3 - paramDimension.height)
			paramGraphics.drawString(paramString, i - paramDimension.width, m + 3 - paramDimension.height - 1)
			paramGraphics.drawString(paramString, i - paramDimension.width + 1, m + 3 - paramDimension.height)
			paramGraphics.drawString(paramString, i - paramDimension.width, m + 3 - paramDimension.height + 1)
			paramGraphics.Color = paramColor
			paramGraphics.drawString(paramString, i - paramDimension.width, m + 3 - paramDimension.height)
			i += Me.fWidth
		  }
		  Exit For
        Case 3

        If (paramAtom.ImplicitHydrogen > 1)
		  {
			paramString = Convert.ToString(paramAtom.ImplicitHydrogen)
			paramGraphics.Font = getlFontSup(paramAtom)
			Me.fm = paramGraphics.FontMetrics
        Me.fWidth = Me.fm.stringWidth(paramString)
        Dim - As j =  Me.fWidth + 1 
			paramGraphics.Color = Me.backC
			paramGraphics.drawString(paramString, j - paramDimension.width - 1, m + 3 - paramDimension.height)
			paramGraphics.drawString(paramString, j - paramDimension.width, m + 3 - paramDimension.height - 1)
			paramGraphics.drawString(paramString, j - paramDimension.width + 1, m + 3 - paramDimension.height)
			paramGraphics.drawString(paramString, j - paramDimension.width, m + 3 - paramDimension.height + 1)
			paramGraphics.Color = paramColor
			paramGraphics.drawString(paramString, j - paramDimension.width, m + 3 - paramDimension.height)
		  }
		  paramString = "H"
		  paramGraphics.Font = getlFont(paramAtom)
		  Me.fm = paramGraphics.FontMetrics
        Me.fWidth = Me.fm.stringWidth(paramString)
        Dim - As j =  Me.fWidth + 1 
		  paramGraphics.Color = Me.backC
		  paramGraphics.drawString(paramString, j - paramDimension.width - 1, m - paramDimension.height)
		  paramGraphics.drawString(paramString, j - paramDimension.width, m - paramDimension.height - 1)
		  paramGraphics.drawString(paramString, j - paramDimension.width + 1, m - paramDimension.height)
		  paramGraphics.drawString(paramString, j - paramDimension.width, m - paramDimension.height + 1)
		  paramGraphics.Color = paramColor
		  paramGraphics.drawString(paramString, j - paramDimension.width, m - paramDimension.height)
		  Exit For
        Case 4 : 

		  paramString = "H"
		  paramGraphics.Font = getlFont(paramAtom)
		  Me.fm = paramGraphics.FontMetrics
        Me.fWidth = Me.fm.stringWidth(paramString)
		  paramGraphics.Color = Me.backC
		  i2 = i1 <> IIf( 0 ,  2 ,  3)

		  paramGraphics.drawString(paramString, k - paramDimension.width - 1, n - i2 - paramDimension.height)
		  paramGraphics.drawString(paramString, k - paramDimension.width, n - i2 - paramDimension.height - 1)
		  paramGraphics.drawString(paramString, k - paramDimension.width + 1, n - i2 - paramDimension.height)
		  paramGraphics.drawString(paramString, k - paramDimension.width, n - i2 - paramDimension.height + 1)
		  paramGraphics.Color = paramColor
		  paramGraphics.drawString(paramString, k - paramDimension.width, n - i2 - paramDimension.height)
		  i = k + Me.fWidth + 1
		  If (paramAtom.ImplicitHydrogen > 1)
		  {
			Dim - As i2 =  3 
			paramString = Convert.ToString(paramAtom.ImplicitHydrogen)
			paramGraphics.Font = getlFontSup(paramAtom)
			Me.fm = paramGraphics.FontMetrics
			paramGraphics.Color = Me.backC
			paramGraphics.drawString(paramString, k + Me.fWidth + 1 - paramDimension.width - 1, n - paramDimension.height - i2)
			paramGraphics.drawString(paramString, k + Me.fWidth + 1 - paramDimension.width, n - paramDimension.height - 1 - i2)
			paramGraphics.drawString(paramString, k + Me.fWidth + 1 - paramDimension.width + 1, n - paramDimension.height - i2)
			paramGraphics.drawString(paramString, k + Me.fWidth + 1 - paramDimension.width, n - paramDimension.height + 1 - i2)
			paramGraphics.Color = paramColor
			paramGraphics.drawString(paramString, k + Me.fWidth + 1 - paramDimension.width, n - paramDimension.height - i2)
			Me.fWidth = Me.fm.stringWidth(paramString)
			i += Me.fWidth + 1
		  }
		  paramGraphics.Font = getlFont(paramAtom)
		  Me.fm = paramGraphics.FontMetrics
        Me.fHeight = Me.fm.Height
        Dim - As n =  Me.fHeight + i2 
		  Exit For
        Case 2 : 

		  paramString = "H"
		  paramGraphics.Font = getlFont(paramAtom)
		  Me.fm = paramGraphics.FontMetrics
        Me.fWidth = Me.fm.stringWidth(paramString)
        Me.fHeight = Me.fm.Height
		  m += Me.fHeight
		  paramGraphics.Color = Me.backC
		  i2 = i1 <> 0 ? - 4 : -6

		  paramGraphics.drawString(paramString, k - paramDimension.width - 1, m - paramDimension.height + i2)
		  paramGraphics.drawString(paramString, k - paramDimension.width, m - paramDimension.height - 1 + i2)
		  paramGraphics.drawString(paramString, k - paramDimension.width + 1, m - paramDimension.height + i2)
		  paramGraphics.drawString(paramString, k - paramDimension.width, m - paramDimension.height + 1 + i2)
		  paramGraphics.Color = paramColor
		  paramGraphics.drawString(paramString, k - paramDimension.width, m - paramDimension.height + i2)
		  If (paramAtom.ImplicitHydrogen > 1)
		  {
			i2 += 3
			paramString = Convert.ToString(paramAtom.ImplicitHydrogen)
			paramGraphics.Font = getlFontSup(paramAtom)
			Me.fm = paramGraphics.FontMetrics
			paramGraphics.Color = Me.backC
			paramGraphics.drawString(paramString, k + Me.fWidth + 1 - paramDimension.width - 1, m + i2 - paramDimension.height)
			paramGraphics.drawString(paramString, k + Me.fWidth + 1 - paramDimension.width, m + i2 - paramDimension.height - 1)
			paramGraphics.drawString(paramString, k + Me.fWidth + 1 - paramDimension.width + 1, m + i2 - paramDimension.height)
			paramGraphics.drawString(paramString, k + Me.fWidth + 1 - paramDimension.width, m + i2 - paramDimension.height + 1)
			paramGraphics.Color = paramColor
			paramGraphics.drawString(paramString, k + Me.fWidth + 1 - paramDimension.width, m + i2 - paramDimension.height)
			Me.fWidth = Me.fm.stringWidth(paramString)
		  }
		  Exit For
        End Select
        Return New Integer()
		{
			i, j, k, m, n
		}

	  }
 
	  Public virtual void paintAtom(java.awt.Graphics paramGraphics, keg.compound.Atom paramAtom, Color paramColor, Integer paramInt1, Integer paramInt2) 
	  {
		Dim str As String = Me.editmode.keggatomname_mode ? paramAtom.KEGGAtomName : paramAtom.getDrawLabel(Me.editmode.hydrogen_draw) 

		If (str.Length = 0)
		{
		  If (paramAtom.Select)
		  {
			paramGraphics.Color = paramColor
			paramGraphics.fillRect(paramAtom.DX(Me.dispscale) - 2, paramAtom.DY(Me.dispscale) - 2, 5, 5)
		  }
 ElseIf (paramAtom.numBond() = 0)
		  {
			paramGraphics.Color = paramAtom.col = IIf( Nothing ,  Me.labelC ,  paramAtom.col)

			paramGraphics.fillRect(paramAtom.DX(Me.dispscale) - 2, paramAtom.DY(Me.dispscale) - 2, 5, 5)
		  }
		}
 Else
		{
		  paramGraphics.Font = Me.lFont
		  Me.fm = paramGraphics.FontMetrics
        Me.fHeight = Me.fm.Ascent
        Me.fHalfW = (Me.fm.stringWidth(str.Substring(0, 1)) / 2)
        Me.fWidth = Me.fm.stringWidth(str)
		  paramGraphics.Color = Me.backC
		  paramGraphics.fillRect(paramAtom.DX(Me.dispscale) - Me.fHalfW + paramInt1 - Me.h0, paramAtom.DY(Me.dispscale) - Me.fHalfW + paramInt2 - Me.v0, Me.fWidth, Me.fHeight)
		  paramGraphics.drawString(str, paramAtom.DX(Me.dispscale) - Me.fHalfW + paramInt1 - Me.h0 - 1, paramAtom.DY(Me.dispscale) + Me.fHeight / 2 - 2 + paramInt2 - Me.v0)
		  paramGraphics.drawString(str, paramAtom.DX(Me.dispscale) - Me.fHalfW + paramInt1 - Me.h0, paramAtom.DY(Me.dispscale) + Me.fHeight / 2 - 2 + paramInt2 - Me.v0 - 1)
		  paramGraphics.drawString(str, paramAtom.DX(Me.dispscale) - Me.fHalfW + paramInt1 - Me.h0 + 1, paramAtom.DY(Me.dispscale) + Me.fHeight / 2 - 2 + paramInt2 - Me.v0)
		  paramGraphics.drawString(str, paramAtom.DX(Me.dispscale) - Me.fHalfW + paramInt1 - Me.h0, paramAtom.DY(Me.dispscale) + Me.fHeight / 2 - 2 + paramInt2 - Me.v0 + 1)
		  paramGraphics.Color = paramColor
		  paramGraphics.drawString(str, paramAtom.DX(Me.dispscale) - Me.fHalfW + paramInt1 - Me.h0, paramAtom.DY(Me.dispscale) + Me.fHeight / 2 - 2 + paramInt2 - Me.v0)
		}
	  }
 
	  Public virtual void paintBond(java.awt.Graphics paramGraphics, keg.compound.Bond paramBond, Color paramColor) 
	  {
		paintBond(paramGraphics,paramBond,paramColor,Me.backC,New DblRect(Me.h0,Me.v0))
	  }
 
	  Public virtual void paintBond(java.awt.Graphics paramGraphics, keg.compound.Bond paramBond, Color paramColor, DblRect paramDimension) 
	  {
		paintBond(paramGraphics, paramBond, paramColor, Me.backC, paramDimension)
	  }
 
	  Public virtual void paintBond(java.awt.Graphics paramGraphics, keg.compound.Bond paramBond, Color paramColor1, Color paramColor2, DblRect paramDimension) 
	  {
		Dim localDimension4 As DblRect
        Dim localDimension3 As DblRect
        Dim localDimension2 As DblRect
        Dim localDimension1 As DblRect = localDimension2 = localDimension3 = localDimension4 = Nothing
        Dim localAtom1 As keg.compound.Atom = paramBond.Atom1
        Dim localAtom2 As keg.compound.Atom = paramBond.Atom2
		paramGraphics.Color = paramColor1
		Dim localVector As ArrayList
        Select Case paramBond.Order
        Case 1

        Dim i As Integer
        Select Case paramBond.Stereo
        Case 4 : 

			localVector = paramBond.coordinate(Me.dispscale, Me.font_margin, Me.bold_width)
			localDimension1 = (DblRect)localVector(0)
			For i = 1 To localVector.Count - 1 Step i + 1
			  localDimension2 = (DblRect)localVector(i)
			  paramGraphics.drawLine(localDimension1.width - paramDimension.width, localDimension1.height - paramDimension.height, localDimension2.width - paramDimension.width, localDimension2.height - paramDimension.height)
			  localDimension1 = localDimension2
			Next
        Exit For
        Case 0

        Case 3 : 

			paramGraphics.drawLine(localAtom1.DX(Me.dispscale) - paramDimension.width, localAtom1.DY(Me.dispscale) - paramDimension.height, localAtom2.DX(Me.dispscale) - paramDimension.width, localAtom2.DY(Me.dispscale) - paramDimension.height)
			Exit For
        Case 1 : 

			localVector = paramBond.coordinate(Me.dispscale, Me.font_margin, Me.bold_width)
			For i = 0 To localVector.Count - 1 Step i + 1
			  localDimension1 = (DblRect)localVector(i)
			  Me.xPoints(i) = (localDimension1.width - paramDimension.width)
        Me.yPoints(i) = (localDimension1.height - paramDimension.height)
        Next
			paramGraphics.fillPolygon(Me.xPoints, Me.yPoints, localVector.Count)
			Exit For
        Case 6 : 

			localVector = paramBond.coordinate(Me.dispscale, Me.font_margin, Me.bold_width, Me.hash_spacing)
			For i = 8 To localVector.Count - 1 Step i += 2
			  localDimension1 = (DblRect)localVector(i)
			  localDimension2 = (DblRect)localVector(i + 1)
			  paramGraphics.drawLine(localDimension1.width - paramDimension.width, localDimension1.height - paramDimension.height, localDimension2.width - paramDimension.width, localDimension2.height - paramDimension.height)
			Next
			paramGraphics.Color = paramColor2
			localDimension1 = (DblRect)localVector(0)
			Me.xPoints(0) = (localDimension1.width - paramDimension.width)
        Me.yPoints(0) = (localDimension1.height - paramDimension.height)
        Me.xPoints(4) = (localDimension1.width - paramDimension.width)
        Me.yPoints(4) = (localDimension1.height - paramDimension.height)
			localDimension2 = (DblRect)localVector(1)
			Me.xPoints(1) = (localDimension2.width - paramDimension.width)
        Me.yPoints(1) = (localDimension2.height - paramDimension.height)
			localDimension3 = (DblRect)localVector(2)
			Me.xPoints(2) = (localDimension3.width - paramDimension.width)
        Me.yPoints(2) = (localDimension3.height - paramDimension.height)
			localDimension4 = (DblRect)localVector(3)
			Me.xPoints(3) = (localDimension4.width - paramDimension.width)
        Me.yPoints(3) = (localDimension4.height - paramDimension.height)
			paramGraphics.fillPolygon(Me.xPoints, Me.yPoints, 5)
			localDimension1 = (DblRect)localVector(4)
			Me.xPoints(0) = (localDimension1.width - paramDimension.width)
        Me.yPoints(0) = (localDimension1.height - paramDimension.height)
        Me.xPoints(4) = (localDimension1.width - paramDimension.width)
        Me.yPoints(4) = (localDimension1.height - paramDimension.height)
			localDimension2 = (DblRect)localVector(5)
			Me.xPoints(1) = (localDimension2.width - paramDimension.width)
        Me.yPoints(1) = (localDimension2.height - paramDimension.height)
			localDimension3 = (DblRect)localVector(6)
			Me.xPoints(2) = (localDimension3.width - paramDimension.width)
        Me.yPoints(2) = (localDimension3.height - paramDimension.height)
			localDimension4 = (DblRect)localVector(7)
			Me.xPoints(3) = (localDimension4.width - paramDimension.width)
        Me.yPoints(3) = (localDimension4.height - paramDimension.height)
			paramGraphics.fillPolygon(Me.xPoints, Me.yPoints, 5)
		Exit For
        End Select
        Exit For
        Case 2 : 

		  localVector = paramBond.coordinate(Me.dispscale, Me.font_margin, Me.bond_spacing)
		  localDimension1 = (DblRect)localVector(0)
		  localDimension2 = (DblRect)localVector(1)
		  localDimension3 = (DblRect)localVector(2)
		  localDimension4 = (DblRect)localVector(3)
		  paramGraphics.drawLine(localDimension1.width - paramDimension.width, localDimension1.height - paramDimension.height, localDimension2.width - paramDimension.width, localDimension2.height - paramDimension.height)
		  paramGraphics.drawLine(localDimension3.width - paramDimension.width, localDimension3.height - paramDimension.height, localDimension4.width - paramDimension.width, localDimension4.height - paramDimension.height)
		  Exit For
        Case 3 : 

		  localVector = paramBond.coordinate(Me.dispscale, Me.font_margin, Me.bond_spacing)
		  localDimension1 = (DblRect)localVector(0)
		  localDimension2 = (DblRect)localVector(1)
		  localDimension3 = (DblRect)localVector(2)
		  localDimension4 = (DblRect)localVector(3)
		  paramGraphics.drawLine(localDimension1.width - paramDimension.width, localDimension1.height - paramDimension.height, localDimension2.width - paramDimension.width, localDimension2.height - paramDimension.height)
		  paramGraphics.drawLine(localDimension3.width - paramDimension.width, localDimension3.height - paramDimension.height, localDimension4.width - paramDimension.width, localDimension4.height - paramDimension.height)
		  localDimension3 = (DblRect)localVector(4)
		  localDimension4 = (DblRect)localVector(5)
		  paramGraphics.drawLine(localDimension3.width - paramDimension.width, localDimension3.height - paramDimension.height, localDimension4.width - paramDimension.width, localDimension4.height - paramDimension.height)
		  Exit For
        Case -1

        Case 0

        Case 4

        Case 5

        Case 6

        Case 7

        Case 8 : 

		  paramGraphics.drawLine(localAtom1.DX(Me.dispscale) - paramDimension.width, localAtom1.DY(Me.dispscale) - paramDimension.height, localAtom2.DX(Me.dispscale) - paramDimension.width, localAtom2.DY(Me.dispscale) - paramDimension.height)
		  Exit For
        Case Else : 

		  paramGraphics.drawLine(localAtom1.DX(Me.dispscale) - paramDimension.width, localAtom1.DY(Me.dispscale) - paramDimension.height, localAtom2.DX(Me.dispscale) - paramDimension.width, localAtom2.DY(Me.dispscale) - paramDimension.height)
	  Exit For
        End Select
	  }
 
	  Public virtual void paintBond(java.awt.Graphics paramGraphics, keg.compound.Bond paramBond, Color paramColor, Integer paramInt1, Integer paramInt2) 
	  {
		Dim localDimension4 As DblRect
        Dim localDimension3 As DblRect
        Dim localDimension2 As DblRect
        Dim localDimension1 As DblRect = localDimension2 = localDimension3 = localDimension4 = Nothing
        Dim localAtom1 As keg.compound.Atom = paramBond.Atom1
        Dim localAtom2 As keg.compound.Atom = paramBond.Atom2
		paramGraphics.Color = paramColor
		Dim localVector As ArrayList
        Select Case paramBond.Order
        Case 1

        Dim i As Integer
        Select Case paramBond.Stereo
        Case 4 : 

			localVector = paramBond.coordinate(Me.dispscale, Me.font_margin, Me.bold_width)
			localDimension1 = (DblRect)localVector(0)
			For i = 1 To localVector.Count - 1 Step i + 1
			  localDimension2 = (DblRect)localVector(i)
			  paramGraphics.drawLine(localDimension1.width + paramInt1 - Me.h0, localDimension1.height + paramInt2 - Me.v0, localDimension2.width + paramInt1 - Me.h0, localDimension2.height + paramInt2 - Me.v0)
			  localDimension1 = localDimension2
			Next
        Exit For
        Case 0

        Case 3 : 

			paramGraphics.drawLine(localAtom1.DX(Me.dispscale) + paramInt1 - Me.h0, localAtom1.DY(Me.dispscale) + paramInt2 - Me.v0, localAtom2.DX(Me.dispscale) + paramInt1 - Me.h0, localAtom2.DY(Me.dispscale) + paramInt2 - Me.v0)
			Exit For
        Case 1 : 

			localVector = paramBond.coordinate(Me.dispscale, Me.font_margin, Me.bold_width)
			For i = 0 To localVector.Count - 1 Step i + 1
			  localDimension1 = (DblRect)localVector(i)
			  Me.xPoints(i) = (localDimension1.width + paramInt1 - Me.h0)
        Me.yPoints(i) = (localDimension1.height + paramInt2 - Me.v0)
        Next
			paramGraphics.fillPolygon(Me.xPoints, Me.yPoints, localVector.Count)
			Exit For
        Case 6 : 

			localVector = paramBond.coordinate(Me.dispscale, Me.font_margin, Me.bold_width, Me.hash_spacing)
			For i = 8 To localVector.Count - 1 Step i += 2
			  localDimension1 = (DblRect)localVector(i)
			  localDimension2 = (DblRect)localVector(i + 1)
			  paramGraphics.drawLine(localDimension1.width + paramInt1 - Me.h0, localDimension1.height + paramInt2 - Me.v0, localDimension2.width + paramInt1 - Me.h0, localDimension2.height + paramInt2 - Me.v0)
			Next
			paramGraphics.Color = Me.backC
			localDimension1 = (DblRect)localVector(0)
			Me.xPoints(0) = (localDimension1.width + paramInt1 - Me.h0)
        Me.yPoints(0) = (localDimension1.height + paramInt2 - Me.v0)
        Me.xPoints(4) = (localDimension1.width + paramInt1 - Me.h0)
        Me.yPoints(4) = (localDimension1.height + paramInt2 - Me.v0)
			localDimension2 = (DblRect)localVector(1)
			Me.xPoints(1) = (localDimension2.width + paramInt1 - Me.h0)
        Me.yPoints(1) = (localDimension2.height + paramInt2 - Me.v0)
			localDimension3 = (DblRect)localVector(2)
			Me.xPoints(2) = (localDimension3.width + paramInt1 - Me.h0)
        Me.yPoints(2) = (localDimension3.height + paramInt2 - Me.v0)
			localDimension4 = (DblRect)localVector(3)
			Me.xPoints(3) = (localDimension4.width + paramInt1 - Me.h0)
        Me.yPoints(3) = (localDimension4.height + paramInt2 - Me.v0)
			paramGraphics.fillPolygon(Me.xPoints, Me.yPoints, 5)
			localDimension1 = (DblRect)localVector(4)
			Me.xPoints(0) = (localDimension1.width + paramInt1 - Me.h0)
        Me.yPoints(0) = (localDimension1.height + paramInt2 - Me.v0)
        Me.xPoints(4) = (localDimension1.width + paramInt1 - Me.h0)
        Me.yPoints(4) = (localDimension1.height + paramInt2 - Me.v0)
			localDimension2 = (DblRect)localVector(5)
			Me.xPoints(1) = (localDimension2.width + paramInt1 - Me.h0)
        Me.yPoints(1) = (localDimension2.height + paramInt2 - Me.v0)
			localDimension3 = (DblRect)localVector(6)
			Me.xPoints(2) = (localDimension3.width + paramInt1 - Me.h0)
        Me.yPoints(2) = (localDimension3.height + paramInt2 - Me.v0)
			localDimension4 = (DblRect)localVector(7)
			Me.xPoints(3) = (localDimension4.width + paramInt1 - Me.h0)
        Me.yPoints(3) = (localDimension4.height + paramInt2 - Me.v0)
			paramGraphics.fillPolygon(Me.xPoints, Me.yPoints, 5)
		Exit For
        End Select
        Exit For
        Case 2 : 

		  localVector = paramBond.coordinate(Me.dispscale, Me.font_margin, Me.bond_spacing)
		  localDimension1 = (DblRect)localVector(0)
		  localDimension2 = (DblRect)localVector(1)
		  localDimension3 = (DblRect)localVector(2)
		  localDimension4 = (DblRect)localVector(3)
		  paramGraphics.drawLine(localDimension1.width + paramInt1 - Me.h0, localDimension1.height + paramInt2 - Me.v0, localDimension2.width + paramInt1 - Me.h0, localDimension2.height + paramInt2 - Me.v0)
		  paramGraphics.drawLine(localDimension3.width + paramInt1 - Me.h0, localDimension3.height + paramInt2 - Me.v0, localDimension4.width + paramInt1 - Me.h0, localDimension4.height + paramInt2 - Me.v0)
		  Exit For
        Case 3 : 

		  localVector = paramBond.coordinate(Me.dispscale, Me.font_margin, Me.bond_spacing)
		  localDimension1 = (DblRect)localVector(0)
		  localDimension2 = (DblRect)localVector(1)
		  localDimension3 = (DblRect)localVector(2)
		  localDimension4 = (DblRect)localVector(3)
		  paramGraphics.drawLine(localDimension1.width - Me.h0, localDimension1.height - Me.v0, localDimension2.width - Me.h0, localDimension2.height - Me.v0)
		  paramGraphics.drawLine(localDimension3.width - Me.h0, localDimension3.height - Me.v0, localDimension4.width - Me.h0, localDimension4.height - Me.v0)
		  localDimension3 = (DblRect)localVector(4)
		  localDimension4 = (DblRect)localVector(5)
		  paramGraphics.drawLine(localDimension3.width - Me.h0, localDimension3.height - Me.v0, localDimension4.width - Me.h0, localDimension4.height - Me.v0)
		  localDimension1 = New DblRect(localAtom1.DX(Me.dispscale), localAtom1.DY(Me.dispscale))
		  localDimension2 = New DblRect(localAtom2.DX(Me.dispscale), localAtom2.DY(Me.dispscale))
		  paramGraphics.drawLine(localDimension1.width + paramInt1 - Me.h0, localDimension1.height + paramInt2 - Me.v0, localDimension2.width + paramInt1 - Me.h0, localDimension2.height + paramInt2 - Me.v0)
		  Exit For
        Case -1

        Case 0

        Case 4

        Case 5

        Case 6

        Case 7

        Case 8 : 

		  paramGraphics.drawLine(localAtom1.DX(Me.dispscale) + paramInt1 - Me.h0, localAtom1.DY(Me.dispscale) + paramInt2 - Me.v0, localAtom2.DX(Me.dispscale) + paramInt1 - Me.h0, localAtom2.DY(Me.dispscale) + paramInt2 - Me.v0)
		  Exit For
        Case Else : 

		  paramGraphics.drawLine(localAtom1.DX(Me.dispscale) + paramInt1 - Me.h0, localAtom1.DY(Me.dispscale) + paramInt2 - Me.v0, localAtom2.DX(Me.dispscale) + paramInt1 - Me.h0, localAtom2.DY(Me.dispscale) + paramInt2 - Me.v0)
	  Exit For
        End Select
	  }
 
	  Public virtual void paintBond(java.awt.Graphics paramGraphics, keg.compound.Bond paramBond, Color paramColor, Integer paramInt1, Integer paramInt2, keg.compound.Atom paramAtom) 
	  {
		Dim localDimension4 As DblRect
        Dim localDimension3 As DblRect
        Dim localDimension2 As DblRect
        Dim localDimension1 As DblRect = localDimension2 = localDimension3 = localDimension4 = Nothing
        Dim localAtom As keg.compound.Atom = paramBond.pairAtom(paramAtom)
		paramGraphics.Color = paramColor
		Select Case paramBond.Order
        Case 1

        Select Case paramBond.Stereo
        Case 0

        Case 1

        Case 3

        Case 4

        Case 6 : 

			paramGraphics.drawLine(paramAtom.DX(Me.dispscale) + paramInt1 - Me.h0, paramAtom.DY(Me.dispscale) + paramInt2 - Me.v0, localAtom.DX(Me.dispscale) - Me.h0, localAtom.DY(Me.dispscale) - Me.v0)
		Exit For
        End Select
        Exit For
        Case 2

        If (paramAtom.Label.Length > 0)
		  {
			localDimension1 = calcInside(paramAtom.DX(Me.dispscale) + paramInt1, paramAtom.DY(Me.dispscale) + paramInt2, localAtom.DX(Me.dispscale), localAtom.DY(Me.dispscale), Me.margin_width)
		  }
 Else
		  {
			localDimension1 = New DblRect(paramAtom.DX(Me.dispscale) + paramInt1, paramAtom.DY(Me.dispscale) + paramInt2)
		  }
		  localDimension2 = calcInside(localAtom.DX(Me.dispscale), localAtom.DY(Me.dispscale), paramAtom.DX(Me.dispscale), paramAtom.DY(Me.dispscale), Me.margin_width)
		  If (localAtom.Label.Length <= 0)
		  {
			localDimension2 = New DblRect(localAtom.DX(Me.dispscale), localAtom.DY(Me.dispscale))
		  }
		  Select Case paramBond.Orientation
        Case 1

        Case 11 : 

			localDimension3 = calcSidePoint(localDimension1.width, localDimension1.height, localDimension2.width, localDimension2.height, Me.bond_spacing, 1)
			localDimension4 = calcSidePoint(localDimension2.width, localDimension2.height, localDimension1.width, localDimension1.height, Me.bond_spacing, 2)
			Exit For
        Case 0

        Case 2

        Case 12 : 

			localDimension3 = calcSidePoint(localDimension1.width, localDimension1.height, localDimension2.width, localDimension2.height, Me.bond_spacing / 2.0D, 2)
			localDimension4 = calcSidePoint(localDimension2.width, localDimension2.height, localDimension1.width, localDimension1.height, Me.bond_spacing / 2.0D, 1)
			localDimension1 = calcSidePoint(localDimension3.width, localDimension3.height, localDimension4.width, localDimension4.height, Me.bond_spacing, 1)
			localDimension2 = calcSidePoint(localDimension4.width, localDimension4.height, localDimension3.width, localDimension3.height, Me.bond_spacing, 2)
			Exit For
        Case 3

        Case 13 : 

			localDimension3 = calcSidePoint(localDimension1.width, localDimension1.height, localDimension2.width, localDimension2.height, Me.bond_spacing, 2)
			localDimension4 = calcSidePoint(localDimension2.width, localDimension2.height, localDimension1.width, localDimension1.height, Me.bond_spacing, 1)
		Exit For
        End Select
		  paramGraphics.drawLine(localDimension1.width - Me.h0, localDimension1.height - Me.v0, localDimension2.width - Me.h0, localDimension2.height - Me.v0)
		  paramGraphics.drawLine(localDimension3.width - Me.h0, localDimension3.height - Me.v0, localDimension4.width - Me.h0, localDimension4.height - Me.v0)
		  Exit For
        Case 3 : 

		  localDimension1 = New DblRect(paramAtom.DX(Me.dispscale) + paramInt1, paramAtom.DY(Me.dispscale) + paramInt2)
		  localDimension2 = New DblRect(localAtom.DX(Me.dispscale), localAtom.DY(Me.dispscale))
		  paramGraphics.drawLine(localDimension1.width - Me.h0, localDimension1.height - Me.v0, localDimension2.width - Me.h0, localDimension2.height - Me.v0)
		  localDimension3 = calcSidePoint(localDimension1.width, localDimension1.height, localDimension2.width, localDimension2.height, Me.bond_spacing, 1)
		  localDimension4 = calcSidePoint(localDimension2.width, localDimension2.height, localDimension1.width, localDimension1.height, Me.bond_spacing, 2)
		  paramGraphics.drawLine(localDimension3.width - Me.h0, localDimension3.height - Me.v0, localDimension4.width - Me.h0, localDimension4.height - Me.v0)
		  localDimension3 = calcSidePoint(localDimension1.width, localDimension1.height, localDimension2.width, localDimension2.height, Me.bond_spacing, 2)
		  localDimension4 = calcSidePoint(localDimension2.width, localDimension2.height, localDimension1.width, localDimension1.height, Me.bond_spacing, 1)
		  paramGraphics.drawLine(localDimension3.width - Me.h0, localDimension3.height - Me.v0, localDimension4.width - Me.h0, localDimension4.height - Me.v0)
		  Exit For
        Case -1

        Case 0

        Case 4

        Case 5

        Case 6

        Case 7

        Case 8 : 

		  paramGraphics.drawLine(paramAtom.DX(Me.dispscale) + paramInt1 - Me.h0, paramAtom.DY(Me.dispscale) + paramInt2 - Me.v0, localAtom.DX(Me.dispscale) - Me.h0, localAtom.DY(Me.dispscale) - Me.v0)
		  Exit For
        Case Else : 

		  paramGraphics.drawLine(paramAtom.DX(Me.dispscale) + paramInt1 - Me.h0, paramAtom.DY(Me.dispscale) + paramInt2 - Me.v0, localAtom.DX(Me.dispscale) - Me.h0, localAtom.DY(Me.dispscale) - Me.v0)
	  Exit For
        End Select
	  }
 
	  Public virtual void paintBracket(java.awt.Graphics paramGraphics, keg.compound.Bracket paramBracket) 
	  {
		paintBracket(paramGraphics,paramBracket,New DblRect(Me.h0,Me.v0))
	  }
 
	  Public virtual void paintBracket(java.awt.Graphics paramGraphics, keg.compound.Bracket paramBracket, DblRect paramDimension) 
	  {
		paramGraphics.setPaintMode()
		If ((paramBracket.Select) And ((paramBracket.SelectAll) Or (paramBracket.SelectSide = 1)))
		{
		  If (OnlyOneBracketSelected)
		  {
			Dim localVector As ArrayList = paramBracket.Sgroup
        Dim localColor As Color = paramGraphics.Color
			paramGraphics.Color = Color.blue
			Dim localObject As Object
        For k = 0 To localVector.Count - 1 Step k + 1
			  localObject = (keg.compound.Atom)localVector(k)
			  fillOval(paramGraphics, ((keg.compound.Atom)localObject).DX(Me.dispscale) - 2, ((keg.compound.Atom)localObject).DY(Me.dispscale) - 2, 4, 4)
			Next
			paramGraphics.Color = localColor
			For k = 0 To ((keg.compound.Reaction)Me.conteiner).objectNum()- 1  Step k + 1
        If ((((keg.compound.Reaction)Me.conteiner).getObject(k) Is keg.compound.Molecule)) 
			  {
				localObject = (keg.compound.Molecule)((keg.compound.Reaction)Me.conteiner).getObject(k)
				Dim n As Integer
        For n = 0 To ((keg.compound.Molecule)localObject).BondNum- 1  Step n + 1
        Dim localBond As keg.compound.Bond = ((keg.compound.Molecule)localObject).getBond(n + 1) 
				  If ((localVector.Contains(localBond.Atom1)) And (localVector.Contains(localBond.Atom2)))
				  {
					paintBond(paramGraphics, localBond, Color.blue)
				  }
				Next
			  }
			Next
		  }
		  paramGraphics.Color = Me.highC
		}
 ElseIf (paramBracket.col1 <> Nothing)
		{
		  paramGraphics.Color = paramBracket.col1
		}
 Else
		{
		  paramGraphics.Color = Me.bC
		}
		Dim i As Integer = paramBracket.DX(Me.dispscale, 1, 1)
        Dim j As Integer = paramBracket.DY(Me.dispscale, 1, 1)
        Dim k As Integer = paramBracket.DX(Me.dispscale, 1, 2)
        Dim m As Integer = paramBracket.DY(Me.dispscale, 1, 2)
		paramGraphics.drawLine(i - paramDimension.width, j - paramDimension.height, k - paramDimension.width, m - paramDimension.height)
		i = paramBracket.DX(Me.dispscale, 1, 3)
		j = paramBracket.DY(Me.dispscale, 1, 3)
		k = paramBracket.DX(Me.dispscale, 1, 2)
		m = paramBracket.DY(Me.dispscale, 1, 2)
		paramGraphics.drawLine(i - paramDimension.width, j - paramDimension.height, k - paramDimension.width, m - paramDimension.height)
		i = paramBracket.DX(Me.dispscale, 1, 3)
		j = paramBracket.DY(Me.dispscale, 1, 3)
		k = paramBracket.DX(Me.dispscale, 1, 4)
		m = paramBracket.DY(Me.dispscale, 1, 4)
		paramGraphics.drawLine(i - paramDimension.width, j - paramDimension.height, k - paramDimension.width, m - paramDimension.height)
		If ((paramBracket.Select) And ((paramBracket.SelectAll) Or (paramBracket.SelectSide = 2)))
		{
		  paramGraphics.Color = Me.highC
		}
 ElseIf (paramBracket.col2 <> Nothing)
		{
		  paramGraphics.Color = paramBracket.col2
		}
 Else
		{
		  paramGraphics.Color = Me.bC
		}
		i = paramBracket.DX(Me.dispscale, 2, 1)
		j = paramBracket.DY(Me.dispscale, 2, 1)
		k = paramBracket.DX(Me.dispscale, 2, 2)
		m = paramBracket.DY(Me.dispscale, 2, 2)
		paramGraphics.drawLine(i - paramDimension.width, j - paramDimension.height, k - paramDimension.width, m - paramDimension.height)
		i = paramBracket.DX(Me.dispscale, 2, 3)
		j = paramBracket.DY(Me.dispscale, 2, 3)
		k = paramBracket.DX(Me.dispscale, 2, 2)
		m = paramBracket.DY(Me.dispscale, 2, 2)
		paramGraphics.drawLine(i - paramDimension.width, j - paramDimension.height, k - paramDimension.width, m - paramDimension.height)
		i = paramBracket.DX(Me.dispscale, 2, 3)
		j = paramBracket.DY(Me.dispscale, 2, 3)
		k = paramBracket.DX(Me.dispscale, 2, 4)
		m = paramBracket.DY(Me.dispscale, 2, 4)
		paramGraphics.drawLine(i - paramDimension.width, j - paramDimension.height, k - paramDimension.width, m - paramDimension.height)
		If ((paramBracket.Select) And ((paramBracket.SelectAll) Or (paramBracket.SelectSide = 2)))
		{
		  paramGraphics.Color = Me.highC
		}
 ElseIf (paramBracket.col0 <> Nothing)
		{
		  paramGraphics.Color = paramBracket.col0
		}
 Else
		{
		  paramGraphics.Color = Me.blC
		}
		paramGraphics.Font = Me.bFont
		paramGraphics.drawString(paramBracket.Label, paramBracket.DX(Me.dispscale, 0, 0) - paramDimension.width, paramBracket.DY(Me.dispscale, 0, 0) - paramDimension.height)
	  }
 
	  Public virtual void paintBracket(java.awt.Graphics paramGraphics, keg.compound.Bracket paramBracket, Integer paramInt1, Integer paramInt2) 
	  {
		paramGraphics.Color = Me.highC
		If ((paramBracket.SelectAll) Or (paramBracket.SelectSide = 1))
		{
		  paramGraphics.drawLine(paramBracket.DX(Me.dispscale, 1, 1) + paramInt1 - Me.h0, paramBracket.DY(Me.dispscale, 1, 1) + paramInt2 - Me.v0, paramBracket.DX(Me.dispscale, 1, 2) + paramInt1 - Me.h0, paramBracket.DY(Me.dispscale, 1, 2) + paramInt2 - Me.v0)
		  paramGraphics.drawLine(paramBracket.DX(Me.dispscale, 1, 2) + paramInt1 - Me.h0, paramBracket.DY(Me.dispscale, 1, 2) + paramInt2 - Me.v0, paramBracket.DX(Me.dispscale, 1, 3) + paramInt1 - Me.h0, paramBracket.DY(Me.dispscale, 1, 3) + paramInt2 - Me.v0)
		  paramGraphics.drawLine(paramBracket.DX(Me.dispscale, 1, 3) + paramInt1 - Me.h0, paramBracket.DY(Me.dispscale, 1, 3) + paramInt2 - Me.v0, paramBracket.DX(Me.dispscale, 1, 4) + paramInt1 - Me.h0, paramBracket.DY(Me.dispscale, 1, 4) + paramInt2 - Me.v0)
		}
		If ((paramBracket.SelectAll) Or (paramBracket.SelectSide = 2))
		{
		  paramGraphics.drawLine(paramBracket.DX(Me.dispscale, 2, 1) + paramInt1 - Me.h0, paramBracket.DY(Me.dispscale, 2, 1) + paramInt2 - Me.v0, paramBracket.DX(Me.dispscale, 2, 2) + paramInt1 - Me.h0, paramBracket.DY(Me.dispscale, 2, 2) + paramInt2 - Me.v0)
		  paramGraphics.drawLine(paramBracket.DX(Me.dispscale, 2, 2) + paramInt1 - Me.h0, paramBracket.DY(Me.dispscale, 2, 2) + paramInt2 - Me.v0, paramBracket.DX(Me.dispscale, 2, 3) + paramInt1 - Me.h0, paramBracket.DY(Me.dispscale, 2, 3) + paramInt2 - Me.v0)
		  paramGraphics.drawLine(paramBracket.DX(Me.dispscale, 2, 3) + paramInt1 - Me.h0, paramBracket.DY(Me.dispscale, 2, 3) + paramInt2 - Me.v0, paramBracket.DX(Me.dispscale, 2, 4) + paramInt1 - Me.h0, paramBracket.DY(Me.dispscale, 2, 4) + paramInt2 - Me.v0)
		}
	  }
 
	  Private void paintAtomMark(java.awt.Graphics paramGraphics, keg.compound.Atom paramAtom) 
	  {
		If (paramGraphics = Nothing)
		{
		  paramGraphics = Graphics
		}
		paramGraphics.Font = Me.lFont
		paramGraphics.Color = Me.foreC
		paramGraphics.XORMode = Me.backC
		Me.fm = paramGraphics.FontMetrics
        Me.fHeight = Me.fm.Height
        Dim i As Integer = paramAtom.DX(Me.dispscale)
        Dim j As Integer = paramAtom.DY(Me.dispscale)
        If (paramAtom.Express_group)
		{
		  Dim k As Integer = Me.fm.stringWidth(paramAtom.DrawLabel)
        If (paramAtom.GroupLabelDirection = 1)
		  {
			Dim - As i =  k - 3 
		  }
 ElseIf (paramAtom.GroupLabelDirection = 3)
		  {
			Dim - As i =  k / 2 
			j += Me.fHeight / 2
		  }
 ElseIf (paramAtom.GroupLabelDirection = 2)
		  {
			Dim - As i =  k / 2 
			Dim - As j =  Me.fHeight / 2 
		  }
		}
		If (paramAtom.Label.Length = 0)
		{
		  paramGraphics.drawRect(i - 3 - Me.h0, j - 3 - Me.v0, 7, 7)
		}
 Else
		{
		  Me.fHalfW = (Me.fm.stringWidth(paramAtom.Label.Substring(0, 1)) / 2)
        Me.fWidth = Me.fm.stringWidth(paramAtom.Label)
		  paramGraphics.drawRect(i - 2 - Me.h0 - Me.fHalfW, j - Me.v0 - Me.fHeight / 2, Me.fWidth + 2, Me.fHeight)
		}
	  }
 
	  Private void paintBondMark(java.awt.Graphics paramGraphics, keg.compound.Bond paramBond) 
	  {
		Dim localAtom1 As keg.compound.Atom = paramBond.Atom1
        Dim localAtom2 As keg.compound.Atom = paramBond.Atom2
        If (paramGraphics = Nothing)
		{
		  paramGraphics = Graphics
		}
		paramGraphics.Color = Me.foreC
		paramGraphics.XORMode = Me.backC
		paramGraphics.drawOval((localAtom1.DX(Me.dispscale) + localAtom2.DX(Me.dispscale)) / 2 - Me.h0 - 3, (localAtom1.DY(Me.dispscale) + localAtom2.DY(Me.dispscale)) / 2 - Me.v0 - 3, 6, 6)
	  }
 
	  Private void paintReactionArrowMark(java.awt.Graphics paramGraphics, keg.compound.ReactionArrow paramReactionArrow) 
	  {
		If (paramGraphics = Nothing)
		{
		  paramGraphics = Graphics
		}
		paramGraphics.Color = Me.foreC
		paramGraphics.XORMode = Me.backC
		paramGraphics.drawOval((paramReactionArrow.DX1() + paramReactionArrow.DX2()) / 2 - Me.h0 - 3, (paramReactionArrow.DY1() + paramReactionArrow.DY2()) / 2 - Me.v0 - 3, 6, 6)
	  }
 
	  Private void paintBracketLabelMark(java.awt.Graphics paramGraphics, keg.compound.Bracket paramBracket) 
	  {
		paramGraphics.Color = Me.foreC
		paramGraphics.XORMode = Me.backC
		Dim localRectangle As Rectangle = paramBracket.getBracketLabelMarkArea(Me.dispscale)
		paramGraphics.fillRect(localRectangle.x - Me.h0, localRectangle.y - Me.v0 + 3, localRectangle.width, localRectangle.height)
	  }
 
	  Private void paintBracketMark(java.awt.Graphics paramGraphics, keg.compound.Bracket paramBracket) 
	  {
		If (paramGraphics = Nothing)
		{
		  paramGraphics = Graphics
		}
		paramGraphics.Color = Me.foreC
		paramGraphics.XORMode = Me.backC
		paramGraphics.drawOval(paramBracket.DX(Me.dispscale, 1, 1) - Me.h0 - 3, paramBracket.DY(Me.dispscale, 1, 1) - Me.v0 - 3, 6, 6)
		paramGraphics.drawOval(paramBracket.DX(Me.dispscale, 1, 4) - Me.h0 - 3, paramBracket.DY(Me.dispscale, 1, 4) - Me.v0 - 3, 6, 6)
		paramGraphics.drawOval(paramBracket.DX(Me.dispscale, 2, 1) - Me.h0 - 3, paramBracket.DY(Me.dispscale, 2, 1) - Me.v0 - 3, 6, 6)
		paramGraphics.drawOval(paramBracket.DX(Me.dispscale, 2, 4) - Me.h0 - 3, paramBracket.DY(Me.dispscale, 2, 4) - Me.v0 - 3, 6, 6)
	  }
 
	  Private DblRect calcInside(Integer paramInt1, Integer paramInt2, Integer paramInt3, Integer paramInt4, Integer paramInt5) 
	  {
		Dim i As Integer = paramInt1
        Dim j As Integer = paramInt2
        Dim k As Integer = (Integer)Math.Sqrt((paramInt3 - paramInt1) *(paramInt3 - paramInt1) +(paramInt4 - paramInt2) *(paramInt4 - paramInt2)) 
		If (k > paramInt5)
		{
		  i = paramInt1 + (paramInt3 - paramInt1) * paramInt5 / k
		  j = paramInt2 + (paramInt4 - paramInt2) * paramInt5 / k
		}
		Return New DblRect(i, j)
	  }
 
	  Private DblRect calcSidePoint(Integer paramInt1, Integer paramInt2, Integer paramInt3, Integer paramInt4, Double paramDouble, Integer paramInt5) 
	  {
		Dim i As Integer = paramInt1
        Dim j As Integer = paramInt2
        Dim d As Double = Math.Sqrt((paramInt3 - paramInt1) * (paramInt3 - paramInt1) + (paramInt4 - paramInt2) * (paramInt4 - paramInt2))
        Dim k As Integer = (Integer)d 
		Dim m As Integer = (Integer)(d * paramDouble)
        Select Case paramInt5
        Case 1 : 

		  i += (paramInt2 - paramInt4) * m / k
		  j += (paramInt3 - paramInt1) * m / k
		  Exit For
        Case 2 : 

		  i += (paramInt4 - paramInt2) * m / k
		  j += (paramInt1 - paramInt3) * m / k
	  Exit For
        End Select
        Return New DblRect(i, j)
	  }
 
	  Private DblRect calcSidePoint(Integer paramInt1, Integer paramInt2, Integer paramInt3, Integer paramInt4, Integer paramInt5, Integer paramInt6) 
	  {
		Dim i As Integer = paramInt1
        Dim j As Integer = paramInt2
        Dim d As Double = Math.Sqrt((paramInt3 - paramInt1) * (paramInt3 - paramInt1) + (paramInt4 - paramInt2) * (paramInt4 - paramInt2))
        Dim k As Integer = (Integer)d 
		Select Case paramInt6
        Case 1 : 

		  i += (paramInt2 - paramInt4) * paramInt5 / k
		  j += (paramInt3 - paramInt1) * paramInt5 / k
		  Exit For
        Case 2 : 

		  i += (paramInt4 - paramInt2) * paramInt5 / k
		  j += (paramInt1 - paramInt3) * paramInt5 / k
	  Exit For
        End Select
        Return New DblRect(i, j)
	  }
 
	  Public virtual void mouseClicked(java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		If (paramMouseEvent.ClickCount <> 0)
		{
		  mouseClickedEvent(paramMouseEvent)
		}
	  }
 
	  Public virtual void mousePressedForLongTime(java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		If (isRingMode(Me.editmode.draw))
		{
		  stopClickTimer()
		  stopTimer()
		  mouseUp(Me.pressEvent, Me.pressEvent.X, Me.pressEvent.Y)
		}
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        Public virtual void mouseClickedEvent(java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		If (paramMouseEvent = Nothing)
		{
		  stopClickTimer()
		  stopTimer()
		  If (Me.pressEvent <> Nothing)
		  {
			mouseClick(Me.pressEvent, Me.pressEvent.X, Me.pressEvent.Y)
		  }
		}
 ElseIf ((paramMouseEvent.ClickCount = 1) Or (paramMouseEvent.ClickCount > 2))
		{
		  startClickTimer()
		}
 ElseIf (Me.clickTimer.Running)
		{
		  stopClickTimer()
		  stopTimer()
		  mouseDoubleClickedEvent(paramMouseEvent)
		}
	  }
 
	  Private void mouseDoubleClickedEvent(java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		Me.upP.x = Me.downP.x
        Me.upP.y = Me.downP.y
        Dim @bool As Boolean =  paramMouseEvent.ShiftDown 
		Dim localPoint1 As java.awt.Point
        Dim localPoint2 As java.awt.Point
        If ((Me.editmode.operation = 3) And (Me.selectedText <> Nothing))
		{
		  localPoint1 = Me.parentK.kegF.Location
		  localPoint2 = New java.awt.Point(localPoint1.x + Me.pressEvent.X, localPoint1.y + Me.pressEvent.Y)
		  showTextPreferenceDialog(Me.selectedText, localPoint2)
		}
		If ((Me.editmode.chemobject <> Nothing) And (Not @bool) And ((Me.editmode.chemobject Is keg.compound.Bracket))) 
		{
		  ((keg.compound.Bracket)Me.editmode.chemobject).selectAll(Me.editmode)
		}
 ElseIf ((Me.editmode.bracket <> Nothing) And (Me.editmode.operation = 3))
		{
		  showBracketTextFiled(Me.pressEvent.X, Me.pressEvent.Y)
		}
 ElseIf ((Me.editmode.atom <> Nothing) And (Me.editmode.operation <> 4))
		{
		  If (Me.pressEvent <> Nothing)
		  {
			localPoint1 = Me.parentK.kegF.Location
			localPoint2 = New java.awt.Point(localPoint1.x + Me.pressEvent.X, localPoint1.y + Me.pressEvent.Y)
			If (Me.editmode.atom.Express_group)
			{
			  If (Me.grp_jd = Nothing)
			  {
				Me.grp_jd = New javax.swing.JDialog(Me.parentK.kegF, True)
        Me.grp_jd.ContentPane.Layout = New java.awt.BorderLayout(5, 5)
				localObject1 = New java.awt.GridLayout(0, 2, 2, 5)
				((java.awt.GridLayout)localObject1).Hgap = 2
				((java.awt.GridLayout)localObject1).Hgap = 4
				localObject2 = New javax.swing.JPanel((java.awt.LayoutManager)localObject1)
				((javax.swing.JPanel)localObject2).add(New javax.swing.JLabel("Label:"))
				Me.grp_jd_field = New javax.swing.JTextField()
				((javax.swing.JPanel)localObject2).add(Me.grp_jd_field)
				((javax.swing.JPanel)localObject2).add(New javax.swing.JLabel(" Side:"))
				Me.grp_jd_combo = New javax.swing.JComboBox(New String() 
				{
					"Right", "Left", "Above", "Below"
				}
)
				((javax.swing.JPanel)localObject2).add(Me.grp_jd_combo)
				Me.grp_jd.ContentPane.add((java.awt.Component)localObject2, "Center")
				localObject1 = New javax.swing.JPanel()
				((javax.swing.JPanel)localObject1).Layout = New java.awt.FlowLayout(2)
				Me.grp_jd_ok = New javax.swing.JButton("OK")
        Me.grp_jd_ok.addActionListener(Me)
        Me.grp_jd_cancel = New javax.swing.JButton("Canncel")
        Me.grp_jd_cancel.addActionListener(Me)
				((javax.swing.JPanel)localObject1).add(Me.grp_jd_cancel)
				((javax.swing.JPanel)localObject1).add(Me.grp_jd_ok)
				Me.grp_jd.ContentPane.add((java.awt.Component)localObject1, "South")
			  }
			  Me.grp_jd_field.Text = Me.editmode.atom.DrawLabel
        Me.grp_jd_combo.SelectedIndex = Me.editmode.atom.GroupLabelDirection
        Me.grp_jd.pack()
        Dim localObject1 As Object = Me.grp_jd.Size
			  ((DblRect)localObject1).width = (((DblRect)localObject1).width < 250 ? 250 : ((DblRect)localObject1).width)

			  Me.grp_jd.Size = (DblRect)localObject1
			  Dim localObject2 As Object = java.awt.Toolkit.DefaultToolkit.ScreenSize
        Dim localPoint3 As java.awt.Point = New java.awt.Point(localPoint2.x + 70, localPoint2.y)
        If (((DblRect)localObject2).width < localPoint3.x + ((DblRect)localObject1).width) 
			  {
				Dim - As localPoint2.x = ((DblRect)localObject1).width 
			  }
			  If (((DblRect)localObject2).height < localPoint3.y + ((DblRect)localObject1).height) 
			  {
				Dim - As localPoint3.y =  localPoint3.y +((DblRect)localObject1).height -((DblRect)localObject2).height 
			  }
			  Me.grp_jd.Location = localPoint3
        Me.grp_jd.show()
			}
 Else
			{
			  showAtomPreferenceDialog(Me.editmode.atom, localPoint2)
			}
		  }
		}
 ElseIf (Me.editmode.bond <> Nothing)
		{
		  If (Me.editmode.operation = 1)
		  {
			mouseClickDraw(paramMouseEvent)
		  }
 ElseIf (Me.pressEvent <> Nothing)
		  {
			localPoint1 = Me.parentK.kegF.Location
			localPoint2 = New java.awt.Point(localPoint1.x + Me.pressEvent.X, localPoint1.y + Me.pressEvent.Y)
			showBondPreferenceDialog(Me.editmode.bond, localPoint2)
		  }
		}
		Me.editmode.resetArea()
		repaint()
		If (Me.parentK <> Nothing)
		{
		  Me.parentK.checkButton()
		}
		Me.editmode.status = 0
	  }
 
	  Public virtual void showAtomPreferenceDialog(keg.compound.Atom paramAtom, java.awt.Point paramPoint) 
	  {
		AtomPreferenceDialog.show(paramPoint, Me)
	  }
 
	  Public virtual void showBondPreferenceDialog(keg.compound.Bond paramBond, java.awt.Point paramPoint) 
	  {
		BondPreferenceDialog.show(paramPoint, Me)
	  }
 
	  Public virtual void mousePressed(java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		If (Me.activeFlag = True)
		{
		  Me.activeFlag = False
        Return
		}
		Me.activeFlag = False
        Me.draggedFlag = False
		startTimer(paramMouseEvent)
		mouseDown(paramMouseEvent, paramMouseEvent.X, paramMouseEvent.Y)
	  }
 
	  Public virtual void mouseReleased(java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		If ((paramMouseEvent.ClickCount = 0) Or ((Me.draggedFlag = True) And (MAC_OS_X <> True) And (paramMouseEvent.ClickCount = 1)))
		{
		  mouseUp(paramMouseEvent, paramMouseEvent.X, paramMouseEvent.Y)
		  stopTimer()
		}
	  }
 
	  Private Boolean isAPartOfBenzene(keg.compound.Atom paramAtom) 
	  {
		If ((paramAtom <> Nothing) And (paramAtom.numBond() > 1))
		{
		  If (Not paramAtom.getsp3())
		  {
			Return True
		  }
		  Dim i As Integer
        For i = 0 To paramAtom.numBond() - 1 Step i + 1
        Dim localBond As keg.compound.Bond = paramAtom.getBond(i)
        If ((localBond.Order = 3) Or (localBond.Order = 2))
			{
			  Return True
			}
		  Next
		}
		Return False
	  }
 
	  Private Boolean isRingMode(Integer paramInt) 
	  {
		Select Case paramInt
        Case 0

        Case 1

        Case 2

        Case 3

        Case 4

        Case 5

        Case 10

        Return False
        End Select
        Return True
	  }
 
	  Public virtual Boolean mouseClick(java.awt.Event.MouseEvent paramMouseEvent, Integer paramInt1, Integer paramInt2) 
	  {
		Me.upP.x = Me.downP.x
        Me.upP.y = Me.downP.y
        Dim @bool As Boolean =  False 
		If (Not Me.eventmode)
		{
		  Return False
		}
		If (Me.editmode.status = 0)
		{
		  Return False
		}
		If (Me.editmode.near_object <> Nothing)
		{
		  If ((Me.editmode.near_object Is keg.compound.Atom))
		  {
			paintAtomMark(Nothing, (keg.compound.Atom)Me.editmode.near_object)
		  }
 ElseIf ((Me.editmode.near_object Is keg.compound.Bond))
		  {
			paintBondMark(Nothing, (keg.compound.Bond)Me.editmode.near_object)
		  }
		  Me.editmode.near_object = Nothing
		}
		Select Case Me.editmode.operation
        Case 1

        If (paramMouseEvent.MetaDown)
		  {
			@bool = False
		  }
 ElseIf (Me.editmode.draw = 10)
		  {
			@bool = mouseUpDraw_CarbonChain(paramMouseEvent, paramInt1, paramInt2)
		  }
 ElseIf (paramMouseEvent.AltDown)
		  {
			If (Not isRingMode(Me.editmode.draw))
			{
			  @bool = mouseUpDraw_CarbonChain(paramMouseEvent, paramInt1, paramInt2)
			}
 Else
			{
			  @bool = False
			}
		  }
 ElseIf ((Me.editmode.atom <> Nothing) And (isRingMode(Me.editmode.draw)) And (Me.editmode.atom.numBond() > 1))
		  {
			Dim i As Integer = Me.editmode.draw
        Me.editmode.draw = 0
			@bool = mouseClickDraw(paramMouseEvent)
			Me.editmode.operation = 1
        Me.editmode.draw = i
        Me.parentK.clearButton()
        Me.parentK.checkButton()
        Me.downP.x = (Me.upP.x = Me.editmode.atom.DX(Me.dispscale))
        Me.downP.y = (Me.upP.y = Me.editmode.atom.DY(Me.dispscale))
        Dim localAtom As keg.compound.Atom = Me.editmode.atom
			@bool = mouseClickDraw(paramMouseEvent)
			localAtom.select(True, Me.editmode)
		  }
 Else
		  {
			@bool = mouseClickDraw(paramMouseEvent)
		  }
		  Exit For
        Case 2

        If (Not Me.parentK.ShrinkMode)
		  {
			@bool = mouseUpErase(paramInt1, paramInt2)
		  }
 Else
		  {
			java.awt.Toolkit.DefaultToolkit.beep()
			JOptionPane.showMessageDialog(this, "When 'Shrink Mode', Can not erase some thing.", "Can't erase some thing.", 2)
		  }
		  Exit For
        Case 3

        Case 8

        Select Case Me.editmode.status
        Case 2

        Case 3 : 

			mouseClickSelect(paramMouseEvent)
			Exit For
        Case 4 : 

			mouseUpRotate(paramInt1, paramInt2)
			Exit For
        Case 5 : 

			mouseUpResize(paramInt1, paramInt2)
		Exit For
        End Select
        Exit For
        Case 7 : 

		  mouseUpBracket(paramInt1, paramInt2)
	  Exit For
        End Select
        If (@bool) 
		{
		  Dim localReaction As keg.compound.Reaction = (keg.compound.Reaction)Me.conteiner 
		  Dim j As Integer
        For j = 0 To localReaction.objectNum() - 1 Step j + 1
        Dim localChemObject As keg.compound.ChemObject = localReaction.getObject(j)
        If ((localChemObject Is keg.compound.Molecule))
			{
			  ((keg.compound.Molecule)localChemObject).resetKEGGAtomName()
			}
		  Next
		}
		If (Me.editmode.status <> 6)
		{
		  Me.editmode.status = 0
		}
		Me.parentK.checkButton()
        Return False
	  }
 
	  Public virtual Boolean mouseUp(java.awt.Event.MouseEvent paramMouseEvent, Integer paramInt1, Integer paramInt2) 
	  {
		Me.upP.x = (paramInt1 + Hoffset)
        Me.upP.y = (paramInt2 + Voffset)
        Dim @bool As Boolean =  False 
		If (Not Me.eventmode)
		{
		  Return False
		}
		If (Me.editmode.status = 0)
		{
		  Return False
		}
		If (Me.editmode.near_object <> Nothing)
		{
		  If ((Me.editmode.near_object Is keg.compound.Atom))
		  {
			paintAtomMark(Nothing, (keg.compound.Atom)Me.editmode.near_object)
		  }
 ElseIf ((Me.editmode.near_object Is keg.compound.Bond))
		  {
			paintBondMark(Nothing, (keg.compound.Bond)Me.editmode.near_object)
		  }
		  Me.editmode.near_object = Nothing
		}
		Dim j As Integer
        If ((Me.editmode.operation = 3) And (Me.selectedText <> Nothing))
		{
		  Dim i As Integer = Me.upP.x - Me.downP.x
		  j = Me.upP.y - Me.downP.y
		  Me.selectedText.moveBy(i, j)
        Return @bool
		}
		Dim localObject As Object
        Select Case Me.editmode.operation
        Case 1

        If (paramMouseEvent.MetaDown)
		  {
			@bool = False
		  }
 ElseIf (Me.editmode.draw = 10)
		  {
			@bool = mouseUpDraw_CarbonChain(paramMouseEvent, paramInt1, paramInt2)
		  }
 ElseIf (paramMouseEvent.AltDown)
		  {
			Select Case Me.editmode.draw
        Case 0

        Case 1

        Case 2

        Case 3

        Case 4

        Case 5

        Case 10 : 

			  @bool = mouseUpDraw_CarbonChain(paramMouseEvent, paramInt1, paramInt2)
		  Exit For
        End Select
			@bool = False
		  }
 Else
		  {
			@bool = mouseUpDraw(paramMouseEvent, paramInt1, paramInt2)
		  }
		  Exit For
        Case 2

        If (ShrinkMode = True)
		  {
			java.awt.Toolkit.DefaultToolkit.beep()
			JOptionPane.showMessageDialog(this, "When 'Shrink Mode', Can not erase some thing.", "Can't erase some thing.", 2)
		  }
 Else
		  {
			@bool = mouseUpSelect(paramInt1, paramInt2, paramMouseEvent)
			If (Me.editmode.selected.Count > 0)
			{
			  localObject = keg.draw.util.PropertiesManager.getProperties("SELECT_DELETE_ALERT")
			  j = 1
			  If ((localObject <> Nothing) And (((String)localObject).Equals("false"))) 
			  {
				j = 0
			  }
			  Dim k As Integer = 0
        If (j <> 0)
			  {
				keg.OKCancelWithCheckBoxDialog.getDialog(Me.parentK.kegF, keg.KegFrame.getResource("DeleteAllSelectedObjectMessage"))
				Dim m As Integer = keg.OKCancelWithCheckBoxDialog.Status
        If ((m = 1) Or (m = 3))
				{
				  k = 1
				}
				If ((m = 2) Or (m = 3))
				{
				  keg.draw.util.PropertiesManager.setProperties("SELECT_DELETE_ALERT", "false")
				}
			  }
 Else
			  {
				k = 0
			  }
			  If (k = 0)
			  {
				Me.parentK.deleteSelectObject()
			  }
			}
		  }
		  Exit For
        Case 3

        Case 8

        If ((Me.upP.x = Me.downP.x) And (Me.upP.y = Me.downP.y))
		  {
			Me.editmode.status = 2
		  }
		  Select Case Me.editmode.status
        Case 3 : 

			mouseUpMove(paramInt1, paramInt2, paramMouseEvent)
			Exit For
        Case 2 : 

			mouseUpSelect(paramInt1, paramInt2, paramMouseEvent)
			Exit For
        Case 4 : 

			mouseUpRotate(paramInt1, paramInt2)
			Exit For
        Case 5 : 

			mouseUpResize(paramInt1, paramInt2)
		Exit For
        End Select
        Exit For
        Case 7 : 

		  mouseUpBracket(paramInt1, paramInt2)
	  Exit For
        End Select
        If (@bool) 
		{
		  localObject = (keg.compound.Reaction)Me.conteiner
		  For j = 0 To ((keg.compound.Reaction)localObject).objectNum()- 1  Step j + 1
        Dim localChemObject As keg.compound.ChemObject = ((keg.compound.Reaction)localObject).getObject(j) 
			If ((localChemObject Is keg.compound.Molecule))
			{
			  ((keg.compound.Molecule)localChemObject).resetKEGGAtomName()
			}
		  Next
		}
		If (Me.editmode.status <> 6)
		{
		  Me.editmode.status = 0
		}
		Me.parentK.checkButton()
        Return False
	  }
 
	  Public virtual void mouseEntered(java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		Me.activeFlag = False
		stopTimer()
	  }
 
	  Public virtual void mouseExited(java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		Me.activeFlag = False
		stopTimer()
		stopClickTimer()
	  }
 
	  Public virtual void mouseDragged(java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		Me.draggedFlag = True
		stopTimer()
		stopClickTimer()
		mouseDrag(paramMouseEvent, paramMouseEvent.X, paramMouseEvent.Y)
	  }
 
	  Public virtual void mouseMoved(java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		stopTimer()
		mouseMove(paramMouseEvent, paramMouseEvent.X, paramMouseEvent.Y)
	  }
 
	  Public virtual void keyTyped(java.awt.Event.KeyEvent paramKeyEvent) 
	  {
		stopTimer()
	  }
 
	  Public virtual void keyPressed(java.awt.Event.KeyEvent paramKeyEvent) 
	  {
		stopTimer()
	  }
 
	  Public virtual void keyReleased(java.awt.Event.KeyEvent paramKeyEvent) 
	  {
		stopTimer()
		Me.parentK.doKeyEvent(paramKeyEvent, paramKeyEvent.KeyCode)
	  }
 
	  Private void startTimer(java.awt.Event.MouseEvent paramMouseEvent) 
	  {
		Me.pressEvent = paramMouseEvent
        Me.timer.start()
	  }
 
	  Private void stopTimer() 
	  {
		Me.timer.stop()
	  }
 
	  Private void startClickTimer() 
	  {
		Me.clickTimer.start()
	  }
 
	  Private void stopClickTimer() 
	  {
		Me.clickTimer.stop()
	  }
 
	  Public virtual void actionPerformed(java.awt.Event.ActionEvent paramActionEvent) 
	  {
		Dim localObject As Object = paramActionEvent.Source
        If (localObject.Equals(Me.timer))
		{
		  mousePressedForLongTime(Nothing)
		  stopClickTimer()
		  stopTimer()
		}
 ElseIf (localObject.Equals(Me.clickTimer))
		{
		  mouseClickedEvent(Nothing)
		  stopClickTimer()
		  stopTimer()
		}
 Else
		{
		  Dim str As String
        If (localObject.Equals(Me.grp_jd_ok))
		  {
			str = Me.grp_jd_field.Text
			If ((str <> Nothing) And (str.Length > 0))
			{
			  Me.editmode.atom.Label = str
        Me.editmode.atom.GroupLabelDirection = Me.grp_jd_combo.SelectedIndex
			}
			Me.grp_jd.hide()
		  }
 ElseIf (localObject.Equals(Me.grp_jd_cancel))
		  {
			Me.grp_jd.hide()
		  }
 ElseIf (localObject.Equals(Me.bracket_text))
		  {
			Me.editmode.status = 0
			str = Me.bracket_text.Text
			If ((Me.editmode.bracket Is keg.compound.Bracket))
			{
			  Me.editmode.UNDO = (keg.compound.Reaction)Me.conteiner
			  Dim localBracket As keg.compound.Bracket = Me.editmode.bracket
			  localBracket.Label = str
			  Dim localGraphics As java.awt.Graphics = Graphics
			  localGraphics.Font = Me.bFont
			  Me.fm = localGraphics.FontMetrics
			  localBracket.Size = New DblRect(Me.fm.stringWidth(str), Me.fm.Height - Me.fHeight_discount)
			}
			Me.bracket_text.hide()
        Me.bracket_text.resize(0, 0)
			repaint()
		  }
		}
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        Public virtual Boolean checkBondsNumber(keg.compound.Bond paramBond) 
	  {
		If (paramBond = Nothing)
		{
		  Return True
		}
		Return (checkBondsNumber(paramBond.Atom1)) && (checkBondsNumber(paramBond.Atom2))
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        Public virtual Boolean checkBondsNumber(keg.compound.Atom paramAtom) 
	  {
		Dim i As Integer = paramAtom.LimitNumberOfConnections
        If (Me.editmode.atom <> Nothing)
		{
		  i = paramAtom.MaxLimitNumberOfConnections
		}
		If (paramAtom.NumberOfConnections > i)
		{
		  Dim str As String = "The number of connection was exceeded."
		  str = str + " (" + paramAtom.Label + ":" + paramAtom.LimitNumberOfConnections + "->" + paramAtom.NumberOfConnections + ")"
		  Dim j As Integer = MessageBox.Show(Me.parentK.kegF, str, "Warning",, MessageBoxIcon.None)
        Return j = 0
		}
		Return True
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        Public virtual Boolean isTooMuchBondsNumber(keg.compound.Bond paramBond) 
	  {
		If (paramBond = Nothing)
		{
		  Return False
		}
		Return (isTooMuchBondsNumber(paramBond.Atom1)) ||(isTooMuchBondsNumber(paramBond.Atom2))
	  }
 
	  <MethodImpl(MethodImplOptions.Synchronized)>
        Public virtual Boolean isTooMuchBondsNumber(keg.compound.Atom paramAtom) 
	  {
		Return (paramAtom.PerviuseNumberOfConnections <= paramAtom.LimitNumberOfConnections) && (paramAtom.NumberOfConnections > paramAtom.LimitNumberOfConnections)
	  }
 
	  Public Overridable ReadOnly Property PointOfCanvasCenter() As DblRect
            Get
                Dim localDimension As DblRect = Me.parentK.Size
                Return New DblRect(localDimension.width / 2 + Hoffset, localDimension.height / 2 + Voffset)
            End Get
        End Property

        Public virtual void initRfr() 
	  {
		Me.rfr.initLocation()
	  }
 
	  Public virtual void painting(java.awt.Graphics paramGraphics) 
	  {
		If ((Me.conteiner Is keg.compound.Reaction))
		{
		  paintReaction(paramGraphics, (keg.compound.Reaction)Me.conteiner)
		  paintTexts(paramGraphics, ((keg.compound.Reaction)Me.conteiner).Texts)
		}
	  }
 
	  Public Overridable Property Dispscale() As Double
            Get
                Return Me.dispscale
            End Get
            Set(ByVal Value As Double)
                Me.dispscale = value
            End Set
        End Property


        Private ReadOnly Property OnlyOneBracketSelected() As Boolean
            Get
                Dim i As Integer = 0
                Dim j As Integer
                For j = 0 To Me.editmode.selected.Count - 1 Step j + 1
                    If ((Me.editmode.selected(j) Is keg.compound.Bracket)) Then
			  {
				If (((keg.compound.Bracket)Me.editmode.selected(j)).SelectAll) 
				{
				  i = i + 1
                            If (i > 1) Then
				  {
					Return False
				  }
				}
			  }
 Else
			  {
				Return False
			  }
            Next
                Return i = 1
            End Get
        End Property

        Private void drawLine(java.awt.Graphics paramGraphics, Integer paramInt1, Integer paramInt2, Integer paramInt3, Integer paramInt4) 
	  {
		paramGraphics.drawLine(paramInt1 - Hoffset, paramInt2 - Voffset, paramInt3 - Hoffset, paramInt4 - Voffset)
	  }
 
	  Private void drawRect(java.awt.Graphics paramGraphics, Integer paramInt1, Integer paramInt2, Integer paramInt3, Integer paramInt4) 
	  {
		paramGraphics.drawRect(paramInt1 - Hoffset, paramInt2 - Voffset, paramInt3, paramInt4)
	  }
 
	  Private void fillRect(java.awt.Graphics paramGraphics, Integer paramInt1, Integer paramInt2, Integer paramInt3, Integer paramInt4) 
	  {
		paramGraphics.fillRect(paramInt1 - Hoffset, paramInt2 - Voffset, paramInt3, paramInt4)
	  }
 
	  Private void drawString(java.awt.Graphics paramGraphics, String paramString, Integer paramInt1, Integer paramInt2) 
	  {
		paramGraphics.drawString(paramString, paramInt1 - Hoffset, paramInt2 - Voffset)
	  }
 
	  Private void drawOval(java.awt.Graphics paramGraphics, Integer paramInt1, Integer paramInt2, Integer paramInt3, Integer paramInt4) 
	  {
		paramGraphics.drawOval(paramInt1 - Hoffset, paramInt2 - Voffset, paramInt3, paramInt4)
	  }
 
	  Private void fillOval(java.awt.Graphics paramGraphics, Integer paramInt1, Integer paramInt2, Integer paramInt3, Integer paramInt4) 
	  {
		paramGraphics.drawOval(paramInt1 - Hoffset, paramInt2 - Voffset, paramInt3, paramInt4)
	  }
 
	  Public virtual void windowOpened(java.awt.Event.WindowEvent paramWindowEvent) 
	  {
	  }
 
	  Public virtual void windowClosing(java.awt.Event.WindowEvent paramWindowEvent) 
	  {
	  }
 
	  Public virtual void windowClosed(java.awt.Event.WindowEvent paramWindowEvent) 
	  {
	  }
 
	  Public virtual void windowIconified(java.awt.Event.WindowEvent paramWindowEvent) 
	  {
	  }
 
	  Public virtual void windowDeiconified(java.awt.Event.WindowEvent paramWindowEvent) 
	  {
	  }
 
	  Public virtual void windowActivated(java.awt.Event.WindowEvent paramWindowEvent) 
	  {
		Me.activeFlag = True
	  }
 
	  Public virtual void windowDeactivated(java.awt.Event.WindowEvent paramWindowEvent) 
	  {
	  }
 
	  Public virtual void setClipboard() 
	  {
		If (Me.clipboard = Nothing)
		{
		  Me.clipboard = java.awt.Toolkit.DefaultToolkit.SystemClipboard
		}
	  }
 
	  Public virtual Rectangle getBoundSize(java.awt.Graphics paramGraphics, keg.compound.Reaction paramReaction) 
	  {
		paramGraphics.Font = Me.lFont
		Dim localFontMetrics As java.awt.FontMetrics = paramGraphics.FontMetrics
        Dim d As Double = Dispscale
        Dim localRectangle As Rectangle = New Rectangle()
        If ((paramReaction.objectNum() = 0) And (paramReaction.bracketNum() = 0))
		{
		  Return localRectangle
		}
		Dim i As Integer = Integer.MaxValue
        Dim j As Integer = Integer.MinValue
        Dim k As Integer = Integer.MaxValue
        Dim m As Integer = Integer.MinValue
        Dim n As Integer = Integer.MaxValue
        Dim i1 As Integer = Integer.MinValue
        Dim i2 As Integer = Integer.MaxValue
        Dim i3 As Integer = Integer.MinValue
        Dim localObject1 As Object
        Dim i6 As Integer
        Dim i4 As Integer
        For i4 = 0 To paramReaction.objectNum() - 1 Step i4 + 1
		  localObject1 = paramReaction.getObject(i4)
		  Dim localObject2 As Object
        If ((localObject1 Is keg.compound.Molecule))
		  {
			localObject2 = ((keg.compound.Molecule)localObject1).moleculeRange(localFontMetrics, d)
			n = localObject2(0)
			i2 = localObject2(1)
			i1 = localObject2(2)
			i3 = localObject2(3)
		  }
 Else
		  {
			If ((localObject1 Is keg.compound.Bracket))
			{
			  localObject2 = (keg.compound.Bracket)localObject1
			  n = ((keg.compound.Bracket)localObject2).DX(d, 0, 0)
			  i1 = ((keg.compound.Bracket)localObject2).DX(d, 0, 0) + ((keg.compound.Bracket)localObject2).size().width + 3
			  i2 = ((keg.compound.Bracket)localObject2).DY(d, 0, 0) - ((keg.compound.Bracket)localObject2).size().height
			  i3 = ((keg.compound.Bracket)localObject2).DY(d, 0, 0)
			  For i6 = 1 To 2 Step i6 + 1
        Dim i7 As Integer
        For i7 = 1 To 4 Step i7 + 1
        If (((keg.compound.Bracket)localObject2).DX(d, i6, i7) < n) 
				  {
					n = ((keg.compound.Bracket)localObject2).DX(d, i6, i7)
				  }
				  If (((keg.compound.Bracket)localObject2).DX(d, i6, i7) > i1) 
				  {
					i1 = ((keg.compound.Bracket)localObject2).DX(d, i6, i7)
				  }
				  If (((keg.compound.Bracket)localObject2).DY(d, i6, i7) < i2) 
				  {
					i2 = ((keg.compound.Bracket)localObject2).DY(d, i6, i7)
				  }
				  If (((keg.compound.Bracket)localObject2).DY(d, i6, i7) > i3) 
				  {
					i3 = ((keg.compound.Bracket)localObject2).DY(d, i6, i7)
				  }
				Next
        Next
        Exit For
			}
			If ((localObject1 Is keg.compound.ReactionArrow))
			{
			  localObject2 = (keg.compound.ReactionArrow)localObject1
			  n = Math.Min(((keg.compound.ReactionArrow)localObject2).DX1(), ((keg.compound.ReactionArrow)localObject2).DX2())
			  i1 = Math.Max(((keg.compound.ReactionArrow)localObject2).DX1(), ((keg.compound.ReactionArrow)localObject2).DX2())
			  i2 = Math.Min(((keg.compound.ReactionArrow)localObject2).DY1(), ((keg.compound.ReactionArrow)localObject2).DY2())
			  i3 = Math.Max(((keg.compound.ReactionArrow)localObject2).DY1(), ((keg.compound.ReactionArrow)localObject2).DY2())
			  Exit For
			}
		  }
		  i = Math.Min(n, i)
		  k = Math.Min(i2, k)
		  j = Math.Max(i1, j)
		  m = Math.Max(i3, m)
		Next
        For i4 = 0 To paramReaction.bracketNum() - 1 Step i4 + 1
		  localObject1 = paramReaction.getBracket(i4)
		  n = ((keg.compound.Bracket)localObject1).DX(d, 0, 0)
		  i1 = ((keg.compound.Bracket)localObject1).DX(d, 0, 0) + ((keg.compound.Bracket)localObject1).size().width + 3
		  i2 = ((keg.compound.Bracket)localObject1).DY(d, 0, 0) - ((keg.compound.Bracket)localObject1).size().height
		  i3 = ((keg.compound.Bracket)localObject1).DY(d, 0, 0)
		  Dim i5 As Integer
        For i5 = 1 To 2 Step i5 + 1
        For i6 = 1 To 4 Step i6 + 1
        If (((keg.compound.Bracket)localObject1).DX(d, i5, i6) < n) 
			  {
				n = ((keg.compound.Bracket)localObject1).DX(d, i5, i6)
			  }
			  If (((keg.compound.Bracket)localObject1).DX(d, i5, i6) > i1) 
			  {
				i1 = ((keg.compound.Bracket)localObject1).DX(d, i5, i6)
			  }
			  If (((keg.compound.Bracket)localObject1).DY(d, i5, i6) < i2) 
			  {
				i2 = ((keg.compound.Bracket)localObject1).DY(d, i5, i6)
			  }
			  If (((keg.compound.Bracket)localObject1).DY(d, i5, i6) > i3) 
			  {
				i3 = ((keg.compound.Bracket)localObject1).DY(d, i5, i6)
			  }
			Next
        Next
		  i = Math.Min(n, i)
		  k = Math.Min(i2, k)
		  j = Math.Max(i1, j)
		  m = Math.Max(i3, m)
		Next
		localRectangle.x = i
		localRectangle.y = k
		localRectangle.width = (j - localRectangle.x)
		localRectangle.height = (m - localRectangle.y)
		Return localRectangle
	  }
 
	  Public virtual void dispose() 
	  {
		stopTimer()
		stopClickTimer()
		Me.timer = Nothing
        Me.clickTimer = Nothing
		System.gc()
	  }
 
	  Public virtual void addText(keg.compound.Text paramText) 
	  {
		((keg.compound.Reaction)Me.conteiner).addText(paramText)
	  }
 
	  Public virtual void removeText(keg.compound.Text paramText) 
	  {
		((keg.compound.Reaction)Me.conteiner).removeText(paramText)
	  }
 
	  Public Overridable WriteOnly Property EnabledShrikPartMarker() As Boolean
            Set(ByVal Value As Boolean)
                Me.isEnabledShrinkPartMarker = value
            End Set
        End Property

        Public virtual void switchShrinkPartMarker() 
	  {
		EnabledShrikPartMarker = Not Me.isEnabledShrinkPartMarker
		repaint()
	  }
 
	  Friend Class RFR
            Inherits java.awt.Point
            Private ReadOnly outerInstance As CompoundCanvas

            Friend __selected As Boolean = False 
 
		internal  void New( void New outerInstance) 
		{
				Me.outerInstance = outerInstance
		}
 
		internal virtual void selected() 
		{
		  Me.__selected = True
		}
 
		Public virtual Boolean contains(Integer paramInt1, Integer paramInt2) 
		{
		  Return (paramInt1 - outerInstance.cx) * (paramInt1 - outerInstance.cx) + (paramInt2 - outerInstance.cy) * (paramInt2 - outerInstance.cy) < 25
		}
 
		Friend Overridable ReadOnly Property Selected() As Boolean
                Get
                    Return Me.__selected
                End Get
            End Property
 
		internal virtual void unselected() 
		{
		  Me.__selected = False
		}
 
		internal virtual void initLocation() 
		{
		  Dim localObject As Object
            If (outerInstance.editmode.selected.Count > 0)
		  {
			localObject = ConnectionPoint
			If (localObject = Nothing)
			{
			  outerInstance.cx = (outerInstance.editmode.select_area.x - outerInstance.tolerance + (outerInstance.editmode.select_area.width + outerInstance.tolerance * 2 + 1 + outerInstance.handlesize) / 2)
			  outerInstance.cy = (outerInstance.editmode.select_area.y - outerInstance.tolerance + (outerInstance.editmode.select_area.height + outerInstance.tolerance * 2) / 2)
			}
 Else
			{
			  outerInstance.cx = ((keg.compound.Atom)localObject).DX(outerInstance.dispscale)
			  outerInstance.cy = ((keg.compound.Atom)localObject).DY(outerInstance.dispscale)
			}
		  }
 Else
		  {
			localObject = outerInstance.Size
			outerInstance.cx = (((DblRect)localObject).width / 2 + outerInstance.h0)
			outerInstance.cy = (((DblRect)localObject).height / 2 + outerInstance.v0)
		  }
		  Me.x = outerInstance.cx
            Me.y = outerInstance.cy
		}
 
		Friend Overridable ReadOnly Property ConnectionPoint() As keg.compound.Atom
                Get
                    Dim localObject As Object = Nothing
                    Dim i As Integer
                    For i = 0 To outerInstance.editmode.selected.Count - 1 Step i + 1
                        If ((outerInstance.editmode.selected(i) Is keg.compound.Atom)) Then
				{
				  Dim localAtom1 As keg.compound.Atom = (keg.compound.Atom)CompoundCanvas.Me.editmode.selected.elementAt(i) 
				  Dim j As Integer
                            For j = 0 To localAtom1.numBond() - 1 Step j + 1
                                If (Not localAtom1.getBond(j).Select) Then
					{
					  If ((localObject = Nothing) Or (localObject.Equals(localAtom1))) Then
					  {
						localObject = localAtom1
					  }
 Else
					  {
						Return Nothing
					  }
					}
 Else
					{
					  Dim localAtom2 As keg.compound.Atom = localAtom1.getBond(j).pairAtom(localAtom1)
                                        If (Not localAtom2.Select) Then
					  {
						If ((localObject = Nothing) Or (localObject.Equals(localAtom2))) Then
						{
						  localObject = localAtom2
						}
 Else
						{
						  Return Nothing
						}
					  }
					}
                  Next
				}
              Next
                    Return (keg.compound.Atom)localObject
			End Get
            End Property
        End Class
    End Class
End Namespace
