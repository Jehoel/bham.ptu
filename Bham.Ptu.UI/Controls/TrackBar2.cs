using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Bham.Ptu.UI {
	
	/// <summary>Trackbar with a ScrollStopped event that fires only when a value has been chosen, and a Logarithmic scale option.</summary>
	public class TrackBar2 : TrackBar {
		
		private int _lastStoppedOn = 0;
		
		public TrackBar2() {
		}
		
#region ScrollStopped
		
		protected override void WndProc(ref Message m) {
			
			base.WndProc(ref m);
			
			if( (m.Msg == 0x2115 || m.Msg == 0x2114) && (int)m.WParam == 8 ) {
				
				if( Value == _lastStoppedOn ) return;
				_lastStoppedOn = Value;
				
				if( ScrollStopped != null )
					ScrollStopped(this, EventArgs.Empty);
			}
			
		}
		
		public event EventHandler ScrollStopped;
		
#endregion
#region Log Scale
		
		private Double _logBase;
		
		public Double LogBase {
			get { return _logBase; }
			set {
				if( value <= 1 ) throw new ArgumentOutOfRangeException("value", value, "Value must be greater than 1");
				Double v = LogValue;
				_logBase = value;
				LogValue = v;
			}
		}
		
		public Double LogValue {
			get { return (int)Math.Pow( _logBase, this.Value ); }
			set { this.Value = (int)Math.Log( value, _logBase ); }
		}
		
#endregion
		
	}
}
