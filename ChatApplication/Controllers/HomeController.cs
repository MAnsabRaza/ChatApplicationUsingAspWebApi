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
            .GroupBy(c => new { c.current_date.Year, c.current_date.Month })
            .Select(g => new
            {
            Month = g.Key.Month,
            Year = g.Key.Year,
            Count = g.Count()
            })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToList();
            ViewBag.ChartLabels = string.Join(",", chartData.Select(x => "'" + new DateTime(x.Year, x.Month, 1).ToString("MMM-yyyy") + "'"));
            ViewBag.ChartCounts = string.Join(",", chartData.Select(x => x.Count));


            return View();
        }
    }
}