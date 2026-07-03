using Microsoft.Data.SqlClient;

namespace Backend.Models
{
    public class RAMDetails
    {
        public string? capacity { get; set; }
        public string? speed { get; set; }
        public string? timings { get; set; }
        public string? type { get; set; }

        public RAMDetails getDetails(string connectionString, int productId)
        {
            RAMDetails details;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT * FROM rams WHERE product_id = @productId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
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
    }
}
