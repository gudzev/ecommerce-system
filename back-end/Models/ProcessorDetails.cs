using Microsoft.Data.SqlClient;

namespace Backend.Models
{
    public class ProcessorDetails
    {
        public string? cores { get; set; }
        public string? threads { get; set; }
        public string? l1Cache { get; set; }
        public string? l2Cache { get; set; }
        public string? l3Cache { get; set; }
        public string? socket { get; set; }
        public string? clockSpeed { get; set; }

        public ProcessorDetails getDetails(string connectionString, int productId)
        {
            ProcessorDetails details;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT * FROM processors WHERE product_id = @productId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            details = new ProcessorDetails();
                            details.cores = reader["cores"].ToString();
                            details.threads = reader["threads"].ToString();
                            details.l1Cache = reader["l1_cache"].ToString();
                            details.l2Cache = reader["l2_cache"].ToString();
                            details.l3Cache = reader["l3_cache"].ToString();
                            details.socket = reader["socket"].ToString();
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
