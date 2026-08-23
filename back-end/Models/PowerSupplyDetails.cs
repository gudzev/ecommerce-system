using Microsoft.Data.SqlClient;

namespace Backend.Models
{
    public class PowerSupplyDetails
    {
        public string? wattage { get; set; }
        public string? efficiency { get; set; }
        public string? brand { get; set; }

        public async Task<PowerSupplyDetails> getDetails(string connectionString, int productId)
        {
            PowerSupplyDetails details;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"SELECT * FROM power_supplies WHERE product_id = @productId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
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

        public async Task postDetails(string connectionString, Product p)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"INSERT INTO power_supplies(product_id, wattage, efficiency, brand) 
                                 VALUES(@product_id, @wattage, @efficiency, @brand)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_id", p.id);
                    command.Parameters.AddWithValue("@wattage", p?.powerSupplyDetails?.wattage);
                    command.Parameters.AddWithValue("@efficiency", p?.powerSupplyDetails?.efficiency);
                    command.Parameters.AddWithValue("@brand", p?.powerSupplyDetails?.brand);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task putDetails(string connectionString, Product p)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"UPDATE power_supplies
                                 SET wattage = @wattage, efficiency = @efficiency, brand = @brand
                                 WHERE product_id = @product_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_id", p.id);
                    command.Parameters.AddWithValue("@wattage", p?.powerSupplyDetails?.wattage);
                    command.Parameters.AddWithValue("@efficiency", p?.powerSupplyDetails?.efficiency);
                    command.Parameters.AddWithValue("@brand", p?.powerSupplyDetails?.brand);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
