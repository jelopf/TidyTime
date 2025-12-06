using System;
using System.Collections.Generic;
using TidyTime.Models;

namespace TidyTime.Services;

public class DayOfWeekService : IDayOfWeekService
{
    public List<DayOfWeekItem> GenerateWeekDays(DateTime selectedDate)
    {
        var weekDays = new List<DayOfWeekItem>();
        var startOfWeek = selectedDate.AddDays(-(int)selectedDate.DayOfWeek + 1);
        var culture = new System.Globalization.CultureInfo("ru-RU");
        
        for (int i = 0; i < 7; i++)
        {
            var date = startOfWeek.AddDays(i);
            weekDays.Add(new DayOfWeekItem
            {
                Date = date,
                DayName = culture.DateTimeFormat.GetAbbreviatedDayName(date.DayOfWeek),
                DayNumber = date.Day.ToString(),
                IsSelected = date.Date == selectedDate.Date,
                IsToday = date.Date == DateTime.Today.Date
            });
        }
        
        return weekDays;
    }
}