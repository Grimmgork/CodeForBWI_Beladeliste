using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayloadDistribution
{
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
