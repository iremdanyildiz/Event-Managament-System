namespace EventManagementSystem.Models
{
    public class Registration
    {
        public int RegistrationID { get; set; }  // Birincil anahtar (PK)
        public int EventID { get; set; }  // Etkinlik ID'si (FK)
        public int UserID { get; set; }  // Kullanıcı ID'si (FK)

        // Navigation Properties
        public required Event Event { get; set; } = null!;
        public required User User { get; set; } = null!;
    }
}