using System.ComponentModel.DataAnnotations;

namespace Webport.ERP.Common.Domain.Contracts.Identity;

public class LoginDto
{
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

public record RefreshTokenRequest(string Token, string RefreshToken);
public record TokenResponse(string Token, string RefreshToken);


