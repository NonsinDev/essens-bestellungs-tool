using Dapper;
using MySqlConnector;
using Backend.Models;


namespace Backend.Router
{
    public static class BalanceRoutes
    {
        public static void MapBalanceRoutes(this RouteGroupBuilder group, string conn_str)
        {

            group.MapPut("/balance/{ticket_id}/remove/{amount}", async (BalanceUpdateRequest req) =>
            {
                try
                {
                    using MySqlConnection conn = new MySqlConnection(conn_str);

                    int rows_affected = await conn.ExecuteAsync(
                        "UPDATE users SET balance = balance - @amount WHERE ticket_id = @ticket_id AND balance >= @amount;",
                        new { req.amount, req.ticket_id });

                    if (rows_affected == 0)
                    {
                        var exists = await conn.ExecuteScalarAsync<long>(
                            "SELECT balance FROM users WHERE ticket_id = @ticket_id;",
                            new { req.ticket_id });
                        if (exists == 0)
                            return Results.NotFound(new { error = "User not found." });
                        
                        return Results.BadRequest(new { error = "Insufficient balance." });
                    }

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in PUT /balance/{{ticket_id}}/remove/{{amount}}: {ex}");
                    return Results.Problem("Internal server error: " + ex.Message);
                }
            });


            group.MapPut("/balance/{ticket_id}/add/{amount}", async (BalanceUpdateRequest req) =>
            {
                try
                {
                    using var conn = new MySqlConnection(conn_str);
                    int rows_affected = await conn.ExecuteAsync(
                        "UPDATE users SET balance = balance + @amount WHERE ticket_id = @ticket_id;",
                        new { req.amount, req.ticket_id });

                    if (rows_affected == 0)
                        return Results.NotFound(new { error = "User not found." });

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in PUT /balance/{{ticket_id}}/add/{{amount}}: {ex}");
                    return Results.Problem("Internal server error: " + ex.Message);
                }
            });
        }
    }
}