using Dapper;
using MySqlConnector;
using System.Linq;
using System.Text;

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
    Console.WriteLine($"✓ tickets table contains {count} row(s)");
}
catch (Exception ex)
{
    Console.WriteLine($"⚠ startup query failed: {ex}");
}

// CORS enabling
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.MapGet("/tickets/ids", async () =>
{
    try
    {
        using var conn = new MySqlConnection(connStr);
        var tickets = await conn.QueryAsync<Ticket>("SELECT id AS Id, first_name AS FirstName, last_name AS LastName, balance AS Balance FROM tickets;");

        var result = tickets.Select(t => new { 
            ticket_id = t.Id.ToString("D6"), 
            first_name = t.FirstName, 
            last_name = t.LastName, 
            balance = t.Balance 
        });

        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in GET /tickets/ids: {ex}");
        return Results.Problem("Internal server error: " + ex.Message);
    }
});

app.MapPost("/tickets/book", async (BookRequest req) =>
{
    try
    {
        if (string.IsNullOrWhiteSpace(req.FirstName) || string.IsNullOrWhiteSpace(req.LastName))
            return Results.BadRequest(new { error = "First name and last name are required." });

        string password = GeneratePassword();

        using var conn = new MySqlConnection(connStr);
        const string query = "INSERT INTO tickets (first_name, last_name, password, balance) VALUES (@fn, @ln, @pw, 0); SELECT LAST_INSERT_ID();";
        
        long id = await conn.QueryFirstAsync<long>(query, new { fn = req.FirstName, ln = req.LastName, pw = password });
        
        return Results.Ok(new { 
            ticket_id = id.ToString("D6"), 
            password = password,
            first_name = req.FirstName,
            last_name = req.LastName
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in POST /tickets/book: {ex}");
        return Results.Problem("Internal server error: " + ex.Message);
    }
});

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
            Console.WriteLine("✓ Database is ready!");
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⏳ Database not ready yet ({i + 1}/{maxRetries}): {ex.Message}");
            if (i < maxRetries - 1)
                await Task.Delay(retryDelay);
        }
    }
    
    throw new Exception("Database failed to become available after 30 retries.");
}

string GeneratePassword()
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    var random = new Random();
    var password = new StringBuilder();
    for (int i = 0; i < 8; i++)
    {
        password.Append(chars[random.Next(chars.Length)]);
    }
    return password.ToString();
}

class BookRequest
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
}

// record properties names must match SELECT aliases used below
record Ticket(long Id, string FirstName, string LastName, decimal Balance);
