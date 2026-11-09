
namespace RaceDay.Web.Models
{
    public class EnrolmentViewModel
    {
        public int RaceEventId { get; set; }
        public int CategoryId { get; set; }

        public List<Category> Categories { get; set; } = new();
    }
}
