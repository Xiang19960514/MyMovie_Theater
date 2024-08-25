using Movie_Theater.Models;
using System.ComponentModel.DataAnnotations;

namespace Movie_Theater.MetaDatas
{
    public class Auditoriums_MetaData
    {
        [Display(Name ="影廳編號")]
        public int AuditoriumId { get; set; }

        [Display(Name ="影城編號")]
        public int? Theater_Id { get; set; }

        [Display(Name ="影廳名稱")]
        public string AuditoriumName { get; set; }

        [Display(Name ="座位數")]
        public int TotalSeats { get; set; }

        [Display(Name ="影廳版本")]
        public string AuditoriumType { get; set; }

        [Display(Name ="影城名稱")]
        public virtual Theaters Theater { get; set; }
    }
}
