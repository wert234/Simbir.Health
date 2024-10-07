using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherad.Domain.Entitys
{
    public class GetRoomRequset(int id, string room)
    {
        public int Id { get; set; } = id;
        public string Room { get; set; } = room;
    }
}
