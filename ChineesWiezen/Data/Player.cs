using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineesWiezen.Data
{
    public class Player
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public int Position { get; set; }

        public Player(string name, int position)
        {
            Name = name;
            Points = 0;
            Position = position;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
