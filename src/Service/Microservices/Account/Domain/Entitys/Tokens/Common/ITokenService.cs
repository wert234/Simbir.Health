using Account.Domain.Entitys.Account;
using System.Security.Claims;
using System.Security.Cryptography;
namespace Account.Domain.Entitys.Tokens.Common
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user, List<Claim> roles);
        string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                var token = Convert.ToBase64String(randomNumber);
                return $"{token}_{Guid.NewGuid()}";
            }
        }
        (bool isSuccess, object result) GetPrincipalFromToken(string token);
    }
}
