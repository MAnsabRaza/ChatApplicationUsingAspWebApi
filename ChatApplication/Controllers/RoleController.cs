using ChatApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ChatApplication.Controllers
{
    public class RoleController : Controller
    {
        private AppDbContext db = new AppDbContext();
        // GET: Role
        public ActionResult Role()
        {
            return View();
        }
        [HttpGet]
        public JsonResult Get()
        {
            var role = db.Role.ToList();
            return Json(role,JsonRequestBehavior.AllowGet); 
        }
        [HttpPost]
        public ActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {

                if (role.Id > 0)
                {
                    var existingRole = db.Role.Find(role.Id);
                    if (existingRole != null)
                    {
                        existingRole.current_date = DateTime.Now;
                        existingRole.role_name = role.role_name;
                        db.Entry(existingRole).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json(new { status = 200, message = "Role updated successfully" });
                    }
                }
                else
                {
                    role.current_date = DateTime.Now;
                    db.Role.Add(role);
                    db.SaveChanges();
                    return Json(new { status = 200, message = "Role Saved Successfully" });
                }
            }
            return Json(new { status = 400, message = "Role invalid" });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var role = db.Role.Find(id);
            if(role != null)
            {
                db.Role.Remove(role);
                db.SaveChanges();
                return Json(new { status = 200, message = "Role Deleted Successfully" });

            }
            return Json(new { status = 400, message = "Role Not Deleted" });
        }
        [HttpGet]
        public JsonResult Edit(int id)
        {
            var role = db.Role.Find(id);
            if(role != null)
            {
                return Json(role ,JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 400, message = "Not Fetch Data" });
        }
    }
}