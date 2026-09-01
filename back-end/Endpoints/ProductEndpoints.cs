using Backend.Models;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Transactions;

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

                    string query = @"SELECT products.id, products.name, price_rsd, price_on_sale, category_id, stock_quantity, is_active, description, categories.name AS category 
                                    FROM products
                                    JOIN categories ON categories.id = products.category_id
                                    WHERE products.id = @productId";

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
                                p.price_rsd = Convert.ToInt32(reader["price_rsd"]);
                                p.price_on_sale = reader["price_on_sale"] != DBNull.Value ? Convert.ToInt32(reader["price_on_sale"]) : null;
                                p.category_id = Convert.ToInt32(reader["category_id"]);
                                p.stock_quantity = Convert.ToInt32(reader["stock_quantity"]);
                                p.is_active = Convert.ToBoolean(reader["is_active"]);
                                p.description = reader["description"].ToString();
                            }
                        }
                    }

                    query = @"SELECT image_url, is_main_image, image_id
                            FROM images 
                            WHERE product_id = @productId
                            ORDER BY is_main_image DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@productId", p?.id);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<Image> imagesList = new List<Image>();
                            while (await reader.ReadAsync())
                            {
                                Image image = new Image();
                                image.id = Convert.ToInt32(reader["image_id"]);
                                image.url = reader["image_url"].ToString();
                                image.is_main_image = Convert.ToBoolean(reader["is_main_image"]);
                                imagesList.Add(image);
                            }
                            p?.images = imagesList;
                        }
                    }

                    query = @"SELECT category_specifications.name, product_specifications.value, product_specifications.category_specification_id AS _category_specification_id
                                FROM product_specifications
                                JOIN category_specifications ON product_specifications.category_specification_id = category_specifications.category_specification_id
                                WHERE product_specifications.product_id = @productId;";

                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@productId", p?.id);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<ProductSpecification> specifications = new List<ProductSpecification>();
                            while(await reader.ReadAsync())
                            {
                                ProductSpecification productSpecification = new ProductSpecification();
                                productSpecification.category_specification_id = Convert.ToInt32(reader["_category_specification_id"]);
                                productSpecification.name = reader["name"].ToString();
                                productSpecification.value = reader["value"].ToString();
                                specifications.Add(productSpecification);
                            }
                            p?.specifications = specifications;
                        }
                    }}

                    return Results.Json(new
                    {
                        id = p?.id,
                        name = p?.name,
                        price_rsd = p?.price_rsd,
                        price_on_sale = p?.price_on_sale,
                        category_id = p?.category_id,
                        stock_quantity = p?.stock_quantity,
                        description = p?.description,
                        is_active = p?.is_active,
                        images = p?.images,
                        details = p?.specifications
                    });
            });

            app.MapGet("/products", async (bool? is_active = null, int? category_id = null, string? search_text = "%", int[]? product_ids = null, int? page = null, int? products_per_page = null) =>
            {
                List<Product> products = new List<Product>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"SELECT products.id, images.image_url, products.name, price_rsd, price_on_sale, category_id, stock_quantity, is_active, description, categories.name AS category
                                     FROM products
                                     JOIN categories ON categories.id = products.category_id
                                     JOIN images ON images.product_id = products.id
                                     WHERE images.is_main_image = 1 AND products.name LIKE @searchText";

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
                                string query = @"INSERT INTO products(name, price_rsd, price_on_sale, category_id, stock_quantity, description)
                                                 OUTPUT INSERTED.id
                                                 VALUES(@name, @price_rsd, @price_on_sale, @category_id, @stock_quantity, @description)";

                                using (SqlCommand command = new SqlCommand(query, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@name", p.name);
                                    command.Parameters.AddWithValue("@price_rsd", Convert.ToInt32(p.price_rsd));
                                    command.Parameters.AddWithValue("@price_on_sale", (p.price_on_sale != null) ? Convert.ToInt32(p.price_on_sale) : DBNull.Value);
                                    command.Parameters.AddWithValue("@category_id", Convert.ToInt32(p.category_id));
                                    command.Parameters.AddWithValue("@stock_quantity", Convert.ToInt32(p.stock_quantity));
                                    command.Parameters.AddWithValue("@description", p.description);

                                    productId = Convert.ToInt32(await command.ExecuteScalarAsync());
                                }

                                p.id = productId;

                                if(p.images.Count > 0)
                                {
                                    query = @"INSERT INTO images(product_id, image_url, is_main_image)
                                          VALUES(@product_id, @image_url, @is_main_image)";

                                    foreach (Image image in p.images)
                                    {
                                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                                        {
                                            command.Parameters.AddWithValue("@product_id", p.id);
                                            command.Parameters.AddWithValue("@image_url", image.url);
                                            command.Parameters.AddWithValue("@is_main_image", image.is_main_image);

                                            await command.ExecuteNonQueryAsync();
                                        }
                                    }
                                }

                                if(p.specifications.Count > 0)
                                {
                                    query = @"INSERT INTO product_specifications(category_specification_id, product_id, value)
                                          VALUES(@category_specification_id, @product_id, @value)";

                                    foreach (ProductSpecification specification in p.specifications)
                                    {
                                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                                        {
                                            command.Parameters.AddWithValue("@category_specification_id", specification.category_specification_id);
                                            command.Parameters.AddWithValue("@product_id", p.id);
                                            command.Parameters.AddWithValue("@value", specification.value);

                                            await command.ExecuteNonQueryAsync();
                                        }
                                    }
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
                            try
                            {
                                string query = @"UPDATE products
                                             SET name = @name, price_rsd = @price_rsd,
                                                price_on_sale = @price_on_sale, category_id = @category_id,
                                                stock_quantity = @stock_quantity, description = @description
                                             WHERE id = @id";

                                using (SqlCommand command = new SqlCommand(query, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@name", p.name);
                                    command.Parameters.AddWithValue("@price_rsd", p.price_rsd);
                                    command.Parameters.AddWithValue("@price_on_sale", (p.price_on_sale != null) ? Convert.ToInt32(p.price_on_sale) : DBNull.Value);
                                    command.Parameters.AddWithValue("@category_id", p.category_id);
                                    command.Parameters.AddWithValue("@stock_quantity", p.stock_quantity);
                                    command.Parameters.AddWithValue("@id", p.id);
                                    command.Parameters.AddWithValue("@description", p.description);

                                    await command.ExecuteNonQueryAsync();
                                }

                                if(p.images.Count > 0)
                                {
                                    query = @"UPDATE images
                                              SET image_url = @image_url, is_main_image = @is_main_image
                                              WHERE image_id = @image_id AND product_id = @product_id";

                                    foreach (Image img in p.images)
                                    {
                                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                                        {
                                            command.Parameters.AddWithValue("@image_url", img.url);
                                            command.Parameters.AddWithValue("@is_main_image", img.is_main_image);
                                            command.Parameters.AddWithValue("@image_id", img.id);
                                            command.Parameters.AddWithValue("@product_id", p.id);

                                            await command.ExecuteNonQueryAsync();
                                        }
                                    }
                                }

                                if(p.specifications.Count > 0)
                                {
                                    query = @"UPDATE product_specifications
                                      SET value = @value
                                      WHERE category_specification_id = @category_specification_id AND product_id = @product_id";

                                    foreach (ProductSpecification ps in p.specifications)
                                    {
                                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                                        {
                                            command.Parameters.AddWithValue("@category_specification_id", ps.category_specification_id);
                                            command.Parameters.AddWithValue("@product_id", p.id);
                                            command.Parameters.AddWithValue("@value", ps.value);

                                            await command.ExecuteNonQueryAsync();
                                        }
                                    }
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
