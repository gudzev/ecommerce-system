using Backend.Models;
using Microsoft.Data.SqlClient;

namespace Backend.Endpoints
{
    public static class CategorySpecificationEndpoints
    {
        public static void MapCategorySpecificationEndpoints(this WebApplication app, string connectionString)
        {
            app.MapGet("/category-specifications", async (int categoryId) =>
            {
                List<CategorySpecification> specificationsList = new List<CategorySpecification>();

                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        await connection.OpenAsync();

                        string query = @"SELECT * 
                                     FROM category_specifications
                                     WHERE category_id = @categoryId";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@categoryId", categoryId);

                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    CategorySpecification spec = new CategorySpecification();
                                    spec.category_specification_id = Convert.ToInt32(reader["category_specification_id"]);
                                    spec.category_id = categoryId;
                                    spec.name = reader["name"].ToString();
                                    specificationsList.Add(spec);
                                }
                            }
                        }
                        return Results.Json(specificationsList);
                    }
                    catch(Exception ex)
                    {
                        return Results.Problem(ex.Message);
                    }

                }
            });

            app.MapPost("/category-specifications", async (CategorySpecification specification) =>
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        await connection.OpenAsync();

                        string query = @"INSERT INTO category_specifications(category_id, name)
                                         VALUES(@category_id, @name)";

                        using(SqlCommand command = new SqlCommand(query,connection))
                        {
                            command.Parameters.AddWithValue("@category_id", specification.category_id);
                            command.Parameters.AddWithValue("@name", specification.name);

                            await command.ExecuteNonQueryAsync();
                        }

                        return Results.Ok(new { success = true });
                    }
                    catch(Exception ex)
                    {
                        return Results.Problem(ex.Message);
                    }
                }
            });

            app.MapPut("/category-specifications", async (CategorySpecification specification) =>
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        await connection.OpenAsync();

                        string query = @"UPDATE category_specifications
                                         SET category_id = @category_id, name = @name
                                         WHERE category_specification_id = @category_specification_id";

                        using(SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@category_id", specification.category_id);
                            command.Parameters.AddWithValue("@name", specification.name);
                            command.Parameters.AddWithValue("@category_specification_id", specification.category_specification_id);

                            await command.ExecuteNonQueryAsync();
                        }

                        return Results.Ok(new { success = true });
                    }
                    catch(Exception ex)
                    {
                        return Results.Problem(ex.Message);
                    }
                }
            });

            app.MapDelete("/category-specifications", async (int category_specification_id) =>
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        await connection.OpenAsync();

                        string query = @"DELETE FROM category_specifications
                                         WHERE category_specification_id = @category_specification_id";

                        using(SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@category_specification_id", category_specification_id);

                            await command.ExecuteNonQueryAsync();
                        }
                        return Results.Ok(new { success = true });
                    }
                    catch(Exception ex)
                    {
                        return Results.Problem(ex.Message);
                    }
                }
            });
        }
    }
}
