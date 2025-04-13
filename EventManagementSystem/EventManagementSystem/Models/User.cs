namespace EventManagementSystem.Models
{
    public class User
    {
        public  int UserID { get; set; }  // Birincil anahtar (PK)
        public required string Name { get; set; }  // Kullanıcının adı
        public required string Email { get; set; } // Kullanıcının email adresi
        public  required string Password { get; set; } // Kullanıcının şifresi
        public required string Role { get; set; }  // Kullanıcının rolü (Admin veya Kullanıcı)
    }
}




