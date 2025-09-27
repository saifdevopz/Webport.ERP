using System.Security.Cryptography;
using System.Text;
using Webport.ERP.Identity.Domain.Entities.Role;
using Webport.ERP.Identity.Domain.Entities.Tenant;

namespace Webport.ERP.Identity.Domain.Entities.User;

public sealed class UserM : AggregateRoot
{

    public int UserId { get; set; }
    public int TenantId { get; set; }
    public int RoleId { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiration { get; set; }
    public TenantM? Tenant { get; set; }
    public RoleM? Role { get; set; }

    public static UserM Create(
        int tenantId,
        int roleId,
        string fullName,
        string email,
        string password)
    {
        CreatePasswordHash(
            password,
            out byte[] passwordHash,
            out byte[] passwordSalt);

        UserM model = new()
        {
            TenantId = tenantId,
            RoleId = roleId,
            FullName = fullName,
            Email = email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        model.AddDomainEvent(new UserCreatedDomainEvent(model.TenantId, model.UserId));

        return model;
    }
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new();
        passwordSalt = hmac.Key;
        passwordHash = hmac
                .ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}
