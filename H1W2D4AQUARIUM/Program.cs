using H1W2D4AQUARIUM.Classes;

namespace H1W2D4AQUARIUM
{
    internal class Program
    {
        private static MenuClass Menu = new MenuClass();
        private static FishClass Fish = new FishClass();
        private static AquariumClass Aquarium = new AquariumClass();
        private static DataClass Data = new DataClass();
        private static UiClass Ui = new UiClass();

        private static void Main(string[] args)
        {
            BuildReferences();
            Data.PrepareProgram();
            Fish.BuildSpeciesList();

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

            Menu.Data = Data;
            Menu.Ui = Ui;
            Menu.Fish = Fish;
            Menu.Aquarium = Aquarium;

            Ui.Menu = Menu;

            Fish.Data = Data;
            Fish.Menu = Menu;
            Fish.Ui = Ui;
            Fish.Aquarium = Aquarium;

            Aquarium.Data = Data;
            Aquarium.Menu = Menu;
            Aquarium.Ui = Ui;
            Aquarium.Fish = Fish;
        }
    }
}