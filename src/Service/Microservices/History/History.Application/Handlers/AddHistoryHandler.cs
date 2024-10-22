using History.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace History.Application.Handlers
{
    public class AddHistoryHandler : IRequestHandler<AddHistoryCommand, IActionResult>
    {
        private readonly IRepository<Domain.Entitys.History, int> _repository;

        public AddHistoryHandler(IRepository<Domain.Entitys.History, int> repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Handle(AddHistoryCommand request, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(new Domain.Entitys.History
            {
                Data = request.Data,
                Date = request.Date,
                DoctorId = request.DoctorId,
                HospitalId = request.HospitalId,
                PacientId = request.PacientId,
                Room = request.Room,
            });

            return new NoContentResult();
        }
    }
}