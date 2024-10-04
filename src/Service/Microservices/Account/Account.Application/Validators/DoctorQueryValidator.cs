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
    public class DoctorQueryValidator : AbstractValidator<DoctorQuery>
    {
        public DoctorQueryValidator(IRepository<User, int> repository)
        {
            RuleFor(query => query.Id)
                .NotEmpty()
                .WithMessage("Id обязательное поле")
                .MustAsync(async (id, cancellation) =>
                {
                    var user = await repository.GetAsync(id);

                    return user.Roles.Contains(Enum.GetName(typeof(Role), Role.Doctor));
                })
                .WithMessage("Некоректный Id");
        }
    }
}
