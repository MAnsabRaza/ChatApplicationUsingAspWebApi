using ChatApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;

namespace ChatApplication.Controllers
{
    public class PermissionController : Controller
    {
        private AppDbContext db=new AppDbContext();
        // GET: Permission
        public ActionResult Permission()
        {
            return View();
        }
        [HttpGet]
        public JsonResult Get()
        {
            var permission = db.Permission.ToList();
            return Json(permission,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Create(Permission permission)
        {
            if (ModelState.IsValid)
            {
                if(permission.Id > 0)
                {
                    var existingPermission = db.Permission.Find(permission.Id);
                    if(existingPermission != null)
                    {
                        existingPermission.current_date = DateTime.Now;
                        existingPermission.status = permission.status;
                        existingPermission.roleId = permission.roleId;
                        existingPermission.moduleId = permission.moduleId;
                        db.Entry(existingPermission).State=System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return Json(new { status = 200, message = "Permission Update successfully" });

                    }
                }
                else
                {
                    permission.current_date= DateTime.Now;
                    db.Permission.Add(permission);
                    db.SaveChanges();
                    return Json(new { status = 200, message = "Permission Saved successfully" });
                }
            }
            return Json(new { status = 400, message = "Permission Invalid" });
        }
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var permission = db.Permission.Find(id);
            if(permission != null)
            {
                db.Permission.Remove(permission);
                db.SaveChanges();
                return Json(new { status = 200, message = "Permission Deleted successfully" });
            }
            return Json(new { status = 400, message = "Invalid" });
        }

        [HttpGet]
        public JsonResult Edit(int id)
        {
            var permission = db.Permission.Find(id);
            if (permission != null)
            {
                return Json(permission, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = 400,
                message = "Permission Not Fetched"
            });
            }
        
    }
}