#include "midiworker.h"
#include "midimessage.h"

#include <sstream>
#include <cmath>
#include <mutex>
#include <vector>
#include <condition_variable>
#include <iostream>


namespace{
    std::vector<MidiMessage> messageBuffer;
    std::mutex messageBufferLock;
	std::mutex messageWaitLock;
    std::condition_variable wait;
}

using namespace std;

void midiCallback(double deltatime, std::vector< unsigned char > *midiData, void *userData)
{
  /*unsigned int nBytes = midiData->size();
  for ( unsigned int i=0; i<nBytes; i++ )
  {
    std::cout << "Byte " << i << " = " << (int)midiData->at(i) << ", ";
  }
  if ( nBytes > 0 )
  {
    std::cout << "stamp = " << deltatime << std::endl;
  }*/

  MidiMessage message(*midiData, deltatime);

  {
	  std::lock_guard<std::mutex> guard(messageBufferLock);
	  messageBuffer.push_back(message);
  }

  wait.notify_all();
}

MidiWorker::MidiWorker(Mapping* m, unsigned int p) :
     m_port(p), doExit(false), m_mapping(m)
{

}

void MidiWorker::start()
{
    try {
      midiIn = new RtMidiIn();

      // Check inputs.
      unsigned int nPorts = midiIn->getPortCount();
      std::cout << "There are " << nPorts << " MIDI input sources available." << std::endl;

      std::string portName;
      for ( unsigned int i=0; i<nPorts; i++ ) {
          try {
              portName = midiIn->getPortName(i);
          }
          catch ( RtMidiError &error ) {
              error.printMessage();
          }

          std::cout <<"  Input Port #" << i+1 << ": " << portName << std::endl;

      }

      if (nPorts > 0)
	  {
          std::cout << "Selecting port #" << m_port << std::endl;
          midiIn->openPort( m_port );
          midiIn->setCallback(&midiCallback);
          midiIn->ignoreTypes(false, false, false);
	  }
	  else
	  {
		  std::cout << "Port #" << m_port << " is not available." << std::endl;
	  }
    }
    catch (RtMidiError &error) {
      // Handle the exception here
		std::cout << error.getMessage() << std::endl;
    }

	{
		std::unique_lock<std::mutex> waitGuard(messageWaitLock); //lock for notification waiting
		while (!doExit)
		{
			wait.wait(waitGuard);
			std::lock_guard<std::mutex> guard(messageBufferLock); //lock for message buffer access
			if (!messageBuffer.empty())
				processMessages();
		}
	}


}

void MidiWorker::processMessages()
{
    for(std::vector<MidiMessage>::const_iterator it = messageBuffer.begin(); it != messageBuffer.end(); ++ it)
    {
        if(it->isController())
        {
            m_mapping->mapInput(it->getChannel(), it->getControllerNumber(), it->getControllerValue());
        }
        else if(it->isNoteOff())
        {
			m_mapping->mapInput(it->getChannel(), it->getControllerNumber(), it->getControllerValue());
        }
        else if(it->isNoteOn())
        {
			m_mapping->mapInput(it->getChannel(), it->getControllerNumber(), it->getControllerValue());
        }
        std::cout << "Ch" << it->getChannel();

        if(it->isController())
        {
            std::cout << " CC " << it->getControllerNumber() << " = "
                    << it->getControllerValue();
        }
        else if(it->isNoteOff())
        {
            std::cout << " Note " << it->getNoteNumber() << "Off";
        }
        else if(it->isNoteOn())
        {
            std::cout << " Note " << it->getControllerNumber() << " On";
        }
        else
        {
            std::cout << " Unknown Message";
        }
		std::cout << std::endl;

    }
    messageBuffer.clear();
}

void MidiWorker::exit()
{
    doExit = true;
    wait.notify_all();
	if (workerThread.joinable())
		workerThread.join();
}

void MidiWorkerRunner(MidiWorker* w)
{
	w->start();
}
