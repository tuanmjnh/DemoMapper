using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using DemoMapper.Extention;
using DemoMapper.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoMapper.Controllers {
    public class HomeController : Controller {
        public IActionResult Index () {
            var data = getData ().mapImage ();
            return View ();
        }

        public IActionResult About () {
            ViewData["Message"] = "Your application description page.";

            return View ();
        }

        public IActionResult Contact () {
            ViewData["Message"] = "Your contact page.";

            return View ();
        }

        public IActionResult Privacy () {
            return View ();
        }

        [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error () {
            return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        List<Models.hocsinh> getData () {
            var hocsinhs = new List<Models.hocsinh> ();
            var hocsinh = new Models.hocsinh ();
            hocsinh.ten = "Nguyen Van A";
            hocsinh.tuoi = 22;
            hocsinh.ngaysinh = DateTime.Now;
            hocsinh.image = "a.jpg";
            hocsinhs.Add (hocsinh);
            // 
            hocsinh = new Models.hocsinh ();
            hocsinh.ten = "Nguyen Van B";
            hocsinh.tuoi = 21;
            hocsinh.ngaysinh = DateTime.Now;
            hocsinh.image = null;
            hocsinhs.Add (hocsinh);
            // 
            hocsinh = new Models.hocsinh ();
            hocsinh.ten = "Nguyen Van C";
            hocsinh.tuoi = 23;
            hocsinh.ngaysinh = DateTime.Now;
            hocsinh.image = "";
            hocsinhs.Add (hocsinh);
            return hocsinhs;
        }
    }
}