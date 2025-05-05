using Backend.Models;

namespace Backend.Interfaces
{
    public interface IPortRepository
    {
        public int Create(Port port);
        public void Delete(int id);
        public Port? GetById(int id);
        public List<Port> GetAll();
        public void Update(Port port);
        public bool CheckIfExistsById(int id);
    }
}
