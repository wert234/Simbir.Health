using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace History.Application.Commands
{
    public class AddHistoryCommand : IRequest<IActionResult>
    {
        public DateTimeOffset Date { get; set; }
        public int PacientId { get; set; }
        public int HospitalId { get; set; }
        public int DoctorId { get; set; }
        public string Room { get; set; }
        public string Data { get; set; }
    }
}
