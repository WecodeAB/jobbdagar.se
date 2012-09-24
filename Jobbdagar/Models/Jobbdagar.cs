using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jobbdagar.Models
{
    public class Day
    {
        public DateTime Date { get; set; }
        public string DateName { get; set; }
    }

    public class Days
    {
        public int NumberOfDays { get; set; }
        public List<Day> Holidays { get; set; }
    }
}