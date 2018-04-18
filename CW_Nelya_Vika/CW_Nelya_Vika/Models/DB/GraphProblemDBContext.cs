using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CW_Nelya_Vika.Models.DB
{
    public class GraphProblemDBContext : DbContext
    {
        // Имя будущей базы данных можно указать через
        // вызов конструктора базового класса
        public GraphProblemDBContext() : base("GraphProblemDBContext")
        { }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Vertex>().HasMany(v => v.AdjacencyEdges).WithMany(e=>e.Verteces);
        //    base.OnModelCreating(modelBuilder);
        //}

        // Отражение таблиц базы данных на свойства с типом DbSet
        public DbSet<Edge> Edges { get; set; }
        public DbSet<Vertex> Vertices { get; set; }
        public DbSet<Graph> Graphs { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Problem> Problems { get; set; }
    }
}