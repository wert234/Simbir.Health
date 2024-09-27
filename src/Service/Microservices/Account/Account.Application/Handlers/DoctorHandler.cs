﻿using Account.Application.Queries;
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
    public class DoctorHandler : IRequestHandler<DoctorQuery, IActionResult>
    {
        private readonly IRepository<User, Guid> _accountRepository;


        public DoctorHandler(IRepository<User, Guid> accountRepository)
        {
            _accountRepository = accountRepository;
        }


        public async Task<IActionResult> Handle(DoctorQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _accountRepository.GetAsync(request.Id);
            return new OkObjectResult(new DoctorDTO
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
            });
        }
    }
}