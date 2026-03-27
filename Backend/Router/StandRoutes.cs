using Backend.Models;
using Dapper;
using MySqlConnector;

namespace Backend.Router
{
    public static class StandRoutes
    {
        public static void MapStandRoutes(this RouteGroupBuilder group, string conn_str)
        {
            // Get all stands
            group.MapGet("/stands/all", async () =>
            {
                try
                {
                    using MySqlConnection conn = new MySqlConnection(conn_str);
                    Stand stands = await conn.QueryFirstAsync<Stand>("SELECT stand_adresse, name, pickup_id, tablet_id FROM stands;");

                    return Results.Ok(stands);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GET /stands: {ex}");
                    return Results.Problem("Internal server error: " + ex.Message);
                }
            });

            // Get stand by ID
            group.MapGet("/stands/{stand_adresse}", async (string stand_adresse) =>
            {
                try
                {
                    using MySqlConnection conn = new MySqlConnection(conn_str);
                    Stand stand = await conn.QueryFirstAsync<Stand>("SELECT stand_adresse, name, pickup_id, tablet_id FROM stands WHERE stand_adresse = @stand_adresse;", new { stand_adresse });

                    if (stand == null)
                        return Results.NotFound(new { error = "Stand not found." });

                    return Results.Ok(stand);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GET /stands/{stand_adresse}: {ex}");
                    return Results.Problem("Internal server error: " + ex.Message);
                }
            });

            // Get items by stand
            group.MapGet("/stands/{stand_adresse}/items", async (string stand_adresse) =>
            {
                try
                {
                    using var conn = new MySqlConnection(conn_str);
                    Item items = await conn.QueryFirstAsync<Item>(
                        "SELECT item_id, stand_id, name, price, stock FROM items WHERE stand_id = @stand_adresse;",
                        new { stand_adresse });

                    return Results.Ok(items);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GET /stands/{stand_adresse}/items: {ex}");
                    return Results.Problem("Internal server error: " + ex.Message);
                }
            });

            // Create new stand
            group.MapPost("/stands/create", async (CreateStandRequest req) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(req.name))
                        return Results.BadRequest(new { error = "Stand name is required." });

                    using MySqlConnection conn = new MySqlConnection(conn_str);

                    int id = await conn.QueryFirstAsync<int>("INSERT INTO stands (name, pickup_adresse, tablet_id, category) VALUES (@name, @pickup_id, @tablet_id, @category);SELECT LAST_INSERT_ID();", new { req.name, req.pickup_id, req.tablet_id, req.category });

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in POST /stands: {ex}");
                    return Results.Problem("Internal server error: " + ex.Message);
                }
            });

            // Update stand
            group.MapPut("/stands/{stand_id}", async (int stand_id, UpdateStandRequest req) =>
            {
                return Results.Ok("Needs rework before use.");

                /* try
                {
                    using MySqlConnection conn = new MySqlConnection(conn_str);
                    
                    List<string> updates = new List<string>();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("id", stand_id);



                    if (updates.Count == 0)
                        return Results.BadRequest(new { error = "No fields to update." });

                    string query = $"UPDATE stands SET {string.Join(", ", updates)} WHERE stand_id = @id;";
                    int rows_affected = await conn.ExecuteAsync(query, parameters);

                    if (rows_affected == 0)
                        return Results.NotFound(new { error = "Stand not found." });

                    return Results.Ok(new { message = $"Stand {stand_id} updated successfully." });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in PUT /stands/{stand_id}: {ex}");
                    return Results.Problem("Internal server error: " + ex.Message);
                } */
            });

            // Delete stand
            group.MapDelete("/stands/{stand_id}", async (int stand_id) =>
            {
                return Results.Ok("Needs rework before use.");

                /* try
                {
                    using var conn = new MySqlConnection(conn_str);
                    
                    // Check if stand has items
                    var item_count = await conn.QueryFirstAsync<int>(
                        "SELECT COUNT(*) FROM items WHERE stand_id = @id;",
                        new { id = stand_id });

                    if (item_count > 0)
                        return Results.BadRequest(new { error = $"Cannot delete stand with {item_count} items. Remove items first." });

                    int rows_affected = await conn.ExecuteAsync(
                        "DELETE FROM stands WHERE stand_id = @id;",
                        new { id = stand_id });

                    if (rows_affected == 0)
                        return Results.NotFound(new { error = "Stand not found." });

                    return Results.Ok(new { message = $"Stand {stand_id} deleted successfully." });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in DELETE /stands/{stand_id}: {ex}");
                    return Results.Problem("Internal server error: " + ex.Message);
                } */
            });
        }
    }
}
