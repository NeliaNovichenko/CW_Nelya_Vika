using CW_Nelya_Vika.Models;
using System.IO;
using System;

namespace CW_Nelya_Vika_Algorithms
{
    public class KernighanLin: IAlgorithm
    {
        public void StartPartition(Graph pGraph, int verticies, int Number)
        {

            int columnLength = (int)Math.Ceiling((decimal)(verticies) / Number);
            var list = pgraph.Select((item, index) => new { index, item })
                       .GroupBy(x => x.index / columnLength)
                       .Select(x => x.Select(y => y.item));
        }
        public Result FindCommunityStructure(Graph pGraph)
        {
            int countVerticies = pGraph.Nodes.Count;
            Console.WriteLine("Введіть кількість підграфів:");
            int Number = Convert.ToInt32(Console.ReadLine());
            StartPartition(pGraph, Number);
            //throw new System.NotImplementedException();
        }
    }
}
