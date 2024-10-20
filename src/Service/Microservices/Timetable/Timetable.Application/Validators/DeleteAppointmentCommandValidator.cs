using FluentValidation;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Application.Commands;
using Timetable.Domain.Entitys;

namespace Timetable.Application.Validators
{
    public class DeleteAppointmentCommandValidator : AbstractValidator<DeleteAppointmentCommand>
    {
        public DeleteAppointmentCommandValidator(IRepository<Appointment, int> appointmentRepository)
        {
            RuleFor(appointment => appointment)
                .MustAsync(async (appointment, c) =>
                {
                    return (await appointmentRepository.GetAllAsync())
                    .FirstOrDefault(findAppointment => findAppointment.Id == appointment.AppointmentId) != null;
                })
                .WithMessage("Такой записи не сущесвует")
                .DependentRules(() =>
                {
                    RuleFor(appointment => appointment)
                        .MustAsync(async (appointment, c) =>
                        {
                            return (await appointmentRepository
                            .GetAsync(appointment.AppointmentId)).UserId == appointment.UserId;
                        })
                        .WithMessage("Нет доступа");
                });

        }
    }
}
