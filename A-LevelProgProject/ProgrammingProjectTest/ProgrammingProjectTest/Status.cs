using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingProjectTest
{
    class Status
    {
        private static Status[] statuses;

        private string name;
        private ConsoleColor displayColor;
        private int turnDuration;
        private bool removedOnSwitch;
        private bool removedAtEndOfTurn;
        private bool afflictedUnitCannotMove;
        private int overrideLevel;

        public Status()
        {

        }

        public Status(string name,ConsoleColor displayColor,int turnDuration,bool removedOnSwitch,bool removedAtEndOfTurn,bool afflictedUnitCannotMove,int overrideLevel)
        {
            this.name = name;
            this.displayColor = displayColor;
            this.turnDuration = turnDuration;
            this.removedOnSwitch = removedOnSwitch;
            this.removedAtEndOfTurn = removedAtEndOfTurn;
            this.afflictedUnitCannotMove = afflictedUnitCannotMove;
            this.overrideLevel = overrideLevel;
        }

        public Status(string name)
        {
            this.name = name;
            displayColor = ConsoleColor.Gray;
        }

        public int DetermineStatusIndex(string statusName)
        {
            int index = -1;
            switch (statusName)
            {
                case "Burn": index = 0; break;
                case "Paralyzed": index = 1;break;
                case "Sleep": index = 2; break;
                case "Freeze": index = 3; break;
                case "Poison": index = 4; break;
                case "Flinch": index = 5; break;
                case "Stun": index = 6; break;
                case "Mirror Coat": index = 7; break;
                case "Sacrifice": index = 8; break;
                case "Outrage": index = 9; break;
                case "Drain": index = 10; break;
            }
            return index;
        }

        public void PrintStatus(bool targetsSelf = false)
        {
            string toDisplay = DisplayName;

            if (targetsSelf)
            {
                toDisplay = "!" + toDisplay; 
            }

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = displayColor;
            Console.Write(toDisplay);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public string DisplayName
        {
            get
            {
                if(name != "")
                {
                    if (char.IsDigit(name[0]))
                    {
                        return name.Substring(2);
                    }
                }
                
                return name;
            }
        }

        public string FullName
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

        public bool RemovedAtEndOfTurn
        {
            get
            {
                return removedAtEndOfTurn;
            }
        }

        public int TurnDuration
        {
            get
            {
                return turnDuration;
            }
        }

        public int OverrideLevel
        {
            get
            {
                return overrideLevel;
            }
        }

        public bool AfflictedUnitCannotMove
        {
            get
            {
                return afflictedUnitCannotMove;
            }
        }

        public Status(int InstansiationDifferentiator)
        {
            statuses = new Status[12];

            string name;
            ConsoleColor displayColor;
            int turnDuration;
            bool removedOnSwitch;
            bool removedAtEndOfTurn;
            bool afflictedUnitCannotMove;
            int overrideLevel;

            name = "Burn";
            displayColor = ConsoleColor.DarkRed;
            turnDuration = -1;
            removedOnSwitch = false;
            removedAtEndOfTurn = false;
            afflictedUnitCannotMove = false;
            overrideLevel = 1;

            statuses[0] = new Status(name, displayColor, turnDuration, removedOnSwitch, removedAtEndOfTurn, afflictedUnitCannotMove, overrideLevel);

            name = "Paralysis";
            displayColor = ConsoleColor.Yellow;
            turnDuration = -1;
            removedOnSwitch = false;
            removedAtEndOfTurn = false;
            afflictedUnitCannotMove = false;
            overrideLevel = 1;

            statuses[1] = new Status(name, displayColor, turnDuration, removedOnSwitch, removedAtEndOfTurn, afflictedUnitCannotMove, overrideLevel);

            name = "Sleep";
            displayColor = ConsoleColor.Gray;
            turnDuration = 2;
            removedOnSwitch = false;
            removedAtEndOfTurn = false;
            afflictedUnitCannotMove = true;
            overrideLevel = 2;

            statuses[2] = new Status(name, displayColor, turnDuration, removedOnSwitch, removedAtEndOfTurn, afflictedUnitCannotMove, overrideLevel);

            name = "Freeze";
            displayColor = ConsoleColor.Cyan;
            turnDuration = 1;
            removedOnSwitch = false;
            removedAtEndOfTurn = false;
            afflictedUnitCannotMove = true;
            overrideLevel = 1;

            statuses[3] = new Status(name, displayColor, turnDuration, removedOnSwitch, removedAtEndOfTurn, afflictedUnitCannotMove, overrideLevel);

            name = "Poison";
            displayColor = ConsoleColor.DarkMagenta;
            turnDuration = -1;
            removedOnSwitch = false;
            removedAtEndOfTurn = false;
            afflictedUnitCannotMove = false;
            overrideLevel = 1;

            statuses[4] = new Status(name, displayColor, turnDuration, removedOnSwitch, removedAtEndOfTurn, afflictedUnitCannotMove, overrideLevel);

            name = "Flinch";
            displayColor = ConsoleColor.White;
            turnDuration = -1;
            removedOnSwitch = true;
            removedAtEndOfTurn = true;
            afflictedUnitCannotMove = true;
            overrideLevel = 2;

            statuses[5] = new Status(name, displayColor, turnDuration, removedOnSwitch, removedAtEndOfTurn, afflictedUnitCannotMove, overrideLevel);

            name = "Stun";
            displayColor = ConsoleColor.Gray;
            turnDuration = 2;
            removedOnSwitch = true;
            removedAtEndOfTurn = false;
            afflictedUnitCannotMove = true;
            overrideLevel = 1;

            statuses[6] = new Status(name, displayColor, turnDuration, removedOnSwitch, removedAtEndOfTurn, afflictedUnitCannotMove, overrideLevel);

            name = "Mirror Coat";
            displayColor = ConsoleColor.DarkGray;
            turnDuration = -1;
            removedOnSwitch = true;
            removedAtEndOfTurn = true;
            afflictedUnitCannotMove = false;
            overrideLevel = 2;

            statuses[7] = new Status(name, displayColor, turnDuration, removedOnSwitch, removedAtEndOfTurn, afflictedUnitCannotMove, overrideLevel);

            name = "Sacrifice";
            displayColor = ConsoleColor.Red;
            turnDuration = 2;
            removedOnSwitch = true;
            removedAtEndOfTurn = false;
            afflictedUnitCannotMove = false;
            overrideLevel = 3;

            statuses[8] = new Status(name, displayColor, turnDuration, removedOnSwitch, removedAtEndOfTurn, afflictedUnitCannotMove, overrideLevel);

            name = "Outrage";
            displayColor = ConsoleColor.DarkBlue;
            turnDuration = 2;
            removedOnSwitch = true;
            removedAtEndOfTurn = false;
            afflictedUnitCannotMove = false;
            overrideLevel = 0;

            statuses[9] = new Status(name, displayColor, turnDuration, removedOnSwitch, removedAtEndOfTurn, afflictedUnitCannotMove, overrideLevel);

            name = "Drain";
            displayColor = ConsoleColor.Green;
            turnDuration = -1;
            removedOnSwitch = true;
            removedAtEndOfTurn = false;
            afflictedUnitCannotMove = false;
            overrideLevel = 0;

            statuses[10] = new Status(name, displayColor, turnDuration, removedOnSwitch, removedAtEndOfTurn, afflictedUnitCannotMove, overrideLevel);

            name = "";
            displayColor = ConsoleColor.Gray;
            turnDuration = -1;
            removedOnSwitch = false;
            removedAtEndOfTurn = false;
            afflictedUnitCannotMove = false;
            overrideLevel = -1;

            statuses[11] = new Status(name, displayColor, turnDuration, removedOnSwitch, removedAtEndOfTurn, afflictedUnitCannotMove, overrideLevel);
        }

        public Status GetStatus(int index)
        {
            return statuses[index];
        }
    }
}
