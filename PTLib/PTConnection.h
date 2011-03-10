#pragma once

#include <stdarg.h>
#include <QtCore/QByteArray>
#include <QtCore/QtEndian>

#include <abstractserial.h>

enum Axis {
	Pan,
	Tilt
};

enum Axes {
	PanAxis,
	TiltAxis,
	Both
};

enum PositionType {
	Absolute,
	Relative
};

enum LimitsMode {
	On,
	Off
};

enum ExecutionMode {
	/// <summary>Instructs pan-tilt unit to immediately execute positional commands. This is the default mode.</summary>
	Immediate,
	/// <summary>Instructs pan-tilt unit to execute positional commands only when an Await Position Command Completion command is executed (see Section 4.8) or when put into Immediate Execution Mode (see Section 4.6). This mode is useful when coordinated pan and tilt axis movements are desired.</summary>
	Await
};

enum PTCommand {
	
	GetPanCurrentPos           = 145,
	GetPanDesiredPos           = 147,
	SetPanPosAbs               = 129,
	SetPanPosRel               = 131,
	
	GetTiltCurrentPos          = 146,
	GetTiltDesiredPos          = 148,
	SetTiltPosAbs              = 130,
	SetTiltPosRel              = 132,
	
	GetPanCurrentSpeed	       = 153,
	GetPanDesiredSpeed	       = 155,
	GetPanBaseSpeed            = 157,
	GetPanUpperSpeedLimit      = 159,
	GetPanLowerSpeedLimit      = 161,
	SetPanSpeedAbs             = 135,
	
	GetTiltCurrentSpeed        = 154,
	GetTiltDesiredSpeed        = 156,
	GetTiltBaseSpeed           = 158,
	GetTiltUpperSpeedLimit     = 160,
	GetTiltLowerSpeedLimit     = 162,
	SetTiltSpeedAbs            = 136,
	
	GetPanAccel                = 163,
	GetTiltAccel               = 164,
	
	SetPanBaseSpeed            = 137,
	SetTiltBaseSpeed           = 138,
	
	SetPanSpeedLimitUpper      = 139,
	SetTiltSpeedLimitUpper     = 140,
	SetPanSpeedLimitLower      = 141,
	SetTiltSpeedLimitLower     = 142,
	
	GetPanMinPosition          = 149,
	GetTiltMinPosition         = 150,
	GetPanMaxPosition          = 151,
	GetTiltMaxPosition         = 152,
	GetPanResolution           = 165,
	GetTiltResolution          = 166,
	
	AwaitCommandCompletion     = 167,
	Halt                       = 168,
	HaltPan                    = 169,
	HaltTilt                   = 170,
	UnitReset                  = 176,
	
	GetPanHoldPower            = 186,
	GetTiltHoldPower           = 187,
	GetPanMovePower            = 188,
	GetTiltMovePower           = 189,
	GetFirmwareVersion         = 196,
	GetFirmwareVersionA        = 'v', // ASCII version
	GetEnvironmentA            = 'o', // ASCII version
	
	SetPanAccel                = 201,
	SetTiltAccel               = 202,
	
	SetUnitId                  = 143,
	SelectUnitId               = 144,
	
	GetPositionLimits          = 171,
	EnablePositionLimits       = 172,
	DisablePositionLimits      = 173,
	
	SetImmediateCommandMode    = 174,
	SetSlavedCommandMode       = 175,
	
	SaveDefaults               = 183,
	RestoreSavedDefaults       = 184,
	RestoreFactoryDefaults     = 185,
	
	GetSpeedControlMode        = 203,
	SetIndependentControlMode  = 204,
	SetPureVelocityControlMode = 205,
	
	UnknownPan                 = 133,
	UnknownTilt                = 134
};

enum PTPower {
	
	HighPower    = 1,
	RegularPower = 2,
	LowPower     = 3,
	PowerOff     = 4,
};

enum PTResult {
	
	OK                      =  0,
	IllegalCommandArgument  =  1,
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
	FirmwareVersionTooLow   = 14
};

enum PTAsyncReturnCode {
	
	PanLimitHit             = 220, // !P
	TiltLimitHit            = 221, // !T
	CableDisconnectDetected = 222, // !D
	PanPositionTriggerHit   = 223, // #P
	TiltPositionTriggerHit  = 224, // #T
	PanSpeedTriggerHit      = 225, // $P
	TiltSpeedTriggerHit     = 226  // $T
};

enum PTSpeedControlMode {
	
	Independent  = 1,
	PureVelocity = 2
};

class PTConnection {

private:
	
	AbstractSerial* _s;
	
	void writeCommand(PTCommand cmd, QByteArray args);
	void waitForData(int nofBytes, int timeout);
	
public:
	PTConnection(const QString comDeviceName);
	~PTConnection(void);
	
	void open();
	
	/// <summary>Blocks for 'delay' miliseconds and returns whatever was received by the serial port in that time.</summary>
	QByteArray getUnknownBuffer(PTCommand cmd, int delay, QByteArray args);
	/// <summary>Blocks until the specified buffer is filled, then returns.</summary>
	QByteArray getKnownBuffer(PTCommand cmd, int bufferSize, QByteArray args);
	
	/// <summary>Blocks until a single byte is recieved, and returns that.</summary>
	char getByte();
	char getByte(PTCommand cmd, QByteArray args);
	
	quint16 getUInt16(PTCommand cmd, QByteArray args);
	qint16  getInt16 (PTCommand cmd, QByteArray args);
	
	quint32 getUInt32(PTCommand cmd, QByteArray args);
	qint32  getInt32 (PTCommand cmd, QByteArray args);
	
	quint64 getUInt64(PTCommand cmd, QByteArray args);
	qint64  getInt64 (PTCommand cmd, QByteArray args);
	
	/////////////////////////////////////////////////////
	
	char getByte(PTCommand cmd);
	char getByte(PTCommand cmd, short param1);
	char getByte(PTCommand cmd, short param1, short param2);
	
	qint16 getInt16(PTCommand cmd);
	qint16 getInt16(PTCommand cmd, short param1);
	qint16 getInt16(PTCommand cmd, short param1, short param2);
	
	quint16 getUInt16(PTCommand cmd);
	quint16 getUInt16(PTCommand cmd, short param1);
	quint16 getUInt16(PTCommand cmd, short param1, short param2);
	
	quint32 getUInt32(PTCommand cmd);
	quint32 getUInt32(PTCommand cmd, short param1);
	quint32 getUInt32(PTCommand cmd, short param1, short param2);
	
};
