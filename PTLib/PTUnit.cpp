#include "StdAfx.h"
#include "PTUnit.h"

PTUnit::PTUnit(const QString comDeviceName) {
	
	_c = new PTConnection(comDeviceName);
	_c->open();
	
	// send ASCII 'v ' first, it seems to knock it out of ASCII input mode if a half-entered command got in there first
	
	QByteArray argsBuffer( (int)1, (char)' ' );
	QByteArray ret = _c->getUnknownBuffer(PTCommand::GetFirmwareVersionA, 500, argsBuffer);
}

PTUnit::~PTUnit(void) {
	
	delete _c;
}

#pragma region Misc and Meta

QString PTUnit::getFirmwareInfo() {
	
	QByteArray argsBuffer( (int)0, (char)0 );
	QByteArray ret = _c->getUnknownBuffer(PTCommand::GetFirmwareVersion, 500, argsBuffer);
	ret = ret.trimmed();
	
	return QString( ret );
}

QString PTUnit::getEnvironment() {
	
	// use the ASCII command version instead
	
	QByteArray argsBuffer( (int)1, (char)' ' );
	QByteArray ret = _c->getUnknownBuffer(PTCommand::GetEnvironmentA, 2500, argsBuffer);
	ret = ret.mid(4).trimmed(); // first 5 characters are 'o * '
	
	return QString( ret );
}

void PTUnit::reset() {
	
	QByteArray args( (int)0, (char)0 );
	char result = _c->getByte(PTCommand::UnitReset, args );
	while(result == (char)PTAsyncReturnCode::PanLimitHit || result == (char)PTAsyncReturnCode::TiltLimitHit) {
		
		result = _c->getByte();
	}
	
}

void PTUnit::validateResult(char result) {
	
	while(result >= PTAsyncReturnCode::PanLimitHit) {
		
		result = _c->getByte();
	}
	
	if(result != PTResult::OK) {
		
		PTResult ptresult = (PTResult)result;
		
		throw PTException(ptresult);
	}
	
}

void PTUnit::waitCompletion() {
	
	char result = _c->getByte( PTCommand::AwaitCommandCompletion );
	
	validateResult( result );
}

void PTUnit::setSpeedMode(PTSpeedControlMode value) {
	
	PTCommand cmd = (value == PTSpeedControlMode::Independent) ? PTCommand::SetIndependentControlMode : PTCommand::SetPureVelocityControlMode;
	
	char result = _c->getByte( cmd );
	
	validateResult( result );
}

#pragma endregion
#pragma region Speed and Position

void PTUnit::setDesiredPositionAbs(Axis axis, short position) {
	
	PTCommand cmd = (axis == Axis::Pan) ? SetPanPosAbs : SetTiltPosAbs;
	
	char result = _c->getByte(cmd, position);
	
	validateResult( result );
}

void PTUnit::setDesiredPositionAbs(short pan, short tilt) {
	
	setDesiredPositionAbs(Axis::Pan, pan);
	setDesiredPositionAbs(Axis::Tilt, tilt);
}

short PTUnit::getCurrentPositionAbs(Axis axis) {
	
	PTCommand cmd = (axis == Axis::Pan) ? GetPanCurrentPos : GetTiltCurrentPos;
	
	qint16 result = _c->getInt16(cmd);
	return result;
}

void PTUnit::setDesiredSpeedAbs(Axis axis, short speed) {
	
	PTCommand cmd = (axis == Axis::Pan) ? SetPanSpeedAbs : SetTiltSpeedAbs;
	
	char result = _c->getByte(cmd, speed);
	
	validateResult( result );
}

#pragma endregion
#pragma region Bounds

short PTUnit::getBoundsMin(Axis axis) {
	
	PTCommand cmd = (axis == Axis::Pan) ? GetPanMinPosition : GetTiltMinPosition;
	
	qint16 result = _c->getInt16(cmd);
	return result;
}

short PTUnit::getBoundsMax(Axis axis) {
	
	PTCommand cmd = (axis == Axis::Pan) ? GetPanMaxPosition : GetTiltMaxPosition;
	
	qint16 result = _c->getInt16(cmd);
	return result;
}

short PTUnit::getSpeedBoundsMax(Axis axis) {
	
	PTCommand cmd = (axis == Axis::Pan) ? GetPanUpperSpeedLimit : GetTiltUpperSpeedLimit;
	
	qint16 result = _c->getInt16(cmd);
	return result;
}

short PTUnit::getSpeedBoundsMin(Axis axis) {
	
	PTCommand cmd = (axis == Axis::Pan) ? GetPanLowerSpeedLimit : GetTiltLowerSpeedLimit;
	
	qint16 result = _c->getInt16(cmd);
	return result;
}

#pragma endregion