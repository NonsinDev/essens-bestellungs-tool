using Dapper;
using MySqlConnector;

namespace Backend.Router
{
    public static class BalanceRoutes
    {
        public static void MapBalanceRoutes(this WebApplication app, string connStr)
        {
            app.MapGet("/balance/{ticketId}", async (string ticketId) =>
            {
                try
                {
                    using var conn = new MySqlConnection(connStr);
                    var balance = await conn.QueryFirstOrDefaultAsync<decimal>(
                        "SELECT balance FROM tickets WHERE id = @id;", new { id = ticketId });

                    if (balance == default)
                        return Results.NotFound(new { error = "Ticket not found." });

                    return Results.Ok(new { balance });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GET /balance/{{ticketId}}: {ex}");
                    return Results.Problem("Internal server error: " + ex.Message);
                }
            });

            app.MapPut("/balance/{ticketId}/update/{newBalance}", async (string ticketId, decimal newBalance) =>
            {
                try
                {
                    using var conn = new MySqlConnection(connStr);
                    const string query =
                        "UPDATE tickets SET balance = @balance WHERE id = @id;";

                    int rowsAffected = await conn.ExecuteAsync(query, new { balance = newBalance, id = ticketId });

                    if (rowsAffected == 0)
                        return Results.NotFound(new { error = "Ticket not found." });

                    return Results.Ok(new { message = "Balance updated successfully." });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in PUT /balance/{{ticketId}}/update/{{newBalance}}: {ex}");
                    return Results.Problem("Internal server error: " + ex.Message);
                }
            });

            app.MapPut("/balance/{ticketId}/remove/{amount}", async (string ticketId, decimal amount) =>
            {
                try
                {
                    using var conn = new MySqlConnection(connStr);
                    const string query =
                        "UPDATE tickets SET balance = GREATEST(0, balance - @amount) WHERE id = @id;";

                    int rowsAffected = await conn.ExecuteAsync(query, new { amount, id = ticketId });

                    if (rowsAffected == 0)
                        return Results.NotFound(new { error = "Ticket not found." });

                    return Results.Ok(new { message = "Amount removed successfully. Balance cannot go below zero." });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in PUT /balance/{{ticketId}}/remove/{{amount}}: {ex}");
                    return Results.Problem("Internal server error: " + ex.Message);
                }
            });
        }
    }

    internal class BalanceUpdateRequest
    {
        public required string TicketId { get; set; }
        public required decimal NewBalance { get; set; }
    }
}