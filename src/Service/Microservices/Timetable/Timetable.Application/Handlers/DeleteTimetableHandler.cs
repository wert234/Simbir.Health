using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Application.Commands;

namespace Timetable.Application.Handlers
{
    public class DeleteTimetableHandler : IRequestHandler<DeleteTimetableCommand, IActionResult>
    {
        private readonly IRepository<Domain.Entitys.Timetable, int> _timetableRepository;


        public DeleteTimetableHandler(IRepository<Domain.Entitys.Timetable, int> timetableRepository)
        {
            _timetableRepository = timetableRepository;
        }


        public async Task<IActionResult> Handle(DeleteTimetableCommand request, CancellationToken cancellationToken)
        {
            await _timetableRepository.DeleteAsync(request.Id);

            return new NoContentResult();
        }
    }
}
