using System;

public static class DateTimeExtension
{
    private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static DateTime FromUnixTimeSeconds(this long unixTime)
    {
        return Epoch.AddSeconds(unixTime);
    }

    public static long ToUnixTimeSeconds(this DateTime date)
    {
        return Convert.ToInt64((date.ToUniversalTime() - Epoch).TotalSeconds);
    }

    public static DateTime FromUnixTimeMilliseconds(this long unixTime)
    {
        return Epoch.AddMilliseconds(unixTime);
    }

    public static long ToUnixTimeMilliseconds(this DateTime date)
    {
        return Convert.ToInt64((date.ToUniversalTime() - Epoch).TotalMilliseconds);
    }

    public static int DaysBetween(this DateTime date, DateTime lastDay)
    {
        return (int)(date.Date - lastDay.Date).TotalDays;
    }
}
