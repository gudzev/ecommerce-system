namespace FrontEndCommunicator
{
    public class DeliveryOption
    {
        public int id { get; set; }
        public int price_per_item { get; set; }
        public string name { get; set; }
        public int free_shipping_minimum_value { get; set; }
        public bool is_default { get; set; }
    }
}
