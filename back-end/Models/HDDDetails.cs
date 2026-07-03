using Microsoft.Data.SqlClient;

namespace Backend.Models
{
    public class HDDDetails
    {
        public string? read_speed { get; set; }
        public string? write_speed { get; set; }
        public string? rpm { get; set; }
        public string? form_factor { get; set; }
        public string? capacity { get; set; }

        public HDDDetails getDetails(string connectionString, int productId)
        {
            HDDDetails details;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT * FROM hdds WHERE product_id = @productId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            details = new HDDDetails();
                            details.rpm = reader["rpm"].ToString();
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
    }
}
