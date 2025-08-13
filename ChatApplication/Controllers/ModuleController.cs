using ChatApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ChatApplication.Controllers
{
    public class ModuleController : Controller
    {
        private AppDbContext db = new AppDbContext();
        // GET: Module
        public ActionResult Module()
        {
            return View();
        }
        [HttpGet]
        public JsonResult Get()
        {
            var module = db.Module.ToList();
            return Json(module,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Create(Module module)
        {
            if (ModelState.IsValid)
            {
                if(module.Id > 0)
                {
                    var existingModule=db.Module.Find(module.Id);
                    if(existingModule != null)
                    {
                        existingModule.current_date = DateTime.Now;
                        existingModule.module_icon= module.module_icon;
                        existingModule.module_name= module.module_name;
                        existingModule.href=module.href;
                        db.Entry(existingModule).State=EntityState.Modified;
                        db.SaveChanges();
                        return Json(new { status = 200, message = "Module Update Successfully" });
                    }
                }
                else
                {
                    module.current_date= DateTime.Now;
                    db.Module.Add(module);
                    db.SaveChanges();
                    return Json(new { status = 200, message = "Module Saved Successfully" });
                }
            }
            return Json(new { status = 400, message = "Invalid" });
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var module= db.Module.Find(id);
            if(module != null)
            {
                db.Module.Remove(module);
                db.SaveChanges();
                return Json(new { status = 200, message = "Module Delete Successfully" });
            }
            return Json(new { status = 400, message = "invalid" });
        }
        [HttpGet]
        public JsonResult Edit(int id)
        {
            var module = db.Module.Find(id);
            if (module != null)
            {
                return Json(module, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 400, message = "Not Fetch Data" }); 
        }
    }
}