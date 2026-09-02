namespace Backend.Models
{
    public class ProductSpecification
    {
        public int? category_specification_id { get; set;}
        public int? product_id { get; set; }
        public string? value { get; set; }

        public string? name { get; set; } // Specification name, property related to CategorySpecifications
    }
}
