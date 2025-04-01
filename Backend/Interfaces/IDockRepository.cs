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
}