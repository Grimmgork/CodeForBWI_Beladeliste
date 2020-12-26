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

            //mapping the name of each position to a index
            Dictionary<string, int> nameIndexMap = new Dictionary<string, int>();
            for(int i = 0; i < data.Length;i++){
                nameIndexMap.Add(data[i].name, i);
            }

            //max available payload of each vehicle 
            int[] payloads = new int[2] { 941900, 941900 };

            Console.WriteLine("All equipment for Bonn:");
            Console.WriteLine("---------------------------------------------------");
            foreach (Equipment e in data)
            {
                Console.WriteLine(e.name + "\t" + e.requiredQuantity + "\t" + e.weight + "\t" + e.value );
            }
            Console.WriteLine();
            Console.WriteLine();

            //generate a loading-list for each vehicle
            PacklistItem[][] packingLists = Distribute(data, payloads);

            int valueCounter = 0;
            
            //visualize results
            for (int i = 0; i < packingLists.Length; i++)
            {
                PacklistItem[] l = packingLists[i];
                if (l == null)
                {
                    Console.WriteLine("List missing!");
                    continue;
                }
                    
                Console.WriteLine("Packing list for transport-" + (i + 1) + ":");
                Console.WriteLine("----------------------------------------");

                int numberOfPositions = l.Length;
                for(int p = 0; p < numberOfPositions; p++)
                {
                    PacklistItem pos = l[p];
                    valueCounter += data[nameIndexMap[pos.name]].value * pos.quantity;
                    Console.WriteLine(pos.name + "\t" + pos.quantity + " Stk.");
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


        public static PacklistItem[][] Distribute(Equipment[] data, int[] payloads)
        {
            List<Equipment> sortedData = data.OrderByDescending(Equipment => Equipment.ValuePerWeightRatio).ThenBy(Equipment => Equipment.weight).ToList();
            PacklistItem[][] result = new PacklistItem[payloads.Length][];
            Dictionary<string, int> quantities = new Dictionary<string, int>();

            for (int i = 0; i < payloads.Length; i++)
            {
                int availableWeight = payloads[i];
                List<PacklistItem> packList = new List<PacklistItem>();
                
                for(int eqIndex = 0; eqIndex < sortedData.Count; eqIndex++)
                {
                    Equipment eq = sortedData[eqIndex];
                    if (i == 0){ //build the quantity-dictionary on the first traversal of all positions
                        quantities.Add(eq.name, eq.requiredQuantity);
                    }

                    int maxQuantity = availableWeight / eq.weight;

                    if (maxQuantity == 0 || quantities[eq.name] == 0)
                        continue;

                    int quantity = Math.Min(eq.requiredQuantity, maxQuantity);

                    packList.Add(new PacklistItem(eq.name, quantity));
                    availableWeight -= quantity * eq.weight;
                    quantities[eq.name] -= quantity;
                }

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
