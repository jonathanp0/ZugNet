#include "MQTTPublisher.h"

#include <stdexcept>

MQTTPublisher::MQTTPublisher(const std::string& address)
{
	MQTTClient_connectOptions conn_opts = MQTTClient_connectOptions_initializer;

	int rc;

	MQTTClient_create(&m_client, address.c_str(), "Midi2MQTT",
		MQTTCLIENT_PERSISTENCE_NONE, NULL);
	conn_opts.keepAliveInterval = 20;
	conn_opts.cleansession = 1;

	MQTTClient_setCallbacks(m_client, NULL, connlost, msgarrvd, delivered);

	if ((rc = MQTTClient_connect(m_client, &conn_opts)) != MQTTCLIENT_SUCCESS)
	{
		throw std::invalid_argument("Failed to connect to MQTT Server");
	}
}

MQTTPublisher::~MQTTPublisher()
{
	MQTTClient_disconnect(m_client, 10000);
	MQTTClient_destroy(&m_client);
}

void MQTTPublisher::publish(const std::string& topic, std::string payload)
{
	MQTTClient_message pubmsg = MQTTClient_message_initializer;

	pubmsg.payload = const_cast<char*>(payload.c_str());
	pubmsg.payloadlen = payload.length();
	pubmsg.qos = 0;
	pubmsg.retained = 0;

	MQTTClient_publishMessage(m_client, topic.c_str(), &pubmsg, nullptr);

}

void delivered(void *context, MQTTClient_deliveryToken dt)
{
	printf("Message with token value %d delivery confirmed\n", dt);
}

int msgarrvd(void *context, char *topicName, int topicLen, MQTTClient_message *message)
{
	int i;
	char* payloadptr;

	printf("Message arrived\n");
	printf("     topic: %s\n", topicName);
	printf("   message: ");

	payloadptr = static_cast<char*>(message->payload);
	for (i = 0; i<message->payloadlen; i++)
	{
		putchar(*payloadptr++);
	}
	putchar('\n');
	MQTTClient_freeMessage(&message);
	MQTTClient_free(topicName);
	return 1;
}

void connlost(void *context, char *cause)
{
	printf("\nConnection lost\n");
	printf("     cause: %s\n", cause);
}