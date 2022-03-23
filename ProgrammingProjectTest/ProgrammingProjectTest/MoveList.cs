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
            CreatureType type = new CreatureType();
            int accuracy;
            int basePower;
            string damageCategory;
            int recoilPercent;
            int priorityLevel;
            string statusName;
            Status status;
            bool statusTargetsSelf;
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
                                type = type.CreatureTypes[type.DetermineType(sr.ReadLine())];
                                damageCategory = sr.ReadLine();
                                accuracy = Convert.ToInt32(sr.ReadLine());
                                recoilPercent = Convert.ToInt32(sr.ReadLine());
                                priorityLevel = Convert.ToInt32(sr.ReadLine());
                                statusName = sr.ReadLine();
                                if(statusName[0] == '!')
                                {
                                    statusTargetsSelf = true;
                                    statusName = statusName.Substring(1);
                                }
                                else
                                {
                                    statusTargetsSelf = false;
                                }
                                status = DetermineStatus(statusName);
                                triggerPercentage = Convert.ToInt32(sr.ReadLine());
                                move = new TriggerMoves(ID, name, type, accuracy, basePower, damageCategory, recoilPercent, priorityLevel, status, statusTargetsSelf, triggerPercentage);
                                AllMoves.Add(move);
                                break;

                            case '1':
                                basePower = Convert.ToInt32(sr.ReadLine());
                                type = type.CreatureTypes[type.DetermineType(sr.ReadLine())];
                                damageCategory = sr.ReadLine();
                                accuracy = Convert.ToInt32(sr.ReadLine());
                                recoilPercent = Convert.ToInt32(sr.ReadLine());
                                priorityLevel = Convert.ToInt32(sr.ReadLine());
                                move = new DamageMoves(ID, name, type, accuracy, basePower, damageCategory, recoilPercent, priorityLevel);
                                AllMoves.Add(move);
                                break;

                            case '2':
                                type = type.CreatureTypes[type.DetermineType(sr.ReadLine())];
                                accuracy = Convert.ToInt32(sr.ReadLine());
                                statusName = sr.ReadLine();
                                if (statusName[0] == '!')
                                {
                                    statusTargetsSelf = true;
                                    statusName = statusName.Substring(1);
                                }
                                else
                                {
                                    statusTargetsSelf = false;
                                }
                                status = DetermineStatus(statusName);
                                priorityLevel = Convert.ToInt32(sr.ReadLine());
                                move = new StatusMoves(ID, name, type, accuracy, status,statusTargetsSelf, priorityLevel);
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

        public Status DetermineStatus(string statusName)
        {
            Status status = new Status();

            int index = status.DetermineStatusIndex(statusName);

            if(index == -1)
            {
                status = new Status(statusName);
            }
            else
            {
                status = status.GetStatus(index);
            }

            return status;
        }
    }
}
