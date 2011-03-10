#pragma once

#include <sdl.h>
#include "PTUnit.h"

class SdlUser {
	
public:
	SdlUser(PTUnit* unit);
	~SdlUser();
	
	void init();
	void end();
	
	void doJoysticks();
	
	PTUnit* unit;
};
