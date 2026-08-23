using Microsoft.Data.SqlClient;

namespace Backend.Models
{
    public class GraphicsCardDetails
    {
        public string? vram { get; set; }
        public string? _interface { get; set; }
        public string? dimensions { get; set; }
        public string? clockSpeed { get; set; }

        public async Task<GraphicsCardDetails> getDetails(string connectionString, int productId)
        {
            GraphicsCardDetails details;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"SELECT * FROM graphics_cards WHERE product_id = @productId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            details = new GraphicsCardDetails();
                            details.vram = reader["vram"].ToString();
                            details._interface = reader["interface"].ToString();
                            details.dimensions = reader["dimensions"].ToString();
                            details.clockSpeed = reader["clock_speed"].ToString();

                            return details;
                        }
                    }
                }
            }
            return null;
        }

        public async Task postDetails(string connectionString, Product p)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"INSERT INTO graphics_cards(product_id, vram, interface, dimensions, clock_speed)
                                 VALUES(@product_id, @vram, @interface, @dimensions, @clock_speed)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_id", p.id);
                    command.Parameters.AddWithValue("@vram", p?.graphicsCardDetails?.vram);
                    command.Parameters.AddWithValue("@interface", p?.graphicsCardDetails?._interface);
                    command.Parameters.AddWithValue("@dimensions", p?.graphicsCardDetails?.dimensions);
                    command.Parameters.AddWithValue("@clock_speed", p?.graphicsCardDetails?.clockSpeed);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task putDetails(string connectionString, Product p)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"UPDATE graphics_cards
                                 SET vram = @vram, interface = @interface, dimensions = @dimensions, clock_speed = @clock_speed
                                 WHERE product_id = @product_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_id", p.id);
                    command.Parameters.AddWithValue("@vram", p?.graphicsCardDetails?.vram);
                    command.Parameters.AddWithValue("@interface", p?.graphicsCardDetails?._interface);
                    command.Parameters.AddWithValue("@dimensions", p?.graphicsCardDetails?.dimensions);
                    command.Parameters.AddWithValue("@clock_speed", p?.graphicsCardDetails?.clockSpeed);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
