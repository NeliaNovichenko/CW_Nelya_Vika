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
    /// Перелік реалізованих алгоритмів
    /// </summary>
    public enum Algorithm : int
    {
        KernighanLin = 0,
        GirvanNewman = 1,
        None = 2
    }

    /// <summary>
    /// Клас, що описує модель поточної проблеми/моделі
    /// Містить граф та його поділи різними алгоритмами
    /// </summary>
    public class Problem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }

        public Graph Graph { get; set; }

        public GraphList GraphList { get; set; }

        public Algorithm Algorithm { get; set; }

        public int? ExecutionTime { get; set; }

        public int? G { get; set; }

        public Problem()
        {
            Graph = new Graph();
            GraphList = new GraphList();
            Algorithm = Algorithm.None;
        }

    }
}