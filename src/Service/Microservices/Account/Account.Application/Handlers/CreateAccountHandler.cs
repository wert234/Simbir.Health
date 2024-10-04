using Account.Application.Commands;
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
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, IActionResult>
    {
        private readonly IRepository<User, int> _accountRepository;

        public CreateAccountHandler(IRepository<User, int> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password),
                Roles = request.Roles,
            }; 

            await _accountRepository.AddAsync(user);

            return new CreatedResult();
        }
    }
}
