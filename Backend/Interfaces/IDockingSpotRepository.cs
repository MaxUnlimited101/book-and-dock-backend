using Backend.Models;

namespace Backend.Interfaces;

public interface IDockingSpotRepository
{
    public Task<List<DockingSpot>> GetAvailableDockingSpotsAsync(
        string? location = null,
        DateTime? date = null,
        decimal? price = null,
        List<string>? services = null,
        string? availability = null);
    
    public bool CheckIfDockingSpotExistsById(int id);

    public DockingSpot? GetDockingSpotById(int id);
    public List<DockingSpot> GetAllDockingSpots();
    public void UpdateDockingSpot(DockingSpot dock);
    public int CreateDockingSpot(DockingSpot dock);
    public void DeleteDockingSpot(int id);
}