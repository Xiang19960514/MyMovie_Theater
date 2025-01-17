﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Movie_Theater.Models;

public partial class Bookings
{
    public int BookingId { get; set; }

    public int? UserId { get; set; }

    public int? ShowingId { get; set; }

    public DateTime BookingDate { get; set; }

    public string MerchantTradeNo { get; set; }

    public string BookingStatus { get; set; }

    public virtual ICollection<BookingCoupons> BookingCoupons { get; set; } = new List<BookingCoupons>();

    public virtual ICollection<BookingSeats_Detail> BookingSeats_Detail { get; set; } = new List<BookingSeats_Detail>();

    public virtual ICollection<BookingSnacks> BookingSnacks { get; set; } = new List<BookingSnacks>();

    public virtual ICollection<BookingTicketTypes_Detail> BookingTicketTypes_Detail { get; set; } = new List<BookingTicketTypes_Detail>();

    public virtual ICollection<PaymentTransactions> PaymentTransactions { get; set; } = new List<PaymentTransactions>();

    public virtual Shows Showing { get; set; }
}