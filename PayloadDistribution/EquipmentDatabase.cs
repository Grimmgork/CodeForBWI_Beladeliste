using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayloadDistribution
{
    public class EquipmentDatabase
    {
        /// <summary>
        /// holds the data to describe each equipment. It behaves like a dictionary: returns the data when providet with the name of the equipment in O(1) time.
        /// It implements the [] accessor
        /// </summary>
        /// 

        public Equipment this[string positonName]
        {
            get
            {
                Data d = data[positonName];
                return new Equipment(positonName, d.weight, d.value);
            }
        }

        private Dictionary<string, Data> data;

        public EquipmentDatabase()
        {
            data = new Dictionary<string, Data>();
        }

        public void Add(Equipment eq)
        {
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
}
