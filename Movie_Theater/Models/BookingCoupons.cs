﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Movie_Theater.Models;

public partial class BookingCoupons
{
    public int BookingCouponID { get; set; }

    public int? BookingID { get; set; }

    public int? CouponID { get; set; }

    public virtual Bookings Booking { get; set; }

    public virtual Coupons Coupon { get; set; }
}