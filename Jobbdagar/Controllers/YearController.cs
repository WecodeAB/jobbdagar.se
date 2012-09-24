using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Jobbdagar.Controllers
{
    using Jobbdagar.Models;

    public class YearController : ApiController
    {
        public Days GetYears(int id, int month = 0)
        {
            int days = 0;
            for(DateTime day = new DateTime(id,1,1); day <=new DateTime(id,12,31); day= day.AddDays(1))
                if(day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                    if(month == 0 || day.Month == month)
                        days++;

            List<Day> holidays = NonWorkingWeekdays(id, month);
            Days d = new Days { NumberOfDays = days - holidays.Count(), Holidays=holidays.OrderBy(day => day.Date).ToList() };

            return d;
        }

        public Days GetAllYears()
        {
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
        }

        private static List<Day> NonWorkingWeekdays(int year, int month)
		{
            /* ALLTID RÖDA/LEDIGA VARDAGAR
             * Långfredag
             * Annandag påsk
             * Kristi himmelsfärd
             * Midsommarafton
             * = 4 st
             * */

            DateTime easterSunday = EasterDay(year);
            List<Day> holidays = new List<Day>();
            holidays.Add(new Day() { Date = easterSunday.AddDays(1), DateName = "Annandag påsk" });
            holidays.Add(new Day() { Date = easterSunday.AddDays(-2), DateName = "Långfredag" });
            holidays.Add(new Day() { Date = easterSunday.AddDays(39), DateName = "Kristi himmelsfärd" });
            holidays.Add(new Day() { Date = MidsummersEve(year), DateName = "Midsommmarafton" });

            int days = 4;

            /* FLYTANDE RÖDA/LEDIGA DAGAR
             * Nyårsdagen
             * Trettondagen
             * Första maj
             * Nationaldagen
             * Julafton
             * Juldagen
             * Annandag jul
             * Nyårsafton
             * = 8
             */

            var newYearsDay = new DateTime(year, 1, 1);
            var epiphany = new DateTime(year, 1, 6);
            var mayFirst = new DateTime(year, 5, 1);
            var nationalDay = new DateTime(year, 6, 6);
            var christmasEve = new DateTime(year, 12, 24);
            var christmasDay = new DateTime(year, 12, 25);
            var christmasBoxing = new DateTime(year, 12, 26);
            var newYearsEve = new DateTime(year, 12, 31);

            if (newYearsDay.DayOfWeek != DayOfWeek.Saturday && newYearsDay.DayOfWeek != DayOfWeek.Sunday)
                holidays.Add(new Day() { Date = newYearsDay, DateName = "Nyårsdagen" });
            if (epiphany.DayOfWeek != DayOfWeek.Saturday && epiphany.DayOfWeek != DayOfWeek.Sunday)
                holidays.Add(new Day() { Date = epiphany, DateName="Trettondagen"});
            if (mayFirst.DayOfWeek != DayOfWeek.Saturday && mayFirst.DayOfWeek != DayOfWeek.Sunday)
                holidays.Add(new Day() { Date = mayFirst, DateName="Första maj"});
            if (nationalDay.DayOfWeek != DayOfWeek.Saturday && nationalDay.DayOfWeek != DayOfWeek.Sunday)
                holidays.Add(new Day() { Date = nationalDay, DateName="Nationaldagen"});
            if (christmasEve.DayOfWeek != DayOfWeek.Saturday && christmasEve.DayOfWeek != DayOfWeek.Sunday)
                holidays.Add(new Day() { Date = christmasEve, DateName="Julafton"});
            if (christmasDay.DayOfWeek != DayOfWeek.Saturday && christmasDay.DayOfWeek != DayOfWeek.Sunday)
                holidays.Add(new Day() { Date = christmasDay, DateName = "Juldagen" });
            if (christmasBoxing.DayOfWeek != DayOfWeek.Saturday && christmasBoxing.DayOfWeek != DayOfWeek.Sunday)
                holidays.Add(new Day() { Date = christmasBoxing, DateName = "Annandag jul" });
            if (newYearsEve.DayOfWeek != DayOfWeek.Saturday && newYearsEve.DayOfWeek != DayOfWeek.Sunday)
                holidays.Add(new Day() { Date = newYearsEve, DateName = "Nyårsafton" });

            if (month != 0 )
                holidays.RemoveAll(item => item.Date.Month != month);

            return holidays;
		}

        private static DateTime EasterDay(int year)
        {
            int month;
            var Y = year;

            if (Y < 100)
            {
                Y = Y + 1900;
            }

            if (Y < 1950)
            {
                Y = Y + 100;
            }

            var A = Y % 19;
            var b = Y % 4;
            var C = Y % 7;
            var D = (19 * A + 24) % 30;
            var E = ((2 * b) + (4 * C) + (6 * D) + 5) % 7;

            var day = 22 + D + E;
            if (day == 57)
            {
                day = day - 7;
            }
            if (day == 56 && D == 28 && E == 6 && A > 10)
            {
                day = day - 7;
            }

            if (day > 31)
            {
                day = day - 31;
                month = 4;
            }
            else
            {
                month = 3;
            }

            return DateTime.Parse((year % 100) + "-" + month + "-" + day);
        }

        private static DateTime MidsummersEve(int year)
        {
            //Midsommarafton alltid mellan 19 och 25 juni
            DateTime d = new DateTime(year, 6, 19);
            while(d.DayOfWeek != DayOfWeek.Friday)
                d = d.AddDays(1);

            return d;
        }
    
    }
}
