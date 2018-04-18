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

        public int Weight { get; set; }

        /// <summary>
        /// Вершина 1, від якої починається ребро
        /// </summary>
        public Vertex VertexOut { get; set; }

        /// <summary>
        /// Вершина 2, в яку входить ребро
        /// </summary>
        public Vertex VertexIn { get; set; }

        public Edge()
        {
            VertexOut = new Vertex();
            VertexIn = new Vertex();
            Weight = 1;
        }
        public Edge(Vertex pVertexA, Vertex pVertexB)
        {
            VertexOut = pVertexA;
            VertexIn = pVertexB;
            Weight = 1;
        }
        public Edge(Vertex pVertexA, Vertex pVertexB, int weight)
        {
            VertexOut = pVertexA;
            VertexIn = pVertexB;
            Weight = weight;
        }
    }
}