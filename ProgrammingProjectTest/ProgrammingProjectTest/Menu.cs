using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingProjectTest
{
    class Menu
    {
        private string[,] displayText;
        private int[,,] CoOrdinates;
        private int pointerX;
        private int pointerY;
        private int optionSelected = -1;

        public Menu()
        {

        }

        public Menu(string[,] displayText, int[,,] coOrdinates)
        {
            this.displayText = displayText;
            this.CoOrdinates = coOrdinates;
        }

        public void Draw()
        {
            //uses setCursorPosition to go to each coOrdinate position and write the display string or "---------" if it is null, unless it's display is "-" then nothing is done
            for(int i = 0;i < displayText.GetLength(1);i++)
            {
                for(int j = 0;j < displayText.GetLength(0); j++)
                {
                    if(displayText[j,i] != "-")
                    {
                        Console.SetCursorPosition(CoOrdinates[j, i, 0], CoOrdinates[j, i, 1]);

                        if (displayText[j, i] != null)
                        {
                            Console.Write(displayText[j, i]);
                        }
                        else
                        {
                            Console.Write("--------");
                        }
                    }
                }
            }
        }

        public void GetInput()
        {
            //gets a keyboard input, if arrowkey move pointerX or pointerY in corresponding direction, if enter change optionSelected to unique number depending on where both pointers are which will then be used outside the loop to select an option
            //loops until it gets a valid input
            bool inputGot = false;
            ConsoleKeyInfo cki;
            do
            {
                cki = Console.ReadKey(true);
                inputGot = true;
                switch (cki.Key)
                {
                    
                    case ConsoleKey.LeftArrow: PointerX -= 1;break;
                    case ConsoleKey.RightArrow: PointerX += 1;break;
                    case ConsoleKey.UpArrow: PointerY -= 1;break;
                    case ConsoleKey.DownArrow: PointerY += 1;break;
                    case ConsoleKey.Enter: optionSelected = (pointerX+1) * 100 + pointerY;break;
                    default: inputGot = false;break;
                }
            } while (inputGot == false);
        }

        public void SetPointer(int X,int Y)
        {
            pointerX = X;
            pointerY = Y;
            Highlight();
        }

        public void Highlight()
        {
            //goes to coOrds that corresponfd with current pointers and re-writes its display string with inverted colours to give highlighting effect
            //used to convey currently selected option to user
            Console.SetCursorPosition(CoOrdinates[pointerX, pointerY, 0], CoOrdinates[pointerX, pointerY, 1]);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(displayText[pointerX,pointerY]);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void Deselect()
        {
            //used to de-highlight a display string
            //when a new option needs to be highlighted
            Console.SetCursorPosition(CoOrdinates[pointerX, pointerY, 0], CoOrdinates[pointerX, pointerY, 1]);
            Console.Write(displayText[pointerX, pointerY]);
        }


        public int PointerX
        {
            get
            {
                return pointerX;
            }
            set
            {
                int direction = 0;
                //makes sure new position of pointerX is available or exists
                if(value < pointerX)
                {
                    direction = -1;
                }
                else
                {
                    direction = 1;
                }

                value = PlaceInBoundsX(value);

                while(displayText[value,pointerY] == null || displayText[value,pointerY] == "-")
                {
                    value += direction;
                    value = PlaceInBoundsX(value);
                }

                //de-highlights old pointer's displayText and then highlights new pointers display text
                Deselect();
                pointerX = value;
                Highlight();
            }
        }

        public int PlaceInBoundsX(int value)
        {
            //make sure updated pointerX corresponds to an existing position in display
            //then moves it to next available slot in direction of arrow input if not, by wrapping around
            if (value < 0)
            {
                value = displayText.GetLength(0) - 1;
            }
            else if (value > displayText.GetLength(0) - 1)
            {
                value = 0;
            }
            return value;
        }

        public int PointerY
        {
            get
            {
                return pointerY;
            }
            set
            {
                int direction = 0;
                //figures out which direction the pointer is moving
                if (value < pointerY)
                {
                    direction = -1;
                }
                else
                {
                    direction = 1;
                }

                value = PlaceInBoundsY(value);

                //if new position in menu is null or "-" move it in the direction of the previous movement
                while (displayText[pointerX,value] == null || displayText[pointerX,value] == "-")
                {
                    value += direction;
                    value = PlaceInBoundsY(value);
                }
                //makes sure new position of pointerY is available or exists

                //de-highlights old pointer's displayText and then highlights new pointers display text
                Deselect();
                pointerY = value;
                Highlight();
            }
        }

        public int PlaceInBoundsY(int value)
        {
            //make sure updated pointerY corresponds to an existing position in display
            //then moves it to next available slot in direction of arrow input if not, by wrapping around
            if (value < 0)
            {
                value = displayText.GetLength(1) - 1;
            }
            else if (value > displayText.GetLength(1) - 1)
            {
                value = 0;
            }
            return value;
        }

        public int OptionSelected
        {
            get
            {
                return optionSelected;
            }
            set
            {
                optionSelected = value;
            }
        }

        public int OptionSelectedReset
        {
            get
            {
                int num = optionSelected;
                optionSelected = -1;
                return num;
            }
        }

        public string[,] Display
        {
            get
            {
                return displayText;
            }
        }

        public int[,,] CoOrds
        {
            get
            {
                return CoOrdinates;
            }
        }
    }
}
