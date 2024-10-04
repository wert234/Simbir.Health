using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherad.Domain.Entitys
{
    public class GetHospitalResponse(bool isExist)
    {
        public bool IsExist { get; set; } = isExist;
    }
}
