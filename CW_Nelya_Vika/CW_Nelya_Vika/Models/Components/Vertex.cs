using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CW_Nelya_Vika.Models
{
    public class Vertex
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Вузли суміжності
        /// </summary>
        public List<Vertex> AdjacencyVertexes { get; set; }
        /// <summary>
        /// Суміжні ребра
        /// </summary>
        public List<Edge> AdjacencyEdges { get; set; }
        /// <summary>
        /// фіксована вершина чи ні
        /// </summary>
        public bool IsFixed { get; set; }

        public Vertex()
        {
            AdjacencyVertexes = new List<Vertex>();
            AdjacencyEdges = new List<Edge>();
            IsFixed = false;
        }

        public override bool Equals(object obj)
        {
            Vertex vertex = obj as Vertex;
            if (vertex is null)
                return false;
            if (this.Id == vertex.Id)
                return true;
            return false;
        }
    }
}