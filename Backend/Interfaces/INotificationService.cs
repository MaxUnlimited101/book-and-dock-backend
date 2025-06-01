using Backend.DTO;
using Backend.Models;

namespace Backend.Interfaces;

public interface INotificationService
{
    Task<IEnumerable<Notification>> GetNotificationsAsync();
    Task<Notification?> GetNotificationByIdAsync(int id);
    Task<int> CreateNotificationAsync(NotificationDTO notification);
    Task UpdateNotificationAsync(NotificationDTO notification);
    Task DeleteNotificationAsync(int id);
}