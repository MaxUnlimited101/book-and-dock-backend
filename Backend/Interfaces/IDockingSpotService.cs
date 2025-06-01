using Backend.DTO;
using Backend.Models;

namespace Backend.Interfaces;

public interface IDockingSpotService
{
    Task<List<DockingSpot>> GetAvailableDockingSpotsAsync(string? location, DateTime? date, decimal? price, 
        List<string>? services, string? availability);
    
    public bool CheckIfDockingSpotExistsById(int id);
    public DockingSpot? GetDockingSpotById(int id);
    public void CreateDockingSpot(DockingSpotDto ds);
    public void UpdateDockingSpot(DockingSpotDto ds);
    public void DeleteDockingSpot(int id);
}