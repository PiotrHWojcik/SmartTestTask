namespace SmartTestTask.Models.DTOs
{
    public class CreateEquipmentPlacementContractDto
    {
        public string ProductionFacilityCode { get; set; }
        public string ProcessEquipmentCode { get; set; }
        public int EquipmentQuantity { get; set; }
    }
}
