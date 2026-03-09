namespace Backend.Class
{
    public class Employee
    {
        public required int employee_id { get; set; }
        public required string username { get; set; }
        public required string password_hash { get; set; }
        public required string first_name { get; set; }
        public required string last_name { get; set; }
        public required string role { get; set; }
        public int? stand_id { get; set; }
        public DateTime created_at { get; set; }
        public bool is_active { get; set; }
    }

    public class EmployeeLoginRequest
    {
        public required string username { get; set; }
        public required string password { get; set; }
    }

    public class CreateEmployeeRequest
    {
        public required string username { get; set; }
        public required string password { get; set; }
        public required string first_name { get; set; }
        public required string last_name { get; set; }
        public required EmployeeRole role { get; set; }
        public int? stand_id { get; set; }
    }

    public enum EmployeeRole
    {
        Admin,
        Cashier,
        StockManager
    }
}
