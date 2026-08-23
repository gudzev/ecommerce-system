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

        public async Task<CaseDetails> getDetails(string connectionString, int productId)
        {
            CaseDetails details;

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"SELECT * FROM cases WHERE product_id = @productId";

                using(SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if(await reader.ReadAsync())
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

        public async Task postDetails(string connectionString, Product p)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"INSERT INTO cases(product_id, max_gpu_length, max_cpu_cooler_height, size, weight, motherboard_size, dimensions, cooling) 
                                 VALUES(@product_id, @max_gpu_length, @max_cpu_cooler_height, @size, @weight, @motherboard_size, @dimensions, @cooling)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_id", p.id);
                    command.Parameters.AddWithValue("@max_gpu_length", p?.caseDetails?.maxGpuLength);
                    command.Parameters.AddWithValue("@max_cpu_cooler_height", p?.caseDetails?.maxCpuCoolerHeight);
                    command.Parameters.AddWithValue("@size", p?.caseDetails?.size);
                    command.Parameters.AddWithValue("@weight", p?.caseDetails?.weight);
                    command.Parameters.AddWithValue("@motherboard_size", p?.caseDetails?.motherboardSize);
                    command.Parameters.AddWithValue("@dimensions", p?.caseDetails?.dimensions);
                    command.Parameters.AddWithValue("@cooling", p?.caseDetails?.cooling);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task putDetails(string connectionString, Product p)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"UPDATE cases
                                 SET max_gpu_length = @max_gpu_length, max_cpu_cooler_height = @max_cpu_cooler_height, size = @size, weight = @weight,
                                     motherboard_size = @motherboard_size, dimensions = @dimensions, cooling = @cooling)
                                 WHERE product_id = @product_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_id", p.id);
                    command.Parameters.AddWithValue("@max_gpu_length", p?.caseDetails?.maxGpuLength);
                    command.Parameters.AddWithValue("@max_cpu_cooler_height", p?.caseDetails?.maxCpuCoolerHeight);
                    command.Parameters.AddWithValue("@size", p?.caseDetails?.size);
                    command.Parameters.AddWithValue("@weight", p?.caseDetails?.weight);
                    command.Parameters.AddWithValue("@motherboard_size", p?.caseDetails?.motherboardSize);
                    command.Parameters.AddWithValue("@dimensions", p?.caseDetails?.dimensions);
                    command.Parameters.AddWithValue("@cooling", p?.caseDetails?.cooling);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
