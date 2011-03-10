#pragma once

#include <stdexcept>

#include "PTConnection.h"

using namespace std;

class PTException : public std::runtime_error {

private:
	PTResult    _result;
	std::string _message;
	
public:
	PTException(PTResult result);
	PTException(std::string message);
	~PTException();
	
	PTResult getResult() { return _result; }
	std::string getMessage() { return _message; }
};
