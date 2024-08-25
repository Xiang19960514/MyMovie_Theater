using System.ComponentModel.DataAnnotations;

namespace Movie_Theater.MetaDatas
{
    internal class HabbitsMetaData
    {
        [Display(Name ="興趣編號")]
        public int HabbitId { get; set; }
        [Display(Name = "興趣")]
        public string Habbit { get; set; }
    }
}