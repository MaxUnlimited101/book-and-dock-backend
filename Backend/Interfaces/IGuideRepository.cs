using System.Collections.Generic;
using Backend.Models;
using Backend.Interfaces;

namespace Backend.Interfaces;

public interface IGuideRepository
{
    IEnumerable<Guide> GetAll();
    Guide? GetGuideById(int id);
    void Add(Guide guide);
    void Update(Guide guide);
    void Delete(int id);
}