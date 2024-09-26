﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Application.Commands
{
    public class SignInCommand : IRequest<IActionResult>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
