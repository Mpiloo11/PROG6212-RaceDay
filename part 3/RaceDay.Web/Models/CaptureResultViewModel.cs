
namespace RaceDay.Web.Models
{
    public class CaptureResultViewModel
    {
        public int EventId { get; set; }
        public int EnrolmentId { get; set; }
        public string FinishTime { get; set; } = "";
        public int Position { get; set; }

        public List<EventEnrolment> Enrolments { get; set; } = new();
    }
}
