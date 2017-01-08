#pragma once

#include <map>
#include <string>
#include <set>
#include <array>

class AxisLookup
{
public:
	std::map<std::string, char> axes;

	AxisLookup();
};

//Utility class to manage Joystick lifecycle
class Joystick
{
public:
	Joystick(int id);
	virtual ~Joystick();

	int getButtonCount();
	bool hasAxis(const std::string& a);

	void setButton(int button, bool onoff);
	void setAxis(int axis, double value);

private:
	int m_id;
	int m_buttonCount;
	std::set<std::string> m_axes;
};

class JoystickMapper
{
public:
	JoystickMapper();
	virtual ~JoystickMapper();

	bool loadMappings(const std::string& filename);
	void mapInput(const std::string& topic, const std::string& payload);

private:

	struct Joy {

		Joy(char d = 0, char a = 0, char b = 0) : axis(a), button(b) {}

		char device;
		char axis;
		char button;
	};

	std::map<std::string, Joy> m_mappings;

	std::array<Joystick*, 16> m_joysticks;
};

