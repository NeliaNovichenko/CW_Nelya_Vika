using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CW_Nelya_Vika.Models
{
    public class Node
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Вузли суміжності
        /// </summary>
        public List<Node> AdjacencyNodes { get; set; }
        /// <summary>
        /// Суміжні ребра
        /// </summary>
        public List<Edge> AdjacencyEdges { get; set; }

        public Node()
        {
            AdjacencyNodes = new List<Node>();
            AdjacencyEdges = new List<Edge>();
        }
    }
}