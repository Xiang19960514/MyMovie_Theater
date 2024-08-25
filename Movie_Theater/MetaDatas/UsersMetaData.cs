using System.ComponentModel.DataAnnotations;

namespace Movie_Theater.MetaDatas
{
    public class UsersMetaData
    {
        
        [Display(Name = "會員編號")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "名稱為必填欄位")]
        [Display(Name = "會員名稱")]
        public string UserName { get; set; }

 
        [Display(Name = "會員暱稱")]
        public string NickName { get; set; }

        [Required(ErrorMessage = "生日為必填欄位")]
        [Display(Name = "生日")]
        public DateOnly Birthday { get; set; }

        [Required(ErrorMessage = "電話為必填欄位")]
        [RegularExpression(@"^09\d{8,10}$", ErrorMessage = "電話號碼必須以09開頭，且長度必須在10到12個字之間")]
        [Display(Name = "電話號碼")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "性別為必填欄位")]
        [Display(Name = "性別")]
        public string Sex { get; set; }

        [Required(ErrorMessage = "信箱為必填欄位")]
        [EmailAddress(ErrorMessage = "信箱格式有誤")]
        [Display(Name = "信箱")]
        public string Email { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [StringLength(4)]
        [Display(Name = "MBTI型別")]
        public string MBTI { get; set; }

        [Display(Name = "點數")]
        public int? Points { get; set; }

        [Display(Name = "驗證狀態")]
        public bool? EmailConfirm { get; set; }
    }
}
