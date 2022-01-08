using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyQuickConsoleGame
{
    class Dice
    {
        private static Random Rand = new Random(Guid.NewGuid().GetHashCode());

        public int Dice1 { get; set; }

        public int Dice2 { get; set; }


        /// <summary>
        /// Throws the 2 dices.
        /// </summary>
        public void Throw()
        {
            Dice1 = Rand.Next(1, 3);
            Dice2 = Rand.Next(1, 3);
        }


        public bool IsSame => Dice1 == Dice2;

        public int Total => Dice1 + Dice2;


        public void Reset()
        {
            this.Dice1 = 0;
            this.Dice2 = 0;
        }
    }
}
