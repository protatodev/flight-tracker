using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FlightTracker.Controllers
{
    public class FlightController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
