namespace Tools.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime LinuxStartTime = new DateTime(1970, 1, 1);

        public static long ToUnixTime(this DateTime date)
        {
            return (long) date.Subtract(LinuxStartTime).TotalSeconds;
        }

        public static DateTime FromUnixTime(this long unixTime)
        {
            return LinuxStartTime.AddSeconds(unixTime);
        }
    }
}