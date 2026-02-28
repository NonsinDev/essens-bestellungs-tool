using Dapper;
using MySqlConnector;
using Backend.Router;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
var app = builder.Build();

string dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "db";
string dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
string dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "essens-bestellungs-tool";
string dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "appuser";
string dbPass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "apppass";

string connStr = $"Server={dbHost};Port={dbPort};Database={dbName};User={dbUser};Password={dbPass};";

// Wait for database to be ready
await WaitForDatabaseAsync(connStr);

// perform a quick sanity query on tickets table to surface errors early
try
{
    using var conn = new MySqlConnection(connStr);
    var count = await conn.QueryFirstAsync<long>("SELECT COUNT(*) FROM tickets;");
    Console.WriteLine($"Tickets table contains {count} row(s)");
}
catch (Exception ex)
{
    Console.WriteLine($"Startup query failed: {ex}");
}

// CORS enabling
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// register endpoints from separate router classes
app.MapTicketRoutes(connStr);
app.MapLoginRoutes(connStr);

app.Run();

async Task WaitForDatabaseAsync(string connStr)
{
    int maxRetries = 30;
    int retryDelay = 2000; // 2 seconds
    
    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            using var conn = new MySqlConnection(connStr);
            await conn.OpenAsync();
            conn.Close();
            Console.WriteLine("Database is ready!");
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database not ready yet ({i + 1}/{maxRetries}): {ex.Message}");
            if (i < maxRetries - 1)
                await Task.Delay(retryDelay);
        }
    }
    
    throw new Exception("Database failed to become available after 30 retries.");
}


