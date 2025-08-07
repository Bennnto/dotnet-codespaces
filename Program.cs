using System.ComponentModel.Design;
using System.Dynamic;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.IO;
using System.Text;

namespace Assignment3
{
    public class Game
    {
        public string itemNumber { get; set; }
        public int userRating { get; set; }
        public double price { get; set; }
        public string itemName { get; set; }
        public Game(string? itemNumber, string? itemName, double price, int userRating)
        {
            this.itemNumber = itemNumber;
            this.itemName = itemName;
            this.userRating = userRating;
            this.price = price;
        }
        public override string ToString()
        {
            return $"{itemNumber},{itemName},{price},{userRating}";
        }
    }
    internal class Program
    {
        private static void Start()
        {
            Console.WriteLine("| WELCOME TO INVENTORY SYSTEM |\n" + breakline);
            Console.WriteLine("| PLEASE SELECT Y(START) or N(EXIT)|: ");
            string input = Console.ReadLine();
            input = input.ToUpper();
            if (input == "Y")
            {
                Main();
            }
            else if (input == "N")
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("|❌ INVALID : invalid input ... Please Try Again|");
                Start();
            }
        }
        private static void Menu()
        {
            Console.WriteLine("Would you like to return to the main menu? (Y/N)");
            string input = Console.ReadLine()?.ToUpper();
            if (input == "Y") // if Y or y will convert to uppercase so if Y execute Main()
            {
                Main();
            }
            else if (input == "N")
            {
                Console.WriteLine("Thank you for using the GameStore System. Goodbye!");
                Environment.Exit(0);
            }
            else
            {   // to handle invalid input
                Console.WriteLine("Invalid input. Please enter Y or N.");
                Menu();
            }
        }
        private static List<string> itNum = new List<string>();
        public static string breakline { get; set; } = "";
        private static string GenerateItemNum()
        { string numBer;
            do
            {
                Random Numb = new Random();
                numBer = Numb.Next(1, 10000).ToString("D4");
            } while (itNum.Contains(numBer));
            itNum.Add(numBer);
            return numBer;
        }
        private static void AddProduct()
        {
            Console.WriteLine(breakline + "\n-------------- ADD NEW ITEM--------------\n" + breakline);
            Console.WriteLine("This function to add Items\n" + "existing Item number/Auto Generate Item Number for New Items\n");

            //Ask for input from "user in each variable
            string itemNumber = "";
            string chkItemnum = @"^\d{4}$";
            Regex ChkNumb = new Regex(chkItemnum);
            Console.WriteLine("🔢 Do you have an Item Number Answer as a number (0 (No), 1(Yes)) :");
            string Ans = Console.ReadLine();
            if (Ans == "1")
            {
                Console.WriteLine("Please Enter Item Number 4 digits : ");
                string ItemNumber = Console.ReadLine();
                if (Regex.IsMatch(ItemNumber, chkItemnum))
                {
                    itemNumber = ItemNumber;
                    Console.WriteLine("Your Item Number input validated");
                }
                else {
                    Console.WriteLine("|❌ INVALID : invalid input ... Please Try Again|");
                    itemNumber = GenerateItemNum();
                }
            }
            if (itemNumber == "")
            {
                itemNumber = GenerateItemNum();
            }
            Console.WriteLine("✅ New Item code is :" + itemNumber);
            Console.WriteLine("🎮 Please Enter Product Name:");
            var itemName = Console.ReadLine();
            Console.WriteLine("New Item Name is :" + itemName);

            //-------------------------------------------------------------
            int userRating; //use do while loop to validate input 0-5
            do
            {
                Console.WriteLine("📈 Please Enter Product Rating 0-5: ");
                userRating = int.Parse(Console.ReadLine());
                if (userRating < 0 || userRating > 5)
                {
                    Console.WriteLine("|❌ INVALID : invalid rating input ... please try again|");
                }
            } while (userRating < 0 || userRating > 5);
            Console.WriteLine("Item Rating is :" + userRating);
            Console.WriteLine("Please Enter Item Price : ");
            var price = double.Parse(Console.ReadLine());
            Console.WriteLine("Item Price is :" + price + "💰");
            var newGame = new Game(itemNumber, itemName, price, userRating);
            Console.WriteLine(breakline + "\nNew Item Add 🎮 \n" + newGame);
            string Line = newGame.ToString();
            string filepath = "videogames.txt";
            using (StreamWriter writer = new StreamWriter(filepath, append: true))
            {
                writer.WriteLine(Line);
            }

            Console.WriteLine("✅ New Item add to system \n " + breakline);
        }
        public static void FindItemNumber()
        {
            Console.WriteLine(breakline + "\n-------------- SEARCH ITEMS -------------- \n" + "This page use Item Number As Indicator"
            + "\nItem Number Characteristic is 4 digits [0-9]{4}$\n" + breakline);
            Console.WriteLine("This function use Item Number (XXXX) for reference");
            Console.WriteLine("Please Enter Item Number 4 digits :");
            var NumberRef = Console.ReadLine();
            string filepath = "videogames.txt";
            bool found = false;
            try
            {
                using StreamReader reader = new StreamReader(filepath);
                string lines;
                while ((lines = reader.ReadLine()) != null)
                {
                    if (lines.StartsWith(NumberRef + ","))
                    {
                        found = true;
                        Console.WriteLine(lines);
                        break;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("Not Match");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Catched");
            }
        }
        public static void Listprice()
        {
            Console.WriteLine(breakline + "\n-------------- List Inventory Price -------------- \n" + "Instruction\n" +
            "When execute this function will lists all items in the inventory \nthat price below or equal to preference Price preference price format XX.XX");
            Console.WriteLine("Please Enter your preference price: ");

            if (double.TryParse(Console.ReadLine(), out double prefprice))
            {
                Console.WriteLine("Your reference price is : " + prefprice);
            }
            else
            {
                Console.WriteLine("|❌ INVALID : invalid input ... Please Try Again|");
                return;
            }
            bool found = false;
            string filepath = "videogames.txt";
            using StreamReader reader1 = new StreamReader(filepath);    
            string line;
            while ((line = reader1.ReadLine()) != null)
            {
                string[] part = line.Split(",");
                if (part.Length >= 3 && double.TryParse(part[2], out double price))
                {
                    if (price <= prefprice)
                    {
                        Console.WriteLine(line);
                        found = true;
                    }
                }
            }
            Menu();
        }
        private static void Average()
        {
            string filepath = "videogames.txt";
            using StreamReader reader2 = new StreamReader(filepath);
            string line;
            double Acc=0;
            double average;
            int i = 0;
            while ((line = reader2.ReadLine()) != null)
            {
                string[] part = line.Split(",");
                if (part.Length >= 3 && double.TryParse(part[2], out double price))
                {
                    Acc += price;
                    i++;
                }
            }
            if (i > 0)
            {
                average = Acc / i;
                Console.WriteLine($"The Average Inventory Price is : {average}");
            }
            else
            {
                Console.WriteLine("|❌ INVALID : not found item matched ... reference|");
            }
        }

        private static void Range()
        {
            string filepath = "videogames.txt";
            double Highprice = double.MaxValue;
            double Lowprice = double.MinValue;
            using StreamReader reader3 = new StreamReader(filepath);
            string line = reader3.ReadLine();
            string Highline = line;
            string Lowline = line;
            while ((line = reader3.ReadLine()) != null)
            {
                string[] part = line.Split(",");
                if (part.Length >= 3 && double.TryParse(part[2], out double price))
                {
                    if (price > Highprice)
                    {
                        Highprice = price;
                        line = Highline;
                    }
                    else if (price < Lowprice)
                    {
                        Lowprice = price;
                        line = Lowline;
                    }
                }
                Console.WriteLine("Inventory Price Range : \n" +
                $"Highest price in the inventory is : {Highline}\n" +
                $"Lowest price in the inventory is : {Lowline}");

            }
        }
        private static void Analyst()
        {
            Console.WriteLine(breakline + "-------------- Inventory Analysis --------------\n" + "Please Select Function to start Analyse\n" +
            "A. Average Inventory Price\n" +
            "B. Inventory Price Range Lowest - Highest Price");
            Console.WriteLine("Please Select between A or B : ");
            string Input = Console.ReadLine();
            Input = Input.ToUpper();
            switch (Input)
            {
                case "A":
                    Console.WriteLine("Average Inventory Price Please Wait System Analyze");
                    Average();
                    Menu();
                    break;
                case "B":
                    Console.WriteLine("Inventory Price Range Please Wait System Analyze");
                    Range();
                    Menu();
                    break;
                default:
                    Console.WriteLine("|❌ INVALID : invalid input ... Please Try Again|");
                    Analyst();
                    break;
            }
        }

        private static void Main()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Start();
            Console.WriteLine("| MAIN MENU |" + "\n| DIRECTORIES |\n");
            Console.WriteLine("| 1. ➕Add Product |\n" + "| 2. 🔎 Search Inventory |\n" + "| 3. 👀Product Lookup |\n" + "| 4. 📊Product Analysis |\n" + breakline);
            Console.WriteLine("| Please Enter 1-5 to Navigate to Function Page : |");
            string inPut = Console.ReadLine();
            Console.WriteLine($"You Entered :  + {inPut}");
            switch (inPut)
            {
                case "1":
                    AddProduct();
                    Menu();
                    break;
                case "2":
                    FindItemNumber();
                    Menu();
                    break;
                case "3":
                    Listprice();
                    Menu();
                    break;
                case "4":
                    Analyst();
                    Menu();
                    break;
                default:
                    Console.WriteLine("|❌ INVALID : invalid input ... Please Try Again|");
                    Main();
                    break;
            }




        }

    }
}