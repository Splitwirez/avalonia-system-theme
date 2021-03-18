using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
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
    //http://www.rdos.net/svn/tags/V9.2.5/watcom/bld/w32api/include/vsstyle.mh
    public partial class WindowsSystemThemeDecoratorImpl : ISystemThemeDecoratorImpl
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr OpenThemeData (IntPtr hWnd, String classList);

        [DllImport("uxtheme.dll")]
		extern static Int32 CloseThemeData (IntPtr hTheme);

        [DllImport("uxtheme.dll")]
        static extern Int32 DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref Rectangle pRect, ref Rectangle pClipRect);

        [DllImport("uxtheme", ExactSpelling=true)]
        public extern static Int32 DrawThemeBackgroundEx(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref Rectangle pRect, ref DTBGOPTS poptions);

        [StructLayout(LayoutKind.Sequential)]
        public struct DTBGOPTS
        {
            public int dwSize;
            public int dwFlags;
            public Rectangle rcClip;
        };

        [DllImport("uxtheme", ExactSpelling=true)]
        public extern static int IsThemeBackgroundPartiallyTransparent(IntPtr hTheme, int iPartId, int iStateId);

        [DllImport("uxtheme", ExactSpelling=true)]
        public extern static Int32 DrawThemeParentBackground(IntPtr hWnd, IntPtr hdc, ref Rectangle pRect);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        [DllImport("gdi32.dll")]
        internal static extern uint SetBkColor(IntPtr hdc, int crColor);

        [DllImport("gdiplus.dll", SetLastError=true)]
        static extern int GdipCreateBitmapFromGdiDib(IntPtr bminfo, IntPtr pixdat, ref IntPtr image);

        [DllImport("gdiplus.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
        static extern int GdipCreateBitmapFromHBITMAP(IntPtr hbitmap, IntPtr hpalette, out IntPtr bitmap);

        [DllImport("gdiplus.dll", SetLastError=true, ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Unicode)] // 3 = Unicode
        internal static extern int GdipCreateHBITMAPFromBitmap(IntPtr nativeBitmap, out IntPtr hbitmap, int argbBackground); 

        [DllImport("gdi32.dll")]
        static extern IntPtr CreatePatternBrush(IntPtr hbmp);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateSolidBrush(uint crColor);

        [DllImport("user32.dll")]
        static extern int FillRect(IntPtr hDC, [In] ref Rectangle lprc, IntPtr hbr);

        /*[DllImport("uxtheme.dll", SetLastError=true)]
        static extern IntPtr BeginBufferedPaint(IntPtr hdcTarget, [In] ref Rectangle prcTarget, BP_BUFFERFORMAT dwFormat, [In] ref BP_PAINTPARAMS pPaintParams, out IntPtr phdc);*/

        /*[DllImport("gdi32.dll", EntryPoint="GdiAlphaBlend")]
        public static extern bool AlphaBlend(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, BLENDFUNCTION blendFunction);*/






        public void Render(DrawingContext context, Rect bounds, ControlType ctrlType, bool isHovered, bool isPressed, bool isTicked, bool isEnabled, Window topLevel)
        {
            IntPtr hWnd = topLevel.PlatformImpl.Handle.Handle;
            //SetWindowTheme(hWnd, "Explorer", null);
            string vsClass = VisualStyleClasses.Button;
            int part = 0;
            int state = 0;
            switch (ctrlType)
            {
                case ControlType.ListBox:
                    vsClass = VisualStyleClasses.ListView;
                    part = ListBoxParts.LBCP_BORDER_NOSCROLL;
                    state = 1; //TODO: Finish this lol
                    break;
                case ControlType.ListBoxItem:
                    vsClass = VisualStyleClasses.ListView;
                    part = ListViewParts.LVP_LISTITEM;
                    if (!isEnabled)
                        state = ListViewParts.States.LISS_DISABLED;
                    else if (isTicked && isHovered)
                        state = ListViewParts.States.LISS_HOTSELECTED;
                    else if (isTicked)
                        state = ListViewParts.States.LISS_SELECTED;
                    else if (isHovered)
                        state = ListViewParts.States.LISS_HOT;
                    else
                        state = ListViewParts.States.LISS_NORMAL;
                    break;
                case ControlType.Button:
                    part = ButtonParts.BP_PUSHBUTTON;

                    if (!isEnabled)
                        state = ButtonParts.States.PBS_DISABLED;
                    else if (isPressed || isTicked)
                        state = ButtonParts.States.PBS_PRESSED;
                    else if (isHovered)
                        state = ButtonParts.States.PBS_HOT;
                    else
                        state = ButtonParts.States.PBS_NORMAL;
                    break;
                
                case ControlType.CheckBox:
                    part = ButtonParts.BP_CHECKBOX;

                    if (isTicked)
                    {
                        if (!isEnabled)
                            state = ButtonParts.States.CBS_CHECKEDDISABLED;
                        else if (isPressed)
                            state = ButtonParts.States.CBS_CHECKEDPRESSED;
                        else if (isHovered)
                            state = ButtonParts.States.CBS_CHECKEDHOT;
                        else
                            state = ButtonParts.States.CBS_CHECKEDNORMAL;
                    }
                    else
                    {
                        if (!isEnabled)
                            state = ButtonParts.States.CBS_UNCHECKEDDISABLED;
                        else if (isPressed)
                            state = ButtonParts.States.CBS_UNCHECKEDPRESSED;
                        else if (isHovered)
                            state = ButtonParts.States.CBS_UNCHECKEDHOT;
                        else
                            state = ButtonParts.States.CBS_UNCHECKEDNORMAL;
                    }
                    break;
                    
                case ControlType.RadioButton:
                    part = ButtonParts.BP_RADIOBUTTON;

                    if (isTicked)
                    {
                        if (!isEnabled)
                            state = ButtonParts.States.RBS_CHECKEDDISABLED;
                        else if (isPressed)
                            state = ButtonParts.States.RBS_CHECKEDPRESSED;
                        else if (isHovered)
                            state = ButtonParts.States.RBS_CHECKEDHOT;
                        else
                            state = ButtonParts.States.RBS_CHECKEDNORMAL;
                    }
                    else
                    {
                        if (!isEnabled)
                            state = ButtonParts.States.RBS_UNCHECKEDDISABLED;
                        else if (isPressed)
                            state = ButtonParts.States.RBS_UNCHECKEDPRESSED;
                        else if (isHovered)
                            state = ButtonParts.States.RBS_UNCHECKEDHOT;
                        else
                            state = ButtonParts.States.RBS_UNCHECKEDNORMAL;
                    }
                    break;

                case ControlType.TextBox:
                    vsClass = VisualStyleClasses.Edit;
                    part = EditParts.EP_EDITBORDER_NOSCROLL;
                    if (!isEnabled)
                        state = EditParts.States.EBWBS_DISABLED;
                    else if (isTicked)
                        state = EditParts.States.EBWBS_FOCUSED;
                    else if (isHovered)
                        state = EditParts.States.EBWBS_HOT;
                    else
                        state = EditParts.States.EBWBS_NORMAL;
                    break;

                case ControlType.TextBoxReadOnly:
                    vsClass = VisualStyleClasses.Edit;
                    part = EditParts.EP_BACKGROUND;
                    if (!isEnabled)
                        state = EditParts.States.EBS_DISABLED;
                    else
                        state = EditParts.States.EBS_READONLY;
                    break;

                default:
                    break;
            }
            /*else if ((ctrlType == ControlType.ListBox) || (ctrlType == ControlType.ListBoxItem))
            {
                vsClass = VisualStyleClasses.ListBox;
                if (ctrlType == ControlType.ListBoxItem)
                {
                    part = ListBoxParts.LBCP_ITEM;
                    if (!isEnabled)
                        state = ListBoxParts.States.LBPSI_SELECTEDNOTFOCUS;
                    if (isTicked && isHovered)
                        state = ListBoxParts.States.LBPSI_HOTSELECTED;
                    else if (isTicked)
                        state = ListBoxParts.States.LBPSI_SELECTED;
                    else if (isHovered)
                        state = ListBoxParts.States.LBPSI_HOT;
                    else
                        state = 0; //ListViewParts.States.LISS_NORMAL;
                }
            }*/

            int width = (int)bounds.Width;
            int height = (int)bounds.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            
            Bitmap bitmap = SetOpacity(new Bitmap(width, height, PixelFormat.Format32bppArgb), 0); //PixelFormat.Format64bppArgb
            Color transp = Color.FromArgb(0x00, 0xFF, 0x00, 0xFF);
            //bitmap.matr
            //https://theartofdev.com/2014/01/12/gdi-text-rendering-to-image/
            IntPtr img = IntPtr.Zero; //bitmap.GetHbitmap(Color.Transparent);
            //bitmap.Handle.Handle
            using (Graphics g2 = Graphics.FromImage(bitmap))
            {
                //g2.
                g2.SmoothingMode = SmoothingMode.HighQuality;
                g2.CompositingMode = CompositingMode.SourceCopy;
                g2.CompositingQuality = CompositingQuality.HighQuality;
                //////g2.Clear(Color.Transparent);
                //g2..InterpolationMode = InterpolationMode.HighQualityBilinear;
                ///////////////IntPtr hdc = g2.GetHdc();
                //SetBkColor(hdc, ColorTranslator.ToWin32(Color.Transparent));
                //Debug.WriteLine("hWnd: " + hWnd);
                ////////IntPtr hTheme = OpenThemeData(hWnd, vsClass);
                //Debug.WriteLine("hTheme: " + hTheme);
                /*Rectangle bounds2 = new Rectangle((int)bounds.Width, (int)bounds.Height, (int)bounds.Width, (int)bounds.Height);
                if (IsThemeBackgroundPartiallyTransparent(hTheme, part, state) != 0)
                    DrawThemeParentBackground(hWnd, hdc, ref bounds2);*/
                IntPtr hTheme = OpenThemeData(hWnd, vsClass);
                int transparent = ColorTranslator.ToWin32(transp); //Color.Transparent);
                //int result1 = GdipCreateHBITMAPFromBitmap(img, out IntPtr hBmp, transparent);
                IntPtr brush = CreateSolidBrush(Convert.ToUInt32(transparent)); //CreatePatternBrush(hBmp);
                //g2.Clear(transp);
                IntPtr hdc = g2.GetHdc();
                //FillRect(hdc, ref rect, brush);
                //SetBkColor(hdc, transparent);
                //g2.Clear(Color.Transparent);
                //g2.DrawImage(bitmap, new System.Drawing.Point(0));

                
                /*if (IsThemeBackgroundPartiallyTransparent(hTheme, part, state) != 0)
                    DrawThemeParentBackground(hWnd, hdc, ref rect);*/
                
                DrawThemeBackground(hTheme, hdc, (int)part, (int)state, ref rect, ref rect);

                //AAAAAAAAAAAAAAAAAAAAA
                /*DTBGOPTS pOptions = new DTBGOPTS();
                pOptions.dwSize = Marshal.SizeOf(typeof(DTBGOPTS));
                pOptions.dwFlags = 0;
                //pOptions.rcClip = IntPtr.Zero; //new ThemeInternal.RECT(clipBounds);
                DrawThemeBackgroundEx(hTheme, hdc, (int)part, (int)state, ref rect, ref pOptions);*/
                g2.ReleaseHdc(hdc);
                /*using (Bitmap bitmap2 = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                {
                    bitmap2.MakeTransparent(Color.Black);
                    using (Graphics g3 = Graphics.FromImage(bitmap2))
                    {
                        g3.CompositingMode = CompositingMode.SourceCopy;
                        g3.Clear(Color.Transparent);
                        /*g3.Clear(Color.Transparent);
                        g3.SmoothingMode = SmoothingMode.HighQuality;
                        g3.CompositingMode = CompositingMode.SourceOver;
                        g3.CompositingQuality = CompositingQuality.HighQuality;*
                    }
                    g2.DrawImage(bitmap2, new System.Drawing.Point(0));
                }*/
                
                //var no = IntPtr.Zero;
                //DrawThemeBackgroundEx(hTheme, hdc, (int)part, (int)state, ref rect, ref no);
                ////////////////g2.ReleaseHdc(hdc);
                CloseThemeData(hTheme);
                bitmap.MakeTransparent(transp);
            }

            var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            stream.Position = 0;

            AvBitmap avBmp = new AvBitmap(stream);
            context.DrawRectangle(new Avalonia.Media.ImageBrush(avBmp), null, bounds.WithX(0).WithY(0));
        }

        public static Bitmap SetOpacity(Bitmap image, float opacity)
        {
            var colorMatrix = new ColorMatrix();
            colorMatrix.Matrix33 = opacity;
            var imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(
                colorMatrix,
                ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);
            var output = new Bitmap(image.Width, image.Height);
            using (var gfx = Graphics.FromImage(output))
            {
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.DrawImage(
                    image,
                    new Rectangle(0, 0, image.Width, image.Height),
                    0,
                    0,
                    image.Width,
                    image.Height,
                    GraphicsUnit.Pixel,
                    imageAttributes);
            }
            return output;
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