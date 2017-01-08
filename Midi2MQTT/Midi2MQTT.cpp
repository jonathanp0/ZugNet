// Midi2MQTT.cpp : Defines the entry point for the console application.
//

#include <algorithm>
#include <iostream>
#include <string>

#include <MQTTClient.h>

#include "SimpleIni.h"
#include "midiworker.h"
#include "MQTTPublisher.h"

int main(int argc, char** argv)
{
	std::cout << "Midi2MQTT V0.1" << std::endl;
	
	//Load Configuration
	unsigned int port = 0;
	std::string mqaddress;

	CSimpleIniA ini;
	if (ini.LoadFile("config.ini")==0)
	{
		port = max(ini.GetLongValue("Midi", "port", 0),0);
		mqaddress = std::string(ini.GetValue("Mqtt", "address", "tcp://127.0.0.1:1883"));
	}
	else
	{
		std::cout << "Error: Can't find config.ini" << std::endl;
		return 1;
	}
	
	//Display Paho version
	MQTTClient_nameValue* version = MQTTClient_getVersionInfo();
	while (version->name)
	{
		std::cout << "MQTT " << version->name << ": " << version->value << std::endl;
		++version;
	}

	//Set things up
	MQTTPublisher publisher(mqaddress);
	publisher.publish("zusi/log", "Midi2MQTT Connected");

	Mapping mapper(&publisher);
	mapper.loadMappings("midi.ini");
	MidiWorker *w = new MidiWorker(&mapper, port);


	//Run the whole thing
	std::thread midiProcThread(MidiWorkerRunner, w);
	std::cout << "\nReading MIDI input ... press <enter> to quit.\n";
	char input;
	std::cin.get(input);

	w->exit();
	if (midiProcThread.joinable())
		midiProcThread.join();

	delete w;

	publisher.publish("zusi/log", "Midi2MQTT Disconnecting");
	
	return 0;
}

