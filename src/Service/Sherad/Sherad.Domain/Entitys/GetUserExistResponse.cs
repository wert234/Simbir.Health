﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherad.Domain.Entitys
{
    public class GetUserExistResponse
    {
        public bool isExist { get; set; }
        public List<string> Roles { get; set; }
    }
}
