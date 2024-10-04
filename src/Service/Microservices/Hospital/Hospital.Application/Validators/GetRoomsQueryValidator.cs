﻿using FluentValidation;
using Hospital.Application.Queries;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Application.Validators
{
    public class GetRoomsQueryValidator : AbstractValidator<GetRoomsQuery>
    {
        public GetRoomsQueryValidator(IRepository<Domain.Entitys.Hospital, int> hospitalRepository)
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