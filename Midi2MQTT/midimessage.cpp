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

#include "midimessage.h"


#include <stdlib.h>

MidiMessage::MidiMessage() : timeStamp(0)
{
    memset(&data[0], 0, sizeof(data));
}

MidiMessage::MidiMessage (std::vector<unsigned char>& d, double t) : timeStamp(t)
{
    for(unsigned int i = 0; i < 3; ++i)
    {
        if (i < d.size())
            data[i] = d[i];
        else
            data[i] = 0;
    }
}

int MidiMessage::getChannel() const
{
    if ((data[0] & 0xf0) != 0xf0)
        return (data[0] & 0xf) + 1;

    return 0;
}

bool MidiMessage::isController() const
{
    return (data[0] & 0xf0) == 0xb0;
}

int MidiMessage::getControllerNumber() const
{
    return data[1];
}

int MidiMessage::getControllerValue() const
{
    return data[2];
}

bool MidiMessage::isNoteOn (const bool returnTrueForVelocity0) const
{
    return ((data[0] & 0xf0) == 0x90)
             && (returnTrueForVelocity0 || data[2] != 0);
}

bool MidiMessage::isNoteOff (const bool returnTrueForNoteOnVelocity0) const
{
    return ((data[0] & 0xf0) == 0x80)
            || (returnTrueForNoteOnVelocity0 && (data[2] == 0) && ((data[0] & 0xf0) == 0x90));
}

int MidiMessage::getNoteNumber() const
{
    return data[1];
}
