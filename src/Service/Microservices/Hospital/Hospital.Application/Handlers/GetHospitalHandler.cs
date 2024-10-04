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
        private readonly IRepository<Domain.Entitys.Hospital, int> _hospitalRepository;

        public GetHospitalHandler(IRepository<Domain.Entitys.Hospital, int> hospitalRepository)
        {
            _hospitalRepository = hospitalRepository;
        }

        public async Task<IActionResult> Handle(GetHospitalQuery request, CancellationToken cancellationToken)
        {
            var hospital = await _hospitalRepository.GetAsync(request.Id);

            return new OkObjectResult(new Dictionary<string, object>
            {
                {"Id", hospital.Id},
                {"Name", hospital.Name},
                {"Address", hospital.Address},
                {"ContactPhone", hospital.ContactPhone},
            });
        }
    }
}
