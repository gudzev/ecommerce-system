namespace Backend.Models
{
    public class MotherboardDetails
    {
        public string? socket { get; set; }
        public string? ramType { get; set; }
        public string? chipset { get; set; }
        public bool? wifi { get; set; }
        public bool? bluetooth { get; set; }
        public int? ramSlots { get; set; }
        public int? m2Slots { get; set; }
        public int? sataSlots { get; set; }
        public int? pcieSlots { get; set; }
        public string? size { get; set; }

        public MotherboardDetails getDetails()
        {
            return null;
        }
    }
}
