using Microsoft.Data.SqlClient;

namespace Backend.Models
{
    public class GraphicsCardDetails
    {
        public string? vram { get; set; }
        public string? _interface { get; set; }
        public string? dimensions { get; set; }
        public string? clockSpeed { get; set; }

        public GraphicsCardDetails getDetails(string connectionString, int productId)
        {
            GraphicsCardDetails details;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT * FROM graphics_cards WHERE product_id = @productId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
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
    }
}
