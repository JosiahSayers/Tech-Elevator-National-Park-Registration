using Capstone.CLI;
using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using Console = Colorful.Console;
using Colorful;

namespace Capstone
{
    class ProgramCLI
    {
        private string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=NationalParkReservation;Integrated Security=True";
        public int userChoice;
        public string userInput;
        private Park currentPark = new Park();
        StyleSheet styleSheet = new StyleSheet(Color.White);

        public void RunCLI()
        {
            Console.ForegroundColor = Color.White;
            styleSheet.AddStyle($"SELECT", (Color.LimeGreen));

            Console.Title = "National Park Campsite Reservation Service";
           
            string titlePage = @"

             _____ ______ _      ______ _____ _______      _           
            / ____|  ____| |    |  ____/ ____|__   __|  /\| |/\        
           | (___ | |__  | |    | |__ | |       | |     \ ` ' /        
            \___ \|  __| | |    |  __|| |       | |    |_     _|       
            ____) | |____| |____| |___| |____   | |     / , . \        
           |_____/|______|______|______\_____|  |_|  _  \/|_|\/        
           |  __ \                              | | (_)                
           | |__) |___  ___  ___ _ ____   ____ _| |_ _  ___  _ __  ___ 
           |  _  // _ \/ __|/ _ \ '__\ \ / / _` | __| |/ _ \| '_ \/ __|
           | | \ \  __/\__ \  __/ |   \ V / (_| | |_| | (_) | | | \__ \
           |_|  \_\___||___/\___|_|    \_/ \__,_|\__|_|\___/|_| |_|___/
                                                               
            
            TOP 1 Console App FOR National Park Campsite Reservations!
                       
                     (press ENTER to plan your Parkcation!)                   

";
            string camper = @"
                                    ,------.
                                   / [<>]   \
                                ,-|         /
                                  '-----OO-'
";

            Console.WriteLine(titlePage, Color.LimeGreen);
            Console.WriteLine(camper, Color.Orange);
            Console.ReadKey();


            while (true)
            {
                Console.Clear();
                PrintAllParks();
            }
        }

        void PrintAllParks()
        {
            ParkDAL dal = new ParkDAL(connectionString);
            List<Park> parks = dal.GetAllParks();


            Console.WriteLineStyled("SELECT a National Park Destination!\n", styleSheet);

            for (int i = 0; i < parks.Count; i++)
            {
                Console.Write($"{i + 1}) ");
                Console.WriteLine($"{parks[i].ParkName}", Color.Orange);
            }

            Console.WriteLine("\nQ) Exit Program");

            Console.WriteStyled("\n\nSELECT a command: ", styleSheet);

            userInput = CLIHelper.GetString(Console.ReadLine());


            if (userInput.ToLower() == "q")
            {
                Environment.Exit(0);
            }
            else
            {
                userChoice = CLIHelper.ParseAsInteger(userInput);
                if (userChoice - 1 >= 0 && userChoice - 1 < parks.Count)
                {
                    currentPark = parks[userChoice - 1];
                    ParkInformation();
                }
            }
        }

        void ParkInformation()
        {
            bool menuRunning = true;

            while (menuRunning)
            {
                Console.Clear();

                Console.WriteLine("National Park Details:\n", Color.LimeGreen);
                Console.WriteAscii($"{currentPark.ParkName}", FigletFont.Default, Color.Orange);
                Console.WriteLine($"Location: {currentPark.Location}");
                Console.WriteLine($"Established: {currentPark.EstablishedDate.ToShortDateString()}");
                Console.WriteLine($"Area (acres): {currentPark.Area.ToString("N0")}");
                Console.WriteLine($"Annual Visitors: {currentPark.AnnualVisitors.ToString("N0")}");
                Console.WriteLine($"\n{currentPark.Description}\n", Color.LimeGreen);


                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------", Color.Orange);

                Console.WriteLine("1) View Campgrounds");
                Console.WriteLine("2) Search for Reservation");
                Console.WriteLine("3) Return to Previous Screen");
                Console.WriteStyled("\nSELECT a command: ", styleSheet);

                userChoice = CLIHelper.ParseAsInteger(Console.ReadLine());

                switch (userChoice)
                {
                    case 1:
                        Console.WriteLine("Park Campgrounds");
                        ViewParkCampgrounds(true);
                        break;
                    case 2:
                        ReservationSearch();
                        break;
                    case 3:
                        menuRunning = false;
                        break;
                    default:
                        Console.WriteLine("Not a valid choice");
                        break;
                }
            }

        }
       
        List<Campground> ViewParkCampgrounds(bool addWait)
        {
            List<Campground> campgrounds;
            bool menuRunning = true;

            do
            {
                Console.Clear();

                CampgroundDAL dal = new CampgroundDAL(connectionString);
                campgrounds = dal.GetCampgrounds(currentPark);

                Console.WriteLine("Campground Details:\n", Color.LimeGreen);
                Console.WriteAscii($"{currentPark.ParkName}", FigletFont.Default, Color.Orange);
                
                
                Console.WriteLine("{0, -2} {1, -30} {2,-30} {3, -25} {4, -25}", "", "", "Season", "Season", "Daily");
                Console.WriteLine("{0, -2} {1, -30} {2,-30} {3, -25} {4, -25}", "", "Campground", "Begins", "Ends", "Fee");
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------", Color.LimeGreen);

                for (int i = 0; i < campgrounds.Count; i++)
                {
                    Console.WriteLine("{0, -2} {1, -30} {2,-30} {3, -25} {4, -25:C0}", $"{i + 1})", campgrounds[i].CampgroundName, campgrounds[i].OpenMonth, campgrounds[i].CloseMonth, campgrounds[i].DailyFee);
                }

                if (addWait)
                {
                    Console.WriteLine("\n------------------------------------------------------------------------------------------------------------------------", Color.Orange);
                    Console.WriteLine("1) Search for Available Reservation");
                    Console.WriteLine("2) Return to Previous Screen");
                    Console.WriteStyled("\nSELECT a command: ", styleSheet);


                    userChoice = CLIHelper.ParseAsInteger(Console.ReadLine());

                    switch (userChoice)
                    {
                        case 1:
                            ReservationSearch();
                            break;
                        case 2:
                            menuRunning = false;
                            break;
                        default:
                            Console.WriteLine("Not a valid choice");
                            break;
                    }
                }
            } while (menuRunning && addWait);

            return campgrounds;
        }

        void ReservationSearch()
        {
            bool menuRunning = true;
            bool emptySearchResults = true;

            while (menuRunning)
            {
                Console.Clear();
                
                ReservationDAL dal = new ReservationDAL(connectionString);
                List<SearchReservation> searchResults = new List<SearchReservation>();

                Console.WriteLine("Search for Campground Reservation");
                List<Campground> campgrounds = ViewParkCampgrounds(false);

                Console.WriteStyled("\nSELECT Campground (enter 0 to cancel): ", styleSheet);
                userChoice = CLIHelper.ParseAsInteger(Console.ReadLine());
                if (userChoice == 0)
                {
                    return;
                }
                else
                {
                    userChoice--;
                }

                while (emptySearchResults)
                {
                    Console.WriteStyled("SELECT Arrival Date (MM/DD/YYYY): ", styleSheet);
                    DateTime arrivalDate = CLIHelper.ParseAsDateTime(Console.ReadLine());

                    Console.WriteStyled("SELECT Departure Date (MM/DD/YYYY): ", styleSheet);
                    DateTime departureDate = CLIHelper.ParseAsDateTime(Console.ReadLine(), arrivalDate);

                    searchResults = dal.ReservationSearch(campgrounds[userChoice], arrivalDate, departureDate);

                    int validSearchReuslts = CLIHelper.ValidateSearchResults(searchResults);
                    if(validSearchReuslts == -1)
                    {
                        return;
                    }
                    else if(validSearchReuslts == 0)
                    {
                        emptySearchResults = true;
                    }
                    else if(validSearchReuslts == 1)
                    {
                        emptySearchResults = false;
                    }
                }

                Console.WriteLine("\nAvailable Campsites:\n", Color.Orange);
                Console.WriteLine("{0, -13} {1, -15} {2, -12} {3, -15} {4, -11} {5, -13}", "Site Number", "Max Occupancy", "Accessible", "Max RV Length", "Utilities", "Total Price");
                foreach (SearchReservation result in searchResults)
                {
                    string accessible = result.Accessible == false ? "No" : "Yes";
                    string maxRvLength = result.MaxRvLength == 0 ? "N/A" : $"{result.MaxRvLength}";
                    string utilities = result.Utilites == false ? "N/A" : "Yes";
                    Console.WriteLine("{0, -13} {1, -15} {2, -12} {3, -15} {4, -11} {5, -13:C0}", result.SiteNumber, result.MaxOccupancy, accessible, maxRvLength, utilities, result.TotalPrice);
                }
                BookReservation(searchResults);
            }
        }

        void BookReservation(List<SearchReservation> searchResults)
        {
            ReservationDAL dal = new ReservationDAL(connectionString);
            styleSheet.AddStyle($"Confirmation ID", (Color.Orange));
            bool reservationCreated = false;

            while (!reservationCreated)
            {
                Console.WriteStyled("\nSELECT Campsite: ", styleSheet);
                int siteNumber = CLIHelper.ParseAsInteger(Console.ReadLine());
                int desiredSiteIndex = CLIHelper.GetChoiceIndexFromSiteNumber(searchResults, siteNumber);
                Console.Write("What name should the reservation be made under: ");
                string name = CLIHelper.GetString(Console.ReadLine());

                Reservation newReservation = new Reservation();
                newReservation.EndDate = searchResults[desiredSiteIndex].ToDate;
                newReservation.StartDate = searchResults[desiredSiteIndex].FromDate;
                newReservation.ReservationName = name;
                newReservation.SiteId = searchResults[desiredSiteIndex].SiteId;

                int reservationId = dal.CreateReservation(newReservation);

                if (reservationId < 0)
                {
                    Console.WriteLine("An error occured. Press enter to try again.");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("\nCONGRATULATIONS: Parkcation Reservation Confirmed!", Color.LimeGreen);
                    Console.WriteLineStyled($"Confirmation ID: #{reservationId}", styleSheet);
                    Console.ReadLine();
                    reservationCreated = true;
                }

            }
        }

       
    }




}

