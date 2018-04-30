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
            var filepath = System.IO.Path.GetFullPath(@"Data\graph1.txt");

            GirvanNewman gn = new GirvanNewman();
            KernighanLin kl = new KernighanLin();
            Graph g = GraphProblemDb.GetGraph(1);
            Console.WriteLine(g);

            //IGraphInitializer graphInitializer = new GraphFromFile(filepath);
            //Graph g = graphInitializer.Initialize();

            //GraphList resuslt = gn.FindCommunityStructure(g);
            GraphList result_kl = kl.FindCommunityStructure(g);

            Console.WriteLine(gn.Log);

            Console.ReadLine();
        }
    }
}
