namespace Backend.Models
{
    public class LoginRequest {
        public required string ticket_id { get; set; }
        public required string username { get; set; }
    }
}