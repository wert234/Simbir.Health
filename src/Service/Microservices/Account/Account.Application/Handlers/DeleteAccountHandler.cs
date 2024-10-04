using Account.Application.Commands;
using Account.Domain.Entitys.Account;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Application.Handlers
{
    public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand, IActionResult>
    {
        private readonly IRepository<User, int> _accountRepository;


        public DeleteAccountHandler(IRepository<User, int> accountRepository)
        {
            _accountRepository = accountRepository;
        }


        public async Task<IActionResult> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            await _accountRepository.DeleteAsync(request.Id);

            return new NoContentResult();
        }
    }
}
