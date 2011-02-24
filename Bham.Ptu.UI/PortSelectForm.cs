using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO.Ports;

using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Collections.ObjectModel;

namespace Bham.Ptu.UI {
	
	public partial class PortSelectForm : Form {
		
		private BindingList<SerialPortWrapper> _ports = new BindingList<SerialPortWrapper>();
		
		public PortSelectForm() {
			
			InitializeComponent();
			
			PopulateForm();
			
			this.__ok.Click += new EventHandler(__ok_Click);
			this.__test.Click += new EventHandler(__test_Click);
			
			this.__port.SelectedIndexChanged += new EventHandler(__port_SelectedIndexChanged);
		}
		
		private void __port_SelectedIndexChanged(object sender, EventArgs e) {
			
			UpdateButtonEnabled();
		}
		
		private void UpdateButtonEnabled() {
			
			if( __port.SelectedIndex == -1 ) {
				__ok.Enabled = false;
				return;
			}
			
			SerialPortWrapper wrap = __port.SelectedItem as SerialPortWrapper;
			__ok.Enabled = wrap.FirmwareInfo != null;
		}
		
		private void PopulateForm() {
			
			///////////////////////////////
			// Serial Ports
			String[] portNames = SerialPort.GetPortNames();
			Array.Sort(portNames, new Comparison<String>( CompareComPorts ) );
			foreach(String portName in portNames) {
				
				_ports.Add( new SerialPortWrapper() { PortName = portName } );
			}
			
			__port.DisplayMember = "PortName";
			__port.DataSource    = _ports;
			
			if( __port.Items.Count > 0 ) __port.SelectedIndex = 0;
			
			
			///////////////////////////////
			// Joysticks
			Collection<JoystickInfo> sticks = Joystick.GetJoysticks();
			foreach(JoystickInfo device in sticks) {
				
				__joystick.Items.Add( new JoystickDeviceWrapper() { Device = device } );
			}
			
			if( __joystick.Items.Count > 0 ) __joystick.SelectedIndex = 0;
			
		}
		
		private static int CompareComPorts(String x, String y) {
			if( x == null && y == null ) return 0;
			if( x == null ) return -1;
			if( y == null ) return 1;
			
			if( x.StartsWith("COM") && y.StartsWith("COM") ) {
				
				int xi = Int32.Parse( x.Substring(3) );
				int yi = Int32.Parse( y.Substring(3) );
				
				if( xi == yi ) return  0;
				if( xi >  yi ) return  1;
				if( xi <  yi ) return -1;
			}
			
			return x.CompareTo( y );
		}
		
		private void __ok_Click(object sender, EventArgs e) {
			
			String name = (__port.SelectedItem as SerialPortWrapper).PortName;
			
			Program.PTUnit = new PTUnit( name ); 
			Program.Joystick = __joystick.SelectedIndex > -1 ? new Joystick( ((JoystickDeviceWrapper)__joystick.SelectedItem ).Device ) : null;
			
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		
		private void __test_Click(object sender, EventArgs e) {
			
			__test.Enabled = false;
			
			foreach(SerialPortWrapper wrap in _ports) {
				
				Thread testThread = new Thread( TestComPort );
				testThread.Start( wrap );
			}
			
		}
		
		private void TestComPort(Object comWrapper) {
			
			SerialPortWrapper wrap = (SerialPortWrapper)comWrapper;
			wrap.IsTesting = true;
			wrap.DisplayName = "Testing...";
			
			Invoke( new DoSub( delegate() {
				_ports.ResetBindings();
			} ) );
			
			try {
				
				PTFirmwareInfo fw = PTUnit.GetFirmwareInfo( wrap.PortName );
				if( fw == null ) wrap.DisplayName = "Not detected";
				else             wrap.DisplayName = fw.FirmwareString;
				
				wrap.FirmwareInfo = fw;
				
			} catch(SystemException sex) {
				
				wrap.DisplayName = "Exception: " + sex.Message;
				wrap.FirmwareInfo = null;
			}
			
			Invoke( new DoSub( delegate() {
				_ports.ResetBindings();
			} ) );
			
			wrap.IsTesting = false;
			
			Invoke( new DoSub( delegate() {
				
				foreach(SerialPortWrapper wrapper in _ports) {
					if( wrapper.IsTesting ) return;
				}
				
				// then none of the ports are currently being tested, so the buttons can be re-enabled
				UpdateButtonEnabled();
				this.__test.Enabled = true;

			} ) );
		}
		
		private class SerialPortWrapper {
			
			public String PortName;
			public String DisplayName;
			public PTFirmwareInfo FirmwareInfo;
			public bool   IsTesting;
			
			public override String ToString() {
				
				if( DisplayName == null ) return PortName;
				return PortName + " - " + DisplayName;
			}
		}
		
		private class JoystickDeviceWrapper {
			
			public JoystickInfo Device;
			
			public override string ToString() {
				return Device.Id + " - " + Device.Capabilities.ProductName;
			}
		}
		
	}
	
	internal delegate void DoSub();
	
}
