using Account.Application.Queries;
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
    public class GetAccountsHandler : IRequestHandler<GetAccountsQuery, IActionResult>
    {
        private readonly IRepository<User, Guid> _accountRepository;

        public GetAccountsHandler(IRepository<User, Guid> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            var users = await _accountRepository.GetAllAsync();
            return new OkObjectResult(users.Skip(request.From).Take(request.Count));
        }
    }
}
