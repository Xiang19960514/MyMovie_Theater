﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Movie_Theater.Models;

public partial class BookingSnacks
{
    public int BookingSnackId { get; set; }

    public int? BookingId { get; set; }

    public int? SnackId { get; set; }

    public int Quantity { get; set; }

    public virtual Bookings Booking { get; set; }

    public virtual Snacks Snack { get; set; }
}