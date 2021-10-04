using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPrac.Models
{
    public class Card
    {
        private string name;
        private int value, cNumber;
        private string imgUrl;

        public string Name { get => name; set => name = value; }
        public int Value { get => value; set => this.value = value; }
        public string ImgUrl { get => imgUrl; set => imgUrl = value; }
        public int CNumber { get => cNumber; set => cNumber = value; }

        public Card(string name, int value, string imgUrl)
        {
            Name = name;
            Value = value;
            ImgUrl = imgUrl;
            cNumber = 0;
        }
    }
}
