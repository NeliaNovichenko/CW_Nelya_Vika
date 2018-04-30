using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CW_Nelya_Vika.Controllers
{
    public class OutputResultController : Controller
    {
        //db = new db
        //db.Problems

        // GET: OutputResult
        public ActionResult Index()
        {
            return View(List<problems>);
        }
    }
}