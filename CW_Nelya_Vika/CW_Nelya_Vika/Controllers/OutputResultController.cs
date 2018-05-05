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
        static List<Problem> problemsFromDb = GraphProblemDb.Problems.ToList();
        public ActionResult OutputResult()
        {
            return View(problemsFromDb);
        }

        [HttpPost]
        public ActionResult ShowResult(FormCollection fc)
        {
            int problemId = Convert.ToInt32(fc.GetValue("ResultId").AttemptedValue);
            Problem selected = problemsFromDb.FirstOrDefault(p => p.Id == problemId);
            ProblemGeneratorController.problem = selected;
            return RedirectToAction("GraphListResult", "GraphListResult");

        }
    }
}