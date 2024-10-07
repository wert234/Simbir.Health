using Sherad.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherad.Domain.Entitys
{
    public class GetRoomResponse(bool isExist)
    {
        public bool IsExist { get; set; } = isExist;
    }
}
