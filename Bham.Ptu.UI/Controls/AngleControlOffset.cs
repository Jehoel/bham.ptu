using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Bham.Ptu.UI {

	/// <summary>Provides a user interface for adjusting an angle value.</summary>
	public class AngleControlOffset : AngleControl {
		
		private Double _base;
		
		public AngleControlOffset() : base() {
		}
		
		public AngleControlOffset(double angle) : base(angle) {
		}
		
		public override Double Angle0 {
			get { return base.Angle0 + BaseAngle; }
			set { base.Angle0 = value - BaseAngle; }
		}
		
		/// <summary>Array of angles (in Degrees) to render.</summary>
		public override Double[] GetAngles() {
			Double[] src = base.GetAngles();
			Double[] ret = new Double[ src.Length ];
			for(int i=0;i<ret.Length;i++) ret[i] = src[i] + BaseAngle;
			return ret;
		}
		
		public override void SetAngles(Double[] angles) {
			Double[] src = angles;
			Double[] dst = new Double[ src.Length ];
			for(int i=0;i<dst.Length;i++) dst[i] = src[i] - BaseAngle;
			base.SetAngles( dst );
		}
		
		public override Double RangeMin {
			get { return base.RangeMin + BaseAngle; }
			set { base.RangeMin = value - BaseAngle; }
		}
		
		public override Double RangeMax {
			get { return base.RangeMax + BaseAngle; }
			set { base.RangeMax = value - BaseAngle; }
		}
		
		public Double BaseAngle {
			get { return _base; }
			set {
				if( value < -360 || value > 0 ) throw new ArgumentOutOfRangeException("value");
				_base = value;
				Refresh();
			}
		}
		
		protected override void OnPaintAngleLabels(PaintEventArgs e) {
			
			String s1 =  BaseAngle.ToString();
			String s2 = (BaseAngle + 90).ToString();
			String s3 = (BaseAngle + 180).ToString();
			String s4 = (BaseAngle + 270).ToString();
			
			e.Graphics.DrawString( s1, new Font("Arial", 8), new SolidBrush(Color.DarkGray), this.Width-18, (this.Height/2)-6);
			e.Graphics.DrawString( s2, new Font("Arial", 8), new SolidBrush(Color.DarkGray), (this.Width/2)-6, this.Height-18);
			e.Graphics.DrawString( s3, new Font("Arial", 8), new SolidBrush(Color.DarkGray), 10, (this.Height/2)-6);
			e.Graphics.DrawString( s4, new Font("Arial", 8), new SolidBrush(Color.DarkGray), (this.Width/2)-10, 10);
		}
		
	}
	
	
}
