using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProgrammingProjectTest
{
    class Unit
    {
        private static Random random = new Random();

        protected int plusNature;
        protected int minusNature;
        protected int[] Stats = new int[5];
        protected int[] baseStats = new int[6];
        protected int[] IVS = new int[6];
        protected double[] Modifiers = new double[5];
        
        protected string templateID;
        private string name;
        private string abilitiy;
        private UnitType type1;
        private UnitType type2;
        private string status;

        private int HP;
        private int CurrentHP;

        private Moves[] moves = new Moves[4];


        public Unit(string templateID)
        {

            plusNature = random.Next(0, 6);
            minusNature = random.Next(0, 6);

            for(int i = 0;i < 6; i++)
            {
                IVS[i] = random.Next(0, 32);
            }

            CreateUnit(templateID);

            //using (Stream stream = File.Open("C:\\TextFiles\\AlevelProgProject\\Units.txt", FileMode.Open))
            //{
            //    PrepareStream(stream, templateID);

            //    using (StreamReader sr = new StreamReader(stream))
            //    {
            //        FillBaseValues(sr);                    
            //    }
            //}

            //DetermineStats();
        }

        public Unit(string templateID,int forcedIV)
        {
            plusNature = 0;
            minusNature = 0;

            for(int i = 0;i < 6; i++)
            {
                IVS[i] = forcedIV;
            }

            CreateUnit(templateID);
        }

        public void CreateUnit(string templateID)
        {
            using (Stream stream = File.Open("C:\\TextFiles\\AlevelProgProject\\Units.txt", FileMode.Open))
            {
                PrepareStream(stream, templateID);

                using (StreamReader sr = new StreamReader(stream))
                {
                    FillBaseValues(sr);
                }
            }

            DetermineStats();
        }

        public void PrepareStream(Stream stream, string TemplateID)
        {
            int byteCount;
            int cutoff;

            cutoff = TemplateID.LastIndexOf('#');
            byteCount = Convert.ToInt32(TemplateID.Substring(cutoff + 1));

            stream.Seek(byteCount, SeekOrigin.Begin);
        }

        public void FillBaseValues(StreamReader sr)
        {
            int randomNum = 0;
            int numOfRandmoves = 0;
            int moveSlotsRemaining = 4;
            string moveTempHolder;
            string[] randomMoveHolder = new string[6];


            sr.ReadLine();
            name = sr.ReadLine();
            type1 = GetType(sr.ReadLine());
            type2 = GetType(sr.ReadLine());
            baseStats[5] = Convert.ToInt32(sr.ReadLine()); //HP
            for (int i = 0; i < 5; i++) //Atk,Def,SpA,SpD,Spe
            {
                baseStats[i] = Convert.ToInt32(sr.ReadLine());
            }
            abilitiy = sr.ReadLine();

            //determines which moves will always be on the unit and then determines which of the random ones will be in the movepool
            //ids that do not begin with '!' are always on the unit
            for (int j = 0; j < 6; j++)
            {
                moveTempHolder = sr.ReadLine();
                if (!moveTempHolder.EndsWith("#"))
                {
                    if (moveTempHolder[0] == '!')
                    {
                        randomMoveHolder[numOfRandmoves] = moveTempHolder;
                        numOfRandmoves++;
                    }
                    else
                    {
                        DetermineMoveCategory(moveTempHolder,4 - moveSlotsRemaining);
                        moveSlotsRemaining--;
                    }
                }
            }
            while (moveSlotsRemaining != 0)
            {
                //randomly decides which move to put in available move slot
                //after this reorganises randomMoveHolder to swap the move chosen with the Move with the highest slot in the array in use and then removes chosen move and decides again if necessary
                randomNum = random.Next(0, numOfRandmoves);
                moveTempHolder = randomMoveHolder[numOfRandmoves - 1];
                randomMoveHolder[numOfRandmoves - 1] = randomMoveHolder[randomNum];
                randomMoveHolder[randomNum] = moveTempHolder;
                DetermineMoveCategory(randomMoveHolder[numOfRandmoves - 1].Substring(1), 4-moveSlotsRemaining);
                moveSlotsRemaining--;
                numOfRandmoves--;
            }
        }

        public UnitType GetType(string typeName)
        {
            UnitType type = new UnitType();
            type = type.UnitTypes[type.DetermineType(typeName)];
            return type;
        }

        public void DetermineMoveCategory(string moveID, int slot) //subclass of moves dependant on first char of ID
        {
           
            switch (moveID[0])
            {
                case '0': moves[slot] = new TriggerMoves(moveID);break;
                case '1': moves[slot] = new DamageMoves(moveID); break;
                case '2': moves[slot] = new StatusMoves(moveID); break;
            }
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
                Stats[i] = ((((2 * baseStats[i] + IVS[i]) / 2) + 5) * numTomultiplyBy) / 10;
                numTomultiplyBy = 10;

            }
            HP = ((2 * baseStats[5] * IVS[5]) / 2) + 60;
        }

        public void ShortPrint(int startX,int startY)
        {
            //this writes out a large amount of info, most code here is to format it in the way I want

            //nameColor getter sets the foreground color to type display color then returns the type name, colorbreak getter changes the color back to default and returns an empty string
            //manual cursor moving allows this to be printed at a non-zero column and format correctly
            InitialFormat(startX,startY);
            startY += 5;
            startX += 1;
            for(int i = 0; i < 4; i++)
            {
                startY++;
                Console.SetCursorPosition(startX, startY);
                Console.Write(moves[i].ShortPrint());
                PrintColoredString(moves[i].MoveType.Name, moves[i].MoveType.DisplayColor);
            }
            Console.SetCursorPosition(startX , startY + 2);
            Console.Write("Ability: " + abilitiy);
        }

        public void InspectPrint(int startX,int startY)
        {
            InitialFormat(startX, startY);
            Console.SetCursorPosition(startX, startY + 5);
            startX += 1;
            startY += 6;
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
            startY += 2;
            Console.SetCursorPosition(startX + 3, startY);
            Console.Write("HP " + baseStats[5] + GetSpaceBlock(6 - Convert.ToString(baseStats[5]).Length) + "|  ATK " + baseStats[1] + GetSpaceBlock(6 - Convert.ToString(baseStats[1]).Length) + "|  Def " + baseStats[2]);
            startY += 1;
            Console.SetCursorPosition(startX + 1, startY);
            Console.Write("Speed " + baseStats[4] + GetSpaceBlock(5 - Convert.ToString(baseStats[4]).Length) + "| Sp Atk " + baseStats[2] + GetSpaceBlock(4 - Convert.ToString(baseStats[2]).Length) + "| Sp Def " + baseStats[3]);
            startY += 2;
            Console.SetCursorPosition(startX, startY);
            Console.Write("Moves:");
            startX += 1;
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
                return abilitiy;
            }
            set
            {
                abilitiy = value;
            }
        }

    }
}
