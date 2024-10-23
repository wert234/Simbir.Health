using FluentValidation;
using History.Application.Commands;
using MassTransit;
using Sherad.Domain.Entitys;
using Sherad.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace History.Application.Validators
{
    public class AddHistoryCommandValidator : AbstractValidator<AddHistoryCommand>
    {
        public AddHistoryCommandValidator(
            IRequestClient<GetUserExistRequest> client,
            IRequestClient<GetHospitalRequset> getHospitalClient,
            IRequestClient<GetUserRequest> getUserClient) 
        {
            RuleFor(query => query.PacientId)
               .MustAsync(async (id, c) =>
               {
                   var result = (await client
                   .GetResponse<GetUserExistResponse>(new GetUserExistRequest(id))).Message;
                   return result.isExist && result.Roles.Contains("User");
               })
               .WithMessage("Пациента не существует");

            RuleFor(query => query.HospitalId)
                .MustAsync(async (HospitalId, c) =>
                {
                    var result = await getHospitalClient
                    .GetResponse<GetHospitalResponse>(new GetHospitalRequset(HospitalId));
                    return result.Message.IsExist;
                })
                .WithMessage("Больницы с таким Id не существует");

            RuleFor(query => query.DoctorId)
                .MustAsync(async (DoctorId, c) =>
                {
                    var result = await getUserClient.GetResponse<GetUserResponse>(new GetUserRequest(DoctorId));
                    return result.Message.IsExist;
                })
                .WithMessage("Доктара с таким Id не существует");

            RuleFor(query => query.Date)
                .Must(date => DateTimeOffset
                .TryParseExact(date
                .ToString("yyyy-MM-ddTHH:mm:ssZ"),
                "yyyy-MM-ddTHH:mm:ssZ",
                null,
                System.Globalization.DateTimeStyles.AssumeUniversal, out _))
                .WithMessage("Неверный формат даты и времени для To. Формат: yyyy-MM-ddTHH:mm:ssZ.")
                .Must(date => date.Minute % 30 == 0)
                .WithMessage("Дата и время должны быть кратны 30 минутам.");

            RuleFor(query => query.Room)
                .NotEmpty()
                .WithMessage("Поле Room не может быть пустым.");
        }
    }
}
