using System;
using System.Collections.Generic;
using CW_Nelya_Vika.Algorithms;
using CW_Nelya_Vika.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace CW_Nelya_Vika_Test
{
    [TestClass]
    public class KernighanLinTest
    {
        [TestMethod]
        public void StartPartition_Test()
        {
            Graph graph = new Graph();
            Vertex vertexA = graph.FindNode(1, true);
            Vertex vertexB = graph.FindNode(2, true);
            Vertex vertexC = graph.FindNode(3, true);

            graph.CreateLink(vertexA, vertexB, 2);
            graph.CreateLink(vertexA, vertexC, 6);
            graph.CreateLink(vertexB, vertexC, 5);
            graph.CommunityCount = 2;
            //arrange
            GraphList expected = new GraphList() {
            new Graph()
            {
                Vertices = new List<Vertex>()
                    {
                        vertexA,
                        vertexB
                    }
            },
                    new Graph()
                    {
                        Vertices = new List<Vertex>()
                        {
                            vertexC
                        }
                    }
                    };
            KernighanLin kernighanLin = new KernighanLin();
            kernighanLin.SetGraph(graph);

            //act
            GraphList actual = kernighanLin.StartPartition(graph);

            //assert
            for (int i = 0; i < expected.Count; i++)
            {
                for (int j = 0; j < expected[i].Vertices.Count; j++)
                {
                    Assert.AreEqual(expected[i].Vertices[j].Label, actual[i].Vertices[j].Label);
                }
            }
        }
    }
}
