﻿using Account.Application.Commands;
using Account.Domain.Entitys.Account;
using Account.Domain.Entitys.Tokens.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
    public class SignInHandler : IRequestHandler<SignInCommand, IActionResult>
    {
        private readonly IRepository<User, Guid> _accountRepository;
        private readonly ITokenGenerator _tokenGenerator;


        public SignInHandler(IRepository<User, Guid> accountRepository, ITokenGenerator tokenGenerator)
        {
            _accountRepository = accountRepository;
            _tokenGenerator = tokenGenerator;
        }


        public async Task<IActionResult> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = (await _accountRepository
            .GetAllAsync())
                .FirstOrDefault(user => user.Username == request.Username &&
                BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.PasswordHash));


            var accessToken = _tokenGenerator.GenerateAccessToken(user, new List<Claim>
            {
                new Claim(ClaimTypes.Role, Enum.GetName(typeof(Role), Role.User)),
            });
            var refreshToken = _tokenGenerator.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _accountRepository.UpdateAsync(user);

            return new OkObjectResult(new Dictionary<string, string>
            {
                {"AccessToken", accessToken },
                {"RefreshToken", refreshToken },
            });
        }
    }
}
