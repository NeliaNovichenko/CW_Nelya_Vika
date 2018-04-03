using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CW_Nelya_Vika.Models
{
    /// <summary>
    /// Задає розмір графу:
    /// - Xs - 0...
    /// - S - ...
    /// - M - ...
    /// - L - ...
    /// - Xl - ...
    /// </summary>
    public enum ProblemClassification: byte
    {
        Xs, S, M, L, Xl
    }

    /// <summary>
    /// Клас, що генерує випадкові ребра та вершини,
    /// враховуючи розмірність
    /// </summary>
    public class GraphGenerator: IGraphInitializer
    {
        /// <summary>
        /// Розмірність графу
        /// </summary>
        public ProblemClassification ProblemClassification { get; set; }
        public Graph Graph { get; private set; }

        public GraphGenerator(Graph g)
        {
            if(g is null)
                g = new Graph();
            if (g.Edges is null)
                g.Edges = new List<Edge>();
            if (g.Nodes is null)
                g.Nodes = new List<Node>();
                g.Nodes = new List<Node>();
            Graph = g;
        }
        /// <summary>
        /// Генерація графу заданої размірності
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="edges"></param>
        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}