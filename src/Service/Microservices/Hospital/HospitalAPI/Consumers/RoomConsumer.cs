using Hospital.Application.Queries;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sherad.Domain.DTOs;
using Sherad.Domain.Entitys;

namespace HospitalAPI.Consumers
{
    public class RoomConsumer : IConsumer<GetRoomRequset>
    {
        private readonly IMediator _mediator;


        public RoomConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<GetRoomRequset> context)
        {
            var result = await _mediator.Send(new GetRoomsQuery(context.Message.Id)) as ObjectResult;

            if(result != null && ((List<string>)result.Value).Contains(context.Message.Room))
            {
                await context.RespondAsync(new GetRoomResponse(true));
            }
            else
            {
                await context.RespondAsync(new GetRoomResponse(false));
            }
        }
    }
 }
