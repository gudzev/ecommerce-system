using Backend.Models;
using Microsoft.Data.SqlClient;

namespace Backend.Endpoints
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this WebApplication app, string connectionString)
        {
            app.MapGet("/categories", async () =>
            {
                List<Category> categories = new List<Category>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM categories", connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Category c = new Category();
                                c.id = Convert.ToInt32(reader["id"]);
                                c.name = reader["name"].ToString();
                                categories.Add(c);
                            }
                        }
                    }
                }
                return Results.Json(categories);
            });

            app.MapPost("/categories", async (Category c) =>
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        string query = @"INSERT INTO categories(name) 
                             VALUES(@name)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@name", c.name);

                            await command.ExecuteNonQueryAsync();
                        }
                    }
                    return Results.Ok(new { success = true });
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });

            app.MapPut("/categories", async (Category c) =>
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        string query = @"UPDATE categories
                             SET name = @name
                             WHERE id = @id";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@name", c.name);
                            command.Parameters.AddWithValue("@id", c.id);

                            await command.ExecuteNonQueryAsync();
                        }
                    }
                    return Results.Ok(new { success = true });
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });

            app.MapDelete("/categories/{categoryId}", async (int categoryId) =>
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        string query = @"DELETE FROM categories
                             WHERE id = @id";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", categoryId);

                            await command.ExecuteNonQueryAsync();
                        }
                    }
                    return Results.Ok(new { success = true });
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });
        }
    }
}
