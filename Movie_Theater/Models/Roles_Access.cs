﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Movie_Theater.Models;

public partial class Roles_Access
{
    public int Role_Id { get; set; }

    public int Access_Id { get; set; }

    public int? Other { get; set; }

    public virtual Access Access { get; set; }

    public virtual Roles Role { get; set; }
}