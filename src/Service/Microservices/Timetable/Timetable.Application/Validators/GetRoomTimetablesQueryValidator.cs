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
using Timetable.Domain.Entitys;
using Timetable.Domain.Models;

namespace Timetable.Application.Validators
{
    public class GetRoomTimetablesQueryValidator : AbstractValidator<GetRoomTimetablesQuery>
    {
        public GetRoomTimetablesQueryValidator(
            IRequestClient<GetHospitalRequset> getUserClient,
            IRequestClient<GetRoomRequset> getRoomClient,
            IRepository<Domain.Entitys.Timetable, int> timetableRepository)
        {
            RuleFor(timetable => timetable.Id)
                .MustAsync(async (HospitalId, c) =>
                {
                    var result = await getUserClient.GetResponse<GetHospitalResponse>(new GetHospitalRequset(HospitalId));
                    return result.Message.IsExist;
                })
                .WithMessage("Больницы с таким Id не существует");

             RuleFor(timetable => timetable)
                   .MustAsync(async (query, c) =>
                   {
                       var result = await getUserClient.GetResponse<GetHospitalResponse>(new GetHospitalRequset(query.Id));

                       if (result.Message.IsExist)
                       {
                           var isRoom = await getRoomClient.GetResponse<GetRoomResponse>(new GetRoomRequset(query.Id, query.Room));

                           return isRoom.Message.IsExist;
                       }
                       else
                       {
                           return true;
                       }
                   })
                   .WithMessage($"Такого кабинета не существует");


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