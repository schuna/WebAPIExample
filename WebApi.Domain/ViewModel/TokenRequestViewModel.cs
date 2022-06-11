using System.ComponentModel.DataAnnotations;

namespace WebApi.Domain.ViewModel;

public class TokenRequestViewModel
{
    [Required]
    public string Token { get; set; } = null!;

    [Required]
    public string RefreshToken { get; set; } = null!;
}