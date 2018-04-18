using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CW_Nelya_Vika.Controllers
{
    public class ProblemGeneratorController : Controller
    {
        // GET: ProblemGenerator
        public ActionResult ProblemGenerator()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Generate(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
            }

            return RedirectToAction("OutputEditGraph");
        }

        public ActionResult OutputEditGraph()
        {
            return View();
        }


    }
}