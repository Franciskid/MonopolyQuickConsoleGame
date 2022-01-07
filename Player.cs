using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyQuickConsoleGame
{
    class Player
    {
        private static Random Rand = new Random(Guid.NewGuid().GetHashCode());

        private int position;

        public int Position
        {
            get => this.position;
            set => this.position = value % 40;
        }

        public string Name { get; private set; }

        /// <summary>
        /// Amount of turns since in prison.
        /// </summary>
        public int PrisonTurns { get; set; }


        private bool prison = false;
        public bool Prison
        {
            get => this.prison;
            set
            {
                if (value)
                    this.position = Monopoly.JAIL_POSITION;

                this.prison = value;
            }
        }


        public int ID { get; private set; }

        public Player(string name, int id)
        {
            this.Name = name;
            this.ID = id;
        }


        public (int, int) RollDices()
        {
            int dice1 = Rand.Next(1, 7);

            int dice2 = Rand.Next(1, 7);

            Console.WriteLine($"{Name} rolls the dices ...");

            Console.WriteLine($"{Name} rolls {dice1}  and  {dice2} !");

            return (dice1, dice2);
        }

        public void Reset()
        {
            this.Position = 0;
            this.PrisonTurns = 0;
            this.Prison = false;
        }
    }
}
