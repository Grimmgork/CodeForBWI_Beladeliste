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
            Equipment[] data = new Equipment[10] {
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

            //max available payload of each vehicle 
            int[] payloads = new int[2] { 941900, 941900 };

            Console.WriteLine("All equipment:");
            Console.WriteLine("---------------------------------------------------");
            foreach (Equipment e in data)
            {
                Console.WriteLine(e.name + "\t" + e.requiredQuantity + "\t" + e.weight + "\t" + e.value );
            }
            Console.WriteLine();
            Console.WriteLine();

            //generate a loading-list for each vehicle
            PacklistItem[][] packingLists = Distribute(data, payloads);

            
            //visualize results
            for (int i = 0; i < packingLists.Length; i++)
            {
                PacklistItem[] l = packingLists[i];
                if (l == null)
                {
                    Console.WriteLine("List missing!");
                    continue;
                }
                    
                Console.WriteLine("Packing list " + (i + 1) + ":");
                Console.WriteLine("----------------------------------------");

                int numberOfPositions = l.Length;
                for(int p = 0; p < numberOfPositions; p++)
                {
                    PacklistItem pos = l[p];
                    Console.WriteLine(pos.name + "\t" + pos.quantity + " Stk.");
                }

                Console.WriteLine();
                Console.WriteLine();
            }

            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }


        public static PacklistItem[][] Distribute(Equipment[] data, int[] payloads)
        {
            List<Equipment> sortedData = data.OrderByDescending(Equipment => Equipment.ValuePerWeightRatio).ThenBy(Equipment => Equipment.weight).ToList();
            PacklistItem[][] result = new PacklistItem[payloads.Length][];

            int eqIndex = 0;
            for (int i = 0; i < payloads.Length; i++)
            {
                int availableWeight = payloads[i];

                Equipment eq = sortedData[eqIndex];
                int requiredQuantity = eq.requiredQuantity;
                
                List<PacklistItem> packList = new List<PacklistItem>();
                
                while()

                result[i] = packList.ToArray();
            }

            return result;
        }

        public struct PacklistItem
        {
            public string name { get; private set; }
            public int quantity { get; private set; }

            public PacklistItem(string name, int quantity)
            {
                this.name = name;
                this.quantity = quantity;
            }
        }

        public struct Equipment
        {
            public string name { get; private set; }
            public int requiredQuantity { get; private set; }
            public int weight { get; private set; }
            public int value { get; private set; }

            public float ValuePerWeightRatio
            {
                get
                {
                    return (float)value / (float)weight;
                }
            }

            public Equipment(string name, int requiredQuantity, int weight, int value)
            {
                this.name = name;
                this.requiredQuantity = requiredQuantity;
                this.weight = weight;
                this.value = value;
            }
        }
    }
}
