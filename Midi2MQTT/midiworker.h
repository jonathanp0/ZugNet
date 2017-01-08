#ifndef MIDIWORKER_H
#define MIDIWORKER_H

#include <RtMidi.h>
#include "midimessage.h"
#include "mapping.h"

#include <thread>


struct RawMidiMessage
{
    double time;
    std::vector< unsigned char > data;
};

class MidiWorker
{

public:
    explicit MidiWorker(Mapping* m, unsigned int p);
    
    void start();
    void exit();

private:
    void processMessages();
	float mapValue(unsigned char input, float min, float max);

	unsigned int m_port;
	bool doExit;
    RtMidiIn *midiIn;
    Mapping* m_mapping;

	std::thread workerThread;
    
};

void MidiWorkerRunner(MidiWorker* w);

#endif // MIDIWORKER_H
