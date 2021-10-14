using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPrac.Models
{
    public class DataModelList
    {
        private List<List<DataModel>> data = new();
        public DataModelList() { }

        public List<List<DataModel>> Data { get => data; set => data = value; }

        public void AddToList(DataModel data)
        {
            bool didInsert = false;
            foreach (var list in Data)
            {
                if(list[0].DealerValue == data.DealerValue && list[0].PlayerValue == data.PlayerValue)
                {
                    list.Add(data);
                    didInsert = true;
                    break;
                }
            }
            if (!didInsert)
            {
                List<DataModel> temp = new();
                temp.Add(data);
                Data.Add(temp);
            }
        }
    }

    public class DataModel
    {
        private int playerValue, dealerValue, realCount;
        private bool shouldHit;
        public DataModel()
        {

        }

        public int PlayerValue { get => playerValue; set => playerValue = value; }
        public int DealerValue { get => dealerValue; set => dealerValue = value; }
        public int RealCount { get => realCount; set => realCount = value; }
        public bool ShouldHit { get => shouldHit; set => shouldHit = value; }
    }
}
