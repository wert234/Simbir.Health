using FluentValidation;
using Hospital.Application.Commands;
using Hospital.Application.Queries;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Application.Validators
{
    public class AddHospitalCommandValidator : AbstractValidator<AddHospitalCommand>
    {
        public AddHospitalCommandValidator(IRepository<Domain.Entitys.Hospital, int> hospitalRepository)
        {
            RuleFor(hospital => hospital.Name)
                .NotEmpty()
                .WithMessage("Имя обязательное поле")
                .MustAsync(async (name, cancellation) =>
                {
                    var hospetal = (await hospitalRepository.GetAllAsync())
                    .FirstOrDefault(hospetal => hospetal.Name == name);

                    return hospetal == null;
                })
                .WithMessage("Больница с таким именем уже существует");

            RuleFor(hospital => hospital.Address)
                .NotEmpty()
                .WithMessage("Адресс обязательное поле");

            RuleFor(hospital => hospital.ContactPhone)
                .NotEmpty()
                .WithMessage("Номер телефон обязательное поле");

            RuleFor(hospital => hospital.Rooms)
                .NotEmpty()
                .WithMessage("Кабинеты обязательное поле");
        }
    }
}
