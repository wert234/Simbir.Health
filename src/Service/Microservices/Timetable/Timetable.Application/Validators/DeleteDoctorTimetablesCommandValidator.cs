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
    public class DeleteDoctorTimetablesCommandValidator : AbstractValidator<DeleteDoctorTimetablesCommand>
    {
        public DeleteDoctorTimetablesCommandValidator(
            IRequestClient<GetUserRequest> getUserClient,
            IRepository<Domain.Entitys.Timetable, int> timetableRepository)
        {
            RuleFor(timetable => timetable.Id)
                .MustAsync(async (DoctorId, c) =>
                {
                    var result = await getUserClient.GetResponse<GetUserResponse>(new GetUserRequest(DoctorId));
                    return result.Message.IsExist;
                })
                .WithMessage("Доктара с таким Id не существует");
        }
    }
}
