using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingProjectTest
{
    class Game
    {
        private UnitType[] types;

        public Game()
        {
            initialiseTypeArray();
            //DamageMoves damageMoves = new DamageMoves("1###785");
            //TriggerMoves triggerMoves = new TriggerMoves("0###998");
            //StatusMoves statusMoves = new StatusMoves("2###904");
            //Console.ReadLine();

        }

        public void initialiseTypeArray()
        {
            //This looks awful but is probably the most efficient way of doing this

            //There is a single array of UnitTypes initialised at the start of the program, all other instances of UnitType will point back to one of these
            string[] effectiveAgainstCurrent;
            string[] inEffectiveAgainstCurrent;
            types = new UnitType[16];

            effectiveAgainstCurrent = new string[] { "Grass", "Bug", "Ice", "Steel" };
            inEffectiveAgainstCurrent = new string[] { "Fire", "Water", "Rock", "Dragon" };

            types[0] = new UnitType("Fire", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Red);

            effectiveAgainstCurrent = new string[] { "Fire", "Ground", "Rock" };
            inEffectiveAgainstCurrent = new string[] { "Water", "Grass", "Dragon", };

            types[1] = new UnitType("Water", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Blue);

            effectiveAgainstCurrent = new string[] { "Water", "Ground", "Rock" };
            inEffectiveAgainstCurrent = new string[] { "Fire", "Grass", "Dragon", "Flying", "Bug", "Steel", "Poison"};

            types[2] = new UnitType("Grass", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkGreen);

            effectiveAgainstCurrent = new string[] { "Water", "Flying" };
            inEffectiveAgainstCurrent = new string[] { "Electric", "Grass", "Dragon" };

            types[3] = new UnitType("Electric", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Yellow, "Ground");

            effectiveAgainstCurrent = new string[] { "Grass", "Ground", "Dragon", "Flying" };
            inEffectiveAgainstCurrent = new string[] { "Fire","Steel","Water","Ice" };

            types[4] = new UnitType("Ice", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Cyan);

            effectiveAgainstCurrent = new string[] { "Dark", "Steel", "Rock","Normal","Ice" };
            inEffectiveAgainstCurrent = new string[] { "Flying", "Poison", "Psychic", "Bug" };

            types[5] = new UnitType("Fighting", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkRed);

            effectiveAgainstCurrent = new string[] { "Grass", "Water"};
            inEffectiveAgainstCurrent = new string[] { "Bug","Poison", "Rock" };

            types[6] = new UnitType("Poison", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkMagenta, "Steel");

            effectiveAgainstCurrent = new string[] { "Fire", "Electric", "Rock" , "Poison" , "Steel" };
            inEffectiveAgainstCurrent = new string[] { "Bug", "Grass" };

            types[7] = new UnitType("Ground", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkYellow,"Flying");

            effectiveAgainstCurrent = new string[] { "Bug", "Fighting", "Grass" };
            inEffectiveAgainstCurrent = new string[] { "Rock", "Steel", "Electric" };

            types[8] = new UnitType("Flying", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkCyan);

            effectiveAgainstCurrent = new string[] { "Fighting", "Poison"};
            inEffectiveAgainstCurrent = new string[] {  "Psychic", "Steel" };

            types[9] = new UnitType("Psychic", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Magenta, "Dark");

            effectiveAgainstCurrent = new string[] { "Water", "Ground", "Rock" };
            inEffectiveAgainstCurrent = new string[] { "Fire","Flying", "Steel", "Poison" ,"Poison"};

            types[10] = new UnitType("Bug", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.Green);

            effectiveAgainstCurrent = new string[] { "Fire", "Ice", "Flying", "Bug" };
            inEffectiveAgainstCurrent = new string[] { "Steel", "Fighting", "Ground"};

            types[11] = new UnitType("Rock", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkGray);

            effectiveAgainstCurrent = new string[] { "Dragon"};
            inEffectiveAgainstCurrent = new string[] { "Steel" };

            types[12] = new UnitType("Dragon", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkBlue);

            effectiveAgainstCurrent = new string[] { "Psychic", "Dragon" };
            inEffectiveAgainstCurrent = new string[] { "Dark", "Steel" };

            types[13] = new UnitType("Dark", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.White);

            effectiveAgainstCurrent = new string[] { "Ice", "Rock" };
            inEffectiveAgainstCurrent = new string[] { "Fire", "Water","Electric","Steel"};

            types[14] = new UnitType("Steel", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkGray);

            effectiveAgainstCurrent = new string[] { };
            inEffectiveAgainstCurrent = new string[] { "Rock" , "Steel" };

            types[15] = new UnitType("Normal", effectiveAgainstCurrent, inEffectiveAgainstCurrent, ConsoleColor.DarkGray);

            types[0].UnitTypes = types;

        }

    }
}
