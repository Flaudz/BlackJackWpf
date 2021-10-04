using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPrac.Models
{
    public class Visibility
    {
        private string visible;

        public string Visible { get => visible; set => visible = value; }

        public Visibility(string v)
        {
            Visible = v;
        }
    }
}
