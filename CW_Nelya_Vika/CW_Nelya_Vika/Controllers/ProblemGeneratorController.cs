using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CW_Nelya_Vika.Models;

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
            Graph g = new Graph();
            g.Edges = new List<Edge>()
            {
                new Edge(new Vertex(){Label = 1}, new Vertex(){Label = 2}, 3),
                new Edge(new Vertex(){Label = 1}, new Vertex(){Label = 3}, 2),
                new Edge(new Vertex(){Label = 2}, new Vertex(){Label = 3}, 5)
            };
            g.Vertices = new List<Vertex>()
            {
                new Vertex(){Label = 1},
                new Vertex(){Label = 2},
                new Vertex(){Label = 3}
            };
            return View(g);
        }

        public ActionResult Solve(HttpPostedFileBase file)
        {
            
            return RedirectToAction("OutputEditGraph");
        }



    }
}