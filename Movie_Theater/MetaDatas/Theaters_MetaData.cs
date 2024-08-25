using Movie_Theater.Models;
using System.ComponentModel.DataAnnotations;

namespace Movie_Theater.MetaDatas
{
    internal class Theaters_MetaData
    {
        public int TheaterId { get; set; }

        [Required(ErrorMessage ="名稱為必填項目")]
        [Display(Name = "影城名稱")]
        public string TheaterName { get; set; }

        [Required(ErrorMessage = "電話為必填項目")]
        [Display(Name = "影城電話")]
        public string TheaterPhone { get; set; }

        [Required(ErrorMessage = "電子郵件為必填項目")]
        [Display(Name = "影城電子郵件")]
        public string TheaterEmail { get; set; }

        [Required(ErrorMessage = "地址為必填項目")]
        [Display(Name = "影城地址")]
        public string TheaterLocation { get; set; }

        [Display(Name = "影城描述")]
        public string TheaterDescription { get; set; }

        [Required(ErrorMessage = "開始時間為必填項目")]
        [Display(Name = "營業開始時間")]
        public TimeOnly TheaterStartTime { get; set; }

        [Required(ErrorMessage = "結束時間為必填項目")]
        [Display(Name = "營業結束時間")]
        public TimeOnly TheaterEndTime { get; set; }

        [Display(Name = "影城圖片")]
        public string TheaterImage { get; set; }

    }
}