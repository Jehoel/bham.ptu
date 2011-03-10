#include "StdAfx.h"

#include "PTConnection.h"
#include "PTException.h"

PTConnection::PTConnection(const QString comDeviceName) {
	
	_s = new AbstractSerial();
	_s->setDeviceName(comDeviceName);
	
	_s->setBaudRate   ( AbstractSerial::BaudRate9600   );
	_s->setDataBits   ( AbstractSerial::DataBits8      );
	_s->setParity     ( AbstractSerial::ParityNone     );
	_s->setStopBits   ( AbstractSerial::StopBits1      );
	_s->setFlowControl( AbstractSerial::FlowControlOff );
}

PTConnection::~PTConnection(void) {
	
	delete _s;
}

void PTConnection::open() {
	
	if( !_s->open(AbstractSerial::ReadWrite) )
		throw new PTException("Serial port is not open");
}

void PTConnection::writeCommand(PTCommand cmd, QByteArray args) {
	
	QByteArray cmdBuffer;
	cmdBuffer.append( cmd );
	
	_s->write( cmdBuffer );
	_s->write( args );
	_s->flush();
	
}

void PTConnection::waitForData(int nofBytes, int timeout) {
	
	QTime timeStart = QTime::currentTime();
	
	while( _s->bytesAvailable() < nofBytes ) {
		
		_s->waitForReadyRead(5); // blocking wait 5ms
		
		if( timeout > 0 && timeStart.msecsTo( QTime::currentTime() ) > timeout ) {
			
			throw new PTException("waitForData, Timeout exceeded");
		}
		
	}
	
}

QByteArray PTConnection::getKnownBuffer(PTCommand cmd, int size, QByteArray args) {
	
	writeCommand(cmd, args);
	
	waitForData(size, 2000);
	
	QByteArray buffer = _s->read( size );
	
	return buffer;
}

QByteArray PTConnection::getUnknownBuffer(PTCommand cmd, int delay, QByteArray args) {
	
	writeCommand(cmd, args);
	
	if( _s->waitForReadyRead(delay) ) {
		
		waitForData(10, 2000); // wait for at least 10 bytes anyway
		
	} else
		throw new PTException("getUnknownBuffer, Timeout exceeded");
	
	int nofBytes = _s->bytesAvailable();
	
	QByteArray buffer = _s->readAll();
	return buffer;
}

#pragma region Data Accessors

char PTConnection::getByte() {
	
	waitForData(1, 0);
	
	QByteArray buffer = _s->read(1);
	return *buffer.data();
}

char PTConnection::getByte(PTCommand cmd, QByteArray args) {
	
	writeCommand(cmd, args);
	
	return getByte();
}

qint16 PTConnection::getInt16(PTCommand cmd, QByteArray args) {
	
	QByteArray buffer = getKnownBuffer(cmd, 2, args);
	
	// NOTE: The serial protocol is big-endian
	
	const uchar* p = (const uchar*)buffer.constData();
	
	qint16 value = qFromBigEndian<qint16>(p);
	
	return value;
}

quint16 PTConnection::getUInt16(PTCommand cmd, QByteArray args) {
	
	short ret = getInt16(cmd, args);
	return (ushort)ret;
}

qint32 PTConnection::getInt32(PTCommand cmd, QByteArray args) {
	
	QByteArray buffer = getKnownBuffer(cmd, 4, args);
	
	// NOTE: The serial protocol is big-endian, but this machine is little-endian
	
	const uchar* p = (const uchar*)buffer.constData();
	
	qint32 value = qFromBigEndian<qint32>(p);
	return value;
}

quint32 PTConnection::getUInt32(PTCommand cmd, QByteArray args) {
	
	int ret = getInt32(cmd, args);
	return (uint)ret;
}

qint64 PTConnection::getInt64(PTCommand cmd, QByteArray args) {
	
	QByteArray buffer = getKnownBuffer(cmd, 4, args);
	
	// NOTE: The serial protocol is big-endian, but this machine is little-endian
	
	const uchar* p = (const uchar*)buffer.constData();
	
	qint64 value = qFromBigEndian<qint64>(p);
	return value;
}

quint64 PTConnection::getUInt64(PTCommand cmd, QByteArray args) {
	
	qint64 ret = getInt64(cmd, args);
	return (quint64)ret;
}

#pragma endregion

#pragma region QByteArray-less methods

//////////////////////////////////////////////
// Char

char PTConnection::getByte(PTCommand cmd) {
	
	QByteArray args( (int)0, (char)0 );
	return this->getByte(cmd, args);
}

char PTConnection::getByte(PTCommand cmd, short param1) {
	
	QByteArray args( (int)2, (char)0 );
	args[0] = (char)(param1 >> 8); // high-order byte
	args[1] = (char)param1; // low-order byte
	
	return getByte(cmd, args);
}

char PTConnection::getByte(PTCommand cmd, short param1, short param2) {
	
	QByteArray args( (int)4, (char)0 );
	args[0] = (char)(param1 >> 8); // high-order byte
	args[1] = (char)param1; // low-order byte
	
	args[2] = (char)(param2 >> 8); // high-order byte
	args[3] = (char)param2; // low-order byte
	
	return getByte(cmd, args);
}

//////////////////////////////////////////////
// Int16

qint16 PTConnection::getInt16(PTCommand cmd) {
	
	QByteArray args( (int)0, (char)0 );
	return getInt16(cmd, args);
}

qint16 PTConnection::getInt16(PTCommand cmd, short param1) {
	
	QByteArray args( (int)0, (char)0 );
	return getInt16(cmd, args);
}



#pragma endregion