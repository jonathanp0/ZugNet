#include "mapping.h"

#include "SimpleIni.h"

#include <cmath>
#include <iostream>
#include <utility>
#include <sstream>

Mapping::Mapping() : m_publisher(nullptr)
{
}

Mapping::Mapping(MQTTPublisher* p) : m_publisher(p)
{
}

bool Mapping::loadMappings(const std::string& filename)
{
	CSimpleIniCaseA ini;
	if (ini.LoadFile(filename.c_str()) < 0)
	{
		std::cout << "Cannot load midi mapping file " << filename << std::endl;
		return false;
	}

	CSimpleIniCaseA::TNamesDepend sections;
	ini.GetAllSections(sections);

	std::cout << "Controls in use:" << std::endl;
	for (const auto& section : sections)
	{
		std::string s(section.pItem);
		std::cout << s << std::endl;
		int ch = ini.GetLongValue(section.pItem, "channel", 1);
		int cc = ini.GetLongValue(section.pItem, "control", 1);
		std::string topic(ini.GetValue(section.pItem, "topic"));

		std::pair<unsigned int, unsigned int> p = std::make_pair(ch, cc);
		controlMap[p].push_back(topic);
	}

    return true;
}


void Mapping::mapInput(unsigned char channel, unsigned char control, unsigned char value)
{
	float outvalue = mapValue(value, 0.0f, 1.0f);
	
	auto it = controlMap.find(std::make_pair(channel, control));
	if (it != controlMap.end())
	{
		for (const auto& topic : it->second)
		{
			publishMessage(topic, outvalue);
		}
	}

}

void Mapping::publishMessage(const std::string& topic, float value)
{
	std::cout << topic << "     " << value << std::endl;
	std::string fulltopic("zusi/control/" + topic);
	std::ostringstream payload;
	payload << value;
	if (m_publisher)
		m_publisher->publish(fulltopic, payload.str());
}

float Mapping::mapValue(unsigned char input, float min, float max)
{
	float diff = max - min;
	min = fabs(min);
	float fpinput = static_cast<float>(input);
	float out = ((fpinput / 127.0f)*diff) - min;
	return out;
}

