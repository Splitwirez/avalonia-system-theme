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
    public class WindowsSystemThemeDecoratorImpl : ISystemThemeDecoratorImpl
    {
        enum Part : int {
            BP_PUSHBUTTON = 1,
            BP_RADIOBUTTON = 2,
            BP_CHECKBOX = 3,
            BP_GROUPBOX = 4,
            BP_USERBUTTON = 5,
            BP_COMMANDLINK = 6,
            BP_COMMANDLINKGLYPH = 7,
        };

        enum State : int {
            PBS_NORMAL = 1,
            PBS_HOT = 2,
            PBS_PRESSED = 3,
            PBS_DISABLED = 4,
            PBS_DEFAULTED = 5,
            PBS_DEFAULTED_ANIMATING = 6,

            RBS_UNCHECKEDNORMAL = 1,
            RBS_UNCHECKEDHOT    = 2,
            RBS_UNCHECKEDPRESSED = 3,
            RBS_UNCHECKEDDISABLED = 4,
            RBS_CHECKEDNORMAL   = 5,
            RBS_CHECKEDHOT      = 6,
            RBS_CHECKEDPRESSED  = 7,
            RBS_CHECKEDDISABLED = 8,

            CBS_UNCHECKEDNORMAL = 1,
            CBS_UNCHECKEDHOT  = 2,
            CBS_UNCHECKEDPRESSED = 3,
            CBS_UNCHECKEDDISABLED = 4,
            CBS_CHECKEDNORMAL = 5,
            CBS_CHECKEDHOT    = 6,
            CBS_CHECKEDPRESSED = 7,
            CBS_CHECKEDDISABLED = 8,
            CBS_MIXEDNORMAL   = 9,
            CBS_MIXEDHOT      = 10,
            CBS_MIXEDPRESSED  = 11,
            CBS_MIXEDDISABLED = 12,
            CBS_IMPLICITNORMAL = 13,
            CBS_IMPLICITHOT   = 14,
            CBS_IMPLICITPRESSED = 15,
            CBS_IMPLICITDISABLED = 16,
            CBS_EXCLUDEDNORMAL = 17,
            CBS_EXCLUDEDHOT   = 18,
            CBS_EXCLUDEDPRESSED = 19,
            CBS_EXCLUDEDDISABLED = 20,

            GBS_NORMAL   = 1,
            GBS_DISABLED = 2,

            CMDLS_NORMAL   = 1,
            CMDLS_HOT      = 2,
            CMDLS_PRESSED  = 3,
            CMDLS_DISABLED = 4,
            CMDLS_DEFAULTED = 5,
            CMDLS_DEFAULTED_ANIMATING = 6,

            CMDLGS_NORMAL   = 1,
            CMDLGS_HOT      = 2,
            CMDLGS_PRESSED  = 3,
            CMDLGS_DISABLED = 4,
            CMDLGS_DEFAULTED = 5,
        };

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr OpenThemeData (IntPtr hWnd, String classList);

        [DllImport("uxtheme.dll")]
		extern static Int32 CloseThemeData (IntPtr hTheme);

        [DllImport("uxtheme.dll")]
        static extern Int32 DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref Rectangle pRect, ref Rectangle pClipRect);

        public void Render(DrawingContext context, Rect bounds, ControlType ctrlType, bool isHovered, bool isPressed, bool isTicked, bool isEnabled, Window topLevel)
        {
            Part part = 0;
            State state = 0;

            if (ctrlType == ControlType.Button)
            {
                part = Part.BP_PUSHBUTTON;

                if (!isEnabled)
                    state = State.PBS_DISABLED;
                else if (isPressed || isTicked)
                    state = State.PBS_PRESSED;
                else if (isHovered)
                    state = State.PBS_HOT;
                else
                    state = State.PBS_NORMAL;
            }
            else if (ctrlType == ControlType.CheckBox)
            {
                part = Part.BP_CHECKBOX;

                if (isTicked)
                {
                    if (!isEnabled)
                        state = State.CBS_CHECKEDDISABLED;
                    else if (isPressed)
                        state = State.CBS_CHECKEDPRESSED;
                    else if (isHovered)
                        state = State.CBS_CHECKEDHOT;
                    else
                        state = State.CBS_CHECKEDNORMAL;
                }
                else
                {
                    if (!isEnabled)
                        state = State.CBS_UNCHECKEDDISABLED;
                    else if (isPressed)
                        state = State.CBS_UNCHECKEDPRESSED;
                    else if (isHovered)
                        state = State.CBS_UNCHECKEDHOT;
                    else
                        state = State.CBS_UNCHECKEDNORMAL;
                }
            }
            else if (ctrlType == ControlType.RadioButton)
            {
                part = Part.BP_RADIOBUTTON;

                if (isTicked)
                {
                    if (!isEnabled)
                        state = State.RBS_CHECKEDDISABLED;
                    else if (isPressed)
                        state = State.RBS_CHECKEDPRESSED;
                    else if (isHovered)
                        state = State.RBS_CHECKEDHOT;
                    else
                        state = State.RBS_CHECKEDNORMAL;
                }
                else
                {
                    if (!isEnabled)
                        state = State.RBS_UNCHECKEDDISABLED;
                    else if (isPressed)
                        state = State.RBS_UNCHECKEDPRESSED;
                    else if (isHovered)
                        state = State.RBS_UNCHECKEDHOT;
                    else
                        state = State.RBS_UNCHECKEDNORMAL;
                }
            }

            int width = (int)bounds.Width;
            int height = (int)bounds.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            
            Bitmap bitmap = new Bitmap(width, height);
            bitmap.MakeTransparent();
            using (Graphics g2 = Graphics.FromImage(bitmap))
            {
                g2.Clear(Color.Transparent);

                IntPtr hdc = g2.GetHdc();
                IntPtr hWnd = topLevel.PlatformImpl.Handle.Handle;
                Debug.WriteLine("hWnd: " + hWnd);
                IntPtr hTheme = OpenThemeData(hWnd, "Button");
                Debug.WriteLine("hTheme: " + hTheme);

                DrawThemeBackground(hTheme, hdc, (int)part, (int)state, ref rect, ref rect);
                CloseThemeData(hTheme);
            }

            var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            stream.Position = 0;

            AvBitmap avBmp = new AvBitmap(stream);
            context.DrawRectangle(new Avalonia.Media.ImageBrush(avBmp), null, bounds.WithX(0).WithY(0));
        }

        public bool TryGetRequestedSize(ControlType type, bool isHovered, bool isPressed, bool isChecked, bool isEnabled, out Size size)
        {
            size = Size.Empty;
            if (
                    (type == ControlType.CheckBox) ||
                    (type == ControlType.RadioButton)
                )
            {
                size = new Size(13, 13);
                return true;
            }
            return false;
        }
    }
}