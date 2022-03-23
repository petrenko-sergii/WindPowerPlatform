using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandsService.Dtos
{
    public class CommandReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int PlatformId { get; set; }   
    }
}