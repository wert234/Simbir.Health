using Account.Application.Queries;
using Account.Domain.Entitys.Account;
using Account.Domain.Entitys.Tokens.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Account.Application.Handlers
{
    public class ValidateHandler : IRequestHandler<ValidateQuery, IActionResult>
    {
        private readonly ITokenService _tokenService;


        public ValidateHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }


        public async Task<IActionResult> Handle(ValidateQuery request, CancellationToken cancellationToken)
        {
            var claimsPrincipal = _tokenService.GetPrincipalFromToken(request.AccessToken);

            if(claimsPrincipal.isSuccess)
            {
                return new OkObjectResult(claimsPrincipal);
            }
            else
            {
                return new BadRequestObjectResult("Некоректный токен");
            }
        }
    }
}
