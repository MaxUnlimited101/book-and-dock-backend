using Backend.DTO.Port;

namespace Backend.Interfaces
{
    public interface IPortService
    {
        int Create(PortDto port);
        void Delete(int id);
    }
}
