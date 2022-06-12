using System.ComponentModel.DataAnnotations;

namespace WebApi.Domain.ViewModel;

public class RegisterViewModel
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    [Required] public string EmailAddress { get; set; } = null!;
    [Required] public string UserName { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
    [Required] public string Role { get; set; } = null!;
}