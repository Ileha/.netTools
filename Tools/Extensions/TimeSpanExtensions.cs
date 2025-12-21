using System;

namespace Tools.Extensions
{
    public static class TimeSpanExtensions
    {
        private const long NanosecondsPerMillisecond = 1000000;

        private static readonly double
            Nanoseconds2Ticks = (double) TimeSpan.TicksPerMillisecond / NanosecondsPerMillisecond;

        public static TimeSpan FromNanoseconds(double nanoseconds)
        {
            var ticks = (long) Math.Round(nanoseconds * Nanoseconds2Ticks);

            return TimeSpan.FromTicks(ticks);
        }
    }
}