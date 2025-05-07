using Backend.DTO;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserRepository _userRepository;
    
    public NotificationService(INotificationRepository notificationRepository, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _notificationRepository = notificationRepository;
    }

    public async Task<IEnumerable<Notification>> GetNotificationsAsync()
    {
        var notifications = await _notificationRepository.GetNotificationsAsync();
        return notifications;
    }

    public async Task<Notification?> GetNotificationByIdAsync(int id)
    {
        var notif = await _notificationRepository.GetNotificationByIdAsync(id);
        if (notif == null)
        {
            throw new ModelInvalidException("Notification not found");
        }
        return notif;
    }

    public async Task<int> CreateNotificationAsync(NotificationDTO notification)
    {
        var newNotification = new Notification
        {
            CreatedBy = notification.CreatedBy,
            Message = notification.Message,
            CreatedOn = DateTime.UtcNow,
        };

        if (notification.CreatedBy == null)
        {
            throw new ModelInvalidException("User id is required");
        }

        var user = await _userRepository.GetUserByIdAsync(notification.CreatedBy.Value);
        
        var createdNotificationId = await _notificationRepository.CreateNotificationAsync(newNotification);
        return createdNotificationId;
    }

    public async Task UpdateNotificationAsync(NotificationDTO notification)
    {
        var existingNotification = await _notificationRepository.GetNotificationByIdAsync(notification.Id);
        if (existingNotification == null)
        {
            throw new ModelInvalidException("Notification not found");
        }

        existingNotification.Message = notification.Message;
        existingNotification.CreatedBy = notification.CreatedBy;
        existingNotification.CreatedOn = DateTime.UtcNow;

        await _notificationRepository.UpdateNotificationAsync(existingNotification);
    }

    public async Task DeleteNotificationAsync(int id)
    {
        var existingNotification = await _notificationRepository.GetNotificationByIdAsync(id);
        if (existingNotification == null)
        {
            throw new ModelInvalidException("Notification not found");
        }

        await _notificationRepository.DeleteNotificationAsync(id);
    }
}