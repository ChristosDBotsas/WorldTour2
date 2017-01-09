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

            var searchVM = new SearchViewModel()
            {
                departuresList = flights.OrderBy(x=> x.Departure_Airport).Select(x => x.Departure_Airport).Distinct().ToList(),
                destinationsList = flights.Select(x => x.Arrival_Airport).Distinct().ToList()
            };          
            return View(searchVM);
        }

        [HttpGet]
        public ActionResult Search(string departureCity, string destinationCity, int? adults, string departureDate, string returnDate, string classType, int? children)
        {
            children = children ?? 0;
            adults = adults ?? 1;

            if (String.IsNullOrEmpty(departureCity) || String.IsNullOrEmpty(destinationCity) || String.IsNullOrEmpty(departureDate))
            {
                ModelState.AddModelError("", "Departure City, Destination City and Departure Date are required fields");
                var flights = db.Flights.Select(x => x);
                var searchVM = new SearchViewModel()
                {
                    departuresList = flights.Select(x => x.Departure_Airport).Distinct().ToList(),
                    destinationsList = flights.Select(x => x.Arrival_Airport).Distinct().ToList()
                };

                return View(searchVM);
            }

            else
            {
                var results = db.Flights.Select(x => x);
                List<Flights> resultsGo = new List<Flights>();
                List<Flights> resultsReturn = new List<Flights>();

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
                       resultsGo = results.OrderBy(x => x.Departure_Date).
                                         Where(x => x.Departure_Airport == departureCity &&
                                         x.Arrival_Airport == destinationCity &&
                                         x.Available_Seats >= (children + adults) &&
                                         DbFunctions.TruncateTime(x.Departure_Date) == DbFunctions.TruncateTime(dep) &&
                                         DbFunctions.TruncateTime(x.Departure_Date) >= DateTime.Today).ToList();               
                    }
                    else
                    {
                        resultsGo = results.OrderBy(x => x.Departure_Date)
                                          .Where(x => x.Departure_Airport == departureCity &&
                                           x.Arrival_Airport == destinationCity &&
                                           x.Available_Seats >= (children + adults) &&
                                           DbFunctions.TruncateTime(x.Departure_Date) == DbFunctions.TruncateTime(dep) &&
                                           DbFunctions.TruncateTime(x.Departure_Date) >= DateTime.Today).ToList();

                        resultsReturn = results.OrderBy(x => x.Departure_Date)
                                                    .Where(x => x.Departure_Airport == destinationCity &&
                                                     x.Arrival_Airport == departureCity &&
                                                     x.Available_Seats >= (children + adults) &&
                                                     DbFunctions.TruncateTime(x.Departure_Date) == DbFunctions.TruncateTime(ret) &&
                                                     DbFunctions.TruncateTime(x.Departure_Date) >= DateTime.Today).ToList();
                    }

                var searchVM = new SearchViewModel()
                {
                    adults = (int)adults,
                    children = (int)children,
                    classType = classType,
                    resultsGo = resultsGo,
                    resultsReturn = resultsReturn,
                    departuresList = db.Flights.Select(x => x.Departure_Airport).Distinct().ToList(),
                    destinationsList = db.Flights.Select(x => x.Arrival_Airport).Distinct().ToList(),
                    DepartureCity = departureCity,
                    ArrivalCity = destinationCity
                };

                return View(searchVM); 
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

            var resultsGo = db.Flights.Where(x => x.Departure_Airport == depCity
                                && x.Arrival_Airport == arrCity
                                && DbFunctions.TruncateTime(x.Departure_Date) >= (DbFunctions.TruncateTime(startDate))
                                && DbFunctions.TruncateTime(x.Departure_Date) >= DateTime.Today
                                && DbFunctions.TruncateTime(x.Departure_Date) <= DbFunctions.TruncateTime(endDate)
                                && x.Available_Seats >= 2).ToList();

            var resultsReturn = db.Flights.Where(x => x.Departure_Airport == arrCity
                                && x.Arrival_Airport == depCity
                                && DbFunctions.TruncateTime(x.Departure_Date) >= DbFunctions.TruncateTime(startDate)
                                && DbFunctions.TruncateTime(x.Departure_Date) >= DateTime.Today
                                && DbFunctions.TruncateTime(x.Departure_Date) <= DbFunctions.TruncateTime(endDate)
                                && x.Available_Seats >= 2).ToList();

            var flights = db.Flights.Select(x => x);
            
            var searchVM = new SearchViewModel()
            {
                adults = 2,
                children = 0,
                classType = "Economy",
                resultsGo = resultsGo,
                resultsReturn = resultsReturn,
                departuresList = db.Flights.Select(x => x.Departure_Airport).Distinct().ToList(),
                destinationsList = db.Flights.Select(x => x.Arrival_Airport).Distinct().ToList(),
                DepartureCity = depCity,
                ArrivalCity = arrCity
            };

            return View("Search", searchVM);
        }

        public ActionResult AllOffers()
        {
            return View();
        }

        public JsonResult GetDestinationCities(string city)
        {
            List<string> returnCities = db.Flights.Where(x => x.Departure_Airport == city).Select(x => x.Arrival_Airport).Distinct().ToList();

            return Json(returnCities, JsonRequestBehavior.AllowGet);
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
