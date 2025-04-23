using System;
using System.ComponentModel.DataAnnotations;

public class RegisterRequestDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
