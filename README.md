# ZugNet

This is a suite of tools designed for connecting hardware devices and displays over a network to railway simulation software.
The primary network protocol used is MQTT. For this a MQTT broker is reuiqred, such as Mosqitto.

Current components are:
* SimKeypressOutput - Triggers keypresses and key holds based on MQTT Messages
* SimXKeysInput - Sends MQTT messages triggered by keypresses on an X-Keys XK-24 device. Also allows custommised control of the LED backlights on the device.

Coming soon
* Component to convert MQTT messages into joystick inputs(based on the vJoy virtal device driver(
* Component to send MQTT messages triggered by MIDI messages
* Broker to feed data from DTG Train Simulator into ZusiDisplay.
