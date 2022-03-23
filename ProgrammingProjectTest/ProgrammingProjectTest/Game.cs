using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProgrammingProjectTest
{
    class Game
    {
        private Creature[] PlayerCreatures;
        private int numOfCreaturesOwned = 0;
        private int currentFight;
        private Creature[] enemyTeam;
        CreatureTemplateList CreatureTemplateList;
        MoveList moveList;
        ProgressionInfo progressionInfo;

        public Game(Creature[] playerCreatures, int currentFight,CreatureTemplateList CreatureTemplateList,MoveList moveList)
        {

            this.PlayerCreatures = playerCreatures;
            this.currentFight = currentFight;
            this.CreatureTemplateList = CreatureTemplateList;
            this.moveList = moveList;
            progressionInfo = new ProgressionInfo(CreatureTemplateList, moveList);
            numOfCreaturesOwned = 1;

            GameLoop();

        }

        public void GameLoop()
        {
            while(currentFight < 18)
            {
                GetNextCreature();
                enemyTeam = progressionInfo.GetEnemyTeam(currentFight);
                Console.Clear();
                Prefight();

                if (currentFight < 3)
                {
                    currentFight = 3;
                }
                else
                {
                    currentFight++;
                }
                
            }

            Console.SetCursorPosition(60, 15);
            Console.WriteLine("You win!");

            

        }

        public void GetNextCreature()
        {
            TemplateCreature[] encounterPool = progressionInfo.GetEncounterPools(currentFight);
            int[] encounterChance = progressionInfo.GetEncounterPercentages(currentFight);

            Random randInt = new Random();
            int choice = randInt.Next(1, 101);
            int aboveCheck = 100;
            for(int i = 0;i < encounterPool.Length; i++)
            {
                aboveCheck -= encounterChance[i];
                if(choice > aboveCheck)
                {
                    PlayerCreatures[numOfCreaturesOwned] = new Creature(encounterPool[i],moveList);
                    i = encounterPool.Length;                    
                }
            }
            Console.Clear();
            Console.WriteLine("=================\n"
                + "NEW Creature ACQUIRED\n" +
                "=================");
            PlayerCreatures[numOfCreaturesOwned].ShortPrint(0, 4);

            Console.SetCursorPosition(0, 17);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Continue");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            numOfCreaturesOwned += 1;
        }

        public void Prefight()
        {
            Menu menu;
            bool fightIncomplete = true;

            PrintTeam(1, 2, "Player", PlayerCreatures);
            PrintTeam(61, 2, "Enemy", enemyTeam);

            menu = CreatePrefightMenu();
            menu.Draw();
            menu.SetPointer(0, 0);
            while (fightIncomplete)
            {
                menu.GetInput();
                switch (menu.OptionSelected)
                {
                    case 100: ChangeTeam();
                        Console.Clear();
                        PrintTeam(1, 2, "Player", PlayerCreatures);
                        PrintTeam(61, 2, "Enemy", enemyTeam);
                        menu.Draw();
                        menu.Highlight();
                        break;

                    case 200: Inspect(false) ;
                        Console.Clear();
                        PrintTeam(1, 2, "Player", PlayerCreatures);
                        PrintTeam(61, 2, "Enemy", enemyTeam);
                        menu.Draw();
                        menu.Highlight();
                        break;

                    case 300: InspectMatchup();
                        Console.Clear();
                        PrintTeam(1, 2, "Player", PlayerCreatures);
                        PrintTeam(61, 2, "Enemy", enemyTeam);
                        menu.Draw();
                        menu.Highlight();
                        break;
                    case 400:
                        Combat combat = new Combat(PlayerCreatures,enemyTeam,this);
                        fightIncomplete = false;
                        break;
                    case 500:
                        fightIncomplete = false;
                        break;
                }
                menu.OptionSelected = -1;
            }

        }

        public void PrintTeam(int X, int Y,string ownerName,Creature[] team)
        {
            Console.SetCursorPosition(X+15, Y);
            Console.Write(ownerName + "'s Team:");
            Console.SetCursorPosition(X, Y + 1);
            Console.Write("==========================================================");
            Y +=2;
            for(int i = 0;i < 6; i++)
            {
                if(team[i] != null)
                {
                    team[i].PrefightPrint(X, Y);
                }
                i++;
                if(team[i] != null)
                {
                    team[i].PrefightPrint(X+30, Y);
                }
                Y += 4;
            }
            Console.SetCursorPosition(X, Y-1);
            Console.Write("==========================================================");
        }

        public void Inspect(bool inFight)
        {
            int enemyTeamStart;
            int yCoOrdUsed;
            int backYCoOrd;
            Menu menu = new Menu();
            Console.Clear();

            if (inFight)
            {
                enemyTeamStart = 1;
                yCoOrdUsed = 7;
                backYCoOrd = 4;
                menu = CreateFightInspectMenu();
            }
            else
            {
                enemyTeamStart = 5;
                yCoOrdUsed = 15;
                backYCoOrd = 8;
                menu = CreatefullInspectMenu();
            }

            Console.SetCursorPosition(15, 1);
            Console.Write("Player Creatures:");

            Console.SetCursorPosition(15, yCoOrdUsed);
            Console.Write("Enemy Creatures:");
 
            menu.Draw();
            menu.SetPointer(0, 0);

            while(menu.OptionSelected != 100 + backYCoOrd) //goes until enter is pressed on location of BACK option
            {
                if(menu.PointerY != backYCoOrd) 
                {
                    if(menu.PointerY > enemyTeamStart)
                    {
                        enemyTeam[menu.PointerX + (menu.PointerY-(enemyTeamStart+1))*3].InspectPrint(70,3);
                    }
                    else
                    {
                        PlayerCreatures[menu.PointerX + (menu.PointerY) * 3].InspectPrint(70, 3);
                    }
                }
                menu.GetInput();
                ClearScreenArea(70, 3, 37, 25);
            }
        }

        public void ChangeTeam()
        {
            Menu menu;
            Menu subMenu;

            Creature temporaryStorage;
            int xPointer;
            int yPointer;
            int swapCreatureIndex1;
            int swapCreatureIndex2;
            bool fixHighlight = false;

            Console.Clear();
            TeamChangeHeaders();

            menu = CreateTeamSwapMenu();

            //creates sub menu from first 6 members of playerCreatures
            int Y = 5;
            string[,] display = new string[4, 2];
            int[,,] coOrds = new int[4, 2, 2];
            MenuCoOrds3Wide(coOrds, ref Y, 15, 0, 6);
            FillDisplayFromCreatureArray(PlayerCreatures, 0, 6, display, 0);
            
            display[3, 0] = "BACK";
            display[3, 1] = "-";
            coOrds[3, 0, 0] = 6;
            coOrds[3, 0, 1] = 5;
            subMenu = new Menu(display, coOrds);
            //

            menu.Draw();
            subMenu.SetPointer(0, 0);
            while(subMenu.OptionSelected != 400)
            {
                if (subMenu.PointerX != 3)
                {
                    PlayerCreatures[subMenu.PointerX + (subMenu.PointerY) * 3].InspectPrint(70, 3);
                }
                subMenu.GetInput();

                if(subMenu.OptionSelected != 400 && subMenu.OptionSelected != -1)
                {
                    
                    xPointer = subMenu.PointerX;
                    yPointer = subMenu.PointerY;

                    //highlights team member to swap in red, this is to distinguish it from the cursor
                    RedHighlight(display[xPointer, yPointer], coOrds[xPointer, yPointer, 0], coOrds[xPointer, yPointer, 1]);

                    //stores Creature to swap
                    swapCreatureIndex1 = ((yPointer) * 3) + xPointer;
                    temporaryStorage = PlayerCreatures[swapCreatureIndex1];

                    ClearScreenArea(70, 3, 37, 25);
                    Console.SetCursorPosition(15, 1);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Select the Creature you want to swap it with              ");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    menu.SetPointer(3, 0);
                    while(menu.OptionSelected == -1)
                    {
                        //prints Creature info of Creature at pointer
                        if (menu.PointerX != 3)
                        {
                            PlayerCreatures[menu.PointerX + (menu.PointerY) * 3].InspectPrint(70, 3);
                        }

                        //checks if cursor is on Creature to swap
                        if (menu.PointerX == xPointer && menu.PointerY == yPointer)
                        {
                            fixHighlight = true;
                        }

                        menu.GetInput();

                        //re-highlights Creature to swap when cursor is move off of it
                        if(menu.OptionSelected == -1 && fixHighlight)
                        {
                            RedHighlight(display[xPointer, yPointer], coOrds[xPointer, yPointer, 0], coOrds[xPointer, yPointer, 1]);
                            fixHighlight = false;
                        }

                        ClearScreenArea(70, 3, 37, 25);

                    }

                    if(menu.OptionSelected != 400)
                    {
                        //takes the index of the 2nd Creature to swap the swaps their positions in the playerCreature array
                        swapCreatureIndex2 = (menu.PointerY * 3) + menu.PointerX;
                        PlayerCreatures[swapCreatureIndex1] = PlayerCreatures[swapCreatureIndex2];
                        PlayerCreatures[swapCreatureIndex2] = temporaryStorage;

                        //swaps the display strings to match with the swapped Creatures and then updates the subMenu to match
                        menu.Display[xPointer, yPointer] = menu.Display[menu.PointerX, menu.PointerY];
                        menu.Display[menu.PointerX, menu.PointerY] = subMenu.Display[xPointer, yPointer];
                        subMenu.Display[xPointer, yPointer] = menu.Display[xPointer, yPointer];
                        if(swapCreatureIndex2 <= 5)
                        {
                            subMenu.Display[menu.PointerX, menu.PointerY] = menu.Display[menu.PointerX, menu.PointerY];
                        }
                    }

                    //draws updated GUI
                    Console.Clear();
                    TeamChangeHeaders();
                    menu.Draw();
                    subMenu.Highlight();


                    menu.OptionSelected = -1;
                    subMenu.OptionSelected = -1;
                }
                ClearScreenArea(70, 3, 37, 25);
            }

        }

        public void InspectMatchup()
        {
            Menu teamMenu = new Menu();
            Menu enemyMenu = new Menu();

            Console.Clear();

            CreateMatchupInspectMenu(ref teamMenu,ref enemyMenu);

            teamMenu.Draw();
            enemyMenu.Draw();

            teamMenu.SetPointer(0,0);

            PlayerCreatures[teamMenu.PointerX + teamMenu.PointerY * 3].MatchupInspectPrint(0, 0, enemyTeam[enemyMenu.PointerX + enemyMenu.PointerY * 3]);
            enemyTeam[enemyMenu.PointerX + enemyMenu.PointerY * 3].MatchupInspectPrint(60, 0, PlayerCreatures[teamMenu.PointerX + teamMenu.PointerY * 3]);

            RedHighlight(enemyMenu.Display[enemyMenu.PointerX,enemyMenu.PointerY],enemyMenu.CoOrds[enemyMenu.PointerX,enemyMenu.PointerY,0],enemyMenu.CoOrds[enemyMenu.PointerX,enemyMenu.PointerY,1]);

            while(teamMenu.OptionSelected != 400)
            {
                teamMenu.GetInput();
                //menu.pointerX+menu.pointerY*3 turns position in grid into pos in 1D array
                if(teamMenu.PointerX != 3)
                {
                    ClearScreenArea(0, 0, 100, 23);
                    PlayerCreatures[teamMenu.PointerX+teamMenu.PointerY*3].MatchupInspectPrint(0,0,enemyTeam[enemyMenu.PointerX+enemyMenu.PointerY*3]);
                    enemyTeam[enemyMenu.PointerX+enemyMenu.PointerY*3].MatchupInspectPrint(60,0,PlayerCreatures[teamMenu.PointerX+teamMenu.PointerY*3]);

                    if( teamMenu.OptionSelected != -1)
                    {
                        RedHighlight(teamMenu.Display[teamMenu.PointerX,teamMenu.PointerY],teamMenu.CoOrds[teamMenu.PointerX,teamMenu.PointerY,0],teamMenu.CoOrds[teamMenu.PointerX,teamMenu.PointerY,1]);
                        enemyMenu.Highlight();

                        while (enemyMenu.OptionSelected != 400)
                        {

                            enemyMenu.GetInput();
                            if(enemyMenu.PointerX != 3)
                            {
                                ClearScreenArea(0, 0, 100, 23);
                                PlayerCreatures[teamMenu.PointerX+teamMenu.PointerY*3].MatchupInspectPrint(0,0,enemyTeam[enemyMenu.PointerX+enemyMenu.PointerY*3]);
                                enemyTeam[enemyMenu.PointerX+enemyMenu.PointerY*3].MatchupInspectPrint(60,0,PlayerCreatures[teamMenu.PointerX+teamMenu.PointerY*3]);

                            }
                        }

                        teamMenu.Highlight();
                        enemyMenu.Deselect();
                        enemyMenu.SetPointerNoHighlight(0,0);
                        RedHighlight(enemyMenu.Display[enemyMenu.PointerX,enemyMenu.PointerY],enemyMenu.CoOrds[enemyMenu.PointerX,enemyMenu.PointerY,0],enemyMenu.CoOrds[enemyMenu.PointerX,enemyMenu.PointerY,1]);
                        teamMenu.OptionSelected = -1;
                        enemyMenu.OptionSelected = -1;

                    }
                }
                
                
                
            }

        }



        public void ClearScreenArea(int startX,int startY,int sizeX,int sizeY)
        {
            string spaceBlock = "";
            for (int k = 0; k < sizeX; k++)
            {
                spaceBlock += " ";
            }
            //this is a console.clear() limited to a certain area to fix harsh flashing visuals of refreshing entire screen repeatedly
            for (int i = 0;i < sizeY; i++)
            {
                Console.SetCursorPosition(startX, startY);
                Console.Write(spaceBlock);
                startY++;
            }

        }

        public Menu CreatePrefightMenu()
        {
            Menu menu;
            string[,] display = new string[5, 1];
            int[,,] coOrds = new int[5, 1, 2];
            display[0, 0] = "CHANGE TEAM";
            display[1, 0] = "INSPECT Creature";
            display[2, 0] = "INSPECT MATCHUP";
            display[3, 0] = "START FIGHT";
            display[4, 0] = "Skip";
            coOrds[0, 0, 0] = 0;
            coOrds[1, 0, 0] = 20;
            coOrds[2, 0, 0] = 40;
            coOrds[3, 0, 0] = 60;
            coOrds[4, 0, 0] = 75;

            for (int i = 0; i < 5; i++)
            {
                coOrds[i, 0, 1] = 17;
            }
            menu = new Menu(display, coOrds);
            return menu;
        }

        public Menu CreatefullInspectMenu()
        {
            Menu menu;

            string[,] display = new string[3,9];
            int[,,] coOrds = new int[3,9, 2];
            int Y;
            Y = 3;
            MenuCoOrds3Wide(coOrds, ref Y, 15,0,18);
            Y += 2;
            MenuCoOrds3Wide(coOrds, ref Y, 15,18,27 );
            FillDisplayFromCreatureArray(PlayerCreatures,0,18,display,0);
            FillDisplayFromCreatureArray(enemyTeam,0,6,display,18); 
            display[0,8] = "BACK";
            display[1, 8] = "-";
            display[2, 8] = "-";
            menu = new Menu(display, coOrds);

            return menu;
        }

        public Menu CreateFightInspectMenu()
        {
            Menu menu;

            string[,] display = new string[3, 5];
            int[,,] coOrds = new int[3, 5, 2];
            int Y;
            Y = 3;
            MenuCoOrds3Wide(coOrds, ref Y, 15, 0, 6);
            Y += 2;
            MenuCoOrds3Wide(coOrds, ref Y, 15, 6, 15);
            FillDisplayFromCreatureArray(PlayerCreatures, 0, 6, display, 0);
            FillDisplayFromCreatureArray(enemyTeam, 0, 6, display, 6);
            display[0, 4] = "BACK";
            display[1, 4] = "-";
            display[2, 4] = "-";
            menu = new Menu(display, coOrds);

            return menu;
        }

        public Menu CreateTeamSwapMenu()//should return menu
        {
            Menu menu;
            int Y = 5;

            string[,] display = new string[4,6];
            int[,,] coOrds = new int[4,6, 2];

            MenuCoOrds3Wide(coOrds,ref Y,15,0,6);
            Y += 4;
            MenuCoOrds3Wide(coOrds,ref Y,15,6,18);

            FillDisplayFromCreatureArray(PlayerCreatures,0,18,display,0);
            display[3, 0] = "BACK";
            coOrds[3, 0, 0] = 6;
            coOrds[3, 0, 1] = 5;
            for(int i = 1;i < coOrds.GetLength(1); i++)
            {
                display[3, i] = "-";
            }

            menu = new Menu(display,coOrds);
            return menu;
        }

        public void CreateMatchupInspectMenu(ref Menu teamMenu,ref Menu enemyMenu)
        {
            int y;

            int[,,] teamCoOrds = new int[4, 2, 2];
            string[,] teamDisplay = new string[4, 2];
            int[,,] enemyCoOrds = new int[4, 2, 2];
            string[,] enemyDisplay = new string[4, 2];

            y = 26;
            MenuCoOrds3Wide(teamCoOrds,ref y, 13, 0, 6);
            y = 26;
            MenuCoOrds3Wide(enemyCoOrds, ref y, 64, 0, 6);

            FillDisplayFromCreatureArray(PlayerCreatures, 0, 6, teamDisplay, 0);
            FillDisplayFromCreatureArray(enemyTeam, 0, 6, enemyDisplay, 0);
            teamDisplay[3, 0] = "BACK";
            teamCoOrds[3, 0, 0] = 55;
            teamCoOrds[3, 0, 1] = 26;
            enemyDisplay[3, 0] = "BACK";
            enemyCoOrds[3, 0, 0] = 55;
            enemyCoOrds[3, 0, 1] = 26;

            teamDisplay[3, 1] = "-";
            enemyDisplay[3, 1] = "-";

            teamMenu = new Menu(teamDisplay, teamCoOrds);
            enemyMenu = new Menu(enemyDisplay, enemyCoOrds);
        }

        public void MenuCoOrds3Wide(int[,,] coOrds,ref int Y,int startX,int start, int end)
        {

            for(int k = start;k < end; k++)
            {
                coOrds[0, k / 3, 0] = startX;
                coOrds[0, k / 3, 1] = Y;
                k++;
                coOrds[1, k / 3, 0] = startX + 14;
                coOrds[1, k / 3, 1] = Y;
                k++;
                coOrds[2, k / 3, 0] = startX + 28;
                coOrds[2, k / 3, 1] = Y;
                Y += 2;
            }
        }

        public void FillDisplayFromCreatureArray(Creature[] arrayUsed,int startPoint, int endPoint, string[,] display, int iDisplacement )
        {
            for(int i = startPoint; i < endPoint; i++)
            {
                if(arrayUsed[i] != null)
                {
                    display[0, (i+iDisplacement)/3] = arrayUsed[i].Name;
                }
                i++;
                if (arrayUsed[i] != null)
                {
                    display[1,(i+iDisplacement)/3] = arrayUsed[i].Name;
                }
                i++;
                if (arrayUsed[i] != null)
                {
                    display[2,(i+iDisplacement)/3] = arrayUsed[i].Name;
                }
            }
        }

        public void TeamChangeHeaders()
        {
            Console.SetCursorPosition(15, 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Select which member of the team you would like to swap");
            Console.SetCursorPosition(15, 3);
            Console.Write("Current Team:");
            Console.SetCursorPosition(15, 9);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Other Available Creatures:");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void RedHighlight(string displayString, int xCoOrd, int yCoOrd)
        {
            Console.SetCursorPosition(xCoOrd,yCoOrd);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(displayString);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        public int NumOfCreaturesOwned
        {
            get
            {
                return numOfCreaturesOwned;
            }
            set
            {
                numOfCreaturesOwned = value;
            }
        }

    }
}
