using Account.Application.Queries;
using Account.Domain.Entitys.Account;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sherad.Application.Repositories;
using Sherad.Domain.DTOs;
using Sherad.Domain.Entitys;

namespace AccountAPI.Consumers
{
    public class UserConsumer : IConsumer<GetUserExistRequest>
    {
        private readonly IMediator _mediator;


        public UserConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<GetUserExistRequest> context)
        {
            var result = await _mediator.Send(new GetAccountsQuery(0, int.MaxValue)) as ObjectResult;

            if (result != null && result.StatusCode == StatusCodes.Status200OK)
            {
                 var user = ((IEnumerable<User>)result.Value).FirstOrDefault(user => user.Id == context.Message.Id);

                if (user != null)
                {
                    await context.RespondAsync(new GetUserExistResponse
                    {
                        isExist = true,
                        Roles = user.Roles
                    });

                    return;
                }
            }

            await context.RespondAsync(new GetUserExistResponse()
            {
                isExist = false,
                Roles = null,
            });
        }
    }
}
