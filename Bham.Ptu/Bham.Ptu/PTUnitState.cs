using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;

namespace Bham.Ptu {
	
	/// <summary>An in-memory cache of a PT Unit's state. Ideal for use with a PropertyGrid control.</summary>
	public class PTUnitState {
		
/*		private struct Value<T> where T : struct {
			private T _val;
			public bool Upd;
			
			public T Old {
				get; set;
			}
			
			public T Val {
				get { return _val; }
				set {
					_val = value;
					Upd = !value.Equals( Old );
				} 
			}
		} */
		
		private PTUnit _unit;
		
		public PTUnitState(PTUnit unit) {
			
			_unit = unit;
		}
		
		public void Refresh() {

// Stopwatch and timer used for debugging
List<long> times = new List<long>();			
Stopwatch sw = new Stopwatch();
sw.Start();
			
			_speedControlModeOld       = _speedControlMode      = _unit.GetSpeedMode();
			_commandModeOld            = _commandMode           = _unit.GetCommandMode();			
			_positionLimitsModeOld     = _positionLimitsMode    = _unit.GetPositionLimitsMode();
			
times.Add( sw.ElapsedTicks );
			
			_panMovePowerOld           = _panMovePower          = _unit.GetPanMovePower();
			_panHoldPowerOld           = _panHoldPower          = _unit.GetPanHoldPower();
			_tiltMovePowerOld          = _tiltMovePower         = _unit.GetTiltMovePower();
			_tiltHoldPowerOld          = _tiltHoldPower         = _unit.GetTiltHoldPower();
			
times.Add( sw.ElapsedTicks );
			
			_panPositionLimitMaxOld    = _panPositionLimitMax   = _unit.GetPanUserPositionLimitMax();
			_panPositionLimitMinOld    = _panPositionLimitMin   = _unit.GetPanUserPositionLimitMin();
			_tiltPositionLimitMaxOld   = _tiltPositionLimitMax  = _unit.GetTiltUserPositionLimitMax();
			_tiltPositionLimitMinOld   = _tiltPositionLimitMin  = _unit.GetTiltUserPositionLimitMin();
			
times.Add( sw.ElapsedTicks );
			
			_panSpeedLimitMaxOld       = _panSpeedLimitMax      = _unit.GetPanSpeedLimitMax();
			_panSpeedLimitMinOld       = _panSpeedLimitMin      = _unit.GetPanSpeedLimitMin();
			_tiltSpeedLimitMaxOld      = _tiltSpeedLimitMax     = _unit.GetTiltSpeedLimitMax();
			_tiltSpeedLimitMinOld      = _tiltSpeedLimitMin     = _unit.GetTiltSpeedLimitMin();
			
times.Add( sw.ElapsedTicks );
			
			_panStepModeOld            = _panStepMode           = _unit.GetPanStepMode();
			_tiltStepModeOld           = _tiltStepMode          = _unit.GetTiltStepMode();
			
times.Add( sw.ElapsedTicks );
			
			_echoOld                   = _echo                  = _unit.GetEchoEnabled();
			_verboseOld                = _verbose               = _unit.GetVerboseEnabled();
			_joystickOld               = _joystick              = _unit.GetJoystickEnabled();
			
times.Add( sw.ElapsedTicks );
			
			PanPositionResolution      = _unit.GetPanPositionResolution();
			PanPositionResolutionArcs  = _unit.GetPanPositionResolutionArcs();
			TiltPositionResolution     = _unit.GetTiltPositionResolution();
			TiltPositionResolutionArcs = _unit.GetTiltPositionResolutionArcs();
			
times.Add( sw.ElapsedTicks );
			
			PanPositionLimitMax        = _unit.GetPanPositionLimitMax();
			PanPositionLimitMin        = _unit.GetPanPositionLimitMin();
			TiltPositionLimitMax       = _unit.GetTiltPositionLimitMax();
			TiltPositionLimitMin       = _unit.GetTiltPositionLimitMin();
			
times.Add( sw.ElapsedTicks );
			
			PanDesiredPosition         = _unit.GetPanDesiredPosition();
			PanCurrentPosition         = _unit.GetPanCurrentPosition();
			
			TiltDesiredPosition        = _unit.GetTiltDesiredPosition();
			TiltCurrentPosition        = _unit.GetTiltCurrentPosition();
			
times.Add( sw.ElapsedTicks );
			
			PanDesiredSpeed            = _unit.GetPanDesiredSpeed();
			PanCurrentSpeed            = _unit.GetPanCurrentSpeed();
			
			TiltDesiredSpeed           = _unit.GetTiltDesiredSpeed();
			TiltCurrentSpeed           = _unit.GetTiltCurrentSpeed();
			
times.Add( sw.ElapsedTicks );
			
			PanBaseSpeed               = _unit.GetPanBaseSpeed();
			PanAcceleration            = _unit.GetPanAcceleration();
			
			TiltBaseSpeed              = _unit.GetTiltBaseSpeed();
			TiltAcceleration           = _unit.GetTiltAcceleration();
			
sw.Stop();
times.Add( sw.ElapsedTicks );
		}
		
		public void SaveChanges() {
			
			if( _speedControlModeUpd ) {
				_unit.SetSpeedMode( _speedControlMode );
				_speedControlModeUpd = false;
			}
			
			if( _commandModeUpd ) {
				_unit.SetCommandMode( _commandMode );
				_commandModeUpd = false;
			}
			
			if( _positionLimitsModeUpd ) {
				_unit.SetPositionLimitsMode( _positionLimitsMode );
				_positionLimitsModeUpd = false;
			}
			
			///////////////////////////////
			
			if( _panMovePowerUpd ) {
				_unit.SetPanMovePower( _panMovePower );
				_panMovePowerUpd = false;
			}
			if( _panHoldPowerUpd ) {
				_unit.SetPanHoldPower( _panHoldPower );
				_panHoldPowerUpd = false;
			}
			if( _tiltMovePowerUpd ) {
				_unit.SetTiltMovePower( _tiltMovePower );
				_tiltMovePowerUpd = false;
			}
			if( _tiltHoldPowerUpd ) {
				_unit.SetTiltHoldPower( _tiltHoldPower );
				_tiltHoldPowerUpd = false;
			}
			
			///////////////////////////////
			
			if( _panPositionLimitMaxUpd ) {
				_unit.SetPanUserPositionLimitMax( _panPositionLimitMax );
				_panPositionLimitMaxUpd = false;
			}
			if( _panPositionLimitMinUpd ) {
				_unit.SetPanUserPositionLimitMin( _panPositionLimitMin );
				_panPositionLimitMinUpd = false;
			}
			if( _tiltPositionLimitMaxUpd ) {
				_unit.SetTiltUserPositionLimitMax( _tiltPositionLimitMax );
				_tiltPositionLimitMaxUpd = false;
			}
			if( _tiltPositionLimitMinUpd ) {
				_unit.SetTiltUserPositionLimitMin( _tiltPositionLimitMin );
				_tiltPositionLimitMinUpd = false;
			}
			
			///////////////////////////////
			
			if( _panSpeedLimitMaxUpd ) {
				_unit.SetPanSpeedLimitMax( _panSpeedLimitMax );
				_panSpeedLimitMaxUpd = false;
			}
			if( _panSpeedLimitMinUpd ) {
				_unit.SetPanSpeedLimitMin( _panSpeedLimitMin );
				_panSpeedLimitMinUpd = false;
			}
			if( _tiltSpeedLimitMaxUpd ) {
				_unit.SetTiltSpeedLimitMax( _tiltSpeedLimitMax );
				_tiltSpeedLimitMaxUpd = false;
			}
			if( _tiltSpeedLimitMinUpd ) {
				_unit.SetTiltSpeedLimitMin( _tiltSpeedLimitMin );
				_tiltSpeedLimitMinUpd = false;
			}
			
			///////////////////////////////
			
			if( _echoUpd ) {
				_unit.SetEcho( _echo );
				_echoUpd = false;
			}
			
			if( _verboseUpd ) {
				_unit.SetVerbose( _verbose );
				_verboseUpd = false;
			}
			
			if( _joystickUpd ) {
				_unit.SetJoystick( _joystick );
				_joystickUpd = false;
			}
			
			///////////////////////////////
			
			// do StepMode last
			
			if( _panStepModeUpd ) {
				_unit.SetPanStepMode( _panStepMode );
				_panStepModeUpd = false;
			}
			if( _tiltStepModeUpd ) {
				_unit.SetTiltStepMode( _tiltStepMode );
				_tiltStepModeUpd = false;
			}
			
			Refresh(); // this will re-set the 'Old' fields
		}
		
		public void ClearReset() {
			RequiresReset = false;
		}
		
#region Configuration
		
	#region Device
		
		private PTSpeedControlMode _speedControlModeOld;
		private PTSpeedControlMode _speedControlMode;
		private bool               _speedControlModeUpd;
		
		[Description("The current Speed Control Mode of the unit. In Independent mode, speed is an unsigned magnitude for positional commands. In PureVelocity mode the PTU is affected by signed changes in speed."),
		Category("Device"), DefaultValue(PTSpeedControlMode.Independent)]
		public PTSpeedControlMode SpeedControlMode {
			get { return _speedControlMode; }
			set {
				_speedControlMode = value;
				_speedControlModeUpd = value != _speedControlModeOld;
			 }
		}
		
		private PTCommandMode _commandModeOld;
		private PTCommandMode _commandMode;
		private bool          _commandModeUpd;
		
		[Category("Device"), DefaultValue(PTCommandMode.Immediate)]
		public PTCommandMode CommandMode {
			get { return _commandMode; }
			set {
				_commandMode = value;
				_commandModeUpd = value != _commandModeOld;
			}
		}
		
		private PTPositionLimitsMode _positionLimitsModeOld;
		private PTPositionLimitsMode _positionLimitsMode;
		private bool                 _positionLimitsModeUpd;
		
		[Category("Device"), DefaultValue(PTPositionLimitsMode.Factory)]
		public PTPositionLimitsMode PositionLimitsMode {
			get { return _positionLimitsMode; }
			set {
				_positionLimitsMode = value;
				_positionLimitsModeUpd = value != _positionLimitsModeOld;
			}
		}
		
		private Boolean _verboseOld;
		private Boolean _verbose;
		private bool    _verboseUpd;
		
		[Category("Device"), DefaultValue(true)]
		public Boolean VerboseEnabled {
			get { return _verbose; }
			set {
				_verbose = value;
				_verboseUpd = value != _verboseOld;
			}
		}
		
		private Boolean _echoOld;
		private Boolean _echo;
		private bool    _echoUpd;
		
		[Category("Device"), DefaultValue(true)]
		public Boolean EchoEnabled {
			get { return _echo; }
			set {
				_echo = value;
				_echoUpd = value != _echoOld;
			}
		}
		
		private Boolean _joystickOld;
		private Boolean _joystick;
		private bool    _joystickUpd;
		
		[Category("Device"), DefaultValue(false)]
		public Boolean JoystickEnabled {
			get { return _joystick; }
			set {
				_joystick = value;
				_joystickUpd = value != _joystickOld;
			}
		}
		
	#endregion
	#region Power
		
		private PTPower _panMovePowerOld;
		private PTPower _panMovePower;
		private bool    _panMovePowerUpd;
		
		[Category("Power"), DefaultValue(PTPower.RegularPower)]
		public PTPower PanMovePower {
			get { return _panMovePower; }
			set {
				if( value == PTPower.PowerOff ) throw new ArgumentOutOfRangeException("value", "Cannot use PowerOff for MovePower");
				_panMovePower = value;
				_panMovePowerUpd = value != _panMovePowerOld;
			}
		}
		
		private PTPower _panHoldPowerOld;
		private PTPower _panHoldPower;
		private bool    _panHoldPowerUpd;
		
		[Category("Power"), DefaultValue(PTPower.LowPower)]
		public PTPower PanHoldPower {
			get { return _panHoldPower; }
			set {
				if( value == PTPower.HighPower ) throw new ArgumentOutOfRangeException("value", "Cannot use HighPower for HoldPower");
				_panHoldPower = value;
				_panHoldPowerUpd = value != _panHoldPowerOld;
			}
		}
		
		private PTPower _tiltMovePowerOld;
		private PTPower _tiltMovePower;
		private bool    _tiltMovePowerUpd;
		
		[Category("Power"), DefaultValue(PTPower.RegularPower)]
		public PTPower TiltMovePower {
			get { return _tiltMovePower; }
			set {
				if( value == PTPower.PowerOff ) throw new ArgumentOutOfRangeException("value", "Cannot use PowerOff for MovePower");
				_tiltMovePower = value;
				_tiltMovePowerUpd = value != _tiltMovePowerOld;
			}
		}
		
		private PTPower _tiltHoldPowerOld;
		private PTPower _tiltHoldPower;
		private bool    _tiltHoldPowerUpd;
		
		[Category("Power"), DefaultValue(PTPower.LowPower)]
		public PTPower TiltHoldPower {
			get { return _tiltHoldPower; }
			set {
				if( value == PTPower.HighPower ) throw new ArgumentOutOfRangeException("value", "Cannot use HighPower for HoldPower");
				_tiltHoldPower = value;
				_tiltHoldPowerUpd = value != _tiltHoldPowerOld;
			}
		}
		
	#endregion
	#region Step Mode
		
		private PTStepMode _panStepModeOld;
		private PTStepMode _panStepMode;
		private bool       _panStepModeUpd;
		
		[Category("Axis"), DefaultValue(PTStepMode.Half)]
		public PTStepMode PanStepMode {
			get { return _panStepMode; }
			set {
				_panStepMode = value;
				_panStepModeUpd = RequiresReset = value != _panStepModeOld;
			}
		}
		
		private PTStepMode _tiltStepModeOld;
		private PTStepMode _tiltStepMode;
		private bool       _tiltStepModeUpd;
		
		[Category("Axis"), DefaultValue(PTStepMode.Half)]
		public PTStepMode TiltStepMode {
			get { return _tiltStepMode; }
			set {
				_tiltStepMode = value;
				_tiltStepModeUpd = RequiresReset = value != _tiltStepModeOld;
			}
		}
		
	#endregion
	#region Position Limits
		
		private Int16 _panPositionLimitMaxOld;
		private Int16 _panPositionLimitMax;
		private bool  _panPositionLimitMaxUpd;
		
		[Category("Position Limits")]
		public Int16 PanPositionUserLimitMax {
			get { return _panPositionLimitMax; }
			set {
				_panPositionLimitMax = value;
				_panPositionLimitMaxUpd = value != _panPositionLimitMaxOld;
			}
		}
		
		private Int16 _panPositionLimitMinOld;
		private Int16 _panPositionLimitMin;
		private bool  _panPositionLimitMinUpd;
		
		[Category("Position Limits")]
		public Int16  PanPositionUserLimitMin {
			get { return _panPositionLimitMin; }
			set {
				_panPositionLimitMin = value;
				_panPositionLimitMinUpd = value != _panPositionLimitMinOld;
			}
		}
		
		private Int16 _tiltPositionLimitMaxOld;
		private Int16 _tiltPositionLimitMax;
		private bool  _tiltPositionLimitMaxUpd;
		
		[Category("Position Limits")]
		public Int16 TiltPositionUserLimitMax {
			get { return _tiltPositionLimitMax; }
			set {
				_tiltPositionLimitMax = value;
				_tiltPositionLimitMaxUpd = value != _tiltPositionLimitMaxOld;
			}
		}
		
		private Int16 _tiltPositionLimitMinOld;
		private Int16 _tiltPositionLimitMin;
		private bool  _tiltPositionLimitMinUpd;
		
		[Category("Position Limits")]
		public Int16 TiltPositionUserLimitMin {
			get { return _tiltPositionLimitMin; }
			set {
				_tiltPositionLimitMin = value;
				_tiltPositionLimitMinUpd = value != _tiltPositionLimitMinOld;
			}
		}
		
	#endregion
	#region Speed Limits
		
		private UInt16 _panSpeedLimitMaxOld;
		private UInt16 _panSpeedLimitMax;
		private bool   _panSpeedLimitMaxUpd;
		
		[Category("Speed Limits")]
		public UInt16 PanSpeedLimitMax {
			get { return _panSpeedLimitMax; }
			set {
				_panSpeedLimitMax = value;
				_panSpeedLimitMaxUpd = value != _panSpeedLimitMaxOld;
			}
		}
		
		private UInt16 _panSpeedLimitMinOld;
		private UInt16 _panSpeedLimitMin;
		private bool   _panSpeedLimitMinUpd;
		
		[Category("Speed Limits")]
		public UInt16 PanSpeedLimitMin {
			get { return _panSpeedLimitMin; }
			set {
				_panSpeedLimitMin = value;
				_panSpeedLimitMinUpd = value != _panSpeedLimitMinOld;
			}
		}
		
		private UInt16 _tiltSpeedLimitMaxOld;
		private UInt16 _tiltSpeedLimitMax;
		private bool   _tiltSpeedLimitMaxUpd;
		
		[Category("Speed Limits")]
		public UInt16 TiltSpeedLimitMax {
			get { return _tiltSpeedLimitMax; }
			set {
				_tiltSpeedLimitMax = value;
				_tiltSpeedLimitMaxUpd = value != _tiltSpeedLimitMaxOld;
			}
		}
		
		private UInt16 _tiltSpeedLimitMinOld;
		private UInt16 _tiltSpeedLimitMin;
		private bool   _tiltSpeedLimitMinUpd;
		
		[Category("Speed Limits")]
		public UInt16 TiltSpeedLimitMin {
			get { return _tiltSpeedLimitMin; }
			set {
				_tiltSpeedLimitMin = value;
				_tiltSpeedLimitMinUpd = value != _tiltSpeedLimitMinOld;
			}
		}
		
	#endregion
		
#endregion
#region Read-Only State
		
		[Category("Device"), DefaultValue(false)]
		public bool RequiresReset {
			get; private set;
		}
		
		[Category("Position")] public UInt32 PanPositionResolution      { get; private set; }
		[Category("Position")] public Double PanPositionResolutionArcs  { get; private set; }
		
		[Category("Position")] public UInt32 TiltPositionResolution     { get; private set; }
		[Category("Position")] public Double TiltPositionResolutionArcs { get; private set; }
		
		[Category("Position")] public Int16 PanDesiredPosition { get; private set; }
		[Category("Position")] public Int16 PanCurrentPosition { get; private set; }
		
		[Category("Position")] public Int16 TiltDesiredPosition { get; private set; }
		[Category("Position")] public Int16 TiltCurrentPosition { get; private set; }
		
		// returns the factory limits, but there is no way to get these values until DP update their protocol/firmware
		
//		[Category("Position Limits")] public Int16 PanPositionFactoryLimitMax { get; private set; }
//		[Category("Position Limits")] public Int16 PanPositionFactoryLimitMin { get; private set; }
//		[Category("Position Limits")] public Int16 TiltPositionFactoryLimitMax { get; private set; }
//		[Category("Position Limits")] public Int16 TiltPositionFactoryLimitMin { get; private set; }
		
		// returns the limits that are in-effect
		
		[Category("Position Limits")] public Int16 PanPositionLimitMax { get; private set; }
		[Category("Position Limits")] public Int16 PanPositionLimitMin { get; private set; }
		[Category("Position Limits")] public Int16 TiltPositionLimitMax { get; private set; }
		[Category("Position Limits")] public Int16 TiltPositionLimitMin { get; private set; }
		
		[Category("Speed")] public UInt16 PanDesiredSpeed { get; private set; }
		[Category("Speed")] public UInt16 PanCurrentSpeed { get; private set; }
		
		[Category("Speed")] public UInt16 TiltDesiredSpeed { get; private set; }
		[Category("Speed")] public UInt16 TiltCurrentSpeed { get; private set; }
		
		[Category("Speed")] public UInt16 PanBaseSpeed    { get; private set; }
		[Category("Speed")] public UInt32 PanAcceleration { get; private set; }
		
		[Category("Speed")] public UInt16 TiltBaseSpeed    { get; private set; }
		[Category("Speed")] public UInt32 TiltAcceleration { get; private set; }
		
#endregion
		
	}
}
