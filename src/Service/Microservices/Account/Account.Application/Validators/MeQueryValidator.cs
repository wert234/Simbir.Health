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
    public class MeQueryValidator : AbstractValidator<MeQuery>
    {
        public MeQueryValidator(IRepository<User,Guid> repository)
        {
            RuleFor(query => query.Id)
                .NotEmpty()
                .WithMessage("Id не должен быть пустым")
                .MustAsync(async (id, cancellation) =>
                {
                    return await repository.GetAsync(id) != null;
                })
                .WithMessage("Некоректный Id");
                
        }
    }
}
