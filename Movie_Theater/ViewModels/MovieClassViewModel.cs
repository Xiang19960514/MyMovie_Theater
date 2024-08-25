using System.ComponentModel.DataAnnotations;

namespace Movie_Theater.ViewModels
{
    public class MovieClassViewModel
    {
        [Display(Name ="電影類型")]
        public string ClassName { get; set; }

        public int Movie_Id { get; set; }

        public int Class_Id { get; set; }
    }
}
