
namespace RaceDay.Web.Models
{
    public class ParticipantEnrolment
    {
        public int Id { get; set; }
        public string Status { get; set; } = "";
        public DateTime EnrolledUtc { get; set; }
        public string Event { get; set; } = "";
        public string Category { get; set; } = "";
    }
}
