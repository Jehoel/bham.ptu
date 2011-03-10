#pragma once

#include "PTConnection.h"
#include "PTException.h"

class PTUnit {
private:
	
	PTConnection*      _c;
	PTSpeedControlMode _speedMode;
	
	double _panRes; // Pan resolution
	double _tilRes; // Tilt resolution
	
public:
	
	PTUnit(const QString comDeviceName);
	~PTUnit();
	
	// Not implemented:
	// * Monitor
	// * Position presets
	// * Continuous rotation
	// * User-defined pan/tilt limits
	// * Default Save/Restore
	// * Echo, Verbose
	// * Power modes
	// * Networking
	// * EIO
	
	void    reset();
	void    setExecutionMode();
	
	void    setPositionLimitsMode(LimitsMode value);
	
	QString getFirmwareInfo();
	QString getEnvironment();
	
	void validateResult(char result);
	
	//////////////////////////////////////////////
	
	void          setExecutionMode(ExecutionMode mode);
	ExecutionMode getExecutionMode();
	
	/// <summary>Blocks until the PT controller reports all operations have completed.</summary>
	void waitCompletion();
	void halt();
	
	//////////////////////////////////////////////
	
	void setDesiredPositionAbs(Axis axis, short position);
	void setDesiredPositionAbs(short pan, short tilt);
	
	short getCurrentPositionAbs(Axis axis);
	
	void setDesiredPositionRel(Axis axis, short position);
	void setDesiredPositionRel(short pan, short tilt);
	
	short getCurrentPositionRel(Axis axis);
	
	//////////////////////////////////////////////
	
	double getResolution(Axis axis);
	
	//////////////////////////////////////////////
	
	short getBoundsMin(Axis axis);
	short getBoundsMax(Axis axis);
	
	//////////////////////////////////////////////
	
	PTSpeedControlMode getSpeedMode();
	void               setSpeedMode(PTSpeedControlMode value);
	
	//////////////////////////////////////////////
	
	short getDesiredSpeedAbs(Axis axis);
	void  setDesiredSpeedAbs(Axis axis, short speed);
	void  setDesiredSpeedAbs(short panSpeed, short tiltSpeed);
	
	short getDesiredSpeedRel(Axis axis);
	void  setDesiredSpeedRel(Axis axis, short deltaSpeed);
	void  setDesiredSpeedRel(short deltaPanSpeed, short deltaTiltSpeed);
	
	short getSpeedBoundsMin(Axis axis);
	short getSpeedBoundsMax(Axis axis);
	
	
	
};
