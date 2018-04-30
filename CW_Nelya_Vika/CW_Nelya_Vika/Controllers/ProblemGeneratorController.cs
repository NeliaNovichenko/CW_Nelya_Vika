using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CW_Nelya_Vika.Models;
using CW_Nelya_Vika.Models.DB;
using CW_Nelya_Vika.Models.Graph_Initializers;
using CW_Nelya_Vika_Algorithms;

namespace CW_Nelya_Vika.Controllers
{
    public class ProblemGeneratorController : Controller
    {
        static Graph graph = new Graph();

        // GET: ProblemGenerator
        public ActionResult ProblemGenerator()
        {
            var graphsFromDb = GraphProblemDb.Problems.Select(p => p.Graph).ToList();
            return View(graphsFromDb);
        }

        [HttpPost]
        public ActionResult Generate(FormCollection fc)
        {
            IGraphInitializer graphInitializer = new GraphGenerator();
            var generationType =fc.GetValue("GenerationType");
            
            switch (generationType.AttemptedValue)
            {
                case "ReadFromFile":
                    //if (file != null && file.ContentLength > 0)
                    //{
                    //    var fileName = Path.GetFileName(file.FileName);
                    //}
                    var file = fc.GetValue("file").AttemptedValue;
                    var filePath = Path.GetFileName(file);
                    graphInitializer = new GraphFromFile(filePath);
                    graph = graphInitializer.Initialize();
                    break;
                case "Generate":
                    int commCount = Convert.ToInt32(fc.GetValue("communityCount").AttemptedValue);
                    int minCommCount = Convert.ToInt32(fc.GetValue("minCommunityCount").AttemptedValue);
                    int maxCommCount = Convert.ToInt32(fc.GetValue("maxCommunityCount").AttemptedValue);
                    var problemClassification = (ProblemClassification)Convert.ToInt32(fc.GetValue("GraphClassification").AttemptedValue);
                    graphInitializer = new GraphGenerator(problemClassification, commCount, minCommCount, maxCommCount);
                    graph = graphInitializer.Initialize();
                    break;
                case "ReadFromDb":
                    int gId = Convert.ToInt32(fc.GetValue("GraphId").AttemptedValue);
                    graph = GraphProblemDb.GetGraph(gId);
                    break;
            }
            
            return RedirectToAction("OutputEditGraph");
        }


        public ActionResult OutputEditGraph()
        {
            return View(graph);
        }

        public ActionResult Solve(FormCollection fc)
        {
            Problem p = new Problem();
            p.Graph = graph;
            GraphList communities = new GraphList();
            switch (fc.GetValue("Algorithm").AttemptedValue)
            {
                case "KernighanLin":
                    p.Algorithm = Algorithm.KernighanLin;
                    IAlgorithm algorithm = new KernighanLin();
                    communities = algorithm.FindCommunityStructure(graph);
                    break;
                case "GirvanNewman":
                    p.Algorithm = Algorithm.GirvanNewman;
                    algorithm = new KernighanLin();
                    communities = algorithm.FindCommunityStructure(graph);
                    break;
            }
            p.GraphList = communities;

            //TODO: перейти на страничку и передать парам
            return RedirectToAction("GraphListResultController/GraphListResult", p);
        }



    }
}