using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Application.Commands
{
    public class AddHospitalCommand : IRequest<IActionResult>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public List<string> Rooms { get; set; }
    }
}
