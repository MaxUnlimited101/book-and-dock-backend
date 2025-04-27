using Backend.Models;
using Backend.Data;
using Backend.Interfaces;

namespace Backend.Repositories;

public class PortRepository : IPortRepository
{
    private readonly BookAndDockContext _context;

    public PortRepository(BookAndDockContext context)
    {
        _context = context;
    }

    public int CreatePort(Port port)
    {
        _context.Ports.Add(port);
        _context.SaveChanges();
        return port.Id;
    }

    public void DeletePort(int id)
    {
        var port = _context.Ports.Find(id);
        if (port != null)
        {
            _context.Ports.Remove(port);
            _context.SaveChanges();
        }
    }

    public IEnumerable<Port> GetAllPorts()
    {
        return _context.Ports;
    }

    public Port? GetPortById(int id)
    {
        var port = _context.Ports.Find(id);
        return port;
    }

    public Port? GetPortByName(string name)
    {
        var ports = _context.Ports.Where(p => p.Name == name).ToList();
        if (ports.Count == 0)
        {
            return null;
        }
        else
        {
            return ports[0];
        }
    }

    public void UpdatePort(Port port)
    {
        _context.Ports.Update(port);
        _context.SaveChanges();
    }
}