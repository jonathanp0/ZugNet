/*
==============================================================================
This file is part of the JUCE library - "Jules' Utility Class Extensions"
Copyright 2004-11 by Raw Material Software Ltd.
------------------------------------------------------------------------------
JUCE can be redistributed and/or modified under the terms of the GNU General
Public License (Version 2), as published by the Free Software Foundation.
A copy of the license is included in the JUCE distribution, or can be found
online at www.gnu.org/licenses.
JUCE is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
------------------------------------------------------------------------------
To release a closed-source product which uses JUCE, commercial licenses are
available: visit www.rawmaterialsoftware.com/juce for more information.
==============================================================================
*/

#ifndef MIDIMESSAGE_H
#define MIDIMESSAGE_H

#include <vector>

class MidiMessage
{
public:

    MidiMessage ();

    MidiMessage (std::vector<unsigned char>& d, double t);



    double getTimeStamp() const                         { return timeStamp; }


    //==============================================================================
    /** Returns the midi channel associated with the message.

        @returns    a value 1 to 16 if the message has a channel, or 0 if it hasn't (e.g.
                    if it's a sysex)
        @see isForChannel, setChannel
    */
    int getChannel() const ;

    //==============================================================================
    /** Returns true if this message is a 'key-down' event.

        @param returnTrueForVelocity0   if true, then if this event is a note-on with
                        velocity 0, it will still be considered to be a note-on and the
                        method will return true. If returnTrueForVelocity0 is false, then
                        if this is a note-on event with velocity 0, it'll be regarded as
                        a note-off, and the method will return false

        @see isNoteOff, getNoteNumber, getVelocity, noteOn
    */
    bool isNoteOn (bool returnTrueForVelocity0 = false) const ;

    /** Returns true if this message is a 'key-up' event.

        If returnTrueForNoteOnVelocity0 is true, then his will also return true
        for a note-on event with a velocity of 0.

        @see isNoteOn, getNoteNumber, getVelocity, noteOff
    */
    bool isNoteOff (bool returnTrueForNoteOnVelocity0 = true) const ;

    /** Returns the midi note number for note-on and note-off messages.

        If the message isn't a note-on or off, the value returned is undefined.

        @see isNoteOff, getMidiNoteName, getMidiNoteInHertz, setNoteNumber
    */
    int getNoteNumber() const ;

    //==============================================================================
    /** Returns true if this is a midi controller message.

        @see getControllerNumber, getControllerValue, controllerEvent
    */
    bool isController() const ;

    /** Returns the controller number of a controller message.

        The name of the controller can be looked up using the getControllerName() method.

        Note that the value returned is invalid for messages that aren't controller changes.

        @see isController, getControllerName, getControllerValue
    */
    int getControllerNumber() const ;

    /** Returns the controller value from a controller message.

        A value 0 to 127 is returned to indicate the new controller position.

        Note that the value returned is invalid for messages that aren't controller changes.

        @see isController, getControllerNumber
    */
    int getControllerValue() const ;

private:
    //==============================================================================
    double timeStamp;
    unsigned char data[3];
    int size;

};

#endif
