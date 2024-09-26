using Account.Domain.Entitys.Account;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Application.Commands
{
    public class UpdateAccountCommand : IRequest<IActionResult>
    {
        [SwaggerSchema(ReadOnly = true)]
        public Guid Id { get; private set; }
        public List<Role> Roles { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


        public UpdateAccountCommand(Guid id, List<Role> roles, string lastName, string firstName, string username, string password)
        {
            Id = id;
            Roles = roles;
            LastName = lastName;
            FirstName = firstName;
            Username = username;
            Password = password;
        }
    }
}
