using ChineesWiezen.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChineesWiezen.Models
{
    public sealed class Score
    {
        public readonly Player Player;
        public readonly int Result;

        public Score(Player player, int result)
        {
            this.Player = player;
            this.Result = result;
        }

        /// <summary>
        /// Fills the score object in based on the state
        /// </summary>
        public static IEnumerable<Score> getScore(GameState state)
        {
            if (state == null)
            {
                throw new ArgumentException("State cannot be NULL!", nameof(state));
            }

            return state.Players
                .Select(player => new Score(player, state.Stats[player].Result))
                .OrderBy(x => -x.Result);
        }
    }
}
