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
    public class DeleteAccountValidator : AbstractValidator<DeleteAccountCommand>
    {
        public DeleteAccountValidator(IRepository<User, Guid> repository)
        {
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
