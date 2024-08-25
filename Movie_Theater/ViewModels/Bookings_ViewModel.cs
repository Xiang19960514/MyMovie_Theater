using Movie_Theater.Models;
using System.ComponentModel.DataAnnotations;

namespace Movie_Theater.ViewModels
{
    public class Bookings_ViewModel
    {
        [Display(Name ="訂單編號")]
        public int BookingId { get; set; }

        [Display(Name ="會員編號")]
        public int? UserId { get; set; }

        [Display(Name ="訂單日期")]
        public DateTime BookingDate { get; set; }

        [Display(Name ="商家編號")]
        public string MerchantTradeNo { get; set; }

        [Display(Name ="訂單狀態")]
        public string BookingStatus { get; set; }

        [Display(Name ="交易編號")]
        public int TransactionId { get; set; }

        [Display(Name ="付款方式")]
        public string PaymentMethod { get; set; }

        [Display(Name ="付款日期")]
        public DateTime PaymentDate { get; set; }

        public IEnumerable<_Detials> detials { get; set; }
        public IEnumerable<_TicketTypes> ticketTypes { get; set; }
        public IEnumerable<_Snacks> snacks { get; set; }
    }

    public class _Detials
    {
        public int? BookingId { get; set; }
        public string AuditoriumName { get; set; }
        public string MovieName { get; set; }
        public int Level { get; set; }
        public string Language { get; set; }
        public string SeatRow { get; set; }
        public int SeatNumber { get; set; }
        public string SeatType { get; set; }
        public DateTime ShowDateTime { get; set; }
    }

    public class _TicketTypes
    {
        public int? BookingId { get; set; }
        public string TypeName { get; set; }
        public int HowManySeatForType { get; set; }
        public int Price { get; set; }
    }

    public class _Snacks
    {
        public int? BookingId { get; set; }
        public string SnackName { get; set; }
        public int Price { get; set; }
    }
}
