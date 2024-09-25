using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Application.Queries
{
    public class GetAccountsQuery(int from, int count) : IRequest<IActionResult>
    {
        public int From { get; set; } = from;
        public int Count { get; set; } = count;
    }
}
