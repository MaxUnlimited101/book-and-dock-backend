using System.Collections.Generic;
using Backend.DTO;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IGuideService
    {
        IEnumerable<GuideDto> GetAllGuides();
        GuideDto? GetGuideById(int id);
        void CreateGuide(GuideDto guide);
        void UpdateGuide(GuideDto updatedGuide);
        void DeleteGuide(int id);
    }
}