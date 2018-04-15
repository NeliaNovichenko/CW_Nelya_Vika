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

        /// <summary>
        /// Constructor gets Problem Classification. 
        /// Default value is 'S'.
        /// </summary>
        /// <param name="pC"></param>
        public GraphGenerator(ProblemClassification pC = ProblemClassification.S)
        {
            ProblemClassification = pC;
        }
        /// <summary>
        /// Генерація графу заданої размірності
        /// </summary>
        public Graph Initialize()
        {
            Graph graph = new Graph();

            throw new NotImplementedException();
        }
    }
}