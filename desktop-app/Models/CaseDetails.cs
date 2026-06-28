namespace Backend.Models
{
    public class CaseDetails
    {
        public string? maxGpuLength { get; set; }
        public string? maxCpuCoolerHeight { get; set; }
        public string? size { get; set; }
        public string? weight { get; set; }
        public string? motherboardSize { get; set; }
        public string? dimensions { get; set; }
        public string? cooling { get; set; }

        public CaseDetails getDetails()
        {
            return null;
        }
    }
}
