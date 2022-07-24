using Microsoft.EntityFrameworkCore;
using BookingAPI.Models;

namespace BookingAPI.Data
{
    public class ApiContext: DbContext
    {
        public DbSet<HotelBooking> Bookings { get; set; }// Model's name go into <>

        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {

        }
    }
}

