using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beladeliste
{
    class Program
    {
        static void Main(string[] args)
        {
            //data

            EquipmentData data = new EquipmentData();
            data.Add(new Equipment("Notebook Büro 13", 2451, 40));
            data.Add(new Equipment("Notebook Büro 14", 2978, 35));
            data.Add(new Equipment("Notebook outdoor", 3625, 80));
            data.Add(new Equipment("Mobiltelefon Büro", 717, 30));
            data.Add(new Equipment("Mobiltelefon Outdoor", 988, 60));
            data.Add(new Equipment("Mobiltelefon Heavy Duty", 1220, 65));
            data.Add(new Equipment("Tablet Büro klein", 20, 4));
            data.Add(new Equipment("Tablet Büro groß", 15, 3));
            data.Add(new Equipment("Tablet outdoor klein", 1690, 45));
            data.Add(new Equipment("Tablet outdoor groß", 1980, 68));

            Dictionary<string, int> quantitys = new Dictionary<string, int>();


            //max available payload of each vehicle 
            int[] payloads = new int[2] { 941900, 941900 };

            //generate a loading-list for each vehicle
            Dictionary<string, int>[] packingLists = Distribute(data, quantitys ,payloads);

            int valueCounter = 0;
            
            //visualize results
            for (int i = 0; i < packingLists.Length; i++)
            {
                Dictionary<string, int> packlist = packingLists[i];
                if (packlist == null)
                {
                    Console.WriteLine("List missing!");
                    continue;
                }
                    
                Console.WriteLine("Packing list for transport-" + (i + 1) + ":");
                Console.WriteLine("----------------------------------------");

                List<string> positionNames = packlist.Keys.ToList();
                positionNames.Sort();
                foreach (string positionName in positionNames)
                {
                    int quantity = packlist[positionName];
                    valueCounter += data[positionName].value * quantity;
                    Console.WriteLine(positionName + "\t" + quantity + " Stk.");
                }

                Console.WriteLine();
                Console.WriteLine();
            }

            Console.WriteLine("Total value:" + "\n"+ valueCounter);
            Console.ReadLine();
            Console.ReadLine();

            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }


        public static Dictionary<string,int>[] Distribute(EquipmentData context, Dictionary<string, int> quantitys, int[] payloads)
        {
            //order the names by its ValuePerWeight ratio and then by its weight
            List<string> sorted = context.GetAllNames().OrderByDescending(name => context[name].ValuePerWeightRatio).ThenBy(name => context[name].weight).ToList();

            Dictionary<string, int>[] result = new Dictionary<string, int>[payloads.Length];

            return result;
        }

        public struct Equipment
        {
            public string name { get; private set; }
            public int weight { get; private set; }
            public int value { get; private set; }

            public float ValuePerWeightRatio
            {
                get
                {
                    return (float)value / (float)weight;
                }
            }

            public Equipment(string name, int weight, int value)
            {
                this.name = name;
                this.weight = weight;
                this.value = value;
            }
        }

        public class EquipmentData
        {
            Dictionary<string, Data> data;

            public Equipment this[string positonName]
            {
                get
                {
                    if (!data.ContainsKey(positonName))
                        throw new Exception("name is not present in the database");

                    Data d = data[positonName];
                    return new Equipment(positonName, d.weight, d.value);
                }
            }

            public EquipmentData()
            {
                data = new Dictionary<string, Data>();
            }

            public IEnumerable<string> GetAllNames()
            {
                return data.Keys;
            }

            public void Add(Equipment eq)
            {
                if (data.ContainsKey(eq.name))
                    throw new Exception("name is already present in the database");

                data.Add(eq.name, new Data(eq.weight, eq.value));
            }

            struct Data
            {
                public int weight { get; private set; }
                public int value { get; private set; }

                public Data(int weight, int value)
                {
                    this.weight = weight;
                    this.value = value;
                }
            }
        }
    }
}
