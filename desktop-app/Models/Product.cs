namespace Backend.Models
{
    public class Product
    {
        public int id { get; set; }
        public string? name { get; set; }

        public string? image_url { get; set; } // Field for an image when only main one is needed
     
        public string? description { get; set; }
        public int price_rsd { get; set; }
        public int? price_on_sale { get; set; }
        public int category_id { get; set; }
        public int stock_quantity { get; set; }
        public bool is_active { get; set; }
        public List<ProductSpecification> specifications { get; set; } = new List<ProductSpecification>();
        public List<Image> images { get; set; } = new List<Image>();

        public Product()
        {

        }
    }
}
