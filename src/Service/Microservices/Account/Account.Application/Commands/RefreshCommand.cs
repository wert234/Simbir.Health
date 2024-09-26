using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Application.Commands
{
    public class RefreshCommand(string refreshToken) : IRequest<IActionResult>
    {
        public string RefreshToken { get; set; } = refreshToken;
    }
}
