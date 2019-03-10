using System;
using Mentoring.Lab2.Library.Wrappers.Interfaces;

namespace Mentoring.Lab2.Library.Wrappers
{
    public class DateTimeWrapper : IDateTimeWrapper
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
