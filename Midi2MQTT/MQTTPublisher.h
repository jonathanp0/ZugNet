#pragma once

#include <MQTTClient.h>
#include <string>

class MQTTPublisher
{
public:
	MQTTPublisher(const std::string& address);
	virtual ~MQTTPublisher();

	void publish(const std::string& topic, std::string payload);

private:

	MQTTClient m_client;
};

//Redundant callbacks
void delivered(void *context, MQTTClient_deliveryToken dt);
int msgarrvd(void *context, char *topicName, int topicLen, MQTTClient_message *message);
void connlost(void *context, char *cause);

