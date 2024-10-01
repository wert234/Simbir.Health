using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sherad.Domain.Entitys
{
    public class TokenValidationResponse
    {
        public bool IsSuccess { get; set; }
        public object Result { get; set; }
    }
}
