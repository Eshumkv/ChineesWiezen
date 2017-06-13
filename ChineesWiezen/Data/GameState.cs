using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineesWiezen.Data
{
    public class GameState
    {
        public HashSet<Player> Players { get; set; }
        public int CurrentRoundNumber { get; private set; }
        public Round CurrentRound
        {
            get
            {
                return rounds.Where(x => x.RoundNumber == CurrentRoundNumber).Single();
            }
        }

        public Suit CurrentSuit
        {
            get
            {
                return SuitOrder[SuitIndex];
            }
        }

        public Player GetNextToGuess
        {
            get
            {
                var position = CurrentRound.DealerPosition + 1;
                if (position >= Players.Count)
                    position = 0;

                var list = Players.OrderBy(p => p.Position);

                for (var i = 0; i < Players.Count; i++)
                {
                    var player = list.ElementAt(position);

                    position += 1;
                    if (position >= Players.Count)
                        position = 0;

                    if (!CurrentRound.Guesses.ContainsKey(player))
                        return player;
                }

                return null;
            }
        }

        public Player GetPreviousToGuess
        {
            get
            {
                var position = CurrentRound.DealerPosition;
                var list = Players.OrderBy(p => p.Position);

                for (var i = 0; i < Players.Count; i++)
                {
                    var player = list.ElementAt(position);

                    position -= 1;
                    if (position < 0)
                        position = Players.Count - 1;

                    if (CurrentRound.Guesses.ContainsKey(player))
                        return player;
                }

                return null;
            }
        }
        
        public bool EveryoneHasGuessed
        {
            get
            {
                return GetNextToGuess == null;
            }
        }

        public Player GetNextToTrick
        {
            get
            {
                var position = CurrentRound.DealerPosition + 1;
                if (position >= Players.Count)
                    position = 0;

                var list = Players.OrderBy(p => p.Position);

                for (var i = 0; i < Players.Count; i++)
                {
                    var player = list.ElementAt(position);

                    position += 1;
                    if (position >= Players.Count)
                        position = 0;

                    if (!CurrentRound.Tricks.ContainsKey(player))
                        return player;
                }

                return null;
            }
        }

        public Player GetPreviousToTrick
        {
            get
            {
                var position = CurrentRound.DealerPosition;
                var list = Players.OrderBy(p => p.Position);

                for (var i = 0; i < Players.Count; i++)
                {
                    var player = list.ElementAt(position);

                    position -= 1;
                    if (position < 0)
                        position = Players.Count - 1;

                    if (CurrentRound.Tricks.ContainsKey(player))
                        return player;
                }

                return null;
            }
        }

        public bool EveryoneHasTricked
        {
            get
            {
                return GetNextToTrick == null;
            }
        }

        public bool NumberOfTricksIsCorrect
        {
            get
            {
                return CurrentRound.Tricks.Sum(x => x.Value) == CurrentRoundNumber;
            }
        }

        public bool IsGameFinished
        {
            get
            {
                return CurrentRoundNumber > numRoundsTillEnd;
            }
        }

        public Player Dealer
        {
            get
            {
                return Players.Where(p => p.Position == CurrentRound.DealerPosition).Single();
            }
        }

        public Player PlayerThatStarts
        {
            get
            {
                var position = CurrentRound.DealerPosition + 1;
                if (position >= Players.Count)
                    position = 0;
                return Players.Where(p => p.Position == position).Single();
            }
        }

        public Dictionary<Player, Statistic> Stats { get; set; } = new Dictionary<Player, Statistic>();

        public Round this[int roundnr]
        {
            get
            {
                return rounds.Where(x => x.RoundNumber == roundnr).Single();
            }
        }

        private List<Round> rounds { get; set; }
        private int numRoundsTillEnd { get; set; }
        private static Random random { get; set; } = new Random();
        private Suit[] SuitOrder { get; set; }
        private int _suit_index;
        private int SuitIndex
        {
            get
            {
                return _suit_index;
            }
            set
            {
                if (value >= SuitOrder.Length)
                    _suit_index = 0;
                else
                    _suit_index = value;
            }
        }

        public GameState()
        {
            Players = new HashSet<Player>();
            rounds = new List<Round>();
        }

        public void NewGame()
        {
            CurrentRoundNumber = 2;
            var order = new List<Suit>() { Suit.Klaveren, Suit.Schoppen, Suit.Harten, Suit.Koeken, Suit.Geen };
            order.Shuffle(random);
            SuitOrder = order.ToArray();
            SuitIndex = 0;
            rounds.Add(new Round(CurrentRoundNumber));
            CurrentRound.DealerPosition = 0;

            foreach (var item in SuitOrder)
            {
                Debug.WriteLine(item);
            }

            foreach (var player in Players)
            {
                Stats.Add(player, new Statistic() { Player = player, Result = 0 });
            }

            numRoundsTillEnd = 52 / Players.Count;
        }

        public void NextRound()
        {
            calculateResults();

            CurrentRoundNumber += 1;
            SuitIndex += 1;
            if (SuitIndex >= SuitOrder.Length)
            {
                SuitIndex = 0;
            }

            rounds.Add(new Round(CurrentRoundNumber));
            CurrentRound.DealerPosition += this[CurrentRoundNumber - 1].DealerPosition + 1;

            if (CurrentRound.DealerPosition >= Players.Count)
            {
                CurrentRound.DealerPosition = 0;
            }
        }

        private void calculateResults()
        {
            foreach (var player in Players)
            {
                if (!CurrentRound.Guesses.ContainsKey(player) || !CurrentRound.Tricks.ContainsKey(player))
                {
                    throw new Exception("Did not enter all needed values!");
                }

                var guess = CurrentRound.Guesses[player];
                var trick = CurrentRound.Tricks[player];
                var result = Math.Min(guess, trick) - Math.Max(guess, trick);

                if (guess == trick)
                    result = guess;

                Stats[player].Result += result;
                Stats[player].PreviousResults.Add(CurrentRoundNumber, result);
            }
        }

        public void AddGuess(Player player, int guess)
        {
            if (guess == CurrentRound.NextCannotGuess)
            {
                throw new Exception("Cannot guess this number!");
            }

            if (CurrentRound.Guesses.ContainsKey(player))
            {
                throw new Exception("This person already guessed!");
            }

            CurrentRound.Guesses.Add(player, guess);
        }

        public void RemovePreviousGuess()
        {
            CurrentRound.Guesses.Remove(GetPreviousToGuess);
        }

        public void AddTrick(Player player, int trick)
        {
            if (CurrentRound.Tricks.ContainsKey(player))
            {
                throw new InvalidOperationException("This person already submitted a trick!");
            }

            if ((CurrentRound.Tricks.Sum(x => x.Value) + trick) > CurrentRoundNumber)
            {
                throw new ArgumentOutOfRangeException(nameof(trick), "sum of tricks is higher than the allowed value!");
            }

            CurrentRound.Tricks.Add(player, trick);
        }

        public void RemovePreviousTrick()
        {
            CurrentRound.Tricks.Remove(GetPreviousToTrick);
        }

        public void MovePlayerUp(int position)
        {
            if (position <= 0)
                return;

            var prevplayers = Players.Where(x => x.Position == position - 1).Single();
            var player = Players.Where(x => x.Position == position).Single();

            player.Position = prevplayers.Position;
            prevplayers.Position = position;
        }

        public void MovePlayerDown(int position)
        {
            if (position >= Players.Count - 1)
                return;

            var nextplayers = Players.Where(x => x.Position == position + 1).Single();
            var player = Players.Where(x => x.Position == position).Single();

            player.Position = nextplayers.Position;
            nextplayers.Position = position;
        }
    }
}
