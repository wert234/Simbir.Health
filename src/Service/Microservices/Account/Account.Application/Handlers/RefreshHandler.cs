using Account.Application.Commands;
using Account.Domain.Entitys.Account;
using Account.Domain.Entitys.Tokens.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Account.Application.Handlers
{
    public class RefreshHandler : IRequestHandler<RefreshCommand, IActionResult>
    {
        private readonly IRepository<User, Guid> _accountRepository;
        private readonly ITokenService _tokenService;

        public RefreshHandler(IRepository<User, Guid> accountRepository, ITokenService tokenService)
        {
            _accountRepository = accountRepository;
            _tokenService = tokenService;
        }

        public async Task<IActionResult> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var user = (await _accountRepository.GetAllAsync())
                        .FirstOrDefault(u => u.RefreshToken == request.RefreshToken);


            var newAccessToken = _tokenService.GenerateAccessToken(user, user.Roles
                .Select(role => new Claim(ClaimTypes.Role, Enum.GetName(typeof(Role), role)))
                .ToList());
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _accountRepository.UpdateAsync(user);

            return new OkObjectResult(new Dictionary<string, string>
            {
                {"AccessToken", newAccessToken },
                {"RefreshToken", newRefreshToken },
            });
        }
    }
}
