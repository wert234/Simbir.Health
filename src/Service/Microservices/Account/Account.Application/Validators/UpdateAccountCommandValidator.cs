using Account.Application.Queries;
using Account.Domain.Entitys.Account;
using FluentValidation;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Application.Validators
{
    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountCommandValidator(IRepository<User, Guid> repository)
        {
            RuleFor(user => user.Password)
                .NotEmpty()
                .WithMessage("Пароль обязательное поле");

            RuleFor(user => user.LastName)
                .NotEmpty()
                .WithMessage("Фамилия обязательное поле");

            RuleFor(user => user)
                .MustAsync(async (user, cancellation) =>
                {
                    var existingUser = (await repository
                    .GetAllAsync())
                    .FirstOrDefault(findUser => findUser.Username == user.Username);

                    return existingUser == null || existingUser.Id == user.Id;
                })
                .WithMessage("Этот логин уже занят");

            RuleFor(user => user.Username)
                .NotEmpty()
                .WithMessage("Логин обязательное поле")
                .MinimumLength(4)
                .WithMessage("Логин должен содержать как минимум 4 символа");

            RuleFor(user => user.FirstName)
                .NotEmpty()
                .WithMessage("Имя обязательное поле");

            RuleFor(user => user.Id)
                .NotEmpty()
                .WithMessage("Id обязательное поле")
                .MustAsync(async (id, cancellation) =>
                {
                    var user = await repository.GetAsync(id);

                    return user != null;
                })
                .WithMessage("Некоректный Id");

            RuleFor(user => user.Roles)
                .NotEmpty()
                .WithMessage("Роль обязательное поле");
        }
    }
}
