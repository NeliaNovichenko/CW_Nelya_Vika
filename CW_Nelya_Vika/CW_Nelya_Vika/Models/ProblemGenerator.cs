using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CW_Nelya_Vika.Models
{
    public enum ProblemClassification: byte
    {
        Xs, S, M, L, Xl
    }

    public enum Algorithm : byte
    {
        GirvanNewman,
        KernighanLin
    }
    public class ProblemGenerator
    {
        [Key]
        public int Id { get; set; }
        public ProblemClassification ProblemClassification { get; set; }
        public Algorithm Algorithm { get; set; }
    }
}