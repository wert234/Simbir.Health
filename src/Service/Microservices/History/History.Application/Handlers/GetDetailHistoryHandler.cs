using History.Application.Queries;
using History.Domain.Entitys;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sherad.Application.Repositories;
using Sherad.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace History.Application.Handlers
{
    public class GetDetailHistoryHandler : IRequestHandler<GetDetailHistoryQuerу, IActionResult>
    {
        private readonly IRepository<Domain.Entitys.History, int> _repository;
        private readonly IRequestClient<GetUserRequest> _userClient;
        private readonly IRequestClient<GetHospitalRequset> _hospitalClient;

        public GetDetailHistoryHandler(
            IRepository<Domain.Entitys.History, int> repository,
            IRequestClient<GetUserRequest> userClient,
            IRequestClient<GetHospitalRequset> hospitalClient)
        {
            _repository = repository;
            _userClient = userClient;
            _hospitalClient = hospitalClient;
        }

        public async Task<IActionResult> Handle(GetDetailHistoryQuerу request, CancellationToken cancellationToken)
        {
            var histories = await _repository.GetAllAsync();
            var result = await Task.WhenAll(histories
                .Where(history => history.PacientId == request.UserId)
                .Select(async history =>
                {
                    var doctorResponse = await _userClient.GetResponse<GetUserResponse>(
                        new GetUserRequest(history.DoctorId));
                    var doctorName = doctorResponse.Message.Data.FirstName + " " +
                                     doctorResponse.Message.Data.LastName.First() + ".";

                    var hospitalResponse = await _hospitalClient.GetResponse<GetHospitalResponse>(
                        new GetHospitalRequset(history.HospitalId));
                    var hospitalName = hospitalResponse.Message.Data.Name;

                    return new Dictionary<string, object>
                    {
                        {"Дата", history.Date },
                        {"Врач", doctorName },
                        {"Больница", hospitalName },
                        {"Данные", history.Data },
                    };
                }));

            return new OkObjectResult(result);
        }
    }
}
