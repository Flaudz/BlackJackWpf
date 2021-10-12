using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPrac.Models
{
    public class CountModel
    {
        private int count;
        private List<Card> usedCards;

        public int Count { get => count; set => count = value; }
        public List<Card> UsedCards { get => usedCards; set => usedCards = value; }

        public CountModel()
        {
            Count = 0;
            UsedCards = new List<Card>();
        }
    }
}
