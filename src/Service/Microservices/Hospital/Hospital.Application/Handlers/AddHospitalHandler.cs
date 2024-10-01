using Hospital.Application.Commands;
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
    public class AddHospitalHandler : IRequestHandler<AddHospitalCommand, IActionResult>
    {
        private readonly IRepository<Domain.Entitys.Hospital, Guid> _hospitalRepository;


        public AddHospitalHandler(IRepository<Domain.Entitys.Hospital, Guid> hospitalRepository)
        {
            _hospitalRepository = hospitalRepository;
        }


        public async Task<IActionResult> Handle(AddHospitalCommand request, CancellationToken cancellationToken)
        {
            await _hospitalRepository.AddAsync(new Domain.Entitys.Hospital
            {
                Address = request.Address,
                ContactPhone = request.ContactPhone,
                Name = request.Name,
                Rooms = request.Rooms,
            });

            return new NoContentResult();
        }
    }
}
