using System.Collections.Generic;
using Backend.Models;
using Backend.Interfaces;
using Backend.DTO;
using Backend.Exceptions;

namespace Backend.Services;

public class GuideService : IGuideService
{
    private readonly IGuideRepository _guideRepository;
    private readonly IUserRepository _userRepository;

    public GuideService(IGuideRepository guideRepository, IUserRepository userRepository)
    {
        _guideRepository = guideRepository;
        _userRepository = userRepository;
    }

    public void CreateGuide(GuideDto guide)
    {
        Guide newGuide = new();
        var user = _userRepository.GetUserById(guide.CreatedBy);
        if (user == null)
        {
            throw new ModelInvalidException("User not found");
        }
        newGuide.Title = guide.Title;
        newGuide.Content = guide.Content;
        newGuide.CreatedBy = guide.CreatedBy;
        newGuide.CreatedOn = DateTime.UtcNow;
        newGuide.IsApproved = guide.IsApproved;
        newGuide.CreatedByNavigation = user;
        _guideRepository.Add(newGuide);
    }

    public void DeleteGuide(int id)
    {
        var guide = _guideRepository.GetGuideById(id);
        if (guide == null)
        {
            throw new ModelInvalidException("Guide not found");
        }
        _guideRepository.Delete(id);
    }

    public IEnumerable<GuideDto> GetAllGuides()
    {
        var guides = _guideRepository.GetAll();
        List<GuideDto> guideDtos = new();
        foreach (var guide in guides)
        {
            guideDtos.Add(new GuideDto(guide.Id, guide.Title, guide.Content, guide.CreatedBy!.Value, guide.CreatedOn, guide.IsApproved));
        }
        return guideDtos;
    }

    public GuideDto? GetGuideById(int id)
    {
        var guide = _guideRepository.GetGuideById(id);
        if (guide == null)
        {
            return null;
        }
        return new GuideDto(guide.Id, guide.Title, guide.Content, guide.CreatedBy!.Value, guide.CreatedOn, guide.IsApproved);
    }

    public void UpdateGuide(int id, GuideDto updatedGuide)
    {
        var guide = _guideRepository.GetGuideById(id);
        if (guide == null)
        {
            throw new ModelInvalidException("Guide not found");
        }
        guide.Title = updatedGuide.Title;
        guide.Content = updatedGuide.Content;
        guide.CreatedBy = updatedGuide.CreatedBy;
        guide.CreatedByNavigation = _userRepository.GetUserById(updatedGuide.CreatedBy);
        guide.CreatedOn = updatedGuide.CreatedOn;
        guide.IsApproved = updatedGuide.IsApproved;
        _guideRepository.Update(guide);
    }
}