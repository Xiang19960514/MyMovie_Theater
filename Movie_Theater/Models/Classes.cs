﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Movie_Theater.Models;

public partial class Classes
{
    public int ClassId { get; set; }

    public string ClassName { get; set; }

    public virtual ICollection<Movie_Class> Movie_Class { get; set; } = new List<Movie_Class>();
}