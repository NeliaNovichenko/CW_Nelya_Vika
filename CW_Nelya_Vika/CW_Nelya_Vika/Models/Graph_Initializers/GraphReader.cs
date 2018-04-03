using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CW_Nelya_Vika.Models.Graph_Initializers
{
    public class GraphReader : IGraphInitializer
    {
        /// <summary>
        /// TODO: Choose some file reader here!
        /// </summary>
        public StreamReader StreamReader { get; set; }
        public Graph Graph { get; private set; }

        public GraphReader(string fileName)
        {
            StreamReader = new StreamReader(fileName,true);
        }
        
        public GraphReader(Graph g)
        {
            if (g is null)
                g = new Graph();
            if (g.Edges is null)
                g.Edges = new List<Edge>();
            if (g.Nodes is null)
                g.Nodes = new List<Node>();
            g.Nodes = new List<Node>();
            Graph = g;
        }

        /// <summary>
        /// Read from file
        /// </summary>
        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}