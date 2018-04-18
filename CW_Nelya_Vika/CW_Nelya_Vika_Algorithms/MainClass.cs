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
        public static void Main(string[] args)
        {
            GraphProblemDBContext dbContext = new GraphProblemDBContext();

            var filepath = System.IO.Path.GetFullPath(@"D:\Studing\ТРПЗ\Курсова робота\CW_Nelya_Vika\CW_Nelya_Vika\CW_Nelya_Vika_Algorithms\Data\graph1.txt");

            GirvanNewman gn = new GirvanNewman();
            KernighanLin kl = new KernighanLin();

            IGraphInitializer graphInitializer = new GraphFromFile(filepath);
            Graph g = graphInitializer.Initialize();

            //SaveToDb

            dbContext.Edges.AddRange(g.Edges);
            //dbContext.Vertices.AddRange(g.Vertices);

            dbContext.SaveChanges();
            //dbContext.Graphs.Add(g);

            Result resuslt = gn.FindCommunityStructure(g);
            Result result_kl = kl.FindCommunityStructure(g);

            Console.WriteLine(gn.Log);
            
            Console.ReadLine();
        }
    }
}
