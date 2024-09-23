using Account.Domain.Entitys.Account;
using System.Security.Claims;
using System.Security.Cryptography;
namespace Account.Domain.Entitys.Tokens.Common
{
    public interface ITokenGenerator
    {
        string GenerateAccessToken(User user, List<Claim> roles);
        string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        object GetPrincipalFromExpiredToken(string token);
    }
}
