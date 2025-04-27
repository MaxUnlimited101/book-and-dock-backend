using Backend.Models;
using System.Collections.Generic;

namespace Backend.Interfaces;

public interface IPortRepository
{
    public IEnumerable<Port> GetAllPorts();
    public Port? GetPortById(int id);
    public Port? GetPortByName(string name);
    public int CreatePort(Port port);
    public void UpdatePort(Port port);
    public void DeletePort(int id);
}