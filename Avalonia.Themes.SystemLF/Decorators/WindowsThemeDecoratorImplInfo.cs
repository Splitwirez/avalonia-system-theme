using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using DrawingContext = Avalonia.Media.DrawingContext;
using Avalonia.Markup.Xaml;
using AvBitmap = Avalonia.Media.Imaging.Bitmap;
using System.Diagnostics;

namespace Avalonia.Themes.SystemLF
{
    public partial class WindowsSystemThemeDecoratorImpl : ISystemThemeDecoratorImpl
    {
        internal static class VisualStyleClasses
        {
            internal static readonly string Button = "BUTTON";
            internal static readonly string Clock = "CLOCK";
            internal static readonly string ComboBox = "COMBOBOX";
            internal static readonly string Communications = "COMMUNICATIONS";
            internal static readonly string ControlPanel = "CONTROLPANEL";
            internal static readonly string DatePicker = "DATEPICKER";
            internal static readonly string DragDrop = "DRAGDROP";
            internal static readonly string Edit = "EDIT";


            internal static readonly string ExplorerBar = "EXPLORERBAR";
            internal static readonly string Flyout = "FLYOUT";
            internal static readonly string Globals = "GLOBALS";
            internal static readonly string ListBox = "LISTBOX";
            internal static readonly string ListView = "EXPLORER::LISTVIEW"; //"LISTVIEW";
            internal static readonly string Menu = "MENU";
            internal static readonly string MenuBand = "MENUBAND";
            internal static readonly string Navigation = "NAVIGATION";
            internal static readonly string Page = "PAGE";
            internal static readonly string ProgressBar = "PROGRESS";
            internal static readonly string ReBar = "REBAR";
            internal static readonly string SearchEditBox = "SEARCHEDITBOX";
            internal static readonly string SpinBox = "SPIN";
            internal static readonly string StartPanel = "STARTPANEL";
            internal static readonly string StatusBar = "STATUS";
            internal static readonly string Tabs = "TAB";
            internal static readonly string Taskband = "TASKBAND";
            internal static readonly string Taskbar = "TASKBAR";
            internal static readonly string TaskDialog = "TASKDIALOG";
            internal static readonly string TextStyle = "TEXTSTYLE";
            internal static readonly string ToolBar = "TOOLBAR";
            internal static readonly string ToolTip = "TOOLTIP";
            internal static readonly string TrayNotify = "TRAYNOTIFY";
            internal static readonly string TreeView = "TREEVIEW";
            internal static readonly string Window = "WINDOW";
        }
        
        
        internal static class ButtonParts
        {
            internal static int BP_PUSHBUTTON = 1;
            internal static int BP_RADIOBUTTON = 2;
            internal static int BP_CHECKBOX = 3;
            internal static int BP_GROUPBOX = 4;
            internal static int BP_USERBUTTON = 5;
            internal static int BP_COMMANDLINK = 6;
            internal static int BP_COMMANDLINKGLYPH = 7;

            internal static class States 
            {
                internal static readonly int PBS_NORMAL = 1;
                internal static readonly int PBS_HOT = 2;
                internal static readonly int PBS_PRESSED = 3;
                internal static readonly int PBS_DISABLED = 4;
                internal static readonly int PBS_DEFAULTED = 5;
                internal static readonly int PBS_DEFAULTED_ANIMATING = 6;

                internal static readonly int RBS_UNCHECKEDNORMAL = 1;
                internal static readonly int RBS_UNCHECKEDHOT    = 2;
                internal static readonly int RBS_UNCHECKEDPRESSED = 3;
                internal static readonly int RBS_UNCHECKEDDISABLED = 4;
                internal static readonly int RBS_CHECKEDNORMAL   = 5;
                internal static readonly int RBS_CHECKEDHOT      = 6;
                internal static readonly int RBS_CHECKEDPRESSED  = 7;
                internal static readonly int RBS_CHECKEDDISABLED = 8;

                internal static readonly int CBS_UNCHECKEDNORMAL = 1;
                internal static readonly int CBS_UNCHECKEDHOT  = 2;
                internal static readonly int CBS_UNCHECKEDPRESSED = 3;
                internal static readonly int CBS_UNCHECKEDDISABLED = 4;
                internal static readonly int CBS_CHECKEDNORMAL = 5;
                internal static readonly int CBS_CHECKEDHOT    = 6;
                internal static readonly int CBS_CHECKEDPRESSED = 7;
                internal static readonly int CBS_CHECKEDDISABLED = 8;
                internal static readonly int CBS_MIXEDNORMAL   = 9;
                internal static readonly int CBS_MIXEDHOT      = 10;
                internal static readonly int CBS_MIXEDPRESSED  = 11;
                internal static readonly int CBS_MIXEDDISABLED = 12;
                internal static readonly int CBS_IMPLICITNORMAL = 13;
                internal static readonly int CBS_IMPLICITHOT   = 14;
                internal static readonly int CBS_IMPLICITPRESSED = 15;
                internal static readonly int CBS_IMPLICITDISABLED = 16;
                internal static readonly int CBS_EXCLUDEDNORMAL = 17;
                internal static readonly int CBS_EXCLUDEDHOT   = 18;
                internal static readonly int CBS_EXCLUDEDPRESSED = 19;
                internal static readonly int CBS_EXCLUDEDDISABLED = 20;

                internal static readonly int GBS_NORMAL   = 1;
                internal static readonly int GBS_DISABLED = 2;

                internal static readonly int CMDLS_NORMAL   = 1;
                internal static readonly int CMDLS_HOT      = 2;
                internal static readonly int CMDLS_PRESSED  = 3;
                internal static readonly int CMDLS_DISABLED = 4;
                internal static readonly int CMDLS_DEFAULTED = 5;
                internal static readonly int CMDLS_DEFAULTED_ANIMATING = 6;

                internal static readonly int CMDLGS_NORMAL   = 1;
                internal static readonly int CMDLGS_HOT      = 2;
                internal static readonly int CMDLGS_PRESSED  = 3;
                internal static readonly int CMDLGS_DISABLED = 4;
                internal static readonly int CMDLGS_DEFAULTED = 5;
            }
        }
        
        
        internal static class ListBoxParts
        {
            internal static int LBCP_BORDER_HSCROLL = 1;
            internal static int LBCP_BORDER_HVSCROLL = 2;
            internal static int LBCP_BORDER_NOSCROLL = 3;
            internal static int LBCP_BORDER_VSCROLL = 4;
            internal static int LBCP_ITEM = 5;

            internal static class States
            {
                internal static int LBPSI_HOT = 1;
                internal static int LBPSI_HOTSELECTED = 2;
                internal static int LBPSI_SELECTED = 3;
                internal static int LBPSI_SELECTEDNOTFOCUS = 4;
            }
        }
        
        
        internal static class ListViewParts
        {
            internal static int LVP_COLLAPSEBUTTON = 9;
            internal static int LVP_COLUMNDETAIL = 10;
            internal static int LVP_EMPTYTEXT = 5;
            internal static int LVP_EXPANDBUTTON = 8;
            internal static int LVP_GROUPHEADER = 6;
            internal static int LVP_GROUPHEADERLINE = 7;
            internal static int LVP_LISTGROUP = 2;
            internal static int LVP_LISTDETAIL = 3;
            internal static int LVP_LISTITEM = 1;
            internal static int LVP_LISTSORTEDDETAIL = 4;
            


            internal static class States
            {
                internal static int LISS_NORMAL = 1;
                internal static int LISS_HOT = 2;
                internal static int LISS_SELECTED = 3;
                internal static int LISS_DISABLED = 4;
                internal static int LISS_SELECTEDNOTFOCUS = 5;
                internal static int LISS_HOTSELECTED = 6;
            }
        }
        
        
        internal static class EditParts
        {
            internal static int EP_EDITTEXT = 1;
            internal static int EP_CARET = 2;
            internal static int EP_BACKGROUND = 3;
            internal static int EP_PASSWORD = 4;
            internal static int EP_BACKGROUNDWITHBORDER = 5;
            internal static int EP_EDITBORDER_NOSCROLL= 6;
            internal static int EP_EDITBORDER_HSCROLL = 7;
            internal static int EP_EDITBORDER_VSCROLL = 8;
            internal static int EP_EDITBORDER_HVSCROLL = 9;
            


            internal static class States
            {
                internal static int ETS_NORMAL = 1;
                internal static int ETS_HOT = 2;
                internal static int ETS_SELECTED = 3;
                internal static int ETS_DISABLED = 4;
                internal static int ETS_FOCUSED = 5;
                internal static int ETS_READONLY = 6;
                internal static int ETS_ASSIST = 7;
                internal static int ETS_CUEBANNER = 8;


                internal static int EBS_NORMAL = 1;
                internal static int EBS_HOT = 2;
                internal static int EBS_DISABLED = 3;
                internal static int EBS_FOCUSED = 4;
                internal static int EBS_READONLY = 5;
                internal static int EBS_ASSIST = 6;


                internal static int EBWBS_NORMAL = 1;
                internal static int EBWBS_HOT = 2;
                internal static int EBWBS_FOCUSED = 3;
                internal static int EBWBS_DISABLED = 4;


                internal static int EPSN_NORMAL = 1;
                internal static int EPSN_HOT = 2;
                internal static int EPSN_FOCUSED = 3;
                internal static int EPSN_DISABLED = 4;


                internal static int EPSH_NORMAL = 1;
                internal static int EPSH_HOT = 2;
                internal static int EPSH_FOCUSED = 3;
                internal static int EPSH_DISABLED = 4;


                internal static int EPSV_NORMAL = 1;
                internal static int EPSV_HOT = 2;
                internal static int EPSV_FOCUSED = 3;
                internal static int EPSV_DISABLED = 4;


                internal static int EPSHV_NORMAL = 1;
                internal static int EPSHV_HOT = 2;
                internal static int EPSHV_FOCUSED = 3;
                internal static int EPSHV_DISABLED = 4;
            }
        }
    }
}