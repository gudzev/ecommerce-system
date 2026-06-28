namespace Backend.Models
{
    public class ProcessorDetails
    {
        public string? cores { get; set; }
        public string? threads { get; set; }
        public string? l1Cache { get; set; }
        public string? l2Cache { get; set; }
        public string? l3Cache { get; set; }
        public string? socket { get; set; }
        public string? clockSpeed { get; set; }

        public ProcessorDetails getDetails()
        {
            return null;
        }
    }
}
