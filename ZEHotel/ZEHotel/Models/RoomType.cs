using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZEHotel.Models
{
    public class RoomType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Capacity { get; set; }
        public string Price { get; set; }
        public List<Room> Rooms { get; set; }
        public bool IsDeactive { get; set; }
    }
}
