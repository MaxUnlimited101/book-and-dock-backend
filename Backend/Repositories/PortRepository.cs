using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class PortRepository
    {
        private readonly BookAndDockContext _context;

        public PortRepository(BookAndDockContext context)
        {
            _context = context;
        }

        public int Create(Port port)
        {
            _context.Ports.Add(port);
            _context.SaveChanges();
            return port.Id;
        }

        public void Delete(int id)
        {
            var port = _context.Ports.Find(id);
            if (port != null)
            {
                _context.Ports.Remove(port);
                _context.SaveChanges();
            }
        }

        public Port? GetById(int id)
        {
            return _context.Ports
                .Include(p => p.Owner)
                .Include(p => p.DockingSpots)
                .Include(p => p.Locations)
                .Include(p => p.Reviews)
                .Include(p => p.Services)
                .FirstOrDefault(p => p.Id == id);
        }

        public List<Port> GetAll()
        {
            return _context.Ports
                .Include(p => p.Owner)
                .Include(p => p.DockingSpots)
                .Include(p => p.Locations)
                .Include(p => p.Reviews)
                .Include(p => p.Services)
                .ToList();
        }

        public void Update(Port port)
        {
            _context.Ports.Update(port);
            _context.SaveChanges();
        }

        public bool CheckIfExistsById(int id)
        {
            return _context.Ports.Any(p => p.Id == id);
        }
    }
}
