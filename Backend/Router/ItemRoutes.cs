using Backend.Models;
using Dapper;
using MySqlConnector;

namespace Backend.Router
{
    public static class ItemRoutes
    {
        public static void MapItemRoutes(this RouteGroupBuilder group, string conn_str)
        {
            // Get all items
            group.MapGet("/items/all", async () =>
            {
                try
                {
                    using MySqlConnection conn = new MySqlConnection(conn_str);

                    Item items = await conn.QueryFirstAsync<Item>(
                        "SELECT item_id, stand_id, name, price, stock FROM items;");

                    return Results.Ok(items);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GET /items: {ex}");
                    return Results.Problem("Internal server error: " + ex.Message);
                }
            });

            // Get item by ID
            group.MapGet("/items/{item_id}", async (int item_id) =>
            {
                try
                {
                    using MySqlConnection conn = new MySqlConnection(conn_str);

                    Item item = await conn.QueryFirstAsync<Item>(
                        "SELECT item_id, stand_id, name, price, stock FROM items WHERE item_id = @item_id;",
                        new { item_id });

                    if (item == null)
                        return Results.NotFound(new { error = "Item not found." });

                    return Results.Ok(item);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GET /items/{item_id}: {ex}");
                    return Results.Problem("Internal server error: " + ex.Message);
                }
            });

            // Create new item
            group.MapPost("/items/create", async (CreateItemRequest req) =>
            {
                try
                {
                    using MySqlConnection conn = new MySqlConnection(conn_str);

                    int id = await conn.QueryFirstAsync<int>(
                        "INSERT INTO items (stand_id, name, price, stock) VALUES (@stand_id, @name, @price, @stock); SELECT LAST_INSERT_ID();",
                        new { req.stand_id, req.name, req.price, req.stock });

                    return Results.Ok(new
                    {
                        item_id = id,
                        req.stand_id,
                        req.name,
                        req.price,
                        req.stock
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in POST /items: {ex}");
                    return Results.Problem("Internal server error: " + ex.Message);
                }
            });

            // Update stock
            group.MapPatch("/items/{item_id}/stock", async (int item_id, UpdateStockRequest req) =>
            {
                try
                {
                    using MySqlConnection conn = new MySqlConnection(conn_str);

                    Item item = await conn.QueryFirstAsync<Item>(
                        "SELECT item_id, stock FROM items WHERE item_id = @item_id;",
                        new { item_id });

                    if (item == null)
                        return Results.NotFound(new { error = "Item not found." });

                    int new_stock = item.stock + req.adjustment;
                    if (new_stock < 0)
                        return Results.BadRequest(new { error = "Stock cannot be negative." });

                    await conn.ExecuteAsync(
                        "UPDATE items SET stock = @new_stock WHERE item_id = @item_id;",
                        new { item_id, new_stock });

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in PATCH /items/{item_id}/stock: {ex}");
                    return Results.Problem("Internal server error: " + ex.Message);
                }
            });
        }
    }
}
