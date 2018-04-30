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
        public ActionResult Index(Problem problem)
        { 
            
            return View("GraphListResult", problem);
        }

        public ActionResult GraphListResult(Problem problem)
        {
            //Problem p = new Problem();
            //Graph initialGraph = new Graph();
            //initialGraph.Edges = new List<Edge>()
            //{
            //    new Edge(new Vertex(){Label = 1}, new Vertex(){Label = 2}, 3),
            //    new Edge(new Vertex(){Label = 1}, new Vertex(){Label = 4}, 2),
            //    new Edge(new Vertex(){Label = 2}, new Vertex(){Label = 3}, 5),
            //    new Edge(new Vertex(){Label = 4}, new Vertex(){Label = 6}, 7),
            //    new Edge(new Vertex(){Label = 4}, new Vertex(){Label = 3}, 2),
            //    new Edge(new Vertex(){Label = 3}, new Vertex(){Label = 1}, 5),
            //    new Edge(new Vertex(){Label = 5}, new Vertex(){Label = 4}, 9)
            //};
            //initialGraph.Vertices = new List<Vertex>()
            //{
            //    new Vertex(){Label = 1},
            //    new Vertex(){Label = 2},
            //    new Vertex(){Label = 3},
            //    new Vertex(){Label = 4},
            //    new Vertex(){Label = 5},
            //    new Vertex(){Label = 6}
            //};

            //p.Graph = initialGraph;
            //p.GraphList = new GraphList()
            //{
            //    new Graph() {Vertices = new List<Vertex>(){new Vertex() { Label = 1}, new Vertex() { Label = 2} }},
            //    new Graph() {Vertices = new List<Vertex>(){new Vertex() { Label = 3} }},
            //    new Graph() {Vertices = new List<Vertex>(){new Vertex() { Label = 4}, new Vertex() { Label = 5} }},
            //    new Graph() {Vertices = new List<Vertex>(){new Vertex() { Label = 6} }},
            //};

            return View(problem);
        }

    }
}