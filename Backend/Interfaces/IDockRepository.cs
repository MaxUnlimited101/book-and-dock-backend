using Backend.Models;

namespace Backend.Interfaces;

public interface IDockRepository
{
    public Task<List<DockingSpot>> GetAvailableDocksAsync(
        string? location = null,
        DateTime? date = null,
        decimal? price = null,
        List<string>? services = null,
        string? availability = null);
    
    public bool CheckIfDockExistsById(int id);

    public DockingSpot? GetDockById(int id);
    public List<DockingSpot> GetAllDocks();
    public void UpdateDock(DockingSpot dock);
    public int CreateDock(DockingSpot dock);
    public void DeleteDock(int id);
}