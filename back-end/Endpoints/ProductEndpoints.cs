using Backend.Models;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;

namespace Backend.Endpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this WebApplication app, string connectionString)
        {
            app.MapGet("/products/{productId}", async (int productId) =>
            {
                Product? p = null;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"SELECT products.id, products.name, image_url, price_rsd, price_on_sale, category_id, stock_quantity, is_active, description, categories.name AS category 
                         FROM products
                         JOIN categories ON categories.id = products.category_id
                         WHERE products.id = @productId;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@productId", productId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                p = new Product();
                                p.id = Convert.ToInt32(reader["id"]);
                                p.name = reader["name"].ToString();
                                p.image_url = reader["image_url"].ToString();
                                p.price_rsd = Convert.ToInt32(reader["price_rsd"]);
                                p.price_on_sale = reader["price_on_sale"] != DBNull.Value ? Convert.ToInt32(reader["price_on_sale"]) : null;
                                p.category_id = Convert.ToInt32(reader["category_id"]);
                                p.stock_quantity = Convert.ToInt32(reader["stock_quantity"]);
                                p.is_active = Convert.ToBoolean(reader["is_active"]);
                                p.description = reader["description"].ToString();

                                if (reader["category"].ToString() == "Grafička karta")
                                {
                                    p.graphicsCardDetails = new GraphicsCardDetails().getDetails(connectionString, p.id);
                                }
                                else if (reader["category"].ToString() == "Procesor")
                                {
                                    p.processorDetails = new ProcessorDetails().getDetails(connectionString, p.id);
                                }
                                else if (reader["category"].ToString() == "Matična ploča")
                                {
                                    p.motherboardDetails = new MotherboardDetails().getDetails(connectionString, p.id);
                                }
                                else if (reader["category"].ToString() == "Memorija")
                                {
                                    p.ramDetails = new RAMDetails().getDetails(connectionString, p.id);
                                }
                                else if (reader["category"].ToString() == "SSD")
                                {
                                    p.ssdDetails = new SSDDetails().getDetails(connectionString, p.id);
                                }
                                else if (reader["category"].ToString() == "HDD")
                                {
                                    p.hddDetails = new HDDDetails().getDetails(connectionString, p.id);
                                }
                                else if (reader["category"].ToString() == "Napajanje")
                                {
                                    p.powerSupplyDetails = new PowerSupplyDetails().getDetails(connectionString, p.id);
                                }
                                else if (reader["category"].ToString() == "Kućište")
                                {
                                    p.caseDetails = new CaseDetails().getDetails(connectionString, p.id);
                                }
                                else
                                {
                                    // Product is not of a defined category
                                }
                            }
                        }
                    }
                }

                return Results.Json(new
                {
                    id = p?.id,
                    name = p?.name,
                    image_url = p?.image_url,
                    price_rsd = p?.price_rsd,
                    price_on_sale = p?.price_on_sale,
                    category_id = p?.category_id,
                    stock_quantity = p?.stock_quantity,
                    is_active = p?.is_active,
                    description = p?.description,
                    details =
                    p?.graphicsCardDetails ??
                    p?.processorDetails ??
                    p?.motherboardDetails ??
                    p?.ramDetails ??
                    p?.ssdDetails ??
                    p?.hddDetails ??
                    (object?)p?.powerSupplyDetails ??
                    p?.caseDetails
                });
            });

            app.MapGet("/products", async (bool? is_active) =>
            {
                List<Product> products = new List<Product>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"SELECT products.id, products.name, image_url, price_rsd, price_on_sale, category_id, stock_quantity, is_active, description, categories.name AS category 
                         FROM products
                         JOIN categories ON categories.id = products.category_id";

                    if (is_active == true)
                    {
                        query += " WHERE is_active = 1";
                    }

                    if (is_active == false)
                    {
                        query += " WHERE is_active = 0";
                    }

                    Product p;

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                p = new Product();
                                p.id = Convert.ToInt32(reader["id"]);
                                p.name = reader["name"].ToString();
                                p.image_url = reader["image_url"].ToString();
                                p.price_rsd = Convert.ToInt32(reader["price_rsd"]);
                                p.price_on_sale = reader["price_on_sale"] != DBNull.Value ? Convert.ToInt32(reader["price_on_sale"]) : null;
                                p.category_id = Convert.ToInt32(reader["category_id"]);
                                p.stock_quantity = Convert.ToInt32(reader["stock_quantity"]);
                                p.is_active = Convert.ToBoolean(reader["is_active"]);
                                p.description = reader["description"].ToString();

                                products.Add(p);
                            }
                        }
                    }
                }
                return Results.Json(products);
            });

            app.MapPost("/products", async (Product p) =>
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        string query = @"INSERT INTO products(name, image_url, price_rsd, price_on_sale, category_id, stock_quantity, description)
                             VALUES(@name, @image_url, @price_rsd, @price_on_sale, @category_id, @stock_quantity, @description)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@name", p.name);
                            command.Parameters.AddWithValue("@image_url", p.image_url);
                            command.Parameters.AddWithValue("@price_rsd", Convert.ToInt32(p.price_rsd));
                            command.Parameters.AddWithValue("@price_on_sale", (p.price_on_sale != null) ? Convert.ToInt32(p.price_on_sale) : DBNull.Value);
                            command.Parameters.AddWithValue("@category_id", Convert.ToInt32(p.category_id));
                            command.Parameters.AddWithValue("@stock_quantity", Convert.ToInt32(p.stock_quantity));
                            command.Parameters.AddWithValue("@description", p.description);

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

            app.MapPut("/products", async (Product p) =>
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        string query = @"UPDATE products
                             SET name = @name, image_url = @image_url, price_rsd = @price_rsd,
                                 price_on_sale = @price_on_sale, category_id = @category_id,
                                 stock_quantity = @stock_quantity, description = @description
                             WHERE id = @id";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@name", p.name);
                            command.Parameters.AddWithValue("@image_url", p.image_url);
                            command.Parameters.AddWithValue("@price_rsd", p.price_rsd);
                            command.Parameters.AddWithValue("@price_on_sale", (p.price_on_sale != null) ? Convert.ToInt32(p.price_on_sale) : DBNull.Value);
                            command.Parameters.AddWithValue("@category_id", p.category_id);
                            command.Parameters.AddWithValue("@stock_quantity", p.stock_quantity);
                            command.Parameters.AddWithValue("@id", p.id);
                            command.Parameters.AddWithValue("@description", p.description);

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

            app.MapPatch("/products/{productId}/status", async (int productId, bool isActive) =>
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        string query = @"UPDATE products
                             SET is_active = @isActive
                             WHERE id = @id";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", productId);
                            command.Parameters.AddWithValue("@isActive", isActive);

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
