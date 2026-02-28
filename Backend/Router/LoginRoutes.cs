using Dapper;
using MySqlConnector;
using static Backend.Router.TicketRoutes;

namespace Backend.Router
{
    public static class LoginRoutes
    {
        public static void MapLoginRoutes(this WebApplication app, string connStr)
        {
            app.MapPost("/login", async (LoginRequest req) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(req.ticketId) || string.IsNullOrWhiteSpace(req.password))
                        return Results.BadRequest(new { error = "Ticket ID and password are required." });

                    using var conn = new MySqlConnection(connStr);
                    
                    // First check if ticket id exists
                    const string ticketCheckQuery =
                        "SELECT id AS Id, first_name AS FirstName, last_name AS LastName, balance AS Balance FROM tickets WHERE id = @id;";
                    var ticket = await conn.QueryFirstOrDefaultAsync<Ticket>(ticketCheckQuery, new { id = req.ticketId });

                    if (ticket == null)
                        return Results.Problem(detail: "Ticket ID not found.", statusCode: 404);

                    // Then check password
                    const string passwordCheckQuery =
                        "SELECT COUNT(*) FROM tickets WHERE id = @id AND password = @pw;";
                    long passwordMatch = await conn.QueryFirstAsync<long>(passwordCheckQuery, new { id = req.ticketId, pw = req.password });

                    if (passwordMatch == 0)
                        return Results.Problem(detail: "Wrong password.", statusCode: 401);

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
        public required string ticketId { get; set; }
        public required string password { get; set; }
    }
}