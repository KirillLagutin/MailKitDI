namespace OnlineStore.Interface;

public interface ICurrentTime
{
    DateTime GetCurrentTimeLocal();
    DateTime GetCurrentTimeUTC();
}