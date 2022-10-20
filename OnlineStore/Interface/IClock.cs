namespace OnlineStore.Interface;

public interface IClock
{
    DateTime GetCurrentTimeLocal();
    DateTime GetCurrentTimeUTC();
}