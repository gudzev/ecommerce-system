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

        public async Task<ProcessorDetails> getDetails(string connectionString, int productId)
        {
            ProcessorDetails details;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"SELECT * FROM processors WHERE product_id = @productId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
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
        public async void postDetails(string connectionString, Product p)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"INSERT INTO processors(product_id, cores, threads, l1Cache, l2Cache, l3Cache, socket, clock_speed)
                                 VALUES(@product_id, @cores, @threads, @l1Cache, @l2Cache, @l3Cache, @socket, @clock_speed)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_id", p.id);
                    command.Parameters.AddWithValue("@cores", p?.processorDetails?.cores);
                    command.Parameters.AddWithValue("@threads", p?.processorDetails?.threads);
                    command.Parameters.AddWithValue("@l1Cache", p?.processorDetails?.l1Cache);
                    command.Parameters.AddWithValue("@l2Cache", p?.processorDetails?.l2Cache);
                    command.Parameters.AddWithValue("@l3Cache", p?.processorDetails?.l3Cache);
                    command.Parameters.AddWithValue("@socket", p?.processorDetails?.socket);
                    command.Parameters.AddWithValue("@clock_speed", p?.processorDetails?.clockSpeed);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async void putDetails(string connectionString, Product p)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"UPDATE processors
                                 SET cores = @cores, threads = @threads, l1Cache = @l1Cache, l2Cache = @l2Cache, l3Cache = @l3Cache, socket = @socket, clock_speed = @clock_speed
                                 WHERE product_id = @product_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_id", p.id);
                    command.Parameters.AddWithValue("@cores", p?.processorDetails?.cores);
                    command.Parameters.AddWithValue("@threads", p?.processorDetails?.threads);
                    command.Parameters.AddWithValue("@l1Cache", p?.processorDetails?.l1Cache);
                    command.Parameters.AddWithValue("@l2Cache", p?.processorDetails?.l2Cache);
                    command.Parameters.AddWithValue("@l3Cache", p?.processorDetails?.l3Cache);
                    command.Parameters.AddWithValue("@socket", p?.processorDetails?.socket);
                    command.Parameters.AddWithValue("@clock_speed", p?.processorDetails?.clockSpeed);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
