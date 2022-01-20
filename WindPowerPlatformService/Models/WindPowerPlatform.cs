using System.ComponentModel.DataAnnotations;

namespace WindPowerPlatformService.Models
{
    public class WindPowerPlatform
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Manufacturer { get; set; }

         [Required]
        public string Description { get; set; }
    }
}