using System.ComponentModel.DataAnnotations;

namespace Movie_Theater.MetaDatas
{
    public class TicketTypesMetaData
    {
        [Display(Name="票種編號")]
        public int TicketTypeId { get; set; }

        [Display(Name = "票種名稱")]
        public string TypeName { get; set; }

        [Display(Name = "票種座位數")]
        public int HowManySeatForType { get; set; }

        [Display(Name = "票種描述")]
        public string TicketDescription { get; set; }

        [Display(Name = "價格")]
        public int Price { get; set; }
    }
}
