using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandsService.Dtos
{
    public class WindPowerPlatformPublishedDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Manufacturer { get; set; }

        public string Description { get; set; }

        public string Event { get; set; }
    }
}