using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProgrammingProjectTest
{
    class CreatureTemplateList
    {
        private List<TemplateCreature> templates;

        public CreatureTemplateList()
        {
            TemplateCreature template;
            int[] baseStats = new int[6];
            string name;
            CreatureType type1 = new CreatureType();
            CreatureType type2;
            string ability;
            string[] moveIDs;

            templates = new List<TemplateCreature>();
            

            //creates a list with all CreatureTemplates from the Creatures file
            using (Stream stream = File.Open("units.txt", FileMode.Open))
            {
                stream.Seek(154, SeekOrigin.Begin);//skips file explanation text
                using (StreamReader sr = new StreamReader(stream))
                {
                    while (!sr.EndOfStream)
                    {
                        baseStats = new int[6];

                        sr.ReadLine();
                        name = sr.ReadLine();
                        type1 = type1.CreatureTypes[type1.DetermineType(sr.ReadLine())];//type1 is being used to access the static values and methods of CreatureType
                        type2 = type1.CreatureTypes[type1.DetermineType(sr.ReadLine())];
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
                        template = new TemplateCreature(baseStats, name, ability, type1, type2, moveIDs);
                        templates.Add(template);
                    }
                }
            }
        }

        public List<TemplateCreature> Templates
        {
            get
            {
                return templates;
            }
        }

    }
}
