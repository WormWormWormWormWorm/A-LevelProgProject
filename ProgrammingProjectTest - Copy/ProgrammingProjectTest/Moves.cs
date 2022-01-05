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

        protected int priorityLevel;
        protected int moveAccuracy;

        public abstract void Use(Unit Target,Unit User);

        public abstract string ShortPrint();

        public abstract void LongPrint(int startX,int startY);

        public abstract void MatchupPrint(int startX,int startY,Unit user,Unit target); 

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

        public DamageMoves()
        {

        }

        public DamageMoves(string moveID, string moveName,UnitType moveType, int moveAccuracy, int basePower,string damageCategory,int recoilPercent,int priorityLevel)
        {
            this.moveID = moveID;
            this.moveName = moveName;
            this.moveType = moveType;
            this.moveAccuracy = moveAccuracy;
            this.basePower = basePower;
            this.damageCategory = damageCategory;
            this.recoilPercent = recoilPercent;
            this.priorityLevel = priorityLevel;
        }

        public DamageMoves(string moveID)
        {
            //100's of instances of stream and streamreader left behind if not disposed
            using(Stream stream = File.Open("AttackMoves.txt", FileMode.Open))
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

        public int DamageCalculation(Unit user,Unit target,double randomMultiplier)
        {
            int finalDamage;
            double tempDamage;
            double typeMultiplier = 1;
            //currently written to crash program if damage category is not phys or spec
            int defenseUsed = 0;
            int attackUsed = 1;
            double sameTypeAttackBonus = 1;

            //gets attack of user and defense of target
            //detemines damage category then uses attack/def for physical and special attac/def for special
            if(damageCategory == "Physical")
            {
                defenseUsed = target.Stats[1];
                attackUsed = user.Stats[0];
            }
            else if(damageCategory == "Special")
            {
                defenseUsed = target.Stats[1];
                attackUsed = user.Stats[0];
            }

            //checks both of the targets unitTypes and returns effectiveness
            typeMultiplier = moveType.EffectivenessCheck(typeMultiplier, target.Type1);
            if(typeMultiplier != 0)
            {
                typeMultiplier = moveType.EffectivenessCheck(typeMultiplier, target.Type2);
            }

            //checks if attack unitType matches the user unitType, then gives roughly a 1.5 
            if(moveType == user.Type1 || moveType == user.Type2)
            {
                sameTypeAttackBonus = 1.5;
            }
            
            //damage calculation
            tempDamage = (((22 * basePower * attackUsed / defenseUsed) / 50 + 2) *typeMultiplier) * sameTypeAttackBonus * randomMultiplier;

            //damage will almost never reach 1000 but would break ui formatting
            if(tempDamage > 999)
            {
                tempDamage = 999;
            }

            finalDamage = Convert.ToInt32(tempDamage);

            return finalDamage;
        }

        public double DamagePercent(int damage,Unit target)
        {
            double damagePercentage;
            double remainder;

            damagePercentage = (damage / target.Hp)*100;

            //truncates num to 1d.p
            remainder = damagePercentage % 0.1;
            damagePercentage -= remainder;

            return damagePercentage;
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
            Console.SetCursorPosition(startX+ 14, startY);
            Console.ForegroundColor = moveType.DisplayColor;
            Console.Write(moveType.Name);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(startX, startY + 1);
            Console.Write("Power: " + basePower + " hit: " + moveAccuracy + "%");
            Console.SetCursorPosition(startX, startY + 2);
            Console.Write("Recoil: "+ recoilPercent + GetSpaceBlock(4 - Convert.ToInt32(basePower)) + " Priority " + priorityLevel);
        }

        

        public override string ToString()
        {
            return ("Name " + moveName + " type " + moveType.Name + " Accuracy " + moveAccuracy + " bp " + basePower + " category " + damageCategory + " recPerc " + recoilPercent + " priority " + priorityLevel);
        }

        public override void MatchupPrint(int startX,int startY,Unit user,Unit target)
        {
            int maxDamage = DamageCalculation(user, target, 1);
            int minDamage = DamageCalculation(user, target, 0.85);

            double maxDamagePercentage = DamagePercent(maxDamage,target);
            double minDamagePercentage = DamagePercent(minDamage, target);

            Console.SetCursorPosition(startX, startY);
            Console.Write(moveName + " ");
            Console.ForegroundColor = moveType.DisplayColor;
            Console.Write(moveType.Name);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" accuracy " + moveAccuracy + "%");
            Console.SetCursorPosition(startX, startY + 1);
            Console.Write("Damage: num(" + minDamage + "-" + maxDamage + "), Perc(" + minDamagePercentage + "%-" + maxDamagePercentage + "%)");
            Console.SetCursorPosition(startX, startY + 2);
            Console.Write("Recoil: " + recoilPercent + "% Priority level: " + priorityLevel);
        }

    }

    class TriggerMoves : DamageMoves
    {
        private string Effect;
        private int triggerPercentage;

        public TriggerMoves()
        {

        }

        public TriggerMoves(string moveID, string moveName, UnitType moveType, int moveAccuracy, int basePower, string damageCategory, int recoilPercent, int priorityLevel,string effect,int triggerPercentage)
        {
            this.moveID = moveID;
            this.moveName = moveName;
            this.moveType = moveType;
            this.moveAccuracy = moveAccuracy;
            this.basePower = basePower;
            this.damageCategory = damageCategory;
            this.recoilPercent = recoilPercent;
            this.priorityLevel = priorityLevel;
            this.Effect = effect;
            this.triggerPercentage = triggerPercentage;
        }

        public TriggerMoves(string moveID)
        {
            using (Stream stream = File.Open("AttackMoves.txt", FileMode.Open))
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
            using (Stream stream = File.Open("AttackMoves.txt", FileMode.Open))
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

        public StatusMoves(string moveID,string moveName,UnitType moveType,int moveAccuracy,string status,int priorityLevel)
        {
            this.moveID = moveID;
            this.moveName = moveName;
            this.moveType = moveType;
            this.moveAccuracy = moveAccuracy;
            this.status = status;
            this.priorityLevel = priorityLevel;
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
            Console.SetCursorPosition(startX + 14, startY);
            Console.ForegroundColor = moveType.DisplayColor;
            Console.Write(moveType.Name);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(startX, startY+1);
            Console.Write(status);
            Console.SetCursorPosition(startX, startY + 2);
            Console.Write("hit: " + moveAccuracy + "%");
        }

        public override void MatchupPrint(int startX,int startY,Unit user,Unit target)
        {
            LongPrint(startX, startY);
        }

        public override string ToString()
        {
            return ("Name " + moveName + " type " + moveType.Name + " Accuracy " + moveAccuracy + " effect " + status);
        }
    }


}
