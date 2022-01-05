﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProgrammingProjectTest
{
    class ProgressionInfo
    {
        List<int[]> encounterPercentagesList = new List<int[]>();
        List<TemplateUnit[]> encounterPoolList = new List<TemplateUnit[]>();
        List<Unit[]> enemyTeams = new List<Unit[]>();

        public ProgressionInfo(UnitTemplateList unitTemplateList,MoveList moveList)
        {
            int currentPoolSize;
            int unitsInEnemyTeam;
            int currentUnitID;
            int ForcedIV;
            string currentMoveID;
            string replacementAbility;
            int[] encounterPercentages;
            TemplateUnit[] encounterPool;
            Unit[] enemyTeam;
            


            using(StreamReader sr = new StreamReader("CurrentProgression.txt"))
            {
                while(sr.ReadLine() != "///////") { } //reads past File Formatting explanation

                while (!sr.EndOfStream)
                {
                    sr.ReadLine();
                    currentPoolSize = Convert.ToInt32(sr.ReadLine());
                    encounterPercentages = new int[currentPoolSize];
                    encounterPool = new TemplateUnit[currentPoolSize];
                    for(int i = 0; i < currentPoolSize; i++)
                    {
                        encounterPercentages[i] = Convert.ToInt32(sr.ReadLine());
                        encounterPool[i] = unitTemplateList.Templates[Convert.ToInt32(sr.ReadLine())];
                    }
                    encounterPercentagesList.Add(encounterPercentages);
                    encounterPoolList.Add(encounterPool);

                    unitsInEnemyTeam = Convert.ToInt32(sr.ReadLine());
                    enemyTeam = new Unit[6];
                    for(int i = 0;i < unitsInEnemyTeam; i++)
                    {
                        ForcedIV = Convert.ToInt32(sr.ReadLine());
                        currentUnitID = Convert.ToInt32(sr.ReadLine());
                        enemyTeam[i] = new Unit(unitTemplateList.Templates[currentUnitID - 1], moveList, ForcedIV);
                        for(int f = 0;f < 4; f++)
                        {
                            currentMoveID = sr.ReadLine();
                            if(currentMoveID != "#")
                            {
                                enemyTeam[i].ChangeMove(f, moveList.AllMoves[Convert.ToInt32(currentMoveID)]);
                            }
                        }
                        replacementAbility = sr.ReadLine();
                        if(replacementAbility != "#")
                        {
                            enemyTeam[i].Ability = replacementAbility;
                        }
                    }

                    enemyTeams.Add(enemyTeam);
                }
            }
        }

        public int[] GetEncounterPercentages(int index)
        {
            return encounterPercentagesList[index];
        }

        public TemplateUnit[] GetEncounterPools(int index)
        {
            return encounterPoolList[index];
        }

        public Unit[] GetEnemyTeam(int index)
        {
            return enemyTeams[index];
        }
    }
}
