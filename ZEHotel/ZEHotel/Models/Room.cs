using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZEHotel.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
        [NotMapped]
        public List<IFormFile> Photos { get; set; }
        public int RoomTypeId { get; set; }
        public List<RoomImage> RoomImages { get; set; }
        public RoomType RoomType { get; set; }
        public bool IsDeactive { get; set; }
    }
}
