#ifndef MAPPING_H
#define MAPPING_H

#include <string>
#include <utility>
#include <vector>
#include <map>

#include "MQTTPublisher.h"

class Mapping
{

public:
    Mapping();
	Mapping(MQTTPublisher* p);

    enum MappingType
    {
        Normal = 0,
        Toggle,
        Reverse
    };

    struct AxisMap
    {
        std::string topic;
        MappingType type;
        float min;
        float max;
    };


    bool loadMappings(const std::string& filename);
	void mapInput(unsigned char channel, unsigned char control, unsigned char value);

private:
    typedef std::map<std::pair<unsigned int, unsigned int>, AxisMap> ControlMap;
    //ControlMap mappings;

	void publishMessage(const std::string& topic, float value);
	float mapValue(unsigned char input, float min, float max);

	MQTTPublisher* m_publisher;
	std::map<std::pair<unsigned int, unsigned int>, std::vector<std::string>> controlMap;
};

#endif // MAPPING_H
