namespace Backend.Models
{
    public class CategorySpecification
    {
        public int? category_specification_id { get; set; }
        public int? category_id { get; set; }
        public string? name { get; set; } // Specification name
    }
}
