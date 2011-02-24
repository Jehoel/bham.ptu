using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Bham.Ptu.UI {
	
	public static class Program {
		
		private static PTUnit      _unit;
		private static PTUnitState _state;
		
		public static PTUnit PTUnit {
			get { return _unit; }
			set {
				_unit = value;
				_state = new PTUnitState(value);
			}
		}
		
		public static PTUnitState PTUnitState {
			get { return _state; }
		}
		
		public static Joystick Joystick {
			get; set;
		}
		
		[STAThread]
		public static void Main() {
			
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			PortSelectForm portSelect = new PortSelectForm();
			if( portSelect.ShowDialog() == DialogResult.OK ) {
				
				MainForm mainForm = new MainForm();
				
				Application.Run( mainForm );
			}
			
		}
	}
}
