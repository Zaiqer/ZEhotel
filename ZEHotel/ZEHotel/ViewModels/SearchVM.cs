using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZEHotel.Models;

namespace ZEHotel.ViewModels
{
    public class SearchVM
    {
        public List<Room> Rooms { get; set; }
        public List<RoomType> RoomTypes { get; set; }
        public List<Position> Positions { get; set; }
    }
}
