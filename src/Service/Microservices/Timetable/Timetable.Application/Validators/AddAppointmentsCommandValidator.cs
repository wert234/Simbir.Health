using FluentValidation;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Application.Commands;
using Timetable.Application.Queries;
using Timetable.Domain.Entitys;
using Timetable.Domain.Models;

namespace Timetable.Application.Validators
{
    public class AddAppointmentsCommandValidator : AbstractValidator<AddAppointmentsCommand>
    {
        public AddAppointmentsCommandValidator(
            IRepository<Domain.Entitys.Timetable, int> timetableRepository,
            IRepository<Appointment, int> appointmentRepository)
        {

            RuleFor(timetable => timetable.Id)
                .MustAsync(async (id, c) =>
                {
                    var result = await timetableRepository.GetAsync(id);
                    return result != null;
                })
                .WithMessage("Распесания с таким Id не существует")
                .DependentRules(() =>
                {
                    RuleFor(appointment => appointment)
                        .MustAsync(async (appointment, c) =>
                        {
                            var timetable = await timetableRepository.GetAsync(appointment.Id);

                            return timetable.To > appointment.Time &&
                                   timetable.From <= appointment.Time;
                        })
                        .WithMessage("В это время, приёма нет");

                });

            RuleFor(x => x.Time)
                .Must(TimetableValidationUtils.IsValidDateTimeFormat)
                .WithMessage("Неверный формат даты и времени для To. Формат: yyyy-MM-ddTHH:mm:ssZ.")
                .Must(TimetableValidationUtils.IsValidDateTimeOffset)
                .WithMessage("Дата и время To должны быть кратны 30 минутам.");

            RuleFor(appointment => appointment)
                .MustAsync(async (appointment, c) =>
                {
                    var appointments = await appointmentRepository.GetAllAsync();

                    return !appointments.Any(finderAppointment =>
                        finderAppointment.TimetableId == appointment.Id &&
                        finderAppointment.Date == appointment.Time);
                })
                .WithMessage("На это время уже назначена запись");
        }
    }
}
