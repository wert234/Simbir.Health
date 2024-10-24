﻿using FluentValidation;
using History.Application.Queries;
using MassTransit;
using MassTransit.Clients;
using Sherad.Application.Repositories;
using Sherad.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MassTransit.ValidationResultExtensions;

namespace History.Application.Validators
{
    public class GetHistoryQueryValidator : AbstractValidator<GetHistoryQuery>
    {
        public GetHistoryQueryValidator(
            IRepository<Domain.Entitys.History, int> historyRepository,
            IRequestClient<GetUserExistRequest> client)
        {
            RuleFor(query => query.UserId)
                .MustAsync(async (id, c) =>
                {
                    var result = (await client.GetResponse<GetUserExistResponse>(new GetUserExistRequest(id))).Message;
                    return result.isExist;
                })
                .WithMessage("Пользователя не существует")
                .DependentRules(() =>
                {
                    RuleFor(query => query)
                        .MustAsync(async (query, c) =>
                        {
                            var result = (await client.GetResponse<GetUserExistResponse>(new GetUserExistRequest(query.RequestUserId))).Message;

                            if (result.Roles.Contains("User"))
                            {
                                return query.UserId == query.RequestUserId; 
                            }
                            else
                            {
                                return true;
                            }
                        })
                        .WithMessage("Нет доступа");
                });
        }
    }
}
