using Account.Application.Queries;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sherad.Domain.DTOs;
using Sherad.Domain.Entitys;
using static MassTransit.Monitoring.Performance.BuiltInCounters;

namespace AccountAPI.Consumers
{
    public class AccountsConsumer : IConsumer<GetUserRequest>
    {
        private readonly IMediator _mediator;


        public AccountsConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }


        public async Task Consume(ConsumeContext<GetUserRequest> context)
        {
            var result = await _mediator.Send(new DoctorQuery(context.Message.Id)) as ObjectResult;

            if(result != null && result.StatusCode == StatusCodes.Status200OK)
            {
                await context.RespondAsync(new GetUserResponse(true, (DoctorDTO)result.Value));
            }
            else
            {
                await context.RespondAsync(new GetUserResponse(false));
            }
        }
    }
}
