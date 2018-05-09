using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CW_Nelya_Vika.Models;
using CW_Nelya_Vika_Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CW_Nelya_Vika_Test
{
    [TestClass]
    public class GirvanNewmanTest
    {
        [TestMethod]
        public void CalculateEdgeBetweenness_Test()
        {
            Graph graph = new Graph();
            Vertex vertexA = graph.FindNode(1, true);
            Vertex vertexB = graph.FindNode(2, true);
            Vertex vertexC = graph.FindNode(3, true);

            graph.CreateLink(vertexA, vertexB, 2);
            graph.CreateLink(vertexA, vertexC, 6);
            graph.CreateLink(vertexB, vertexC, 5);
            graph.CommunityCount = 2;

            GirvanNewman gn = new GirvanNewman();

            var expected = new Dictionary<Edge, double>();
            expected.Add(graph.FindEdge(vertexA, vertexB), 2);
            expected.Add(graph.FindEdge(vertexA, vertexC), 2);
            expected.Add(graph.FindEdge(vertexB, vertexC), 2);

            gn.InitializeEdgeBetweenness(graph, graph);
            var actual = gn.CalculateEdgeBetweenness(graph);

            //assert
            var expectedKeys = expected.Keys.ToList();
            var actualKeys = actual.Keys.ToList();

            for (int i = 0; i < expected.Count; i++)
            {
               Assert.AreEqual(expected[expectedKeys[i]], actual[actualKeys[i]]);
            }
        }
    }
}
