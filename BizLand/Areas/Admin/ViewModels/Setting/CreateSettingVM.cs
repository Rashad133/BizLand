using System.ComponentModel.DataAnnotations;

namespace BizLand.Areas.Admin.ViewModels
{
    public class CreateSettingVM
    {
        [Required]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
