using Sherad.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherad.Domain.Entitys
{
    public class GetHospitalResponse(bool isExist, HospitalDTO data = null)
    {
        public bool IsExist { get; set; } = isExist;
        public HospitalDTO Data { get; set; } = data;
    }
}
