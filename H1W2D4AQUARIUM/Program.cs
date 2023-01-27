using H1W2D4AQUARIUM.Classes;

namespace H1W2D4AQUARIUM
{
    internal class Program
    {
        static MenuClass Menu = new MenuClass();
        static FishClass Fish = new FishClass();
        static AquariumClass Aquarium = new AquariumClass();
        static DataClass Data = new DataClass();
        static void Main(string[] args)
        {
            BuildReferences();
            Data.PrepareProgram();

            Menu.CurrentViewModel = MenuClass.ViewModel.AquariumList;
            Console.CursorVisible = false;

            while (true)
            {
                Menu.ShowMenu();
                Menu.SelectMenuItem();
            }

        }

        private static void BuildReferences()
        {
            // Build references for our classes
            Data.Aquarium = Aquarium;
            Data.Fish = Fish;

            Fish.Aquarium = Aquarium;
            Fish.Menu = Menu;

            Aquarium.Menu = Menu;
            Aquarium.Data = Data;
            Aquarium.Fish = Fish;

            Fish.Data = Data;
            Menu.Data = Data;

            Menu.Fish = Fish;
            Menu.Aquarium = Aquarium;
        }
    }
}