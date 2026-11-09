
namespace RaceDay.Web.Models
{
    public class EventEnrolment
    {
        public int Id { get; set; }
        public string Status { get; set; } = "";
        public string Participant { get; set; } = "";
        public string Email { get; set; } = "";
        public string Category { get; set; } = "";
    }
}
