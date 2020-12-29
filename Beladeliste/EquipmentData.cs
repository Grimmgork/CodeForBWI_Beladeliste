using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayloadDistribution
{
    public class EquipmentData
    {
        /// <summary>
        /// holds the data to describe each equipment. It behaves like a dictionary: returns the data when providet with the name of the equipment.
        /// It implements the array accessor -> []
        /// </summary>
        /// 

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

        private Dictionary<string, Data> data;

        public EquipmentData()
        {
            data = new Dictionary<string, Data>();
        }

        public void Add(Equipment eq)
        {
            if (data.ContainsKey(eq.name))
                throw new Exception("name is already present in the database");

            data.Add(eq.name, new Data(eq.weight, eq.value));
        }

        private struct Data
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

    public struct Equipment
    {
        /// <summary>
        /// describes a equipment
        /// </summary>
        /// 

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
