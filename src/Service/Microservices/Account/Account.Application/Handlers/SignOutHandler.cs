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
    public class SignOutHandler : IRequestHandler<SignOutCommand, IActionResult>
    {
        private readonly IRepository<User, Guid> _accountRepository;


        public SignOutHandler(IRepository<User, Guid> accountRepository)
        {
            _accountRepository = accountRepository;
        }


        public async Task<IActionResult> Handle(SignOutCommand request, CancellationToken cancellationToken)
        {
            var user = await _accountRepository.GetAsync(request.UserId);

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.MinValue;

            await _accountRepository.UpdateAsync(user);

            return new NoContentResult();
        }
    }
}
