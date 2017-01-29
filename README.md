# ZugNet
ZugNet is a suite of tools designed for connecting hardware devices and displays over a network to railway simulation software.
The primary network protocol used is MQTT. For this a MQTT broker is requred, such as Mosqitto.
They are designed to allow flexibility when using several computers to power the simulator, and also to support multiple different types of simulation software.

Current components are:
* SimKeypressOutput - Triggers keypresses and key holds based on MQTT Messages
* SimXKeysInput - Sends MQTT messages triggered by keypresses on an X-Keys XK-24 device. Also allows custommised control of the LED backlights on the device.
* MQTT2DirectInput - Converts MQTT messages into joystick inputs(based on the vJoy virtal device driver)
* Midi2MQTT - Converts MIDI messages from an attached USB MIDI device(e.g. a "mixer" or other controller-style device) into MQTT messages

## User Guide
### XKeysInput and KeypressOutput
These tools are configured using a combined "profile.xml" and there is an example in the ProfileXML folder.
A ProfileXML file contains multiple "profiles", for example for different simulators, and only one is active at a time. A profile contains settings for each program.
The ProfileXML can be loaded and a profile selected with the GUI. Alternatively the file and profile names can be given as the first and second arguments of the program.

Example for XKeysInput section:
```
<Key ID="1">
     <type>normal</type>
     <topic>zusi/control/handbrake/aus</topic>
     <on><blue>off</blue><red>on</red></on>
     <off><blue>off</blue><red>off</red></off>
</Key>
```
* ID is the Buttomer number on the X-Keys Device.
* type can be normal to act like a normal key button, or toggle, to have a latching function.
* topic is the MQTT topic
* The on and off sections set the LED backlight state, values can be on, off or blink.

Example for KeypressOutput section:
````
<keypress>
  <topic>zusi/control/pause</topic>
  <hold>KEY_M</hold>
  OR
  <toggle>
  <activate ctrl="true" shift="true" alt="true">F2</activate>
  <deactivate>F2</deactivate>
  </toggle>
  OR
  <sequence>
				<step>F11</step>
				<step>F12</step>
				<step>F9</step>
  </sequence>
</keypress>
 ```
* topic is the MQTT topic
* There are three modes of operation dependending on which tag is used.
 * Hold depresses the key when '1' is received and releases it when '0' is received.
 * Toggle sends one keystroke a '1' is recieved and then another different one when '0' is received.
 * Sequence sends a series of keystrokes in steps, the next step is activated after a '1' is received. This can be used for instance to switch between a set different view angles withut needing different keys for each one.
 
 ### Midi2MQTT and MQTT2DirectInput
 
 These are command line tools which use an INI-like file format

## Requirements
To build the software you will need to some extra binary components:
* Eclipse Paho MQTT Client for C Version 1.x.x - https://eclipse.org/paho/
* RtMidi Version 2.x.x - http://www.music.mcgill.ca/~gary/rtmidi/
* vJoy Feeder SDK - http://vjoystick.sourceforge.net/site/

###Licenses
Includes code from the SimpleIni project(https://github.com/brofield/simpleini) licensed under the MIT License:
Copyright (c) 2006-2012, Brodie Thiesfield

Includes code derived from PInvoke.NET(http://www.pinvoke.net/) under the PInvoke.NET Terms of Use (http://www.pinvoke.net/termsofuse.htm)

Includes library headers from the vJoy Project(http://vjoystick.sourceforge.net/) under public domain Copyright (c) Shaul Eizikovich.

Includes library headers from the the Eclipse Paho Project(https://eclipse.org/paho/) licensed under the Eclipse Public License V 1.0:
Copyright (c) 2009, 2015 IBM Corp.

Includes library headers from the RtMidi Project(http://www.music.mcgill.ca/~gary/rtmidi/) licensed under the RtMidi license. Copyright (c) 2003-2016 Gary P. Scavone
