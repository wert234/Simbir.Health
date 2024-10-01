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
    public class GetRoomsHandler : IRequestHandler<GetRoomsQuery, IActionResult>
    {
        private readonly IRepository<Domain.Entitys.Hospital, Guid> _hospitalRepository;


        public GetRoomsHandler(IRepository<Domain.Entitys.Hospital, Guid> hospitalRepository)
        {
            _hospitalRepository = hospitalRepository;
        }


        public async Task<IActionResult> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
        {
            return new OkObjectResult((await _hospitalRepository.GetAsync(request.Id)).Rooms);
        }
    }
}
