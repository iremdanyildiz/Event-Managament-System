using EventManagementSystem.Data;
using EventManagementSystem.Models;

namespace EventManagementSystem.Services
{
    public class EventService
    {
        private readonly AppDbContext _context;

        public EventService(AppDbContext context)
        {
            _context = context;
        }

        // Etkinlik oluşturma
        public void CreateEvent(Event ev)
        {
            ev.Date = DateTime.SpecifyKind(ev.Date, DateTimeKind.Utc);
            _context.Events.Add(ev);
            _context.SaveChanges();
        }


        // Tüm etkinlikleri listeleme
        public List<Event> GetAllEvents()
        {
            return _context.Events.ToList();
        }

        // Etkinlikleri ID'ye göre getirme
        public Event GetEventById(int eventId)
        {
            return _context.Events.Find(eventId);
        }
    }
}