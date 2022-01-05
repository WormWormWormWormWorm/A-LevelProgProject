using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProgrammingProjectTest
{
    class TemplateUnit
    {
        protected int[] baseStats;
        protected string name;
        protected string ability;
        protected UnitType type1;
        protected UnitType type2;
        private string[] moveIDs;

        public TemplateUnit()
        {

        }

        public TemplateUnit(int[] baseStats, string name, string ability, UnitType type1, UnitType type2, string[] moveIDs)
        {
            this.baseStats = baseStats;
            this.name = name;
            this.ability = ability;
            this.type1 = type1;
            this.type2 = type2;
            this.moveIDs = moveIDs;
        }

        public int[] BaseStats
        {
            get
            {
                return baseStats;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Ability
        {
            get
            {
                return ability;
            }
        }

        public UnitType Type1
        {
            get
            {
                return type1;
            }
        }

        public UnitType Type2
        {
            get
            {
                return type2;
            }
        }

        public string[] MoveIDs
        {
            get
            {
                return moveIDs;
            }
        }
    }

    class Unit : TemplateUnit
    {
        private static Random random = new Random();

        protected int plusNature;
        protected int minusNature;
        protected int[] stats = new int[5];
        protected int[] IVS = new int[6];
        protected double[] Modifiers = new double[5];
        
        private string status;

        private int hp;
        private int CurrentHP;

        private Moves[] moves = new Moves[4];

        public Unit()
        {

        }

        public Unit(TemplateUnit template, MoveList moveList)
        {
            plusNature = random.Next(0, 6);
            minusNature = random.Next(0, 6);

            for (int i = 0; i < 6; i++)
            {
                IVS[i] = random.Next(0, 32);
            }

            CreateUnit(template,moveList); 

        }

        public Unit(TemplateUnit template,MoveList moveList,int forcedIV)
        {
            plusNature = 0;
            minusNature = 0;

            for(int i = 0;i < 6; i++)
            {
                IVS[i] = forcedIV;
            }

            CreateUnit(template, moveList);

        }

        public void CreateUnit(TemplateUnit template,MoveList moveList)
        {
            int randomNum = 0;
            int numOfRandmoves = 0;
            int CurrentAvailableMoveSlot = 0;
            string CurrentMoveID;
            string[] bonusMoveIDs = new string[3];


            name = template.Name;
            baseStats = template.BaseStats;
            type1 = template.Type1;
            type2 = template.Type2;
            ability = template.Ability;

            DetermineStats();

            //determines which moves will always be on the unit and then determines which of the random ones will be in the movepool
            //ids that do not begin with '!' are always in the unit
            for (int j = 0; j < 6; j++)
            {
                CurrentMoveID = template.MoveIDs[j];
                if (!CurrentMoveID.EndsWith("#"))
                {
                    if (CurrentMoveID[0] == '!')
                    {
                        bonusMoveIDs[numOfRandmoves] = CurrentMoveID;
                        numOfRandmoves++;
                    }
                    else
                    {
                        moves[CurrentAvailableMoveSlot] = moveList.AllMoves[Convert.ToInt32(CurrentMoveID)];
                        CurrentAvailableMoveSlot++;
                    }
                }
            }
            if(CurrentAvailableMoveSlot == 3)//fills last slot with a random one of the bonus moves if it's empty
            {
                randomNum = random.Next(0, numOfRandmoves);
                moves[CurrentAvailableMoveSlot] = moveList.AllMoves[Convert.ToInt32(bonusMoveIDs[randomNum].Substring(1))];
            }
            
        }

        public UnitType GetType(string typeName)
        {
            UnitType type = new UnitType();
            type = type.UnitTypes[type.DetermineType(typeName)];
            return type;
        }

        public void DetermineStats()
        {
            //previously gathered baseStats and a randomly generated value are used in a formula to get stat values

            //single 1.1 and 0.9 multipliers are generated then assigned each to a stat, if overlapped they cancel out
            //numToMultiplyby exist to replicate multiplication by a fraction multiplying by 9 - 11 then div by 10
            int numTomultiplyBy = 10;

            for (int i = 0; i < 5; i++)
            {
                if(i == plusNature)
                {
                     numTomultiplyBy += 1;
                }
                if(i == minusNature)
                {
                    numTomultiplyBy -= 1;
                }
                stats[i] = ((((2 * baseStats[i] + IVS[i]) / 2) + 5) * numTomultiplyBy) / 10;
                numTomultiplyBy = 10;

            }
            hp = ((2 * baseStats[5] + IVS[5]) / 2) + 60;
        }

        public void ChangeMove(int slot,Moves move)
        {
            moves[slot] = move;
        }

        public void ShortPrint(int startX,int startY)
        {
            //this writes out a large amount of info, most code here is to format it in the way I want

            InitialFormat(startX,startY);

            startY += 2;
            Console.SetCursorPosition(startX + 3, startY);
            Console.Write("HP " + baseStats[5] + GetSpaceBlock(6 - Convert.ToString(baseStats[5]).Length) + "|  ATK " + baseStats[0] + GetSpaceBlock(6 - Convert.ToString(baseStats[0]).Length) + "|  Def " + baseStats[1]);
            startY += 1;
            Console.SetCursorPosition(startX + 1, startY);
            Console.Write("Speed " + baseStats[4] + GetSpaceBlock(5 - Convert.ToString(baseStats[4]).Length) + "| Sp Atk " + baseStats[2] + GetSpaceBlock(4 - Convert.ToString(baseStats[2]).Length) + "| Sp Def " + baseStats[3]);
            startY += 2;
            Console.SetCursorPosition(startX, startY);
            Console.Write("Moves:");

            startX += 1;
            for(int i = 0; i < 4; i++)
            {
                startY++;
                Console.SetCursorPosition(startX, startY);
                Console.Write(moves[i].ShortPrint());
                PrintColoredString(moves[i].MoveType.Name, moves[i].MoveType.DisplayColor);
            }
            Console.SetCursorPosition(startX , startY + 2);
            Console.Write("Ability: " + Ability);
        }

        public void InspectPrint(int startX,int startY)
        {
            InitialFormat(startX, startY);

            startY += 2;
            Console.SetCursorPosition(startX + 3, startY);
            Console.Write("HP " + hp + GetSpaceBlock(6 - Convert.ToString(hp).Length) + "|  ATK " + stats[0] + GetSpaceBlock(6 - Convert.ToString(stats[0]).Length) + "|  Def " + stats[1]);
            startY += 1;
            Console.SetCursorPosition(startX + 1, startY);
            Console.Write("Speed " + stats[4] + GetSpaceBlock(5 - Convert.ToString(stats[4]).Length) + "| Sp Atk " + stats[2] + GetSpaceBlock(4 - Convert.ToString(stats[2]).Length) + "| Sp Def " + stats[3]);
            startY += 2;
            Console.SetCursorPosition(startX, startY);
            Console.Write("Moves:");

            Console.SetCursorPosition(startX, startY);
            startX += 1;
            startY += 1;
            for(int i = 0;i < 4; i++)
            {
                moves[i].LongPrint(startX, startY);
                startY += 5;
            }
        }

        public void InitialFormat(int startX,int startY)
        {
            Console.SetCursorPosition(startX, startY);
            Console.Write(name + GetSpaceBlock(12 - name.Length) + "| ");
            PrintColoredString(type1.Name, type1.DisplayColor);
            Console.Write("/");
            PrintColoredString(type2.Name, type2.DisplayColor);
            //startY += 2;
            //Console.SetCursorPosition(startX + 3, startY);
            //Console.Write("HP " + baseStats[5] + GetSpaceBlock(6 - Convert.ToString(baseStats[5]).Length) + "|  ATK " + baseStats[1] + GetSpaceBlock(6 - Convert.ToString(baseStats[1]).Length) + "|  Def " + baseStats[2]);
            //startY += 1;
            //Console.SetCursorPosition(startX + 1, startY);
            //Console.Write("Speed " + baseStats[4] + GetSpaceBlock(5 - Convert.ToString(baseStats[4]).Length) + "| Sp Atk " + baseStats[2] + GetSpaceBlock(4 - Convert.ToString(baseStats[2]).Length) + "| Sp Def " + baseStats[3]);
            //startY += 2;
            //Console.SetCursorPosition(startX, startY);
            //Console.Write("Moves:");
            //startX += 1;
        }



        public string GetSpaceBlock(int numOfSpaces)//returns string made up of spaces the the length of numOfSpaces
        {
            string toReturn = "";
            for(int i = 0;i < numOfSpaces; i++)
            {
                toReturn += " ";
            }
            return toReturn;
        }

        public void PrefightPrint(int X,int Y)
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(name);
            Console.SetCursorPosition(X + 12, Y);
            PrintColoredString(type1.Name, type1.DisplayColor);
            Console.Write("/");
            PrintColoredString(type2.Name, type2.DisplayColor);

            Console.SetCursorPosition(X, Y + 1);
            PrintColoredString(moves[0].Name, moves[0].MoveType.DisplayColor);

            Console.SetCursorPosition(X + 14,Y + 1);
            PrintColoredString(moves[1].Name, moves[1].MoveType.DisplayColor);

            Console.SetCursorPosition(X, Y + 2);
            PrintColoredString(moves[2].Name, moves[2].MoveType.DisplayColor);

            Console.SetCursorPosition(X + 14, Y + 2);
            PrintColoredString(moves[3].Name, moves[3].MoveType.DisplayColor);
        }

        public void MatchupInspectPrint(int startX,int startY,Unit target)
        {
            InitialFormat(startX, startY);

            startY += 1;
            Console.SetCursorPosition(startX + 3, startY);
            Console.Write("HP " + hp + GetSpaceBlock(6 - Convert.ToString(hp).Length) + "|  ATK " + stats[0] + GetSpaceBlock(6 - Convert.ToString(stats[0]).Length) + "|  Def " + stats[1]);
            startY += 1;
            Console.SetCursorPosition(startX + 1, startY);
            Console.Write("Speed " + stats[4] + GetSpaceBlock(5 - Convert.ToString(stats[4]).Length) + "| Sp Atk " + stats[2] + GetSpaceBlock(4 - Convert.ToString(stats[2]).Length) + "| Sp Def " + stats[3]);
            startY += 1;
            Console.SetCursorPosition(startX, startY);
            Console.Write("Moves:");
            startY += 1;
            moves[0].MatchupPrint(startX, startY, this, target);
            moves[1].MatchupPrint(startX, startY + 5, this, target);
            moves[2].MatchupPrint(startX, startY + 10, this, target);
            moves[3].MatchupPrint(startX, startY + 15, this, target);

        }

        public void PrintColoredString(string stringWrite,ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(stringWrite);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                char[] checks = value.ToCharArray();

                if((int)checks[0] > 57)
                {
                    status = value;
                }
                else if((int)checks[0] > 47)
                {
                    Modifiers[Convert.ToInt32(checks[1])] = Convert.ToInt32(checks[0]) - 2;
                }
            }
        }

        public UnitType Type1
        {
            get
            {
                return type1;
            }
        }

        public UnitType Type2
        {
            get
            {
                return type2;
            }
        }

        public string Ability
        {
            get
            {
                return ability;
            }
            set
            {
                ability = value;
            }
        }

        public int[] Stats
        {
            get
            {
                return stats;
            }
        }

        public int Hp
        {
            get
            {
                return hp;
            }
        }

    }
}
