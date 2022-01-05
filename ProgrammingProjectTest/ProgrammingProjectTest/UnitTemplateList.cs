using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProgrammingProjectTest
{
    class UnitTemplateList
    {
        private List<TemplateUnit> templates;

        public UnitTemplateList()
        {
            TemplateUnit template;
            int[] baseStats = new int[6];
            string name;
            UnitType type1 = new UnitType();
            UnitType type2;
            string ability;
            string[] moveIDs;

            templates = new List<TemplateUnit>();
            

            //creates a list with all unitTemplates from the Units file
            using (Stream stream = File.Open("Units.txt", FileMode.Open))
            {
                stream.Seek(154, SeekOrigin.Begin);//skips file explanation text
                using (StreamReader sr = new StreamReader(stream))
                {
                    while (!sr.EndOfStream)
                    {
                        baseStats = new int[6];

                        sr.ReadLine();
                        name = sr.ReadLine();
                        type1 = type1.UnitTypes[type1.DetermineType(sr.ReadLine())];//type1 is being used to access the
                        type2 = type1.UnitTypes[type1.DetermineType(sr.ReadLine())];//static values and methods of unitType
                        baseStats[5] = Convert.ToInt32(sr.ReadLine());
                        for(int i = 0; i < 5; i++)
                        {
                            baseStats[i] = Convert.ToInt32(sr.ReadLine());
                        }
                        ability = sr.ReadLine();
                        moveIDs = new string[6];
                        for(int i = 0; i < 6; i++)
                        {
                            moveIDs[i] = sr.ReadLine();
                        }
                        template = new TemplateUnit(baseStats, name, ability, type1, type2, moveIDs);
                        templates.Add(template);
                    }
                }
            }
        }

        public List<TemplateUnit> Templates
        {
            get
            {
                return templates;
            }
        }

    }
}
