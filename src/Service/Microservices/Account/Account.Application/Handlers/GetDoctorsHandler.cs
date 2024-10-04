using Account.Application.Queries;
using Account.Domain.DTOs;
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
    public class GetDoctorsHandler : IRequestHandler<DoctorsQuery, IActionResult>
    {
        private readonly IRepository<User, int> _accountRepository;

        public GetDoctorsHandler(IRepository<User, int> accountRepository)
        {
            _accountRepository = accountRepository;
        }


        public async Task<IActionResult> Handle(DoctorsQuery request, CancellationToken cancellationToken)
        {
            return new OkObjectResult((await _accountRepository
                .GetAllAsync())
                .Where(user => user.Roles.Contains(Enum.GetName(typeof(Role), Role.Doctor)) && 
                (request.NameFilter == "" || request.NameFilter == null ||
                user.LastName.Contains(request.NameFilter) ||
                user.FirstName.Contains(request.NameFilter)))
                .Skip(request.From)
                .Take(request.Count)
                .Select(user => new DoctorDTO
                {
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                })
                .ToList());
        }
    }
}
