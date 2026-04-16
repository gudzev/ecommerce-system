using System.Collections.ObjectModel;

namespace Backend
{
    public class Order
    {
        public int id { get; set; }
        public string? email { get; set; }
        public string? name { get; set; }
        public string? surname { get; set; }
        public string? street { get; set; }
        public string? apartment_number { get; set; }
        public string? city { get; set; }
        public string? additional { get; set; }
        public string? phone_number { get; set; }
        public int delivery_method_id { get; set; }
        public DateTime created_at { get; set; }
        public ObservableCollection<OrderItem>? orderItems { get; set; }
        public bool? is_fulfilled { get; set; }
    }
}
