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
            GirvanNewman gn = new GirvanNewman();
            IGraphInitializer graphInitializer = new GraphFromFile(@"D:\Studing\ТРПЗ\Курсова робота\CW_Nelya_Vika\CW_Nelya_Vika\CW_Nelya_Vika_Algorithms\Data\graph1.txt");
            Graph g = graphInitializer.Initialize();
            Result resuslt = gn.FindCommunityStructure(g);
            Console.WriteLine(gn.Log);
            
            Console.ReadLine();
        }
    }
}
