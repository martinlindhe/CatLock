using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

// TODO 1st: when started, run "turn off screen" command in linux after 5 seconds
//		xset -display :0.0 dpms force off
// TODO if lost focus, get focus
// TODO paint window on all attached screens
// FIXME in fullscreen, on my desktop (xfce) my toolbar is always visible anyway. is that a xfce setting maybe?! other apps can fullscreen without it
// FIXME buttons are not exactly centered
namespace CatLock
{
	public class App : Form
	{
		int randomNumber;

		public App ()
		{
			DrawWindow ();
		}

		public static void Main ()
		{
			Application.Run (new App ());
		}

		/**
		 * Fullscreens the window
		 */
		void GoFullScreen ()
		{
			this.FormBorderStyle = FormBorderStyle.None;
			this.WindowState = FormWindowState.Maximized;

			// NOTE: this is needed to update this.Width & this.Height correctly, at least in mono/debian 3.0.2 // 22 feb-2014
			this.Bounds = Screen.PrimaryScreen.Bounds;
		}

		/**
		 * Forces this window always on top of other windows on the desktop
		 */
		void SetWindowOnTop ()
		{
			this.TopMost = true;
		}

		void HideWindowIcon ()
		{
			// NOTE: in mono & linux, this hides the default icon, but a "form icon" is shown instead, XXX can that be disabled or is that a bug?
			this.ShowIcon = false;
		}

		/**
		 * Disable all key input from switching focus away from this application
		 */
		protected override bool ProcessCmdKey (ref Message msg, Keys keyData)
		{
			return true;
		}

		/** 
		 * Moves mouse cursor at center width, and below the buttons
		 */
		private void PlaceMouseCursor ()
		{
			this.Cursor = new Cursor (Cursor.Current.Handle);
			Cursor.Position = new Point (this.Width / 2, (this.Height / 2) + 200);
			Cursor.Clip = new Rectangle (this.Location, this.Size);
		}

		private void DrawWindow ()
		{
			var font = new Font ("Sans", 26.0f, FontStyle.Bold);

			SetWindowOnTop ();

			GoFullScreen ();

			HideWindowIcon ();

			PlaceMouseCursor ();


			this.Deactivate += new EventHandler (delegate(object sender, EventArgs ea) {
				Console.WriteLine ("Application deactivated");
			});

			this.Text = "Cat Lock";

			int b_width = 40;
			int b_height = 40;
			// FIXME: now with such large font, can we move the text on the button up more? 

			int b_count = 6;

			int b_padding = 60;
			int b_top = (this.Height / 2) - (b_height / 2);
			int b_left = (this.Width / 2) - ((b_count * (b_width + b_padding)) / 2);

			
			Random rnd = new Random ();
			this.randomNumber = rnd.Next (1, b_count + 1);
	

			var label = new Label ();
			label.Text = "Click button " + this.randomNumber;
			label.Location = new Point (0, 200);
			label.Size = new Size (this.Width, b_height);
			label.TextAlign = ContentAlignment.MiddleCenter;
			label.Font = font;
			Controls.Add (label);

			this.ActiveControl = label;

			for (int i = 1; i <= b_count; i++) {
				var button = new Button ();
				button.Text = i.ToString ();

				button.Click += new EventHandler (delegate(object sender, EventArgs ea) {

					var value = Convert.ToInt16 ((sender as Button).Text);

					if (this.randomNumber == value)
						Application.Exit ();
				});

				button.Location = new Point (b_left + (b_width * (i - 1)) + ((i - 1) * b_padding), b_top);

				button.Size = new Size (b_width, b_height);
				button.Font = font;
				Controls.Add (button);
			}
		}
	}
}

