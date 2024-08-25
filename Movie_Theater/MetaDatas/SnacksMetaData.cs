using System.ComponentModel.DataAnnotations;

namespace Movie_Theater.MetaDatas
{
    public class SnacksMetaData
    {
        [Display(Name = "點心編號")]
        public int SnackId { get; set; }

        [Display(Name = "點心名稱")]
        public string SnackName { get; set; }

        [Display(Name = "點心圖片")]
        public string SnackImages { get; set; }

        [Display(Name = "價格")]
        public int Price { get; set; }
    }
}
