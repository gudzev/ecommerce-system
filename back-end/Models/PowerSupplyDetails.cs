using Microsoft.Data.SqlClient;

namespace Backend.Models
{
    public class PowerSupplyDetails
    {
        public string? wattage { get; set; }
        public string? efficiency { get; set; }
        public string? brand { get; set; }

        public PowerSupplyDetails getDetails(string connectionString, int productId)
        {
            PowerSupplyDetails details;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT * FROM power_supplies WHERE product_id = @productId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            details = new PowerSupplyDetails();
                            details.brand = reader["brand"].ToString();
                            details.efficiency = reader["efficiency"].ToString();
                            details.wattage = reader["wattage"].ToString();

                            return details;
                        }
                    }
                }
            }
            return null;
        }
    }
}
