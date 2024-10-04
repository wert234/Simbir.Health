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
    public class DeleteTimetableCommandValidator : AbstractValidator<DeleteTimetableCommand>
    {

        public DeleteTimetableCommandValidator(
            IRepository<Domain.Entitys.Timetable, int> timetableRepository)
        {
            RuleFor(command => command.Id)
                .MustAsync(async (id, c) =>
                {
                    var result = await timetableRepository.GetAsync(id);

                    return result != null;
                })
                .WithMessage("Некоректный id запеси");
        }
    }
}