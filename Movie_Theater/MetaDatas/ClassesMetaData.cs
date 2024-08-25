using System.ComponentModel.DataAnnotations;

namespace Movie_Theater.MetaDatas
{
    public class ClassesMetaData
    {
        [Display(Name ="類別名稱")]
        public string ClassName { get; set; }
    }
}
