using Backend.DTO;

namespace Backend.Interfaces
{
    public interface IPortService
    {
        int Create(PortDto port);
        void Delete(int id);
    }
}
