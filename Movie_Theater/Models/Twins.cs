﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Movie_Theater.Models;

public partial class Twins
{
    public int User_Id { get; set; }

    public int Invitee_Id { get; set; }

    public int Show_Id { get; set; }

    public int TwinupState { get; set; }

    public virtual Users Invitee { get; set; }

    public virtual Shows Show { get; set; }

    public virtual Users User { get; set; }
}