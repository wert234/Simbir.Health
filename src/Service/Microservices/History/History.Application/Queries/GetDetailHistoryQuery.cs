using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace History.Application.Queries
{
    public class GetDetailHistoryQuery(int userId, int requestUserId) : IRequest<IActionResult>
    {
        public int UserId { get; set; } = userId;
        public int RequestUserId { get; set; } = requestUserId;
    }
}
