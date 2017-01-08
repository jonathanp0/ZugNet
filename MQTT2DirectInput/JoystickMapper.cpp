#include "JoystickMapper.h"

#include <map>
#include <string>
#include <iostream>

#include <windows.h>
#include <basetyps.h>
#include <public.h>
#include <vjoyinterface.h>

#include "SimpleIni.h"

AxisLookup::AxisLookup()
{
	axes["x"] = HID_USAGE_X;
	axes["y"] = HID_USAGE_Y;
	axes["z"] = HID_USAGE_Z;
	axes["rx"] = HID_USAGE_RX;
	axes["ry"] = HID_USAGE_RY;
	axes["rz"] = HID_USAGE_RY;
	axes["s0"] = HID_USAGE_SL0;
	axes["s1"] = HID_USAGE_SL1;
	axes["w"] = HID_USAGE_WHL;

}

static AxisLookup sAxisLookup;

Joystick::Joystick(int id) : m_id(id), m_buttonCount(0)
{
	VjdStat status = GetVJDStatus(id);
	switch (status)
	{
	case VJD_STAT_OWN:
		printf("vJoy Device %d is already owned by this feeder\n", id);
		throw std::invalid_argument("vJoy Device is already owned by this feeder");
		break;
	case VJD_STAT_FREE:
		if (!AcquireVJD(id))
			throw std::invalid_argument("vJoy Device cannot be acquired");
		break;
	case VJD_STAT_BUSY:
		printf("vJoy Device %d is already owned by another feeder\nCannot continue\n", id);
		throw std::invalid_argument("vJoy Device is already owned by another feeder");
		break;
	case VJD_STAT_MISS:
		printf("vJoy Device %d is not installed or disabled\nCannot continue\n", id);
		throw std::invalid_argument("vJoy Device is not installed or is disabled");
		break;
	default:
		printf("vJoy Device %d general error\nCannot continue\n", id);
		throw std::invalid_argument("vJoy Device general error");
		break;
	};

	ResetVJD(m_id);

	if (GetVJDButtonNumber(m_id) > 0)
		m_buttonCount = GetVJDButtonNumber(m_id);

	for (const auto& axis : sAxisLookup.axes)
	{
		if (GetVJDAxisExist(m_id, axis.second))
			m_axes.insert(axis.first);
	}
}

Joystick::~Joystick()
{
	if (GetVJDStatus(m_id) == VJD_STAT_OWN)
	{
		RelinquishVJD(m_id);
	}
}

int Joystick::getButtonCount()
{
	return m_buttonCount;
}

bool Joystick::hasAxis(const std::string& a)
{
	return m_axes.count(a) > 0;
}

void Joystick::setButton(int button, bool onoff)
{
	if (button > 0 && button <= getButtonCount())
		SetBtn(onoff, m_id, button);
}

void Joystick::setAxis(int axis, double value)
{
	int axisvalue = static_cast<int>(value * 32767)+1;
	if (axisvalue > 0 && axisvalue <= 0x8000)
		SetAxis(axisvalue, m_id, axis);
}

JoystickMapper::JoystickMapper()
{
	m_joysticks.fill(nullptr);
}


JoystickMapper::~JoystickMapper()
{
	for (auto j : m_joysticks)
		if (j != nullptr)
			delete j;
}


bool JoystickMapper::loadMappings(const std::string& filename)
{
	CSimpleIniCaseA ini;
	if (ini.LoadFile(filename.c_str()) < 0)
	{
		std::cout << "Cannot load mapping file " << filename << std::endl;
		return false;
	}

	CSimpleIniCaseA::TNamesDepend sections;
	ini.GetAllSections(sections);

	std::cout << "Controls in use:" << std::endl;
	for (const auto& section : sections)
	{
		std::string s(section.pItem);
		std::cout << s << std::endl;

		Joy map;

		//Set up the device
		int dev = ini.GetLongValue(section.pItem, "device", 1);
		if (dev > 16)
		{
			std::cout << "Ignoring" << section.pItem << " - invalid device number " << std::endl;
			continue;
		}
		map.device = dev-1;
		if (m_joysticks[map.device] == nullptr)
			m_joysticks[map.device] = new Joystick(dev);

		//Process the mapping
		const char* topic = ini.GetValue(section.pItem, "topic");
		std::string tstring;
		if (topic != nullptr)
			tstring = topic;
		else
		{
			std::cout << "Ignoring" << section.pItem << " - no topic" << std::endl;
			continue;
		}

		const char* axis = ini.GetValue(section.pItem, "axis");
		if (axis != nullptr)
		{
			std::string astring(axis);
			if (m_joysticks[map.device]->hasAxis(astring))
			{
				map.axis = sAxisLookup.axes[astring];
			}
			else
			{
				std::cout << "Device does not support axis " << astring << std::endl;
			}
		}

		map.button = static_cast<char>(ini.GetLongValue(section.pItem, "button"));
		if (m_joysticks[map.device]->getButtonCount() <= map.button)
		{
			std::cout << "Device does not support button" << map.button << std::endl;
			map.button = 0;
		}

		m_mappings[tstring] = map;
	}

	return true;
}

void JoystickMapper::mapInput(const std::string& topic, const std::string& payload)
{
	auto it = m_mappings.find(topic);
	double num = atof(payload.c_str());
	if (it != m_mappings.end())
	{
		if (it->second.axis > 0)
			m_joysticks[0]->setAxis(it->second.axis, num);

		if (it->second.button > 0)
			m_joysticks[0]->setButton(it->second.button, num > 0.0);
	}
}

