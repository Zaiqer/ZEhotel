using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZEHotel.Models;

namespace ZEHotel.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Position> Positions { get; set; }
    }
}
