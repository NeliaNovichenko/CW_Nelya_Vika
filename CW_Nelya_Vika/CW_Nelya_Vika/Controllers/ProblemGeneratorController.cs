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
        public static Graph graph = new Graph();
        public static Problem problem = new Problem();
        public ActionResult ProblemGenerator()
        {
            List<Graph> graphsFromLists = new List<Graph>();
            var graphLists = GraphProblemDb.Problems.Select(p => p.GraphList).ToList();
            foreach (var graphList in graphLists)
            {
                graphsFromLists.AddRange(graphList);
            }

            var graphsFromDb = GraphProblemDb.Graphs.Where(g => !graphsFromLists.Contains(g)).ToList();
            return View(graphsFromDb);
        }


        [HttpPost]
        public ActionResult Generate(FormCollection fc)
        {
            //Graph graph = new Graph();
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
                    var filePath = Path.GetFullPath(file);
                    //var filePath = Path.GetFileName(file);
                    graphInitializer = new GraphFromFile(filePath);
                    graph = graphInitializer.Initialize();
                    break;
                case "Generate":
                    int commCount = Convert.ToInt32(fc.GetValue("communityCount").AttemptedValue);
                    int minCommCount = Convert.ToInt32(fc.GetValue("minCommunityCount").AttemptedValue);
                    int maxCommCount = Convert.ToInt32(fc.GetValue("maxCommunityCount").AttemptedValue);
                    var problemClassification = ProblemClassification.Xs;
                    switch (fc.GetValue("GraphClassification").AttemptedValue)
                    {
                        case "Xs":
                            problemClassification = ProblemClassification.Xs;
                            break;
                        case "S":
                            problemClassification = ProblemClassification.S;
                            break;
                        case "M":
                            problemClassification = ProblemClassification.M;
                            break;
                        case "L":
                            problemClassification = ProblemClassification.L;
                            break;
                        case "Xl":
                            problemClassification = ProblemClassification.Xl;
                            break;
                    }
                    graphInitializer = new GraphGenerator(problemClassification, commCount, minCommCount, maxCommCount);
                    graph = graphInitializer.Initialize();
                    break;
                case "ReadFromDb":
                    int gId = Convert.ToInt32(fc.GetValue("GraphId").AttemptedValue);
                    graph = GraphProblemDb.GetGraph(gId);
                    break;
            }
            
            return RedirectToAction("OutputEditGraph", graph);
        }


        public ActionResult OutputEditGraph()
        {
            return View(graph);
        }

        [HttpPost]
        public ActionResult SaveToDb()
        {
            GraphProblemDb.AddGraph(graph);
            return View("OutputEditGraph", graph);
        }

        public ActionResult Solve(FormCollection fc)
        {
            problem.Graph = graph;
            GraphList communities = new GraphList();
            switch (fc.GetValue("Algorithm").AttemptedValue)
            {
                case "KernighanLin":
                    problem.Algorithm = Algorithm.KernighanLin;
                    IAlgorithm algorithm = new KernighanLin();
                    communities = algorithm.FindCommunityStructure(graph);
                    break;
                case "GirvanNewman":
                    problem.Algorithm = Algorithm.GirvanNewman;
                    algorithm = new KernighanLin();
                    communities = algorithm.FindCommunityStructure(graph);
                    break;
            }
            problem.GraphList = communities;

            //TODO: перейти на страничку и передать парам
            return RedirectToAction("GraphListResult", "GraphListResult", problem);
        }



    }
}