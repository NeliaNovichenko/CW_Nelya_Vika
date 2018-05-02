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
        //Problem p = new Problem()
        //{
        //    Graph = new Graph()
        //    {
        //        Vertices = new List<Vertex>()
        //        {
        //            new Vertex() {Label = 1},
        //            new Vertex() {Label = 2},
        //            new Vertex() {Label = 3}
        //        },
        //        Edges = new List<Edge>()
        //        {
        //            new Edge(){VertexOut = new Vertex() {Label = 1}, VertexIn =  new Vertex() {Label = 2}, Weight = 2},
        //            new Edge(){VertexOut = new Vertex() {Label =2}, VertexIn =  new Vertex() {Label = 3}, Weight = 5},
        //            new Edge(){VertexOut = new Vertex() {Label = 1}, VertexIn =  new Vertex() {Label = 3}, Weight = 6}
        //        }
        //    },
        //    GraphList = new GraphList()
        //    {
        //        new Graph()
        //        {
        //            Vertices = new List<Vertex>()
        //            {
        //                new Vertex() {Label = 1},
        //                new Vertex() {Label = 2}
        //            }
        //        },
        //            new Graph() {
        //                Vertices = new List<Vertex>()
        //                {
        //                    new Vertex() {Label = 3}
        //                }
        //            }
        //    }

        //};
        // GET: GraphList
        public ActionResult Index()
        {
            return View("GraphListResult", ProblemGeneratorController.problem);
        }

        public ActionResult GraphListResult()
        {
            return View(ProblemGeneratorController.problem);
        }

    }
}