using System;

namespace MobileAppAPI.Models
{
    public class Tasks
    {
        public int id { get; set; }
        public string name { get; set; }
        public Nullable<DateTime> date_finish { get; set; }
        public Nullable<DateTime> time_notification { get; set; }
        public bool priority { get; set; }
        public bool repetition { get; set; }
        public bool state { get; set; }
    }
}