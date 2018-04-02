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
        /// <summary>
        /// Ідентифікатор вершини
        /// </summary>
        [Key]
        public int Id { get; set; }
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
            NodeOut = new Node();
            NodeIn = new Node();
        }
        public Edge(Node pNodeA, Node pNodeB)
        {
            NodeOut = pNodeA;
            NodeIn = pNodeB;
        }
    }
}