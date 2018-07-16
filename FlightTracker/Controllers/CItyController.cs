using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FlightTracker.Models;

namespace FlightTracker.Controllers
{
    public class CityController : Controller
    {
        [HttpGet("new-city")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("new-city")]
        public ActionResult CreatePost()
        {
            string name = Request.Form["name"];
            City newCity = new City(name);
            newCity.Save();

            return RedirectToAction("ViewAll");
        }

        [HttpGet("view-cities")]
        public ActionResult ViewAll()
        {
            List<City> allCities = City.GetAll();
            return View(allCities);
        }
    }
}
