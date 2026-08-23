using Microsoft.Data.SqlClient;

namespace Backend.Models
{
    public class RAMDetails
    {
        public string? capacity { get; set; }
        public string? speed { get; set; }
        public string? timings { get; set; }
        public string? type { get; set; }

        public async Task<RAMDetails> getDetails(string connectionString, int productId)
        {
            RAMDetails details;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"SELECT * FROM rams WHERE product_id = @productId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            details = new RAMDetails();
                            details.capacity = reader["capacity"].ToString();
                            details.timings = reader["timings"].ToString();
                            details.speed = reader["speed"].ToString();
                            details.type = reader["type"].ToString();

                            return details;
                        }
                    }
                }
            }
            return null;
        }

        public async void postDetails(string connectionString, Product p)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"INSERT INTO rams(product_id, capacity, speed, timings, type) 
                                 VALUES(@product_id, @capacity, @speed, @timings, @type)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_id", p.id);
                    command.Parameters.AddWithValue("@capacity", p?.ramDetails?.capacity);
                    command.Parameters.AddWithValue("@speed", p?.ramDetails?.speed);
                    command.Parameters.AddWithValue("@timings", p?.ramDetails?.timings);
                    command.Parameters.AddWithValue("@type", p?.ramDetails?.type);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async void putDetails(string connectionString, Product p)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"UPDATE rams 
                                 SET capacity = @capacity, speed = @speed, timings = @timings, type = @type
                                 WHERE product_id = @product_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_id", p.id);
                    command.Parameters.AddWithValue("@capacity", p?.ramDetails?.capacity);
                    command.Parameters.AddWithValue("@speed", p?.ramDetails?.speed);
                    command.Parameters.AddWithValue("@timings", p?.ramDetails?.timings);
                    command.Parameters.AddWithValue("@type", p?.ramDetails?.type);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
