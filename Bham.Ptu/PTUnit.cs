using System;
using System.Text.RegularExpressions;

using Cult = System.Globalization.CultureInfo;

using NS = System.Globalization.NumberStyles;

namespace Bham.Ptu {
	
	/// <summary>Encapsulates a single Directed Perception or FLIR Motion Control Pan/Tilt Unit connected to the specified serial port.</summary>
	public class PTUnit : IDisposable {
		
		private PTConnection       _c;
		
		private PTCommandMode        _commandMode;
		private PTSpeedControlMode   _controlMode;
		private PTPositionLimitsMode _limitsMode;
		
		private Double    _panResolution; // axis resolution is defined as the number of arcseconds per step, but is affected by the current stepping mode
		private Double    _tiltResolution;
		
		private PTStepMode _panStepMode;
		private PTStepMode _tiltStepMode;
		
		public static readonly PTFirmwareInfo MinSupportedVersion = new PTFirmwareInfo( 1, 7, 6 );
		private const int _defaultTimeout = 500; // 50ms default timeout for nonblocking operations.
		
		public PTUnit(String comPortName) {
			
			ComPort = comPortName;
			_c = new PTConnection(comPortName);
			
			Initialize();
		}
		
		public void Initialize() {
			
			_c.ResetParser();
			
			_c.SetTimeout( _defaultTimeout );
			
			this.SetSpeedMode(PTSpeedControlMode.Independent);
			this.SetCommandMode(PTCommandMode.Immediate);
			
			_panResolution  = this.GetPanPositionResolutionArcs();
			_tiltResolution = this.GetTiltPositionResolutionArcs();
			
			_panStepMode  = this.GetPanStepMode();
			_tiltStepMode = this.GetTiltStepMode();
			
			_limitsMode = this.GetPositionLimitsMode();
		}
		
		public String ComPort {
			get; private set;
		}
		
#region IDisposable
		public void Dispose() {
			
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		protected virtual void Dispose(bool managed) {
			
			if( managed ) {
				
				if( _c != null ) _c.Dispose();
			}
		}
#endregion
		
#region Utility
		
		private void ValidateResult(Byte result) {
			ValidateResult(result, false);
		}
		
		/// <summary>Determines if the returned status code indicates that the last operation was executed successfully (then it returns), otherwise a PTControllerException is raised.</summary>
		private void ValidateResult(Byte result, bool async) {
			
			result = Utility.GetUnsignedByteValue( result );
			
			const byte asyncMin = (byte)PTResult.PanLimitHit;
			const byte asyncMax = (byte)PTResult.TiltSpeedTriggerHit;
			
			if( async ) {
				
				while(result >= asyncMin && result <= asyncMax ) {
					
					if( result == (byte)PTResult.CableDisconnectDetected )
						throw new PTControllerException("Asynchronous Cable Disconnection detected");
					
					result = _c.GetByte();
					result = Utility.GetUnsignedByteValue( result );
				}
				
			}
			
			if( result != (byte)PTResult.Ok ) {
				
				const byte errorMin = (byte)PTResult.IllegalCommandArgument;
				const byte errorMax = (byte)PTResult.FirmwareVersionTooLow;
				
				if( result >= errorMin && result <= errorMax ) {
					
					PTResult ptResult = (PTResult)result;
					throw new PTControllerException( ptResult );
				
				} else if( result == (Byte)PTCommand.Halt ) {
					
					// NOOP
					
				} else {
					
					// There are a few anomalous results I haven't been able to reliably reproduce
					// there was one in the 80s, and another around 250
					
					throw new PTControllerException("Non-OK but Unknown Result: " + result);
				}
			}
		}
		
		/// <summary>Inspects an ASCII response. If the response is OK, it removes the status response at the beginning of the line and returns it so you can process it. Returns null if there was no value to return (e.g. from a Setter operation).</summary>
		private static String ValidateResult(String asciiResponse) {
			
			// successful responses are in the format "* <result>d"
			// failure responses are "! <message>\r"
			// async responses are "!P" or "!T"
			
			// it seems there's an undocumented special case when setting the step-mode of the D46 where it returns '??*'
			// so catch that too
			
			if( String.IsNullOrEmpty( asciiResponse ) ) throw new ArgumentException("No response given", "asciiResponse");
			
			// OK response
			if( asciiResponse[0] == '*' ) {
				
				if( asciiResponse.Length == 1 ) return null;
				return asciiResponse.Substring( 2 ); // 0 = '*' 1 = ' '
			}
			if( asciiResponse == "??*" ) return null;
			
			if( asciiResponse.Length < 2 ) throw new ArgumentException("Invalid response given (too short)", "asciiResponse");
			
			// Async response
			if( asciiResponse[0] == '!' && (asciiResponse[1] == 'P' || asciiResponse[1] == 'T' ) ) return null;
			
			// Failure
			throw new PTControllerException("ASCII Command Failure: " + asciiResponse );
		}
		
		private String AsciiExchange(String command, int timeout) {
			
			// Save current ASCII protocol state
			bool verbose = _c.GetByte( PTCommand.GetVerbose ) == 1;
			bool echo    = _c.GetByte( PTCommand.GetEcho    ) == 1;
			
			Byte result;
			
			result = _c.GetByte( PTCommand.SetEchoDisabled );
			ValidateResult( result );
			result = _c.GetByte( PTCommand.SetVerboseDisabled );
			ValidateResult( result );
			
			///////////////////////////////////////////
			
			_c.SetTimeout( timeout );
			
			String response = _c.AsciiExchange( command ); // the delimiter is added by PTConnection.WriteCommand
			
			_c.SetTimeout( _defaultTimeout );
			
			///////////////////////////////////////////
			
			result = _c.GetByte( verbose ? PTCommand.SetVerboseEnabled : PTCommand.SetVerboseDisabled );
			ValidateResult( result );
			result = _c.GetByte( echo ? PTCommand.SetEchoEnabled : PTCommand.SetEchoDisabled );
			ValidateResult( result );
			
			return response;
		}
		
		private Int16 AsciiExchangeInt16(String command, int timeout) {
			
			String response = AsciiExchange(command, timeout );
			
			response = ValidateResult( response );
			
			Int16 ret = 0;
			if( Int16.TryParse( response, NS.Integer, Cult.InvariantCulture, out ret ) ) return ret;
			else throw new PTException("Could not parse ASCII PT unit response: " + response );
		}
		
		public Double ConvertPanStepsToDegrees(int steps) {
			
			// 1. Modify steps by the current stepping
			// 2. Convert steps to sarcs
			// 3. Convert sarcs to degrees
			
			double sarcs = steps * _panResolution;
			
			double degrees = sarcs / (60 * 60);
			return degrees;
		}
		
		public Double ConvertTiltStepsToDegrees(int steps) {
			
			double sarcs = steps * _tiltResolution;
			
			double degrees = sarcs / (60 * 60);
			return degrees;
		}
		
		public Int16 ConvertPanDegreesToSteps(Double degrees) {
			
			// 1. convert degrees into sarcs
			// 2. convert sarcs into steps
			
			double sarcs = degrees * 60 * 60;
			
			double steps = sarcs / _panResolution;
			
			short ret = (Int16)steps;
			
			return ret;
		}
		
		public Int16 ConvertTiltDegreesToSteps(Double degrees) {
			
			double sarcs = degrees * 60 * 60;
			
			double steps = sarcs / _tiltResolution;
			
			short ret = (Int16)steps;
			
			return ret;
		}
		
		public Double PanResolution {
			get { return _panResolution; }
		}
		
		public Double TiltResolution {
			get { return _tiltResolution; }
		}
		
		public PTPositionLimitsMode LimitsMode {
			get { return _limitsMode; }
		}
		
#endregion
		
#region Device Configuration and Misc
		
		/// <summary>Tests the specified serial port to see if there is a PTU controller attached. This method is blocking, but throws if the unit is unresponsive (2000ms timeout).</summary>
		public static PTFirmwareInfo GetFirmwareInfo(String portName) {
			
			PTFirmwareInfo ret;
			
			using(PTConnection c = new PTConnection(portName)) {
				
				try {
					
					c.ResetParser();
					
					c.SetTimeout( 500 );
					String firmware = c.GetLine(PTCommand.GetFirmwareVersion);
					ret = PTFirmwareInfo.FromString( firmware );
					c.SetTimeout( _defaultTimeout );
					
				} catch(TimeoutException) {
					
					return null;
				}
			}
			
			return ret;
			
		}
		
		/// <summary>This operation is blocking.</summary>
		public String GetFirmwareInfo() {
			
			_c.SetTimeout( 500 );
			
			String firmwareString = _c.GetLine(PTCommand.GetFirmwareVersion);
			
			_c.SetTimeout( _defaultTimeout );
			
			return firmwareString;
		}
		
		/// <summary>This operation is blocking.</summary>
		public String GetEnvironmentInfo() {
			
			// GetEnvironment takes a looooong time to return, so a 5-sec timeout period is fine.
			
			String environment = AsciiExchange( PTAsciiCommand.GetEnvironment, 5000 );
			
			environment = environment.Substring( environment.IndexOf(' ') + 1 );
			
			return environment;
		}
		
		/// <summary>This operation is blocking.</summary>
		public void Reset() {
			
			_c.SetTimeout( -1 );
			
			lock( _c.SyncRoot ) { // nested locks are okay!
				
				Byte result = _c.GetByte(PTCommand.UnitReset);
				
				ValidateResult( result, true );
				
			}
			
			_c.SetTimeout( _defaultTimeout );
			
			////////////////////
			
			Initialize();
		}
		
		/// <summary>This operation is blocking.</summary>
		public void ResetPan() {
			
			_c.SetTimeout( -1 );
			
			lock( _c.SyncRoot ) { // nested locks are okay!
				
				Byte result = _c.GetByte(PTCommand.UnitResetPan);
				
				ValidateResult( result, true );
				
			}
			
			_c.SetTimeout( _defaultTimeout );
			
			////////////////////
			
			Initialize();
		}
		
		/// <summary>This operation is blocking.</summary>
		public void ResetTilt() {
			
			_c.SetTimeout( -1 );
			
			lock( _c.SyncRoot ) { // nested locks are okay!
				
				Byte result = _c.GetByte(PTCommand.UnitResetTilt);
				
				ValidateResult( result, true );
				
			}
			
			_c.SetTimeout( _defaultTimeout );
			
			////////////////////
			
			Initialize();
		}
		
		/// <summary>This operation is blocking.</summary>
		public void ResetParser() {
			
			_c.ResetParser();
		}
		
		/// <summary>This operation is blocking.</summary>
		public void AwaitCompletion() {
			
			if( _controlMode == PTSpeedControlMode.PureVelocity ) return;
			
			_c.SetTimeout( -1 );
			
			Byte result = _c.GetByte( PTCommand.AwaitCommandCompletion );
			
			_c.SetTimeout( _defaultTimeout );
			
			ValidateResult( result );
			
		}
		
		/// <summary>Indicates whether position limits are enabled or disabled. However there is no way to determine if these limits are User limits or Factory limits.</summary>
		public bool GetPositionLimitsEnabled() {
			
			Byte result = _c.GetByte( PTCommand.GetPositionLimits );
			return result == 1;
		}
		
		/// <summary>Determines the current position limits mode by toying with the unit. This method is blocking.</summary>
		public PTPositionLimitsMode GetPositionLimitsMode() {
			
			// if disabled, return
			
			// record the current position limits
			// then explicitly set to factory
			// record the limits
			// then explicitly set to user
			// record the limits
			// see which limits refer to the original limits
			// then set the position limits mode to what it was originally
			
			if( !GetPositionLimitsEnabled() ) return PTPositionLimitsMode.Disabled;
			
			short oPanMin = GetPanPositionLimitMin();
			short oPanMax = GetPanPositionLimitMax();
			short oTilMin = GetTiltPositionLimitMin();
			short oTilMax = GetTiltPositionLimitMax();
			
			SetPositionLimitsMode(PTPositionLimitsMode.User);
			
			short uPanMin = GetPanUserPositionLimitMin();
			short uPanMax = GetPanUserPositionLimitMax();
			short uTilMin = GetTiltUserPositionLimitMin();
			short uTilMax = GetTiltUserPositionLimitMax();
			
			SetPositionLimitsMode(PTPositionLimitsMode.Factory);
			
			short fPanMin = GetPanPositionLimitMin();
			short fPanMax = GetPanPositionLimitMax();
			short fTilMin = GetTiltPositionLimitMin();
			short fTilMax = GetTiltPositionLimitMax();
			
			///////////////////////////////////
			
			PTPositionLimitsMode mode = PTPositionLimitsMode.Disabled;
			
			// note that this approach falls apart if user limits are enabled, and user limits equal the factory limits
			// because switching between user mode and factory mode yields the same limits, so there is no way to know
			// oo-er!
			
			// the only way I can think of is to explicitly set a user limit to a known value, then compare, then restore
			
			if( oPanMin == fPanMin &&
			    oPanMax == fPanMax &&
			    oTilMin == fTilMin &&
			    oTilMax == fTilMax ) {
				
				// user mode
				mode = PTPositionLimitsMode.Factory;
				
			} else if(
			    oPanMin == uPanMin &&
			    oPanMax == uPanMax &&
			    oTilMin == uTilMin &&
			    oTilMax == uTilMax ) {
				
				mode = PTPositionLimitsMode.User;
				
			} else {
				
				SetPositionLimitsMode(mode);
				throw new PTControllerException("Could not determine position limits mode");
			}
			
			SetPositionLimitsMode(mode);
			
			return mode;
		}
		
		public void SetPositionLimitsMode(PTPositionLimitsMode mode) {
			
			switch(mode) {
				case PTPositionLimitsMode.Disabled:
					
					Byte result1 = _c.GetByte( PTCommand.SetPositionLimitsDisabled );
					ValidateResult( result1 );
					
					break;
				case PTPositionLimitsMode.Factory:
					
					Byte result2 = _c.GetByte( PTCommand.SetPositionLimitsEnabled );
					ValidateResult( result2 );
					
					break;
				case PTPositionLimitsMode.User:
					
					String response = AsciiExchange( PTAsciiCommand.SetPositionLimitsUser, _defaultTimeout );
					ValidateResult( response );
					
					break;
			}
			
		}
		
		public void Halt() {
			_c.SetTimeout( -1 );
			ValidateResult( _c.GetByte( PTCommand.Halt ) );
			_c.SetTimeout( _defaultTimeout );
		}
		
		public void HaltPan() {
			_c.SetTimeout( -1 );
			ValidateResult( _c.GetByte( PTCommand.HaltPan ) );
			_c.SetTimeout( _defaultTimeout );
		}
		
		public void HaltTilt() {
			_c.SetTimeout( -1 );
			ValidateResult( _c.GetByte( PTCommand.HaltTilt ) );
			_c.SetTimeout( _defaultTimeout );
		}
		
		public void SaveConfiguration() {
			_c.SetTimeout( -1 );
			ValidateResult( _c.GetByte( PTCommand.DefaultsSave ) );
			_c.SetTimeout( _defaultTimeout );
		}
		
		public void RestoreConfiguration() {
			_c.SetTimeout( -1 );
			ValidateResult( _c.GetByte( PTCommand.DefaultsRestore ) );
			_c.SetTimeout( _defaultTimeout );
		}
		
		public void FactoryResetConfiguration() {
			_c.SetTimeout( -1 );
			ValidateResult( _c.GetByte( PTCommand.DefaultsFactoryRestore ) );
			_c.SetTimeout( _defaultTimeout );
		}
		
		public bool GetEchoEnabled() {
			return _c.GetByte(PTCommand.GetEcho) == 1;
		}
		
		public void SetEcho(bool enabled) {
			Byte result = _c.GetByte( enabled ? PTCommand.SetEchoEnabled : PTCommand.SetEchoDisabled );
			ValidateResult( result );
		}
		
		public bool GetVerboseEnabled() {
			return _c.GetByte(PTCommand.GetVerbose) == 1;
		}
		
		public void SetVerbose(bool enabled) {
			Byte result = _c.GetByte( enabled ? PTCommand.SetVerboseEnabled : PTCommand.SetVerboseDisabled );
			ValidateResult( result );
		}
		
		public bool GetJoystickEnabled() {
			return _c.GetByte(PTCommand.GetJoystick) == 1;
		}
		
		public void SetJoystick(bool enabled) {
			Byte result = _c.GetByte( enabled ? PTCommand.SetJoystickEnabled : PTCommand.SetJoystickDisabled );
			ValidateResult( result );
		}
		
		public void SetResetOnPowerUp(bool enabled) {
			Byte result = _c.GetByte( enabled ? PTCommand.UnitResetOnPowerUpEnabled : PTCommand.UnitResetOnPowerUpDisabled );
			ValidateResult( result );
		}
		
	#region Modes
		
		public void SetCommandMode(PTCommandMode mode) {
			
			PTCommand cmd = mode == PTCommandMode.Immediate ?
				PTCommand.SetCommandModeImmediate :
				PTCommand.SetCommandModeSlaved;
			
			Byte result = _c.GetByte( cmd );
			
			ValidateResult( result );
			
			this._commandMode = mode;
		}
		
		public PTCommandMode GetCommandMode() {
			return _commandMode;
		}
		
		public void SetSpeedMode(PTSpeedControlMode mode) {
			
			PTCommand cmd = mode == PTSpeedControlMode.Independent ?
				PTCommand.SetControlModeIndependent :
				PTCommand.SetControlModePureVelocity;
			
			Byte result = _c.GetByte( cmd );
			
			ValidateResult( result );
			
			this._controlMode = mode;
		}
		
		public PTSpeedControlMode GetSpeedMode() {
			
			Byte b = _c.GetByte( PTCommand.GetControlMode );
			_controlMode = (PTSpeedControlMode)b;
			return _controlMode;
		}
		
	#endregion
		
#endregion
		
#region Resolution and Limits
		
	#region Pan
		
		/// <summary>Returns 60x the Arc/second resolution of the unit in Half-Step resolution Mode. You *must* take the current stepping into consideration when calculating angles. See GetPanStepMode.</summary>
		public UInt32 GetPanPositionResolution() {
			
			UInt32 value = _c.GetUInt32( PTCommand.GetPanResolution );
			return value;
		}
		
		/// <summary>Returns an Arc/sec value.</summary>
		public Double GetPanPositionResolutionArcs() {
			// with the binary protocol, the unit returns 60*sarcs
			// so to get the sarcs, divide by 60
			// note the value returned is actually *less* precise as the ASCII protocol version
			
			// They could have just returned an IEEE754 value
			UInt32 value = GetPanPositionResolution();
			
			Double ret = (double)value / 60d;
			
			return ret;
		}
		
		/// <summary>Returns the factory-defined maximum pan position. This limit cannot be changed, but limit enforcement can be disabled.</summary>
		public Int16 GetPanPositionLimitMax() {
			
			Int16 value = _c.GetInt16( PTCommand.GetPanPositionMax );
			
			return value;
		}
		
		/// <summary>Returns the factory-defined minimum pan position. This limit cannot be changed, but limit enforcement can be disabled.</summary>
		public Int16 GetPanPositionLimitMin() {
			
			Int16 value = _c.GetInt16( PTCommand.GetPanPositionMin );
			
			return value;
		}
		
		/// <summary>Returns the user-defined maximum pan position. This limit can be changed and also disabled.</summary>
		public Int16 GetPanUserPositionLimitMax() {
			
			return AsciiExchangeInt16( PTAsciiCommand.GetPanUserPositionLimitMax, _defaultTimeout );
		}
		
		/// <remarks>To use UserPositionLimits you must SetPositionLimitsMode to PTPositionLimitsMode.User</remarks>
		public void SetPanUserPositionLimitMax(Int16 limit) {
			
			String command = PTAsciiCommand.SetPanUserPositionLimitMax;
			command = String.Format( command, limit );
			String response = AsciiExchange( command, _defaultTimeout );
			ValidateResult( response );
		}
		
		/// <summary>Returns the user-defined minimum pan position. This limit can be changed and also disabled.</summary>
		public Int16 GetPanUserPositionLimitMin() {
			
			return AsciiExchangeInt16( PTAsciiCommand.GetPanUserPositionLimitMin, _defaultTimeout );
		}
		
		/// <remarks>To use UserPositionLimits you must SetPositionLimitsMode to PTPositionLimitsMode.User</remarks>
		public void SetPanUserPositionLimitMin(Int16 limit) {
			
			String command = PTAsciiCommand.SetPanUserPositionLimitMin;
			command = String.Format( command, limit );
			String response = AsciiExchange( command, _defaultTimeout );
			ValidateResult( response );
		}
		
		/// <remarks>The D46 performs an axis calibration reset when executing this command, but the D48 doesn't (however the D48 must be reset after executing the command).</remarks>
		public void SetPanStepMode(PTStepMode mode) {
			
			_panStepMode = mode;
			
			String command = String.Format( PTAsciiCommand.SetPanStepMode, (char)mode );
			
			String response = AsciiExchange( command, -1 );
			
			ValidateResult( response );
			
			// Update the resolution
			_panResolution = this.GetPanPositionResolutionArcs();
		}
		
		public PTStepMode GetPanStepMode() {
			
			String response = AsciiExchange( PTAsciiCommand.GetPanStepMode, 250 );
			
			// For some reason the controller returns in this form; "H*" rather than "*H", so don't validate it
			
			char c = response[0];
			return _panStepMode = (PTStepMode)c;
		}
		
	#endregion
	
	#region Tilt
		
		public UInt32 GetTiltPositionResolution() {
			
			UInt32 value = _c.GetUInt32( PTCommand.GetTiltResolution );
			return value;
		}
		
		/// <summary>Returns an Arc/sec value.</summary>
		public Double GetTiltPositionResolutionArcs() {
			
			UInt32 value = GetTiltPositionResolution();
			
			Double ret = (double)value / 60d;
			
			return ret;
		}
		
		public Int16 GetTiltPositionLimitMax() {
			
			Int16 value = _c.GetInt16( PTCommand.GetTiltPositionMax );
			return value;
		}
		
		public Int16 GetTiltPositionLimitMin() {
			
			Int16 value = _c.GetInt16( PTCommand.GetTiltPositionMin );
			return value;
		}
		
		/// <summary>Returns the user-defined maximum Tilt position. This limit can be changed and also disabled.</summary>
		public Int16 GetTiltUserPositionLimitMax() {
			
			String command = PTAsciiCommand.GetTiltUserPositionLimitMax;
			return AsciiExchangeInt16( command, _defaultTimeout );
		}
		
		/// <remarks>To use UserPositionLimits you must SetPositionLimitsMode to PTPositionLimitsMode.User</remarks>
		public void SetTiltUserPositionLimitMax(Int16 limit) {
			
			String command = PTAsciiCommand.SetTiltUserPositionLimitMax;
			command = String.Format( command, limit );
			String response = AsciiExchange( command, _defaultTimeout );
			ValidateResult( response );
		}
		
		/// <summary>Returns the user-defined minimum Tilt position. This limit can be changed and also disabled.</summary>
		public Int16 GetTiltUserPositionLimitMin() {
			
			return AsciiExchangeInt16( PTAsciiCommand.GetTiltUserPositionLimitMin, _defaultTimeout );
		}
		
		/// <remarks>To use UserPositionLimits you must SetPositionLimitsMode to PTPositionLimitsMode.User</remarks>
		public void SetTiltUserPositionLimitMin(Int16 limit) {
			
			String command = PTAsciiCommand.SetTiltUserPositionLimitMin;
			command = String.Format( command, limit );
			String response = AsciiExchange( command, _defaultTimeout );
			ValidateResult( response );
		}
		
		/// <remarks>The D46 performs an axis calibration reset when executing this command, but the D48 doesn't (however the D48 must be reset after executing the command).</remarks>
		public void SetTiltStepMode(PTStepMode mode) {
			
			_tiltStepMode = mode;
			
			String command = String.Format( PTAsciiCommand.SetTiltStepMode, (char)mode );
			
			String response = AsciiExchange( command, -1 );
			
			ValidateResult( response );
			
			_tiltResolution = this.GetTiltPositionResolutionArcs();
		}
		
		public PTStepMode GetTiltStepMode() {
			
			String response = AsciiExchange( PTAsciiCommand.GetTiltStepMode, 250 );
			
			// For some reason the controller returns in this form; "H*\r" rather than "*H\r"
			
			char c = response[0];
			return _tiltStepMode = (PTStepMode)c;
		}
		
	#endregion
		
#endregion
		
#region Position
		
	#region Pan
		
		public Int16 GetPanDesiredPosition() {
			
			Int16 result = _c.GetInt16( PTCommand.GetPanPositionDesired );
			return result;
		}
		
		public Int16 GetPanCurrentPosition() {
			
			Int16 result = _c.GetInt16( PTCommand.GetPanPositionCurrent );
			return result;
		}
		
		public void SetPanDesiredPosition(Int16 step) {
			
			Byte result = _c.GetByte( PTCommand.SetPanPositionAbsolute, step );
			ValidateResult( result );
		}
		
		public void SetPanDesiredPositionDelta(Int16 deltaStep) {
			
			Byte result = _c.GetByte( PTCommand.SetPanPositionOffset, deltaStep );
			ValidateResult( result );
		}
		
	#endregion
	#region Tilt
		
		public Int16 GetTiltDesiredPosition() {
			
			Int16 result = _c.GetInt16( PTCommand.GetTiltPositionDesired );
			return result;
		}
		
		public Int16 GetTiltCurrentPosition() {
			
			Int16 result = _c.GetInt16( PTCommand.GetTiltPositionCurrent );
			return result;
		}
		
		public void SetTiltDesiredPosition(Int16 step) {
			
			Byte result = _c.GetByte( PTCommand.SetTiltPositionAbsolute, step );
			ValidateResult( result );
		}
		
		public void SetTiltDesiredPositionDelta(Int16 deltaStep) {
			
			Byte result = _c.GetByte( PTCommand.SetTiltPositionOffset, deltaStep);
			ValidateResult( result );
		}
		
	#endregion
		
#endregion
	
#region Speed and Acceleration
	
	#region Pan
		
		public UInt16 GetPanCurrentSpeed() {
			
			UInt16 result = _c.GetUInt16( PTCommand.GetPanSpeedCurrent );
			return result;
		}
		
		public UInt16 GetPanDesiredSpeed() {
			
			UInt16 result = _c.GetUInt16( PTCommand.GetPanSpeedDesired );
			return result;
		}
		
		public UInt16 GetPanBaseSpeed() {
			
			UInt16 result = _c.GetUInt16( PTCommand.GetPanSpeedBase );
			return result;
		}
		
		/// <summary>Returns the acceleration and deceleration of the pan axis' motion for speeds above the Base Speed. Acceleration is specified in steps/second^2.</summary>
		public UInt32 GetPanAcceleration() {
			
			UInt32 result = _c.GetUInt32( PTCommand.GetPanAcceleration );
			return result;
		}
		
		/// <remarks>In Pure Velocity mode, speed is interpreted as a signed 16-bit integer. In Independent Mode it is interpreted as an unsigned magnitude.</remarks>
		public void SetPanDesiredSpeed(Int16 speed) {
			
			Byte result = _c.GetByte( PTCommand.SetPanSpeedAbsolute, speed );
			
			ValidateResult( result );
		}
		
		public void SetPanDesiredSpeedDelta(Int16 speed) {
			
			Byte result = _c.GetByte( PTCommand.SetPanSpeedOffset, speed );
			
			ValidateResult( result );
		}
		
		/// <summary>Sets the base (start-up or initial) speed. Base speed is specified in steps/second. The default is 57 steps/second.</summary>
		/// <remarks>This method is blocking. Changes to the base speed cannot be made instantly because it needs to recompute the internal acceleration tables.</remarks>
		public void SetPanBaseSpeed(UInt16 speed) {
			
			_c.SetTimeout( -1 );
			
			Byte result = _c.GetByte( PTCommand.SetPanBaseSpeed, speed );
			ValidateResult( result );
			
			_c.SetTimeout( _defaultTimeout );
		}
		
		/// <summary>Sets the acceleration and deceleration of the pan axis' motion for speeds above the Base Speed. Acceleration is specified in steps/second^2. There is no upper-bound defined.</summary>
		/// <remarks>This method is blocking. The controller needs to recompute the acceleration tables.</remarks>
		public void SetPanAcceleration(UInt32 acceleration) {
			
			_c.SetTimeout( -1 );
			
			Byte result = _c.GetByte( PTCommand.SetPanAcceleration, acceleration );
			ValidateResult( result );
			
			_c.SetTimeout( _defaultTimeout );
		}
		
	#endregion
	#region Tilt
		
		public UInt16 GetTiltCurrentSpeed() {
			
			UInt16 result = _c.GetUInt16( PTCommand.GetTiltSpeedCurrent );
			return result;
		}
		
		public UInt16 GetTiltDesiredSpeed() {
			
			UInt16 result = _c.GetUInt16( PTCommand.GetTiltSpeedDesired );
			return result;
		}
		
		public UInt16 GetTiltBaseSpeed() {
			
			UInt16 result = _c.GetUInt16( PTCommand.GetTiltSpeedBase );
			return result;
		}
		
		public UInt32 GetTiltAcceleration() {
			
			UInt32 result = _c.GetUInt32( PTCommand.GetTiltAcceleration );
			return result;
		}
		
		/// <remarks>In Pure Velocity mode, speed is interpreted as a signed 16-bit integer. In Independent Mode it is interpreted as an unsigned magnitude.</remarks>
		public void SetTiltDesiredSpeed(Int16 speed) {
			
			Byte result = _c.GetByte( PTCommand.SetTiltSpeedAbsolute, speed );
			
			ValidateResult( result );
		}
		
		public void SetTiltDesiredSpeedDelta(Int16 speed) {
			
			Byte result = _c.GetByte( PTCommand.SetTiltSpeedOffset, speed );
			ValidateResult( result );
		}
		
		public void SetTiltBaseSpeed(UInt16 speed) {
			
			_c.SetTimeout( -1 );
			
			Byte result = _c.GetByte( PTCommand.SetTiltBaseSpeed, speed );
			ValidateResult( result );
			
			_c.SetTimeout( _defaultTimeout );
		}
		
		public void SetTiltAcceleration(UInt32 acceleration) {
			
			_c.SetTimeout( -1 );
			
			Byte result = _c.GetByte( PTCommand.SetTiltAcceleration, acceleration );
			ValidateResult( result );
			
			_c.SetTimeout( _defaultTimeout );
		}
		
	#endregion
	
#endregion
		
#region Speed Limits
		
	#region Pan
		
		public void SetPanSpeedLimitMin(UInt16 speedLimit) {
			Byte result = _c.GetByte( PTCommand.SetPanSpeedLimitMin, speedLimit );
			ValidateResult( result );
		}
		
		/// <summary>Returns the user-defined minimum pan Speed. This limit can be changed and also disabled.</summary>
		public UInt16 GetPanSpeedLimitMin() {
			
			UInt16 value = _c.GetUInt16(PTCommand.GetPanSpeedLimitMin);
			return value;
		}
		
		/// <summary>This method is blocking. This method causes a recomputation of the internal acceleration tables.</summary>
		public void SetPanSpeedLimitMax(UInt16 speedLimit) {
			_c.SetTimeout( -1 );
			Byte result = _c.GetByte( PTCommand.SetPanSpeedLimitMax, speedLimit );
			ValidateResult( result );
			_c.SetTimeout( _defaultTimeout );
		}
		
		/// <summary>Returns the user-defined maximum pan Speed. This limit can be changed and also disabled.</summary>
		public UInt16 GetPanSpeedLimitMax() {
			
			UInt16 value = _c.GetUInt16(PTCommand.GetPanSpeedLimitMax);
			return value;
		}
		
	#endregion
	#region Tilt
		
		public void SetTiltSpeedLimitMin(UInt16 speedLimit) {
			Byte result = _c.GetByte( PTCommand.SetTiltSpeedLimitMin, speedLimit );
			ValidateResult( result );
		}
		
		/// <summary>Returns the user-defined minimum Tilt Speed. This limit can be changed and also disabled.</summary>
		public UInt16 GetTiltSpeedLimitMin() {
			
			UInt16 value = _c.GetUInt16(PTCommand.GetTiltSpeedLimitMin);
			return value;
		}
		
		/// <summary>This method is blocking. This method causes a recomputation of the internal acceleration tables.</summary>
		public void SetTiltSpeedLimitMax(UInt16 speedLimit) {
			_c.SetTimeout( -1 );
			Byte result = _c.GetByte( PTCommand.SetTiltSpeedLimitMax, speedLimit );
			ValidateResult( result );
			_c.SetTimeout( _defaultTimeout );
		}
		
		/// <summary>Returns the user-defined maximum Tilt Speed. This limit can be changed and also disabled.</summary>
		public UInt16 GetTiltSpeedLimitMax() {
			
			UInt16 value = _c.GetUInt16(PTCommand.GetTiltSpeedLimitMax);
			return value;
		}
		
	#endregion
		
#endregion
		
#region Power Mode
		
		public void SetPanHoldPower(PTPower power) {
			
			Byte result = _c.GetByte( PTCommand.SetPanPowerHold, (byte)power );
			ValidateResult( result );
		}
		
		public void SetPanMovePower(PTPower power) {
			
			Byte result = _c.GetByte( PTCommand.SetPanPowerMove, (byte)power );
			ValidateResult( result );
		}
		
		public PTPower GetPanHoldPower() {
			
			Byte value = _c.GetByte( PTCommand.GetPanPowerHold );
			return (PTPower)value;
		}
		
		public PTPower GetPanMovePower() {
			
			Byte value = _c.GetByte( PTCommand.GetPanPowerMove );
			return (PTPower)value;
		}
		
		public void SetTiltHoldPower(PTPower power) {
			
			Byte result = _c.GetByte( PTCommand.SetTiltPowerHold, (byte)power );
			ValidateResult( result );
		}
		
		public void SetTiltMovePower(PTPower power) {
			
			Byte result = _c.GetByte( PTCommand.SetTiltPowerMove, (byte)power );
			ValidateResult( result );
		}
		
		public PTPower GetTiltHoldPower() {
			
			Byte value = _c.GetByte( PTCommand.GetTiltPowerHold );
			return (PTPower)value;
		}
		
		public PTPower GetTiltMovePower() {
			
			Byte value = _c.GetByte( PTCommand.GetTiltPowerMove );
			return (PTPower)value;
		}
		
#endregion
		
#region Shortcut Methods
		
		public void GetCurrentPosition(out Int16 panSteps, out Int16 tiltSteps) {
			
			panSteps  = GetPanCurrentPosition();
			tiltSteps = GetTiltCurrentPosition();
		}
		
#endregion
		
#region Position Presets and other ASCII-only Commands
		
		public void SetPositionPreset(int presetNumber) {
			
			if( presetNumber < 0 || presetNumber > 32 ) throw new ArgumentOutOfRangeException("presetNumber","value must be between 0 and 32 (inclusive).");
			
			String command = String.Format( Cult.InvariantCulture, PTAsciiCommand.SetPreset, presetNumber );
			
			String response = AsciiExchange( command, 250 );
			
			ValidateResult( response );
		}
		
		public void GotoPositionPreset(int presetNumber) {
			
			if( presetNumber < 0 || presetNumber > 32 ) throw new ArgumentOutOfRangeException("presetNumber","value must be between 0 and 32 (inclusive).");
			
			String command = String.Format( Cult.InvariantCulture, PTAsciiCommand.GoToPreset, presetNumber );
			
			String response = AsciiExchange( command, 250 );
			
			ValidateResult( response );
		}
		
		public void ClearPreset(int presetNumber) {
			
			if( presetNumber < 0 || presetNumber > 32 ) throw new ArgumentOutOfRangeException("presetNumber","value must be between 0 and 32 (inclusive).");
			
			String command = String.Format( Cult.InvariantCulture, PTAsciiCommand.ClearPreset, presetNumber );
			
			String response = AsciiExchange( command, 250 );
			
			ValidateResult( response );
		}
		
#endregion
		
	}
	
	public class PTFirmwareInfo {
		
		public static PTFirmwareInfo FromString(String firmwareString) {
			
			// C parser uses this:
			// sscanf( firmwareVersionString, "%1d %c %2d %c %2d", &modelVersion, &c1, &codeVersion, &c2, &revision);
			
			// the firmwareString looks like this:
			// Pan-Tilt Controller v2.13.4r0(D48-C14/E), (C)2009 Directed Perception, Inc,All Rights Reserved
			
			// the firmwareVersionString is this part:
			// 2.13.4r0(D48-C14/E)
			
			// so it only reads in 2 | . | 13 | 4r
			
			try {
				// get the start of the version string
				int i=0;
				while( i < firmwareString.Length && Char.ToLowerInvariant( firmwareString[i] ) != 'v' ) i++;
				i++;
				
				Regex regex = new Regex(@"(\d+)\.(\d+)\.(\d+)", RegexOptions.CultureInvariant );
				Match match = regex.Match( firmwareString, i );
				
				System.Collections.Generic.List<String> digits = new System.Collections.Generic.List<string>();
				
				while(match != Match.Empty) {
					foreach(Group g in match.Groups) {
						foreach(Capture cap in g.Captures) {
							digits.Add( cap.Value );
						}
					}
					match = match.NextMatch();
				}
				
				String tok1 = digits[1]; // [0] is the whole capture
				String tok2 = digits[2];
				String tok3 = digits[3];
				
				int vModel = Int32.Parse( tok1, Cult.InvariantCulture );
				int vCode  = Int32.Parse( tok2, Cult.InvariantCulture );
				int vRev   = Int32.Parse( tok3, Cult.InvariantCulture );
				
				return new PTFirmwareInfo( firmwareString, vModel, vCode, vRev );
				
			} catch(IndexOutOfRangeException iex) {
				
				throw new FormatException("Could not parse firmwareString.", iex);
				
			} catch(FormatException fex) {
				
				throw new FormatException("Could not parse firmwareString.", fex);
			}
			
		}
		
		internal PTFirmwareInfo(int modelVersion, int codeVersion, int revision) {
			
			ModelVersion = modelVersion;
			CodeVersion  = codeVersion;
			Revision     = revision;
		}
		
		private PTFirmwareInfo(String firmwareString, int modelVersion, int codeVersion, int revision) {
			
			FirmwareString = firmwareString;
			ModelVersion   = modelVersion;
			CodeVersion    = codeVersion;
			Revision       = revision;
		}
		
		public String FirmwareString { get; private set; }
		public Int32  ModelVersion   { get; private set; }
		public Int32  CodeVersion    { get; private set; }
		public Int32  Revision       { get; private set; }
		
		public static int Comparison(PTFirmwareInfo x, PTFirmwareInfo y) {
			
			if( x == null && y == null ) return  0;
			if( x == null && y != null ) return -1;
			if( x != null && y == null ) return  1;
			
			int retModel = x.ModelVersion.CompareTo( y.ModelVersion );
			if( retModel != 0 ) return retModel;
			
			int retCode = x.CodeVersion.CompareTo( y.CodeVersion );
			if( retCode != 0 ) return retCode;
			
			int retRev = x.Revision.CompareTo( y.Revision );
			return retRev;
		}
		
	}
	
}
