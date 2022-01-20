using System.ComponentModel.DataAnnotations;

namespace  WindPowerPlatformService.Dtos
{
    public class WindPowerPlatformCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Manufacturer { get; set; }

        [Required]
        public string Description { get; set; }
    }
}