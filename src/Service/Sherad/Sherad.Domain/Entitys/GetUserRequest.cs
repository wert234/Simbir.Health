﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherad.Domain.Entitys
{
    public class GetUserRequest(int id)
    {
        public int Id { get; set; } = id;
    }
}
