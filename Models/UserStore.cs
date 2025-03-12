namespace TheLab.Models
{
    public class UserStore
    {
        public static List<(string Username, string Password, string Role)> Users { get; } = new()
        {
            ("admin", "1234", "Admin"),
            ("user", "5678", "User")
        };
    }
}
