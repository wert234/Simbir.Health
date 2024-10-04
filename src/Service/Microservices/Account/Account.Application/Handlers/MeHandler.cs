using Account.Application.Commands;
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
    public class MeHandler : IRequestHandler<MeQuery, IActionResult>
    {
        private readonly IRepository<User, int> _accountRepository;


        public MeHandler(IRepository<User, int> accountRepository)
        {
            _accountRepository = accountRepository;
        }


        public async Task<IActionResult> Handle(MeQuery request, CancellationToken cancellationToken)
        {
            var user = await _accountRepository.GetAsync(request.Id);

            return new OkObjectResult(new Dictionary<string, string>
            {
                { "lastName", user.LastName },
                { "firstName", user.FirstName },
                { "username", user.Username },
            });
        }
    }
}
