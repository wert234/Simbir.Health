using FluentValidation;
using Hospital.Application.Queries;
using Hospital.Domain.Entitys;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Application.Validators
{
    public class GetHospitalQueryValidator : AbstractValidator<GetHospitalQuery>
    {
        public GetHospitalQueryValidator(IRepository<Domain.Entitys.Hospital, int> hospitalRepository)
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
        }
    }
}
