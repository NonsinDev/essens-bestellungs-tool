using Dapper;
using MySqlConnector;
using static Backend.Router.TicketRoutes;

namespace Backend.Router
{
    public static class LoginRoutes
    {
        public static void MapLoginRoutes(this WebApplication app, string connStr)
        {
            app.MapGet("/login", async (LoginRequest req) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(req.TicketId) || string.IsNullOrWhiteSpace(req.Password))
                        return Results.BadRequest(new { error = "Ticket ID and password are required." });

                    using var conn = new MySqlConnection(connStr);
                    const string query =
                        "SELECT id AS Id, first_name AS FirstName, last_name AS LastName, balance AS Balance FROM tickets WHERE id = @id AND password = @pw;";

                    var ticket = await conn.QueryFirstOrDefaultAsync<Ticket>(query, new { id = req.TicketId, pw = req.Password });

                    if (ticket == null)
                        return Results.Unauthorized();

                    return Results.Ok(new
                    {
                        ticket_id = ticket.Id.ToString("D6"),
                        first_name = ticket.FirstName,
                        last_name = ticket.LastName,
                        balance = ticket.Balance
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in POST /login: {ex}");
                    return Results.Problem("Internal server error: " + ex.Message);
                }
            });
        }
    }

    internal class LoginRequest
    {
        public required string TicketId { get; set; }
        public required string Password { get; set; }
    }
}