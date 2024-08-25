using System.ComponentModel.DataAnnotations;

namespace Movie_Theater.ViewModels
{
    public class Login_ViewModel
    {
        [Required(ErrorMessage = "帳號為必填欄位")]
        [StringLength(20)]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Required(ErrorMessage = "密碼為必填欄位")]
        [Display(Name = "密碼")]
        public string AdminPassword { get; set; }
    }
}
