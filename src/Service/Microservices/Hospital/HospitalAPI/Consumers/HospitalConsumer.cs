using Hospital.Application.Queries;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sherad.Domain.Entitys;

namespace HospitalAPI.Consumers
{
    public class HospitalConsumer : IConsumer<GetHospitalRequset>
    {
        private readonly IMediator _mediator;


        public HospitalConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<GetHospitalRequset> context)
        {
            var result = await _mediator.Send(new GetHospitalQuery(context.Message.Id)) as ObjectResult;

            if(result != null && result.StatusCode == StatusCodes.Status200OK)
            {
                await context.RespondAsync(new GetHospitalResponse(true));
            }
            else
            {
                await context.RespondAsync(new GetHospitalResponse(false));
            }
        }
    }
}
