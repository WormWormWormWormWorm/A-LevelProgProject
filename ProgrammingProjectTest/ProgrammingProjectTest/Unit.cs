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

        protected Moves[] Moves = new Moves[4];


        public Unit(string templateID)
        {

            using (Stream stream = File.Open("C:\\TextFiles\\AlevelProgProject\\Units.txt", FileMode.Open))
            {
                PrepareStream(stream, templateID);

                using (StreamReader sr = new StreamReader(stream))
                {
                    FillBaseValues(sr);                    
                }
            }


            for(int i = 0;i > 5; i++)
            {
                IVS[i] = random.Next(0, 32);
                
            }
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
            baseStats[6] = Convert.ToInt32(sr.ReadLine()); //HP
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
                        DetermineMoveCategory(moveTempHolder, moveSlotsRemaining);
                        moveSlotsRemaining--;
                    }
                }
            }
            while (moveSlotsRemaining != 0)
            {
                //randomly decides which move to put in available move slot
                //if another slot is available after this reorganises randomMoveHolder to swap the move chosen with the Move with the highest slot in the array and then removes chosen move and decides again
                randomNum = random.Next(0, numOfRandmoves);
                if (moveSlotsRemaining > 1)
                {
                    moveTempHolder = randomMoveHolder[numOfRandmoves - 1];
                    randomMoveHolder[numOfRandmoves - 1] = randomMoveHolder[randomNum];
                    randomMoveHolder[randomNum] = moveTempHolder;
                }
                DetermineMoveCategory(randomMoveHolder[numOfRandmoves - 1], moveSlotsRemaining);
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

        public void DetermineMoveCategory(string moveID, int remainingSlots) //subclass of moves dependant on first char of ID
        {
            //flipping the value to get lowest available slot
            remainingSlots = 4 - remainingSlots;
            switch (moveID[0])
            {
                case '0': Moves[remainingSlots] = new TriggerMoves(moveID);break;
                case '1': Moves[remainingSlots] = new DamageMoves(moveID); break;
                case '2': Moves[remainingSlots] = new StatusMoves(moveID); break;
            }
        }

        public void DetermineStats()
        {
            //previously gathered baseStats and a randomly generated value are used in a formula to get stat values

            //single 1.1 and 0.9 multipliers are generated then assigned each to a stat, if overlapped they cancel out
            //numToMultiplyby exist to replicate multiplication by a fraction multiplying by 9 - 11 then div by 10
            int numTomultiplyBy = 10;

            plusNature = random.Next(0, 6);
            minusNature = random.Next(0, 6);

            for (int i = 0; i > 5; i++)
            {
                IVS[i] = random.Next(0, 32);
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
            HP = ((2 * baseStats[6] * IVS[6]) / 2) + 60;
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

    }
}
