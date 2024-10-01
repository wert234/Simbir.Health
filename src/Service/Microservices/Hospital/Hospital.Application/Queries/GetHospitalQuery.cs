using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Application.Queries
{
    public class GetHospitalQuery(Guid id) : IRequest<IActionResult>
    {
        public Guid Id { get; set; } = id;
    }
}
