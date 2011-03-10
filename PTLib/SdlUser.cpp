#include "StdAfx.h"
#include "SdlUser.h"

SdlUser::SdlUser(PTUnit* unit) {
	
	this->unit = unit;
}

SdlUser::~SdlUser() {
	
}

void SdlUser::init() {
	
	int res = SDL_Init( SDL_INIT_VIDEO | SDL_INIT_JOYSTICK ); // apparently you need to init video to get events
	if( res != 0 ) {
//		char* message = SDL_GetError();
//		throw new PTException("Couldn't not init SDL: " + message);
	}
}

void SdlUser::end() {
	
	SDL_Quit();
}

void SdlUser::doJoysticks() {
	
	int nofSticks = SDL_NumJoysticks();
	for(int i=0;i<nofSticks;i++) {
		
		const char* name = SDL_JoystickName(i);
		printf("%s\n", name );
	}
	
	// open the first one
	
	SDL_Joystick* stick1;
	SDL_JoystickEventState(SDL_ENABLE);
	stick1 = SDL_JoystickOpen(0);
	
	// Get range of motion
	short panMin = this->unit->getBoundsMin( Axis::Pan );
	short panMax = this->unit->getBoundsMax( Axis::Pan );
	short tilMin = this->unit->getBoundsMin( Axis::Tilt );
	short tilMax = this->unit->getBoundsMax( Axis::Tilt );
	
	// Get range of speeds
	short panMinS = this->unit->getSpeedBoundsMin(Axis::Pan);
	short panMaxS = this->unit->getSpeedBoundsMax(Axis::Pan);
	short tilMinS = this->unit->getSpeedBoundsMin(Axis::Tilt);
	short tilMaxS = this->unit->getSpeedBoundsMax(Axis::Tilt);
	
	// Event loop
	
	this->unit->setSpeedMode(PTSpeedControlMode::PureVelocity);
	
	SDL_Event sevent;
	while(SDL_WaitEvent(&sevent)) {
		
		switch(sevent.type) {
			case SDL_KEYDOWN:
				break;
			case SDL_QUIT:
				return;
			case SDL_JOYAXISMOTION:  /* Handle Joystick Motion */
				
				if ( sevent.jaxis.value < -255 || sevent.jaxis.value > 255 ) { // is out of 512 deadzone
					
					printf("Axis: %d, Value: %d\n", sevent.jaxis.axis, sevent.jaxis.value);
					
					if( sevent.jaxis.axis == 0 ) {
						
						// convert from axis range to PTU speed range
						double perc = (double)sevent.jaxis.value / (double)32768; // will be from -1 to +1
						short  pan  = (short)(perc * (double)panMaxS);
//						pan = max( pan, panMinS );
						this->unit->setDesiredSpeedAbs(Axis::Pan, pan );
						
						
						
					} else if( sevent.jaxis.axis == 1 ) {
						

						// convert from axis range to PTU speed range
						double perc = (double)sevent.jaxis.value / (double)32768; // will be from -1 to +1
						short  tilt = (short)(perc * (double)tilMaxS);
//						tilt = max( tilt, tilMinS );
						this->unit->setDesiredSpeedAbs(Axis::Tilt, tilt );
						
					}
					
				} else { // is in deadzone
					
					printf("Axis: %d, Zeroed\n", sevent.jaxis.axis);
					
					if( sevent.jaxis.axis == 0 ) {
						
						this->unit->setDesiredSpeedAbs( Axis::Pan, 0 );
						
					} else if( sevent.jaxis.axis == 1 ) {
						
						this->unit->setDesiredSpeedAbs( Axis::Tilt, 0 );
					}
					
				}
				break;
			case SDL_JOYHATMOTION:
				
				printf("Hat: %d, Position: %d\n", sevent.jhat.hat, sevent.jhat.value);
				
				break;
			case SDL_JOYBUTTONDOWN:
				
				printf("Button: %d Down\n", sevent.jbutton.button);
				
				break;
			case SDL_JOYBUTTONUP:
				
				printf("Button: %d Up\n", sevent.jbutton.button);
				
				break;
		}//switch
		
	}//while
	
	this->unit->setSpeedMode(PTSpeedControlMode::PureVelocity);
	
}