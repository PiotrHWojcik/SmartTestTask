using System.ComponentModel.DataAnnotations;

namespace SmartTestTask.Models
{
    public class ProductionFacility
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Code { get; set; }

        [Required]
        [MaxLength(120)]
        public string Name { get; set; }

        [Range(0, double.MaxValue)]
        public double StandardArea { get; set; }
    }
}
