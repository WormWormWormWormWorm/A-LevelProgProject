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
                    Console.SetCursorPosition(CoOrdinates[j, i, 0], CoOrdinates[j, i, 1]);

                    if (displayText[j,i] != null)
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
                    case ConsoleKey.Enter: optionSelected = pointerX+1 * 100 + pointerY;break;
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
                //if(value > -1 && value < displayText.GetLength(0))
                //{
                //    if (displayText[value,pointerY] != null)
                //    {
                //        Deselect();
                //        pointerX = value;
                //        Highlight();
                //    }
                //}
                value = PlaceInBoundsX(value);
                if(displayText[value,pointerY] == null)
                {
                    if(value > pointerX)
                    {
                        while(displayText[value, pointerY] == null)
                        {
                            value = PlaceInBoundsX(value + 1);
                        }
                    }
                    else
                    {
                        while(displayText[value,pointerY] == null)
                        {
                            value = PlaceInBoundsX(value - 1);
                        }
                    }
                }
                Deselect();
                pointerX = value;
                Highlight();
            }
        }

        public int PlaceInBoundsX(int value)
        {
            if (value < 0)
            {
                value = displayText.GetLength(0) - 1;
                while (displayText[value, pointerY] == null)
                {
                    value--;
                }
            }
            else if (value > displayText.GetLength(0) - 1)
            {
                value = 0;
                while (displayText[value, pointerY] == null)
                {
                    value++;
                }
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
                //if (value > -1 && value < displayText.GetLength(1))
                //{
                //    if (displayText[pointerX, value] != null)
                //    {
                //        Deselect();
                //        pointerY = value;
                //        Highlight();
                //    }
                //}

                value = PlaceInBoundsY(value);
                if (displayText[pointerX,value] == null)
                {
                    if (value > pointerY)
                    {
                        while (displayText[pointerX, value] == null)
                        {
                            value = PlaceInBoundsY(value + 1);
                        }
                    }
                    else
                    {
                        while (displayText[pointerX, value] == null)
                        {
                            value = PlaceInBoundsY(value - 1);
                        }
                    }
                }
                Deselect();
                pointerY = value;
                Highlight();
            }
        }

        public int PlaceInBoundsY(int value)
        {
            if (value < 0)
            {
                value = displayText.GetLength(1) - 1;
                while (displayText[pointerX,value] == null)
                {
                    value--;
                }
            }
            else if (value > displayText.GetLength(1) - 1)
            {
                value = 0;
                while (displayText[pointerX, value] == null)
                {
                    value++;
                }
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
    }
}
