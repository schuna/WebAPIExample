using System.ComponentModel.DataAnnotations;

namespace WebApi.DataAccess.DTOs.User;

public class UserUpdateDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    [EmailAddress] public string Email { get; set; } = null!;

    private string? _password;

    [MinLength(6)]
    public string? Password
    {
        get => _password;
        set => _password = ReplaceEmptyWithNull(value);
    }

    private string? _confirmPassword;

    [Compare("Password")]
    public string? ConfirmPassword
    {
        get => _confirmPassword;
        set => _confirmPassword = ReplaceEmptyWithNull(value);
    }

    private static string? ReplaceEmptyWithNull(string? value)
    {
        return string.IsNullOrEmpty(value) ? null : value;
    }
}