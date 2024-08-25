using System.ComponentModel.DataAnnotations;

namespace Movie_Theater.ViewModels
{
    public class UsersViewModel
    {
        [Display(Name = "會員編號")]
        public int UserId { get; set; }
        [Display(Name = "會員名稱")]          
        public string UserName { get; set; }
        [Display(Name = "會員暱稱")]
        public string NickName { get; set; }
        [Display(Name = "生日")]
        public DateOnly Birthday { get; set; }
        [Display(Name = "電話號碼")]
        public string Phone { get; set; }
        [Display(Name = "性別")]
        public string Sex { get; set; }
        [Display(Name = "信箱")]
        public string Email { get; set; }
        [Display(Name = "地址")]
        public string Address { get; set; }
        [Display(Name = "MBTI型別")]
        public string MBTI { get; set; }    

    }
}
