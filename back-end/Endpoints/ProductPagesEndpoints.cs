using Microsoft.Data.SqlClient;

namespace Backend.Endpoints
{
    public static class ProductPagesEndpoints
    {
        public static void MapProductPagesEndpoints(this WebApplication app, string connectionString)
        {
            app.MapGet("/product-pages", async (int products_per_page, int? category_id = null, string? search_text = "%") =>
            {
                int productNumber = 0;

                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"SELECT COUNT(*) AS total_products
                                     FROM products
                                     WHERE products.name LIKE @searchText";

                    if(category_id.HasValue)
                    {
                        query += " AND category_id = @category_id ";
                    }

                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        if(category_id.HasValue)
                        {
                            command.Parameters.AddWithValue("@category_id", category_id);
                        }

                        command.Parameters.AddWithValue("@searchText", search_text == "%" ? search_text : search_text + "%");

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if(await reader.ReadAsync())
                            {
                                productNumber = Convert.ToInt32(reader["total_products"]);
                            }
                            else
                            {
                                return Results.NoContent();
                            }
                        }
                    }
                }

                double page_number = Math.Ceiling((double)productNumber / products_per_page);

                return Results.Ok(page_number);
            });
        }
    }
}
