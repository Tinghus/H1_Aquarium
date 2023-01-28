﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static H1W2D4AQUARIUM.Classes.AquariumClass;
using static System.Collections.Specialized.BitVector32;

namespace H1W2D4AQUARIUM.Classes
{
    internal class FishClass
    {
        public DataClass Data;
        public AquariumClass Aquarium;
        public List<FishObject> FishList = new List<FishObject>();
        public List<SpeciesObject> SpeciesList = new List<SpeciesObject>();

        public MenuClass Menu;


        public void PrepareFishClass()
        {
            BuildSpeciesList();
        }

        public void ShowCreateFishViewModel()
        {
            // Used as context/sub menu when the Add Fish menu is hovered
            Console.WriteLine("Fish:\n");

            // You can not add fish unless you have an aquarium
            if (Aquarium.AquariumList.Count == 0)
            {
                Console.Write("You need to have an aquarium before you can get fish, or they will");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(" DIE!!");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            string output =
                "Name: \n" +
                "Species:                       (use left and right arrow keys)\n" +
                "Watertype: \n" +
                "Aquarium: \n";

            Console.WriteLine(output);
            Console.WriteLine();

            // Displays an overview over the aquariums
            Console.Write(Aquarium.ShowAquariumList());

        }

        public void AddFish()
        {
            // Handles adding fish to aqauriums

            FishObject NewFish = new FishObject();

            Console.CursorVisible = true;
            Console.WriteLine("Fish:\n");

            // Easy way to manipulate where we write our text
            int startingLine = 5;
            Console.SetCursorPosition(0, startingLine);

            string output =
                "Name: \n" +
                "Species:                       (use left and right arrow keys)\n" +
                "Watertype: \n" +
                "Aquarium: \n";

            Console.WriteLine(output);
            Console.WriteLine();
            Console.Write(Aquarium.ShowAquariumList());

            // Name input is valid?
            while (true)
            {
                Console.SetCursorPosition(15, startingLine + 0);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (input.Length > 15)
                    {
                        NewFish.Name = input.Substring(0, 15);
                    }
                    else
                    {
                        NewFish.Name = input;
                    }
                    break;
                }
            }

            // Species input is valid?. We might want to add a species "selecter" instead
            int selectedIndex = 0;
            while (true)
            {
                bool speciesIsSelected = false;

                // Clearing the species line
                Console.SetCursorPosition(15, startingLine + 1);
                Console.Write("                (use left and right arrow keys)");

                // Show the currently selected species
                Console.SetCursorPosition(15, startingLine + 1);
                Console.Write(SpeciesList[selectedIndex].SpeciesName);


                Console.SetCursorPosition(15, startingLine + 2);
                Console.Write(SpeciesList[selectedIndex].SpeciesWatertype + "  ");


                ConsoleKeyInfo consoleKey;

                Console.CursorVisible = false;
                consoleKey = Console.ReadKey(true);

                switch (consoleKey.Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (selectedIndex > 0)
                        {
                            selectedIndex--;
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        if (selectedIndex < SpeciesList.Count - 1)
                        {
                            selectedIndex++;
                        }
                        break;

                    case ConsoleKey.Enter:
                        speciesIsSelected = true;
                        NewFish.Species = SpeciesList[selectedIndex].SpeciesName;
                        break;
                }

                if (speciesIsSelected)
                {
                    Console.CursorVisible = true;
                    break;
                }

            }

            //TODO update watertype to be stored as "Saltwater" and "Freshwater" instead of "f/s"
            // Watertype
            NewFish.Watertype = SpeciesList[selectedIndex].SpeciesWatertype[0].ToString().ToLower();

            // Aquarium check if input can be converted to int. Then checks if the Aquarium exist
            int aquarium = 0;
            while (true)
            {
                Console.SetCursorPosition(15, startingLine + 3);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (int.TryParse(input, out aquarium) && Aquarium.DoAquariumExist(aquarium))
                    {
                        NewFish.Aquarium = aquarium;
                        break;
                    }
                }
            }

            // Checks to make sure the fish can actually live in the aquarium. We might want to do a size check as well
            if (NewFish.Watertype == Aquarium.GetAquariumDetails(null, NewFish.Aquarium).Watertype)
            {
                NewFish.FishId = FindAvailableId();
                FishList.Add(NewFish);
                Data.SaveData("fish");
                Menu.MenuItemIsActive = false;
                return;
            }

            // Mismatched watertype between fish and aquarium
            Console.Clear();
            Console.WriteLine("\nYou put the fish in the wrong tank and it died\n");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("YOU MONSTER !!!");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.CursorVisible = false;
            Console.ReadKey();


        }

        public void RemoveFish(int fishPos)
        {
            // Removes the fish

            // Warns the user that they are about to delete some data
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("You are currently trying to delete this item :");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(GetFriendlyName(FishList[fishPos].FishId));
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("This action cannot be undone. \n are you sure this is what you want to do?");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("y/n");

            // Checks for user confirmation
            bool DeleteThis = Menu.ConfirmAction();

            // Removes the fish
            if (DeleteThis)
            {
                FishList.RemoveAt(fishPos);
                Data.SaveData("fish");
                Menu.MenuItemIsActive = false;
                Menu.ShowMenu();
            }

        }

        public string GetFriendlyName(int id)
        {
            //Used to return Fish Id together with name.

            foreach (FishObject fish in FishList)
            {
                if (fish.FishId == id)
                {
                    return "[" + fish.FishId + "] " + fish.Name;
                }
            }
            return null;

        }

        public string ShowFishList()
        {
            // Writes list of all fish to the console
            // We no longer need a return type as we changed the method from GetFishList to ShowFishList

            string output;

            // Before we try to write 
            if (FishList.Count == 0)
            {
                Console.WriteLine("You got no fish yet :-(");
                return "";
            }

            Console.WriteLine("Fish:\n");

            output = "Id".PadRight(5) + "Name".PadRight(15) + "Species".PadRight(12) + "Aquarium".PadRight(25) + "WaterType";
            Console.WriteLine(output);

            // Outputs a list of all the fish to console
            for (int i = 0; i < FishList.Count; i++)
            {
                FishObject fish = FishList[i];

                // Apply the hover effect if the fish is currently selected
                if (Menu.MenuItemIsActive && Menu.VerticalMenuItemSelected == i)
                {
                    Menu.HoverEffect(true);
                }

                output = Convert.ToString(fish.FishId).PadRight(5) + fish.Name.PadRight(15) + fish.Species.PadRight(12) + Aquarium.GetFriendlyName(fish.Aquarium).PadRight(25) + fish.Watertype;
                Console.WriteLine(output);

                // Ensures that the hover effect is only applied to the relevant line
                if (Menu.MenuItemIsActive)
                {
                    Menu.HoverEffect(false);
                }

            }

            return "";
        }

        private int FindAvailableId()
        {
            //Finds the next id 

            // If there is no fish in the list we can safely assume that 1 is available as id
            if (FishList.Count == 0)
            {
                return 1;
            }

            // Lambda "function" that finds the highest currently used id. Then adds 1 and returns it as the next available id
            int nextId = FishList.Max(id => id.FishId) + 1;

            return nextId;
        }

        public void BuildSpeciesList()
        {
            SpeciesList.Add(new SpeciesObject
            {
                SpeciesName = "Neon tetra",
                SpeciesWatertype = "Freshwater",
                SpeciesSize = 1
            });

            SpeciesList.Add(new SpeciesObject
            {
                SpeciesName = "Guppy",
                SpeciesWatertype = "Freshwater",
                SpeciesSize = 2
            });


            SpeciesList.Add(new SpeciesObject
            {
                SpeciesName = "Goldfish",
                SpeciesWatertype = "Freshwater",
                SpeciesSize = 5
            });


            SpeciesList.Add(new SpeciesObject
            {
                SpeciesName = "Clownfish",
                SpeciesWatertype = "Saltwater",
                SpeciesSize = 3
            });


            SpeciesList.Add(new SpeciesObject
            {
                SpeciesName = "Flame Angelfish",
                SpeciesWatertype = "Saltwater",
                SpeciesSize = 4
            });

            SpeciesList.Add(new SpeciesObject
            {
                SpeciesName = "Blue Tang",
                SpeciesWatertype = "Saltwater",
                SpeciesSize = 6
            });

        }

        public class FishObject
        {
            public int FishId { get; set; }
            public string Name { get; set; }
            public int Aquarium { get; set; }
            public string Species { get; set; }
            public string Watertype { get; set; }
        }

        public class SpeciesObject
        {
            public string SpeciesName { get; set; }
            public string SpeciesWatertype { get; set; }
            public int SpeciesSize { get; set; }
        }
    }
}
