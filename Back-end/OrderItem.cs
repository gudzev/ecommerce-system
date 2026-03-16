namespace FrontEndCommunicator
{
    public class OrderItem
    {
        public int productId { get; set; }
        public int order_id { get; set; }
        public int quantity { get; set; }
        public int price_at_purchase { get; set; }
        public string? product_name { get; set; }
        public string? product_image_url { get; set; }

        public OrderItem()
        {

        }

        public OrderItem(int productId, int order_id, int quantity, int price_at_purchase, string name, string image_url)
        {
            this.productId = productId;
            this.order_id = order_id;
            this.quantity = quantity;
            this.price_at_purchase = price_at_purchase;
            this.product_name = name;
            this.product_image_url = image_url;
        }
    }
}
