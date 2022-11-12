using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZEHotel.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        [Required(ErrorMessage = "Ad xanası boş ola bilməz. Zəhmət olmasa bu xananı doldurun!")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email adresi xanası boş ola bilməz. Zəhmət olmasa bu xananı doldurun!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Əlaqə nömrəsi xanası boş ola bilməz. Zəhmət olmasa bu xananı doldurun!")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Maaş xanası boş ola bilməz. Zəhmət olmasa bu xananı doldurun!")]
        public int Salary { get; set; }
        public bool IsDeactive { get; set; }
        public Position Position { get; set; }
        public int PositionId { get; set; }
    }
}
