using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProgrammingProjectTest
{
    class UnitType
    {
        private static UnitType[] unitTypes;
        private string name;
        private ConsoleColor displayColor;
        private string[] effectiveAgainst;
        private string[] ineffectiveAgainst;
        private string immune;

        public UnitType()
        {

        }

        public UnitType(string name,string[] effectiveAgainst,string[] ineffectiveAgainst,ConsoleColor displayColor,string immune = null)
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

        public int TypeMultiplier(string userType,string targetType1,string targetType2)
        {
            //to avoid using doubles multipliers will multiply by the (second digit value)/10 then divide by the first digit value
            int multiplier = 22;

            if(targetType1 == immune || targetType2 == immune)
            {
                return 0;
            }

            if(userType == name || userType == name)
            {
                multiplier = 32;
            }

            multiplier = EffectivenessCheck(multiplier, targetType1);
            multiplier = EffectivenessCheck(multiplier, targetType2);
            return multiplier;

        }

        public int EffectivenessCheck(int multiplier, string targetType)
        {
            int checkForSkip = 0;
            for (int i = 0; i < effectiveAgainst.Length; i++)
            {
                if (targetType == effectiveAgainst[i])
                {
                    multiplier += multiplier - multiplier % 10;
                    i = effectiveAgainst.Length;
                    checkForSkip = ineffectiveAgainst.Length;
                }
            }
            for (int j = checkForSkip; j < ineffectiveAgainst.Length; j++)
            {
                if (targetType == ineffectiveAgainst[j])
                {
                    multiplier += multiplier % 10;
                    j = ineffectiveAgainst.Length;
                }
            }
            return multiplier;
        }

        public UnitType[] UnitTypes
        {
            get
            {
                return unitTypes;
            }
            set
            {
                unitTypes = value;
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

        public UnitType(string useless)
        {
            //There is a single static array of UnitTypes initialised at the start of the program, all other instances of UnitType will reference back to one of these

            //This constructor initialises the static UnitTypes Array
            string[] effectiveAgainstCurrent;
            string[] inEffectiveAgainstCurrent;
            unitTypes = new UnitType[17];

            effectiveAgainstCurrent = new string[] { "Grass", "Bug", "Ice", "Steel" };
            inEffectiveAgainstCurrent = new string[] { "Fire", "Water", "Rock", "Dragon" };

            unitTypes[0] = new UnitType("Fire", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkRed);

            effectiveAgainstCurrent = new string[] { "Fire", "Ground", "Rock" };
            inEffectiveAgainstCurrent = new string[] { "Water", "Grass", "Dragon", };

            unitTypes[1] = new UnitType("Water", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Blue);

            effectiveAgainstCurrent = new string[] { "Water", "Ground", "Rock" };
            inEffectiveAgainstCurrent = new string[] { "Fire", "Grass", "Dragon", "Flying", "Bug", "Steel", "Poison" };

            unitTypes[2] = new UnitType("Grass", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkGreen);

            effectiveAgainstCurrent = new string[] { "Water", "Flying" };
            inEffectiveAgainstCurrent = new string[] { "Electric", "Grass", "Dragon" };

            unitTypes[3] = new UnitType("Electric", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Yellow, "Ground");

            effectiveAgainstCurrent = new string[] { "Grass", "Ground", "Dragon", "Flying" };
            inEffectiveAgainstCurrent = new string[] { "Fire", "Steel", "Water", "Ice" };

            unitTypes[4] = new UnitType("Ice", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Cyan);

            effectiveAgainstCurrent = new string[] { "Dark", "Steel", "Rock", "Normal", "Ice" };
            inEffectiveAgainstCurrent = new string[] { "Flying", "Poison", "Psychic", "Bug" };

            unitTypes[5] = new UnitType("Fight", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Red);

            effectiveAgainstCurrent = new string[] { "Grass", "Water" };
            inEffectiveAgainstCurrent = new string[] { "Bug", "Poison", "Rock" };

            unitTypes[6] = new UnitType("Poison", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkMagenta, "Steel");

            effectiveAgainstCurrent = new string[] { "Fire", "Electric", "Rock", "Poison", "Steel" };
            inEffectiveAgainstCurrent = new string[] { "Bug", "Grass" };

            unitTypes[7] = new UnitType("Ground", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkYellow, "Flying");

            effectiveAgainstCurrent = new string[] { "Bug", "Fight", "Grass" };
            inEffectiveAgainstCurrent = new string[] { "Rock", "Steel", "Electric" };

            unitTypes[8] = new UnitType("Flying", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkCyan);

            effectiveAgainstCurrent = new string[] { "Fight", "Poison" };
            inEffectiveAgainstCurrent = new string[] { "Psychic", "Steel" };

            unitTypes[9] = new UnitType("Psychic", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Magenta, "Dark");

            effectiveAgainstCurrent = new string[] { "Water", "Ground", "Rock" };
            inEffectiveAgainstCurrent = new string[] { "Fire", "Flying", "Steel", "Poison", "Poison" };

            unitTypes[10] = new UnitType("Bug", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Green);

            effectiveAgainstCurrent = new string[] { "Fire", "Ice", "Flying", "Bug" };
            inEffectiveAgainstCurrent = new string[] { "Steel", "Fight", "Ground" };

            unitTypes[11] = new UnitType("Rock", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkGray);

            effectiveAgainstCurrent = new string[] { "Dragon" };
            inEffectiveAgainstCurrent = new string[] { "Steel" };

            unitTypes[12] = new UnitType("Dragon", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkBlue);

            effectiveAgainstCurrent = new string[] { "Psychic", "Dragon" };
            inEffectiveAgainstCurrent = new string[] { "Dark", "Steel" };

            unitTypes[13] = new UnitType("Dark", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.White);

            effectiveAgainstCurrent = new string[] { "Ice", "Rock" };
            inEffectiveAgainstCurrent = new string[] { "Fire", "Water", "Electric", "Steel" };

            unitTypes[14] = new UnitType("Steel", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkGray);

            effectiveAgainstCurrent = new string[] { };
            inEffectiveAgainstCurrent = new string[] { "Rock", "Steel" };

            unitTypes[15] = new UnitType("Normal", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Gray);

            effectiveAgainstCurrent = new string[] { };
            inEffectiveAgainstCurrent = new string[] { };

            unitTypes[16] = new UnitType("", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Gray);
        }

    }
}
