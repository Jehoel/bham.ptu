using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Bham.Ptu.UI {

	public class AngleEditorTestControl : UserControl {
		
		private Double _angle;
		
		public AngleEditorTestControl() {
			this._angle    = 90;
			this.Size      = new Size(190, 42);
			this.BackColor = Color.Beige;
		}
		
		[BrowsableAttribute(true)]
		[EditorAttribute(typeof(AngleEditor), typeof(UITypeEditor))]
		public double Angle {
			get { return _angle; }
			set { _angle = value; }
		}
		
		protected override void OnPaint(PaintEventArgs e) {
			
			if (this.DesignMode) {
				
				e.Graphics.DrawString("Use the Properties Window to access", new Font("Arial", 8), new SolidBrush(Color.Black), 3, 2);
				e.Graphics.DrawString("the AngleEditor UITypeEditor by", new Font("Arial", 8), new SolidBrush(Color.Black), 3, 14);
				e.Graphics.DrawString("configuring the \"Angle\" property.", new Font("Arial", 8), new SolidBrush(Color.Black), 3, 26);
				
			} else {
				
				e.Graphics.DrawString("This example requires design mode.", new Font("Arial", 8), new SolidBrush(Color.Black), 3, 2);
			}
			
		}
		
	}
}
