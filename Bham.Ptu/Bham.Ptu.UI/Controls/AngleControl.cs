using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Bham.Ptu.UI {

	/// <summary>Provides a user interface for adjusting an angle value.</summary>
	public class AngleControl : Control {
		
		private Color[] _angleColors = {
			Color.Red,
			Color.Blue,
			Color.Green
		};
		
		private Double[] _angles = { 0 };
		
		private Double _rMin =   0;
		private Double _rMax = 360;
		
		private int _dbX    = -10;
		private int _dbY    = -10;
		
		private bool _showAngle = true;
		
		public AngleControl() {
			this.DoubleBuffered = true;
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.BackColor = Color.Transparent;
		}
		
		public AngleControl(double angle) : this() {
			_angles = new Double[] { angle };
		}
		
#region Properties
		
		/// <summary>Gets and sets the first angle (in Degrees) in the collection of angles rendered.</summary>
		public virtual Double Angle0 {
			get { return _angles[0]; }
			set {
				if( value < 0 || value > 360 ) throw new ArgumentOutOfRangeException("value", "Argument must be from 0 to 360 (inclusive). 360 and 0 are equivalent.");
				if( _angles[0] == value ) return;
				_angles[0] = value;
				Refresh();
			}
		}
		
		/// <summary>Array of angles (in Degrees) to render.</summary>
		public virtual Double[] GetAngles() {
			return _angles; // NOTE: Maybe return a copy?
		}
		
		public virtual void SetAngles(Double[] angles) {
			if( angles == null || angles.Length == 0 ) throw new ArgumentNullException("value", "Argument must be an array of double with at least one element.");
			if( angles.Length > _angleColors.Length ) throw new ArgumentException("The number of angles exceeds the number of colors", "value");
			
			if( angles.Length == _angles.Length ) { // check the arrays are different, so not refreshing needlessly
				
				for(int i=0;i<angles.Length;i++) {
					if( angles[i] != _angles[i] ) {
						
						_angles = angles;
						Refresh();
						break;
					}
				}
				
			} else {
				
				_angles = angles;
				Refresh();
			}
			
			
			
		}
		
		public Color[] AngleColors {
			get { return _angleColors; }
			set {
				if( value.Length < _angles.Length ) throw new ArgumentException("The number of angle colors is less than the number of angles", "value");
				_angleColors = value;
				Refresh();
			}
		}
		
		public virtual Double RangeMin {
			get { return _rMin; }
			set {
				if( value < 0 || value > _rMax ) throw new ArgumentOutOfRangeException("value");
				if( value == _rMin ) return;
				_rMin = value;
				Refresh();
			}
		}
		
		public virtual Double RangeMax {
			get { return _rMax; }
			set {
				if( value < _rMin || value > 360d ) throw new ArgumentOutOfRangeException("value");
				if( value == _rMax ) return;
				_rMax = value;
				Refresh();
			}
		}
		
		public bool ShowAngle {
			get { return _showAngle; }
			set { _showAngle = value; }
		}
		
#endregion
#region Painting
		
		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			Invalidate();
		}
		
		protected override void OnPaint(PaintEventArgs e) {
			
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			
			// Set angle origin point at center of control.
			int originX = this.Width  / 2;
			int originY = this.Height / 2;
			
			// Fill background and ellipse and center point.
			e.Graphics.FillEllipse( SystemBrushes.ControlDark,            0,           0, Width - 1, Height - 1 );
			e.Graphics.FillEllipse( SystemBrushes.ControlLight,           1,           1, Width - 3, Height - 3 );
			e.Graphics.FillEllipse(       Brushes.SlateGray   , originX - 1, originY - 1,         3,          3 );
			
			PaintRange(e, originX, originY);
			
			// Draw angle markers.
			OnPaintAngleLabels(e);
			
			PaintAngleLines(e.Graphics, new Point( originX, originY ) );
			
			if( _showAngle ) {
				// Output angle information.
				OnPaintAngleText(e);
			}
			
			// Draw square at mouse position of last angle adjustment.
			e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black), 1), _dbX - 2, _dbY - 2, 4, 4);
		}
		
		protected virtual void OnPaintAngleLabels(PaintEventArgs e) {
			
			e.Graphics.DrawString(  "0", new Font("Arial", 8), new SolidBrush(Color.DarkGray), this.Width-18, (this.Height/2)-6);
			e.Graphics.DrawString( "90", new Font("Arial", 8), new SolidBrush(Color.DarkGray), (this.Width/2)-6, this.Height-18);
			e.Graphics.DrawString("180", new Font("Arial", 8), new SolidBrush(Color.DarkGray), 10, (this.Height/2)-6);
			e.Graphics.DrawString("270", new Font("Arial", 8), new SolidBrush(Color.DarkGray), (this.Width/2)-10, 10);
		}
		
		protected virtual void OnPaintAngleText(PaintEventArgs e) {
			
			e.Graphics.FillRectangle(new SolidBrush(Color.Gray), this.Width-84, 3, 82, 13);
			e.Graphics.DrawString("Angle: "+ Angle0.ToString("F4"), new Font("Arial", 8), new SolidBrush(Color.Yellow), this.Width-84, 2);
		}
		
		private void PaintRange(PaintEventArgs e, int originX, int originY) {
			
			Double start = GetRenderAngle( _rMin, originX, originY );
			Double stop  = GetRenderAngle( _rMax, originX, originY );
			if( stop == 0 ) stop = 360;
			
			Double sweep = stop - start;
			if( sweep == 0 ) sweep = 360;
			
			Rectangle rect = new Rectangle( 1, 1, Width - 3, Height - 3 );
			e.Graphics.FillPie( Brushes.White, rect, (float)start, (float)sweep );
			
//			PaintAngleLine( e.Graphics, Color.Green  , _rMin, new Point( originX, originY ) );
//			PaintAngleLine( e.Graphics, Color.Magenta, _rMax, new Point( originX, originY ) );
			
//			String s = String.Format("Start: {0:000} -> {1:000}\nStop: {2:000} -> {3:000}\nSweep: {4:000} -> {5:000}", _rMin, start, _rMax, stop, sweep, 0 );
//			e.Graphics.DrawString(s, SystemFonts.IconTitleFont, Brushes.Black, 5, 50 );
		}
		
		/// <summary>Converts a specified angle (in degrees) into an angle that takes the aspect ratio of the control into account.</summary>
		private Double GetRenderAngle(Double angle, int originX, int originY) {
			
			angle = GetEffectiveAngle(angle);
			
			double radians = ( angle * Math.PI) / 180d;
			
			int dx = (int)( (double)originX * Math.Cos(radians) );
			int dy = (int)( (double)originY * Math.Sin(radians) );
			
			//////////////////////////////
			
			radians = Math.Atan2( dy, dx );
			
			angle = Utility.RadiansToDegrees( radians );
			
			return (angle + 360d) % 360d;
		}
		
		private void PaintAngleLines(Graphics g, Point origin) {
			
			int i=0;
			foreach(Double angle in _angles) {
				
				Color c = _angleColors[i++];
				
				PaintAngleLine(g, c, angle, origin);
			}
			
		}
		
		private void PaintAngleLine(Graphics g, Color c, Double angleDeg, Point origin) {
			
			// Draw line along the current angle.         
			double radians = ( GetEffectiveAngle(angleDeg) * Math.PI) / 180d;
			
			Point p2 = new Point(
				origin.X + (int)( (double)origin.X * Math.Cos(radians) ),
				origin.Y + (int)( (double)origin.Y * Math.Sin(radians) )
			);
			
			g.DrawLine(
				new Pen(new SolidBrush( c ), 1),
				origin, p2
			);
			
		}
		
		/// <summary>Ensures the angle is between 0 and 360.</summary>
		private Double GetEffectiveAngle(Double angle) {
			
			Double degrees = ( angle + 360d) % 360d;
			return degrees;
		}
		
#endregion
#region Event-Dispatch
		
		/// <summary>Raised when the user has changed the angle (but not yet raised the mouse button). This event is often rapidly fired.</summary>
		public event EventHandler Angle0Changed;
		
		protected void OnAngle0Changed() {
			
			if( Angle0Changed != null ) Angle0Changed(this, EventArgs.Empty);
		}
		
#endregion
#region Event-Handling
		
		protected override void OnMouseDown(MouseEventArgs e) {
			
			UpdateAngle(e.X, e.Y);
		}
		
		protected override void OnMouseMove(MouseEventArgs e) {
			
			if(e.Button == MouseButtons.Left) {
				UpdateAngle(e.X, e.Y);
			}
				
			this.Refresh();
		}
		
		private void UpdateAngle(int mx, int my) {
			
			// Store mouse coordinates.
			_dbX = mx;
			_dbY = my;
			
			// Translate y coordinate input to GetAngle function to correct for ellipsoid distortion.
			double widthToHeightRatio = (double)this.Width/(double)this.Height;
			
			int tmy;
			if(my == 0)
				tmy = my;
				
			else if(my < this.Height/2)
				tmy = (this.Height/2)-(int)(((this.Height/2)-my)*widthToHeightRatio);
				
			else
				tmy = (this.Height/2)+(int)((double)(my-(this.Height/2))*widthToHeightRatio);
			
			double angle = GetAngle(this.Width/2, this.Height/2, mx, tmy);
			
			// Retrieve updated angle based on rise over run.
			// set the value directly, rather than using the virtual property
			_angles[0] = angle % 360;
			Refresh();
			
			OnAngle0Changed();
		}
		
		private double GetAngle(int x1, int y1, int x2, int y2) {
			double degrees;
			
			// Avoid divide by zero run values.
			if(x2-x1 == 0) {
				if(y2 > y1)
					degrees = 90;
				else
					degrees = 270;
			} else {
				// Calculate angle from offset.
				double riseoverrun = (double)(y2-y1)/(double)(x2-x1);
				double radians = Math.Atan(riseoverrun);
				degrees = radians * ((double)180/Math.PI);
				
				// Handle quadrant specific transformations.       
				if((x2-x1) < 0 || (y2-y1) < 0)
					degrees += 180;
				if((x2-x1) > 0 && (y2-y1) < 0)
					degrees -= 180;
				if(degrees < 0)
					degrees += 360;
			}
			
			return degrees;
		}
		
#endregion
	}
	
	
}
