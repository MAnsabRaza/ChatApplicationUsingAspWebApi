using ChatApplication.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ChatApplication.Controllers
{
    public class PermissionController : Controller
    {
        private AppDbContext db = new AppDbContext();

        public ActionResult Permission()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Get()
        {
            var permission = db.Permission
                .Include(r => r.Role)
                .Include(m => m.Module)
                .Select(p => new
                {
                    p.Id,
                    current_date = p.current_date,
                    p.status,
                    roleId = p.roleId,
                    roleName = p.Role.role_name,
                    moduleId = p.moduleId,
                    moduleName = p.Module.module_name
                })
                .ToList()
                .Select(p => new
                {
                    p.Id,
                    current_date = p.current_date.ToString("yyyy-MM-dd"),
                    p.status,
                    roleId = p.roleId,
                    roleName = p.roleName,
                    moduleId = p.moduleId,
                    moduleName = p.moduleName
                });
            return Json(permission, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(Permission permission)
        {
            if (ModelState.IsValid)
            {
                if (permission.Id > 0)
                {
                    var existingPermission = db.Permission.Find(permission.Id);
                    if (existingPermission != null)
                    {
                        existingPermission.current_date = permission.current_date;
                        existingPermission.status = permission.status;
                        existingPermission.roleId = permission.roleId;
                        existingPermission.moduleId = permission.moduleId;
                        db.Entry(existingPermission).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json(new { status = 200, message = "Permission updated successfully" });
                    }
                }
                else
                {
                    permission.current_date = DateTime.Now;
                    db.Permission.Add(permission);
                    db.SaveChanges();
                    return Json(new { status = 200, message = "Permission saved successfully" });
                }
            }
            return Json(new { status = 400, message = "Invalid data" });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var permission = db.Permission.Find(id);
            if (permission != null)
            {
                db.Permission.Remove(permission);
                db.SaveChanges();
                return Json(new { status = 200, message = "Permission deleted successfully" });
            }
            return Json(new { status = 400, message = "Invalid request" });
        }

        [HttpGet]
        public JsonResult Edit(int id)
        {
            var permission = db.Permission.Find(id);
            if (permission != null)
            {
                var result = new
                {
                    permission.Id,
                    current_date = permission.current_date.ToString("yyyy-MM-dd"),
                    permission.status,
                    permission.roleId,
                    permission.moduleId
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 400, message = "Permission not found" });
        }
    }
}