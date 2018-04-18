using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.Entity;

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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Weight { get; set; }
        public List<Vertex> Verteces { get; set; }
        public Graph Graphs { get; set; }

        /// <summary>
        /// Вершина 1, від якої починається ребро
        /// </summary>
        public Vertex VertexOut { get { return Verteces[0]; } set { Verteces[0] = value; } }

        /// <summary>
        /// Вершина 2, в яку входить ребро
        /// </summary>
        public Vertex VertexIn { get { return Verteces[1]; } set { Verteces[1] = value; } }



        public Edge()
        {
            Verteces = new List<Vertex>();
            VertexOut = new Vertex();
            VertexIn = new Vertex();
            Weight = 1;
        }
        public Edge(Vertex pVertexA, Vertex pVertexB)
        {
            Verteces = new List<Vertex>();
            Verteces.Add(new Vertex());
            Verteces.Add(new Vertex());
            VertexOut = pVertexA;
            VertexIn = pVertexB;
            Weight = 1;
        }
        public Edge(Vertex pVertexA, Vertex pVertexB, int weight)
        {
            Verteces = new List<Vertex>();
            VertexOut = pVertexA;
            VertexIn = pVertexB;
            Weight = weight;
        }
    }
}