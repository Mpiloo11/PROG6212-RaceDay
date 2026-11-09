
using System.ComponentModel.DataAnnotations;

namespace RaceDay.Web.Models
{
    public class CategoryFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Category description is required.")]
        public string Description { get; set; } = "";

        [Range(1, int.MaxValue,
            ErrorMessage = "Please select an event.")]
        public int RaceEventId { get; set; }

        public List<Event> Events { get; set; } = new();
    }
}
