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
        private Unit[] PlayerUnits;
        private int numOfUnitsOwned = 0;
        private int currentFight;
        private Unit[] enemyTeam;
        UnitTemplateList unitTemplateList;
        MoveList moveList;
        ProgressionInfo progressionInfo;

        public Game(Unit[] playerUnits, int currentFight,UnitTemplateList unitTemplateList,MoveList moveList)
        {

            this.PlayerUnits = playerUnits;
            this.currentFight = currentFight;
            this.unitTemplateList = unitTemplateList;
            this.moveList = moveList;
            progressionInfo = new ProgressionInfo(unitTemplateList, moveList);
            numOfUnitsOwned = 1;

            GameLoop(false);

        }

        public Game(Unit[] PlayerUnits,int numOfUnitsOwned, int currentFight,UnitTemplateList unitTemplateList, MoveList moveList)
        {
            this.PlayerUnits = PlayerUnits;
            this.numOfUnitsOwned = numOfUnitsOwned;
            this.currentFight = currentFight;
            this.unitTemplateList = unitTemplateList;
            this.moveList = moveList;
            progressionInfo = new ProgressionInfo(unitTemplateList, moveList);

            GameLoop(true);
        }

        public void GameLoop(bool encounterCollected)
        {

            if (encounterCollected == false)
            {
                GetNextUnit();
            }
            enemyTeam = progressionInfo.GetEnemyTeam(currentFight);
            Console.Clear();
            Prefight();
        }

        public void GetNextUnit()
        {
            TemplateUnit[] encounterPool = progressionInfo.GetEncounterPools(currentFight);
            int[] encounterChance = progressionInfo.GetEncounterPercentages(currentFight);

            Random randInt = new Random();
            int choice = randInt.Next(1, 101);
            int aboveCheck = 100;
            for(int i = 0;i < encounterPool.Length; i++)
            {
                aboveCheck -= encounterChance[i];
                if(choice > aboveCheck)
                {
                    PlayerUnits[numOfUnitsOwned] = new Unit(encounterPool[i],moveList);
                    i = encounterPool.Length;                    
                }
            }
            PlayerUnits[numOfUnitsOwned].ShortPrint(0, 0);
            numOfUnitsOwned += 1;
        }

        public void SrFind(string toFind, StreamReader sr)
        {
            while(sr.ReadLine() != toFind) { };
        }

        public void Prefight()
        {
            Menu menu;
            PlayerUnits[2] = new Unit(unitTemplateList.Templates[3], moveList);
            PlayerUnits[3] = new Unit(unitTemplateList.Templates[4], moveList);
            PlayerUnits[4] = new Unit(unitTemplateList.Templates[5], moveList);
            PlayerUnits[5] = new Unit(unitTemplateList.Templates[6], moveList);
            PlayerUnits[6] = new Unit(unitTemplateList.Templates[6], moveList);
            PlayerUnits[7] = new Unit(unitTemplateList.Templates[55], moveList);
            PlayerUnits[8] = new Unit(unitTemplateList.Templates[49], moveList);
            PlayerUnits[9] = new Unit(unitTemplateList.Templates[60], moveList);
            PlayerUnits[10] = new Unit(unitTemplateList.Templates[66], moveList);
            PlayerUnits[11] = new Unit(unitTemplateList.Templates[71], moveList);
            PlayerUnits[12] = new Unit(unitTemplateList.Templates[69], moveList);
            PlayerUnits[13] = new Unit(unitTemplateList.Templates[65], moveList);
            //PlayerUnits[6].IsAlive = false;

            PrintTeam(1, 2, "Player", PlayerUnits);
            PrintTeam(61, 2, "Enemy", enemyTeam);

            menu = CreatePrefightMenu();
            menu.Draw();
            menu.SetPointer(0, 0);
            while (menu.OptionSelected != 500)
            {
                menu.GetInput();
                switch (menu.OptionSelected)
                {
                    case 100: ChangeTeam();
                        Console.Clear();
                        PrintTeam(1, 2, "Player", PlayerUnits);
                        PrintTeam(61, 2, "Enemy", enemyTeam);
                        menu.Draw();
                        menu.Highlight();
                        break;

                    case 200: Inspect(false) ;
                        Console.Clear();
                        PrintTeam(1, 2, "Player", PlayerUnits);
                        PrintTeam(61, 2, "Enemy", enemyTeam);
                        menu.Draw();
                        menu.Highlight();
                        break;

                    case 300: InspectMatchup();
                        Console.Clear();
                        PrintTeam(1, 2, "Player", PlayerUnits);
                        PrintTeam(61, 2, "Enemy", enemyTeam);
                        menu.Draw();
                        menu.Highlight();
                        break;
                    case 500:
                        Combat combat = new Combat(PlayerUnits,enemyTeam,this);
                        break;
                }
                menu.OptionSelected = -1;
            }

        }

        public void PrintTeam(int X, int Y,string ownerName,Unit[] team)
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
            Console.Write("Player Units:");

            Console.SetCursorPosition(15, yCoOrdUsed);
            Console.Write("Enemy Units:");
 
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
                        PlayerUnits[menu.PointerX + (menu.PointerY) * 3].InspectPrint(70, 3);
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

            Unit temporaryStorage;
            int xPointer;
            int yPointer;
            int swapUnitIndex1;
            int swapUnitIndex2;
            bool fixHighlight = false;

            Console.Clear();
            TeamChangeHeaders();

            menu = CreateTeamSwapMenu();

            //creates sub menu from first 6 members of playerUnits
            int Y = 5;
            string[,] display = new string[4, 2];
            int[,,] coOrds = new int[4, 2, 2];
            MenuCoOrds3Wide(coOrds, ref Y, 15, 0, 6);
            FillDisplayFromUnitArray(PlayerUnits, 0, 6, display, 0);
            
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
                if (subMenu.PointerX != 4)
                {
                    PlayerUnits[subMenu.PointerX + (subMenu.PointerY) * 3].InspectPrint(70, 3);
                }
                subMenu.GetInput();

                if(subMenu.OptionSelected != 400 && subMenu.OptionSelected != -1)
                {
                    
                    xPointer = subMenu.PointerX;
                    yPointer = subMenu.PointerY;

                    //highlights team member to swap in red, this is to distinguish it from the cursor
                    Highlight(display[xPointer, yPointer], coOrds[xPointer, yPointer, 0], coOrds[xPointer, yPointer, 1]);

                    //stores unit to swap
                    swapUnitIndex1 = ((yPointer) * 3) + xPointer;
                    temporaryStorage = PlayerUnits[swapUnitIndex1];

                    ClearScreenArea(70, 3, 37, 25);
                    Console.SetCursorPosition(15, 1);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Select the unit you want to swap it with              ");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    menu.SetPointer(3, 0);
                    while(menu.OptionSelected == -1)
                    {
                        //prints unit info of unit at pointer
                        if (menu.PointerX != 4)
                        {
                            PlayerUnits[menu.PointerX + (menu.PointerY) * 3].InspectPrint(70, 3);
                        }

                        //checks if cursor is on unit to swap
                        if (menu.PointerX == xPointer && menu.PointerY == yPointer)
                        {
                            fixHighlight = true;
                        }

                        menu.GetInput();

                        if(menu.OptionSelected != -1 && !PlayerUnits[menu.PointerX + (menu.PointerY) * 3].IsAlive)
                        {
                            menu.OptionSelected = -1;
                        }

                        //re-highlights unit to swap when cursor is move off of it
                        if(menu.OptionSelected == -1 && fixHighlight)
                        {
                            Highlight(display[xPointer, yPointer], coOrds[xPointer, yPointer, 0], coOrds[xPointer, yPointer, 1]);
                            fixHighlight = false;
                        }

                        ClearScreenArea(70, 3, 37, 25);

                    }

                    if(menu.OptionSelected != 400)
                    {
                        //takes the index of the 2nd unit to swap the swaps their positions in the playerUnit array
                        swapUnitIndex2 = (menu.PointerY * 3) + menu.PointerX;
                        PlayerUnits[swapUnitIndex1] = PlayerUnits[swapUnitIndex2];
                        PlayerUnits[swapUnitIndex2] = temporaryStorage;

                        //swaps the display strings to match with the swapped units and then updates the subMenu to match
                        menu.Display[xPointer, yPointer] = menu.Display[menu.PointerX, menu.PointerY];
                        menu.Display[menu.PointerX, menu.PointerY] = subMenu.Display[xPointer, yPointer];
                        subMenu.Display[xPointer, yPointer] = menu.Display[xPointer, yPointer];
                        if(swapUnitIndex2 <= 5)
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

            PlayerUnits[teamMenu.PointerX + teamMenu.PointerY * 3].MatchupInspectPrint(0, 0, enemyTeam[enemyMenu.PointerX + enemyMenu.PointerY * 3]);
            enemyTeam[enemyMenu.PointerX + enemyMenu.PointerY * 3].MatchupInspectPrint(60, 0, PlayerUnits[teamMenu.PointerX + teamMenu.PointerY * 3]);

            Highlight(enemyMenu.Display[enemyMenu.PointerX,enemyMenu.PointerY],enemyMenu.CoOrds[enemyMenu.PointerX,enemyMenu.PointerY,0],enemyMenu.CoOrds[enemyMenu.PointerX,enemyMenu.PointerY,1]);

            while(teamMenu.OptionSelected != 400)
            {
                teamMenu.GetInput();
                //menu.pointerX+menu.pointerY*3 turns position in grid into pos in 1D array
                if(teamMenu.PointerX != 3)
                {
                    ClearScreenArea(0, 0, 100, 23);
                    PlayerUnits[teamMenu.PointerX+teamMenu.PointerY*3].MatchupInspectPrint(0,0,enemyTeam[enemyMenu.PointerX+enemyMenu.PointerY*3]);
                    enemyTeam[enemyMenu.PointerX+enemyMenu.PointerY*3].MatchupInspectPrint(60,0,PlayerUnits[teamMenu.PointerX+teamMenu.PointerY*3]);

                    if( teamMenu.OptionSelected != -1)
                    {
                        Highlight(teamMenu.Display[teamMenu.PointerX,teamMenu.PointerY],teamMenu.CoOrds[teamMenu.PointerX,teamMenu.PointerY,0],teamMenu.CoOrds[teamMenu.PointerX,teamMenu.PointerY,1]);
                        enemyMenu.Highlight();

                        while (enemyMenu.OptionSelected != 400)
                        {

                            enemyMenu.GetInput();
                            if(enemyMenu.PointerX != 3)
                            {
                                ClearScreenArea(0, 0, 100, 23);
                                PlayerUnits[teamMenu.PointerX+teamMenu.PointerY*3].MatchupInspectPrint(0,0,enemyTeam[enemyMenu.PointerX+enemyMenu.PointerY*3]);
                                enemyTeam[enemyMenu.PointerX+enemyMenu.PointerY*3].MatchupInspectPrint(60,0,PlayerUnits[teamMenu.PointerX+teamMenu.PointerY*3]);

                            }
                        }

                        teamMenu.Highlight();
                        enemyMenu.Deselect();
                        enemyMenu.SetPointerNoHighlight(0,0);
                        Highlight(enemyMenu.Display[enemyMenu.PointerX,enemyMenu.PointerY],enemyMenu.CoOrds[enemyMenu.PointerX,enemyMenu.PointerY,0],enemyMenu.CoOrds[enemyMenu.PointerX,enemyMenu.PointerY,1]);
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
            display[1, 0] = "INSPECT UNIT";
            display[2, 0] = "INSPECT MATCHUP";
            display[3, 0] = "SAVE";
            display[4, 0] = "START FIGHT";
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
            FillDisplayFromUnitArray(PlayerUnits,0,18,display,0);
            FillDisplayFromUnitArray(enemyTeam,0,6,display,18); 
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
            FillDisplayFromUnitArray(PlayerUnits, 0, 6, display, 0);
            FillDisplayFromUnitArray(enemyTeam, 0, 6, display, 6);
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

            FillDisplayFromUnitArray(PlayerUnits,0,18,display,0);
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

            FillDisplayFromUnitArray(PlayerUnits, 0, 6, teamDisplay, 0);
            FillDisplayFromUnitArray(enemyTeam, 0, 6, enemyDisplay, 0);
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

        public void FillDisplayFromUnitArray(Unit[] arrayUsed,int startPoint, int endPoint, string[,] display, int iDisplacement )
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
            Console.Write("Other Available Units:");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void Highlight(string displayString, int xCoOrd, int yCoOrd)
        {
            Console.SetCursorPosition(xCoOrd,yCoOrd);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(displayString);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
