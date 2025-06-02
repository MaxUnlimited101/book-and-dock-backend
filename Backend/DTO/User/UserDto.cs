using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.DTO;

public class UserDto
{
    [Range(0, int.MaxValue)]
    public int? Id { get; set; }

    [StringLength(100, MinimumLength = 2)]
    public string? Name { get; set; } = null;

    [StringLength(100, MinimumLength = 2)]
    public string? Surname { get; set; } = null!;

    [EmailAddress]
    [StringLength(255, MinimumLength = 3)]
    public string? Email { get; set; } = null!;

    [Phone]
    public string? PhoneNumber { get; set; }

    public string? Role { get; set; } = "User";
}