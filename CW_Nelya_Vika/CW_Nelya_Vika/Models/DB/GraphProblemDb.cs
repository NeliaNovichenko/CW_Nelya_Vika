using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CW_Nelya_Vika.Models.DB
{
    public static class GraphProblemDb
    {
        public static List<Graph> Graphs { get; private set; }
        public static List<Problem> Problems { get; private set; }

        //string attachDbPath = Path.GetFullPath(@"~\CW_Nelya_Vika\CW_Nelya_Vika\App_Data\CW_Nelya_Vika.mdf");
        //string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='" + attachDbPath + "';Integrated Security=True";
        private const string connString =
                @"Data Source=(LocalDB)\MSSQLLocalDB;
                    AttachDbFilename='D:\Studing\ТРПЗ\Курсова робота\CW_Nelya_Vika\CW_Nelya_Vika\CW_Nelya_Vika\App_Data\CW_Nelya_Vika.mdf';
                    Integrated Security=True";

        private static SqlConnection sqlConn = new SqlConnection(connString);

        /// <summary>
        /// Ініціалізація при першому зверненні до бд
        /// </summary>
        static GraphProblemDb()
        {
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
            Graph graph = Graphs.Select(g => g).First(g => g.Id == graphId);

            /*Graph graph = new Graph();
            SqlCommand sqlCommand = new SqlCommand("select VertexOut, VertexIn, weight from edge where GraphId = @graphId", sqlConn);
            sqlCommand.Parameters.AddWithValue("@graphId", graphId);
            try
            {
                sqlConn.Open();
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idNodeOut = (int)reader["VertexOut"],
                            idNodeIn = (int)reader["VertexIn"],
                            weight = (int)reader["weight"];
                        Vertex vertexA = graph.FindNode(idNodeOut, true);
                        Vertex vertexB = graph.FindNode(idNodeIn, true);
                        graph.CreateLink(vertexA, vertexB);
                    }
                }
                sqlCommand = new SqlCommand(
                    "select CommunityCount, MinCountInSubgraph, MaxCountInSubgraph from graph where ID = @graphId", sqlConn);
                sqlCommand.Parameters.AddWithValue("@graphId", graphId);
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int CommunityCount = (int)reader["CommunityCount"],
                            MinCountInSubgraph = (int)reader["MinCountInSubgraph"],
                            MaxCountInSubgraph = (int)reader["MaxCountInSubgraph"];
                        graph.CommunityCount = CommunityCount;
                        graph.MinCountInSubgraph = MinCountInSubgraph;
                        graph.MaxCountInSubgraph = MaxCountInSubgraph;
                    }
                }
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                sqlConn.Close();
            }*/
            return graph;
        }

        /// <summary>
        /// знайти задачу/проблему по заданому ідентифікатору
        /// </summary>
        /// <param name="problemId"></param>
        /// <returns></returns>
        public static Problem GetProblem(int problemId)
        {
            Problem problem = Problems.First(g => g.Id == problemId);
            return problem;
        }

        /// <summary>
        /// зчитати данні про всі графи з бд
        /// </summary>
        private static void GetAllGraph()
        {
            Graphs = new List<Graph>();
            try
            {
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
            }
            catch (Exception e)
            {
                //TODO:
            }
            finally
            {
                sqlConn.Close();
            }
        }

        /// <summary>
        /// зчитати дані про всі графи з бд
        /// </summary>
        private static void GetAllProblems()
        {
            Problems = new List<Problem>();
            try
            {
                sqlConn.Open();
                SqlCommand sqlCommand = new SqlCommand("select id, InnitialGraphId from problem", sqlConn);

                using (SqlDataReader problemReader = sqlCommand.ExecuteReader())
                {
                    while (problemReader.Read())
                    {
                        Problem problem = new Problem();
                        problem.Id = (int)problemReader["id"];
                        int graphId = (int)problemReader["InnitialGraphId"];
                        problem.Graph = Graphs.Select(g => g).First(g => g.Id == graphId);
                        Problems.Add(problem);
                    }
                }
                foreach (var problem in Problems)
                {
                    sqlCommand = new SqlCommand("select id, graphId, algorithm from ResultList where problemId = @problemId", sqlConn);
                    sqlCommand.Parameters.AddWithValue("@problemId", problem.Id);
                    using (SqlDataReader resultListReader = sqlCommand.ExecuteReader())
                    {
                        while (resultListReader.Read())
                        {
                            int resultGraphId = (int)resultListReader["graphId"];
                            Graph resulGraph = Graphs.Select(g => g).First(g => g.Id == resultGraphId);
                            problem.GraphList.Add(resulGraph);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                //TODO:
            }
            finally
            {
                sqlConn.Close();
            }
        }

        public static bool AddGraph(Graph g)
        {
            bool result = true;
            try
            {
                sqlConn.Open();
                SqlCommand sqlCommand = new SqlCommand("insert into Graph (CommunityCount, MinCountInSubgraph, MaxCountInSubgraph)" +
                                                       "values (@CommunityCount, @MinCountInSubgraph, @MaxCountInSubgraph);" +
                                                       "SELECT SCOPE_IDENTITY()", sqlConn);
                sqlCommand.Parameters.AddWithValue("@CommunityCount", g.CommunityCount);
                sqlCommand.Parameters.AddWithValue("@MinCountInSubgraph", g.MinCountInSubgraph);
                sqlCommand.Parameters.AddWithValue("@MaxCountInSubgraph", g.MaxCountInSubgraph);
                int insertedId = (int)sqlCommand.ExecuteScalar();
                g.Id = insertedId;


                foreach (var edge in g.Edges)
                {
                    sqlCommand = new SqlCommand("insert into Edge (GraphId, VertexOut, VertexIn, Weight)" +
                                                "values (@GraphId, @VertexOut, @VertexIn, @Weight);" +
                                                "SELECT SCOPE_IDENTITY()", sqlConn);

                    sqlCommand.Parameters.AddWithValue("@GraphId", g.Id);
                    sqlCommand.Parameters.AddWithValue("@VertexOut", edge.VertexOut);
                    sqlCommand.Parameters.AddWithValue("@VertexIn", edge.VertexIn);
                    sqlCommand.Parameters.AddWithValue("@Weight", edge.Weight);
                    sqlCommand.ExecuteNonQuery();
                }
                Update();
            }
            catch (Exception e)
            {
                //TODO:
                result = false;
            }
            finally
            {
                sqlConn.Close();
            }
            return result;
        }

        public static bool AddProblem(Problem p)
        {
            bool result = true;
            try
            {
                sqlConn.Open();
                SqlCommand sqlCommand = new SqlCommand("insert into Problem (InitialGraphId, Algorithm)" +
                                                       "values (@InitialGraphId, @Algorithm);" +
                                                       "SELECT SCOPE_IDENTITY()", sqlConn);
                sqlCommand.Parameters.AddWithValue("@InitialGraphId",p.Graph.Id);
                sqlCommand.Parameters.AddWithValue("@Algorithm", (int)p.Algorithm);
                int insertedId = (int)sqlCommand.ExecuteScalar();
                p.Id = insertedId;


                foreach (var g in p.GraphList)
                {
                    AddGraph(g);
                    sqlCommand = new SqlCommand("insert into ResultList (ProblemId, GraphId)" +
                                                "values (@ProblemId, @GraphId)", sqlConn);
                    sqlCommand.Parameters.AddWithValue("@ProblemId", p.Id);
                    sqlCommand.Parameters.AddWithValue("@GraphId", g.Id);
                    sqlCommand.ExecuteNonQuery();
                }
                Update();
            }
            catch (Exception e)
            {
                //TODO:
                result = false;
            }
            finally
            {
                sqlConn.Close();
            }
            return result;
        }




        //todo:
        public static bool UpdateGraph(Graph g)
        {
            bool result = true;
            try
            {
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
            }
            catch (Exception e)
            {
                //TODO:
                result = false;
            }
            finally
            {
                sqlConn.Close();
            }
            return result;
        }

        //todo:
        public static bool UpdateProblem(Problem p)
        {
            bool result = true;
            try
            {
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
            }
            catch (Exception e)
            {
                //TODO:
                result = false;
            }
            finally
            {
                sqlConn.Close();
            }
            return result;
        }

    }
}