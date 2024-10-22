using History.Application.Queries;
using History.Domain.Entitys;
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
    public class GetHistoryHandler : IRequestHandler<GetHistoryQuerу, IActionResult>
    {
        private readonly IRepository<Domain.Entitys.History, int> _repository;


        public GetHistoryHandler(IRepository<Domain.Entitys.History, int> repository)
        {
            _repository = repository;
        }


        public async Task<IActionResult> Handle(GetHistoryQuerу request, CancellationToken cancellationToken)
        {
            return new OkObjectResult((await _repository
                .GetAllAsync())
                .Where(history => history.PacientId == request.UserId)
                .Select(history => new Dictionary<string, object>
                {
                    {"Дата", history.Date },
                    {"Данные", history.Data },
                }));
        }
    }
}
