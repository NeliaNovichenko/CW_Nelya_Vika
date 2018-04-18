using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CW_Nelya_Vika.Models
{
    /// <summary>
    /// Перелік реалізованих алгоритмів
    /// </summary>
    public enum Algorithm : byte
    {
        GirvanNewman,
        KernighanLin
    }

    /// <summary>
    /// Клас, що описує модель поточної проблеми/моделі
    /// Містить граф та його поділи різними алгоритмами
    /// </summary>
    public class Problem
    {
        [Key]
        public int Id { get; set; }

        public Graph Graph { get; set; }

        public Result Result { get; set; }

        public Algorithm Algorithm { get; set; }

    }
}