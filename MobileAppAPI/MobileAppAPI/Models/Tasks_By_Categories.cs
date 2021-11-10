using System;

namespace MobileAppAPI.Models
{
    public class Tasks_By_Categories
    {
        public int id { get; set; }
        public int id_user { get; set; }
        public int id_category { get; set; }
        public Nullable<int> id_task { get; set; }
    }
}