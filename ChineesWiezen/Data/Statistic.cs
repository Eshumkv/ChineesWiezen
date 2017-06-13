using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineesWiezen.Data
{
    public class Statistic
    {
        public Player Player { get; set; }
        public int Result { get; set; }
        public Dictionary<int, int> PreviousResults { get; set; } = new Dictionary<int, int>();
    }
}
