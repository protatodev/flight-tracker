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

        [HttpGet("city/{id}/details")]
        public ActionResult Details(int id)
        {
            City newCity = City.Find(id);
            return View(newCity);
        }

        [HttpGet("city/{id}/update")]
        public ActionResult Edit(int id)
        {
            City newCity = City.Find(id);
            return View(newCity);
        }

        [HttpPost("city/{id}/update")]
        public ActionResult EditDetails(int id)
        {
            string newName = Request.Form["newName"];
            City newCity = City.Find(id);
            newCity.Edit(newName);
            return RedirectToAction("ViewAll");
        }

        [HttpPost("city/{id}/delete")]
        public ActionResult Delete(int id)
        {
            City newCity = City.Find(id);
            newCity.Delete();
            return RedirectToAction("ViewAll");
        }
    }
}
