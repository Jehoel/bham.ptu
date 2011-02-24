using System;
using System.Collections.Generic;
using System.Text;

namespace Bham.Ptu {
	
	public enum PTCommand : byte {
		
		// All opcodes start from 129 to stay out of the ASCII range
		// Codes go from 129 to 205 as of v1.7.7
		
		// Key:
		// bool     8-bit  (1 byte) value. Either 1 or 0.
		// int16	16-bit (2 byte) signed integer.
		// uint16	16-bit (2 byte) unsigned integer.
		// uint32	32-bit (4 byte) unsigned integer.
		
		// status	8-bit. See PTResult for possible values.
		// power	8-bit. See PTPowerMode for possible values.
		// speed    8-bit. See PTSpeedControlMode for possible values.
		
		SetPanPositionAbsolute     = 129, // status op(int16)
		SetTiltPositionAbsolute    = 130, // status op(int16)
		SetPanPositionOffset       = 131, // status op(int16)
		SetTiltPositionOffset      = 132, // status op(int16)
		
		SetPanSpeedOffset          = 133, // status op(int16)
		SetTiltSpeedOffset         = 134, // status op(int16)
		SetPanSpeedAbsolute        = 135, // status op(int16) in Pure Velocity Mode, byte op(uint16) in Independent Mode
		SetTiltSpeedAbsolute       = 136, // status op(int16) in Pure Velocity Mode, byte op(uint16) in Independent Mode
		
		SetPanBaseSpeed            = 137, // status op(uint16)
		SetTiltBaseSpeed           = 138, // status op(uint16)
		
		SetPanSpeedLimitMax        = 139, // status op(uint16)
		SetTiltSpeedLimitMax       = 140, // status op(uint16)
		SetPanSpeedLimitMin        = 141, // status op(uint16)
		SetTiltSpeedLimitMin       = 142, // status op(uint16)
		
		SetUnitId                  = 143, // status op(uint16)
		SelectUnitId               = 144, // status op(uint16)
		
		GetPanPositionCurrent      = 145, // int16 op()
		GetTiltPositionCurrent     = 146, // int16 op()
		GetPanPositionDesired      = 147, // int16 op()
		GetTiltPositionDesired     = 148, // int16 op()
		
		GetPanPositionMin          = 149, // int16 op()
		GetTiltPositionMin         = 150, // int16 op()
		GetPanPositionMax          = 151, // int16 op()
		GetTiltPositionMax         = 152, // int16 op()
		
		GetPanSpeedCurrent	       = 153, // uint16 op()
		GetTiltSpeedCurrent        = 154, // uint16 op() // NOTE: might this return a signed int in Pure Velocity mode?
		GetPanSpeedDesired	       = 155, // uint16 op()
		GetTiltSpeedDesired        = 156, // uint16 op()
		
		GetPanSpeedBase            = 157, // uint16 op()
		GetTiltSpeedBase           = 158, // uint16 op()
		
		GetPanSpeedLimitMax        = 159, // uint16 op()
		GetTiltSpeedLimitMax       = 160, // uint16 op()
		GetPanSpeedLimitMin        = 161, // uint16 op()
		GetTiltSpeedLimitMin       = 162, // uint16 op()
		
		GetPanAcceleration         = 163, // uint32 op()
		GetTiltAcceleration        = 164, // uint32 op()
		
		GetPanResolution           = 165, // uint32 op() // NOTE: Value is 60x arc secs
		GetTiltResolution          = 166, // uint32 op()
		
		AwaitCommandCompletion     = 167, // status op()
		Halt                       = 168, // status op()
		HaltPan                    = 169, // status op()
		HaltTilt                   = 170, // status op()
		
		GetPositionLimits          = 171, // bool op() // 1 if limits enabled, 0 otherwise
		SetPositionLimitsEnabled   = 172, // status op()
		SetPositionLimitsDisabled  = 173, // status op()
		
		SetCommandModeImmediate    = 174, // status op()
		SetCommandModeSlaved       = 175, // status op()
		
		UnitReset                  = 176, // async op() // See async return codes
		UnitResetOnPowerUpEnabled  = 177, // status op()
		UnitResetOnPowerUpDisabled = 178, // status op()
		
		GetUnitId                  = 179, // byte op() // Returns unit ID // NOTE: May not be supported
		
		GetEcho                    = 180, // bool op() // Returns 1 if echo, otherwise 0
		SetEchoEnabled             = 181, // status op()
		SetEchoDisabled            = 182, // status op()
		
		DefaultsSave               = 183, // status op()
		DefaultsRestore            = 184, // status op()
		DefaultsFactoryRestore     = 185, // status op()
		
		GetPanPowerHold            = 186, // power op()
		GetTiltPowerHold           = 187, // power op()
		GetPanPowerMove            = 188, // power op()
		GetTiltPowerMove           = 189, // power op()
		
		GetVerbose                 = 190, // bool op() // Returns 1 if verbose, otherwise 0 (terse)
		SetVerboseEnabled          = 191, // status op()
		SetVerboseDisabled         = 192, // status op()
		
		GetJoystick                = 193, // bool op() // Returns 1 if joystick enabled, otherwise 0
		SetJoystickEnabled         = 194, // status op()
		SetJoystickDisabled        = 195, // status op()
		
		GetFirmwareVersion         = 196, // char[] op() // Returns string terminated with '\n'
		
		SetPanPowerHold            = 197, // status op(power)
		SetTiltPowerHold           = 198, // status op(power)
		SetPanPowerMove            = 199, // status op(power)
		SetTiltPowerMove           = 200, // status op(power)
		
		SetPanAcceleration         = 201, // status op(uint32)
		SetTiltAcceleration        = 202, // status op(uint32)
		
		GetControlMode             = 203, // speed op()
		SetControlModeIndependent  = 204, // status op()
		SetControlModePureVelocity = 205, // status op()
		
		UnitResetPan               = 220, // async op() // Undocumented
		UnitResetTilt              = 221, // async op() // Undocumented
	}
	
	/// <summary>This class contains the ASCII strings for ASCII-only PTU commands that have no binary equivalents. Where possible use the binary commands because there is significantly less possibility of trouble parsing the result.</summary>
	public static class PTAsciiCommand {
		public const String GetEnvironment = "o";
		
		public const String BeginMonitor        = "m";
		public const String BeginMonitorPan     = "m{0}{1}";
		public const String BeginMonitorPanTilt = "m{0}{1}{2}{3}";
		
		public const String GetMonitorOnPowerUp         = "mq";
		public const String SetMonitorOnPowerUpEnabled  = "me";
		public const String SetMonitorOnPowerUpDisabled = "md";
		
		public const String GetPanStepMode  = "wp";
		public const String SetPanStepMode  = "wp{0}";
		public const String GetTiltStepMode = "wt";
		public const String SetTiltStepMode = "wt{0}";
		
		public const String SetPreset      = "xs{0}"; // 0 = Position index, 0 through 32
		public const String GoToPreset     = "xg{0}"; // 0 = Position index, 0 through 32
		public const String ClearPreset    = "xc{0}"; // 0 = Position index, 0 through 32
		
		public const String UnitResetTilt = "rt";
		public const String UnitResetPan  = "rp";
		
		public const String UnitSetSerialBaud  = "@({0}, {1}, {2})";
		public const String UnitEstablishChA   = "@A";
		public const String UnitEstablishChB   = "@B";
		public const String UnitDisestablishCh = "@";
		public const String UnitEstablishChABytes = "@A{0}";
		public const String UnitEstablishChBBytes = "@B{0}";
		public const String UnitEstablishChAMouse = "@A(M)";
		public const String UnitEstablishChBMouse = "@B(M)";
		
		public const String UnitSetInteractive = "U0";
		public const String UnitBroadcast      = "_0";
		
//		public const String GetPositionLimits         = "l";  // there is no way to determine if User limits are enabled or just Factory limits; just to tell if *any* limits are enabled
//		public const String SetPositionLimitsDisabled = "ld"; // use the bin version
//		public const String SetPositionLimitsFactory  = "le"; // use the bin version
		public const String SetPositionLimitsUser     = "lu";
		
		public const String GetPanUserPositionLimitMin  = "pnu";
		public const String GetPanUserPositionLimitMax  = "pxu";
		public const String GetTiltUserPositionLimitMin = "tnu";
		public const String GetTiltUserPositionLimitMax = "txu";
		
		public const String SetPanUserPositionLimitMin  = "pnu{0}"; // the CRM is wrong, the command is 'PNUx' and not 'PNx'
		public const String SetPanUserPositionLimitMax  = "pxu{0}";
		public const String SetTiltUserPositionLimitMin = "tnu{0}";
		public const String SetTiltUserPositionLimitMax = "txu{0}";
		
		// I have no idea how to properly implement the continuous rotation commands.
//		public const String SetContinuousRotation = "%%1C{0}{1}"; // 0 = Axis, either 'T' or 'P', 1 = Enable continuous access, either 'T' or 'P', whatever that means
//		public const String SetContinuousRotationPowerUpEnable = "ds";
		
		// but apparently it causes damage unless you're using the D300
		// so let's not...
		
		
	}
	
	public enum PTPower : byte {
		
		HighPower    = 1,
		RegularPower = 2,
		LowPower     = 3,
		PowerOff     = 4,
	}
	
	public enum PTResult : byte {
		
		// Synchronous return codes
		Ok                      =  0,
		IllegalCommandArgument  =  1, // NOTE: Some operations return 1 as a boolean value
		IllegalCommand          =  2,
		IllegalPositionArgument =  3,
		IllegalSpeedArgument    =  4,
		AccelTableExceeded      =  5,
		DefaultsEepromFault     =  6,
		SavedDefaultsCorrupted  =  7,
		LimitHit                =  8,
		CableDisconnected       =  9,
		IllegalUnitId           = 10,
		IllegalPowerMode        = 11,
		ResetFailed             = 12,
		NotResponding           = 13,
		FirmwareVersionTooLow   = 14,
		
		// Async return codes
		PanLimitHit             = 220, // !P
		TiltLimitHit            = 221, // !T
		CableDisconnectDetected = 222, // !D
		PanPositionTriggerHit   = 223, // #P
		TiltPositionTriggerHit  = 224, // #T
		PanSpeedTriggerHit      = 225, // $P
		TiltSpeedTriggerHit     = 226  // $T
	}
	
	public enum PTSpeedControlMode : byte {
		
		Independent  = 1,
		PureVelocity = 2
	}
	
	/// <summary>This enumeration is for convenience only. It is not used by the PT controller hardware nor is it part of the protocol.</summary>
	public enum PTCommandMode {
		Immediate,
		Slaved
	}
	
	/// <summary>This enumeration is for convenience only. It is not used by the PT controller hardware nor is it part of the protocol.</summary>
	public enum PTPositionLimitsMode {
		Disabled,
		Factory,
		User
	}
	
	public enum PTStepMode : byte {
		Full    = (byte)'F',
		Half    = (byte)'H',
		Quarter = (byte)'Q',
		Eighth  = (byte)'E',
		/// <summary>Resolution of the axis varies according to current speed. When Auto is selected all units are in Eighth Step Mode resolution.</summary>
		/// <remarks>Auto-stepping requires Firmware version 2.13.0 or greater.</remarks>
		Auto    = (byte)'A'
	}
	
	internal enum PTInternalResult : byte {
		InteractiveMainMenu           = 1,
		DisplayPanMotorCommands       = 2,
		DisplayTiltMotorCommands      = 3,
		DisplayMenuOptions            = 4,
		IllegalCommandWithDelimiter   = 5,
		AsciiCommandWithNoBinaryEquiv = 6,
		IllegalOpcode                 = 7,
		IllegalArgument               = 8
	}
	
}
