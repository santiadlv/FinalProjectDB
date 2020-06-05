using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfessorFeedback.Classes
{
    static class MasterControl
    {
        private static string choice;

        public static void StartProgram()
        {
            Console.Clear();
            PrintTitle();
            Console.WriteLine("\nWelcome! This program aims at being a friendlier interface for the Professor Feedback Database.");
            ChooseFunctionType();
        }

        private static void ChooseFunctionType()
        {
            Console.Write("Would you like to execute Views or Stored Procedures? Write 'V' for views or 'SP' for stored procedures: ");
            choice = Console.ReadLine();
            CheckChoice();
        }

        private static void CheckChoice()
        {
            string lowChoice = choice.ToLower();
            if (lowChoice == "v")
            {
                UseViews();
            }
            else if (lowChoice == "sp")
            {
                UseStoredProcedures();
            }
            else
            {
                Console.WriteLine("\nYour input didn't match either 'V' or 'SP'. Please try again.");
                ChooseFunctionType();
            }
        }

        private static void UseViews()
        {
            GC.Collect();
            Console.Clear();
            PrintTitle();
            Console.WriteLine("\nYou chose to execute a view. Which of the following do you want to execute?\n");
            RedirectToViews();
            ReturnAfterOperation(0);
        }

        private static void RedirectToViews()
        {
            Views view = new Views();
            int viewChoice = ChooseView();

            if (viewChoice > 4 || viewChoice < 1)
            {
                Console.WriteLine("\nYour input didn't match any of the view options, please try again.\n");
                RedirectToViews();
            }
            else
            {
                if (viewChoice == 1)
                {
                    view.ViewProfessors();
                }
                else if (viewChoice == 2)
                {
                    view.ViewComments();
                }
                else if (viewChoice == 3)
                {
                    view.ViewGrades();
                }
                else if (viewChoice == 4)
                {
                    view.ViewAverages();
                }
            }
        }

        private static int ChooseView()
        {
            int temp;
            Console.WriteLine("1. View all professors\n2. View all comments\n3. View all grades\n4. View all average grades\n");
            Console.Write("Enter the number of the view to execute: ");
            temp = FormatData.TryIntConvert(Console.ReadLine());
            return temp;
        }

        private static void UseStoredProcedures()
        {
            GC.Collect();
            Console.Clear();
            PrintTitle();
            Console.WriteLine("\nYou chose to execute a stored procedure. Which of the following do you want to execute?\n");
            RedirectToStoredProcedures();
            ReturnAfterOperation(1);
        }

        private static void RedirectToStoredProcedures()
        {
            StoredProcedures sp = new StoredProcedures();
            int spChoice = ChooseStoredProcedure();

            if (spChoice > 8 || spChoice < 1)
            {
                Console.WriteLine("\nYour input didn't match any of the stored procedure options, please try again.\n");
                RedirectToStoredProcedures();
            }
            else
            {
                if (spChoice == 1)
                {
                    sp.GetAverageBetweenIntervals();
                }
                else if (spChoice == 2)
                {
                    sp.GetCommentsWithName();
                }
                else if (spChoice == 3)
                {
                    sp.GetCommentsWithNameAndDate();
                }
                else if (spChoice == 4)
                {
                    sp.InsertComment();
                    sp.ResetMessages();
                }
                else if (spChoice == 5)
                {
                    sp.GetLastCommentSinceDays();
                }
                else if (spChoice == 6)
                {
                    sp.DeleteComment();
                    sp.ResetMessages();
                }
                else if (spChoice == 7)
                {
                    sp.UpdateValue();
                    sp.ResetMessages();
                }
                else if (spChoice == 8)
                {
                    sp.GetCommentsBetweenDates();
                }
            }
        }

        private static int ChooseStoredProcedure()
        {
            int temp;
            Console.WriteLine("1. Get average grades between intervals\n2. Get comments with a certain name");
            Console.WriteLine("3. Get comments with a certain name and date\n4. Insert comment into database");
            Console.WriteLine("5. Get professors without any comment since a specific number of days\n6. Delete a comment from database");
            Console.WriteLine("7. Update a professor's grade\n8. Get all comments made between two dates\n");
            Console.Write("Enter the number of the stored procedure to execute: ");
            temp = FormatData.TryIntConvert(Console.ReadLine());
            return temp;
        }

        private static void ReturnAfterOperation(int viewOrSP)
        {
            int returnChoice;
            string executeAgain = "";

            if (viewOrSP == 0)
            {
                executeAgain = "view";
            }
            else if (viewOrSP == 1)
            {
                executeAgain = "stored procedure";
            }

            Console.WriteLine("Your request has been fulfilled. What do you want to do next?\n");
            Console.WriteLine("1. Execute another " + executeAgain + "\n2. Return to main menu\n3. Exit application\n");
            Console.Write("Enter the number of your choice: ");
            returnChoice = FormatData.TryIntConvert(Console.ReadLine());

            if (returnChoice > 3 || returnChoice < 1)
            {
                Console.WriteLine("Your input didn't match any of the options, please try again.");
                ReturnAfterOperation(viewOrSP);
            }
            else
            {
                if (returnChoice == 1)
                {
                    if (viewOrSP == 0)
                    {
                        UseViews();
                    }
                    else if (viewOrSP == 1)
                    {
                        UseStoredProcedures();
                    }
                }
                else if (returnChoice == 2)
                {
                    StartProgram();
                }
                else if (returnChoice == 3)
                {
                    Environment.Exit(0);
                }
            }
        }

        public static void PrintTitle()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Professor Feedback Database Interface");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
