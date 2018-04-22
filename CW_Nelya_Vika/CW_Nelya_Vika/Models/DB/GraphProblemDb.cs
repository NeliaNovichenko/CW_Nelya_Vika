using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CW_Nelya_Vika.Models.DB
{
    public class GraphProblemDb
    {
        public static List<Graph> graphs = new List<Graph>();
        public static List<Problem> problems = new List<Problem>();
        //string attachDbPath = Path.GetFullPath(@"~\CW_Nelya_Vika\CW_Nelya_Vika\App_Data\CW_Nelya_Vika.mdf");
        //string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='" + attachDbPath + "';Integrated Security=True";

        private const string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Uni\TRPZ\CourseWork\CW_Nelya_Vika\CW_Nelya_Vika\CW_Nelya_Vika\App_Data\CW_Nelya_Vika.mdf;Integrated Security=True";
        private const string connString2 = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='D:\Studing\ТРПЗ\Курсова робота\CW_Nelya_Vika\CW_Nelya_Vika\CW_Nelya_Vika\App_Data\CW_Nelya_Vika.mdf';Integrated Security=True";
        static SqlConnection sqlConn = new SqlConnection(connString);


        /// <summary>
        /// Ініціалізація при першому зверненні до бд
        /// </summary>
        static GraphProblemDb()
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
            Graph graph = graphs.Select(g => g).First(g => g.Id == graphId);

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
            Problem problem = problems.First(g => g.Id == problemId);
            return problem;
        }

        /// <summary>
        /// зчитати данні про всі графи з бд
        /// </summary>
        private static void GetAllGraph()
        {
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
                        graphs.Add(graph);
                    }
                }
                foreach (var graph in graphs)
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
                        problem.Graph = graphs.Select(g => g).First(g => g.Id == graphId);
                        problems.Add(problem);
                    }
                }
                foreach (var problem in problems)
                {
                    sqlCommand = new SqlCommand("select id, graphId, algorithm from ResultList where problemId = @problemId", sqlConn);
                    sqlCommand.Parameters.AddWithValue("@problemId", problem.Id);
                    using (SqlDataReader resultListReader = sqlCommand.ExecuteReader())
                    {
                        while (resultListReader.Read())
                        {
                            int resultGraphId = (int)resultListReader["graphId"];
                            Graph resulGraph = graphs.Select(g => g).First(g => g.Id == resultGraphId);
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

        //TODO:
        public static bool AddGraph(Graph g)
        {
            return false;
        }

        public static bool AddProblem(Problem p)
        {
            return false;
        }

        public static bool UpdateGraph()
        {
            return false;
        }

        public static bool UpdateProblem()
        {
            return false;
        }

    }
}