﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatApplication.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult User()
        {
            return View();
        }
    }
}