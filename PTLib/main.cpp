#include "stdafx.h"
#include <QtCore/QCoreApplication>
#include <QtCore/QStringList>
#include <QtCore/QTextStream>
#include <iostream>

#include <abstractserial.h>
#include <serialdevicewatcher.h>

#include "PTUnit.h"
#include "PTException.h"

#include "SdlUSer.h"

using namespace std;

void moveTo(PTUnit* unit, short pan, short tilt);
void printPosition(PTUnit* unit);

int main(int argc, char *argv[]) {
	
	QCoreApplication a(argc, argv);
	
	///////////////////////////////////////
	// Enumerate the COM ports
	
	SerialDeviceWatcher watcher;
	QStringList devices = watcher.devicesAvailable();
	
	cout << "Select the COM port to use:" << endl;
	for(int i=0;i<devices.size();i++) {
		cout << "[" << i << "] " <<  devices.at(i).toLocal8Bit().constData() << endl;
	}
	
	cout << "Enter device index:" << endl;
	int idx = -1;
	cin >> idx;
	
	QString deviceName = devices.at( idx );
	
	try {
	
		PTUnit* unit = new PTUnit( deviceName );
		
//		cout << "Resetting..." << endl;
//		unit->reset();
		
		cout << "Firmware:" << endl;
		QString firmware = unit->getFirmwareInfo();
		cout << firmware.toStdString() << endl;
		
		cout << "Environment:" << endl;
		QString env = unit->getEnvironment();
		cout << env.toStdString() << endl;
		
		short panMin, panMax, tiltMin, tiltMax;
		panMin  = unit->getBoundsMin(Axis::Pan);
		panMax  = unit->getBoundsMax(Axis::Pan);
		tiltMin = unit->getBoundsMin(Axis::Tilt);
		tiltMax = unit->getBoundsMax(Axis::Tilt);
		
		cout << "Bounds: Pan(" << panMin << "," << panMax << ") Tilt(" << tiltMin << "," << tiltMax << ")" << endl;
		
		short panMinSpeed, panMaxSpeed, tiltMinSpeed, tiltMaxSpeed;
		panMinSpeed  = unit->getSpeedBoundsMin(Axis::Pan);
		panMaxSpeed  = unit->getSpeedBoundsMax(Axis::Pan);
		tiltMinSpeed = unit->getSpeedBoundsMin(Axis::Tilt);
		tiltMaxSpeed = unit->getSpeedBoundsMax(Axis::Tilt);
		
		cout << "Speed Bounds: Pan(" << panMinSpeed << "," << panMaxSpeed << ") Tilt(" << tiltMinSpeed << "," << tiltMaxSpeed << ")" << endl;
		
		unit->setDesiredSpeedAbs( Axis::Pan, 0.75 * panMaxSpeed );
		unit->setDesiredSpeedAbs( Axis::Tilt, 0.75 * tiltMaxSpeed );
		
//		moveTo(unit, 0, 0);
//		moveTo(unit, panMin, tiltMin);
//		moveTo(unit, panMax, tiltMax);
//		moveTo(unit, 0, 0);
		
		printPosition(unit);
		
		SdlUser user(unit);
		user.init();
		user.doJoysticks();
		user.end();
		
	} catch(PTException& ex) {
		
		cout << "PTException caught:" << endl;
		cout << ex.what() << endl;
	}
	
	return a.exec();
}

void moveTo(PTUnit* unit, short pan, short tilt) {
	
	cout << "Moving to (" << pan << "," << tilt << ") ..." << endl;
	unit->setDesiredPositionAbs(pan, tilt);
	
	cout << "Waiting..." << endl;
	unit->waitCompletion();
}

void printPosition(PTUnit* unit) {
	
	qint16 currentPositionPan  = unit->getCurrentPositionAbs(Axis::Pan);
	qint16 currentPositionTilt = unit->getCurrentPositionAbs(Axis::Tilt);
	
	cout << "Current position: " << currentPositionPan << "," << currentPositionTilt << endl;
}