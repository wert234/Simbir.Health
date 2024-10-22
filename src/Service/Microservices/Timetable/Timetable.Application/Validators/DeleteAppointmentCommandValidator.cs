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

namespace Timetable.Application.Validators
{
    public class DeleteAppointmentCommandValidator : AbstractValidator<DeleteAppointmentCommand>
    {
        public DeleteAppointmentCommandValidator(
            IRepository<Appointment, int> appointmentRepository,
            IRequestClient<GetUserExistRequest> client)
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
                    RuleFor(query => query)
                        .MustAsync(async (query, c) =>
                        {
                            var result = (await client.GetResponse<GetUserExistResponse>(new GetUserExistRequest(query.UserId))).Message;
                            var appointment = (await appointmentRepository
                            .GetAllAsync())
                            .First(findAppointment => findAppointment.Id == query.AppointmentId);

                            if (result.Roles.Contains("User"))
                            {
                                return appointment.UserId == query.UserId;
                            }
                            else
                            {
                                return true;
                            }
                        })
                        .WithMessage("Нет доступа");
                });

        }
    }
}
