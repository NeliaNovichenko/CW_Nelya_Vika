using CW_Nelya_Vika.Models;

namespace CW_Nelya_Vika.Algorithms
{
    /// <summary>
    /// Усі алгоритми приймають однаковій набір параметрів та return type
    /// </summary>
    public abstract class AbstractAlgorithm
    {
        /// <summary>
        ///     Граф, для якого проводиться розбиття
        /// </summary>
        protected Graph _graph;

        /// <summary>
        /// Знаходить вагу перерізу
        /// </summary>
        /// <param name="graphs"></param>
        /// <param name="original"></param>
        /// <returns></returns>
        public int CalculateG(GraphList graphs, Graph original)
        {
            var g = 0;
            foreach (var subgraph in graphs)
            foreach (var v1 in subgraph.Vertices)
            {
                var tmp = original.FindNode(v1.Label, false);
                foreach (var v2 in original.Vertices)
                {
                    var e = original.FindEdge(tmp, v2);
                    if (e != null)
                    {
                        var v = subgraph.FindNode(v2.Label, false);
                        if (v == null)
                            g += e.Weight;
                    }
                }
            }
            return g / 2;
        }

        public abstract Problem FindCommunityStructure(Graph pGraph);
    }
}
