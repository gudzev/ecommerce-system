namespace Backend
{
    public class Product
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? image_url { get; set; }
        public string? description { get; set; }
        public int price_rsd { get; set; }
        public int? price_on_sale { get; set; }
        public int category_id { get; set; }
        public int stock_quantity { get; set; }
        public bool is_active { get; set; }

        public Product()
        {

        }
    }
}
