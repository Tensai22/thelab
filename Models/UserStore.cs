namespace TheLab.Models
{
    public class UserStore
    {
        public static List<(string Username, string Password, string Role)> Users { get; } = new()
        {
            ("admin123", "12345678", "Admin"),
            ("user", "5678", "User")
        };
    }
}
