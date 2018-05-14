using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CW_Nelya_Vika.Models;
using CW_Nelya_Vika.Models.DB;

namespace CW_Nelya_Vika.Controllers
{
    public class OutputResultController : Controller
    {
        public OutputResultController()
        {
            GraphProblemDb.Update();
        }
        public ActionResult OutputResult()
        {
            return View(GraphProblemDb.Problems);
        }

        [HttpPost]
        public ActionResult ShowResult(FormCollection fc)
        {
            GraphProblemDb.Update();
            int problemId = Convert.ToInt32(fc.GetValue("ResultId").AttemptedValue);
            Problem selected = GraphProblemDb.GetProblem(problemId);
            GraphProblemDb.CurrentProblem = selected;
            return RedirectToAction("GraphListResult", "GraphListResult", GraphProblemDb.CurrentProblem);

        }
    }
}