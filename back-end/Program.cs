using Backend.Endpoints;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 120,
                Window = TimeSpan.FromMinutes(1)
            }));
});

var app = builder.Build();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception();

app.UseRouting();
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseRateLimiter();

app.MapProductEndpoints(connectionString);
app.MapCategoryEndpoints(connectionString);
app.MapDeliveryOptionEndpoints(connectionString);
app.MapOrderEndpoints(connectionString);
app.MapProductPagesEndpoints(connectionString);
app.MapCategorySpecificationEndpoints(connectionString);

app.Run();