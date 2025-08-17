using ChatApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatApplication.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext db=new AppDbContext();
        // GET: Home
        public ActionResult Home()
        {
            ViewBag.TotalUsers = db.User.Count();
            ViewBag.TotalChats = db.Chat.Count();

            var chartData = db.Chat
       .GroupBy(c => DbFunctions.TruncateTime(c.current_date)) 
       .Select(g => new
       {
           Date = g.Key.Value,
           Count = g.Count()
       })
       .ToList();
            ViewBag.ChartLabels = string.Join(",", chartData.Select(x => "'" + x.Date.ToString("yyyy-MM-dd") + "'"));
            ViewBag.ChartCounts = string.Join(",", chartData.Select(x => x.Count));

            return View();
        }
    }
}