﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Movie_Theater.Models;

public partial class User_Habbit
{
    public int Habbit_Id { get; set; }

    public int User_Id { get; set; }

    public int? Other { get; set; }

    public virtual Habbits Habbit { get; set; }

    public virtual Users User { get; set; }
}