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

            
            int payloadSize = 941900; 
            Dictionary<string, int>[] packlists = Distribute(data, packlistBonn, payloadSize, 2); //compute the packlists

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
                Console.WriteLine("total payload used: " + weightCounter + "g/" + payloadSize + "g");
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.WriteLine("total value:" + "\n" + valueCounter);

            #endregion  

            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }


        public static Dictionary<string,int>[] Distribute(EquipmentData equipmentData, Dictionary<string, int> packlist, int payloadSize, int numberOfPayloads)
        {
            //sort the positions of the packlist by its ValuePerWeightRatio and weight
            List<string> positionNames = packlist.Keys
                                        .OrderBy(name => equipmentData[name].ValuePerWeightRatio)
                                        .ThenByDescending(name => equipmentData[name].weight)
                                        .ToList();

            Dictionary<string, int>[] result = new Dictionary<string, int>[numberOfPayloads];

            int payloadIndex = 0;
            int availablePayload = payloadSize;
            int positionIndex = positionNames.Count - 1;
            Equipment currentPosition = equipmentData[positionNames[positionIndex]];

            result[payloadIndex] = new Dictionary<string, int>();

            while (true)
            {
                //compute the maximum quantity wich could now fit inside the container
                int quantity = Math.Min(availablePayload / currentPosition.weight, packlist[currentPosition.name]); 
                if (quantity == 0) //if the equipment does not fit at least once 
                {
                    if (positionIndex > 0)
                    {
                        //go to position with next lower ValuePerWeightRatio
                        positionIndex--;
                        currentPosition = equipmentData[positionNames[positionIndex]];
                        continue;
                    }
                    else
                    {
                        payloadIndex++;
                        if (payloadIndex >= numberOfPayloads)
                            break; //end the algorithm if there is no container left

                        //go to next container
                        result[payloadIndex] = new Dictionary<string, int>();
                        availablePayload = payloadSize;

                        //go to position with the largest ValuePerWeightRatio (last element of sortedNames)
                        positionIndex = positionNames.Count - 1;
                        currentPosition = equipmentData[positionNames[positionIndex]];
                        continue;
                    }
                }

                //add a position to the packlist
                result[payloadIndex].Add(currentPosition.name, quantity);

                //update the requiredQuantity and the availablePayload
                availablePayload -= quantity * currentPosition.weight;
                packlist[currentPosition.name] -= quantity;

                //if full amount of the current equipment could be loadet 
                if (packlist[currentPosition.name] == 0)
                {
                    //remove the position
                    positionNames.RemoveAt(positionIndex);
                    //end the algorithm if there is no other position left
                    if (positionNames.Count == 0)
                        break;
                        
                    //go to position with next lower ValuePerWeightRatio
                    positionIndex--;
                    currentPosition = equipmentData[positionNames[positionIndex]];
                }
            }

            //init empty packlists for the unused containers
            for (int i = payloadIndex + 1; i < result.Length; i++)
                result[i] = new Dictionary<string, int>();

            return result;
        }
    }
}
