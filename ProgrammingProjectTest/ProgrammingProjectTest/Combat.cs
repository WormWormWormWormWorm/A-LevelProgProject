using System;

namespace ProgrammingProjectTest
{
    class Combat
    {
        private Creature[] playerTeam;
        private Creature[] enemyTeam;
        private int playerTeamRemaining;
        private int enemyCreaturesRemaining;
        private Game game;
        private Menu fightMenu;

        public bool endOfTurnProceedNessecary;

        public Combat(Creature[] playerTeam, Creature[] enemyTeam, Game game)
        {
            this.playerTeam = playerTeam;
            this.enemyTeam = enemyTeam;
            this.game = game;

            int numOfCreaturesInPlayerTeam;
            for(int i = 0;i < 6; i++)
            {
                if(playerTeam[i] != null)
                {
                    playerTeamRemaining++;
                }

                if(enemyTeam[i] != null)
                {
                    enemyCreaturesRemaining++;
                }
                
            }

            numOfCreaturesInPlayerTeam = playerTeamRemaining;

            int playerAction = 0;
            int enemyBestMoveIndex;

            EnemyDescisionMaker descisionMaker = new EnemyDescisionMaker(enemyTeam);

            bool playerGoesFirst;
            Random random = new Random();

            fightMenu = FightMenu();

            setUpUI();

            if(CheckIntimidate(playerTeam[0],enemyTeam[0]) || CheckIntimidate(enemyTeam[0], playerTeam[0]))
            {
                Proceed();
            }

            fightMenu.Draw();
            fightMenu.Highlight();

            while (enemyCreaturesRemaining != 0 && playerTeamRemaining != 0)
            {
                enemyBestMoveIndex = descisionMaker.GetAction(playerTeam[0]);
                endOfTurnProceedNessecary = false;

                while(playerAction == 0) //loops until user chooses a move or to swap Creature
                {
                    fightMenu.GetInput(); //gets input

                    switch (fightMenu.OptionSelected)//changes from -1 when player presses enter on an option
                    {
                        case 100:
                            game.Inspect(true);
                            setUpUI();//clears Inspect UI and re-draws fight menu UI
                            fightMenu.Draw();
                            fightMenu.Highlight();
                            break;
                        case 200:
                            game.InspectMatchup();
                            setUpUI();//clears InspectMatchup UI and re-draws fight menu UI
                            fightMenu.Draw();
                            fightMenu.Highlight();
                            break;
                        case 300:
                            playerAction = SwapCreature(false);//allows player to swap Creature in play ending their turn
                            setUpUI();//clears SwapCreature UI and draws updated fight menu UI
                            fightMenu.Draw();
                            fightMenu.Highlight();
                            break;
                        case 400:
                            playerAction = ChooseMove();//allows player to choose a move ending their turn
                            Console.SetCursorPosition(0, 26);
                            Console.Write("                                                                                "); //clears move menu
                            break;
                    }
                    fightMenu.OptionSelected = -1;
                }

                game.ClearScreenArea(0, 21, 80, 9);
                Console.SetCursorPosition(0, 21);

                if(playerAction != 5)
                {
                    //figures out which side moves first, move with higher priority level goes first, if tied, Creature with higher speed goes first, if tied again, pick randomly

                    if(enemyTeam[0].Moves[enemyBestMoveIndex].PriorityLevel == playerTeam[0].Moves[playerAction - 1].PriorityLevel)//tie is by far most common so checked first
                    {
                        if(enemyTeam[0].CurrentStat(4) < playerTeam[0].CurrentStat(4))
                        {
                            playerGoesFirst = true;
                        }
                        else if(enemyTeam[0].CurrentStat(4) > playerTeam[0].CurrentStat(4))
                        {
                            playerGoesFirst = false;
                        }
                        else
                        {
                            if(random.Next(0,2) == 2)
                            {
                                playerGoesFirst = true;
                            }
                            else
                            {
                                playerGoesFirst = false;
                            }
                        }
                    }
                    else if(enemyTeam[0].Moves[enemyBestMoveIndex].PriorityLevel < playerTeam[0].Moves[playerAction - 1].PriorityLevel)
                    {
                        playerGoesFirst = true;
                    }
                    else
                    {
                        playerGoesFirst = false;
                    }

                    if (playerGoesFirst)
                    {
                        playerTeam[0].Moves[playerAction - 1].Use(enemyTeam[0], playerTeam[0]);

                        Proceed();

                        if (enemyTeam[0].IsAlive)
                        {
                            enemyTeam[0].Moves[enemyBestMoveIndex].Use(playerTeam[0], enemyTeam[0]);
                            Proceed();
                        }
                    }
                    else
                    {
                        enemyTeam[0].Moves[enemyBestMoveIndex].Use(playerTeam[0], enemyTeam[0]);

                        Proceed();

                        if (playerTeam[0].IsAlive)
                        {
                            playerTeam[0].Moves[playerAction - 1].Use(enemyTeam[0], playerTeam[0]);
                            Proceed();
                        }
                    }
                }
                else
                {
                    if (CheckIntimidate(playerTeam[0], enemyTeam[0]))
                    {
                        Proceed();
                    }
                    

                    enemyTeam[0].NextMoveRandomMultiplier = 0;
                    enemyTeam[0].Moves[enemyBestMoveIndex].Use(playerTeam[0], enemyTeam[0]);
                    Proceed();
                }

                if (playerTeam[0].IsAlive)
                {
                   EndOfTurnProceedNessecary = playerTeam[0].EndOfTurnUpdate(enemyTeam[0]);
                }
                if (enemyTeam[0].IsAlive)
                {
                    EndOfTurnProceedNessecary = enemyTeam[0].EndOfTurnUpdate(playerTeam[0]);
                }
                

                if (enemyTeam[0].IsAlive == false)
                {
                    enemyCreaturesRemaining -= 1;
                    if (enemyCreaturesRemaining > 0)
                    {
                        EnemySwitch(playerTeam[0]);
                        Console.WriteLine("Enemy has switched in " + enemyTeam[0].Name);
                        
                        CheckIntimidate(enemyTeam[0],playerTeam[0]);

                        endOfTurnProceedNessecary = true;
                    }
                }
                if(endOfTurnProceedNessecary == true)
                {
                    Proceed();
                }


                if (playerTeam[0].IsAlive == false)
                {
                    playerTeamRemaining -= 1;
                    if(playerTeamRemaining > 0)
                    {
                        SwapCreature(true);
                    }
                    setUpUI();
                    if (CheckIntimidate(playerTeam[0], enemyTeam[0]))
                    {
                        Proceed();
                    }  
                    
                }
                else
                {
                    setUpUI();
                }
                
             

                fightMenu.Draw();
                fightMenu.Highlight();
                playerAction = 0;

            }

            Console.Clear();

            if(playerTeamRemaining == 0)
            {
                Console.SetCursorPosition(60, 15);
                Console.Write("Game Over");

                Console.SetCursorPosition(50,17);
                Console.Write("your save file has been deleted");

                while (true)
                {

                }
            }
            else if(playerTeamRemaining != numOfCreaturesInPlayerTeam)
            {
                int CreaturesLost = numOfCreaturesInPlayerTeam - playerTeamRemaining;
                for(int i = numOfCreaturesInPlayerTeam;i < game.NumOfCreaturesOwned; i++)
                {
                    playerTeam[i - CreaturesLost] = playerTeam[i];
                }

                game.NumOfCreaturesOwned -= CreaturesLost;
            }

            for(int i = 0;i < playerTeamRemaining;i++)
            {
                playerTeam[i].ClearModifiers();
                playerTeam[i].resetHealth();
                playerTeam[i].ClearStatus();
            }
            
        }

        public int SwapCreature(bool currentCreatureDead)
        {
            int slotSelected;
            int playerAction = 0;
            int slotSelectedOffset = 0;

            Menu swapCreatureMenu = CreateSwapCreatureMenu(currentCreatureDead);
            //sets up UI
            Console.Clear();
            Console.SetCursorPosition(5, 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("SELECT WHICH Creature YOU WOULD LIKE TO SWAP INTO THE FIELD");
            Console.ForegroundColor = ConsoleColor.Gray;

            swapCreatureMenu.Draw();
            if (currentCreatureDead)
            {
                swapCreatureMenu.SetPointer(0, 0);
                slotSelectedOffset = 1;
            }
            else
            {
                swapCreatureMenu.SetPointer(1, 0);
            }

            slotSelected = (swapCreatureMenu.PointerX + swapCreatureMenu.PointerY * 3) + slotSelectedOffset;
            
            CombatPrint(62, playerTeam[slotSelected], enemyTeam[0]);
            //
            while (swapCreatureMenu.OptionSelected == -1)//loops until option is selected
            {
                slotSelected = (swapCreatureMenu.PointerX + swapCreatureMenu.PointerY * 3) + slotSelectedOffset;
                swapCreatureMenu.GetInput();
                
                if(swapCreatureMenu.OptionSelected != 100 || currentCreatureDead)
                {
                    if (swapCreatureMenu.OptionSelected != -1)
                    {
                        //swaps Creature in first position(Creature currently in the field) with the Creature chosen by the player
                        SwapCreatureIn(slotSelected, playerTeam, playerTeamRemaining, enemyTeam[0]);

                        playerAction = 5;
                    }
                    else
                    {
                        //updates combatPrint UI for new pointer position in menu
                        game.ClearScreenArea(62, 0, 40, 20);
                        slotSelected = (swapCreatureMenu.PointerX + swapCreatureMenu.PointerY * 3) + slotSelectedOffset;
                        CombatPrint(62, playerTeam[slotSelected], enemyTeam[0]);
                    }
                }

            }

            return playerAction;
        }

        public void SwapCreatureIn(int chosenCreatureIndex,Creature[] team,int CreaturesRemainingInTeam,Creature opponent)
        {
            if (team[0].IsAlive)
            {
                if (team[0].Status.RemovedAtEndOfTurn)
                {
                    team[0].ClearStatus();
                }
                team[0].ClearModifiers();
                team[0].TurnsOnField = 0;

                Creature placeholder = team[0];
                team[0] = playerTeam[chosenCreatureIndex];
                team[chosenCreatureIndex] = placeholder;
            }
            else
            {
                team[0] = team[chosenCreatureIndex];
                team[chosenCreatureIndex] = team[CreaturesRemainingInTeam];
                team[CreaturesRemainingInTeam] = null;
            }

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
            display[0, 0] = "INSPECT Creature";
            display[1, 0] = "INSPECT MATCHUP";
            display[2, 0] = "SWAP Creature";
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
            //creates move menu for current player Creature in use
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

        public void CombatPrint(int xStart, Creature CreatureUsed,Creature target)
        {
            //prints relevant info about Creature and its moves to console
            Console.SetCursorPosition(xStart, 0);
            Console.Write(CreatureUsed.Name);
            Console.ForegroundColor = CreatureUsed.Type1.DisplayColor;
            Console.Write(" " + CreatureUsed.Type1.Name);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("/");
            Console.ForegroundColor = CreatureUsed.Type2.DisplayColor;
            Console.Write(CreatureUsed.Type2.Name);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(xStart, 1);
            CreatureUsed.Status.PrintStatus();
            Console.SetCursorPosition(xStart, 3);
            Console.Write("HP: " + CreatureUsed.CurrentHp);
            Console.SetCursorPosition(xStart, 4);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("=====================");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(xStart, 5);
            Console.Write("Speed: " + CreatureUsed.CurrentStat(4));
            for(int i = 0;i < 4; i++)
            {
                CreatureUsed.Moves[i].CombatPrint(xStart,7+i*3,CreatureUsed,target);
            }
            Console.SetCursorPosition(xStart, 19);
            Console.Write("Ability: {0}",CreatureUsed.Ability);
        }

        public void setUpUI()
        {
            //draws UI
            Console.Clear();
            CombatPrint(0, playerTeam[0], enemyTeam[0]);
            CombatPrint(60, enemyTeam[0], playerTeam[0]);
            Console.SetCursorPosition(0, 20);
            Console.Write("========================================================================================================================");
            
        }

        public bool CheckIntimidate(Creature user,Creature target)
        {
            if(user.Ability == "Intimidate")
            {
                Console.Write("Due to " +  user.Name + "'s Intimidate, ");
                target.Status = new Status("10Atk Drop");
                return true;
            }
            return false;
        }

        public void Proceed()
        {
            //stops until enter is pressed so user can read damage Dealt

            Console.SetCursorPosition(0, 29);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Continue");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;

            while(Console.ReadKey(true).Key != ConsoleKey.Enter) { }

            game.ClearScreenArea(0, 21, 80, 9);
            Console.SetCursorPosition(0, 21);
        }

        public void HealthBar()
        {
            //create if you have time
        }

        public Menu CreateSwapCreatureMenu(bool currentCreatureDead)
        {
            //sets up menu for SwapCreature
            Menu swapCreatureMenu;
            int Y;
            int iOffset;
            string[,] display = new string[3, 2];
            int[,,] coOrds = new int[3, 2, 2];

            if (currentCreatureDead)
            {
                iOffset = -1;
            }
            else
            {
                display[0, 0] = "Back";
                iOffset = 0;
            }

            Y = 3;
            
            for(int i = 1;i < 6; i++)
            {
                if(playerTeam[i] != null && playerTeam[i].IsAlive)
                {
                    display[(i+iOffset) % 3, (i+iOffset) / 3] = playerTeam[i].Name;
                }
            }
            

            game.MenuCoOrds3Wide(coOrds,ref Y, 14, 0, 6);

            swapCreatureMenu = new Menu(display, coOrds);

            return swapCreatureMenu;
        }

        public void EnemySwitch(Creature opponent)
        {
            int bestSwitchIndex = 1;
            int bestSwitchWeight = enemyTeam[1].CalculateSwitchWeight(opponent);

            int comparisonSwitchWeight;

            for (int i = 1; i > 6; i++)
            {
                comparisonSwitchWeight = enemyTeam[i].CalculateSwitchWeight(opponent);

                if (comparisonSwitchWeight > bestSwitchWeight)
                {
                    bestSwitchIndex = i;
                    bestSwitchWeight = comparisonSwitchWeight;
                }
            }

            SwapCreatureIn(bestSwitchIndex,enemyTeam,enemyCreaturesRemaining,playerTeam[0]);
        }

        public bool EndOfTurnProceedNessecary
        {
            set
            {
                if(value == true)
                {
                    endOfTurnProceedNessecary = true;
                }
            }
        }

    }

    class EnemyDescisionMaker
    {
        Creature[] enemyTeam;
        

        public EnemyDescisionMaker(Creature[] enemyTeam)
        {
            this.enemyTeam = enemyTeam;
        }

        public int GetAction(Creature target)
        {
            int bestMoveIndex;
            int bestMoveDamage;
            int comparisonMoveDamage;

            int statusBucketIndex = -1;

            int tieOdds = 1;
            int tieBounds = 2;

            double[] moveRandomMultipliers = new double[4];
            int[] moveWeights = new int[4];

            Random random;

            bestMoveIndex = 0;
            bestMoveDamage = 0;
            comparisonMoveDamage = 0;

            for(int i = 0;i < 4; i++)
            {
                moveRandomMultipliers[i] = enemyTeam[0].Moves[i].NewRandomMultiplier();
            }
            
            moveWeights[0] = enemyTeam[0].Moves[0].GetAIWeight(target,enemyTeam[0],ref bestMoveDamage,moveRandomMultipliers[0]);

            for(int i =  1;i < 4; i++)
            {
                moveWeights[i] = enemyTeam[0].Moves[i].GetAIWeight(target, enemyTeam[0], ref comparisonMoveDamage,moveRandomMultipliers[i]);

                if(moveWeights[bestMoveIndex] > moveWeights[i])
                {
                    if(moveWeights[i] == 1)
                    {
                        statusBucketIndex = i;
                    }
                }
                else if(moveWeights[bestMoveIndex] < moveWeights[i])
                {
                    if(moveWeights[bestMoveIndex] == 1)
                    {
                        statusBucketIndex = bestMoveIndex;
                    }

                    bestMoveIndex = i;
                    bestMoveDamage = comparisonMoveDamage;
                    tieOdds = 1;
                    tieBounds = 2;
                }
                else if(moveWeights[bestMoveIndex] == moveWeights[i] && moveWeights[i] == 2)
                {
                    //if best move is highest damage nothing needs to change so no check

                    if(bestMoveDamage < comparisonMoveDamage)
                    {
                        bestMoveIndex = i;
                        bestMoveDamage = comparisonMoveDamage;
                        tieOdds = 1;
                        tieBounds = 2;
                    }
                    else if(bestMoveDamage == comparisonMoveDamage)
                    {
                        TiedMoveCheck(bestMoveIndex, ref tieOdds, ref tieBounds, i);
                    }
                    
                }
                else
                {
                    TiedMoveCheck(bestMoveIndex, ref tieOdds, ref tieBounds, i);
                }
            }

            if(moveWeights[bestMoveIndex] == 2 && statusBucketIndex != -1)
            {
                random = new Random();
                if(random.Next(1,11) > 7)
                {
                    bestMoveIndex = statusBucketIndex;
                }
            }

            enemyTeam[0].NextMoveRandomMultiplier = moveRandomMultipliers[bestMoveIndex];

            return bestMoveIndex;
        }

        public int TiedMoveCheck(int bestMoveIndex, ref int tieOdds, ref int tieBounds, int replacementIndex)
        {
            Random random = new Random();

            //this sets the bestMoveIndex to that of the move with the highest priority level;
            //on a tie one of the moves is picked at random

            //the odds are increased when a move wins a random selection to account for the fact that moves that have been through random selection multiple times would not have an equal chance of being the final move than moves checked once
            //the odds are set back to 50/50 when a new move is set as the best move

            if(enemyTeam[0].Moves[bestMoveIndex].PriorityLevel == enemyTeam[0].Moves[replacementIndex].PriorityLevel)//by far most likely occurance so checked first
            {
                if(random.Next(0,tieBounds) >= tieOdds)
                {
                    tieOdds = 1;
                    tieBounds = 2;

                    return replacementIndex;
                }
                else
                {
                    tieOdds++;
                    tieBounds++;

                    return bestMoveIndex;
                }
            }
            else if(enemyTeam[0].Moves[bestMoveIndex].PriorityLevel < enemyTeam[0].Moves[replacementIndex].PriorityLevel)
            {
                tieOdds = 1;
                tieBounds = 2;

                return replacementIndex;
            }
            return bestMoveIndex;
        }
 
    }
}
