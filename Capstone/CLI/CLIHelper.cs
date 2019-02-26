using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.CLI
{
    public class CLIHelper
    {
        public static int ParseAsInteger(string input)
        {
            int output;
            do
            {
                try
                {
                    output = Int32.Parse(input);
                }
                catch (Exception)
                {
                    output = -1;
                    Console.Write("Invalid input. Please try again: ");
                    input = Console.ReadLine();
                }
            } while (output < 0);

            return output;
        }

        public static DateTime ParseAsDateTime(string input)
        {
            DateTime output = new DateTime();
            bool successful = false;

            while (!successful)
            {
                try
                {
                    output = DateTime.Parse(input);
                    if (output < DateTime.Now.Date)
                    {
                        Console.Write("Date can not be in the past! Please try again (MM/DD/YYYY): ");
                        input = Console.ReadLine();
                    }
                    else
                    {
                        successful = true;
                    }
                }
                catch (Exception)
                {
                    Console.Write("Invalid input. Please try again (MM/DD/YYYY): ");
                    input = Console.ReadLine();
                }
            }
            return output;
        }

        public static DateTime ParseAsDateTime(string input, DateTime fromDate)
        {
            DateTime output = new DateTime();
            bool successful = false;

            while (!successful)
            {
                try
                {
                    output = DateTime.Parse(input);
                    if (output <= fromDate.Date)
                    {
                        Console.Write("Departure date must be after arrival date! Please try again (MM/DD/YYYY): ");
                        input = Console.ReadLine();
                    }
                    else
                    {
                        successful = true;
                    }
                }
                catch (Exception)
                {
                    Console.Write("Invalid input. Please try again (MM/DD/YYYY): ");
                    input = Console.ReadLine();
                }
            }
            return output;
        }

        public static string GetString(string input)
        {
            while (String.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Invalid input. Please try again: ");
                input = Console.ReadLine();
            }
            return input;
        }

        public static int GetChoiceIndexFromSiteNumber(List<SearchReservation> reservations, int desiredSiteNumber)
        {
            int output = -1;
            bool numFound = false;

            do
            {
                for(int i=0; i<reservations.Count; i++)
                {
                    if (reservations[i].SiteNumber == desiredSiteNumber)
                    {
                        output = i;
                        numFound = true;
                    }
                }
                if (!numFound)
                {
                    Console.WriteLine("Invalid Site Number, please try again: ");
                    output = CLIHelper.ParseAsInteger(Console.ReadLine());
                }
            } while (!numFound);

            return output;
        }

        public static int ValidateSearchResults(List<SearchReservation> searchResults)
        {
            int output = 0; // 1: search results valid, 0: search results empty, -1: search results empty, user does not want to try another search

            if(searchResults.Count > 0)
            {
                output = 1;
            }
            else
            {
                Console.Write("Unfortunately there are no sites available in that date range. Would you like to try again? (Y/N): ");
                string userInput = CLIHelper.GetString(Console.ReadLine());
                if(userInput.ToLower() == "y")
                {
                    output = 0;
                }
                else
                {
                    output = -1;
                }
            }

            return output;
        }
    }
}
