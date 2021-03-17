using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;
using Thread = System.Threading.Thread;
using System.Timers;
using Avalonia;
using AvWindow = Avalonia.Controls.Window;
using DrawingContext = Avalonia.Media.DrawingContext;
using Avalonia.Markup.Xaml;
using AvBitmap = Avalonia.Media.Imaging.Bitmap;
using System.Diagnostics;
using Gtk;
using Gdk;
using Cairo;
using GtkApplication = Gtk.Application;
using GtkWindow = Gtk.Window;
using GdkWindow = Gdk.Window;
using IOPath = System.IO.Path;

namespace Avalonia.Themes.SystemLF
{
    internal static class GtkMethods
    {
        //https://github.com/GtkSharp/GtkSharp/blob/master/Source/Libs/Shared/GLibrary.cs
        [DllImport ("libcairo.so.2", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr cairo_image_surface_create(int format, int width, int height);


        [DllImport ("libcairo.so.2", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr cairo_create(IntPtr target);

        
        [DllImport ("libgdk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void gdk_cairo_set_source_window(IntPtr cr, IntPtr window, double x, double y);


        [DllImport ("libcairo.so.2", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void cairo_paint(IntPtr cr);


        [DllImport ("libcairo.so.2", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr cairo_surface_write_to_png(IntPtr surface, string filename);

        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern bool gtk_main_iteration();

        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern bool gtk_events_pending();


        [DllImport ("libgdk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr gdk_event_peek();


        [DllImport ("libgdk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gdk_event_free(IntPtr e);

        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void gtk_widget_queue_draw(IntPtr widget);




        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void gtk_widget_draw(IntPtr widget, IntPtr cr);

        




        
        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr gtk_offscreen_window_new();

        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr gtk_window_new(int type);


        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr gtk_offscreen_window_get_surface(IntPtr offscreen);

        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr gtk_offscreen_window_get_pixbuf(IntPtr offscreen);
        
        
        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void gtk_container_add(IntPtr container, IntPtr widget);

        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void gtk_widget_set_app_paintable(IntPtr window, bool state);

        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr gtk_widget_get_window(IntPtr widget);

        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void gtk_widget_realize(IntPtr widget);

        [DllImport ("libgdk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool gdk_pixbuf_save(IntPtr pixbuf, string filename, string type, out IntPtr error, IntPtr idklol);




        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void gtk_widget_set_size_request(IntPtr widget, int width, int height);


        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void gtk_window_resize(IntPtr window, int width, int height);

        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void gtk_widget_show(IntPtr widget);
        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void gtk_widget_hide(IntPtr widget);
        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void gtk_widget_show_all(IntPtr widget);

        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void gtk_application_add_window(IntPtr application, IntPtr window);

        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void gtk_widget_size_request(IntPtr widget, out IntPtr size);

        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern bool gtk_widget_is_drawable(IntPtr widget);




        [DllImport ("libgobject-2.0.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr g_object_ref(IntPtr obj);

        /*[DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void gtk_container_child_get_property(IntPtr container, IntPtr child, );*/


        

        [DllImport ("libgdk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr gdk_event_new(Gdk.EventType type);


        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_widget_set_state(IntPtr widget, Gtk.StateType state);

        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_widget_set_sensitive(IntPtr widget, bool sensitive);

        [DllImport ("libgtk-3.so.0", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_toggle_button_set_active(IntPtr widget, bool sensitive);
    }


    public class GtkThemeDecoratorImpl : ISystemThemeDecoratorImpl
    {
        internal static GtkApplication APP = null;
        internal static Dictionary<ControlType, IntPtr[]> IDLE_WINDOWS = new Dictionary<ControlType, IntPtr[]>();
        internal static Dictionary<ControlType, IntPtr[]> HOVER_WINDOWS = new Dictionary<ControlType, IntPtr[]>();
        internal static Dictionary<ControlType, IntPtr[]> PRESSED_WINDOWS = new Dictionary<ControlType, IntPtr[]>();
        internal static Dictionary<ControlType, IntPtr[]> CHECKED_WINDOWS = new Dictionary<ControlType, IntPtr[]>();
        internal static Dictionary<ControlType, IntPtr[]> DISABLED_WINDOWS = new Dictionary<ControlType, IntPtr[]>();
        internal static Dictionary<ControlType, IntPtr[]> DISABLED_CHECKED_WINDOWS = new Dictionary<ControlType, IntPtr[]>();

        internal static Button button = null;
        internal static CheckButton checkButton = null;


        internal static IntPtr buttonRef = IntPtr.Zero;
        internal static IntPtr checkButtonRef = IntPtr.Zero;
        public GtkThemeDecoratorImpl()
        {
            if (APP == null)
            {
                //GtkApplication.Init();

                APP = new GtkApplication("org.GtkTest.GtkTest", GLib.ApplicationFlags.None);
                APP.Register(GLib.Cancellable.Current);

                /*OFFSCREEN_GTK_WINDOW = GtkMethods.gtk_offscreen_window_new();
                //OFFSCREEN_GDK_WINDOW = GtkMethods.gtk_widget_get_window(OFFSCREEN_GTK_WINDOW);
                button = new Button(/*"AAAAAAAA"*);
                checkButton = new CheckButton(/*"AAAAAAAA"*);
                buttonRef = GtkMethods.g_object_ref(button.Handle);
                checkButtonRef = GtkMethods.g_object_ref(checkButton.Handle);
                
                GtkMethods.gtk_widget_realize(OFFSCREEN_GTK_WINDOW);
                GtkMethods.gtk_container_add(OFFSCREEN_GTK_WINDOW, button.Handle);
                GtkMethods.gtk_application_add_window(APP.Handle, OFFSCREEN_GTK_WINDOW);


                GtkMethods.gtk_widget_show_all(OFFSCREEN_GTK_WINDOW);*/


                CreateGtkWindows();
            }
        }

        void CreateGtkWindows()
        {
            CreateGtkWindow<Button>(ControlType.Button, ref IDLE_WINDOWS);
            CreateGtkWindow<Button>(ControlType.Button, ref HOVER_WINDOWS, isHovered: true);
            CreateGtkWindow<Button>(ControlType.Button, ref PRESSED_WINDOWS, isPressed: true);
            CreateGtkWindow<Button>(ControlType.Button, ref CHECKED_WINDOWS, isPressed: true);
            CreateGtkWindow<Button>(ControlType.Button, ref DISABLED_WINDOWS, isEnabled: false);
            CreateGtkWindow<Button>(ControlType.Button, ref DISABLED_CHECKED_WINDOWS, isPressed: true, isEnabled: false);

            CreateGtkWindow<CheckButton>(ControlType.CheckBox, ref IDLE_WINDOWS);
            CreateGtkWindow<CheckButton>(ControlType.CheckBox, ref HOVER_WINDOWS, isHovered: true);
            CreateGtkWindow<CheckButton>(ControlType.CheckBox, ref PRESSED_WINDOWS, isPressed: true);
            CreateGtkWindow<CheckButton>(ControlType.CheckBox, ref CHECKED_WINDOWS, isChecked: true);
            CreateGtkWindow<CheckButton>(ControlType.CheckBox, ref DISABLED_WINDOWS, isEnabled: false);
            CreateGtkWindow<CheckButton>(ControlType.CheckBox, ref DISABLED_CHECKED_WINDOWS, isChecked: true, isEnabled: false);

            /*CreateGtkWindow<ToggleButton>(ControlType.RadioButton, ref IDLE_WINDOWS);
            CreateGtkWindow<ToggleButton>(ControlType.RadioButton, ref HOVER_WINDOWS, isHovered: true);
            CreateGtkWindow<ToggleButton>(ControlType.RadioButton, ref PRESSED_WINDOWS, isPressed: true);
            CreateGtkWindow<ToggleButton>(ControlType.RadioButton, ref CHECKED_WINDOWS, isChecked: true);
            CreateGtkWindow<ToggleButton>(ControlType.RadioButton, ref DISABLED_WINDOWS, isEnabled: false);
            CreateGtkWindow<ToggleButton>(ControlType.RadioButton, ref DISABLED_CHECKED_WINDOWS, isChecked: true, isEnabled: false);*/

            string text = "";
            CreateGtkWindow(ControlType.RadioButton, ref IDLE_WINDOWS, () => new RadioButton(text).Handle, (ptr) => { });
            CreateGtkWindow(ControlType.RadioButton, ref HOVER_WINDOWS, (() => 
            {
                return new RadioButton(text).Handle;
            }), (ptr) => SetState(ptr, isHovered: true));
            CreateGtkWindow(ControlType.RadioButton, ref PRESSED_WINDOWS, (() => 
            {
                return new RadioButton(text).Handle;
            }), (ptr) => SetState(ptr, isPressed: true));
            CreateGtkWindow(ControlType.RadioButton, ref CHECKED_WINDOWS, (() => 
            {
                return new RadioButton(text).Handle;
            }), (ptr) => SetState(ptr, isChecked: true));
            CreateGtkWindow(ControlType.RadioButton, ref DISABLED_WINDOWS, (() => 
            {
                return new RadioButton(text).Handle;
            }), (ptr) => SetState(ptr, isEnabled: false));
            CreateGtkWindow(ControlType.RadioButton, ref DISABLED_CHECKED_WINDOWS, (() => 
            {
                return new RadioButton(text).Handle;
            }), (ptr) => SetState(ptr, isChecked: true, isEnabled: false));


            /*CreateGtkWindow<RadioButton>(ControlType.RadioButton, ref PRESSED_WINDOWS, isPressed: true);
            CreateGtkWindow<RadioButton>(ControlType.RadioButton, ref CHECKED_WINDOWS, isChecked: true);
            CreateGtkWindow<RadioButton>(ControlType.RadioButton, ref DISABLED_WINDOWS, isEnabled: false);
            CreateGtkWindow<RadioButton>(ControlType.RadioButton, ref DISABLED_CHECKED_WINDOWS, isChecked: true, isEnabled: false);*/

            /*CreateGtkWindow(ref IDLE_WINDOWS, (() => new CheckButton().Handle));

            CreateGtkWindow(ref IDLE_WINDOWS, (() => new RadioButton().Handle));*/
        }

        void CreateGtkWindow<T>(ControlType ctrlType, ref Dictionary<ControlType, IntPtr[]> dictionary, bool isHovered = false, bool isPressed = false, bool isChecked = false, bool isEnabled = true) where T : Widget, new()
        {
            var OFFSCREEN_GTK_WINDOW = GtkMethods.gtk_offscreen_window_new();
            GtkMethods.gtk_widget_set_app_paintable(OFFSCREEN_GTK_WINDOW, true);
            GtkMethods.gtk_widget_realize(OFFSCREEN_GTK_WINDOW);

            var wdg = new T();
            GtkMethods.gtk_container_add(OFFSCREEN_GTK_WINDOW, wdg.Handle);
            GtkMethods.gtk_application_add_window(APP.Handle, OFFSCREEN_GTK_WINDOW);
            
            SetState(wdg.Handle, isHovered, isPressed, isChecked, isEnabled);
            /*var gdEvent = new Gdk.Event(GtkMethods.gdk_event_new(Gdk.EventType.EnterNotify));
            gdEvent.Native.Window = gtk_widget_get_window(OFFSCREEN_GTK_WINDOW);
            
            gdEvent.SendEvent = true;*/
            GtkMethods.gtk_widget_queue_draw(wdg.Handle);
            GtkMethods.gtk_widget_queue_draw(OFFSCREEN_GTK_WINDOW);
            GtkMethods.gtk_widget_show_all(OFFSCREEN_GTK_WINDOW);
            
            dictionary[ctrlType] = new IntPtr[]
            {
                OFFSCREEN_GTK_WINDOW,
                wdg.Handle
            };
        }

        void CreateGtkWindow(ControlType ctrlType, ref Dictionary<ControlType, IntPtr[]> dictionary, Func<IntPtr> child, Action<IntPtr> setState)
        {
            /*var OFFSCREEN_GTK_WINDOW = GtkMethods.gtk_offscreen_window_new();
            GtkMethods.gtk_widget_realize(OFFSCREEN_GTK_WINDOW);
            var wdg = child();
            GtkMethods.gtk_container_add(OFFSCREEN_GTK_WINDOW, wdg);
            GtkMethods.gtk_application_add_window(APP.Handle, OFFSCREEN_GTK_WINDOW);
            GtkMethods.gtk_widget_queue_draw(wdg);
            GtkMethods.gtk_widget_queue_draw(OFFSCREEN_GTK_WINDOW);
            GtkMethods.gtk_widget_show_all(OFFSCREEN_GTK_WINDOW);
            
            dictionary[ctrlType] = new IntPtr[]
            {
                OFFSCREEN_GTK_WINDOW,
                wdg
            };*/

            var OFFSCREEN_GTK_WINDOW = GtkMethods.gtk_offscreen_window_new();
            GtkMethods.gtk_widget_set_app_paintable(OFFSCREEN_GTK_WINDOW, true);
            GtkMethods.gtk_widget_realize(OFFSCREEN_GTK_WINDOW);

            var wdg = child();
            GtkMethods.gtk_container_add(OFFSCREEN_GTK_WINDOW, wdg);
            GtkMethods.gtk_application_add_window(APP.Handle, OFFSCREEN_GTK_WINDOW);
            
            setState(wdg);
            /*var gdEvent = new Gdk.Event(GtkMethods.gdk_event_new(Gdk.EventType.EnterNotify));
            gdEvent.Native.Window = gtk_widget_get_window(OFFSCREEN_GTK_WINDOW);
            
            gdEvent.SendEvent = true;*/
            GtkMethods.gtk_widget_queue_draw(wdg);
            GtkMethods.gtk_widget_queue_draw(OFFSCREEN_GTK_WINDOW);
            GtkMethods.gtk_widget_show_all(OFFSCREEN_GTK_WINDOW);
            
            dictionary[ctrlType] = new IntPtr[]
            {
                OFFSCREEN_GTK_WINDOW,
                wdg
            };
        }

        void SetState(IntPtr wdg, bool isHovered = false, bool isPressed = false, bool isChecked = false, bool isEnabled = true)
        {
            //https://github.com/GtkSharp/GtkSharp/blob/master/Source/OldStuff/doc/en/Gtk/StateType.xml
            if (!isEnabled)
                GtkMethods.gtk_widget_set_sensitive(wdg, false);
            
            if (isChecked)
                    GtkMethods.gtk_toggle_button_set_active(wdg, true);
            
            if (isPressed)
                GtkMethods.gtk_widget_set_state(wdg, Gtk.StateType.Active);
            else if (isHovered)
                GtkMethods.gtk_widget_set_state(wdg, Gtk.StateType.Prelight);
            else
                GtkMethods.gtk_widget_set_state(wdg, Gtk.StateType.Normal);
        }

        public void Render(DrawingContext context, Rect bounds, ControlType ctrlType, bool isHovered, bool isPressed, bool isTicked, bool isEnabled, AvWindow topLevel)
        {
            //var dict = IDLE_WINDOWS;
            //https://github.com/GtkSharp/GtkSharp/blob/master/Source/OldStuff/doc/en/Gtk/StateType.xml
            /*Widget wdg = null;
            if (ctrlType == ControlType.Button)
                wdg = button;
            else
                wdg = checkButton;*/

            var ctrlT = ctrlType; //ControlType.Button;
            var size = bounds.Size;
            Avalonia.Media.IBrush brush = GetWidgetBrush(GetOffscreenWindow(ctrlT, isHovered, isPressed, isTicked, isEnabled), size);
            
            /*if (!isEnabled)
            {
                if (isTicked)
                    brush = GetWidgetBrush(DISABLED_CHECKED_WINDOWS[ctrlT], size);
                else
                    brush = GetWidgetBrush(DISABLED_WINDOWS[ctrlT], size);
            }
            else if (isTicked)
                brush = GetWidgetBrush(CHECKED_WINDOWS[ctrlT], size);
            else if (isPressed)
                brush = GetWidgetBrush(PRESSED_WINDOWS[ctrlT], size);
            else if (isHovered)
                brush = GetWidgetBrush(HOVER_WINDOWS[ctrlT], size);
            else
                brush = GetWidgetBrush(IDLE_WINDOWS[ctrlT], size);*/


            context.DrawRectangle(brush, null, bounds.WithX(0).WithY(0));
        }


        IntPtr[] GetOffscreenWindow(ControlType ctrlType, bool isHovered, bool isPressed, bool isTicked, bool isEnabled)
        {
            if (!isEnabled)
            {
                if (isTicked)
                    return DISABLED_CHECKED_WINDOWS[ctrlType];
                else
                    return DISABLED_WINDOWS[ctrlType];
            }
            else if (isTicked)
                return CHECKED_WINDOWS[ctrlType];
            else if (isPressed)
                return PRESSED_WINDOWS[ctrlType];
            else if (isHovered)
                return HOVER_WINDOWS[ctrlType];
            else
                return IDLE_WINDOWS[ctrlType];
        }


        static readonly string IMAGE_CACHE_PATH = IOPath.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, "systemLFTmp.png");
        
        IntPtr _prevWidget = IntPtr.Zero;
        int _lastWidth = -1;
        int _lastHeight = -1;
        int _imgCount = 0;
        private Avalonia.Media.IBrush GetWidgetBrush(IntPtr[] handles, Size size)
        {
            IntPtr window = handles[0];
            IntPtr wdgHandle = handles[1];
            int width = Math.Max(1, (int)size.Width);
            int height = Math.Max(1, (int)size.Height);


            
            Debug.WriteLine("Size: " + size);
                
                
            GtkMethods.gtk_widget_set_size_request(wdgHandle, width, height);
            GtkMethods.gtk_window_resize(window, width, height);
            
            
            GtkMethods.gtk_widget_show_all(window);


            

            while (GtkApplication.EventsPending())
                GtkApplication.RunIteration(true);
                
            bool canDraw = GtkMethods.gtk_widget_is_drawable(wdgHandle);
            Debug.WriteLine("CAN DRAW: " + canDraw);
            if (canDraw)
            {
                Debug.WriteLine("window: " + window);
                if (window != IntPtr.Zero)
                {
                    if (File.Exists(IMAGE_CACHE_PATH))
                        File.Delete(IMAGE_CACHE_PATH);
                    
                    GtkMethods.gdk_pixbuf_save(GtkMethods.gtk_offscreen_window_get_pixbuf(window), IMAGE_CACHE_PATH, "png", out IntPtr error, IntPtr.Zero);

                    if (File.Exists(IMAGE_CACHE_PATH))
                    {
                        Avalonia.Media.Imaging.Bitmap bmp = null;

                        using (MemoryStream strm = new MemoryStream())
                        {
                            using (FileStream fStrm = new FileStream(IMAGE_CACHE_PATH, FileMode.Open))
                            {
                                fStrm.CopyTo(strm);
                                strm.Seek(0, SeekOrigin.Begin);
                            }

                            bmp = new Avalonia.Media.Imaging.Bitmap(strm);
                        }

                        if (bmp != null)
                            return new Avalonia.Media.ImageBrush(bmp);
                    }
                }
            }
            return new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Blue);
        }

        public bool TryGetRequestedSize(ControlType type, bool isHovered, bool isPressed, bool isChecked, bool isEnabled, out Size size)
        {
            //IntPtr[] window = GetOffscreenWindow(ctrlT, isHovered, isPressed, isTicked, isEnabled);
            //GtkMethods.gtk_widget_size_request(window[1], out IntPtr size);
            //how 2 gtk_widget_get_preferred_size?????
            size = Size.Empty;
            if (
                    (type == ControlType.CheckBox) ||
                    (type == ControlType.RadioButton)
                )
            {
                size = new Size(17, 17);
                return true;
            }
            return false;
        }
    }
}