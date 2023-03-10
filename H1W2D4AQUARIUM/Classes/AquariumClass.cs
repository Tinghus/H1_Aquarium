namespace H1W2D4AQUARIUM.Classes
{
    internal class AquariumClass
    {
        public DataClass Data;
        public MenuClass Menu;
        public UiClass Ui;
        public FishClass Fish;

        public List<AquariumObject> AquariumList = new List<AquariumObject>();
        public string GetFriendlyName(int aquariumID)
        {
            //Used to return Aquarium Id together with name.

            foreach (AquariumObject aquarium in AquariumList)
            {
                if (aquarium.AquariumId == aquariumID)
                {
                    return "[" + aquarium.AquariumId + "] " + aquarium.Name;
                }
            }
            return Convert.ToString(aquariumID);
        }

        public void ShowAquariumDetails(int aquariumPos)
        {
            // Used to display aquarium details along with a list of all the fish in the selected aquarium

            AquariumObject aquarium = new AquariumObject();

            aquarium = GetAquariumDetails(aquariumPos, null);

            if (aquarium == null)
            {
                return;
            }

            int numberOfFish = 0;

            string outPutFishDetails = "";

            // Builds the fish list and prepares it for output. We build it here because we need some of the data for the aquarium info.
            foreach (FishClass.FishObject fish in Fish.FishList)
            {
                if (fish.AquariumId == aquarium.AquariumId)
                {
                    outPutFishDetails += Convert.ToString(fish.FishId).PadRight(5) + fish.Name.PadRight(15) + fish.Species.PadRight(12) + "\n";
                    numberOfFish++;
                }
            }

            // Displays the aquarium details
            string outputAquariumDetails =
                "Id: ".PadRight(17) + aquarium.AquariumId +
                "\nName: ".PadRight(17) + aquarium.Name +
                "\nTemperature: ".PadRight(17) + aquarium.temperature.ToString() +
                "\nSize: ".PadRight(17) + aquarium.Size.ToString() +
                "\nWatertype: ".PadRight(17) + aquarium.Watertype +
                "\nNumber of fish: ".PadRight(17) + numberOfFish.ToString();

            Console.WriteLine(outputAquariumDetails);
            Console.WriteLine();

            // Only show the fish table if the is actually some fish to display
            if (numberOfFish > 0)
            {
                Console.WriteLine("Fish Overview");
                Console.WriteLine("Id".PadRight(5) + "Name".PadRight(15) + "Species".PadRight(12));
                Console.Write(outPutFishDetails);
            }
        }

        public void ShowAquariumList()
        {
            // Shows a list of all aquariums

            string output;

            if (AquariumList.Count == 0)
            {
                Console.Write("You go no aquariums yet. Go to \"Add Aquarium\" to get started");
                return;
            }

            Console.WriteLine("Aquariums:\n");

            output = "Id".PadRight(5) + "Name".PadRight(15) + "Watertype".PadRight(12) + "Temperature".PadRight(14) + "Size";
            Console.WriteLine(output);

            for (int i = 0; i < AquariumList.Count; i++)
            {
                AquariumObject aquarium = AquariumList[i];

                output = Convert.ToString(aquarium.AquariumId).PadRight(5) + aquarium.Name.PadRight(15) + aquarium.Watertype.PadRight(12) + Convert.ToString(aquarium.temperature).PadRight(14) + Convert.ToString(aquarium.Size) + "\n";

                // Apply hover effect to the selected item
                if (Menu.MenuItemIsActive && Menu.VerticalMenuItemSelected == i && Menu.CurrentViewModel != MenuClass.ViewModel.AddFish)
                {
                    Ui.ApplyPredefinedEffect(output, UiClass.VisualEffects.HoverEffect);
                }
                else
                {
                    Console.Write(output);
                }
            }
        }

        private int FindAvailableId()
        {
            // Returns a unique id

            if (AquariumList.Count == 0)
            {
                return 1;
            }

            int nextId = AquariumList.Max(id => id.AquariumId) + 1;

            return nextId;
        }

        public AquariumObject GetAquariumDetails(int? aquariumPos, int? aquariumId)
        {
            // Returns an AquariumObject based on either its position on the list or its id

            if (aquariumPos == null)
            {
                for (int i = 0; i < AquariumList.Count; i++)
                {
                    if (AquariumList[i].AquariumId == aquariumId)
                    {
                        return AquariumList[i];
                    }
                }
            }

            if (aquariumPos < AquariumList.Count)
            {
                return AquariumList[aquariumPos ?? 0];
            }

            return null;
        }

        public void ShowAddAquariumViewModel()
        {
            Ui.ApplyViewModel(MenuClass.ViewModel.AddAquarium);
        }

        public void AddAquarium()
        {
            Console.CursorVisible = true;

            AquariumObject NewAquarium = new AquariumObject();

            Ui.ApplyViewModel(MenuClass.ViewModel.AddAquarium);

            int startingLine = 5;

            GetAquariumName(NewAquarium, startingLine);
            GetAquariumSize(NewAquarium, startingLine);
            GetAquariumWaterType(NewAquarium, startingLine);
            GetAquariumTemperature(NewAquarium, startingLine);

            NewAquarium.AquariumId = FindAvailableId();
            AquariumList.Add(NewAquarium);
            Data.SaveData("aquarium");
        }

        private void GetAquariumName(AquariumObject NewAquarium, int startingLine)
        {
            while (true)
            {
                Console.SetCursorPosition(19, startingLine + 0);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (input.Length > 15)
                    {
                        NewAquarium.Name = input.Substring(0, 15);
                    }
                    else
                    {
                        NewAquarium.Name = input;
                    }
                    break;
                }
            }
        }

        private void GetAquariumSize(AquariumObject NewAquarium, int startingLine)
        {
            int size = 0;

            while (true)
            {
                Console.SetCursorPosition(19, startingLine + 1);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (int.TryParse(input, out size))
                    {
                        NewAquarium.Size = size;
                        break;
                    }
                }
            }
        }

        private void GetAquariumWaterType(AquariumObject NewAquarium, int startingLine)
        {
            while (true)
            {
                Console.SetCursorPosition(19, startingLine + 2);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (input.ToLower() == "f" || input.ToLower() == "s")
                    {
                        NewAquarium.Watertype = input == "f" ? "Freshwater" : "Saltwater";
                        break;
                    }
                }
            }
        }

        private void GetAquariumTemperature(AquariumObject NewAquarium, int startingLine)
        {
            double temperature = 0;
            while (true)
            {
                Console.SetCursorPosition(19, startingLine + 3);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (double.TryParse(input, out temperature))
                    {
                        NewAquarium.temperature = temperature;
                        break;
                    }
                }
            }
        }

        public void RemoveAquarium(int aquariumPos)
        {
            Ui.ChangeTextColor("You are currently trying to delete this item : \n", ConsoleColor.DarkRed);
            Ui.ChangeTextColor(GetFriendlyName(AquariumList[aquariumPos].AquariumId) + "\n", ConsoleColor.DarkBlue);
            Ui.ChangeTextColor("This action cannot be undone. \n are you sure this is what you want to do?\n", ConsoleColor.DarkRed);
            Console.WriteLine("y/n");

            bool DeleteThis = Menu.ConfirmAction();

            if (DeleteThis)
            {
                AquariumList.RemoveAt(aquariumPos);
                Data.SaveData("aquarium");
                Menu.MenuItemIsActive = false;
                Menu.ShowMenu();
            }
        }

        public class AquariumObject
        {
            public int AquariumId { get; set; }
            public string Name { get; set; }
            public double temperature { get; set; }
            public int Size { get; set; }
            public string Watertype { get; set; }
        }
    }
}
