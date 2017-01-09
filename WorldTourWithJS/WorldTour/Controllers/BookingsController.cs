using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WorldTour.Models;

namespace WorldTour.Controllers
{
    public class BookingsController : Controller
    {
        private WorldTourContext db = new WorldTourContext();


        //Here we are going to get input from the user
        //about the details of the passengers

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(BTViewModel t)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Ticket/GetBookingDetails.cshtml", t);
            }
            else
            {
                var t2 = new CheckoutViewModel()
                {
                    BookingName = t.BookingName,
                    Children = t.Children,
                    Class = t.Class,
                    Email = t.Email,
                    FID = t.FID,
                    RFID = t.RFID,
                    Phone = t.Phone,
                    Quantity = t.Quantity,
                    Tickets = t.Tickets,
                    Total_Price = t.Total_Price
                };

                Flights retf;
                Flights depf = db.Flights.Where(x => x.FlightID == t.FID).FirstOrDefault();
                if (t.RFID != null)
                {
                    retf = db.Flights.Where(x => x.FlightID == t.RFID).FirstOrDefault();
                    ViewBag.RetFlight = retf;
                }

                ViewBag.DepFlight = depf;

                return View(t2);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Completion(CheckoutViewModel CVM, string CardType)
        {

            if (!ModelState.IsValid)
            {

                Flights retf;
                Flights depf = db.Flights.Where(x => x.FlightID == CVM.FID).FirstOrDefault();
                if (CVM.RFID != null)
                {
                    retf = db.Flights.Where(x => x.FlightID == CVM.RFID).FirstOrDefault();
                    ViewBag.RetFlight = retf;
                }

                ViewBag.DepFlight = depf;

                return View("~/Views/Bookings/Checkout.cshtml", CVM);
            }
            else
            {
                string passengers = "";
                string DepFlight = "";
                string RetFlight = "";
                Flights depf, retf;
                //Checks if phone is null and formats string accordingly
                if (CVM.Phone == null) CVM.Phone = "You have not given a phone number.";

                //Formats the passenger information text properly

                for (int i = 0; i < CVM.Quantity; i++)
                {
                    passengers += $"<li>Passenger Name: {CVM.Tickets[i].FirstName} {CVM.Tickets[i].LastName} (Adult)\n";
                }
                for (int i = CVM.Quantity; i < CVM.Children; i++)
                {
                    passengers += $"<li>Passenger Name: {CVM.Tickets[i].FirstName} {CVM.Tickets[i].LastName} (Child)\n";
                }

                //Formatting the flight information text
                depf = db.Flights.Where(x => x.FlightID == CVM.FID).FirstOrDefault();
                DepFlight = $@"<h3>Flight Details</h3>
                <ul>
                <li>Flight Number: {depf.FlightID}</li>
                <li>Departure Airport: {depf.Departure_Airport}</li>
                <li>Departure Date & Time: {depf.Departure_Date.ToString()}</li>
                <li>Arrival Airport: {depf.Arrival_Airport}</li>
                <li>Arrival Date & Time: {depf.Arrival_Date.ToString()}</li>
                <li>Company: {depf.Company_Name}</li>
                </ul>";

                //Formats the return flight information text properly
                if (CVM.RFID == null)
                {
                    retf = null;
                    RetFlight = "You have not booked a return flight.";
                }
                else
                {
                    retf = db.Flights.Where(x => x.FlightID == CVM.RFID).FirstOrDefault();

                    RetFlight = $@"<h3>Return Flight Details:</h3>
                    <ul>
                    <li>Flight Number: {retf.FlightID}</li>
                    <li>Departure Airport: {retf.Departure_Airport}</li>
                    <li>Departure Date & Time: {retf.Departure_Date.ToString()}</li>
                    <li>Arrival Airport: {retf.Arrival_Airport}</li>
                    <li>Arrival Date & Time: {retf.Arrival_Date.ToString()}</li>
                    <li>Company: {retf.Company_Name}</li>
                    </ul>";
                }

                var message = $@"Dear {CVM.BookingName},<br />
                You have made the following booking with World Tour: <br />
                <h3>Booking Details:</h3>
                <ul>
                <li>Name: {CVM.BookingName}</li>
                <li>E-mail: {CVM.Email}</li>
                <li> Phone: {CVM.Phone}</li>
                <li>Tickets Booked: {CVM.Quantity + CVM.Children}</li>
                <li>Ticket Class: {CVM.Class}</li>
                <li>Total Cost: {Math.Round(CVM.Total_Price, 2)} &euro;</li>
                </ul>
                <h3>Passenger Details:</h3>
                <ul>
                {passengers}
                </ul>
                {DepFlight}
                {RetFlight}
                <p>Purchase completed with {CardType} credit card ending with {CVM.CardNumber.Substring(CVM.CardNumber.Length - 4) }.</p>";

                var mail = new MailMessage();
                mail.To.Add(CVM.Email);
                mail.Subject = "Booking confirmation from World Travel.";
                mail.Body = message;
                mail.IsBodyHtml = true;

                

                using (db)
                {
                    foreach (var ticket in CVM.Tickets)
                    {
                        db.Tickets.Add(ticket);
                    }

                    Booking book = new Booking
                    {
                        BookingName = CVM.BookingName,
                        Class = CVM.Class,
                        Email = CVM.Email,
                        FlightID = CVM.FID,
                        ReturnFlightID = CVM.RFID,
                        Phone = CVM.Phone,
                        Quantity = CVM.Quantity + CVM.Children,
                        Total_Price = CVM.Total_Price
                    };
                    db.Booking.Add(book);

                    db.Entry(depf).State = EntityState.Detached;
                    depf.Available_Seats -= book.Quantity;
                    db.Entry(depf).State = EntityState.Modified;

                    if (retf != null)
                    {
                        db.Entry(retf).State = EntityState.Detached;
                        retf.Available_Seats -= book.Quantity;
                        db.Entry(retf).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }

                using (var smtp = new SmtpClient())
                {
                    try
                    {
                        smtp.Send(mail);
                    }
                    catch (Exception)
                    {
                        CVM.errmessage = "There was a problem with our e-mail service. Your booking has been succesfully completed.";
                        return View(CVM);
                    }
                }

                return View(CVM);
            }
        }

    }
}