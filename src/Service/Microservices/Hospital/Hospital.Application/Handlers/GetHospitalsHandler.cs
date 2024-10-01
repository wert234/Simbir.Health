using Hospital.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Application.Handlers
{
    public class GetHospitalsHandler : IRequestHandler<GetHospitalsQuery, IActionResult>
    {
        private readonly IRepository<Domain.Entitys.Hospital, Guid> _hospitalsRepository;

        public GetHospitalsHandler(IRepository<Domain.Entitys.Hospital, Guid> hospitalsRepository)
        {
            _hospitalsRepository = hospitalsRepository;
        }


        public async Task<IActionResult> Handle(GetHospitalsQuery request, CancellationToken cancellationToken)
        {
            return new OkObjectResult((await _hospitalsRepository
                .GetAllAsync())
                .Skip(request.From)
                .Take(request.Count));
        }
    }
}
