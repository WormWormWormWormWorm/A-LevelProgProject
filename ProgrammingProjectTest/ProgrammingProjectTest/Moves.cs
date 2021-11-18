using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProgrammingProjectTest
{
    abstract class Moves
    {
        public static StreamReader sr;

        protected string moveID;
        protected string moveName;
        protected UnitType moveType;

        protected int moveAccuracy;

        public abstract void Use(Unit Target,Unit User);

        public void PrepareStream(Stream stream, string TemplateID)
        {
            int byteCount;
            int cutoff;

            cutoff = TemplateID.LastIndexOf('#');
            byteCount = Convert.ToInt32(TemplateID.Substring(cutoff + 1));

            stream.Seek(byteCount, SeekOrigin.Begin);
        }

        public void GetType(string typeName)
        {
            UnitType type = new UnitType();
            type = type.UnitTypes[type.DetermineType(typeName)];
            moveType = type;
        }

        public UnitType MoveType
        {
            get
            {
                return moveType;
            }
        }

    }

    class DamageMoves : Moves
    {
        protected int basePower;
        protected string damageCategory;
        protected int recoilPercent;
        protected int priorityLevel;

        public DamageMoves()
        {

        }

        public DamageMoves(string moveID)
        {
            //100's of instances of stream and streamreader left behind if not disposed
            using(Stream stream = File.Open("C:\\TextFiles\\AlevelProgProject\\AttackMoves.txt", FileMode.Open))
            {
                PrepareStream(stream, moveID);

                using(StreamReader sr = new StreamReader(stream))
                {
                    PullDetails(sr);
                }
            }
        }

        public void PullDetails(StreamReader sr)
        {
            sr.ReadLine();
            moveName = sr.ReadLine();
            basePower = Convert.ToInt32(sr.ReadLine());
            GetType(sr.ReadLine());
            damageCategory = sr.ReadLine();
            moveAccuracy = Convert.ToInt32(sr.ReadLine());
            recoilPercent = Convert.ToInt32(sr.ReadLine());
            priorityLevel = Convert.ToInt32(sr.ReadLine());
        }

        public override void Use(Unit Target,Unit User)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return ("Name " + moveName + " type " + moveType.Name + " Accuracy " + moveAccuracy + " bp " + basePower + " category " + damageCategory + " recPerc " + recoilPercent + " priority " + priorityLevel);
        }

    }

    class TriggerMoves : DamageMoves
    {
        string Effect;
        string EffectName;
        int triggerPercentage;

        public TriggerMoves()
        {

        }

        public TriggerMoves(string moveID)
        {
            using (Stream stream = File.Open("C:\\TextFiles\\AlevelProgProject\\AttackMoves.txt", FileMode.Open))
            {
                PrepareStream(stream, moveID);

                using (StreamReader sr = new StreamReader(stream))
                {
                    PullDetails(sr);
                    Effect = sr.ReadLine();
                    triggerPercentage = Convert.ToInt32(sr.ReadLine());
                }
            }
        }

        public override void Use(Unit Target, Unit User)
        {
            base.Use(Target, User);
        }

        public override string ToString()
        {
            return (base.ToString() + " effect " + Effect + " trigPerc " + triggerPercentage) ;
        }

    }

    class StatusMoves : Moves
    {
        protected string status;

        public StatusMoves(string moveID)
        {
            using (Stream stream = File.Open("C:\\TextFiles\\AlevelProgProject\\AttackMoves.txt", FileMode.Open))
            {
                PrepareStream(stream, moveID);

                using (StreamReader sr = new StreamReader(stream))
                {
                    sr.ReadLine();
                    moveName = sr.ReadLine();
                    GetType(sr.ReadLine());
                    moveAccuracy = Convert.ToInt32(sr.ReadLine());
                    status = sr.ReadLine();
                }
            }
        }

        public override void Use(Unit Target,Unit User)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return ("Name " + moveName + " type " + moveType.Name + " Accuracy " + moveAccuracy + " effect " + status);
        }
    }


}
