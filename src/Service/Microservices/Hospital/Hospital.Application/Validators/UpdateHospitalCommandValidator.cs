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
    public class UpdateHospitalCommandValidator : AbstractValidator<UpdateHospitalCommand>
    {
        public UpdateHospitalCommandValidator(IRepository<Domain.Entitys.Hospital, int> hospitalRepository)
        {
            RuleFor(hospital => hospital.Id)
                 .NotEmpty()
                 .WithMessage("Id обязательное поле")
                 .MustAsync(async (id, cancellation) =>
                 {
                     var hospital = await hospitalRepository.GetAsync(id);

                     return hospital != null;
                 })
                 .WithMessage("Некоректный id больницы");

            RuleFor(hospital => hospital.Name)
                 .NotEmpty()
                 .WithMessage("Имя обязательное поле");

            RuleFor(hospital => hospital)
                  .MustAsync(async (hospital, cancellation) =>
                  {
                      var findHospetal = (await hospitalRepository.GetAllAsync())
                      .FirstOrDefault(findHospetal => findHospetal.Name == hospital.Name);

                      return findHospetal == null || findHospetal.Id == hospital.Id;
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
