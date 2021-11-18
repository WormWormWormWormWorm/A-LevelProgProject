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

        public Menu(string[,] displayText, int[,,] coOrdinates)
        {
            this.displayText = displayText;
            this.CoOrdinates = coOrdinates;
        }

        public void Draw()
        {
            for(int i = 0;i < displayText.GetLength(1);i++)
            {
                for(int j = 0;j < displayText.GetLength(0); j++)
                {
                    if (displayText[j,i] != null)
                    {
                        Console.SetCursorPosition(CoOrdinates[j, i, 0], CoOrdinates[j, i, 1]);

                        Console.Write(displayText[j, i]);
                    }
                }
            }
        }

        public void GetInput()
        {
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
                    case ConsoleKey.Enter: optionSelected = pointerX * 100 + pointerY;break;
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
            Console.SetCursorPosition(CoOrdinates[pointerX, pointerY, 0], CoOrdinates[pointerX, pointerY, 1]);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(displayText[pointerX,pointerY]);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void Deselect()
        {
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
                if(value > -1 && value < displayText.GetLength(0))
                {
                    if (displayText[value,pointerY] != null)
                    {
                        Deselect();
                        pointerX = value;
                        Highlight();
                    }
                }
            }
        }

        public int PointerY
        {
            get
            {
                return pointerY;
            }
            set
            {
                if (value > -1 && value < displayText.GetLength(1))
                {
                    if (displayText[pointerX, value] != null)
                    {
                        Deselect();
                        pointerY = value;
                        Highlight();
                    }
                }
            }
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
    }
}
