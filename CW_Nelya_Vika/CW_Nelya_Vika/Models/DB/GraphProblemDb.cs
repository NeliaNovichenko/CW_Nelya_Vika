using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CW_Nelya_Vika.Models.DB
{
    public class GraphProblemDb
    {
        //string attachDbPath = Path.GetFullPath(@"~\CW_Nelya_Vika\CW_Nelya_Vika\App_Data\CW_Nelya_Vika.mdf");
        //string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='" + attachDbPath + "';Integrated Security=True";
        private const string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Uni\TRPZ\CourseWork\CW_Nelya_Vika\CW_Nelya_Vika\CW_Nelya_Vika\App_Data\CW_Nelya_Vika.mdf;Integrated Security=True";
        private const string connString2 = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='D:\Studing\ТРПЗ\Курсова робота\CW_Nelya_Vika\CW_Nelya_Vika\CW_Nelya_Vika\App_Data\CW_Nelya_Vika.mdf';Integrated Security=True";
        private SqlConnection sqlConn = new SqlConnection(connString);

        public Graph GetGraph(int graphId)
        {
            Graph graph = new Graph();
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
               
            }
            finally
            {
                sqlConn.Close();
            }
            return graph;
        }
    }
}