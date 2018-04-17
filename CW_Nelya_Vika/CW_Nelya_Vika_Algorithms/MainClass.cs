using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CW_Nelya_Vika.Models;
using CW_Nelya_Vika.Models.Graph_Initializers;

namespace CW_Nelya_Vika_Algorithms
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var filepath = System.IO.Path.GetFullPath(@"Data\graph2.txt");

            GirvanNewman gn = new GirvanNewman();
            KernighanLin kl = new KernighanLin();

            IGraphInitializer graphInitializer = new GraphFromFile(filepath);
            Graph g = graphInitializer.Initialize();

            Result resuslt = gn.FindCommunityStructure(g);
            Result result_kl = kl.FindCommunityStructure(g);

            Console.WriteLine(gn.Log);
            
            Console.ReadLine();
        }
    }
}
