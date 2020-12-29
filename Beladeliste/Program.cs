using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayloadDistribution
{
    class Program
    {
        static void Main(string[] args)
        {
            //describe the equipment
            EquipmentData data = new EquipmentData();
            data.Add(new Equipment("Notebook Büro 13", 2451, 40));
            data.Add(new Equipment("Notebook Büro 14", 2978, 35));
            data.Add(new Equipment("Notebook outdoor", 3625, 80));
            data.Add(new Equipment("Mobiltelefon Büro", 717, 30));
            data.Add(new Equipment("Mobiltelefon Outdoor", 988, 60));
            data.Add(new Equipment("Mobiltelefon Heavy Duty", 1220, 65));
            data.Add(new Equipment("Tablet Büro klein", 1405, 40));
            data.Add(new Equipment("Tablet Büro groß", 1455, 40));
            data.Add(new Equipment("Tablet outdoor klein", 1690, 45));
            data.Add(new Equipment("Tablet outdoor groß", 1980, 68));

            //create the packlist for Bonn
            Dictionary<string, int> packlistBonn = new Dictionary<string, int>();
            packlistBonn.Add("Notebook Büro 13", 205);
            packlistBonn.Add("Notebook Büro 14", 420);
            packlistBonn.Add("Notebook outdoor", 450);
            packlistBonn.Add("Mobiltelefon Büro", 60);
            packlistBonn.Add("Mobiltelefon Outdoor", 157);
            packlistBonn.Add("Mobiltelefon Heavy Duty", 220);
            packlistBonn.Add("Tablet Büro klein", 620);
            packlistBonn.Add("Tablet Büro groß", 250);
            packlistBonn.Add("Tablet outdoor klein", 540);
            packlistBonn.Add("Tablet outdoor groß", 370);

            //max available payload of each vehicle in gramm (1100kg - weight of drivers)
            int[] payloads = new int[2] { 941900, 941900 };

            //compute the packlists for the transports
            Dictionary<string, int>[] packlists = Distribute(data, packlistBonn, payloads);

            #region print the packlists

            int valueCounter = 0;
            for (int i = 0; i < packlists.Length; i++)
            {
                int weightCounter = 0;
                Dictionary<string, int> packlist = packlists[i];
                if (packlist == null)
                {
                    Console.WriteLine("list missing!");
                    continue;
                }
                    
                Console.WriteLine("packlist for payload-" + (i + 1) + ":");
                Console.WriteLine("----------------------------------------");

                List<string> positionNames = packlist.Keys.ToList();
                foreach (string positionName in positionNames)
                {
                    int quantity = packlist[positionName];
                    valueCounter += data[positionName].value * quantity;
                    weightCounter += data[positionName].weight * quantity;
                    Console.WriteLine("~ " + positionName.PadRight(30, ' ') + quantity + " Stk.");
                }
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("payload used: " + weightCounter + "g/" + payloads[i] + "g");
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.WriteLine("total value:" + "\n" + valueCounter);
            Console.ReadLine();
            Console.ReadLine();

            Console.WriteLine("Press enter to close...");
            Console.ReadLine();

            #endregion  
        }

        public static Dictionary<string,int>[] Distribute(EquipmentData equipmentData, Dictionary<string, int> requiredQuantities, int[] payloads)
        {
            //order the Equipment-Names by its ValuePerWeight ratio. If the ratios are the same sort them by the weight
            List<string> sortedNames = requiredQuantities.Keys.OrderBy(name => equipmentData[name].ValuePerWeightRatio).ThenByDescending(name => equipmentData[name].weight).ToList();

            Dictionary<string, int>[] result = new Dictionary<string, int>[payloads.Length];

            int payloadIndex = 0;
            int index = sortedNames.Count - 1;
            int availablePayload = payloads[payloadIndex];
            result[payloadIndex] = new Dictionary<string, int>();
            Equipment currentEquipment = equipmentData[sortedNames[index]];

            while (true)
            {
                //compute the maximum quantity wich could fit inside the container
                int quantity = Math.Min(availablePayload / currentEquipment.weight, requiredQuantities[currentEquipment.name]); 
                if (quantity == 0) //if the equipment does not fit at least once in the container
                {
                    if (index > 0)
                    {
                        //go to equipment with next lower ValuePerWeightRatio
                        index--;
                        currentEquipment = equipmentData[sortedNames[index]];
                        continue;
                    }
                    else
                    {
                        payloadIndex++;
                        if (payloadIndex >= payloads.Length)
                            break; //end the algorithm if there is no container left

                        //go to next container
                        result[payloadIndex] = new Dictionary<string, int>();
                        availablePayload = payloads[payloadIndex];

                        //go to equipment with the largest ValuePerWeight ratio (last element of sortedNames)
                        index = sortedNames.Count - 1;
                        currentEquipment = equipmentData[sortedNames[index]];
                        continue;
                    }
                }

                //add the equipment to the resulting packlist
                result[payloadIndex].Add(currentEquipment.name, quantity);

                //update the requiredQuantity and the availablePayload
                availablePayload -= quantity * currentEquipment.weight;
                requiredQuantities[currentEquipment.name] -= quantity;

                //if all of the current equipment could be loadet 
                if (requiredQuantities[currentEquipment.name] == 0)
                {
                    //remove the equipment from the requiredQuantities table and sortedNames (so the list can be searched faster)
                    requiredQuantities.Remove(currentEquipment.name);
                    sortedNames.RemoveAt(index);

                    //go to equipment with next lower ValuePerWeightRatio
                    index--;
                    currentEquipment = equipmentData[sortedNames[index]];

                    //end the algorithm if there is no equipment position left
                    if (requiredQuantities.Count == 0)
                        break;
                }
            }

            return result;
        }
    }
}
