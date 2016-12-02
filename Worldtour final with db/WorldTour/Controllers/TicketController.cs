using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldTour.Models;

namespace WorldTour.Controllers
{
    public class TicketController : Controller
    {
        WorldTourContext db = new WorldTourContext();
        public ActionResult Tickets()
        {
            return View();
        }


        //Quantity will be given by Booking Controller

        [HttpGet]
        public ActionResult GetBookingDetails(int? goID, int? returnID, int persons, int children, decimal totalPrice, string classType, decimal goPricePP, decimal returnPricePP)
        {
            if (!goID.HasValue && returnID.HasValue)
            {
                goID = returnID;
                returnID = null;
            }
            //We create a demo booking 
            var BookTicketInfo = new BTViewModel()
            {
                FID = (int)goID,
                RFID = returnID,
                Total_Price = totalPrice,
                Quantity = persons,
                Children = children,
                Class = classType,
            };

            return View(BookTicketInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}