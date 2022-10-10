using OnlineStore.Interface;

namespace OnlineStore.Services;

public class UTCCurrentTime : ICurrentTime
{
    public DateTime GetCurrentTimeLocal() => DateTime.Now;

    public DateTime GetCurrentTimeUTC() => 
        TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
}