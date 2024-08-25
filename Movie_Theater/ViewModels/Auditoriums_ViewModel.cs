using Movie_Theater.Models;
using System.ComponentModel.DataAnnotations;

namespace Movie_Theater.ViewModels
{
    public class Auditoriums_ViewModel
    {
        [Required(ErrorMessage =("影城為必填欄位"))]
        [Display(Name ="影城名稱")]
        public int? Theater_Id { get; set; }

        [Required(ErrorMessage =("名稱為必填欄位"))]
        [Display(Name ="影廳名稱")]
        public string AuditoriumName { get; set; }

        [Required]
        [Display(Name ="座位數")]
        public int TotalSeats { get; set; }

        [Required(ErrorMessage =("影城為必填欄位"))]
        [Display(Name ="影廳版本")]
        public string AuditoriumType { get; set; }

        [Required]
        public string SeatsJson { get; set; }

        public  IEnumerable<Seats>? Seats { get; set; } = new List<Seats>();
    }
}
