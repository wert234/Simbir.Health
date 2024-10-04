using Hospital.Application.Commands;
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
    public class UpdateHospitalHandler : IRequestHandler<UpdateHospitalCommand, IActionResult>
    {
        private readonly IRepository<Domain.Entitys.Hospital, int> _hospitalsRepository;


        public UpdateHospitalHandler(IRepository<Domain.Entitys.Hospital, int> hospitalsRepository)
        {
            _hospitalsRepository = hospitalsRepository;
        }


        public async Task<IActionResult> Handle(UpdateHospitalCommand request, CancellationToken cancellationToken)
        {
            var hospital = await _hospitalsRepository.GetAsync(request.Id);

            hospital.Name = request.Name == "" || request.Name == null ? hospital.Name : request.Name;
            hospital.Address = request.Address == "" || request.Address == null ? hospital.Address : request.Address;
            hospital.ContactPhone = request.ContactPhone == "" || request.ContactPhone == null ? hospital.ContactPhone : request.ContactPhone;
            hospital.Rooms = request.Rooms == null ? hospital.Rooms : request.Rooms;

            await _hospitalsRepository.UpdateAsync(hospital);

            return new NoContentResult();
        }
    }

}
