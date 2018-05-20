using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebGrease.Css.Extensions;

namespace CW_Nelya_Vika.Models.DB
{
    public static class GraphProblemDb
    {
        public static Problem CurrentProblem { get; set; }
        public static List<Problem> Problems { get; private set; }
        public static List<Graph> Graphs { get; set; }
        private static readonly string ConnString = ConfigurationManager.ConnectionStrings["CW_Nelya_Vika"].ConnectionString;
        private static readonly SqlConnection sqlConn = new SqlConnection(ConnString);

        /// <summary>
        /// Ініціалізація при першому зверненні до бд
        /// </summary>
        static GraphProblemDb()
        {
            sqlConn.Open();
            Update();
        }

        public static void Update()
        {
            GetAllGraph();
            GetAllProblems();
        }

        /// <summary>
        /// Знайти граф по заданому ідентифікатору
        /// </summary>
        /// <param name="graphId"></param>
        /// <returns></returns>
        public static Graph GetGraph(int graphId)
        {
            Graph graph = Graphs.Select(g => g).FirstOrDefault(g => g.Id == graphId);
            return graph;
        }

        /// <summary>
        /// знайти задачу/проблему по заданому ідентифікатору
        /// </summary>
        /// <param name="problemId"></param>
        /// <returns></returns>
        public static Problem GetProblem(int problemId)
        {
            Problem problem = Problems.FirstOrDefault(g => g.Id == problemId);
            return problem;
        }

        /// <summary>
        /// зчитати данні про всі графи з бд
        /// </summary>
        public static void GetAllGraph()
        {
            Graphs = new List<Graph>();
            if (sqlConn.State != ConnectionState.Open)
                sqlConn.Open();
            SqlCommand sqlCommand = new SqlCommand("select id, CommunityCount, MinCountInSubgraph, MaxCountInSubgraph from graph", sqlConn);

            using (SqlDataReader graphReader = sqlCommand.ExecuteReader())
            {
                while (graphReader.Read())
                {
                    Graph graph = new Graph
                    {
                        Id = (int)graphReader["id"],
                        CommunityCount = (int)graphReader["CommunityCount"],
                        MinCountInSubgraph = (int)graphReader["MinCountInSubgraph"],
                        MaxCountInSubgraph = (int)graphReader["MaxCountInSubgraph"]
                    };
                    Graphs.Add(graph);
                }
            }
            foreach (var graph in Graphs)
            {
                sqlCommand = new SqlCommand("select VertexOut, VertexIn, weight from edge where GraphId = @graphId", sqlConn);
                sqlCommand.Parameters.AddWithValue("@graphId", graph.Id);
                using (SqlDataReader edgeReader = sqlCommand.ExecuteReader())
                {
                    while (edgeReader.Read())
                    {
                        int idNodeOut = edgeReader["VertexOut"] == DBNull.Value ? 0 : (int)edgeReader["VertexOut"],
                            idNodeIn = edgeReader["VertexIn"] == DBNull.Value ? 0 : (int)edgeReader["VertexIn"],
                            weight = edgeReader["weight"] == DBNull.Value ? 0 : (int)edgeReader["weight"];
                        Vertex vertexA = null, vertexB = null;
                        if (idNodeOut != 0)
                            vertexA = graph.FindNode(idNodeOut, true);
                        if (idNodeIn != 0)
                            vertexB = graph.FindNode(idNodeIn, true);
                        if (idNodeOut != 0 && idNodeIn != 0)
                            graph.CreateLink(vertexA, vertexB, weight);
                    }
                }
            }
        }

        /// <summary>
        /// зчитати дані про всі графи з бд
        /// </summary>
        public static void GetAllProblems()
        {
            Problems = new List<Problem>();
            if (sqlConn.State != ConnectionState.Open)
                sqlConn.Open();
            SqlCommand sqlCommand = new SqlCommand("select id, InnitialGraphId, algorithm, G, ExecutionTime from problem", sqlConn);
            GetAllGraph();

            using (SqlDataReader problemReader = sqlCommand.ExecuteReader())
            {
                while (problemReader.Read())
                {
                    Problem problem = new Problem();
                    problem.Id = (int)problemReader["id"];
                    int graphId = (int)problemReader["InnitialGraphId"];
                    problem.Algorithm = (Algorithm)(int)problemReader["algorithm"];
                    problem.G = problemReader["G"] == DBNull.Value ? null : (int?)problemReader["G"];
                    problem.ExecutionTime = problemReader["ExecutionTime"] == DBNull.Value ? null : (int?)problemReader["ExecutionTime"];
                    problem.Graph = Graphs.Select(g => g).First(g => g.Id == graphId);
                    Problems.Add(problem);
                }
            }

            foreach (var problem in Problems)
            {
                sqlCommand = new SqlCommand("select id, subGraphId from ResultList where problemId = @problemId", sqlConn);
                sqlCommand.Parameters.AddWithValue("@problemId", problem.Id);
                using (SqlDataReader resultListReader = sqlCommand.ExecuteReader())
                {
                    while (resultListReader.Read())
                    {
                        int resultGraphId = (int)resultListReader["subGraphId"];
                        Graph resulGraph = Graphs.Select(g => g).First(g => g.Id == resultGraphId);
                        problem.GraphList.Add(resulGraph);
                    }
                }
            }

        }

        public static void AddGraph(Graph g, Graph parent = null)
        {
            if (sqlConn.State != ConnectionState.Open)
                sqlConn.Open();

            if (g.Id != null)
                return;

            var sqlCommand = new SqlCommand(
                "insert into Graph (CommunityCount, MinCountInSubgraph, MaxCountInSubgraph)" +
                "values (@CommunityCount, @MinCountInSubgraph, @MaxCountInSubgraph);" +
                "SELECT SCOPE_IDENTITY()", sqlConn);
            sqlCommand.Parameters.AddWithValue("@CommunityCount", g.CommunityCount);
            sqlCommand.Parameters.AddWithValue("@MinCountInSubgraph", g.MinCountInSubgraph);
            sqlCommand.Parameters.AddWithValue("@MaxCountInSubgraph", g.MaxCountInSubgraph);
            var insertedGraphId = sqlCommand.ExecuteScalar();
            g.Id = Convert.ToInt32(insertedGraphId);

            if (parent == null)
            {
                foreach (var edge in g.Edges)
                {
                    if (edge == null)
                        continue;
                    sqlCommand = new SqlCommand("insert into Edge(GraphId, VertexOut, VertexIn, Weight)" +
                                                "values (@GraphId, @VertexOut, @VertexIn, @Weight);" +
                                                "SELECT SCOPE_IDENTITY()", sqlConn);

                    sqlCommand.Parameters.AddWithValue("@GraphId", g.Id);
                    sqlCommand.Parameters.AddWithValue("@VertexOut", edge.VertexOut.Label);
                    sqlCommand.Parameters.AddWithValue("@VertexIn", edge.VertexIn.Label);
                    sqlCommand.Parameters.AddWithValue("@Weight", edge.Weight);
                    var insertedId = sqlCommand.ExecuteScalar();
                    edge.Id = Convert.ToInt32(insertedId);
                }
            }
            else
            {
                foreach (var v in g.Vertices)
                {
                    sqlCommand = new SqlCommand("insert into Edge(GraphId, VertexOut, VertexIn, Weight)" +
                                                "values (@GraphId, @VertexOut, @VertexIn, @Weight);" +
                                                "SELECT SCOPE_IDENTITY()", sqlConn);
                    sqlCommand.Parameters.AddWithValue("@GraphId", g.Id);
                    sqlCommand.Parameters.AddWithValue("@VertexOut", v.Label);
                    sqlCommand.Parameters.AddWithValue("@VertexIn", DBNull.Value);
                    sqlCommand.Parameters.AddWithValue("@Weight", DBNull.Value);
                }
            }
            GetAllGraph();
        }

        public static void AddProblem(Problem p)
        {
            if (p.Id != null)
                return;
            if (p.Graph.Id == null)
                AddGraph(p.Graph);
            if (sqlConn.State != ConnectionState.Open)
                sqlConn.Open();
            var sqlCommand = new SqlCommand("insert into Problem (InnitialGraphId, Algorithm, G, ExecutionTime)" +
                                                   "values (@InitialGraphId, @Algorithm, @G, @ExecutionTime);" +
                                                   "SELECT SCOPE_IDENTITY()", sqlConn);
            sqlCommand.Parameters.AddWithValue("@InitialGraphId", p.Graph.Id);
            sqlCommand.Parameters.AddWithValue("@Algorithm", (int)p.Algorithm);
            sqlCommand.Parameters.AddWithValue("@G", (object)p.G ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@ExecutionTime", (object)p.ExecutionTime ?? DBNull.Value);
            var insertedId = sqlCommand.ExecuteScalar();
            p.Id = Convert.ToInt32(insertedId);

            foreach (var g in p.GraphList)
            {
                AddGraph(g, p.Graph);
                if (sqlConn.State == ConnectionState.Closed)
                    sqlConn.Open();
                sqlCommand = new SqlCommand("insert into ResultList (ProblemId, SubGraphId)" +
                                            "values (@ProblemId, @GraphId);" +
                                            "SELECT SCOPE_IDENTITY()", sqlConn);
                sqlCommand.Parameters.AddWithValue("@ProblemId", p.Id);
                sqlCommand.Parameters.AddWithValue("@GraphId", g.Id);
                sqlCommand.ExecuteNonQuery();
            }
            GetAllProblems();
        }
    }

}