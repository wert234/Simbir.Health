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
using Timetable.Domain.Entitys;
using Timetable.Domain.Models;

namespace Timetable.Application.Validators
{
    public class AddTimetableCommandValidator : AbstractValidator<AddTimetableCommand>
    {

        public AddTimetableCommandValidator(
            IRepository<Domain.Entitys.Timetable, int> timetableRepository,
            IRequestClient<GetUserRequest> getUserClient,
            IRequestClient<GetHospitalRequset> getHospitalClient)
        {

            RuleFor(timetable => timetable.HospitalId)
                .MustAsync(async (HospitalId, c) =>
                {
                    var result = await getHospitalClient.GetResponse<GetHospitalResponse>(new GetHospitalRequset(HospitalId));
                    return result.Message.IsExist;
                })
                .WithMessage("Больницы с таким Id не существует");

            RuleFor(timetable => timetable.DoctorId)
                .MustAsync(async (DoctorId, c) =>
                {
                   var result = await getUserClient.GetResponse<GetUserResponse>(new GetUserRequest(DoctorId));
                    return result.Message.IsExist;
                })
                .WithMessage("Доктара с таким Id не существует");

            RuleFor(timetable => timetable)
               .MustAsync(async (timetable, c) => 
               {
                   var timetables = await timetableRepository.GetAllAsync();

                   return TimetableValidationUtils.CheckForOverlapping(
                       new Domain.Entitys.Timetable
                       {
                           DoctorId = timetable.DoctorId,
                           HospitalId = timetable.HospitalId,
                           From = timetable.From,
                           To = timetable.To,
                           Room = timetable.Room,
                       },
                       timetables.Where(findTimetable =>
                     findTimetable.DoctorId == timetable.DoctorId).ToList());
                  
        })
               .WithMessage("Расписание не может перекрываться с существующими расписаниями.");

            RuleFor(x => x.From)
                .Must(TimetableValidationUtils.IsValidDateTimeFormat)
                .WithMessage("Неверный формат даты и времени для From. Формат: yyyy-MM-ddTHH:mm:ssZ.")
                .Must(TimetableValidationUtils.IsValidDateTimeOffset)
                .WithMessage("Дата и время From должны быть кратны 30 минутам.");

            RuleFor(x => x.To)
                .Must(TimetableValidationUtils.IsValidDateTimeFormat)
                .WithMessage("Неверный формат даты и времени для To. Формат: yyyy-MM-ddTHH:mm:ssZ.")
                .Must(TimetableValidationUtils.IsValidDateTimeOffset)
                .WithMessage("Дата и время To должны быть кратны 30 минутам.");

            RuleFor(x => x.From)
                .LessThan(x => x.To)
                .WithMessage("Дата и время From должны быть меньше To.");

            RuleFor(x => x.To)
                .GreaterThanOrEqualTo(x => x.From)
                .Must((x, to) => (to - x.From).TotalHours <= 12)
                .WithMessage("Разница между To и From не должна превышать 12 часов.");

            RuleFor(x => x.Room)
                .NotEmpty()
                .WithMessage("Поле Room не может быть пустым.");
        }
    }
}
