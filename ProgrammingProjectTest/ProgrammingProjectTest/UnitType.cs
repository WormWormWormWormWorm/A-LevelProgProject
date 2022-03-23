using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProgrammingProjectTest
{
    class CreatureType
    {
        private static CreatureType[] creatureTypes;
        private string name;
        private ConsoleColor displayColor;
        private string[] effectiveAgainst;
        private string[] ineffectiveAgainst;
        private string immune;

        public CreatureType()
        {

        }

        public CreatureType(string name,string[] effectiveAgainst,string[] ineffectiveAgainst,ConsoleColor displayColor,string immune = null)
        {
            this.name = name;
            this.effectiveAgainst = effectiveAgainst;
            this.displayColor = displayColor;
            this.ineffectiveAgainst = ineffectiveAgainst;
            this.immune = immune;
        }



        public int DetermineType(string typeName)
        {
            int indexPos = -1;
            switch (typeName)
            {
                case "Fire": indexPos = 0;break;
                case "Water": indexPos = 1; break;
                case "Grass": indexPos = 2; break;
                case "Electric": indexPos = 3; break;
                case "Ice": indexPos = 4; break;
                case "Fighting": indexPos = 5; break;
                case "Poison": indexPos = 6; break;
                case "Ground": indexPos = 7; break;
                case "Flying": indexPos = 8; break;
                case "Psychic": indexPos = 9; break;
                case "Bug": indexPos = 10; break;
                case "Rock": indexPos = 11; break;
                case "Dragon": indexPos = 12; break;
                case "Dark": indexPos = 13; break;
                case "Steel": indexPos = 14; break;
                case "Normal": indexPos = 15; break;
                case "###": indexPos = 16; break;
            }
            return indexPos;
        }

        public double EffectivenessCheck(double currentMultiplier, CreatureType targetType)
        {
            int loopSkip = 0;

            if(immune == targetType.Name)
            {
                currentMultiplier = 0;
            }
            else
            {
                for (int i = 0; i < effectiveAgainst.Length;i++)
                {
                    if(effectiveAgainst[i] == targetType.Name)
                    {
                        currentMultiplier *= 2;
                        i = effectiveAgainst.Length;
                        loopSkip = ineffectiveAgainst.Length;
                    }
                }
                for(int j = loopSkip; j < ineffectiveAgainst.Length; j++)
                {
                    if (ineffectiveAgainst[j] == targetType.Name)
                    {
                        currentMultiplier /= 2;
                        j = ineffectiveAgainst.Length;
                    }
                }
            }

            return currentMultiplier;
        }

        public CreatureType[] CreatureTypes
        {
            get
            {
                return creatureTypes;
            }
            set
            {
                creatureTypes = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public ConsoleColor DisplayColor
        {
            get
            {
                return displayColor;
            }
        }

        public string Immune
        {
            get
            {
                return immune;
            }
        }

        public CreatureType(string useless)
        {
            //There is a single static array of CreatureTypes initialised at the start of the program, all other instances of CreatureType will reference back to one of these

            //This constructor initialises the static CreatureTypes Array
            string[] effectiveAgainstCurrent;
            string[] inEffectiveAgainstCurrent;
            creatureTypes = new CreatureType[17];

            effectiveAgainstCurrent = new string[] { "Grass", "Bug", "Ice", "Steel" };
            inEffectiveAgainstCurrent = new string[] { "Fire", "Water", "Rock", "Dragon" };

            creatureTypes[0] = new CreatureType("Fire", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkRed);

            effectiveAgainstCurrent = new string[] { "Fire", "Ground", "Rock" };
            inEffectiveAgainstCurrent = new string[] { "Water", "Grass", "Dragon", };

            creatureTypes[1] = new CreatureType("Water", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Blue);

            effectiveAgainstCurrent = new string[] { "Water", "Ground", "Rock" };
            inEffectiveAgainstCurrent = new string[] { "Fire", "Grass", "Dragon", "Flying", "Bug", "Steel", "Poison" };

            creatureTypes[2] = new CreatureType("Grass", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkGreen);

            effectiveAgainstCurrent = new string[] { "Water", "Flying" };
            inEffectiveAgainstCurrent = new string[] { "Electric", "Grass", "Dragon" };

            creatureTypes[3] = new CreatureType("Electric", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Yellow, "Ground");

            effectiveAgainstCurrent = new string[] { "Grass", "Ground", "Dragon", "Flying" };
            inEffectiveAgainstCurrent = new string[] { "Fire", "Steel", "Water", "Ice" };

            creatureTypes[4] = new CreatureType("Ice", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Cyan);

            effectiveAgainstCurrent = new string[] { "Dark", "Steel", "Rock", "Normal", "Ice" };
            inEffectiveAgainstCurrent = new string[] { "Flying", "Poison", "Psychic", "Bug" };

            creatureTypes[5] = new CreatureType("Fight", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Red);

            effectiveAgainstCurrent = new string[] { "Grass"};
            inEffectiveAgainstCurrent = new string[] { "Bug", "Poison", "Rock","Ground" };

            creatureTypes[6] = new CreatureType("Poison", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkMagenta, "Steel");

            effectiveAgainstCurrent = new string[] { "Fire", "Electric", "Rock", "Poison", "Steel" };
            inEffectiveAgainstCurrent = new string[] { "Bug", "Grass" };

            creatureTypes[7] = new CreatureType("Ground", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkYellow, "Flying");

            effectiveAgainstCurrent = new string[] { "Bug", "Fight", "Grass" };
            inEffectiveAgainstCurrent = new string[] { "Rock", "Steel", "Electric" };

            creatureTypes[8] = new CreatureType("Flying", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkCyan);

            effectiveAgainstCurrent = new string[] { "Fight", "Poison" };
            inEffectiveAgainstCurrent = new string[] { "Psychic", "Steel" };

            creatureTypes[9] = new CreatureType("Psychic", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Magenta, "Dark");

            effectiveAgainstCurrent = new string[] { "Grass", "Dark", "Psychic" };
            inEffectiveAgainstCurrent = new string[] { "Fire", "Flying", "Steel", "Poison"};

            creatureTypes[10] = new CreatureType("Bug", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Green);

            effectiveAgainstCurrent = new string[] { "Fire", "Ice", "Flying", "Bug" };
            inEffectiveAgainstCurrent = new string[] { "Steel", "Fight", "Ground" };

            creatureTypes[11] = new CreatureType("Rock", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkGray);

            effectiveAgainstCurrent = new string[] { "Dragon" };
            inEffectiveAgainstCurrent = new string[] { "Steel" };

            creatureTypes[12] = new CreatureType("Dragon", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkBlue);

            effectiveAgainstCurrent = new string[] { "Psychic", "Dragon" };
            inEffectiveAgainstCurrent = new string[] { "Dark", "Steel" , "Fight" };

            creatureTypes[13] = new CreatureType("Dark", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.White);

            effectiveAgainstCurrent = new string[] { "Ice", "Rock" };
            inEffectiveAgainstCurrent = new string[] { "Fire", "Water", "Electric", "Steel" };

            creatureTypes[14] = new CreatureType("Steel", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkGray);

            effectiveAgainstCurrent = new string[] { };
            inEffectiveAgainstCurrent = new string[] { "Rock", "Steel" };

            creatureTypes[15] = new CreatureType("Normal", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Gray);

            effectiveAgainstCurrent = new string[] { };
            inEffectiveAgainstCurrent = new string[] { };

            creatureTypes[16] = new CreatureType("", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Gray);
        }

    }
}
