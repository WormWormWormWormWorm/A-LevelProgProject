using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProgrammingProjectTest
{
    abstract class Moves
    {
        public static StreamReader sr;

        protected string moveID;
        protected string moveName;
        protected UnitType moveType;

        protected int moveAccuracy;

        public abstract void Use(Unit Target,Unit User);

        public abstract string ShortPrint();

        public abstract void LongPrint(int startX,int startY);

        public void PrepareStream(Stream stream, string TemplateID)
        {
            int byteCount;
            int cutoff;

            cutoff = TemplateID.LastIndexOf('#');
            byteCount = Convert.ToInt32(TemplateID.Substring(cutoff + 1));

            stream.Seek(byteCount, SeekOrigin.Begin);
        }

        public void GetType(string typeName)
        {
            UnitType type = new UnitType();
            type = type.UnitTypes[type.DetermineType(typeName)];
            moveType = type;
        }

        public string GetSpaceBlock(int numOfSpaces)//returns string made up of spaces the the length of numOfSpaces
        {
            string toReturn = "";
            for (int i = 0; i < numOfSpaces; i++)
            {
                toReturn += " ";
            }
            return toReturn;
        }

        public UnitType MoveType
        {
            get
            {
                return moveType;
            }
        }

        public string Name
        {
            get
            {
                return moveName;
            }
        }

    }

    class DamageMoves : Moves
    {
        protected int basePower;
        protected string damageCategory;
        protected int recoilPercent;
        protected int priorityLevel;

        public DamageMoves()
        {

        }

        public DamageMoves(string moveID)
        {
            //100's of instances of stream and streamreader left behind if not disposed
            using(Stream stream = File.Open("C:\\TextFiles\\AlevelProgProject\\AttackMoves.txt", FileMode.Open))
            {
                PrepareStream(stream, moveID);

                using(StreamReader sr = new StreamReader(stream))
                {
                    PullDetails(sr);
                }
            }
        }

        public void PullDetails(StreamReader sr)
        {
            sr.ReadLine();
            moveName = sr.ReadLine();
            basePower = Convert.ToInt32(sr.ReadLine());
            GetType(sr.ReadLine());
            damageCategory = sr.ReadLine();
            moveAccuracy = Convert.ToInt32(sr.ReadLine());
            recoilPercent = Convert.ToInt32(sr.ReadLine());
            priorityLevel = Convert.ToInt32(sr.ReadLine());
        }

        public override void Use(Unit Target,Unit User)
        {
            throw new NotImplementedException();
        }

        public override string ShortPrint()
        {
            return (moveName + GetSpaceBlock(14 - moveName.Length) + "| power " + GetSpaceBlock(4 - Convert.ToString(basePower).Length) + basePower +  " | ");

        }

        public override void LongPrint(int startX,int startY)
        {
            Console.SetCursorPosition(startX, startY);
            Console.Write(moveName);
            Console.SetCursorPosition(startX+ 13, startY);
            Console.ForegroundColor = moveType.DisplayColor;
            Console.Write(moveType.Name);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(startX, startY + 1);
            Console.Write("Power: " + basePower + " hit: " + moveAccuracy + "%");
            Console.SetCursorPosition(startX, startY + 2);
            Console.Write("Recoil: " + GetSpaceBlock(4 - Convert.ToInt32(basePower)) + " Priority " + priorityLevel);
        }

        public override string ToString()
        {
            return ("Name " + moveName + " type " + moveType.Name + " Accuracy " + moveAccuracy + " bp " + basePower + " category " + damageCategory + " recPerc " + recoilPercent + " priority " + priorityLevel);
        }

    }

    class TriggerMoves : DamageMoves
    {
        string Effect;
        string EffectName;
        int triggerPercentage;

        public TriggerMoves()
        {

        }

        public TriggerMoves(string moveID)
        {
            using (Stream stream = File.Open("C:\\TextFiles\\AlevelProgProject\\AttackMoves.txt", FileMode.Open))
            {
                PrepareStream(stream, moveID);

                using (StreamReader sr = new StreamReader(stream))
                {
                    PullDetails(sr);
                    Effect = sr.ReadLine();
                    triggerPercentage = Convert.ToInt32(sr.ReadLine());
                }
            }
        }

        public override void Use(Unit Target, Unit User)
        {
            base.Use(Target, User);
        }

        public override void LongPrint(int startX, int startY)
        {
            base.LongPrint(startX, startY);
            Console.SetCursorPosition(startX, startY + 3);
            Console.WriteLine("Effect: " + Effect + " " + triggerPercentage + "%");
        }

        public override string ToString()
        {
            return (base.ToString() + " effect " + Effect + " trigPerc " + triggerPercentage) ;
        }

    }

    class StatusMoves : Moves
    {
        protected string status;

        public StatusMoves(string moveID)
        {
            using (Stream stream = File.Open("C:\\TextFiles\\AlevelProgProject\\AttackMoves.txt", FileMode.Open))
            {
                PrepareStream(stream, moveID);

                using (StreamReader sr = new StreamReader(stream))
                {
                    sr.ReadLine();
                    moveName = sr.ReadLine();
                    GetType(sr.ReadLine());
                    moveAccuracy = Convert.ToInt32(sr.ReadLine());
                    status = sr.ReadLine();
                }
            }
        }

        public override void Use(Unit Target,Unit User)
        {
            throw new NotImplementedException();
        }

        public override string ShortPrint()
        {
            return (moveName + GetSpaceBlock(14 - moveName.Length) + "| " + status + GetSpaceBlock(10 - status.Length) + " | ");
        }

        public override void LongPrint(int startX, int startY)
        {
            Console.SetCursorPosition(startX, startY);
            Console.Write(moveName);
            Console.SetCursorPosition(startX + 13, startY);
            Console.ForegroundColor = moveType.DisplayColor;
            Console.Write(moveType.Name);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(startX, startY+1);
            Console.Write(status);
            Console.SetCursorPosition(startX, startY + 2);
            Console.Write("hit: " + moveAccuracy + "%");
        }

        public override string ToString()
        {
            return ("Name " + moveName + " type " + moveType.Name + " Accuracy " + moveAccuracy + " effect " + status);
        }
    }


}
