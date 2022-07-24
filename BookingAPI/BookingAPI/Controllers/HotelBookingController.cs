using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BookingAPI.Models;
using BookingAPI.Data;

namespace BookingAPI.Controllers
{
    [Route("api/[controller]/[action]")] // actions give us the names of the functions displayed in swagger UI
    [ApiController]
    public class HotelBookingController : ControllerBase
    {
        private readonly ApiContext _context;   // this is our database context, provided from dependency injection

        public HotelBookingController(ApiContext context)
        {
            _context = context;
        }

        // Create/Edit
        [HttpPost] // really important
        public JsonResult CreateEdit(HotelBooking booking) // Return is of type JsonResult, because then we can return objects of type JSON
        {
         // check if booking ID is 0
            // if it is 0 or NULL, then we create a new booking
            if (booking.Id == 0) 
            {
                _context.Bookings.Add(booking); // this way we add the new booking to Bookings Table
            }

            // Else
                // If ID is not NUll, then we might have this booking previously added
                    // Then we search for the booking in the DB
                        // If booking doesn't exsist
                            // Then return JsonResult of content Not Found
                        // Else
                            // We have the booking
                            // So replace the current booking, in current Database, with new one that gets sent in from Parameter
            else
            {
                var bookingInDb = _context.Bookings.Find(booking.Id); // find the booking

                if(bookingInDb == null) // booking not found
                {
                    return new JsonResult(NotFound());
                }
                bookingInDb = booking; // booking found, and replace the new booking (sent through parameter) with Old booking (i.e. in DB)
            }

            // Very important always save your database context, after modifying anything
            _context.SaveChanges();

            // and return the Ok result with the booking in JSON format
            return new JsonResult(Ok(booking));
        }

        // Get
        [HttpGet]
        public JsonResult Get(int id)
        {
            var result = _context.Bookings.Find(id); //using find method to search for the booking 

            if (result == null) // if not found, return that JSON object with NotFound
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(result));  // else return the result with Ok Status
        }

        // Delete
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var result = _context.Bookings.Find(id);    // again find the booking

            if(result == null)
            {
                return new JsonResult(NotFound());  // return not found if not found in database
            }

            _context.Bookings.Remove(result);   // else remove the result
            _context.SaveChanges(); // Since changes are made to the database, we need to save them again

            return new JsonResult(NoContent()); // this time return Json Object saying no Content at that place
        }

        // Get All
        [HttpGet]
        public JsonResult GetAll()
        {
            var result = _context.Bookings.ToList(); // simply convert all the elements into list
            return new JsonResult(Ok(result));  // and return result
        }
    }
}

