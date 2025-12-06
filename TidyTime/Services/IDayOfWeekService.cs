using System;
using System.Collections.Generic;
using TidyTime.Models;

namespace TidyTime.Services
{
    public interface IDayOfWeekService
    {
        List<DayOfWeekItem> GenerateWeekDays(DateTime selectedDate);
    }
}