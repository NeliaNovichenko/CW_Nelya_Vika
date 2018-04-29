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
            
            return View("GraphListResult");
        }

        public ActionResult GraphListResult()
        {
            //Graph g = GraphProblemDb.GetGraph(1);
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

    }
}