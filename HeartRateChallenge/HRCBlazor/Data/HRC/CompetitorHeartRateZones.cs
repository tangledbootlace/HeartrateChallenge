﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HRCDB.Data.HRC
{
    public partial class CompetitorHeartRateZones
    {
        public string Username { get; set; }
        public int? Zone1LowerBound { get; set; }
        public int? Zone1UpperBound { get; set; }
        public int? Zone2LowerBound { get; set; }
        public int? Zone2UpperBound { get; set; }
        public int? Zone3LowerBound { get; set; }
        public int? Zone3UpperBound { get; set; }
        public int? Zone4LowerBound { get; set; }
        public int? Zone4UpperBound { get; set; }
        public int? Zone5LowerBound { get; set; }
        public int? Zone5UpperBound { get; set; }

        public virtual Leaderboard UsernameNavigation { get; set; }
    }
}