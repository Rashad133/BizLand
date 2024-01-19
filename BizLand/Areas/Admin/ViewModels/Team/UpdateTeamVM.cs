using BizLand.Models;
using System.ComponentModel.DataAnnotations;

namespace BizLand.Areas.Admin.ViewModels
{
    public class UpdateTeamVM
    {
        [Required]
        public string Name { get; set; }

        public string? Image { get; set; }
        public IFormFile? Photo { get; set; }

        public string? TwitLink { get; set; }
        public string? FaceLink { get; set; }
        public string? InstaLink { get; set; }
        public string? LinkedLink { get; set; }

        public int PositionId { get; set; }
        public List<Position>? Positions { get; set; }
    }
}
