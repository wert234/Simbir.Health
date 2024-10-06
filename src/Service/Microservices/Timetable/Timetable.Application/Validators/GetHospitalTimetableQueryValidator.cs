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
using Timetable.Application.Queries;
using Timetable.Domain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Timetable.Application.Validators
{
    public class GetHospitalTimetableQueryValidator : AbstractValidator<GetHospitalTimetableQuery>
    {
        public GetHospitalTimetableQueryValidator(
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

            
                RuleFor(x => x.From)
                       .Must(date =>
                       {
                           var t = TimetableValidationUtils.IsValidDateTimeFormat((DateTimeOffset)date);
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
