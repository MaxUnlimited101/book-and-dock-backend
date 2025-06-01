using Backend.Models;

namespace Backend.Interfaces;

public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetNotificationsAsync();
    Task<Notification?> GetNotificationByIdAsync(int id);
    Task<int> CreateNotificationAsync(Notification notification);
    Task UpdateNotificationAsync(Notification notification);
    Task DeleteNotificationAsync(int id);
}