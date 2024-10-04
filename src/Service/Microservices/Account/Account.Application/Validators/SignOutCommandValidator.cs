using Account.Application.Commands;
using Account.Domain.Entitys.Account;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Application.Validators
{
    public class SignOutCommandValidator : AbstractValidator<SignOutCommand>
    {
        public SignOutCommandValidator(IRepository<User, int> repository)
        {
            RuleFor(user => user.UserId)
                .MustAsync(async (userId, cancellation) =>
                {
                    var user = await repository.GetAsync(userId);

                    return user != null;
                })
                .WithMessage("Неверный id пользователя");
        }
    }
}
