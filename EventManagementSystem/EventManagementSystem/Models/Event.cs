namespace EventManagementSystem.Models
{
    public class Event
    {
        public int EventID { get; set; }  // Birincil anahtar (PK)
        public string? Title { get; set; }  // Etkinlik başlığı
        public string ?Description { get; set; } // Etkinlik açıklaması
        public DateTime Date { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);// Etkinlik tarihi

        public string? Location { get; set; } // Etkinlik yeri
        public int OrganizerID { get; set; }  // Etkinliği oluşturan kullanıcı ID'si (FK)
       

       
    }
}
