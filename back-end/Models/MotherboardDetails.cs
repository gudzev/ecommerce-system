using Microsoft.Data.SqlClient;

namespace Backend.Models
{
    public class MotherboardDetails
    {
        public string? socket { get; set; }
        public string? ramType { get; set; }
        public string? chipset { get; set; }
        public bool? wifi { get; set; }
        public bool? bluetooth { get; set; }
        public int? ramSlots { get; set; }
        public int? m2Slots { get; set; }
        public int? sataSlots { get; set; }
        public int? pcieSlots { get; set; }
        public string? size { get; set; }

        public MotherboardDetails getDetails(string connectionString, int productId)
        {
            MotherboardDetails details;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT * FROM motherboards WHERE product_id = @productId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            details = new MotherboardDetails();
                            details.socket = reader["socket"].ToString();
                            details.size = reader["size"].ToString();
                            details.ramType = reader["ram_type"].ToString();
                            details.chipset = reader["chipset"].ToString();
                            details.ramSlots = Convert.ToInt32(reader["ram_slots"]);
                            details.m2Slots = Convert.ToInt32(reader["m2_slots"]);
                            details.sataSlots = Convert.ToInt32(reader["sata_slots"]);
                            details.pcieSlots = Convert.ToInt32(reader["pcie_slots"]);
                            details.wifi = Convert.ToBoolean(reader["wi_fi"]);
                            details.bluetooth = Convert.ToBoolean(reader["bluetooth"]);

                            return details;
                        }
                    }
                }
            }
            return null;
        }
    }
}
