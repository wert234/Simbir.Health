using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace History.Application.Queries
{
    public class GetHistoryQuerу(int id) : IRequest<IActionResult>
    {
        public int Id { get; set; } = id;
    }
}
