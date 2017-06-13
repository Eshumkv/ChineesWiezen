using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineesWiezen.Data
{
    public class Round
    {
        public int RoundNumber { get; set; }
        public Dictionary<Player, int> Guesses { get; set; }
        public Dictionary<Player, int> Tricks { get; set; }
        public int DealerPosition { get; set; }

        public int NextCannotGuess
        {
            get
            {
                int totalGuesses = 0;

                foreach(KeyValuePair<Player, int> kv in Guesses)
                {
                    totalGuesses += kv.Value;
                }
                
                if (totalGuesses <= RoundNumber)
                {
                    return RoundNumber - totalGuesses;
                }
                else
                {
                    return -1;
                }
            }
        }

        public Round(int roundnumber)
        {
            Guesses = new Dictionary<Player, int>();
            Tricks = new Dictionary<Player, int>();
            RoundNumber = roundnumber;
        }
    }
}
