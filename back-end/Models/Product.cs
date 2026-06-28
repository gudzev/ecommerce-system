namespace Backend.Models
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
        public CaseDetails? caseDetails { get; set; }
        public MotherboardDetails? motherboardDetails { get; set; }
        public ProcessorDetails? processorDetails { get; set; }
        public SSDDetails? ssdDetails { get; set; }
        public HDDDetails? hddDetails { get; set; }
        public GraphicsCardDetails? graphicsCardDetails { get; set; }
        public PowerSupplyDetails? powerSupplyDetails { get; set; }
        public RAMDetails? ramDetails { get; set; }

        public Product()
        {

        }
    }
}
