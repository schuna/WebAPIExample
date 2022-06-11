using System.ComponentModel.DataAnnotations;

namespace WebApi.Domain.ViewModel;

public class LoginViewModel
{
    [Required] public string EmailAddress { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}