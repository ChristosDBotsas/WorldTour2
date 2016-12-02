using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WorldTour.Models;

namespace WorldTour.Controllers
{
    public class HomeController : Controller
    {
        private WorldTourContext db = new WorldTourContext();

        // GET: Flights
        public ActionResult Index()
        {
            var flights = db.Flights.Select(x => x);
            ViewBag.departuresList = flights.Select(x => x.Departure_Airport).Distinct();
            ViewBag.destinationsList = flights.Select(x => x.Arrival_Airport).Distinct();

            return View();
        }

        [HttpGet]
        public ActionResult Search(string departureCity, string destinationCity, int? adults, string departureDate, string returnDate, string classType, int? children)
        {
            children = children ?? 0;
            adults = adults ?? 1;

            if (String.IsNullOrEmpty(departureCity) || String.IsNullOrEmpty(destinationCity))
            {
                ModelState.AddModelError("", "Departure, Destination and Number of Persons are required fields");
                var flights = db.Flights.Select(x => x);
                ViewBag.departuresList = flights.Select(x => x.Departure_Airport).Distinct();
                ViewBag.destinationsList = flights.Select(x => x.Arrival_Airport).Distinct();
                return View();
            }

            else
            {
                var results = db.Flights.Select(x => x);
                
                ViewBag.departuresList = results.Select(x => x.Departure_Airport).Distinct();
                ViewBag.destinationsList = results.Select(x => x.Arrival_Airport).Distinct();
                ViewBag.ClassType = classType;
                ViewBag.Adults = adults;
                ViewBag.Children = children;

                DateTime datevalue;
                DateTime dep;
                DateTime ret;
                if (DateTime.TryParse(departureDate, out datevalue))
                {
                    dep = datevalue;
                }
                else
                {
                    dep = new DateTime(1970, 01, 01);
                }
                if (DateTime.TryParse(returnDate, out datevalue))
                {
                    ret = datevalue;
                }
                else
                {
                    ret = new DateTime(1970, 01, 01);
                }

                if (String.IsNullOrEmpty(returnDate))
                    {
                        if (String.IsNullOrEmpty(destinationCity) && (children + adults) > 0 && !String.IsNullOrEmpty(departureDate))
                        {
                            results = results.OrderBy(x => x.Departure_Date).
                                              Where(x => x.Departure_Airport == departureCity &&
                                              x.Available_Seats >= (children + adults) &&
                                              DbFunctions.TruncateTime(x.Departure_Date) == DbFunctions.TruncateTime(dep));
                        }
                        else if (!String.IsNullOrEmpty(destinationCity) && (children + adults) > 0)
                        {
                            if (String.IsNullOrEmpty(departureDate))
                            {
                                results = results.OrderBy(x => x.Departure_Date).
                                             Where(x => x.Departure_Airport == departureCity &&
                                             x.Arrival_Airport == destinationCity &&
                                             x.Available_Seats >= (children + adults));
                            }
                            else
                            {
                                results = results.OrderBy(x => x.Departure_Date).
                                                  Where(x => x.Departure_Airport == departureCity &&
                                                  x.Arrival_Airport == destinationCity &&
                                                  x.Available_Seats >= (children + adults) &&
                                                  DbFunctions.TruncateTime(x.Departure_Date) == DbFunctions.TruncateTime(dep));
                            }
                        }
                        ViewBag.ResultsGo = results;
                    }
                    else
                    {
                        var resultsGo = results.OrderBy(x => x.Departure_Date)
                                          .Where(x => x.Departure_Airport == departureCity &&
                                           x.Arrival_Airport == destinationCity &&
                                           x.Available_Seats >= (children + adults) &&
                                           DbFunctions.TruncateTime(x.Departure_Date) == DbFunctions.TruncateTime(dep));

                        var resultsReturn = results.OrderBy(x => x.Departure_Date)
                                                    .Where(x => x.Departure_Airport == destinationCity &&
                                                     x.Arrival_Airport == departureCity &&
                                                     x.Available_Seats >= (children + adults) &&
                                                     DbFunctions.TruncateTime(x.Departure_Date) == DbFunctions.TruncateTime(ret));

                        ViewBag.ResultsGo = resultsGo;
                        ViewBag.ResultsReturn = resultsReturn;
                    }
                return View(); 
            }
        }

        [HttpPost]
        public ActionResult Search(int? goFlight, int? returnFlight, string classType, int adults, int children)
        {
            decimal goPrice = 0;
            decimal returnPrice = 0;

            if (goFlight != null)
            {
                goPrice = db.Flights.Where(x => x.FlightID == goFlight).Select(x => x.Starting_Price).FirstOrDefault();
            }
            if (returnFlight != null)
            {
                returnPrice = db.Flights.Where(x => x.FlightID == returnFlight).Select(x => x.Starting_Price).FirstOrDefault();
            }

            switch (classType)
            {
                case "First":
                    goPrice *= 2;
                    returnPrice *= 2;
                    break;
                case "Business":
                    goPrice *= (decimal)1.5;
                    returnPrice *= (decimal)1.5;
                    break;
                default:
                    break;
            }

            decimal totalGoPrice = (adults * goPrice) + children * (goPrice * (decimal)0.5);
            decimal totalReturnPrice = (adults * returnPrice) + children * (returnPrice * (decimal)0.5);

            decimal totalPrice = totalGoPrice + totalReturnPrice;

            return RedirectToAction("GetBookingDetails", "Ticket", new { goID = goFlight, returnID = returnFlight, persons = adults, children = children, totalPrice = totalPrice, classType = classType, goPricePP = goPrice, returnPricePP = returnPrice });
        }

        public ActionResult Offers(string depCity, string arrCity, DateTime startDate, DateTime endDate)
        {

            ViewBag.ResultsGo = db.Flights.Where(x => x.Departure_Airport == depCity
                                && x.Arrival_Airport == arrCity
                                && DbFunctions.TruncateTime(x.Departure_Date) >= DbFunctions.TruncateTime(startDate)
                                && DbFunctions.TruncateTime(x.Departure_Date) <= DbFunctions.TruncateTime(endDate)
                                && x.Available_Seats >= 2);

            ViewBag.ResultsReturn = db.Flights.Where(x => x.Departure_Airport == arrCity
                                && x.Arrival_Airport == depCity
                                && DbFunctions.TruncateTime(x.Departure_Date) >= DbFunctions.TruncateTime(startDate)
                                && DbFunctions.TruncateTime(x.Departure_Date) <= DbFunctions.TruncateTime(endDate)
                                && x.Available_Seats >= 2);

            var flights = db.Flights.Select(x => x);
            ViewBag.departuresList = flights.Select(x => x.Departure_Airport).Distinct();
            ViewBag.destinationsList = flights.Select(x => x.Arrival_Airport).Distinct();
            ViewBag.Adults = 2;
            ViewBag.Children = 0;
            ViewBag.ClassType = "Economy";

            return View("Search");
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
