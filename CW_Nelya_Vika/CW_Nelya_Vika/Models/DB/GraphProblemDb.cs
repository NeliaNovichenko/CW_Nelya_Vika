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
        public static List<Graph> Graphs { get; private set; }
        public static List<Problem> Problems { get; private set; }


        //private const string ConnString =
        //    @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CW_Nelya_Vika;Integrated Security=False;Trusted_Connection=True;Pooling=False;User ID=VikaNelya;Password=123456";

        private static string ConnString = ConfigurationManager.ConnectionStrings["CW_Nelya_Vika"].ConnectionString;

        private static SqlConnection sqlConn = new SqlConnection(ConnString);

        /// <summary>
        /// Ініціалізація при першому зверненні до бд
        /// </summary>
        static GraphProblemDb()
        {
            sqlConn.Open();
            Update();
        }

        private static void Update()
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
        private static void GetAllGraph()
        {
            Graphs = new List<Graph>();
            //try
            //{
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
                        int idNodeOut = (int)edgeReader["VertexOut"],
                            idNodeIn = (int)edgeReader["VertexIn"],
                            weight = (int)edgeReader["weight"];

                        Vertex vertexA = graph.FindNode(idNodeOut, true);
                        Vertex vertexB = graph.FindNode(idNodeIn, true);

                        graph.CreateLink(vertexA, vertexB);
                    }
                }
            }
            //}
            //catch (Exception e)
            //{
            //    //TODO:
            //}
        }

        /// <summary>
        /// зчитати дані про всі графи з бд
        /// </summary>
        private static void GetAllProblems()
        {
            Problems = new List<Problem>();
            //try
            //{
            if (sqlConn.State != ConnectionState.Open)
                sqlConn.Open();
            SqlCommand sqlCommand = new SqlCommand("select id, InnitialGraphId, algorithm from problem", sqlConn);

            using (SqlDataReader problemReader = sqlCommand.ExecuteReader())
            {
                while (problemReader.Read())
                {
                    Problem problem = new Problem();
                    problem.Id = (int)problemReader["id"];
                    int graphId = (int)problemReader["InnitialGraphId"];
                    Algorithm algorithm = (Algorithm)(int)problemReader["algorithm"];
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

            //}
            //catch (Exception e)
            //{
            //    //TODO:
            //}
            //finally
            //{
            //    sqlConn.Close();
            //}
        }

        public static bool AddGraph(Graph g, Graph parent = null)
        {
            bool result = true;
            SqlCommand sqlCommand;
            //try
            //{
            if (sqlConn.State != ConnectionState.Open)
                sqlConn.Open();

            if (g.Id != null)
                return false;

            sqlCommand = new SqlCommand(
                "insert into Graph (CommunityCount, MinCountInSubgraph, MaxCountInSubgraph)" +
                "values (@CommunityCount, @MinCountInSubgraph, @MaxCountInSubgraph);" +
                "SELECT SCOPE_IDENTITY()", sqlConn);
            sqlCommand.Parameters.AddWithValue("@CommunityCount", g.CommunityCount);
            sqlCommand.Parameters.AddWithValue("@MinCountInSubgraph", g.MinCountInSubgraph);
            sqlCommand.Parameters.AddWithValue("@MaxCountInSubgraph", g.MaxCountInSubgraph);
            var insertedGraphId = sqlCommand.ExecuteScalar();
            g.Id = Convert.ToInt32(insertedGraphId);

            if (parent != null)
            {
                Update();
                return result;
            }

            //foreach (var v1 in g.Vertices)
            //{
            //    foreach (var v2 in g.Vertices)
            //    {
            //        Edge edge = parent.FindEdge(v1, v2);
            //        if (edge == null)
            //            continue;
            //        sqlCommand = new SqlCommand("insert into Edge(GraphId, VertexOut, VertexIn, Weight)" +
            //                                    "values (@GraphId, @VertexOut, @VertexIn, @Weight);" +
            //                                    "SELECT SCOPE_IDENTITY()", sqlConn);

            //        sqlCommand.Parameters.AddWithValue("@GraphId", g.Id);
            //        sqlCommand.Parameters.AddWithValue("@VertexOut", edge.VertexOut.Label);
            //        sqlCommand.Parameters.AddWithValue("@VertexIn", edge.VertexIn.Label);
            //        sqlCommand.Parameters.AddWithValue("@Weight", edge.Weight);
            //        var insertedId = sqlCommand.ExecuteScalar();
            //        edge.Id = Convert.ToInt32(insertedId);
            //    }
            //}

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

            Update();
            //}
            //catch (Exception e)
            //{
            //    //TODO:
            //    result = false;
            //}
            return result;
        }

        public static bool AddProblem(Problem p)
        {
            bool result = true;
            SqlCommand sqlCommand;
            //try
            //{

            if (p.Id != null)
                return false;
            if (p.Graph.Id == null)
                AddGraph(p.Graph);
            if (sqlConn.State != ConnectionState.Open)
                sqlConn.Open();
            sqlCommand = new SqlCommand("insert into Problem (InnitialGraphId, Algorithm)" +
                                                   "values (@InitialGraphId, @Algorithm);" +
                                                   "SELECT SCOPE_IDENTITY()", sqlConn);
            sqlCommand.Parameters.AddWithValue("@InitialGraphId", p.Graph.Id);
            sqlCommand.Parameters.AddWithValue("@Algorithm", (int)p.Algorithm);
            var insertedId = sqlCommand.ExecuteScalar();
            p.Id = Convert.ToInt32(insertedId);


            foreach (var g in p.GraphList)
            {
                if (g.Id == null)
                    AddGraph(g, p.Graph);
                if (sqlConn.State == ConnectionState.Closed)
                    sqlConn.Open();
                sqlCommand = new SqlCommand("insert into ResultList (ProblemId, SubGraphId)" +
                                            "values (@ProblemId, @GraphId);" +
                                            "SELECT SCOPE_IDENTITY()", sqlConn);
                sqlCommand.Parameters.AddWithValue("@ProblemId", p.Id);
                sqlCommand.Parameters.AddWithValue("@GraphId", g.Id);

                int i = sqlCommand.ExecuteNonQuery();
            }
            Update();
            //}
            //catch (Exception e)
            //{
            //    //TODO:
            //    result = false;
            //}
            return result;
        }




        //todo:
        public static bool UpdateGraph(Graph g)
        {
            bool result = true;
            //try
            //{
            if (sqlConn.State != ConnectionState.Open)
                sqlConn.Open();
            SqlCommand sqlCommand = new SqlCommand("update Graph " +
                                                   "set CommunityCount = @CommunityCount, " +
                                                   "MinCountInSubgraph = @MinCountInSubgraph, " +
                                                   "MaxCountInSubgraph = @MaxCountInSubgraph;" +
                                                   "SELECT SCOPE_IDENTITY()", sqlConn);
            sqlCommand.Parameters.AddWithValue("@CommunityCount", g.CommunityCount);
            sqlCommand.Parameters.AddWithValue("@MinCountInSubgraph", g.MinCountInSubgraph);
            sqlCommand.Parameters.AddWithValue("@MaxCountInSubgraph", g.MaxCountInSubgraph);
            int UpdatedId = (int)sqlCommand.ExecuteScalar();


            foreach (var edge in g.Edges)
            {
                sqlCommand = new SqlCommand("update Edge set GraphId = @, " +
                                            "VertexOut = @VertexOut, " +
                                            "VertexIn = @VertexIn, " +
                                            "Weight = @Weight", sqlConn);

                sqlCommand.Parameters.AddWithValue("@GraphId", g.Id);
                sqlCommand.Parameters.AddWithValue("@VertexOut", edge.VertexOut);
                sqlCommand.Parameters.AddWithValue("@VertexIn", edge.VertexIn);
                sqlCommand.Parameters.AddWithValue("@Weight", edge.Weight);
                sqlCommand.ExecuteNonQuery();
            }
            Update();
            //}
            //catch (Exception e)
            //{
            //    //TODO:
            //    result = false;
            //}
            return result;
        }

        //todo:
        public static bool UpdateProblem(Problem p)
        {
            bool result = true;
            //try
            //{
            if (sqlConn.State != ConnectionState.Open)
                sqlConn.Open();
            SqlCommand sqlCommand = new SqlCommand("update Problem " +
                                                   "set InitialGraphId = @InitialGraphId, Algorithm = @Algorithm " +
                                                   "where id = @problemId");
            sqlCommand.Parameters.AddWithValue("@problemId", p.Id);
            sqlCommand.Parameters.AddWithValue("@InitialGraphId", p.Graph.Id);
            sqlCommand.Parameters.AddWithValue("@Algorithm", (int)p.Algorithm);
            sqlCommand.ExecuteNonQuery();


            foreach (var g in p.GraphList)
            {
                AddGraph(g);
                sqlCommand = new SqlCommand("update ResultList set " +
                                            "ProblemId = @ProblemId, " +
                                            "GraphId = @GraphId " +
                                            "where id = @id", sqlConn);
                sqlCommand.Parameters.AddWithValue("@id", p.Id);
                sqlCommand.Parameters.AddWithValue("@ProblemId", p.Id);
                sqlCommand.Parameters.AddWithValue("@GraphId", g.Id);
                sqlCommand.ExecuteNonQuery();
            }
            Update();
            //}
            //catch (Exception e)
            //{
            //    //TODO:
            //    result = false;
            //}
            return result;
        }
        public static void CloseConnection()
        {
            sqlConn.Close();
        }
    }

}