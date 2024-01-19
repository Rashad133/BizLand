using System.ComponentModel.DataAnnotations;

namespace BizLand.Areas.Admin.ViewModels
{
    public class CreateServiceVM
    {
        [Required]
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Icon { get; set; }
    }
}
