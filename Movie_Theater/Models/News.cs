﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Movie_Theater.Models;

public partial class News
{
    public int NewsId { get; set; }

    public string NewsType { get; set; }

    public string NewsTitle { get; set; }

    public string NewsDescription { get; set; }

    public string NewsEventNotice { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly ExpiryDate { get; set; }

    public string NewsImage { get; set; }
}