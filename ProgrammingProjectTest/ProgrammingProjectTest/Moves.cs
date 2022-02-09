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
        protected string moveID;
        protected string moveName;
        protected UnitType moveType;

        protected int priorityLevel;
        protected int moveAccuracy;

        protected static Random random = new Random();

        public abstract void Use(Unit Target,Unit User);

        public abstract int GetAIWeight(Unit target, Unit user, ref int damage, double randomMultiplier);

        public abstract double NewRandomMultiplier();

        public abstract string ShortPrint();

        public abstract void LongPrint(int startX,int startY);

        public abstract void CombatPrint(int startX,int startY,Unit user,Unit target);

        public abstract void MatchupPrint(int startX,int startY,Unit user,Unit target); 

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

        public int PriorityLevel
        {
            get
            {
                return priorityLevel;
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
                defenseUsed = target.CurrentStat(1);
                attackUsed = user.CurrentStat(0);
            }
            else if(damageCategory == "Special")
            {
                defenseUsed = target.CurrentStat(3);
                attackUsed = user.CurrentStat(2);
            }

            //checks both of the targets unitTypes and returns effectiveness
            typeMultiplier = moveType.EffectivenessCheck(typeMultiplier, target.Type1);
            if(typeMultiplier != 0)
            {
                typeMultiplier = moveType.EffectivenessCheck(typeMultiplier, target.Type2);
            }

            //this segment checks for abilities of the target that alter damage
            if(target.Ability == "Thick Fat" && (moveType.Name == "Fire" || moveType.Name == "Ice"))
            {
                typeMultiplier /= 2;
            }
            else if(target.Ability == "Multiscale")
            {

            }

            //checks if attack unitType matches the user unitType, then gives roughly a 1.5 
            if(moveType == user.Type1 || moveType == user.Type2)
            {
                sameTypeAttackBonus = 1.5;
            }
            
            //damage calculation
            tempDamage = ((((22 * basePower * attackUsed / defenseUsed) / 50) + 2) *typeMultiplier) * sameTypeAttackBonus * randomMultiplier;

            //damage will almost never reach 1000 but would break ui formatting
            if(tempDamage > 999)
            {
                tempDamage = 999;
            }

            finalDamage = Convert.ToInt32(Math.Truncate(tempDamage));

            return finalDamage;
        }

        public double DamagePercent(int dmg,Unit target)
        {
            double damagePercentage;
            double damage = dmg;
            double health = target.Hp;

            damagePercentage = (damage/health);
            damagePercentage = damagePercentage*100;

            return damagePercentage;
        }

        public override void Use(Unit target,Unit user)
        {
            if (user.CanUnitMove())
            {
                if (random.Next(0, 100) <= moveAccuracy)
                {
                    Attack(user, target);
                }
                
            }
            else
            {
                Console.SetCursorPosition(0, 25);
                Console.Write(user.Name + " cannot move due to " + user.Status.FullName);
            }

            if (user.Status.TurnDuration != -1)
            {
                user.ReduceStatusTurnsRemaining();
            }


        }

        public void Attack(Unit user,Unit target)
        {
            int damage;
            double recoilMultiplier;
            int recoilDamage;


            if (user.NextMoveRandomMultiplier == 0)
            {
                damage = DamageCalculation(user, target, NewRandomMultiplier());
            }
            else
            {
                damage = DamageCalculation(user, target, user.NextMoveRandomMultiplier);
                user.NextMoveRandomMultiplier = 0;
            }

            if (random.Next(0, 16) == 1)
            {
                damage *= 2;
            }

            target.TakeDamage(damage,moveName);
            if (recoilPercent != 0)
            {
                recoilMultiplier = recoilPercent;
                recoilMultiplier /= 100;
                if(damage > target.CurrentHp)
                {
                    damage = target.CurrentHp;
                }
                recoilDamage = Convert.ToInt32(recoilMultiplier * damage);
                user.TakeDamage(recoilDamage,"recoil");
            }
            
        }

        public override int GetAIWeight(Unit target, Unit user, ref int damage, double randomMultiplier)
        {

            int moveWeight = 2;

            damage = DamageCalculation(user, target, randomMultiplier);//gets value for damage dealt

            if (damage >= target.CurrentHp)
            {
                moveWeight += 3;
                damage = target.CurrentHp;
            }

            if (moveName == "Fake Out" && user.TurnsOnField == 0)
            {
                moveWeight += 2;
            }
            
            return moveWeight;
        }

        public int RollDamage(Unit user,Unit target)
        {
            int damage;
          
            damage = DamageCalculation(user,target,NewRandomMultiplier());

            return damage;
        }

        public override double NewRandomMultiplier()
        {
            double multiplier = random.Next(84, 100);
            multiplier /= 100;

            return multiplier;
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

        public override void CombatPrint(int startX, int startY, Unit user,Unit target)
        {
            double minDamage = DamageCalculation(user, target, 0.85);
            double maxDamage = DamageCalculation(user, target, 1);

            Console.SetCursorPosition(startX, startY);
            Console.Write(moveName + " ");
            Console.ForegroundColor = moveType.DisplayColor;
            Console.Write(moveType.Name);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" ,recoil {0}%", recoilPercent);
            Console.SetCursorPosition(startX, startY + 1);
            Console.WriteLine("deals({0} to {1}) ,hits {2}%",minDamage,maxDamage,moveAccuracy);
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
            Console.Write("Damage: num({0} - {1}), Perc({2:#.#}%-{3:#.#}%)",minDamage,maxDamage,minDamagePercentage,maxDamagePercentage);
            Console.SetCursorPosition(startX, startY + 2);
            Console.Write("Recoil: " + recoilPercent + "% Priority level: " + priorityLevel);
        }

    }

    class TriggerMoves : DamageMoves
    {
        private Status status;
        private bool statusTargetsSelf;
        private int triggerPercentage;

        public TriggerMoves()
        {

        }

        public TriggerMoves(string moveID, string moveName, UnitType moveType, int moveAccuracy, int basePower, string damageCategory, int recoilPercent, int priorityLevel,Status status,bool statusTargetsSelf,int triggerPercentage)
        {
            this.moveID = moveID;
            this.moveName = moveName;
            this.moveType = moveType;
            this.moveAccuracy = moveAccuracy;
            this.basePower = basePower;
            this.damageCategory = damageCategory;
            this.recoilPercent = recoilPercent;
            this.priorityLevel = priorityLevel;
            this.status = status;
            this.statusTargetsSelf = statusTargetsSelf;
            this.triggerPercentage = triggerPercentage;
        }

        public override void Use(Unit target, Unit user)
        {

            if (user.CanUnitMove())
            {
                if (random.Next(0, 100) <= moveAccuracy)
                {
                    Attack(user, target);

                    if (target.IsAlive)
                    {
                        if (random.Next(0, 100) <= triggerPercentage)
                        {
                            if (!statusTargetsSelf)
                            {
                                target.Status = status;
                            }
                            else
                            {
                                user.Status = status;
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(user.Name + " cannot move due to " + user.Status.FullName);
            }

            if (user.Status.TurnDuration != -1)
            {
                user.ReduceStatusTurnsRemaining();
            }
        }

        public override void LongPrint(int startX, int startY)
        {
            base.LongPrint(startX, startY);
            Console.SetCursorPosition(startX, startY + 3);
            Console.Write("Effect: ");
            status.PrintStatus(statusTargetsSelf);
            Console.Write(" {0} %", triggerPercentage);
        }

        public override string ToString()
        {
            return (base.ToString() + " effect " + status + " trigPerc " + triggerPercentage) ;
        }

        

    }

    class StatusMoves : Moves
    {
        private Status status;
        private bool statusTargetsSelf;

        public StatusMoves(string moveID,string moveName,UnitType moveType,int moveAccuracy,Status status,bool statusTargetsSelf,int priorityLevel)
        {
            this.moveID = moveID;
            this.moveName = moveName;
            this.moveType = moveType;
            this.moveAccuracy = moveAccuracy;
            this.status = status;
            this.statusTargetsSelf = statusTargetsSelf;
            this.priorityLevel = priorityLevel;
        }

        public override void Use(Unit Target,Unit user)
        {
            if(user.CanUnitMove())
            {
                if (random.Next(0, 100) <= moveAccuracy)
                {
                    if (!statusTargetsSelf)
                    {
                        Target.Status = status;
                    }
                    else
                    {
                        user.Status = status;
                    }
                }
            }
            else
            {
                Console.SetCursorPosition(0, 25);
                Console.Write(user.Name + " cannot move due to " + user.Status.FullName);
            }

            if (user.Status.TurnDuration != -1)
            {
                user.ReduceStatusTurnsRemaining();
            }
        }

        public override int GetAIWeight(Unit target, Unit user, ref int damage,double randomMultiplier)// damage ref is used to make abstract method work with all subclasses
        {
            int moveWeight;

            damage = -1;

            if(target.Type1.Immune == moveType.Name || target.Type2.Immune == moveType.Name)//if target is immune to the move give negative weight
            {
                moveWeight = -3;
            }
            else if (target.Stats[4] >= user.Stats[4] && status.FullName[2] == '4' )//if target is faster than unit and status effect changes speed give high weight
            {
                moveWeight = 3;
            }
            else
            {
                moveWeight = 1;
            }

            return moveWeight;
        }

        public override double NewRandomMultiplier()
        {
            return 0;
        }

        public override string ShortPrint()
        {
            return (moveName + GetSpaceBlock(14 - moveName.Length) + "| " + status.DisplayName + GetSpaceBlock(10 - status.DisplayName.Length) + " | ");
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
            status.PrintStatus(statusTargetsSelf);
            Console.SetCursorPosition(startX, startY + 2);
            Console.Write("hit: " + moveAccuracy + "%");
        }

        public override void MatchupPrint(int startX,int startY,Unit user,Unit target)
        {
            LongPrint(startX, startY);
        }

        public override void CombatPrint(int startX, int startY,Unit user,Unit target)
        {
            Console.SetCursorPosition(startX, startY);
            Console.Write(moveName + " ");
            Console.ForegroundColor = moveType.DisplayColor;
            Console.Write(moveType.Name);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(startX, startY + 1);
            Console.Write("Status: ");
            status.PrintStatus(statusTargetsSelf);
            Console.Write(" hit: " + moveAccuracy + "%");
        }

        public override string ToString()
        {
            return ("Name " + moveName + " type " + moveType.Name + " Accuracy " + moveAccuracy + " effect " + status);
        }
    }


}
