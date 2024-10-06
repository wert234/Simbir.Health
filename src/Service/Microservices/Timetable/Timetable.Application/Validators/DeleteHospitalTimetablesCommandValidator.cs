using FluentValidation;
using MassTransit;
using Sherad.Application.Repositories;
using Sherad.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Application.Commands;

namespace Timetable.Application.Validators
{
    public class DeleteHospitalTimetablesCommandValidator : AbstractValidator<DeleteHospitalTimetablesCommand>
    {
        public DeleteHospitalTimetablesCommandValidator(
            IRequestClient<GetHospitalRequset> getUserClient,
            IRepository<Domain.Entitys.Timetable, int> timetableRepository)
        {
            RuleFor(timetable => timetable.Id)
                .MustAsync(async (HospitalId, c) =>
                {
                    var result = await getUserClient.GetResponse<GetHospitalResponse>(new GetHospitalRequset(HospitalId));
                    return result.Message.IsExist;
                })
                .WithMessage("Больницы с таким Id не существует");
        }
    }
}
