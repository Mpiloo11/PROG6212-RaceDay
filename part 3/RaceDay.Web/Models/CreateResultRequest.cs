
namespace RaceDay.Web.Models
{
    public class CreateResultRequest
    {
        public int EnrolmentId { get; set; }
        public string FinishTime { get; set; } = "";
        public int Position { get; set; }
    }
}
