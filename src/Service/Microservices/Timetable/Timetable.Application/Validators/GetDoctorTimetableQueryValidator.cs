using FluentValidation;
using MassTransit;
using Sherad.Application.Repositories;
using Sherad.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Application.Queries;
using Timetable.Domain.Models;

namespace Timetable.Application.Validators
{
    public class GetDoctorTimetableQueryValidator : AbstractValidator<GetDoctorTimetableQuery>
    {
        public GetDoctorTimetableQueryValidator(
            IRequestClient<GetUserRequest> getUserClient,
            IRepository<Domain.Entitys.Timetable, int> timetableRepository)
        {
            RuleFor(timetable => timetable.Id)
                .MustAsync(async (doctorId, c) =>
                {
                    var result = await getUserClient.GetResponse<GetUserResponse>(new GetUserRequest(doctorId));
                    return result.Message.IsExist;
                })
            .WithMessage("Доктора с таким Id не существует");


            RuleFor(x => x.From)
                   .Must(date =>
                   {
                       if (date != null)
                           return TimetableValidationUtils.IsValidDateTimeFormat((DateTimeOffset)date);
                       else
                           return true;
                   })
                   .WithMessage("Неверный формат даты и времени для From. Формат: yyyy-MM-ddTHH:mm:ssZ.");


            RuleFor(x => x.To)
                       .Must(date =>
                       {
                           if (date != null)
                               return TimetableValidationUtils.IsValidDateTimeFormat((DateTimeOffset)date);
                           else
                               return true;
                       })
                       .WithMessage("Неверный формат даты и времени для To. Формат: yyyy-MM-ddTHH:mm:ssZ.");
        }
    }
}
