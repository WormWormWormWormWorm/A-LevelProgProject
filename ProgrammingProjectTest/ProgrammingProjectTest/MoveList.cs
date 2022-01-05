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
        List<Moves> allMoves;

        public MoveList()
        {
            Moves move;
            string ID;
            string name;
            UnitType type = new UnitType();
            int accuracy;
            int basePower;
            string damageCategory;
            int recoilPercent;
            int priorityLevel;
            string effect;
            int triggerPercentage;

            allMoves = new List<Moves>();


            using (Stream stream = File.Open("AttackMoves.txt", FileMode.Open))
            {
                stream.Seek(133, SeekOrigin.Begin);//skips file explanation text
                using (StreamReader sr = new StreamReader(stream))
                {
                    while(!sr.EndOfStream)
                    {
                        ID = sr.ReadLine();
                        name = sr.ReadLine();
                        switch (ID[0])
                        {
                            case '0': 
                                basePower = Convert.ToInt32(sr.ReadLine());
                                type = type.UnitTypes[type.DetermineType(sr.ReadLine())];
                                damageCategory = sr.ReadLine();
                                accuracy = Convert.ToInt32(sr.ReadLine());
                                recoilPercent = Convert.ToInt32(sr.ReadLine());
                                priorityLevel = Convert.ToInt32(sr.ReadLine());
                                effect = sr.ReadLine();
                                triggerPercentage = Convert.ToInt32(sr.ReadLine());
                                move = new TriggerMoves(ID, name, type, accuracy, basePower, damageCategory, recoilPercent, priorityLevel, effect, triggerPercentage);
                                AllMoves.Add(move);
                                break;

                            case '1':
                                basePower = Convert.ToInt32(sr.ReadLine());
                                type = type.UnitTypes[type.DetermineType(sr.ReadLine())];
                                damageCategory = sr.ReadLine();
                                accuracy = Convert.ToInt32(sr.ReadLine());
                                recoilPercent = Convert.ToInt32(sr.ReadLine());
                                priorityLevel = Convert.ToInt32(sr.ReadLine());
                                move = new DamageMoves(ID, name, type, accuracy, basePower, damageCategory, recoilPercent, priorityLevel);
                                AllMoves.Add(move);
                                break;

                            case '2':
                                type = type.UnitTypes[type.DetermineType(sr.ReadLine())];
                                accuracy = Convert.ToInt32(sr.ReadLine());
                                effect = sr.ReadLine();
                                priorityLevel = Convert.ToInt32(sr.ReadLine());
                                move = new StatusMoves(ID, name, type, accuracy, effect, priorityLevel);
                                AllMoves.Add(move);
                                break;
                        }
                    }
                }
            }
        }

        public List<Moves> AllMoves
        {
            get
            {
                return allMoves;
            }
        }
    }
}
