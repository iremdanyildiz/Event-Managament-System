using Microsoft.AspNetCore.Mvc;
using EventManagementSystem.Services;
using EventManagementSystem.Models;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly EventService _eventService;

    public EventController(EventService eventService)
    {
        _eventService = eventService;
    }

    [HttpPost("create")]
    public IActionResult CreateEvent([FromBody] Event ev)
    {
        try
        {
            if (ev == null || string.IsNullOrEmpty(ev.Name))
            {
                return BadRequest("Event bilgileri eksik.");
            }

            _eventService.CreateEvent(ev);
            return Ok(new { message = "Event created successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Bir hata oluştu.", details = ex.Message });
        }
    }

    [HttpGet("all")]
    public IActionResult GetAll()
    {
        try
        {
            var events = _eventService.GetAllEvents();
            return Ok(events);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Eventler alınırken bir hata oluştu.", details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetEventById(int id)
    {
        try
        {
            var ev = _eventService.GetEventById(id);
            if (ev == null)
            {
                return NotFound("Event not found");
            }
            return Ok(ev);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Bir hata oluştu.", details = ex.Message });
        }
    }
}
