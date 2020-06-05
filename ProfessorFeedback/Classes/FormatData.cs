using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfessorFeedback.Classes
{
    static class FormatData
    {
        public static float TryFloatConvert(string input)
        {
            float result;
            try
            {
                result = Convert.ToSingle(input);
            }
            catch (FormatException)
            {
                Console.Write("Data type invalid. Please enter a valid input: ");
                result = TryFloatConvert(Console.ReadLine());
            }
            return result;
        }

        public static int TryIntConvert(string input)
        {
            int result;
            try
            {
                result = Convert.ToInt32(input);
            }
            catch (FormatException)
            {
                Console.Write("Data type invalid. Please enter a valid input: ");
                result = TryIntConvert(Console.ReadLine());
            }
            return result;
        }

        public static DateTime TryDateTimeConvert(string input)
        {
            DateTime result;
            try
            {
                result = Convert.ToDateTime(input);
            }
            catch (FormatException)
            {
                Console.Write("Data type invalid. Please enter a valid input: ");
                result = TryDateTimeConvert(Console.ReadLine());
            }
            return result;
        }
    }
}