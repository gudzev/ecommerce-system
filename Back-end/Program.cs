using Microsoft.Data.SqlClient;
using FrontEndCommunicator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://gudzev-store.netlify.app").AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

app.MapGet("/getProducts", () =>
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
                    p.price_on_sale = Convert.ToInt32(reader["price_rsd"]);
                    p.category_id = Convert.ToInt32(reader["category_id"]);
                    p.stock_quantity = Convert.ToInt32(reader["stock_quantity"]);
                    products.Add(p);
                }
            }
        }
    }
    return Results.Json(products);
});

app.MapGet("/getCategories", () =>
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

app.MapGet("/getDeliveryOptions", () =>
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

app.MapPost("/createOrder", (Order o) =>
{
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
                        Console.WriteLine($"Inserting product_id={item.productId}, order_id={orderId}");
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
                return Results.Json(new { success = false, ex.Message });
            }
        }
    }
});

app.Run();