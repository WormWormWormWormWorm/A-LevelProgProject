using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProgrammingProjectTest
{
    class MoveList
    {
        List<Moves> AllMoves;

        public MoveList()
        {
            Moves move;
            string ID;


            using (Stream stream = File.Open("AttackMoves.txt", FileMode.Open))
            {
                stream.Seek(133, SeekOrigin.Begin);//skips file explanation text
                using (StreamReader sr = new StreamReader(stream))
                {
                    while(!sr.EndOfStream)
                    {
                        ID = sr.ReadLine();
                        switch (ID[0])
                        {
                            case '0':
                                move = new TriggerMoves(ID,sr.ReadLine(),; break;
                            case '1': move = new DamageMoves(moveID); break;
                            case '2': move = new StatusMoves(moveID); break;
                        }
                    }
                }
            }
        }
    }
}
