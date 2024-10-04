using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Application.Commands
{
    public class UpdateHospitalCommand : IRequest<IActionResult>
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public List<string> Rooms { get; set; }


        public UpdateHospitalCommand(int id, string name, string address, string contactPhone, List<string> rooms)
        {
            Id = id;
            Name = name;
            Address = address;
            ContactPhone = contactPhone;
            Rooms = rooms;
        }
    }
}
