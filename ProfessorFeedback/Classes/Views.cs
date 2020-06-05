using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ProfessorFeedback.Classes
{
    class Views
    {
        private string conn = @"Server=localhost\SQLExpress;Database=Prototype_1;Trusted_Connection=True;";

        public Dictionary<string, string> allViews = new Dictionary<string, string>
        {
            { "prof", "VProfesores" },
            { "comm", "VComentarios" },
            { "cal", "VCalificaciones" },
            { "prom", "VPromedios" }
        };

        public void ViewProfessors()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    Console.WriteLine("\nYou selected to view all professors. Executing...\n");

                    string sql = "SELECT * FROM " + allViews["prof"];

                    var table = new ConsoleTable("ID", "Full name");

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                table.AddRow(reader.GetString(0), reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3));
                            }
                        }
                    }
                    table.Write();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void ViewComments()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    Console.WriteLine("\nYou selected to view all comments. Executing...\n");

                    string sql = "SELECT * FROM " + allViews["comm"];

                    var table = new ConsoleTable("ID", "Name", "Comment", "Date");

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                table.AddRow(reader.GetString(0), reader.GetString(1) + " " + reader.GetString(2), reader.GetString(3), reader.GetDateTime(4).ToString());
                            }
                        }
                    }
                    table.Write();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void ViewGrades()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    Console.WriteLine("\nYou selected to view all grades. Executing...\n");

                    string sql = "SELECT * FROM " + allViews["cal"];

                    var table = new ConsoleTable("ID", "Name", "C1", "C2", "C3", "C4", "C5", "C6");

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                table.AddRow(reader.GetString(0), reader.GetString(1) + " " + reader.GetString(2), reader.GetDouble(3), reader.GetDouble(4), reader.GetDouble(5), reader.GetDouble(6), reader.GetDouble(7), reader.GetDouble(8));
                            }
                        }
                    }
                    table.Write();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void ViewAverages()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    Console.WriteLine("\nYou selected to view all averages. Executing...\n");

                    string sql = "SELECT * FROM " + allViews["prom"];

                    var table = new ConsoleTable("ID", "Name", "Average");

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                table.AddRow(reader.GetString(0), reader.GetString(1) + " " + reader.GetString(2), reader.GetDouble(3));
                            }
                        }
                    }
                    table.Write();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
