using ChatApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ChatApplication.Controllers
{
    public class UserController : Controller
    {
        private AppDbContext db = new AppDbContext();
        // GET: User
        public ActionResult User()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public JsonResult Get()
        {
            var user = db.User.ToList();
            return Json(user,JsonRequestBehavior.AllowGet); 
        }
        [HttpPost]
        public JsonResult Create(User user)
        {
            if(ModelState.IsValid)
            {
                if (user.Id > 0)
                {
                    var existingUser = db.User.Find(user.Id);
                    if (existingUser != null)
                    {
                        existingUser.current_date = user.current_date;
                        existingUser.name = user.name;
                        existingUser.email = user.email;
                        if (!string.IsNullOrWhiteSpace(user.password))
                        {
                            existingUser.password = BCrypt.Net.BCrypt.HashPassword(user.password);
                        }
                        existingUser.phone_number = user.phone_number;
                        existingUser.address = user.address;
                        db.Entry(existingUser).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json(new { status = 200, message = "User Update Successfully" });
                    }
                    else
                    {
                        user.password=BCrypt.Net.BCrypt.HashPassword(user.password);
                        db.User.Add(user);
                        db.SaveChanges();
                        return Json(new { status = 200, message = "User Created Successfully" });
                    }
                }
            }
            return Json(new { status = 200, message = "User Invalid" });
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var user= db.User.Find(id);
            if (user != null)
            {
                db.User.Remove(user);
                db.SaveChanges();
                return Json(new { status = 200, message = "Deleted Successfully" });
            }
            return Json(new { status = 200, message = "User Invalid" });
        }
        [HttpGet]
        public JsonResult Get(int id)
        {
            var user = db.User.Find(id);
            if (user != null)
            {
                return Json(user,JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 200, message = "Not Fetch Data" });
        }
        [HttpPost]
        public JsonResult LoginCheck(RequestLogin login)
        {
            if(string.IsNullOrWhiteSpace(login.email) && string.IsNullOrWhiteSpace(login.password))
            {
                return Json(new { status = 400, message = "Email and password are required" });
            }
            var user=db.User.FirstOrDefault(u=>u.email == login.email);
            if (user == null)
            {
                return Json(new { status = 404, message = "User not found" });
            }
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(login.password, user.password);
            if (!isPasswordValid)
            {
                return Json(new { status = 401, message = "Invalid password" });
            }
           var token = ChatApplication.Models.JwtHelper.GenerateToken(user.email, user.Id);
            return Json(new
            {
                status = 200,
                message = "Login successful",
                token=token
            });
        }
    }
}