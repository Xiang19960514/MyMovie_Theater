using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Movie_Theater.ViewModels
{
    public class MoviesViewModel
    {  
        [Display(Name = "電影編號")]
        public int MovieId { get; set; }

        [Required(ErrorMessage ="名稱為必填")]
        [Display(Name = "電影名稱")]
        public string MovieName { get; set; }

        [Required(ErrorMessage = "英文名稱為必填")]
        [Display(Name = "電影英文名稱")]
        public string MovieEnglishName { get; set; }

        [Required(ErrorMessage = "描述為必填")]
        [Display(Name = "電影描述")]
 
        public string MovieDescription { get; set; }

        [Required(ErrorMessage = "日期為必填")]
        [Display(Name = "上映日期")]
        public DateOnly ReleaseDate { get; set; }

        [Required(ErrorMessage = "片長為必填")]
        [Display(Name = "片長")]
        public int Runtime { get; set; }


        [Required(ErrorMessage ="分級為必填")]
        [Display(Name = "等級分級")]
        public int Level { get; set; }


        [Required(ErrorMessage ="語言為必填")]
        [Display(Name = "語言")]
        public string Language { get; set; }

        [Display(Name = "電影海報")]
        public string? MovieImage { get; set; }

        [Display(Name = "電影預告片")]
        public string? Movievideo { get; set; }


        [Required(ErrorMessage ="導演名稱為必填")]
        [Display(Name = "導演名稱")]
        public string DirectorName { get; set; }


        [Display(Name = "電影上下架狀態")]
        public int MovieState { get; set; }


        [Display(Name = "電影類型編號")]
        public int ClassId { get; set; }

        [ValidateNever]
        [Display(Name = "電影類型")]
        public List<ClassesViewModel> ClassName { get; set; }

        [Display(Name = "演員ID")]
        public int ActorId { get; set; }

        [ValidateNever]
        [Display(Name = "演員名稱")]
        public List<ActorViewModel> ActorName { get; set; }

        
        public string? ActorNameJson { get; set; }
       
        public string? GenreNameJson { get; set; }

    }

    public class Genre
    {
        public string GenreName { get; set; }
    }

    public class Actor
    {
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public int Order { get; set; }
    }
    public class ActorViewModel
    {
        public string ActorName { get; set; }

        public int MainLevel { get; set; }
        
    }

    public class ClassesViewModel
    {
        public string ClassName { get; set; }

    }
}
