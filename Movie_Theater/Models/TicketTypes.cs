﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Movie_Theater.Models;

public partial class TicketTypes
{
    public int TicketTypeId { get; set; }

    public string TypeName { get; set; }

    public int HowManySeatForType { get; set; }

    public string TicketDescription { get; set; }

    public int Price { get; set; }

    public virtual ICollection<BookingTicketTypes_Detail> BookingTicketTypes_Detail { get; set; } = new List<BookingTicketTypes_Detail>();
}