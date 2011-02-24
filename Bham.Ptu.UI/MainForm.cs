using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using System.Diagnostics;

namespace Bham.Ptu.UI {
	
	public partial class MainForm : Form {
		
		private PTQueue _q;
		
		public MainForm() {
			
			InitializeComponent();
			
			this.Load += new EventHandler(MainForm_Load);
			this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
			
			this.__pan.Angle0Changed += new EventHandler(__pan_Angle0Changed);
			this.__tilt.Angle0Changed += new EventHandler(__tilt_Angle0Changed);
			
			this.__panSpeed .ValueChanged += new EventHandler(__panSpeed_ValueChanged);
			this.__panBase  .ValueChanged += new EventHandler(__panBase_ValueChanged);
			this.__panAccl  .ValueChanged += new EventHandler(__panAccl_ValueChanged);
			this.__tiltSpeed.ValueChanged += new EventHandler(__tiltSpeed_ValueChanged);
			this.__tiltBase .ValueChanged += new EventHandler(__tiltBase_ValueChanged);
			this.__tiltAccl .ValueChanged += new EventHandler(__tiltAccl_ValueChanged);
			
			this.__panSpeed .ScrollStopped += new EventHandler(__panSpeed_ScrollStopped);
			this.__panBase  .ScrollStopped += new EventHandler(__panBase_ScrollStopped);
			this.__panAccl  .ScrollStopped += new EventHandler(__panAccl_ScrollStopped);
			this.__tiltSpeed.ScrollStopped += new EventHandler(__tiltSpeed_ScrollStopped);
			this.__tiltBase .ScrollStopped += new EventHandler(__tiltBase_ScrollStopped);
			this.__tiltAccl .ScrollStopped += new EventHandler(__tiltAccl_ScrollStopped);
			
			this.__timer.Tick += new EventHandler(__timer_Tick);
			
			this.__home.Click += new EventHandler(__home_Click);
			this.__joystick.CheckedChanged += new EventHandler(__joystick_CheckedChanged);
			this.__reset.Click += new EventHandler(__reset_Click);
			
			
			this.__pan .SetAngles( new Double[] { 0, 0 } );
			this.__tilt.SetAngles( new Double[] { 0, 0 } );
			
			this.__joystickPollThread.DoWork += new DoWorkEventHandler(__joystickPollThread_DoWork);
			
			this.__refreshInit.Click += new EventHandler(__refreshInit_Click);
			this.__configSet.Click += new EventHandler(__configSet_Click);
			
			//////////////////////
			
			this.__configLoad.Click += new EventHandler(__configLoad_Click);
			this.__configSave.Click += new EventHandler(__configSave_Click);
			this.__configFactory.Click += new EventHandler(__configFactory_Click);
		}
		
		private void MainForm_Load(object sender, EventArgs e) {
			
			RefreshState(true);
			
			__timer.Start();
		}
		
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			
			_q.Stop();
			
			__joystickPollThread.CancelAsync();
			this._useJoystick = false;
			
			Thread.Sleep( _pollDelay * 3 ); // let it sleep so the joystick poller stops
			// I can't think of a better way of stopping it, because BackgroundWorker doesn't expose a .Join method
		}
		
		private void _q_StatusChanged(object sender, EventArgs e) {
			// this is called from the PTQueue thread
			
			BeginInvoke( new DoSub( delegate() {
				
				__status.Text = _q.Status;
			} ) );
		}
		
		private void RefreshState(bool init) {
			
			__state.SelectedObject = Program.PTUnitState;
			__joystick.Enabled = Program.Joystick != null;
			if(init) {
				if( _q != null ) {
					_q.Stop();
				}
				_q = new PTQueue( PT );
				_q.StatusChanged += new EventHandler(_q_StatusChanged);
				_q.Start();
				
				__firmware   .Text = PT.GetFirmwareInfo();
				__environment.Text = PT.GetEnvironmentInfo();
				__firmwareLbl.Text = PT.ComPort + ":";
			}
			
			RefreshAngleState(init);
			RefreshSliderState(init);
			RefreshConfigurationState();
		}
		
		private void RefreshAngleState(bool init) {
			
			if(init) {
				
				////////////////////////
				// Setup Ranges
				
				// use 'soft limits' rather than 'user limits' because the 'soft limits' *are* the user limits (or rather, the current limits-in-effect)
				int panMinSteps  = PT.GetPanPositionLimitMin();
				int panMaxSteps  = PT.GetPanPositionLimitMax();
				int tiltMinSteps = PT.GetTiltPositionLimitMin();
				int tiltMaxSteps = PT.GetTiltPositionLimitMax();
				
				double panMinDegs  = PT.ConvertPanStepsToDegrees(panMinSteps);
				double panMaxDegs  = PT.ConvertPanStepsToDegrees(panMaxSteps);
				double tiltMinDegs = PT.ConvertTiltStepsToDegrees(tiltMinSteps);
				double tiltMaxDegs = PT.ConvertTiltStepsToDegrees(tiltMaxSteps);
				
				__pan .BaseAngle = -180d;
				__tilt.BaseAngle = -180d;
				
				 // hmm, it seems with the D48 in 360-deg mode (and how the fark did it get into that state?) it returns a value just-beyond 180... but for the tilt axis. This is weird.
				__pan.RangeMin = panMinDegs < -180 ? -180 : panMinDegs;
				__pan.RangeMax = panMaxDegs >  180 ?  180 : panMaxDegs;
				
				__tilt.RangeMin = tiltMinDegs < -180 ? -180 : tiltMinDegs;
				__tilt.RangeMax = tiltMaxDegs >  180 ?  180 : tiltMaxDegs;
			}
			
			_q.AddCommand( new PTQueuedCommandInfo("Idle", delegate(PTUnit unit) { // "Idle" is a lie, it's really polling, but whatever; when you set it to "Polling" it just flashes annoyingly
				
				////////////////////////
				// Set position
				
				short panStepsDes, tiltStepsDes;
				short panStepsCur, tiltStepsCur;
				
				PT.GetCurrentPosition(out panStepsCur, out tiltStepsCur);
				panStepsDes  = PT.GetPanDesiredPosition();
				tiltStepsDes = PT.GetTiltDesiredPosition();
				
				Double panDegreesDes  = PT.ConvertPanStepsToDegrees(panStepsDes);
				Double tiltDegreesDes = PT.ConvertTiltStepsToDegrees(tiltStepsDes);
				Double panDegreesCur  = PT.ConvertPanStepsToDegrees(panStepsCur);
				Double tiltDegreesCur = PT.ConvertTiltStepsToDegrees(tiltStepsCur);
				
				// Angle0 = Desired Position
				// Angle1 = Current Position
				
				Invoke( new DoSub( delegate() {
					
					if( IsDisposed ) return;
					
					__pan .SetAngles( new Double[] { panDegreesDes, panDegreesCur } );
					__tilt.SetAngles( new Double[] { tiltDegreesDes, tiltDegreesCur } );
					
				}));
				
			} ) );
		}
		
		private void RefreshSliderState(bool init) {
			
			///////////////////////////////
			// Pan
			
			short panSpeed = (short)PT.GetPanDesiredSpeed();
			
			if( init ) {
				
				if( panSpeed > __panSpeed.Maximum || panSpeed < __panSpeed.Minimum ) {
					panSpeed = (short)((__panSpeed.Maximum - __panSpeed.Minimum) / 2);
					PT.SetPanDesiredSpeed( panSpeed );
				}
				
				__panSpeed.Minimum = __panBase.Minimum = PT.GetPanSpeedLimitMin();
				__panSpeed.Maximum = __panBase.Maximum = PT.GetPanSpeedLimitMax();
			}
			
			__panSpeed.Value = panSpeed;
			__panBase .Value = PT.GetPanBaseSpeed();
			__panAccl .Value = (int)PT.GetPanAcceleration();
			
			///////////////////////////////
			// Tilt
			
			short tiltSpeed = (short)PT.GetTiltDesiredSpeed();
			
			if( init ) {
				
				if( tiltSpeed > __tiltSpeed.Maximum || tiltSpeed < __tiltSpeed.Minimum ) {
					tiltSpeed = (short)((__tiltSpeed.Maximum - __tiltSpeed.Minimum) / 2);
					PT.SetTiltDesiredSpeed( tiltSpeed );
				}
				
				__tiltSpeed.Minimum = __tiltBase.Minimum = PT.GetTiltSpeedLimitMin();
				__tiltSpeed.Maximum = __tiltBase.Maximum = PT.GetTiltSpeedLimitMax();
			}
			
			UInt32 tiltAccel = PT.GetTiltAcceleration();
			
			__tiltSpeed.Value = tiltSpeed;
			__tiltBase .Value = PT.GetTiltBaseSpeed();
			__tiltAccl .Value = (int)tiltAccel;
			
		}
		
		private void RefreshConfigurationState() {
			
			Program.PTUnitState.Refresh();
			
			__state.SelectedObject = Program.PTUnitState;
		}
		
#region Angle UI
		
		private void __timer_Tick(object sender, EventArgs e) {
			
			if( _useJoystick ) return;
			
			RefreshAngleState(false);
		}
		
		private PTUnit PT { get { return Program.PTUnit; } }
		
		private static short Bound(short value, short min, short max) {
			if( value < min ) return min;
			if( value > max ) return max;
			return value;
		}
		
		private void __pan_Angle0Changed(object sender, EventArgs e) {
			
			Double angleDeg = __pan.Angle0;
			
			short steps = PT.ConvertPanDegreesToSteps( angleDeg );
			
			steps = Bound(steps, Program.PTUnitState.PanPositionLimitMin, Program.PTUnitState.PanPositionLimitMax );
			
			PT.SetPanDesiredPosition( steps );
		}
		
		private void __tilt_Angle0Changed(object sender, EventArgs e) {
			
			Double angleDeg = __tilt.Angle0;
			
			short steps = PT.ConvertTiltDegreesToSteps( angleDeg );
			
			steps = Bound(steps, Program.PTUnitState.TiltPositionLimitMin, Program.PTUnitState.TiltPositionLimitMax );
			
			PT.SetTiltDesiredPosition( steps );
		}
		
#endregion
		
#region Buttons
		
		private void __refreshInit_Click(object sender, EventArgs e) {
			RefreshState(true);
		}
		
		private void __configSet_Click(object sender, EventArgs e) {
			
			__timer.Stop();
			
			String status = "Set Configuration";
			
			PTUnitState state = __state.SelectedObject as PTUnitState;
			bool reset;
			if( reset = state.RequiresReset ) {
				status += " and resetting";
				MessageBox.Show(this, "A unit calibration reset is required after setting the configuration. This will be carried out automatically.", "PTU Controller", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
			}
			
			_q.AddCommand( new PTQueuedCommandInfo(status, delegate(PTUnit unit) {
				
				state.SaveChanges();
				state.ClearReset();
				
				Invoke( new DoSub( delegate() {
					
					if( reset ) unit.Reset();
					RefreshState(true);
					__timer.Start();
				} ) );
			} ) );
			
		}
		
		private void __home_Click(object sender, EventArgs e) {
			
			__pan .Angle0 = 0;
			__tilt.Angle0 = 0;
			
			_q.AddCommand( new PTQueuedCommandInfo("Returning to Home position", delegate(PTUnit unit) {
				
				unit.SetPanDesiredPosition(0);
				unit.SetTiltDesiredPosition(0);
			} ) );
			
		}
		
		private void __reset_Click(object sender, EventArgs e) {
			
			__timer.Stop();
			
			_q.AddCommand( new PTQueuedCommandInfo("Performing reset calibration", delegate(PTUnit unit) {
				
				unit.ResetParser();
				unit.Reset();
				
				Invoke( new DoSub( delegate() {
					RefreshState(true);
					__timer.Start();
				} ) );
			} ) );
			
		}
		
		private void __configFactory_Click(object sender, EventArgs e) {
			
			__timer.Stop();
			
			_q.AddCommand( new PTQueuedCommandInfo("Restoring configuration and EPROM to Factory Defaults", delegate(PTUnit unit) {
				
				unit.FactoryResetConfiguration();
				
				Invoke( new DoSub( delegate() {
					RefreshState(true);
					__timer.Start();
				} ) );
			} ) );
			
		}
		
		private void __configSave_Click(object sender, EventArgs e) {
			
			__timer.Stop();
			
			_q.AddCommand( new PTQueuedCommandInfo("Saving current configuration to EPROM", delegate(PTUnit unit) {
				
				unit.SaveConfiguration();
				__timer.Start();
			} ) );
			
		}
		
		private void __configLoad_Click(object sender, EventArgs e) {
			
			__timer.Stop();
			
			_q.AddCommand( new PTQueuedCommandInfo("Restoring configuration from last saved EPROM", delegate(PTUnit unit) {
				
				unit.RestoreConfiguration();
				
				Invoke( new DoSub( delegate() {
					RefreshState(true);
					__timer.Start();
				} ) );
			} ) );
			
		}
		
#endregion
		
#region Sliders
	#region ValueChanged
		private void __panSpeed_ValueChanged(object sender, EventArgs e) {
			
			__speed1Lbl.Text = __panSpeed.Value + "u/s";
		}
		
		private void __panBase_ValueChanged(object sender, EventArgs e) {
			
			__base1Lbl.Text = __panBase.Value + "u/s";
		}
		
		private void __panAccl_ValueChanged(object sender, EventArgs e) {
			
			__accl1Lbl.Text = __panAccl.Value + "u/s²";
		}
		
		private void __tiltSpeed_ValueChanged(object sender, EventArgs e) {
			
			__speed2Lbl.Text = __tiltSpeed.Value + "u/s";
		}
		
		private void __tiltBase_ValueChanged(object sender, EventArgs e) {
			
			__base2Lbl.Text = __tiltBase.Value + "u/s";
		}
		
		private void __tiltAccl_ValueChanged(object sender, EventArgs e) {
			
			__accl2Lbl.Text = __tiltAccl.Value + "u/s²";
		}
	#endregion
	#region ScrollStopped
		
		private void __panSpeed_ScrollStopped(object sender, EventArgs e) {
			
			PT.SetPanDesiredSpeed( (short)__panSpeed.Value );
		}
		
		private void __panBase_ScrollStopped(object sender, EventArgs e) {
			
			PT.SetPanBaseSpeed( (ushort)__panBase.Value );
		}
		
		private void __panAccl_ScrollStopped(object sender, EventArgs e) {
			
			PT.SetPanAcceleration( (uint)__panAccl.Value );
		}
		
		private void __tiltSpeed_ScrollStopped(object sender, EventArgs e) {
			
			PT.SetTiltDesiredSpeed( (short)__tiltSpeed.Value );
		}
		
		private void __tiltBase_ScrollStopped(object sender, EventArgs e) {
			
			PT.SetTiltBaseSpeed( (ushort)__tiltBase.Value );
		}
		
		private void __tiltAccl_ScrollStopped(object sender, EventArgs e) {
			
			PT.SetTiltAcceleration( (uint)__tiltAccl.Value );
		}
		
	#endregion
#endregion
		
#region Joystick
		
		private bool   _useJoystick = false;
		private int    _pollDelay = 100;
		
		private void __joystick_CheckedChanged(object sender, EventArgs e) {
			
			if( __joystick.Checked ) {
				
				_useJoystick = true;
				
				PT.SetSpeedMode(PTSpeedControlMode.PureVelocity);
				
				__joystickPollThread.RunWorkerAsync();
				
			} else {
				
				_useJoystick = false;
				
				PT.SetSpeedMode(PTSpeedControlMode.Independent);
			}
			
		}
		
		private void __joystickPollThread_DoWork(object sender, DoWorkEventArgs e) {
			
			int panMaxSpeed  = PT.GetPanSpeedLimitMax();
			int tiltMaxSpeed = PT.GetTiltSpeedLimitMax();
			
			while( _useJoystick ) {
				
				JoystickStatus status = Program.Joystick.PollStatus();
				
				// JoyStatus axis values range from -1 to +1
				
				Int16  panSpeed = (Int16)( status.XAxis * panMaxSpeed );
				
				if( status.XAxis >= -0.10 && status.XAxis <= 0.10 ) {
					
					panSpeed = 0;
				}
				
				Int16 tiltSpeed = (Int16)( status.YAxis * tiltMaxSpeed );
				
				if( status.YAxis >= -0.10 && status.YAxis <= 0.10 ) {
					
					tiltSpeed = 0;
				}
				
				Invoke( new SetPTSpeedDelegate( SetPTSpeed ), panSpeed, tiltSpeed );
			
				Thread.Sleep( _pollDelay );
			}
			
		}
		
		private delegate void SetPTSpeedDelegate(Int16 pan, Int16 tilt);
		
		private void SetPTSpeed(Int16 pan, Int16 tilt) {
			
			PT.SetPanDesiredSpeed( pan );
			PT.SetTiltDesiredSpeed( tilt );
			
			RefreshAngleState(false);
		}
		
		
#endregion
		
	}
}
