using Backend.Models;
using Microsoft.Data.SqlClient;

namespace Backend.Endpoints
{
    public static class DeliveryOptionEndpoints
    {
        public static void MapDeliveryOptionEndpoints(this WebApplication app, string connectionString)
        {
            app.MapGet("/delivery-options", async () =>
            {
                List<DeliveryOption> deliveryOptions = new List<DeliveryOption>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM delivery_options", connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                DeliveryOption d = new DeliveryOption();
                                d.id = Convert.ToInt32(reader["id"]);
                                d.name = reader["name"].ToString();
                                d.price_per_item = Convert.ToInt32(reader["price_per_item"]);
                                d.free_shipping_minimum_value = Convert.ToInt32(reader["free_shipping_minimum_value"]);
                                deliveryOptions.Add(d);
                            }
                        }
                    }
                }
                return Results.Json(deliveryOptions);
            });

            app.MapPost("/delivery-options", async (DeliveryOption o) =>
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        string query = @"INSERT INTO delivery_options(price_per_item, name, free_shipping_minimum_value)
                         VALUES(@price_per_item, @name, @free_shipping_minimum_value)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@price_per_item", o.price_per_item);
                            command.Parameters.AddWithValue("@name", o.name);
                            command.Parameters.AddWithValue("@free_shipping_minimum_value", o.free_shipping_minimum_value);

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

            app.MapPut("/delivery-options", async (DeliveryOption d) =>
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        string query = @"UPDATE delivery_options
                             SET price_per_item = @price_per_item, name = @name, free_shipping_minimum_value = @free_shipping_minimum_value
                             WHERE id = @id";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@price_per_item", d.price_per_item);
                            command.Parameters.AddWithValue("@name", d.name);
                            command.Parameters.AddWithValue("@free_shipping_minimum_value", d.free_shipping_minimum_value);
                            command.Parameters.AddWithValue("@id", d.id);

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

            app.MapDelete("/delivery-options/{deliveryOptionID}", async (int deliveryOptionID) =>
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        string query = @"DELETE FROM delivery_options 
                             WHERE id = @id";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", deliveryOptionID);

                            int rowsAffected = await command.ExecuteNonQueryAsync();

                            if (rowsAffected == 0)
                            {
                                return Results.NotFound(new { message = "Delivery option not found." });
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
        }
    }
}
