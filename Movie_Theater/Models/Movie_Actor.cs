﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Movie_Theater.Models;

public partial class Movie_Actor
{
    public int Movie_Id { get; set; }

    public int Actor_Id { get; set; }

    public int MainLevel { get; set; }

    public virtual Actors Actor { get; set; }

    public virtual Movies Movie { get; set; }
}