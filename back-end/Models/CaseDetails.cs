using Microsoft.Data.SqlClient;

namespace Backend.Models
{
    public class CaseDetails
    {
        public string? maxGpuLength { get; set; }
        public string? maxCpuCoolerHeight { get; set; }
        public string? size { get; set; }
        public string? weight { get; set; }
        public string? motherboardSize { get; set; }
        public string? dimensions { get; set; }
        public string? cooling { get; set; }

        public CaseDetails getDetails(string connectionString, int productId)
        {
            CaseDetails details;

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT * FROM cases WHERE product_id = @productId";

                using(SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            details = new CaseDetails();
                            details.weight = reader["weight"].ToString();
                            details.size = reader["size"].ToString();
                            details.maxGpuLength = reader["max_gpu_length"].ToString();
                            details.maxCpuCoolerHeight = reader["max_cpu_cooler_height"].ToString();
                            details.motherboardSize = reader["motherboard_size"].ToString();
                            details.dimensions = reader["dimensions"].ToString();
                            details.cooling = reader["cooling"].ToString();

                            return details;
                        }
                    }
                }
            }
            return null;
        }
    }
}
