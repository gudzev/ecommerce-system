namespace Backend.Models
{
    public class SSDDetails
    {
        public int productId { get; set; }
        public string? read_speed { get; set; }
        public string? write_speed { get; set; }
        public string? _interface { get; set; }
        public string? dimensions { get; set; }

        public SSDDetails getDetails()
        {
            return null;
        }
    }
}
