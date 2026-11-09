
using System.ComponentModel.DataAnnotations;

namespace RaceDay.Web.Models
{
    public class EventFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public string Location { get; set; } = "";

        [Range(0.1, 1000)]
        public double DistanceKm { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Please select an event type.")]
        public int EventTypeId { get; set; }

        public List<EventType> EventTypes { get; set; } = new();
    }
}
