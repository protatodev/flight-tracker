using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FlightTracker.Models;

namespace FlightTracker.Controllers
{
    public class FlightController : Controller
    {
        [HttpGet("new-flight")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("new-flight")]
        public ActionResult CreatePost()
        {

            int flightNum = int.Parse(Request.Form["number"]);
            DateTime time = Convert.ToDateTime(Request.Form["time"]);
            string arrival_departure = Request.Form["arrival_departure"];
            string status = Request.Form["status"];

            City newFlight = new Flight();
            newFlight.Save();
            return RedirectToAction("ViewAll");
        }

        [HttpGet("view-flights")]
        public ActionResult ViewAll()
        {
            List<Flight> allFlights = Flight.GetAll();
            return View(allFlights);
        }
    }
}
