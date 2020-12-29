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
            EquipmentDatabase data = new EquipmentDatabase();
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

            //the complete packlist for the facility in Bonn
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

            //available payload for each vehicle in gramm
            int[] payloads = new int[2] { 941900, 941900 };

            //compute the packlists
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
                    
                Console.WriteLine("packlist for vehicle " + (i + 1) + ":");
                Console.WriteLine("----------------------------------------");

                List<string> positionNames = packlist.Keys.ToList();
                foreach (string positionName in positionNames)
                {
                    int quantity = packlist[positionName];
                    valueCounter += data[positionName].value * quantity;
                    weightCounter += data[positionName].weight * quantity;
                    Console.WriteLine("~ " + positionName.PadRight(28, ' ') + quantity.ToString().PadLeft(5,' ') + " Stk.");
                }
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("total payload used: " + weightCounter + "g/" + payloads[i] + "g");
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.WriteLine("total value:" + "\n" + valueCounter);

            #endregion  

            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }


        public static Dictionary<string,int>[] Distribute(EquipmentDatabase equipmentData, Dictionary<string, int> packlist, int[] payloads)
        {
            //sort the positions of the packlist by its equipments ValuePerWeight ratio and weight
            List<string> sortedNames = packlist.Keys
                                       .OrderBy(name => equipmentData[name].ValuePerWeightRatio)
                                       .ThenByDescending(name => equipmentData[name].weight)
                                       .ToList();

            Dictionary<string, int>[] result = new Dictionary<string, int>[payloads.Length];

            int payloadIndex = 0;
            int index = sortedNames.Count - 1;
            int availablePayload = payloads[payloadIndex];
            Equipment currentEquipment = equipmentData[sortedNames[index]];
            result[payloadIndex] = new Dictionary<string, int>();

            while (true)
            {
                //compute the maximum quantity wich could now fit inside the container
                int quantity = Math.Min(availablePayload / currentEquipment.weight, packlist[currentEquipment.name]); 
                if (quantity == 0) //if the equipment does not fit at least once 
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
                packlist[currentEquipment.name] -= quantity;

                //if full amount of the current equipment could be loadet 
                if (packlist[currentEquipment.name] == 0)
                {
                    sortedNames.RemoveAt(index);
                    //end the algorithm if there is no equipment position left
                    if (sortedNames.Count == 0)
                        break;

                    //go to equipment with next lower ValuePerWeightRatio
                    index--;
                    currentEquipment = equipmentData[sortedNames[index]];
                }
            }

            return result;
        }
    }
}
