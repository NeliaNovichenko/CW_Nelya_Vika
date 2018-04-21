using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CW_Nelya_Vika.Models.DB
{
    public class GraphProblemDBContext : DbContext
    {

        public DbSet<Edge> Edges { get; set; }
        public DbSet<Vertex> Verteces { get; set; }
        public DbSet<Graph> Graphs { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Problem> Problems { get; set; }
    }
}