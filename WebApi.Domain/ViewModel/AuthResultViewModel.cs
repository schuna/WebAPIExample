namespace WebApi.Domain.ViewModel;

public class AuthResultViewModel
{
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime ExpireAt { get; set; }
}