using Account.Domain.Entitys.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Account.Domain.Entitys.Tokens.Common
{
    public interface ITokenGenerator
    {
        string GenerateToken(User user, List<Claim> roles);
    }
}
