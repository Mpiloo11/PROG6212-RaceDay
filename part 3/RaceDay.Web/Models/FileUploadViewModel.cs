using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RaceDay.Web.Models
{
    public class FileUploadViewModel
    {
        [Required(ErrorMessage = "Please select a file.")]
        public IFormFile? File { get; set; }

        public string? FileUrl { get; set; }
    }
}
