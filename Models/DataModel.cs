using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPrac.Models
{
    public class DataModel
    {
        private int realCount, playerValue, dealerValue;
        private int cardCount;
        private bool shouldStay;

        public int RealCount { get => realCount; set => realCount = value; }
        public int PlayerValue { get => playerValue; set => playerValue = value; }
        public int DealerValue { get => dealerValue; set => dealerValue = value; }
        public bool ShouldStay { get => shouldStay; set => shouldStay = value; }
        public int CardCount { get => cardCount; set => cardCount = value; }
    }
}
