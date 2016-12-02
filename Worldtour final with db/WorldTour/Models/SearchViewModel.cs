
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldTour.Models
{
    public class SearchViewModel
    {
        public string classType { get; set; }
        public int? adults { get; set; }
        public int? children { get; set; }
        public List<Flights> resultsGo { get; set; }
        public List<Flights> resultsReturn { get; set; }
        public List<string> departuresList { get; set; }
        public List<string> destinationsList { get; set; }
    }
}