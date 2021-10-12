using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPrac.Models
{
    public class DataModel
    {
        private float myValue, dealersValue, hitOr, realCount;

        public float MyValue { get => myValue; set => myValue = value; }
        public float DealersValue { get => dealersValue; set => dealersValue = value; }
        public float HitOr { get => hitOr; set => hitOr = value; }
        public float RealCount { get => realCount; set => realCount = value; }
    }
}
