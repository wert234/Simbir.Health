﻿using Account.Application.Commands;
using Account.Domain.Entitys.Account;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Application.Handlers
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, IActionResult>
    {
        private readonly IRepository<User, int> _accountRepository;

        public SignUpHandler(IRepository<User, int> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _accountRepository.AddAsync(new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Username = request.Username,
                    PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password),
                    Roles = new List<string> { Enum.GetName(typeof(Role), Role.User) },
                });

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message); 
            }
        }
    }
}
