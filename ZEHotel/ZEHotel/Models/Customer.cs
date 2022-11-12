using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZEHotel.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Ad xanası boş ola bilməz. Zəhmət olmasa bu xananı doldurun!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Soyad xanası boş ola bilməz. Zəhmət olmasa bu xananı doldurun!")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Əlaqə nömrəsi xanası boş ola bilməz. Zəhmət olmasa bu xananı doldurun!")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Email adresi xanası boş ola bilməz. Zəhmət olmasa bu xananı doldurun!")]
        public string Mail { get; set; }
        public List<Room> Rooms { get; set; }
        public bool IsDeactive { get; set; }
    }
}
