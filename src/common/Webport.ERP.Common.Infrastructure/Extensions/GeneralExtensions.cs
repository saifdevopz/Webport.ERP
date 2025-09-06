using System.Security.Cryptography;
using System.Text;

namespace Webport.ERP.Common.Infrastructure.Extensions;
public static class GeneralExtensions
{
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new();
        passwordSalt = hmac.Key;
        passwordHash = hmac
                .ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public static string BuildConnectionString(string databaseName)
    {
        //return $"Server=127.0.0.1;Port=5432;Database={databaseName};User Id=postgres;Password=25122000;";
        return $"Host=102.214.9.184;Port=5432;Database={databaseName};Username=postgres;Password=25122000SK;Pooling=true;MinPoolSize=10;MaxPoolSize=100;Include Error Detail=true";
    }
}


