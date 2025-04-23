using Backend.Models;

namespace Backend.Interfaces;

public interface IDockingSpotService
{
    Task<List<DockingSpot>> GetAvailableDocksAsync(string? location, DateTime? date, decimal? price, 
        List<string>? services, string? availability);
    
    public bool CheckIfDockExistsById(int id);
}