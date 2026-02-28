using Dapper;
using MySqlConnector;
using System.Text;

namespace Backend.Router
{
    public static class TicketRoutes
    {
        public static void MapTicketRoutes(this WebApplication app, string connStr)
        {
            app.MapGet("/tickets", async () =>
            {
                try
                {
                    using var conn = new MySqlConnection(connStr);
                    var tickets = await conn.QueryAsync<Ticket>(
                        "SELECT id AS Id, first_name AS FirstName, last_name AS LastName, balance AS Balance FROM tickets;");

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
                    const string query =
                        "INSERT INTO tickets (first_name, last_name, password, balance) VALUES (@fn, @ln, @pw, 0); SELECT LAST_INSERT_ID();";

                    long id = await conn.QueryFirstAsync<long>(query, new { fn = req.FirstName, ln = req.LastName, pw = password });

                    return Results.Ok(new
                    {
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
        }

        private static string GeneratePassword()
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

        // request/response models
        public class BookRequest
        {
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
        }

        // record properties names must match SELECT aliases used above
        public record Ticket(long Id, string FirstName, string LastName, decimal Balance);
    }
}
