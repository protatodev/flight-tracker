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
            return View(City.GetAll());
        }

        [HttpPost("new-flight")]
        public ActionResult CreatePost()
        {

            int flightNum = int.Parse(Request.Form["number"]);
            string time = Request.Form["time"];
            string arrival_departure = Request.Form["arrival_departure"];
            string status = Request.Form["status"];
            int cityId = int.Parse(Request.Form["city"]);

            TimeSpan timeTo = TimeSpan.Parse(time);

            City newCity = City.Find(cityId);

            Flight newFlight = new Flight(flightNum, timeTo, arrival_departure, status);
            newFlight.Save();
            newFlight.AddCity(newCity);

            return RedirectToAction("ViewAll");
        }

        [HttpGet("view-flights")]
        public ActionResult ViewAll()
        {
            List<Flight> allFlights = Flight.GetAll();
            return View(allFlights);
        }

        [HttpGet("flight/{id}/details")]
        public ActionResult Details(int id)
        {
            Flight newFlight = Flight.Find(id);
            return View(newFlight);
        }

        [HttpGet("flight/{id}/update")]
        public ActionResult Edit(int id)
        {
            Flight newFlight = Flight.Find(id);
            return View(newFlight);
        }

        [HttpPost("flight/{id}/update")]
        public ActionResult EditDetails(int id)
        {
            int flightNum = int.Parse(Request.Form["newFlightNum"]);
            string time = Request.Form["newTime"];
            string arrival_departure = Request.Form["newArrival_departure"];
            string status = Request.Form["newStatus"];
            int cityId = int.Parse(Request.Form["newCityId"]);

            TimeSpan timeTo = TimeSpan.Parse(time);

            Flight newFlight = Flight.Find(id);
            newFlight.Edit(flightNum, timeTo, arrival_departure, status, cityId);
            return RedirectToAction("ViewAll");
        }

        [HttpPost("flight/{id}/delete")]
        public ActionResult Delete(int id)
        {
            Flight newFlight = Flight.Find(id);
            newFlight.Delete();
            return RedirectToAction("ViewAll");
        }
    }
}
