using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandsService.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}