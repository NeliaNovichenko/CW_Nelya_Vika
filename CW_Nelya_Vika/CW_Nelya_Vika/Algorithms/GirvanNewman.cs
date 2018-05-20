using System;
using System.Collections.Generic;
using System.Linq;
using CW_Nelya_Vika.Models;

namespace CW_Nelya_Vika.Algorithms
{
    public class GirvanNewman : AbstractAlgorithm
    {
        /// <summary>
        ///     Кількість найкоротших шляхів між усіма вершинами, що проходять через дане ребро
        /// </summary>
        private Dictionary<Edge, double> _edgeBetweenness;

        /// <summary>
        ///     List<Graph> result - результат розбиття
        /// </summary>
        private GraphList _graphList;

        /// <summary>
        ///     Вхідна функція виконання алгоритму
        /// </summary>
        /// <param name="pGraph"></param>
        /// <returns></returns>
        public override Problem FindCommunityStructure(Graph pGraph)
        {
            var start = DateTime.UtcNow;
            var p = new Problem
            {
                Graph = pGraph,
                Algorithm = Algorithm.GirvanNewman
            };
            _graph = pGraph.Clone();
            var tempCS = GetCommunityStructure();
            var initCount = tempCS.Count;
            var countCommunity = initCount;
            InitializeEdgeBetweenness(_graph);

            while (countCommunity < pGraph.CommunityCount)
            {
                while (countCommunity <= initCount)
                {
                    var community = RemoveMaxEdgeBetweenness(tempCS);

                    CalculateEdgeBetweenness(community);

                    tempCS = GetCommunityStructure();
                    countCommunity = tempCS.Count;
                }

                initCount = countCommunity;
                _graphList = tempCS;

                if (_graph.Edges.Count == 0)
                    break;
            }

            foreach (var g in _graphList)
                foreach (var v1 in g.Vertices)
                    foreach (var v2 in g.Vertices)
                    {
                        var v11 = _graph.FindNode(v1.Label, false);
                        var v22 = _graph.FindNode(v2.Label, false);
                        var e = pGraph.FindEdge(v11, v22);
                        if (e == null)
                            continue;
                        g.CreateLink(e.VertexOut, e.VertexIn, e.Weight);
                        g.Edges.Add(e);
                    }
            var end = DateTime.UtcNow;
            p.ExecutionTime = (end - start).Milliseconds;
            p.G = CalculateG(_graphList, pGraph);
            p.GraphList = _graphList;
            return p;
        }

        /// <summary>
        ///     Видалити ребро з найбільшим показником Betweenness
        /// </summary>
        /// <param name="pTempCS"></param>
        /// <returns></returns>
        private Graph RemoveMaxEdgeBetweenness(GraphList pTempCS)
        {
            if (_edgeBetweenness == null || _edgeBetweenness.Count == 0)
                return null;
            var max = _edgeBetweenness.Max(n => n.Value);
            var e = _edgeBetweenness.Where(n => Math.Abs(n.Value - max) < 1e-6).ToArray()[0].Key;

            _graph.Edges.Remove(e);

            e.VertexOut.AdjacencyEdges.Remove(e);
            e.VertexIn.AdjacencyEdges.Remove(e);

            e.VertexOut.GetAdjacencyVertices().Remove(e.VertexIn);
            e.VertexIn.GetAdjacencyVertices().Remove(e.VertexOut);

            _edgeBetweenness.Remove(e);

            foreach (var subgraph in pTempCS)
                if (subgraph.Vertices.Contains(e.VertexOut))
                    return subgraph;
            return null;
        }

        /// <summary>
        ///     Ініціалізуємо _edgeBetweenness та
        /// </summary>
        /// <param name="result"></param>
        /// <param name="community"></param>
        public void InitializeEdgeBetweenness(Graph community = null, Graph g = null)
        {
            if (g is null)
                g = _graph;
            if (_edgeBetweenness == null)
            {
                _edgeBetweenness = new Dictionary<Edge, double>();
                //Ініціалізуємо _edgeBetweenness: для всіх ребер спочатку значення = 0
                foreach (var e in g.Edges)
                    _edgeBetweenness.Add(e, 0);
            }

            CalculateEdgeBetweenness(community);
        }

        /// <summary>
        ///     Розраховує Betweenness для кожного ребра в графі
        /// </summary>
        /// <param name="subgraph"></param>
        public Dictionary<Edge, double> CalculateEdgeBetweenness(Graph subgraph)
        {
            foreach (var node1 in subgraph.Vertices)
                foreach (var node2 in subgraph.Vertices)
                {
                    var currEdge = subgraph.FindEdge(node1, node2);
                    var path = subgraph.DijkstraAlgorithm(node1, node2);
                    if (path is null)
                        continue;
                    for (var i = 0; i < path.Count - 1; i++)
                    {
                        Vertex nOut = subgraph.Vertices.Find(n => n.Label == path[i].Label),
                            nIn = subgraph.Vertices.Find(n => n.Label == path[i + 1].Label);
                        var tmpEdge = subgraph.FindEdge(nOut, nIn);
                        _edgeBetweenness[tmpEdge] += 1;
                    }
                }
            return _edgeBetweenness;
        }

        /// <summary>
        /// Знайти підграфи після видалення ребра
        /// </summary>
        /// <returns></returns>
        private GraphList GetCommunityStructure()
        {
            var cs = new GraphList();

            var count = 0;
            var n = _graph.Vertices.Count;
            var visited = new Dictionary<Vertex, bool>();
            for (var i = 0; i < n; i++)
                visited.Add(_graph.Vertices[i], false);
            // Якщо є "висячі" підграфи або вершини, додаємо їх до розбиття
            foreach (var i in _graph.Vertices)
                if (visited[i] == false)
                {
                    count++;
                    var subgraph = new Graph();

                    var Q = new Queue<Vertex>();
                    visited[i] = true;
                    Q.Enqueue(i);

                    //subgraph.Vertices.Add(i);
                    subgraph.FindNode(i.Label, true);

                    while (Q.Count != 0)
                    {
                        var v = Q.Dequeue();
                        foreach (var j in v.GetAdjacencyVertices())
                            if (visited[j] == false)
                            {
                                //subgraph.Vertices.Add(j);
                                subgraph.FindNode(j.Label, true);
                                var e = _graph.FindEdge(v, j);
                                subgraph.CreateLink(v, j, e.Weight);
                                visited[j] = true;
                                Q.Enqueue(j);
                            }
                    }
                    cs.Add(subgraph);
                }
            return cs;
        }
    }
}