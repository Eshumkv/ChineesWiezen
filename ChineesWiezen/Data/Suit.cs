using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineesWiezen.Data
{
    public class Suit
    {
        public int Value { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }

        public Suit(string name, int value, string code)
        {
            Name = name;
            Value = value;
            Code = code;
        }

        public static Suit Harten { get { return new Suit("Harten", 1, "♥"); } }
        public static Suit Koeken { get { return new Suit("Koeken", 2, "♦"); } }
        public static Suit Schoppen { get { return new Suit("Schoppen", 3, "♠"); } }
        public static Suit Klaveren { get { return new Suit("Klaveren", 4, "♣"); } }
        public static Suit Geen { get { return new Suit("Geen", 5, "X"); } }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Code, Name);
        }
    }
}
