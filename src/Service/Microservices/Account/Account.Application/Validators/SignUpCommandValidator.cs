﻿using Account.Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Application.Validators
{
    public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
    {
        public SignUpCommandValidator()
        {
            RuleFor(user => user.Password)
                .NotEmpty()
                .WithMessage("Пароль обязательное поле")
                .MinimumLength(4)
                .WithMessage("Пароль должен содержать как минимум 4 символа");
            RuleFor(user => user.Username)
                .NotEmpty()
                .WithMessage("Логин обязательное поле")
                .MinimumLength(4)
                .WithMessage("Логин должен содержать как минимум 4 символа");
            RuleFor(user => user.FirstName)
                .NotEmpty()
                .WithMessage("Имя обязательное поле")
                .MinimumLength(4)
                .WithMessage("Имя должно содержать как минимум 4 символа");
            RuleFor(user => user.LastName)
                .NotEmpty()
                .WithMessage("Фамилия обязательное поле")
                .MinimumLength(4)
                .WithMessage("Фамилия должна содержать как минимум 4 символа");
        }
    }
}
