using System.ComponentModel.DataAnnotations;

namespace SmartTestTask.Models
{
    public class EquipmentPlacementContract
    {
        public int Id { get; set; }

        [Required]
        public int ProductionFacilityId { get; set; }
        public ProductionFacility ProductionFacility { get; set; }

        [Required]
        public int ProcessEquipmentId { get; set; }
        public ProcessEquipment ProcessEquipment { get; set; }

        [Range(1, int.MaxValue)]
        public int EquipmentQuantity { get; set; }
    }
}
