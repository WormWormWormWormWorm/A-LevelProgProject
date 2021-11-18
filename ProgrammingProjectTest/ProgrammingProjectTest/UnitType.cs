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


    }
}
