﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Movie_Theater.Models;

public partial class BookingTicketTypes_Detail
{
    public int TicketTypesDetailId { get; set; }

    public int? Booking_Id { get; set; }

    public int? TicketTypeId { get; set; }

    public virtual Bookings Booking { get; set; }

    public virtual TicketTypes TicketType { get; set; }
}