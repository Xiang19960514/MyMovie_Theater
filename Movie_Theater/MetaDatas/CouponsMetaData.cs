using System.ComponentModel.DataAnnotations;

namespace Movie_Theater.MetaDatas
{
    public class CouponsMetaData
    {
        [Display(Name = "折扣編號")]
        public int CouponID { get; set; }

        [Display(Name = "折扣碼")]
        public string CouponCode { get; set; }

        [Display(Name = "折扣類型")]
        public string DiscountType { get; set; }

        [Display(Name = "折扣價值")]
        public decimal DiscountValue { get; set; }

        [Display(Name = "開始日期")]
        public DateOnly StartDate { get; set; }

        [Display(Name = "到期日期")]
        public DateOnly ExpiryDate { get; set; }

        [Display(Name = "最大使用次數")]
        public int? MaxUsageCount { get; set; }

        [Display(Name = "目前使用次數")]
        public int? CurrentUsageCount { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }
    }
}