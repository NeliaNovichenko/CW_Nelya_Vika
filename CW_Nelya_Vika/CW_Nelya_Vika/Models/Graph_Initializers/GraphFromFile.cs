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
        public Graph Initialize()
        {
            Graph graph = new Graph();
            try
            {      
                if (reader == null)
                    return null;
                int c = 0, min = 0, max = 0;
                string[] s1 = reader.ReadLine().Split(' ');
                Int32.TryParse(s1[0], out c);
                Int32.TryParse(s1[1], out min);
                Int32.TryParse(s1[2], out max);
                graph.CommunityCount = c;
                graph.MinCountInSubgraph = min;
                graph.MaxCountInSubgraph = max;

                while (reader.EndOfStream == false)
                {
                    string[] values = reader.ReadLine().Split(' ');
                    int idNodeOut, idNodeIn, weight;
                    if (!Int32.TryParse(values[0], out idNodeOut) || 
                        !Int32.TryParse(values[1], out idNodeIn) ||
                        !Int32.TryParse(values[2], out weight))
                        return null;
                    //true for add if such vertex doesn't exists in list
                    Vertex vertexA = graph.FindNode(idNodeOut, true);
                    Vertex vertexB = graph.FindNode(idNodeIn, true);
                    
                    graph.CreateLink(vertexA, vertexB, weight);
                }
            }
            catch (IOException ex)
            {
                //TODO: Handle this exception
                return null;
            }
            catch (Exception ex)
            {
                //TODO: Handle this exception
                return null;
            }

            return graph;
        }
    }
}