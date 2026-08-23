using Microsoft.Data.SqlClient;

namespace Backend.Models
{
    public class SSDDetails
    {
        public string? read_speed { get; set; }
        public string? write_speed { get; set; }
        public string? _interface { get; set; }
        public string? form_factor { get; set; }
        public string? capacity { get; set; }

        public async Task<SSDDetails> getDetails(string connectionString, int productId)
        {
            SSDDetails details;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"SELECT * FROM ssds WHERE product_id = @productId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            details = new SSDDetails();
                            details._interface = reader["interface"].ToString();
                            details.form_factor = reader["form_factor"].ToString();
                            details.capacity = reader["capacity"].ToString();
                            details.read_speed = reader["read_speed"].ToString();
                            details.write_speed = reader["write_speed"].ToString();

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

                string query = @"INSERT INTO ssds(product_id, interface, form_factor, capacity, read_speed, write_speed) 
                                 VALUES(@product_id, @interface, @form_factor, @capacity, @read_speed, @write_speed)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_id", p.id);
                    command.Parameters.AddWithValue("@interface", p?.ssdDetails?._interface);
                    command.Parameters.AddWithValue("@form_factor", p?.ssdDetails?.form_factor);
                    command.Parameters.AddWithValue("@capacity", p?.ssdDetails?.capacity);
                    command.Parameters.AddWithValue("@read_speed", p?.ssdDetails?.read_speed);
                    command.Parameters.AddWithValue("@write_speed", p?.ssdDetails?.write_speed);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task putDetails(string connectionString, Product p)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"UPDATE ssds
                                 SET interface = @interface, form_factor = @form_factor, capacity = @capacity, read_speed = @read_speed, write_speed = @write_speed
                                 WHERE product_id = @product_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_id", p.id);
                    command.Parameters.AddWithValue("@interface", p?.ssdDetails?._interface);
                    command.Parameters.AddWithValue("@form_factor", p?.ssdDetails?.form_factor);
                    command.Parameters.AddWithValue("@capacity", p?.ssdDetails?.capacity);
                    command.Parameters.AddWithValue("@read_speed", p?.ssdDetails?.read_speed);
                    command.Parameters.AddWithValue("@write_speed", p?.ssdDetails?.write_speed);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
