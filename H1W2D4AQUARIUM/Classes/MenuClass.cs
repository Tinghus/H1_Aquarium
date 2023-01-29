namespace H1W2D4AQUARIUM.Classes
{
    internal class MenuClass
    {
        public AquariumClass Aquarium;
        public FishClass Fish;
        public DataClass Data;
        public UiClass Ui;
        public ViewModel CurrentViewModel { get; set; } // Determines what text the console loads

        public int HorizontalMenuItemSelected = 0; // Used for main menu navigation
        public int VerticalMenuItemSelected = 0; // User for sub menu navigation

        public bool MenuItemIsActive = false; // Keeps track of wether or not we are inside a sub menu

        private string[] menuItems = new string[] { "Show Aquariums", "Show Fish", "Add Aquarium", "Remove Aquarium", "Add Fish", "Remove Fish", "Exit" };

        public void ShowMenu()
        {
            // Shows the main menu then adds the context/sub menu

            Console.Clear();

            Console.SetCursorPosition(3, 0);
            Console.Write("Menu |");

            // Loads the menu items
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (HorizontalMenuItemSelected == i && !MenuItemIsActive)
                {
                    // If the current menu items is selected and no submenu is active. Main menu item gets highlighted
                    Console.Write(" ");
                    Ui.ApplyPredefinedEffect(menuItems[i], UiClass.VisualEffects.HoverEffect);
                    Console.Write(" |");
                }
                else if (HorizontalMenuItemSelected == i && MenuItemIsActive)
                {
                    // If the current menu items is selected and a submenu is active. Main menu item gets inactive effect applied
                    Console.Write(" ");
                    Ui.ApplyPredefinedEffect(menuItems[i], UiClass.VisualEffects.InactiveEffect);
                    Console.Write(" |");
                }
                else
                {
                    // If no special rules are present we just display the item normally
                    Console.Write($" {menuItems[i]} |");
                }
            }

            // Adding a line to seperate the main menu from the context menu. Then sets cursor pos so context menu gets displayed in the correct position
            Console.SetCursorPosition(0, 1);
            Ui.DrawMenuLine();
            Console.SetCursorPosition(0, 3);

            LoadViewModel(CurrentViewModel);
        }


        public void LoadViewModel(ViewModel viewModel)
        {
            // Loads the context menu based on the currently active ViewModel

            switch (viewModel)
            {
                case ViewModel.AquariumList:
                    Aquarium.ShowAquariumList();
                    return;

                case ViewModel.FishList:
                    Fish.ShowFishList();
                    return;

                case ViewModel.AddFish:
                    if (!MenuItemIsActive)
                    {
                        Fish.ShowAddFishViewModel();
                    }
                    else if (Aquarium.AquariumList.Count > 0)
                    {
                        Fish.AddFish();
                        MenuItemIsActive = false;
                        ShowMenu();
                    }
                    return;

                case ViewModel.RemoveFish:
                    Fish.ShowFishList();
                    return;

                case ViewModel.AquariumDetails:
                    Aquarium.ShowAquariumDetails(VerticalMenuItemSelected);
                    return;

                case ViewModel.AddAquarium:
                    if (!MenuItemIsActive)
                    {
                        Aquarium.ShowAddAquariumViewModel();
                    }
                    else
                    {
                        Aquarium.AddAquarium();
                        MenuItemIsActive = false;
                        ShowMenu();
                    }
                    return;

                case ViewModel.RemoveAquarium:
                    Aquarium.ShowAquariumList();
                    return;
            }
        }

        public void SelectMenuItem()
        {
            // Used for menu navigation

            ConsoleKeyInfo consoleKey;

            Console.CursorVisible = false;
            consoleKey = Console.ReadKey(true);

            switch (consoleKey.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (MenuItemIsActive)
                    {
                        MenuItemIsActive = false;
                    }
                    ChangeHorizontalMenuItem(-1);
                    return;

                case ConsoleKey.RightArrow:
                    if (MenuItemIsActive)
                    {
                        MenuItemIsActive = false;
                    }
                    ChangeHorizontalMenuItem(1);
                    return;

                case ConsoleKey.UpArrow:
                    ChangeVerticalMenuItem(-1, "up");
                    return;

                case ConsoleKey.DownArrow:
                    ChangeVerticalMenuItem(1, "down");
                    return;

                // If a menu is active we need to send the keypress to the active context/sub menu instead of the main menu 
                case ConsoleKey.Enter:
                    if (MenuItemIsActive)
                    {
                        PressEnterOnActiveMenu();
                    }

                    MenuItemIsActive = true;
                    return;

                default:
                    break;
            }
        }

        private void PressEnterOnActiveMenu()
        {
            // Runs condionally if there is an active context/sub menu

            switch (CurrentViewModel)
            {
                case ViewModel.FishList:
                    break;

                case ViewModel.RemoveFish:
                    Fish.RemoveFish(VerticalMenuItemSelected);
                    break;

                case ViewModel.AquariumList:
                    CurrentViewModel = ViewModel.AquariumDetails;
                    break;

                case ViewModel.RemoveAquarium:
                    Aquarium.RemoveAquarium(VerticalMenuItemSelected);
                    break;

                case ViewModel.Exit:
                    Environment.Exit(0);
                    break;

                default:
                    break;
            }
        }

        private void ChangeVerticalMenuItem(int valueModifier, string keyInitiator)
        {
            // Handles changing of the vertical selected item on the context/sub menu

            int MenuLowerLimit = 0;
            int MenuUpperLimit = 0;

            switch (CurrentViewModel)
            {
                case ViewModel.AquariumList:
                    MenuUpperLimit = Aquarium.AquariumList.Count - 1;
                    break;

                case ViewModel.RemoveAquarium:
                    MenuUpperLimit = Aquarium.AquariumList.Count - 1;
                    break;

                case ViewModel.RemoveFish:
                    MenuUpperLimit = Fish.FishList.Count - 1;
                    break;

                case ViewModel.FishList:
                    MenuUpperLimit = Fish.FishList.Count - 1;
                    break;

                case ViewModel.AddAquarium:
                    return;

                case ViewModel.AddFish:
                    return;

                case ViewModel.AquariumDetails:
                    CurrentViewModel = ViewModel.AquariumList;
                    return;

            }

            // If we are on the main menu and press down we go into the context/sub menu
            if (valueModifier == 1 && !MenuItemIsActive)
            {
                MenuItemIsActive = true;
                return;
            }

            // We need to make sure that we do not try to lookout a non existing menu item
            if (valueModifier == -1 && VerticalMenuItemSelected == MenuLowerLimit)
            {
                MenuItemIsActive = false;
                return;
            }

            // We need to make sure that we do not try to lookout a non existing menu item
            if (valueModifier == 1 && VerticalMenuItemSelected == MenuUpperLimit)
            {
                return;
            }

            // If all checks are passed we change what item is selected
            VerticalMenuItemSelected += valueModifier;
        }

        private void ChangeHorizontalMenuItem(int valueModifier)
        {
            // Handles changin of the horizontal oriented item on the main menu

            // If we are in a context/sub menu we ignore input that should only be valid for the main menu (arrowkey[left] and arrowkey[right])
            if (MenuItemIsActive)
            {
                return;
            }

            // Make sure the selection is valid
            if (valueModifier == -1 && HorizontalMenuItemSelected == 0)
            {
                return;
            }

            // Make sure the selection is valid
            if (valueModifier == 1 && HorizontalMenuItemSelected == menuItems.Length - 1)
            {
                return;
            }

            // If all checks are passed update the selected menu item
            HorizontalMenuItemSelected += valueModifier;
            SetCurrentlySelectedMenuItem();
            ShowMenu();
        }

        private void SetCurrentlySelectedMenuItem()
        {
            // Loads context/sub menu based on the currently selected menu item

            switch (menuItems[HorizontalMenuItemSelected])
            {
                case "Show Aquariums":
                    CurrentViewModel = ViewModel.AquariumList;
                    return;

                case "Show Fish":
                    CurrentViewModel = ViewModel.FishList;
                    return;

                case "Add Aquarium":
                    CurrentViewModel = ViewModel.AddAquarium;
                    return;

                case "Remove Aquarium":
                    CurrentViewModel = ViewModel.RemoveAquarium;
                    return;

                case "Add Fish":
                    CurrentViewModel = ViewModel.AddFish;
                    return;

                case "Remove Fish":
                    CurrentViewModel = ViewModel.RemoveFish;
                    return;

                case "Exit":
                    CurrentViewModel = ViewModel.Exit;
                    return;
            }

        }

        public bool ConfirmAction()
        {
            // This method asks the user to verify their current actions

            ConsoleKeyInfo consoleKey = Console.ReadKey(true);

            switch (consoleKey.Key)
            {
                case ConsoleKey.Y:
                    return true;

                case ConsoleKey.N:
                    return false;

            }

            return false;
        }

        public enum ViewModel
        {
            FishList,
            AddFish,
            RemoveFish,
            Data,
            AquariumList,
            AquariumDetails,
            AddAquarium,
            RemoveAquarium,
            Exit
        }
    }
}
