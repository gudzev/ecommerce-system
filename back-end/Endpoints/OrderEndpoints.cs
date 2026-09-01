using Backend.Models;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;

namespace Backend.Endpoints
{
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this WebApplication app, string connectionString)
        {
            app.MapGet("/orders", async (int is_fulfilled) =>
            {
                List<Order> orders = new List<Order>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"SELECT orders.id, orders.name, surname, email, street, apartment_number, additional, city, delivery_method_id, created_at, is_fulfilled, phone_number, order_id, order_items.product_id, quantity, price_at_purchase, products.name AS product_name, images.image_url
                         FROM orders
                         JOIN order_items ON order_items.order_id = orders.id
                         JOIN products ON products.id = order_items.product_id
                         JOIN images ON products.id = images.product_id
                         WHERE images.is_main_image = 1 AND is_fulfilled = @is_fulfilled";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@is_fulfilled", is_fulfilled);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            Dictionary<int, Order> ordersDict = new Dictionary<int, Order>();

                            while (await reader.ReadAsync())
                            {
                                int orderId = Convert.ToInt32(reader["id"]);

                                if (!ordersDict.ContainsKey(orderId))
                                {
                                    Order o = new Order
                                    {
                                        id = orderId,
                                        name = reader["name"].ToString(),
                                        surname = reader["surname"].ToString(),
                                        email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null,
                                        street = reader["street"] != DBNull.Value ? reader["street"].ToString() : null,
                                        apartment_number = reader["apartment_number"] != DBNull.Value ? reader["apartment_number"].ToString() : null,
                                        additional = reader["additional"] != DBNull.Value ? reader["additional"].ToString() : null,
                                        city = reader["city"] != DBNull.Value ? reader["city"].ToString() : null,
                                        delivery_method_id = Convert.ToInt32(reader["delivery_method_id"]),
                                        created_at = Convert.ToDateTime(reader["created_at"]),
                                        is_fulfilled = Convert.ToBoolean(reader["is_fulfilled"]),
                                        phone_number = reader["phone_number"].ToString(),
                                        orderItems = new ObservableCollection<OrderItem>()
                                    };

                                    ordersDict.Add(orderId, o);
                                }

                                ordersDict[orderId]?.orderItems?.Add(new OrderItem(
                                    Convert.ToInt32(reader["product_id"]),
                                    Convert.ToInt32(reader["order_id"]),
                                    Convert.ToInt32(reader["quantity"]),
                                    Convert.ToInt32(reader["price_at_purchase"]),
                                    reader["product_name"].ToString() ?? "Product Name not found",
                                    reader["image_url"].ToString() ?? "Image not found"
                                ));
                            }

                            orders = ordersDict.Values.ToList();
                        }
                    }
                }
                return Results.Json(orders);
            });

            app.MapGet("/orders/{id}", async (int id) =>
            {
                Order order = new Order();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"SELECT orders.id, orders.name, surname, email, street, apartment_number, additional, city, delivery_method_id, created_at, is_fulfilled, phone_number, order_id, product_id, quantity, price_at_purchase, products.name AS product_name, image_url
                         FROM orders
                         JOIN order_items ON order_items.order_id = orders.id
                         JOIN products ON products.id = order_items.product_id
                         WHERE orders.id = @id AND is_fulfilled = 0";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                order.id = Convert.ToInt32(reader["id"]);
                                order.name = reader["name"].ToString();
                                order.surname = reader["surname"].ToString();
                                order.email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null;
                                order.street = reader["street"] != DBNull.Value ? reader["street"].ToString() : null;
                                order.apartment_number = reader["apartment_number"] != DBNull.Value ? reader["apartment_number"].ToString() : null;
                                order.additional = reader["additional"] != DBNull.Value ? reader["additional"].ToString() : null;
                                order.city = reader["city"] != DBNull.Value ? reader["city"].ToString() : null;
                                order.delivery_method_id = Convert.ToInt32(reader["delivery_method_id"]);
                                order.created_at = Convert.ToDateTime(reader["created_at"]);
                                order.is_fulfilled = Convert.ToBoolean(reader["is_fulfilled"]);
                                order.phone_number = reader["phone_number"].ToString();
                                order.orderItems = new ObservableCollection<OrderItem>();

                                do
                                {
                                    order?.orderItems?.Add(new OrderItem(
                                    Convert.ToInt32(reader["product_id"]),
                                    Convert.ToInt32(reader["order_id"]),
                                    Convert.ToInt32(reader["quantity"]),
                                    Convert.ToInt32(reader["price_at_purchase"]),
                                    reader["product_name"].ToString() ?? "Product Name not found",
                                    reader["image_url"].ToString() ?? "Image not found"));
                                }
                                while (await reader.ReadAsync());
                            }
                            else
                            {
                                return Results.NotFound();
                            }
                        }
                    }
                }
                return Results.Json(order);
            });

            app.MapPost("/orders", async (Order o) =>
            {
                if (o.orderItems == null) return Results.Json(new { success = false, errorMessage = "Order items are empty." });

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string query = @"INSERT INTO orders (email, name, surname, street, apartment_number, city, additional, phone_number, delivery_method_id, created_at)
                                 OUTPUT INSERTED.id
                                 VALUES (@email, @name, @surname, @street, @apartment_number, @city, @additional, @phone_number, @delivery_method_id, @created_at)";
                            int orderId;

                            using (SqlCommand command = new SqlCommand(query, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@email", o.email);
                                command.Parameters.AddWithValue("@name", o.name);
                                command.Parameters.AddWithValue("@surname", o.surname);
                                command.Parameters.AddWithValue("@street", o.street);
                                command.Parameters.AddWithValue("@apartment_number", o.apartment_number);
                                command.Parameters.AddWithValue("@city", o.city);
                                command.Parameters.AddWithValue("@additional", o.additional);
                                command.Parameters.AddWithValue("@phone_number", o.phone_number);
                                command.Parameters.AddWithValue("@delivery_method_id", Convert.ToInt32(o.delivery_method_id));
                                command.Parameters.AddWithValue("@created_at", DateTime.Now);

                                orderId = Convert.ToInt32(await command.ExecuteScalarAsync());
                            }

                            foreach (OrderItem item in o.orderItems)
                            {
                                int priceAtPurchase;
                                using (SqlCommand command = new SqlCommand("SELECT MIN(COALESCE(price_on_sale, price_rsd)) FROM products WHERE id = @id", connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@id", item.productId);
                                    priceAtPurchase = Convert.ToInt32(await command.ExecuteScalarAsync());
                                }

                                using (SqlCommand command = new SqlCommand("INSERT INTO order_items(product_id, order_id, quantity, price_at_purchase) VALUES(@product_id, @order_id , @quantity, @price_at_purchase)", connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@product_id", Convert.ToInt32(item.productId));
                                    command.Parameters.AddWithValue("@order_id", orderId);
                                    command.Parameters.AddWithValue("@quantity", Convert.ToInt32(item.quantity));
                                    command.Parameters.AddWithValue("@price_at_purchase", priceAtPurchase);

                                    await command.ExecuteNonQueryAsync();
                                }
                            }

                            transaction.Commit();

                            return Results.Json(new { success = true, orderId });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return Results.Json(new { success = false, errorMessage = ex.Message });
                        }
                    }
                }
            });

            app.MapPatch("/orders", async (Order o) =>
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        string query = @"UPDATE orders 
                             SET is_fulfilled = 1
                             WHERE id = @id";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", o.id);

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
