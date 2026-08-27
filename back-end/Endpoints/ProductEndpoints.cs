using Backend.Models;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Text.Json;

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
                                    p.graphicsCardDetails = await new GraphicsCardDetails().getDetails(connectionString, p.id);
                                }
                                else if (reader["category"].ToString() == "Procesor")
                                {
                                    p.processorDetails = await new ProcessorDetails().getDetails(connectionString, p.id);
                                }
                                else if (reader["category"].ToString() == "Matična ploča")
                                {
                                    p.motherboardDetails = await new MotherboardDetails().getDetails(connectionString, p.id);
                                }
                                else if (reader["category"].ToString() == "Memorija")
                                {
                                    p.ramDetails = await new RAMDetails().getDetails(connectionString, p.id);
                                }
                                else if (reader["category"].ToString() == "SSD")
                                {
                                    p.ssdDetails = await new SSDDetails().getDetails(connectionString, p.id);
                                }
                                else if (reader["category"].ToString() == "HDD")
                                {
                                    p.hddDetails = await new HDDDetails().getDetails(connectionString, p.id);
                                }
                                else if (reader["category"].ToString() == "Napajanje")
                                {
                                    p.powerSupplyDetails = await new PowerSupplyDetails().getDetails(connectionString, p.id);
                                }
                                else if (reader["category"].ToString() == "Kućište")
                                {
                                    p.caseDetails = await new CaseDetails().getDetails(connectionString, p.id);
                                }
                                else
                                {
                                    // Product is not of a defined category
                                }
                            }
                        }
                    }

                    query = "SELECT image_url FROM product_images WHERE product_id = @productId";

                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@productId", p?.id);

                        using(SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<Image> secondaryImagesList = new List<Image>();
                            while(await reader.ReadAsync())
                            {
                                Image image = new Image();
                                image.url = reader["image_url"].ToString();
                                secondaryImagesList.Add(image);
                            }
                            p?.other_images = secondaryImagesList;
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
                    other_images = p?.other_images,
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

            app.MapGet("/products", async (bool? is_active = true, int? category_id = null, string? search_text = "%", int[]? product_ids = null, int? page = null, int? products_per_page = null) =>
            {
                List<Product> products = new List<Product>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"SELECT products.id, products.name, image_url, price_rsd, price_on_sale, category_id, stock_quantity, is_active, description, categories.name AS category
                                     FROM products
                                     JOIN categories ON categories.id = products.category_id
                                     WHERE products.name LIKE @searchText";

                    if (is_active == true)
                    {
                        query += " AND is_active = 1 ";
                    }

                    if (is_active == false)
                    {
                        query += " AND is_active = 0 ";
                    }

                    if(category_id.HasValue)
                    {
                        query += " AND category_id = @category_id ";
                    }

                    if (!page.HasValue && !products_per_page.HasValue && product_ids?.Length > 0)
                    {
                        query += " AND (";
                        for (int i = 0; i < product_ids?.Length; i++)
                        {
                            query += " products.id = " + product_ids[i];

                            if(i != product_ids?.Length - 1)
                            {
                                query += " OR ";
                            }
                        }
                        query += ")";
                    }

                    if (page.HasValue && products_per_page.HasValue)
                    {
                        // OFFSET and FETCH are used for pagination in SQL SERVER and must be used with ORDER BY statement
                        query += @"ORDER BY products.id
                               OFFSET " + page * products_per_page + @" ROWS
                               FETCH NEXT " + products_per_page + " ROWS ONLY";
                    }

                    Product p;

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if(category_id.HasValue)
                        {
                            command.Parameters.AddWithValue("@category_id", category_id);
                        }

                        command.Parameters.AddWithValue("@searchText", search_text == "%" ? search_text : search_text + "%");

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

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                int productId;
                                string query = @"INSERT INTO products(name, image_url, price_rsd, price_on_sale, category_id, stock_quantity, description)
                                                 OUTPUT INSERTED.id
                                                 VALUES(@name, @image_url, @price_rsd, @price_on_sale, @category_id, @stock_quantity, @description)";

                                using (SqlCommand command = new SqlCommand(query, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@name", p.name);
                                    command.Parameters.AddWithValue("@image_url", p.image_url);
                                    command.Parameters.AddWithValue("@price_rsd", Convert.ToInt32(p.price_rsd));
                                    command.Parameters.AddWithValue("@price_on_sale", (p.price_on_sale != null) ? Convert.ToInt32(p.price_on_sale) : DBNull.Value);
                                    command.Parameters.AddWithValue("@category_id", Convert.ToInt32(p.category_id));
                                    command.Parameters.AddWithValue("@stock_quantity", Convert.ToInt32(p.stock_quantity));
                                    command.Parameters.AddWithValue("@description", p.description);

                                    productId = Convert.ToInt32(await command.ExecuteScalarAsync());
                                }

                                if (p.caseDetails == null && p.powerSupplyDetails == null && p.graphicsCardDetails == null && p.hddDetails == null && p.motherboardDetails == null && p.processorDetails == null && p.ramDetails == null && p.ssdDetails == null)
                                {
                                    transaction.Commit();
                                    return Results.Ok(new { success = true, additionalMessage = "No specifications provided." });
                                }

                                string? categoryName = "";

                                query = @"SELECT name FROM categories WHERE id = @id";

                                using (SqlCommand command = new SqlCommand(query, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@id", p.category_id);

                                    using(SqlDataReader reader = await command.ExecuteReaderAsync())
                                    {
                                        if(await reader.ReadAsync())
                                        {
                                            categoryName = reader["name"].ToString();
                                        }
                                    }
                                }

                                p.id = productId;
                                switch (categoryName)
                                {
                                case "Grafička karta":
                                    await p?.graphicsCardDetails.postDetails(connectionString, p);
                                    break;

                                case "Procesor":
                                    await p?.processorDetails?.postDetails(connectionString, p);
                                    break;

                                case "Matična ploča":
                                    await p?.motherboardDetails?.postDetails(connectionString, p);
                                    break;

                                case "Memorija":
                                    await p?.ramDetails?.postDetails(connectionString, p);
                                    break;

                                case "SSD":
                                    await p?.ssdDetails?.postDetails(connectionString, p);
                                    break;

                                case "HDD":
                                    await p?.hddDetails?.postDetails(connectionString, p);
                                    break;

                                case "Napajanje":
                                    await p?.powerSupplyDetails?.postDetails(connectionString, p);
                                    break;

                                case "Kućište":
                                    await p?.caseDetails?.postDetails(connectionString, p);
                                    break;

                                default:
                                    // There is no product details
                                    break;
                                }

                                transaction.Commit();
                            }
                            catch(Exception ex)
                            {
                                transaction.Rollback();
                                return Results.Problem(ex.Message);
                            }
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

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            string query = @"UPDATE products
                                             SET name = @name, image_url = @image_url, price_rsd = @price_rsd,
                                                price_on_sale = @price_on_sale, category_id = @category_id,
                                                stock_quantity = @stock_quantity, description = @description
                                             WHERE id = @id";

                            using (SqlCommand command = new SqlCommand(query, connection, transaction))
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

                            if (p.caseDetails == null && p.powerSupplyDetails == null && p.graphicsCardDetails == null && p.hddDetails == null && p.motherboardDetails == null && p.processorDetails == null && p.ramDetails == null && p.ssdDetails == null)
                            {
                                transaction.Commit();
                                return Results.Ok(new { success = true, additionalMessage = "No specifications provided." });
                            }

                            string? categoryName = "";

                            query = @"SELECT name FROM categories WHERE id = @id";

                            using (SqlCommand command = new SqlCommand(query, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@id", p.category_id);

                                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                                {
                                    if (await reader.ReadAsync())
                                    {
                                        categoryName = reader["name"].ToString();
                                    }
                                }
                            }

                            switch (categoryName)
                            {
                                case "Grafička karta":
                                    await p?.graphicsCardDetails.putDetails(connectionString, p);
                                    break;

                                case "Procesor":
                                    await p?.processorDetails?.putDetails(connectionString, p);
                                    break;

                                case "Matična ploča":
                                    await p?.motherboardDetails?.putDetails(connectionString, p);
                                    break;

                                case "Memorija":
                                    await p?.ramDetails?.putDetails(connectionString, p);
                                    break;

                                case "SSD":
                                    await p?.ssdDetails?.putDetails(connectionString, p);
                                    break;

                                case "HDD":
                                    await p?.hddDetails?.putDetails(connectionString, p);
                                    break;

                                case "Napajanje":
                                    await p?.powerSupplyDetails?.putDetails(connectionString, p);
                                    break;

                                case "Kućište":
                                    await p?.caseDetails?.putDetails(connectionString, p);
                                    break;

                                default:
                                    // There is no product details
                                    break;
                            }

                            transaction.Commit();
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
