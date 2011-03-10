#include "StdAfx.h"
#include "PTException.h"


PTException::PTException(PTResult result) : runtime_error("PTException") {
	
	_result = result;
}

PTException::PTException(std::string message) : runtime_error("PTException") {
	
	_message = message;
}

PTException::~PTException() {
	
}
