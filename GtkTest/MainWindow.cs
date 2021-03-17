using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace GtkTest
{
    class MainWindow : Window
    {
        [UI] private Label _label1 = null;
        [UI] private Button _button1 = null;

        private int _counter;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetObject("MainWindow").Handle)
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
            //_button1.Clicked += Button1_Clicked;
			
			
			
			var w = this;
			
			// Custom widget sample
			//a = new PrettyGraphic ();

			// Event-based drawing
			DrawingArea b = new DrawingArea ();
			//b.ExposeEvent += new ExposeEventHandler (ExposeHandler);
			//b.SizeAllocated += new SizeAllocatedHandler (SizeAllocatedHandler);

			Button c = new Button ("Quit");
			//c.Clicked += new EventHandler (quit);

			//MovingText m = new MovingText ();
			
			Box box = new HBox (true, 0);
			//box.Add (a);
			box.Add (b);
			//box.Add (m);
			box.Add (c);
			w.Add (box);
			
			w.ShowAll ();
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        /*private void Button1_Clicked(object sender, EventArgs a)
        {
            _counter++;
            _label1.Text = "Hello World! This button has been clicked " + _counter + " time(s).";
        }*/
    }
}
