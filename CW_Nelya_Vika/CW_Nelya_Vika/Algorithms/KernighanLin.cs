using CW_Nelya_Vika.Models;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CW_Nelya_Vika.Algorithms
{
    public class KernighanLin : AbstractAlgorithm
    {
        struct Pair
        {
            public Vertex v1;
            public Vertex v2;
            public int delta_g;

            public Pair(Vertex ver1, Vertex ver2, int d)
            {
                v1 = ver1;
                v2 = ver2;
                delta_g = d;
            }
        }

        public void SetGraph(Graph g)
        {
            _graph = g;
        }
        public GraphList StartPartition(Graph pGraph)
        {
            GraphList r = new GraphList();
            int countVerticies = pGraph.Vertices.Count;
            int columnLength = (int)Math.Ceiling((decimal)(pGraph.Vertices.Count) / pGraph.CommunityCount);
            var list = pGraph.Vertices.Select((item, index) => new { index, item })
                       .GroupBy(x => x.index / columnLength)
                       .Select(x => x.Select(y => y.item).ToList()).ToList();
            foreach (var sublist in list)
            {
                Graph d = new Graph();
                d.Vertices = sublist;
                r.Add(d);
            }
            return r;

        }
        public Dictionary<Vertex, int> D(GraphList graphList)
        {
            Dictionary<Vertex, int> Dv = new Dictionary<Vertex, int>();
            for (int i = 0; i < graphList.Count; i++)
            {
                Graph subgraph = graphList[i];
                for (int j = 0; j < subgraph.Vertices.Count; j++)
                {
                    int innerEdge = 0, crossEdge = 0;

                    var adjNode = _graph.FindNode(subgraph.Vertices[j].Label, false).GetAdjacencyVertices();
                    foreach (var node in adjNode)
                    {
                        if (node.IsFixed == false)
                        {
                            var e = _graph.FindEdge(subgraph.Vertices[j], node);
                            if (e is null)
                            {
                                continue;
                            }
                            var w = e.Weight;
                            if (subgraph.FindNode(node.Label, false) != null)
                                innerEdge += w;
                            else crossEdge += w;
                        }
                    }
                    Dv.Add(subgraph.Vertices[j], crossEdge - innerEdge);
                }
            }
            return Dv;
        }
        public GraphList CountGrowth(Dictionary<Vertex, int> Dv, GraphList graphList)
        {

            List<Pair> deltag = new List<Pair>();
            for (int i = 0; i < graphList.Count; i++)
            {
                Graph subgraph = graphList[i];
                for (int j = 0; j < subgraph.Vertices.Count; j++)
                {
                    for (int k = 0; k < _graph.Vertices.Count; k++)
                    {
                        var w1 = _graph.FindNode(subgraph.Vertices[j].Label, false);
                        if (w1.IsFixed || _graph.Vertices[k].IsFixed) { continue; }
                        int w = 0;
                        if (subgraph.FindNode(_graph.Vertices[k].Label, false) == null)
                        {
                            var e = _graph.FindEdge(subgraph.Vertices[j], _graph.Vertices[k]);
                            if (e == null) w = 0;
                            else w = e.Weight;
                            int delta_g = Dv[_graph.Vertices[k]] + Dv[subgraph.Vertices[j]] - 2 * w;
                            deltag.Add(new Pair(subgraph.Vertices[j], _graph.Vertices[k], delta_g));
                        }
                    }
                }
            }
            if (deltag.Count == 0)
            {
                foreach (var item in _graph.Vertices)
                {
                    item.IsFixed = true;
                }
                return null;
            }
            int maxvalue = deltag[0].delta_g;
            Pair maxpair = deltag[0];
            for (int i = 0; i < deltag.Count; i++)
            {
                if (deltag[i].delta_g > maxvalue)
                {
                    maxvalue = deltag[i].delta_g;
                    maxpair = deltag[i];
                }
            }
            maxpair.v1.IsFixed = true;
            maxpair.v2.IsFixed = true;
            int sub1 = 0;
            int sub2 = 0;
            for (int i = 0; i < graphList.Count; i++)
            {
                Graph subgraph = graphList[i];
                var tmp1 = subgraph.FindNode(maxpair.v1.Label, false);
                if (tmp1 != null) { sub1 = i; }
            }
            for (int i = 0; i < graphList.Count; i++)
            {
                Graph subgraph = graphList[i];
                var tmp2 = subgraph.FindNode(maxpair.v2.Label, false);
                if (tmp2 != null) { sub2 = i; }
            }
            var t1 = graphList[sub1].Vertices.Find(x => x.Label == maxpair.v1.Label);
            var t2 = graphList[sub2].Vertices.Find(x => x.Label == maxpair.v2.Label);
            graphList[sub1].Vertices.Remove(t1);
            graphList[sub1].Vertices.Add(t2);
            graphList[sub1].Vertices.Find(x => x.Label == t2.Label).IsFixed = true;
            _graph.FindNode(t1.Label, false).IsFixed = true;
            graphList[sub2].Vertices.Remove(t2);
            graphList[sub2].Vertices.Add(t1);
            graphList[sub2].Vertices.Find(x => x.Label == t1.Label).IsFixed = true;
            _graph.FindNode(t2.Label, false).IsFixed = true;

            return graphList;
        }
        private bool IsAllFixed(List<Vertex> vertices)
        {
            foreach (var item in vertices)
            {
                if (item.IsFixed == false)
                    return false;
            }
            return true;
        }
        public override Problem FindCommunityStructure(Graph pGraph)
        {
            DateTime start = DateTime.UtcNow;
            Problem p = new Problem();
            p.Graph = pGraph;
            p.Algorithm = Algorithm.KernighanLin;
            _graph = pGraph.Clone();
            GraphList graphList;

            // начальное разбиение
            graphList = StartPartition(pGraph);
            while (IsAllFixed(_graph.Vertices) == false)
            {
                Dictionary<Vertex, int> Dv = D(graphList);
                GraphList graphlist1 = CountGrowth(Dv, graphList);
                if (graphlist1 == null)
                    continue;
                foreach (var sublist in graphlist1)
                {
                    foreach (var node1 in sublist.Vertices)
                    {
                        Console.Write(node1.Label);
                        Console.Write(' ');
                    }
                }

            }

            foreach (var g in graphList)
            {
                foreach (var v1 in g.Vertices)
                {
                    foreach (var v2 in g.Vertices)
                    {
                        Edge e = pGraph.FindEdge(v1, v2);
                        if (e == null)
                            continue;
                        g.CreateLink(e.VertexOut, e.VertexIn, e.Weight);
                        g.Edges.Add(e);
                    }
                }
            }
            DateTime end = DateTime.UtcNow;
            p.ExecutionTime = (end - start).Milliseconds;
            p.G = CalculateG(graphList, pGraph);
            p.GraphList = graphList;
            return p;
        }
    }
}
