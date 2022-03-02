using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingProjectTest
{
    class Program
    {
        static void Main(string[] args)
        {

            //string[,] nameArray = new string[9, 9];
            //int[,,] coOrdArray = new int[9, 9, 2];
            //int numFill = 10;

            //Console.CursorVisible = false;

            //for(int y = 0;y < nameArray.GetLength(1); y++)
            //{
            //    for(int x = 0;x < nameArray.GetLength(0); x++)
            //    {
            //        nameArray[x, y] = Convert.ToString(numFill);
            //        numFill++;
            //        coOrdArray[x, y, 0] = x * 3;
            //        coOrdArray[x, y, 1] = y * 3;
            //    }
            //}

            //Menu menuTest = new Menu(nameArray, coOrdArray);

            //menuTest.Draw();
            //menuTest.SetPointer(0, 0);

            //while(menuTest.OptionSelected == -1)
            //{
            //    menuTest.GetInput();
            //}

            StartUp game = new StartUp();


        }
    }
}
