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
            var problems = GraphProblemDb.Problems;
            var graphs = problems.Select(p => p.GraphList);
            var graphLists = graphs.ToList();
            foreach (var graphList in graphLists)
            {
                graphsFromLists.AddRange(graphList);
            }

            var graphsFromDb = GraphProblemDb.Graphs.Where(g => !graphsFromLists.Contains(g)).ToList();
            return View(graphsFromDb);
        }


        [HttpPost]
        public ActionResult ReadFromFile()
        {
            if (HttpContext.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Request.Files[0];

                if (httpPostedFile != null)
                {
                    var index = httpPostedFile.FileName.LastIndexOf(@"\");
                    var shotName = httpPostedFile.FileName.Substring(index == -1? 0: index);
                    var fileSavePath = HttpContext.Server.MapPath("~/UploadedFiles") + shotName;

                    httpPostedFile.SaveAs(fileSavePath);
                    IGraphInitializer graphInitializer = new GraphFromFile(fileSavePath);
                    graph = graphInitializer.Initialize();

                }
            }

            return RedirectToAction("OutputEditGraph", graph);
        }

        [HttpPost]
        public ActionResult Generate(FormCollection fc)
        {
            int commCount = Convert.ToInt32(fc.GetValue("communityCount").AttemptedValue);
            int minCommCount = Convert.ToInt32(fc.GetValue("minCommunityCount").AttemptedValue);
            int maxCommCount = Convert.ToInt32(fc.GetValue("maxCommunityCount").AttemptedValue);
            var problemClassification = ProblemClassification.Xs;
            switch (fc.GetValue("GraphClassification").AttemptedValue)
            {
                case "XS":
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
                case "XL":
                    problemClassification = ProblemClassification.Xl;
                    break;
            }
            IGraphInitializer graphInitializer = new GraphGenerator(problemClassification, commCount, minCommCount, maxCommCount);
            graph = graphInitializer.Initialize();

            return RedirectToAction("OutputEditGraph", graph);
        }

        [HttpPost]
        public ActionResult ReadFromDb(FormCollection fc)
        {
            int gId = Convert.ToInt32(fc.GetValue("GraphId").AttemptedValue);
            graph = GraphProblemDb.GetGraph(gId);

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
                    algorithm = new GirvanNewman();
                    communities = algorithm.FindCommunityStructure(graph);
                    break;
            }
            problem.GraphList = communities;

            //TODO: перейти на страничку и передать парам
            return RedirectToAction("GraphListResult", "GraphListResult", problem);
        }



    }
}