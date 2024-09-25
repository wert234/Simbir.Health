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
    public class UpdateHandler : IRequestHandler<UpdateCommand, IActionResult>
    {
        private readonly IRepository<User, Guid> _accountRepository;

        public UpdateHandler(IRepository<User, Guid> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var user = await _accountRepository.GetAsync(request.Id);

            user.FirstName = request.FirstName == "" || request.FirstName == null ? user.FirstName : request.FirstName;
            user.LastName = request.LastName == "" || request.LastName == null ? user.LastName : request.LastName;
            user.PasswordHash = request.Password == "" || request.Password == null ? user.PasswordHash : BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);

            await _accountRepository.UpdateAsync(user);

            return new NoContentResult();
        }
    }
}
