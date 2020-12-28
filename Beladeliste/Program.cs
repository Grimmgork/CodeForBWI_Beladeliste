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
            Equipment[] context = new Equipment[10] {
                new Equipment("Notebook Büro 13", 205, 2451, 40),
                new Equipment("Notebook Büro 14", 420, 2978, 35),
                new Equipment("Notebook outdoor", 450, 3625, 80),
                new Equipment("Mobiltelefon Büro", 60, 717, 30),
                new Equipment("Mobiltelefon Outdoor", 157, 988, 60),
                new Equipment("Mobiltelefon Heavy Duty", 220, 1220, 65),
                new Equipment("Tablet Büro klein", 620, 20, 4),
                new Equipment("Tablet Büro groß", 250, 15, 3),
                new Equipment("Tablet outdoor klein", 540, 1690, 45),
                new Equipment("Tablet outdoor groß", 370, 1980, 68)
            };

            //mapping the name of each position to a index (to sum up the value at the e)
            Dictionary<string, int> nameIndexMap = new Dictionary<string, int>();
            for(int i = 0; i < context.Length;i++){
                nameIndexMap.Add(context[i].name, i);
            }

            //max available payload of each vehicle 
            int[] payloads = new int[2] { 941900, 941900 };

            //generate a loading-list for each vehicle
            Dictionary<string, int>[] packingLists = Distribute(context, payloads);

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
                    valueCounter += context[nameIndexMap[positionName]].value * quantity;
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


        public static Dictionary<string,int>[] Distribute(Equipment[] context, Dictionary<string, int> packlist, int[] payloads)
        {
            List<Equipment> sorted = context.OrderByDescending(Equipment => Equipment.ValuePerWeightRatio).ThenBy(Equipment => Equipment.weight).ToList();


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
    }
}
