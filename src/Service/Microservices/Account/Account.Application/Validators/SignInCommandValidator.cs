using Account.Application.Commands;
using Account.Domain.Entitys.Account;
using FluentValidation;
using MediatR;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Application.Validators
{
    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator(IRepository<User, Guid> repository)
        {
            RuleFor(user => user.Password)
                .NotEmpty()
                .WithMessage("Пароль обязательное поле");

            RuleFor(user => user.Username)
                .NotEmpty()
                .WithMessage("Логин обязательное поле");

            RuleFor(user => user)
                .MustAsync(async (user, cancellation) =>
                {
                    var existingUser = (await repository
                    .GetAllAsync())
                    .FirstOrDefault(findUser => findUser.Username == user.Username &&
                     BCrypt.Net.BCrypt.EnhancedVerify(user.Password, findUser.PasswordHash));

                    return existingUser != null;
                })
                .WithMessage("Неправельный логин или пароль");
        }
    }
}
