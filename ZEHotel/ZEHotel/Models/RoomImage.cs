using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZEHotel.Models
{
    public class RoomImage
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public Room Room { get; set; }
        public int RoomId { get; set; }
    }
}
