using Account.Application.Commands;
using Account.Domain.Entitys.Account;
using FluentValidation;
using Newtonsoft.Json.Linq;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Application.Validators
{
    public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
    {
        public RefreshCommandValidator(IRepository<User, Guid> accountRepository)
        {
            RuleFor(token => token.RefreshToken)
                .NotEmpty()
                .WithMessage("Токен не должен быть пустым")
                .MustAsync(async (token, cancellation) =>
                {
                    var user = (await accountRepository.GetAllAsync())
                            .FirstOrDefault(u => u.RefreshToken == token);

                    return user != null && user.RefreshTokenExpiryTime > DateTime.UtcNow;
                })
                .WithMessage("Некоректный или просроченный токен");
        }
    }
}
