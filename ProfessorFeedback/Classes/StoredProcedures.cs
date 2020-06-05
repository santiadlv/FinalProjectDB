using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfessorFeedback.Classes
{
    class StoredProcedures
    {
        private string conn = @"Server=localhost\SQLExpress;Database=Prototype_1;Trusted_Connection=True;";
        string msgFromSP = "";
        string linesAffected = "(0 row(s) affected)";

        public void GetAverageBetweenIntervals()
        {
            Console.WriteLine("\nYou selected to get all averages between an interval. Executing...\n");
            string sp = "dbo.ReturnRange";

            float lowerLimit;
            float upperLimit;

            Console.Write("Enter lower limit of interval: ");
            lowerLimit = FormatData.TryFloatConvert(Console.ReadLine());
            Console.Write("Enter upper limit of interval: ");
            upperLimit = FormatData.TryFloatConvert(Console.ReadLine());
            Console.WriteLine();

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    var table = new ConsoleTable("ID", "Average");

                    using (SqlCommand command = new SqlCommand(sp, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@range1", lowerLimit);
                        command.Parameters.AddWithValue("@range2", upperLimit);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                table.AddRow(reader.GetString(0), reader.GetDouble(1));
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

        public void GetCommentsWithName()
        {
            Console.WriteLine("\nYou selected to get all comments with the specified name. Executing...\n");
            string sp = "dbo.ReturnName";

            string fname;
            string lname;

            Console.Write("Enter first name: ");
            fname = Console.ReadLine();
            Console.Write("Enter last name: ");
            lname = Console.ReadLine();
            Console.WriteLine();

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    var table = new ConsoleTable("ID", "Full name", "Comment", "Date");

                    using (SqlCommand command = new SqlCommand(sp, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Fname", fname);
                        command.Parameters.AddWithValue("@Lname", lname);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                table.AddRow(reader.GetString(0), reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3), reader.GetString(4), reader.GetDateTime(5).ToString());
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

        public void GetCommentsWithNameAndDate()
        {
            Console.WriteLine("\nYou selected to get all comments with the specified name and date. Executing...\n");
            string sp = "dbo.ReturnNameDate";

            string fname;
            string lname;
            DateTime date;

            Console.Write("Enter first name: ");
            fname = Console.ReadLine();
            Console.Write("Enter last name: ");
            lname = Console.ReadLine();
            Console.Write("Enter search date: ");
            date = FormatData.TryDateTimeConvert(Console.ReadLine());
            Console.WriteLine();

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    var table = new ConsoleTable("ID", "Full name", "Comment", "Date");

                    using (SqlCommand command = new SqlCommand(sp, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Fname", fname);
                        command.Parameters.AddWithValue("@Lname", lname);
                        command.Parameters.AddWithValue("@date", date);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                table.AddRow(reader.GetString(0), reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3), reader.GetString(4), reader.GetDateTime(5).ToString());
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

        public void InsertComment()
        {
            Console.WriteLine("\nYou selected to insert a commment. Information is case sensitive. Executing...\n");
            string sp = "dbo.InsertComment";

            string fname;
            string lname;
            string comment;
            DateTime date;

            Console.Write("Enter first name: ");
            fname = Console.ReadLine();
            Console.Write("Enter last name: ");
            lname = Console.ReadLine();
            Console.Write("Enter comment to insert: ");
            comment = Console.ReadLine();
            Console.Write("Enter comment date: ");
            date = FormatData.TryDateTimeConvert(Console.ReadLine());
            Console.WriteLine();

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    connection.InfoMessage += new SqlInfoMessageEventHandler(Connection_InfoMessage);

                    using (SqlCommand command = new SqlCommand(sp, connection))
                    {
                        command.StatementCompleted += Command_StatementCompleted;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Fname", fname);
                        command.Parameters.AddWithValue("@Lname", lname);
                        command.Parameters.AddWithValue("@comment", comment);
                        command.Parameters.AddWithValue("@date", date);

                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine(msgFromSP);
                    Console.WriteLine(linesAffected);
                    Console.WriteLine();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void GetLastCommentSinceDays()
        {
            Console.WriteLine("\nYou selected to get the professors without any comment since a certain amount of days. Executing...\n");
            string sp = "dbo.ReturnTodaySchedule";

            int days;

            Console.Write("Enter days since last comment: ");
            days = FormatData.TryIntConvert(Console.ReadLine());
            Console.WriteLine();

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    var table = new ConsoleTable("ID", "Days since last comment");

                    using (SqlCommand command = new SqlCommand(sp, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@days", days);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                table.AddRow(reader.GetString(0), reader.GetInt32(1));
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

        public void DeleteComment()
        {
            Console.WriteLine("\nYou selected to delete a commment. Executing...\n");
            string sp = "dbo.EraseComment";

            string ID;
            DateTime date;

            Console.Write("Enter professor ID (case sensitive): ");
            ID = Console.ReadLine();
            Console.Write("Enter comment date: ");
            date = FormatData.TryDateTimeConvert(Console.ReadLine());
            Console.WriteLine();

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    connection.InfoMessage += new SqlInfoMessageEventHandler(Connection_InfoMessage);

                    using (SqlCommand command = new SqlCommand(sp, connection))
                    {
                        command.StatementCompleted += Command_StatementCompleted;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", ID);
                        command.Parameters.AddWithValue("@date", date);

                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine(msgFromSP);
                    Console.WriteLine(linesAffected);
                    Console.WriteLine();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void UpdateValue()
        {
            Console.WriteLine("\nYou selected to update a professor's grade. Executing...\n");
            string sp = "dbo.UpdateValue";

            int column;
            string ID;
            float newGrade;

            Console.Write("Enter column number: ");
            column = FormatData.TryIntConvert(Console.ReadLine());
            Console.Write("Enter professor ID (case sensitive): ");
            ID = Console.ReadLine();
            Console.Write("Enter new grade: ");
            newGrade = FormatData.TryFloatConvert(Console.ReadLine());
            Console.WriteLine();

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    connection.InfoMessage += new SqlInfoMessageEventHandler(Connection_InfoMessage);

                    using (SqlCommand command = new SqlCommand(sp, connection))
                    {
                        command.StatementCompleted += Command_StatementCompleted;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@column", column);
                        command.Parameters.AddWithValue("@ID", ID);
                        command.Parameters.AddWithValue("@newScore", newGrade);

                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine(msgFromSP);
                    Console.WriteLine(linesAffected);
                    Console.WriteLine();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void GetCommentsBetweenDates()
        {
            Console.WriteLine("\nYou selected to get all comments between two dates. Executing...\n");
            string sp = "dbo.Report";

            DateTime date1;
            DateTime date2;

            Console.Write("Enter lower limit of date interval: ");
            date1 = FormatData.TryDateTimeConvert(Console.ReadLine());
            Console.Write("Enter upper limit of date interval: ");
            date2 = FormatData.TryDateTimeConvert(Console.ReadLine());
            Console.WriteLine();

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    var table = new ConsoleTable("ID", "Full name", "Comment", "Date");

                    using (SqlCommand command = new SqlCommand(sp, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@range1", date1);
                        command.Parameters.AddWithValue("@range2", date2);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                table.AddRow(reader.GetString(0), reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3), reader.GetString(4), reader.GetDateTime(5).ToString());
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

        public void ResetMessages()
        {
            msgFromSP = "";
            linesAffected = "(0 row(s) affected)";
        }

        private void Command_StatementCompleted(object sender, StatementCompletedEventArgs e)
        {
            linesAffected = "(" + e.RecordCount + " row(s) affected)";
        }

        private void Connection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            msgFromSP = e.Message;
        }
    }
}
