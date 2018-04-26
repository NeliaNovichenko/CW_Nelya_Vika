using CW_Nelya_Vika.Models;
using CW_Nelya_Vika.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CW_Nelya_Vika.Controllers
{
    public class GraphListResultController : Controller
    {
        // GET: GraphList
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GraphListResult()
        {
            Graph g = GraphProblemDb.GetGraph(1);
            ViewBag.graph = g.ToString();
            return View();
        }

        //public ActionResult Show(HttpPostedFileBase file)
        //{
        //    return Content("Hi there!");
        //}
    }
}