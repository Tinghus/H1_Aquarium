namespace H1W2D4AQUARIUM.Classes
{
    internal class UiClass
    {
        public MenuClass Menu;
        public AquariumClass Aquarium;

        public enum VisualEffects
        {
            HoverEffect = 0,
            InactiveEffect = 1,
        }

        public void ChangeTextColor(string stringToColor, ConsoleColor colorToUse)
        {
            ConsoleColor startingColor = Console.ForegroundColor;
            Console.ForegroundColor = colorToUse;

            Console.Write(stringToColor);

            Console.ForegroundColor = startingColor;
        }

        public void ApplyPredefinedEffect(string stringToStyle, VisualEffects visualEffect)
        {
            ConsoleColor startingForegroundColor = Console.ForegroundColor;
            ConsoleColor startingBackgroundColor = Console.BackgroundColor;

            switch (visualEffect)
            {
                case VisualEffects.HoverEffect:
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case VisualEffects.InactiveEffect:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;

                default:
                    break;
            }

            Console.Write(stringToStyle);

            Console.ForegroundColor = startingForegroundColor;
            Console.BackgroundColor = startingBackgroundColor;
        }

        public void DrawMenuLine()
        {
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write("-");
            }
        }

        public void ApplyViewModel(MenuClass.ViewModel viewModel)
        {
            string output = "";

            switch (viewModel)
            {
                case MenuClass.ViewModel.FishList:
                    break;

                case MenuClass.ViewModel.AddFish:
                    output = ViewModelAddFish();
                    break;

                case MenuClass.ViewModel.RemoveFish:
                    break;

                case MenuClass.ViewModel.Data:
                    break;

                case MenuClass.ViewModel.AquariumList:
                    break;

                case MenuClass.ViewModel.AquariumDetails:
                    break;

                case MenuClass.ViewModel.AddAquarium:
                    output = ViewModelAddAquarium();
                    break;

                case MenuClass.ViewModel.RemoveAquarium:
                    break;

                case MenuClass.ViewModel.Exit:
                    break;

            }

            Console.Write(output);
        }

        private string ViewModelAddFish()
        {
            string output =
                "Name: \n" +
                "Species:                             (use left and right arrow keys, enter to confirm)\n" +
                "Watertype: \n" +
                "Aquarium:                            (use left and right arrow keys, enter to confirm)\n";

            return output;
        }


        private string ViewModelAddAquarium()
        {
            string output =
                "Add new aquarium: \n\n" +
                "Name: \n" +
                "Size in liters :\n" +
                "Watertype f/s: \n" +
                "Temperature (c): \n";

            return output;
        }

        private string ViewModelAquariumDetails(AquariumClass.AquariumObject aquarium)
        {
            string output =
                "Id: ".PadRight(17) + aquarium.AquariumId +
                "\nName: ".PadRight(17) + aquarium.Name +
                "\nTemperature: ".PadRight(17) + aquarium.temperature.ToString() +
                "\nSize: ".PadRight(17) + aquarium.Size.ToString() +
                "\nWatertype: ".PadRight(17) + aquarium.Watertype;


            return output;
        }

    }
}
