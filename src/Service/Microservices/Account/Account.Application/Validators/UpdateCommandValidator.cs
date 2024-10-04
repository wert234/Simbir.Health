using Account.Application.Commands;
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
    public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
    {
        public UpdateCommandValidator(IRepository<User, int> repository) 
        {
            RuleFor(user => user.Password)
                .NotEmpty()
                .WithMessage("Пароль обязательное поле");

            RuleFor(user => user.LastName)
                .NotEmpty()
                .WithMessage("Фамилия обязательное поле");

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
        }
    }
}
