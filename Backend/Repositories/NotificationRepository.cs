using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly BookAndDockContext _context;
    
    public NotificationRepository(BookAndDockContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Notification>> GetNotificationsAsync()
    {
        return await _context.Notifications.ToListAsync();
    }

    public async Task<Notification?> GetNotificationByIdAsync(int id)
    {
        return await _context.Notifications.FindAsync(id);
    }

    public async Task<int> CreateNotificationAsync(Notification notification)
    {
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        return notification.Id;
    }

    public async Task UpdateNotificationAsync(Notification notification)
    {
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteNotificationAsync(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification != null)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }
    }
}