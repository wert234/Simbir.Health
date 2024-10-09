using FluentValidation;
using Sherad.Application.Repositories;
using Sherad.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Application.Queries;

namespace Timetable.Application.Validators
{
    public class GetAppointmentsQueryValidator : AbstractValidator<GetAppointmentsQuery>
    {
        public GetAppointmentsQueryValidator(
            IRepository<Domain.Entitys.Timetable, int> timetableRepository)
        {
            RuleFor(timetable => timetable.Id)
                .MustAsync(async (id, c) =>
                {
                    var result = await timetableRepository.GetAsync(id);
                    return result != null;
                })
                .WithMessage("Распесания с таким Id не существует");
        }
    }
}
