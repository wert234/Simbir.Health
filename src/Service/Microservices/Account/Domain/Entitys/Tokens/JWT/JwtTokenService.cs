using Account.Domain.Entitys.Account;
using Account.Domain.Entitys.Tokens.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Account.Domain.Entitys.Tokens.JWT
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
         private readonly JwtOptions _jwtOptions;


        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtOptions = _configuration.GetSection("JWT").Get<JwtOptions>();
        }


        public string GenerateAccessToken(User user, List<Claim> roles)
        {
            var token = new JwtSecurityToken(
                issuer:_jwtOptions.Issuer,
                audience:_jwtOptions.Audience,
                claims: roles,
                expires: DateTime.Now.AddSeconds(int.Parse(_jwtOptions.ExpirationSeconds)),
                notBefore: DateTime.Now,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.SigningKey)),
                    SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public (bool isSuccess, object result) GetPrincipalFromToken(string token)
        {
            var principal = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            ClaimsPrincipal claimsPrincipal;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = 
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
                ValidateLifetime = false,
            };

            try
            {
                claimsPrincipal = principal.ValidateToken(token, tokenValidationParameters, out validatedToken);
            }
            catch (Exception ex)
            {

                return (isSuccess: false, result: ex.Message);
            }

            if(validatedToken == null ||
                !(validatedToken as JwtSecurityToken).Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase) ||
                validatedToken.ValidTo < DateTime.UtcNow)
            {
                return (isSuccess: false, result: "Некоректный токен");
            }


            return (isSuccess: true, result: claimsPrincipal.Claims
                .Where(claim => claim.Type == ClaimTypes.Role)
                .Select(role => role.Value)
                .ToArray());
        }
    }
}
