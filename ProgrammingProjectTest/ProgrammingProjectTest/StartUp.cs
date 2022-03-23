using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingProjectTest
{
    class StartUp
    {
        CreatureTemplateList CreatureTemplateList;
        MoveList moveList;

        public StartUp()
        {
            Console.CursorVisible = false;
            CreatureType arrayInitialiser = new CreatureType(""); //initialises static CreatureType array
            Status statusArrayInitialiser = new Status(0);
            CreatureTemplateList = new CreatureTemplateList();
            moveList = new MoveList();
            InitiateGame();
            Console.ReadLine();

        }
        
        public void InitiateGame()
        {
            Creature[] playerCreatures = new Creature[18];
            Menu menu = CreateSaveMenu();

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(36, 5);
            Console.WriteLine("USE ARROW KEYS TO MOVE AND PRESS ENTER TO SELECT");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(32, 7);
            Console.WriteLine("do you want to open an existing save or make a new game?");
            menu.Draw();

            menu.SetPointer(0, 0);
            while(menu.OptionSelected == -1)
            {
                menu.GetInput();
            }
            if (menu.OptionSelectedReset == 100)
            {
                StarterSelection(playerCreatures);
            }
            else
            {

            }

        }

        public Menu CreateSaveMenu()
        {
            Menu menu;
            string[,] display = new string[2, 1];
            int[,,] coOrds = new int[2,1,2];
            display[0, 0] = "New Save";
            display[1, 0] = "Open file";
            coOrds[0, 0, 0] = 49;
            coOrds[0, 0, 1] = 13;
            coOrds[1, 0, 0] = 60;
            coOrds[1, 0, 1] = 13;

            menu = new Menu(display, coOrds);
            return menu;
        }

        public void StarterSelection(Creature[] playerCreatures)
        {
            int fightIndex = 0;

            Console.Clear();
            Menu menu = CreateStarterChoiceMenu();
            Creature option1 = new Creature(CreatureTemplateList.Templates[0], moveList);
            Creature option2 = new Creature(CreatureTemplateList.Templates[1], moveList);
            Creature option3 = new Creature(CreatureTemplateList.Templates[2], moveList);

            menu.Draw();
            Console.SetCursorPosition(45, 0);
            Console.Write("Choose a starter Creature");

            option1.ShortPrint(0, 6);
            option2.ShortPrint(40, 6);
            option3.ShortPrint(80, 6);

            menu.SetPointer(0, 0);
            while (menu.OptionSelected == -1)
            {
                menu.GetInput();
            }

            if(menu.OptionSelected == 100)
            {
                playerCreatures[0] = option1;
                fightIndex = 0;
            }
            else if(menu.OptionSelected == 200)
            {
                playerCreatures[0] = option2;
                fightIndex = 2;
            }
            else if (menu.OptionSelected == 300)
            {
                playerCreatures[0] = option3;
                fightIndex = 1;
            }

            Game game = new Game(playerCreatures,fightIndex,CreatureTemplateList,moveList);
        }

        public Menu CreateStarterChoiceMenu()
        {
            Menu menu;
            string[,] display = new string[3, 1];
            int[,,] coOrds = new int[3, 1, 2];
            display[0, 0] = "Blaziken";
            display[1, 0] = "Feraligater";
            display[2, 0] = "Venusaur";
            coOrds[0, 0, 0] = 10;
            coOrds[0, 0, 1] = 3;
            coOrds[1, 0, 0] = 50;
            coOrds[1, 0, 1] = 3;
            coOrds[2, 0, 0] = 90;
            coOrds[2, 0, 1] = 3;

            menu = new Menu(display, coOrds);
            return menu;
        }

    }
}
