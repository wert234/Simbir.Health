using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Application.Commands
{
    public class DeleteAccountCommand(int id) : IRequest<IActionResult> 
    {
        public int Id { get; set; } = id;
    }
}
