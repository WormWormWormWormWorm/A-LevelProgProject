﻿using System;
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
        private string currentFight;
        private Unit[] enemyTeam;

        public Game(Unit[] playerUnits, string currentFight)
        {

            this.PlayerUnits = playerUnits;
            this.currentFight = currentFight;
            numOfUnitsOwned = 1;

            GameLoop(false);

        }

        public Game(Unit[] PlayerUnits,int numOfUnitsOwned, string currentFight)
        {
            this.PlayerUnits = PlayerUnits;
            this.numOfUnitsOwned = numOfUnitsOwned;
            this.currentFight = currentFight;

            GameLoop(true);
        }

        public void GameLoop(bool encounterCollected)
        {
            string[] encounterPool;

            using (StreamReader sr = new StreamReader("CurrentProgression.txt"))
            {
                encounterPool = FindEncounterInfo(encounterCollected, sr);
                if(encounterCollected == false)
                {
                    GetNextUnit(encounterPool);
                }
                enemyTeam = GetEnemyTeam(sr);
                Console.Clear();
            }
            Prefight();
        }

        public string[] FindEncounterInfo(bool EncounterCollected, StreamReader sr)
        {

            //this searches CurrentProgression.txt for CurrentFight then and pulls the encounterPool size then does sr.readline for that many times entering the result into encounter pool if EncounterCollected false

            //EncounterCollected True is used to go straight to enemyTeam info without gathering an encounter pool in the case of a save being loaded and the encounter has already been collected
            string[] encounterPool;
            int encounterPoolSize;
            SrFind(currentFight, sr);
            encounterPoolSize = Convert.ToInt32(sr.ReadLine());
            encounterPool = new string[encounterPoolSize];
            if (EncounterCollected == false)
            {
                for (int i = 0; i < encounterPoolSize; i++)
                {
                    encounterPool[i] = sr.ReadLine();
                }
            }
            else
            {
                for (int i = 0; i < encounterPoolSize; i++)
                {
                    sr.ReadLine();
                }
            }
            return encounterPool;
        }

        public void GetNextUnit(string[] encounterPool)
        {
            Console.Clear();
            Random randInt = new Random();
            int choice = randInt.Next(1, 101);
            int aboveCheck = 100;
            int ChanceOfEncounter;
            for(int i = 0;i < encounterPool.Length; i++)
            {
                ChanceOfEncounter = Convert.ToInt32(encounterPool[i].Substring(0,2));
                aboveCheck -= ChanceOfEncounter;
                if(choice > aboveCheck)
                {
                    PlayerUnits[numOfUnitsOwned] = new Unit(encounterPool[i]);
                    i = encounterPool.Length;                    
                }
            }
            PlayerUnits[numOfUnitsOwned].ShortPrint(0, 0);
            numOfUnitsOwned += 1;
        }

        public Unit[] GetEnemyTeam(StreamReader sr)
        {
            int enemyIVS;
            string pulledInfoStore;
            Unit[] enemyTeam = new Unit[6];
            int unitsInTeam = Convert.ToInt32(sr.ReadLine());

            for (int i = 0; i < unitsInTeam; i++)
            {
                enemyIVS = Convert.ToInt32(sr.ReadLine());
                enemyTeam[i] = new Unit(sr.ReadLine(), enemyIVS);//this pulls enemy unitIDs from CurrentProgression.txt and fill the enemyTeam array with them

                for (int k = 0; k < 4; k++)//this determines if any moves need to be swapped from the default set of this unit
                {
                    pulledInfoStore = sr.ReadLine();
                    if (pulledInfoStore != "#")
                    {
                        enemyTeam[i].DetermineMoveCategory(pulledInfoStore, k);
                    }
                }
                pulledInfoStore = sr.ReadLine();
                if(pulledInfoStore != "#")//this determines whether ability needs to be replaced
                {
                    enemyTeam[i].Ability = pulledInfoStore;
                }
            }

            return enemyTeam;
        }

        public void SrFind(string toFind, StreamReader sr)
        {
            while(sr.ReadLine() != toFind) { };
        }

        public void Prefight()
        {
            Menu menu;
            PlayerUnits[2] = new Unit("##1017");
            PlayerUnits[3] = new Unit("##1017");
            PlayerUnits[4] = new Unit("##1017");
            PlayerUnits[5] = new Unit("###897");
            PrintTeam(1, 2, "Player", PlayerUnits);
            PrintTeam(61, 2, "Enemy", enemyTeam);

            menu = CreatePrefightMenu();
            menu.Draw();
            menu.SetPointer(0, 0);
            while (menu.OptionSelected != 104)
            {
                menu.GetInput();
                switch (menu.OptionSelected)
                {
                    case 100: Console.Clear(); break;
                    case 101: Inspect(true) ;
                        Console.Clear();
                        PrintTeam(1, 2, "Player", PlayerUnits);
                        PrintTeam(61, 2, "Enemy", enemyTeam);
                        menu.Draw();
                        menu.Highlight();
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
            Console.Write("----------------------------------------------------------");
            Y+=2;
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
            Console.Write("----------------------------------------------------------");
        }

        public void Inspect(bool inFight)
        {
            Menu menu = CreatefullInspectMenu();
            Console.Clear();
            menu.Draw();
            menu.SetPointer(0, 0);

            while(menu.OptionSelected != 108)
            {
                if(menu.PointerY != 8)
                {
                    if(menu.PointerY > 5)
                    {
                        enemyTeam[menu.PointerX + (menu.PointerY-6)*3].InspectPrint(70,3);
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
            Unit[] arrayUsed;

            string[,] display = new string[3,9];
            int[,,] coOrds = new int[3,9, 2];
            int Y = 3;
            Console.SetCursorPosition(15, Y);
            Console.Write("Player Units:");
            arrayUsed = PlayerUnits;
            MenuCoOrds3x3(coOrds, ref Y, 15,0,18);
            Console.SetCursorPosition(15, Y);
            Console.Write("Enemy Units:");
            Y += 2;
            MenuCoOrds3x3(coOrds, ref Y, 15,18,27 );
            for(int i = 0; i < 18; i++)
            {
                if(arrayUsed[i] != null)
                {
                    display[0, i/3] = arrayUsed[i].Name;
                }
                i++;
                if (arrayUsed[i] != null)
                {
                    display[1,i/3] = arrayUsed[i].Name;
                }
                i++;
                if (arrayUsed[i] != null)
                {
                    display[2,i/3] = arrayUsed[i].Name;
                }
                Y += 2;
            }
            arrayUsed = enemyTeam;
            for(int i = 0;i < 6; i ++)
            {
                if (arrayUsed[i] != null)
                {
                    display[0,(i + 18) / 3] = arrayUsed[i].Name;
                }
                i++;
                if (arrayUsed[i] != null)
                {
                    display[1,(i+18) / 3] = arrayUsed[i].Name;
                }
                i++;
                if (arrayUsed[i] != null)
                {
                    display[2,(i+18) / 3] = arrayUsed[i].Name;
                }
            } 
            display[0,8] = "BACK";
            menu = new Menu(display, coOrds);

            return menu;
        }

        public void MenuCoOrds3x3(int[,,] coOrds,ref int Y,int startX,int start, int end)
        {

            for (int k = start; k < end; k++)
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

    }
}
