using OnlineStore.Interface;

namespace OnlineStore.Services;

public class Clock : IClock
{
    public DateTime GetCurrentTimeLocal() => DateTime.Now;

    public DateTime GetCurrentTimeUTC() => 
        TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
}