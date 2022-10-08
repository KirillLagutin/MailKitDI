using OnlineStore.Interface;

namespace OnlineStore.Services;

public class UTCCurrentTime : ICurrentTime
{
    public DateTime GetCurrentTimeLocal()
    {
        return DateTime.Now;
    }
    
    public DateTime GetCurrentTimeUTC()
    {
        return TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
    }
}