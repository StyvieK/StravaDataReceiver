//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StravaConsole
{
    using System;
    using System.Collections.Generic;
    
    public partial class Lap
    {
        public int ActivityId { get; set; }
        public int LapId { get; set; }
        public Nullable<System.DateTime> Start { get; set; }
        public Nullable<int> Id { get; set; }
        public Nullable<int> ResourceState { get; set; }
        public string Name { get; set; }
        public Nullable<int> ElapsedTime { get; set; }
        public Nullable<int> MovingTime { get; set; }
        public Nullable<int> ElapsedTimespan { get; set; }
        public Nullable<int> MovingTimespan { get; set; }
        public Nullable<System.DateTime> StartLocal { get; set; }
        public Nullable<decimal> MaxHeartRate { get; set; }
        public Nullable<decimal> Distance { get; set; }
        public Nullable<int> StartIndex { get; set; }
        public Nullable<int> EndIndex { get; set; }
        public Nullable<decimal> TotalElevationGain { get; set; }
        public Nullable<decimal> AveSpeed { get; set; }
        public Nullable<decimal> MaxSpeed { get; set; }
    }
}