using ChineesWiezen.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineesWiezen.Models
{
    public sealed class Details
    {
        public Player Player { get; set; }
        public int Value { get; set; }

        public static void VulCollection(ObservableCollection<Details> vorigegokken, Dictionary<Player, int> dict)
        {
            vorigegokken.Clear();

            foreach (var item in dict)
            {
                vorigegokken.Add(new Details() { Player = item.Key, Value = item.Value });
            }
        }
    }
}
