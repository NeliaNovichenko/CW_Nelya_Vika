using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CW_Nelya_Vika.Models;
using CW_Nelya_Vika.Models.DB;
using CW_Nelya_Vika.Models.Graph_Initializers;

namespace CW_Nelya_Vika_Algorithms
{
    class MainClass
    {
        static string result;
        public static void Main(string[] args)
        {
            var filepath = System.IO.Path.GetFullPath(@"D:\Studing\ТРПЗ\Курсова робота\CW_Nelya_Vika\CW_Nelya_Vika\CW_Nelya_Vika_Algorithms\Data\graph1.txt");

            GirvanNewman gn = new GirvanNewman();
            //KernighanLin kl = new KernighanLin();
            //Graph g = GraphProblemDb.GetGraph(1);
            //Console.WriteLine(g);
            //IGraphInitializer graphInitializer = new GraphGenerator(ProblemClassification.Xs);

            IGraphInitializer graphInitializer = new GraphFromFile(filepath);
            Graph g1 = graphInitializer.Initialize();

            g1.CommunityCount = 5;
            GraphList resuslt = gn.FindCommunityStructure(g1);

            //Problem problem1 = new Problem();
            //problem1.Graph = g1;
            //problem1.Algorithm = Algorithm.GirvanNewman;
            //problem1.GraphList = resuslt;

            ////GraphProblemDb.AddProblem(problem1);

            //GraphList result_kl = kl.FindCommunityStructure(g1);

            //Problem problem2 = new Problem();
            //problem2.Graph = g1;
            //problem2.Algorithm = Algorithm.KernighanLin;
            //problem2.GraphList = result_kl;
            //GraphProblemDb.AddProblem(problem2);


            //GraphProblemDb.CloseConnection();

            Console.WriteLine(gn.Log);

            Console.ReadLine();
        }
    }
}
