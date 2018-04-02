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

        public GraphReader(string fileName)
        {
            StreamReader = new StreamReader(fileName,true);
        }

        /// <summary>
        /// Read from file
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="edges"></param>
        public void Initialize(List<Node> nodes, List<Edge> edges)
        {
            throw new NotImplementedException();
        }
    }
}