using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace H1W2D4AQUARIUM.Classes
{
    internal class DataClass
    {
        // Adding classes so that we can access the data in them. We could also just send the data as objects and handle it that way.
        public FishClass Fish;
        public AquariumClass Aquarium;

        public void PrepareProgram()
        {
            // Makes sure that the required data files exist. If they do not we create them. Then we load the data

            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Fish.dat"))
            {
                using (File.Create(AppDomain.CurrentDomain.BaseDirectory + "Fish.dat")) { }
            }

            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Aquarium.dat"))
            {
                using (File.Create(AppDomain.CurrentDomain.BaseDirectory + "Aquarium.dat")) { }
            }

            LoadData();
        }
        public void LoadData()
        {
            //Loads the relevant data

            LoadAquarium();
            LoadFish();
        }

        private void LoadAquarium()
        {
            // Check that there are contents in the files and then load them. We could potentially add another layer of validation and make sure the file is not corrupt

            string jsonData = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Aquarium.dat");
            if (!string.IsNullOrWhiteSpace(jsonData))
            {
                Aquarium.AquariumList = JsonSerializer.Deserialize<List<AquariumClass.AquariumObject>>(jsonData);
            }
        }

        private void LoadFish()
        {
            // Check that there are contents in the files and then load them. We could potentially add another layer of validation and make sure the file is not corrupt

            string jsonData = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Fish.dat");
            if (!string.IsNullOrWhiteSpace(jsonData))
            {
                Fish.FishList = JsonSerializer.Deserialize<List<FishClass.FishObject>>(jsonData);
            }

        }

        public void SaveData(string arg)
        {
            // Saves data based on the passed in arguments as we only want to update the files if they have changes

            string jsonData;

            if (arg == "all" || arg == "fish")
            {
                SaveFish();
            }

            if (arg == "all" || arg == "aquarium")
            {
                SaveAquarium();
            }
        }

        private void SaveAquarium()
        {
            // Serializes AquariumList and save to file

            string jsonData = JsonSerializer.Serialize(Aquarium.AquariumList);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Aquarium.dat", jsonData);
        }

        private void SaveFish()
        {
            // Serializes FishList and save to file

            string jsonData = JsonSerializer.Serialize(Fish.FishList);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Fish.dat", jsonData);
        }
    }
}
