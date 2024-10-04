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
    public class DeleteHospitalHandler : IRequestHandler<DeleteHospitalCommand, IActionResult>
    {
        private readonly IRepository<Domain.Entitys.Hospital, int> _hospitalRepository;


        public DeleteHospitalHandler(IRepository<Domain.Entitys.Hospital, int> hospitalRepository)
        {
            _hospitalRepository = hospitalRepository;
        }


        public async Task<IActionResult> Handle(DeleteHospitalCommand request, CancellationToken cancellationToken)
        {
            await _hospitalRepository.DeleteAsync(request.Id);

            return new NoContentResult();
        }
    }
}
