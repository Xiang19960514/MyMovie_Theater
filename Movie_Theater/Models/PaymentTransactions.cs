﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Movie_Theater.Models;

public partial class PaymentTransactions
{
    public int TransactionId { get; set; }

    public int? BookingId { get; set; }

    public string PaymentMethod { get; set; }

    public DateTime PaymentDate { get; set; }

    public virtual Bookings Booking { get; set; }
}