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
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;
         private readonly JwtOptions _jwtOptions;


        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtOptions = _configuration.GetSection("JWT").Get<JwtOptions>();
        }


        public string GenerateToken(User user, List<Claim> roles)
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
    }
}
