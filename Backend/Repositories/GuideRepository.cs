using System.Collections.Generic;
using Backend.Models;
using Backend.Interfaces;
using Backend.Data;

namespace Backend.Repositories;

public class GuideRepository : IGuideRepository
{
    private readonly BookAndDockContext _context;

    public GuideRepository(BookAndDockContext context)
    {
        _context = context;
    }

    public IEnumerable<Guide> GetAll()
    {
        return _context.Guides;
    }

    public Guide? GetGuideById(int id)
    {
        return _context.Guides.Find(id);
    }

    public void Add(Guide guide)
    {
        _context.Guides.Add(guide);
        _context.SaveChanges();
    }

    public void Update(Guide guide)
    {
        _context.Guides.Update(guide);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var guide = GetGuideById(id);
        if (guide != null)
        {
            _context.Guides.Remove(guide);
            _context.SaveChanges();
        }
    }
}