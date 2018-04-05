using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CW_Nelya_Vika.Models.Graph_Initializers
{
    public class GraphFromFile : IGraphInitializer
    {
        private StreamReader reader;

        public GraphFromFile(string path)
        {
            reader = new StreamReader(path);
        }

        /// <summary>
        /// Read from file
        /// </summary>
        public bool Initialize(Graph graph)
        {
            try
            {
                graph = new Graph();
                graph.Edges = new List<Edge>();
                graph.Nodes = new List<Node>();

                if (reader == null)
                    return false;

                while (reader.EndOfStream == false)
                {
                    string[] nodes = reader.ReadLine().Split(' ');
                    int idNodeOut, idNodeIn;
                    if (!Int32.TryParse(nodes[0], out idNodeOut) || !Int32.TryParse(nodes[1], out idNodeIn))
                        return false;
                    //true for add if such node doesn't exists in list
                    Node nodeA = graph.FindNode(idNodeOut, true);
                    Node nodeB = graph.FindNode(idNodeIn, true);

                    graph.CreateLink(nodeA, nodeB);
                }
            }
            catch (IOException)
            {
                //TODO: Handle this exception
                return false;
            }
            catch (Exception ex)
            {
                //TODO: Handle this exception
                return false;
            }

            return true;
        }
    }
}