using Microsoft.Data.SqlClient;
using Backend;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "https://gudzev-store.netlify.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

app.MapGet("/products", () =>
{
    List<Product> products = new List<Product>();

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand("SELECT * FROM products", connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Product p = new Product();
                    p.id = Convert.ToInt32(reader["id"]);
                    p.name = reader["name"].ToString();
                    p.image_url = reader["image_url"].ToString();
                    p.price_rsd = Convert.ToInt32(reader["price_rsd"]);
                    p.price_on_sale = reader["price_on_sale"] != DBNull.Value ? Convert.ToInt32(reader["price_on_sale"]) : null;
                    p.category_id = Convert.ToInt32(reader["category_id"]);
                    p.stock_quantity = Convert.ToInt32(reader["stock_quantity"]);
                    products.Add(p);
                }
            }
        }
    }
    return Results.Json(products);
});

app.MapGet("/categories", () =>
{
    List<Category> categories = new List<Category>();

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using(SqlCommand command = new SqlCommand("SELECT * FROM categories", connection))
        {
            using(SqlDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
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

app.MapGet("/delivery-options", () =>
{
    List<DeliveryOption> deliveryOptions = new List<DeliveryOption>();

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand("SELECT * FROM delivery_options", connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    DeliveryOption d = new DeliveryOption();
                    d.id = Convert.ToInt32(reader["id"]);
                    d.name = reader["name"].ToString();
                    d.price_per_item = Convert.ToInt32(reader["price_per_item"]);
                    d.free_shipping_minimum_value = Convert.ToInt32(reader["free_shipping_minimum_value"]);
                    d.is_default = Convert.ToBoolean(reader["is_default"]);
                    deliveryOptions.Add(d);
                }
            }
        }
    }
    return Results.Json(deliveryOptions);
});

app.MapPost("/add-delivery-option", (DeliveryOption o) =>
{
    try
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = @"INSERT INTO delivery_options(price_per_item, name, free_shipping_minimum_value, is_default)
                         VALUES(@price_per_item, @name, @free_shipping_minimum_value, @is_default)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@price_per_item", o.price_per_item);
                command.Parameters.AddWithValue("@name", o.name);
                command.Parameters.AddWithValue("@free_shipping_minimum_value", o.free_shipping_minimum_value);
                command.Parameters.AddWithValue("@is_default", o.is_default);

                command.ExecuteNonQuery();
            }
        }
        return Results.Ok(new { success = true });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPut(("/update-delivery-option"), (DeliveryOption d) =>
{
    try
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = @"UPDATE delivery_options
                             SET price_per_item = @price_per_item, name = @name, is_default = @is_default, free_shipping_minimum_value = @free_shipping_minimum_value
                             WHERE id = @id";

            using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@price_per_item", d.price_per_item);
                command.Parameters.AddWithValue("@name", d.name);
                command.Parameters.AddWithValue("@is_default", d.is_default);
                command.Parameters.AddWithValue("@free_shipping_minimum_value", d.free_shipping_minimum_value);

                command.ExecuteNonQuery();
            }
        }
        return Results.Ok(new { success = true });
    }
    catch(Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapDelete("/delete-delivery-option/{deliveryOptionID}", (int deliveryOptionID) =>
{
    try
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = @"DELETE FROM delivery_options 
                             WHERE id = @id";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", deliveryOptionID);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return Results.NotFound(new { message = "Delivery option not found." });
                }
            }
        }
        return Results.Ok(new { success = true });
    }
    catch(Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/orders", () =>
{
    List<Order> orders = new List<Order>();

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand("SELECT * FROM Orders WHERE is_fulfilled = 0", connection))
        {
            using(SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Order o = new Order();
                    o.id = Convert.ToInt32(reader["id"]);
                    o.name = reader["name"].ToString();
                    o.surname = reader["surname"].ToString();
                    o.email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null;
                    o.street = reader["street"] != DBNull.Value ? reader["street"].ToString() : null;
                    o.apartment_number = reader["apartment_number"] != DBNull.Value ? reader["apartment_number"].ToString() : null;
                    o.additional = reader["additional"] != DBNull.Value ? reader["additional"].ToString() : null;
                    o.city = reader["city"] != DBNull.Value ? reader["city"].ToString() : null;
                    o.delivery_method_id = Convert.ToInt32(reader["delivery_method_id"]);
                    o.created_at = Convert.ToDateTime(reader["created_at"]);
                    o.is_fullfilled = Convert.ToBoolean(reader["is_fulfilled"]);
                    o.phone_number = reader["phone_number"].ToString();
                    orders.Add(o);
                }
            }
        }
    }
    return Results.Json(orders);
});

app.MapGet("/orders/{id}", (int id) =>
{
    Order order = new Order();

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        string query = @"SELECT orders.id, orders.name, surname, email, street, apartment_number, additional, city, delivery_method_id, created_at, is_fulfilled, phone_number, order_id, product_id, quantity, price_at_purchase, products.name AS product_name, image_url
                         FROM orders
                         JOIN order_items ON order_items.order_id = orders.id
                         JOIN products ON products.id = order_items.product_id
                         WHERE orders.id = @id";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
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
                    order.is_fullfilled = Convert.ToBoolean(reader["is_fulfilled"]);
                    order.phone_number = reader["phone_number"].ToString();
                    order.orderItems = new List<OrderItem>();

                    do
                    {
                        order.orderItems.Add(new OrderItem(
                        Convert.ToInt32(reader["product_id"]),
                        Convert.ToInt32(reader["order_id"]),
                        Convert.ToInt32(reader["quantity"]),
                        Convert.ToInt32(reader["price_at_purchase"]),
                        reader["product_name"].ToString(),
                        reader["image_url"].ToString()));
                    }
                    while (reader.Read());
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

app.MapPost("/add-product", (Product p) =>
{
    try
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = @"INSERT INTO products(name, image_url, price_rsd, price_on_sale, category_id, stock_quantity)
                         VALUES(@name, @image_url, @price_rsd, @price_on_sale, @category_id, @stock_quantity)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", p.name);
                command.Parameters.AddWithValue("@image_url", p.image_url);
                command.Parameters.AddWithValue("@price_rsd", p.price_rsd);
                command.Parameters.AddWithValue("@price_on_sale", p.price_on_sale);
                command.Parameters.AddWithValue("@category_id", p.category_id);
                command.Parameters.AddWithValue("@stock_quantity", p.stock_quantity);

                command.ExecuteNonQuery();
            }
        }
        return Results.Ok(new { success = true });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPost("/add-order", (Order o) =>
{
    if (o.orderItems == null) return Results.Json(new { success = false, errorMessage = "Order items are empty." });

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

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

                    orderId = Convert.ToInt32(command.ExecuteScalar());
                }

                foreach (OrderItem item in o.orderItems)
                {
                    int priceAtPurchase;
                    using (SqlCommand command = new SqlCommand("SELECT MIN(COALESCE(price_on_sale, price_rsd)) FROM products WHERE id = @id", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@id", item.productId);
                        priceAtPurchase = Convert.ToInt32(command.ExecuteScalar());
                    }

                    using (SqlCommand command = new SqlCommand("INSERT INTO order_items(product_id, order_id, quantity, price_at_purchase) VALUES(@product_id, @order_id , @quantity, @price_at_purchase)", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@product_id", Convert.ToInt32(item.productId));
                        command.Parameters.AddWithValue("@order_id", orderId);
                        command.Parameters.AddWithValue("@quantity", Convert.ToInt32(item.quantity));
                        command.Parameters.AddWithValue("@price_at_purchase", priceAtPurchase);

                        command.ExecuteNonQuery();
                    }
                }

                transaction.Commit();

                return Results.Json(new { success = true, orderId });
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                return Results.Json(new { success = false, errorMessage = ex.Message });
            }
        }
    }
});

app.Run();