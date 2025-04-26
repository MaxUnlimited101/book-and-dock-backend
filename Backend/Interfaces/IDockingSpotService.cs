using Backend.DTO;
using Backend.Models;

namespace Backend.Interfaces;

public interface IDockingSpotService
{
    Task<List<DockingSpot>> GetAvailableDocksAsync(string? location, DateTime? date, decimal? price, 
        List<string>? services, string? availability);
    
    public bool CheckIfDockExistsById(int id);
    public DockingSpot? GetDockById(int id);
    public void CreateDock(DockingSpotDto ds);
    public void UpdateDock(DockingSpotDto ds);
    public void DeleteDock(int id);
}