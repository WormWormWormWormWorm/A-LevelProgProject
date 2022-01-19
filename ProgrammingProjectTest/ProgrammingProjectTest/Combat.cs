using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingProjectTest
{
    class Combat
    {
        private Unit[] playerTeam;
        private Unit[] enemyTeam;
        private Game game;
        private Menu fightMenu;

        public Combat(Unit[] playerTeam, Unit[] enemyTeam, Game game)
        {
            this.playerTeam = playerTeam;
            this.enemyTeam = enemyTeam;
            this.game = game;

            bool enemyTeamAlive = true;
            bool playerTeamAlive = true;
            int playerAction = 0;

            fightMenu = FightMenu();

            setUpUI();

            while (enemyTeamAlive && playerTeamAlive)
            {
                while(playerAction == 0) //loops until user chooses a move or to swap unit
                {
                    fightMenu.GetInput(); //gets input

                    switch (fightMenu.OptionSelected)//changes from -1 when player presses enter on an option
                    {
                        case 100:
                            game.Inspect(true);
                            setUpUI();//clears Inspect UI and re-draws fight menu UI
                            break;
                        case 200:
                            game.InspectMatchup();
                            setUpUI();//clears InspectMatchup UI and re-draws fight menu UI
                            break;
                        case 300:
                            playerAction = SwapUnit();//allows player to swap unit in play ending their turn
                            setUpUI();//clears SwapUnit UI and draws updated fight menu UI
                            break;
                        case 400:
                            playerAction = ChooseMove();//allows player to choose a move ending their turn
                            Console.SetCursorPosition(0, 26);
                            Console.Write("                                                                                "); //clears move menu
                            break;
                    }
                    fightMenu.OptionSelected = -1;
                }

                Console.SetCursorPosition(0, 26);
                Console.Write(playerAction);
                playerAction = 0;
            }
            
        }

        public int SwapUnit()
        {
            int slotSelected;
            int playerAction = 0;
            Unit placeholder;

            Menu swapUnitMenu = CreateSwapUnitMenu();
            //sets up UI
            Console.Clear();
            Console.SetCursorPosition(5, 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("SELECT WHICH UNIT YOU WOULD LIKE TO SWAP INTO THE FIELD");
            Console.ForegroundColor = ConsoleColor.Gray;

            swapUnitMenu.Draw();
            swapUnitMenu.SetPointer(1, 0);

            slotSelected = swapUnitMenu.PointerX + swapUnitMenu.PointerY * 3;
            CombatPrint(62, playerTeam[slotSelected], enemyTeam[0]);
            //
            while (swapUnitMenu.OptionSelected == -1)//loops until option is selected
            {
                slotSelected = swapUnitMenu.PointerX + swapUnitMenu.PointerY * 3;
                swapUnitMenu.GetInput();
                
                if(swapUnitMenu.OptionSelected != 100)
                {
                    if (swapUnitMenu.OptionSelected != -1)
                    {
                        //swaps unit in first position(unit currently in the field) with the unit chosen by the player
                        placeholder = playerTeam[0];
                        playerTeam[0] = playerTeam[slotSelected];
                        playerTeam[slotSelected] = placeholder;

                        playerAction = 5;
                    }
                    else
                    {
                        //updates ui for new pointer position in menu
                        game.ClearScreenArea(62, 0, 40, 20);
                        slotSelected = swapUnitMenu.PointerX + swapUnitMenu.PointerY * 3;
                        CombatPrint(62, playerTeam[slotSelected], enemyTeam[0]);
                    }
                }

            }

            return playerAction;
        }

        public int ChooseMove()
        {

            Menu moveMenu = CreateMoveMenu();

            //draws menu
            moveMenu.Draw();
            moveMenu.Highlight();

            while (moveMenu.OptionSelected == -1) //loops until option selected
            {
                moveMenu.GetInput();
            }

            return moveMenu.PointerX; //this returns 0 when back is selected and 1-4 when moves selected
        }

        public Menu FightMenu()
        {
            //sets up FightMenu for option selection
            Menu fightMenu;

            string[,] display = new string[4, 1];
            int[,,] coOrds = new int[4, 1, 2];
            display[0, 0] = "INSPECT UNIT";
            display[1, 0] = "INSPECT MATCHUP";
            display[2, 0] = "SWAP UNIT";
            display[3, 0] = "ATTACK";
            coOrds[0, 0, 0] = 0;
            coOrds[1, 0, 0] = 20;
            coOrds[2, 0, 0] = 40;
            coOrds[3, 0, 0] = 60;

            for (int i = 0; i < 4; i++)
            {
                coOrds[i, 0, 1] = 24;
            }
            fightMenu = new Menu(display, coOrds);

            return fightMenu;
        }

        public Menu CreateMoveMenu()
        {
            //creates move menu for current player unit in use
            Menu moveMenu;
            string[,] display = new string[5, 1];
            int[,,] coOrds = new int[5, 1, 2];

            display[0, 0] = "BACK";
            coOrds[0, 0, 0] = 0;
            coOrds[0, 0, 1] = 26;

            for (int i = 1;i < 5; i++)
            {
                display[i,0] = playerTeam[0].Moves[i - 1].Name;
                coOrds[i, 0, 0] = i * 14;
                coOrds[i, 0, 1] = 26;
            }

            moveMenu = new Menu(display, coOrds);

            return moveMenu;
        }

        public void CombatPrint(int xStart, Unit unitUsed,Unit target)
        {
            //prints relevant info about unit and its moves to console
            Console.SetCursorPosition(xStart, 0);
            Console.Write(unitUsed.Name);
            Console.ForegroundColor = unitUsed.Type1.DisplayColor;
            Console.Write(" " + unitUsed.Type1.Name);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("/");
            Console.ForegroundColor = unitUsed.Type2.DisplayColor;
            Console.Write(unitUsed.Type2.Name);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(xStart, 1);
            Console.Write("status!");//fix
            Console.SetCursorPosition(xStart, 3);
            Console.Write("HP: " + unitUsed.CurrentHp);
            Console.SetCursorPosition(xStart, 4);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("=====================");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(xStart, 5);
            Console.Write("Speed: " + unitUsed.Stats[4]);
            Console.SetCursorPosition(xStart, 7);
            Console.Write("Moves:");
            for(int i = 0;i < 4; i++)
            {
                unitUsed.Moves[i].CombatPrint(xStart,9+i*3,unitUsed,target);
            }
            Console.SetCursorPosition(xStart, 21);
            Console.Write("Ability: {0}",unitUsed.Ability);
        }

        public void setUpUI()
        {
            //draws UI
            Console.Clear();
            CombatPrint(0, playerTeam[0], enemyTeam[0]);
            CombatPrint(60, enemyTeam[0], playerTeam[0]);
            Console.SetCursorPosition(0, 22);
            Console.Write("========================================================================================================================");
            fightMenu.Draw();
            fightMenu.Highlight();
        }

        public void HealthBar()
        {
            //create if you have time
        }

        public Menu CreateSwapUnitMenu()
        {
            //sets up menu for SwapUnit
            Menu swapUnitMenu;
            int Y;
            string[,] display = new string[3, 2];
            int[,,] coOrds = new int[3, 2, 2];

            Y = 3;

            display[1, 0] = playerTeam[1].Name;
            display[2, 0] = playerTeam[2].Name;
            display[0, 1] = playerTeam[3].Name;
            display[1, 1] = playerTeam[4].Name;
            display[2, 1] = playerTeam[5].Name;
            display[0, 0] = "BACK";
            game.MenuCoOrds3Wide(coOrds,ref Y, 14, 0, 6);

            swapUnitMenu = new Menu(display, coOrds);

            return swapUnitMenu;
        }
    }
}
