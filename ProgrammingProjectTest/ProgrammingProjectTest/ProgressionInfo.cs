using System;
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
        List<TemplateCreature[]> encounterPoolList = new List<TemplateCreature[]>();
        List<Creature[]> enemyTeams = new List<Creature[]>();

        public ProgressionInfo(CreatureTemplateList CreatureTemplateList,MoveList moveList)
        {
            int currentPoolSize;
            int CreaturesInEnemyTeam;
            int currentCreatureID;
            int ForcedIV;
            string currentMoveID;
            string replacementAbility;
            int[] encounterPercentages;
            TemplateCreature[] encounterPool;
            Creature[] enemyTeam;
            


            using(StreamReader sr = new StreamReader("CurrentProgression.txt"))
            {
                while(sr.ReadLine() != "///////") { } //reads past File Formatting explanation

                while (!sr.EndOfStream)
                {
                    sr.ReadLine();
                    currentPoolSize = Convert.ToInt32(sr.ReadLine());
                    encounterPercentages = new int[currentPoolSize];
                    encounterPool = new TemplateCreature[currentPoolSize];
                    for(int i = 0; i < currentPoolSize; i++)
                    {
                        encounterPercentages[i] = Convert.ToInt32(sr.ReadLine());
                        encounterPool[i] = CreatureTemplateList.Templates[Convert.ToInt32(sr.ReadLine())];
                    }
                    encounterPercentagesList.Add(encounterPercentages);
                    encounterPoolList.Add(encounterPool);

                    CreaturesInEnemyTeam = Convert.ToInt32(sr.ReadLine());
                    enemyTeam = new Creature[6];
                    for(int i = 0;i < CreaturesInEnemyTeam; i++)
                    {
                        ForcedIV = Convert.ToInt32(sr.ReadLine());
                        currentCreatureID = Convert.ToInt32(sr.ReadLine());
                        enemyTeam[i] = new Creature(CreatureTemplateList.Templates[currentCreatureID], moveList, ForcedIV);
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

        public TemplateCreature[] GetEncounterPools(int index)
        {
            return encounterPoolList[index];
        }

        public Creature[] GetEnemyTeam(int index)
        {
            return enemyTeams[index];
        }
    }
}
