namespace CW_Nelya_Vika.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Edges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Weight = c.Int(nullable: false),
                        Vertex_Id = c.Int(),
                        VertexIn_Id = c.Int(),
                        VertexOut_Id = c.Int(),
                        Graph_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vertices", t => t.Vertex_Id)
                .ForeignKey("dbo.Vertices", t => t.VertexIn_Id)
                .ForeignKey("dbo.Vertices", t => t.VertexOut_Id)
                .ForeignKey("dbo.Graphs", t => t.Graph_Id)
                .Index(t => t.Vertex_Id)
                .Index(t => t.VertexIn_Id)
                .Index(t => t.VertexOut_Id)
                .Index(t => t.Graph_Id);
            
            CreateTable(
                "dbo.Vertices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsFixed = c.Boolean(nullable: false),
                        Vertex_Id = c.Int(),
                        Graph_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vertices", t => t.Vertex_Id)
                .ForeignKey("dbo.Graphs", t => t.Graph_Id)
                .Index(t => t.Vertex_Id)
                .Index(t => t.Graph_Id);
            
            CreateTable(
                "dbo.Graphs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommunityCount = c.Int(nullable: false),
                        MinCountInSubgraph = c.Int(nullable: false),
                        MaxCountInSubgraph = c.Int(nullable: false),
                        Problem_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Problems", t => t.Problem_Id)
                .Index(t => t.Problem_Id);
            
            CreateTable(
                "dbo.Problems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Algorithm = c.Byte(nullable: false),
                        Graph_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Graphs", t => t.Graph_Id)
                .Index(t => t.Graph_Id);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Capacity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Graphs", "Problem_Id", "dbo.Problems");
            DropForeignKey("dbo.Problems", "Graph_Id", "dbo.Graphs");
            DropForeignKey("dbo.Vertices", "Graph_Id", "dbo.Graphs");
            DropForeignKey("dbo.Edges", "Graph_Id", "dbo.Graphs");
            DropForeignKey("dbo.Edges", "VertexOut_Id", "dbo.Vertices");
            DropForeignKey("dbo.Edges", "VertexIn_Id", "dbo.Vertices");
            DropForeignKey("dbo.Vertices", "Vertex_Id", "dbo.Vertices");
            DropForeignKey("dbo.Edges", "Vertex_Id", "dbo.Vertices");
            DropIndex("dbo.Problems", new[] { "Graph_Id" });
            DropIndex("dbo.Graphs", new[] { "Problem_Id" });
            DropIndex("dbo.Vertices", new[] { "Graph_Id" });
            DropIndex("dbo.Vertices", new[] { "Vertex_Id" });
            DropIndex("dbo.Edges", new[] { "Graph_Id" });
            DropIndex("dbo.Edges", new[] { "VertexOut_Id" });
            DropIndex("dbo.Edges", new[] { "VertexIn_Id" });
            DropIndex("dbo.Edges", new[] { "Vertex_Id" });
            DropTable("dbo.Results");
            DropTable("dbo.Problems");
            DropTable("dbo.Graphs");
            DropTable("dbo.Vertices");
            DropTable("dbo.Edges");
        }
    }
}
