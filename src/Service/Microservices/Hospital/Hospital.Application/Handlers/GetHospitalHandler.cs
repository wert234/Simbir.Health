using Hospital.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Application.Handlers
{
    public class GetHospitalHandler : IRequestHandler<GetHospitalQuery, IActionResult>
    {
        private readonly IRepository<Domain.Entitys.Hospital, Guid> _hospitalRepository;

        public GetHospitalHandler(IRepository<Domain.Entitys.Hospital, Guid> hHospitalRepository)
        {
            _hospitalRepository = hHospitalRepository;
        }

        public async Task<IActionResult> Handle(GetHospitalQuery request, CancellationToken cancellationToken)
        {
            return new OkObjectResult(await _hospitalRepository.GetAsync(request.Id));
        }
    }
}
