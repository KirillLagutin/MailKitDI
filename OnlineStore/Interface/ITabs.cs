using System.Collections.Concurrent;
using OnlineStore.Models;

namespace OnlineStore.Interface;

public interface ITabs
{
    ConcurrentBag<Tab> GetTabs();
    void AddTabs(Tab tab);
    void DeleteTab(int id);
}