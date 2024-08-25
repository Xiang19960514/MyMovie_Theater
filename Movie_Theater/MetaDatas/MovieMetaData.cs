using System.ComponentModel.DataAnnotations;

namespace Movie_Theater.MetaDatas
{
    public class MovieMetaData
    {
        [Display(Name = "電影編號")]
        public int MovieId { get; set; }


        [Display(Name = "電影名稱")]
        public string MovieName { get; set; }


        [Display(Name = "電影英文名稱")]
        public string MovieEnglishName { get; set; }


        [Display(Name = "電影描述")]
        public string MovieDescription { get; set; }

        [Display(Name = "上映日期")]
        public DateOnly ReleaseDate { get; set; }

        [Display(Name = "片長")]
        public int Runtime { get; set; }

        [Display(Name = "等級分級")]
        public int Level { get; set; }

        [Display(Name = "語言")]
        public string Language { get; set; }

        [Display(Name = "電影海報")]
        public string MovieImage { get; set; }

        [Display(Name = "電影預告片")]
        public string Movievideo { get; set; }

        [Display(Name = "導演名稱")]
        public string DirectorName { get; set; }


        [Display(Name = "電影上下架狀態")]
        public bool MovieState { get; set; }

    }
}
