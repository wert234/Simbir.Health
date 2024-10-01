using Account.Application.Commands;
using Account.Application.Queries;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sherad.Domain.Entitys;
using System.Text;
using System.Text.Json;
using IMediator = MediatR.IMediator;

namespace AccountAPI.Consumers
{
    public class AuthConsumer : IConsumer<ValidateTokenRequest>
    {
        private readonly IMediator _mediator;


        public AuthConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }


        public async Task Consume(ConsumeContext<ValidateTokenRequest> context)
        {
            var token = context.Message.Token;

            ObjectResult result = await _mediator.Send(new ValidateQuery(token)) as ObjectResult;

            if(result != null && result.StatusCode == StatusCodes.Status200OK)
            {
                (bool isSuccess, object result) response = ((bool isSuccess, object result))result.Value;
                await context.RespondAsync(new TokenValidationResponse {
                    IsSuccess = response.isSuccess, 
                    Result = response.result 
                });
            }
            else
            {
                await context.RespondAsync(new TokenValidationResponse
                {
                    IsSuccess = false,
                    Result = "Некоректный токен"
                });
            }
        }
    }
}
