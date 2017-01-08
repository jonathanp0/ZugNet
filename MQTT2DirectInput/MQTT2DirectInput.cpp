#include <algorithm>
#include <iostream>
#include <string>
#include <stdio.h>

#include <windows.h>
#include <basetyps.h>
#include <public.h>
#include <vjoyinterface.h>
#include <MQTTClient.h>

#include "SimpleIni.h"

#include "JoystickMapper.h"


#include "MQTTPublisher.h"

#include <stdexcept>


void mqtt_connect(MQTTClient &client, const std::string& address);
void mqtt_disconnect(MQTTClient &client);
void delivered(void *context, MQTTClient_deliveryToken dt);
int msgarrvd(void *context, char *topicName, int topicLen, MQTTClient_message *message);
void connlost(void *context, char *cause);

static std::string s_topic("zusi/control/#");
static JoystickMapper s_mapper;

void mqtt_connect(MQTTClient &client, const std::string& address)
{
	MQTTClient_connectOptions conn_opts = MQTTClient_connectOptions_initializer;

	int rc;

	MQTTClient_create(&client, address.c_str(), "Midi2DirectInput",
		MQTTCLIENT_PERSISTENCE_NONE, NULL);
	conn_opts.keepAliveInterval = 20;
	conn_opts.cleansession = 1;

	MQTTClient_setCallbacks(client, NULL, connlost, msgarrvd, delivered);

	if ((rc = MQTTClient_connect(client, &conn_opts)) != MQTTCLIENT_SUCCESS)
	{
		throw std::invalid_argument("Failed to connect to MQTT Server");
	}

	MQTTClient_subscribe(client, "zusi/control/#", 2);
}

void mqtt_disconnect(MQTTClient &client)
{
	MQTTClient_disconnect(client, 10000);
	MQTTClient_destroy(&client);
}



void delivered(void *context, MQTTClient_deliveryToken dt)
{
	printf("Message with token value %d delivery confirmed\n", dt);
}

int msgarrvd(void *context, char *topicName, int topicLen, MQTTClient_message *message)
{
	int i;
	char* payloadptr;
	std::string spayload;
	std::string stopic(topicName);

	printf("Message arrived\n");
	printf("     topic: %s\n", topicName);
	printf("   message: ");

	payloadptr = static_cast<char*>(message->payload);
	for (i = 0; i<message->payloadlen; i++)
	{
		spayload.push_back(*payloadptr++);
	}
	MQTTClient_freeMessage(&message);
	MQTTClient_free(topicName);
	std::cout << spayload << std::endl;

	s_mapper.mapInput(stopic.substr(s_topic.size()-1), spayload);
	return 1;
}

void connlost(void *context, char *cause)
{
	printf("\nConnection lost\n");
	printf("     cause: %s\n", cause);
}

int main(int argc, char** argv)
{
	std::cout << "MQTT2DirectInput V0.1 (C) Jonathan Pilborough" << std::endl << std::endl;

	//Read command line
	std::string configFile("config.ini");
	std::string mapFile("directinput.ini");

	if (argc > 1)
		configFile.assign(argv[1]);
	if (argc > 2)
		configFile.assign(argv[2]);

	//Load Configuration
	std::string mqaddress;

	CSimpleIniA ini;
	if (ini.LoadFile(configFile.c_str()) == 0)
	{
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

	//Display vJoy Info
	if (!vJoyEnabled())
	{
		std::cout << "Cannot access joystick. Is vJoy installed?" << std::endl;
		return 1;
	}
	printf("vJoy Library: %S\nvJoy Vendor: %S\nvJoy Version: %S\n", 
		GetvJoyProductString(),
		GetvJoyManufacturerString(), 
		GetvJoySerialNumberString());

	WORD VerDll, VerDrv;
	if (!DriverMatch(&VerDll, &VerDrv))
		printf("Warning\r\nvJoy Driver (version %04x) does not match vJoyInterface DLL(version % 04x)\n", VerDrv ,VerDll);
	else
	    printf("OK - vJoy Driver and vJoyInterface DLL match(%04x)\n", VerDrv);

	s_mapper.loadMappings(mapFile);

	MQTTClient client;
	mqtt_connect(client, mqaddress);

	std::cout << "\nReading MQTT input ... press <enter> to quit.\n";
	char input;
	std::cin.get(input);

	mqtt_disconnect(client);

	return 0;
}

