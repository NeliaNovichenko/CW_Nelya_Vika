using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CW_Nelya_Vika.Models
{
    public class Node
    {
        //private static int count = 0;
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
            //Id = count++;
            AdjacencyNodes = new List<Node>();
            AdjacencyEdges = new List<Edge>();
        }
    }
}