using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CW_Nelya_Vika.Models
{
    /// <summary>
    /// Класс, що описує ребро графу
    /// </summary>
    public class Edge
    {
        private static int count = 0;
        /// <summary>
        /// Ідентифікатор вершини
        /// </summary>
        [Key]
        public int Id { get; set; }

        public int Weight { get; set; }
        /// <summary>
        /// Вершина 1, від якої починається ребро
        /// </summary>
        public Node NodeOut { get; set; }
        /// <summary>
        /// Вершина 2, в яку входить ребро
        /// </summary>
        public Node NodeIn { get; set; }

        public Edge()
        {
            Id = count++;
            NodeOut = new Node();
            NodeIn = new Node();
            Weight = 1;
        }
        public Edge(Node pNodeA, Node pNodeB)
        {
            Id = count++;
            NodeOut = pNodeA;
            NodeIn = pNodeB;
            Weight = 1;
        }
        public Edge(Node pNodeA, Node pNodeB, int weight)
        {
            Id = count++;
            NodeOut = pNodeA;
            NodeIn = pNodeB;
            Weight = weight;
        }
    }
}