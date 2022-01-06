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

        public bool Prison { get; set; }


        public int ID { get; private set; }

        public Player(string name, int id)
        {
            this.Name = name;
            this.ID = id;
        }


        public int RollDices()
        {
            if (Prison)
            {
                Console.WriteLine($"{Name} rolls the dices ...");
                int dice1 = Rand.Next(1, 7);
                int dice2 = Rand.Next(1, 7);

                Console.WriteLine($"  {dice1}  and  {dice2} !");
                if (PrisonTurns == 2)
                {
                    Console.WriteLine("It's being 3 turns ! Next turn you will be out of prison !");
                    Prison = false;
                    PrisonTurns = 0;

                    return 0;
                }
                else if (dice1 == dice2)
                {
                    Console.WriteLine("Same number ! Congrats, you're out of prison !");
                    Prison = false;
                    PrisonTurns = 0;
                    Position += dice1 + dice2;
                    return dice1 + dice2;
                }
                else
                {
                    Console.WriteLine("Better luck next time !");
                    return 0;
                }
            }

            int sum = 0;
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"{Name} rolls the dices ...");
                int dice1 = Rand.Next(1, 7);
                int dice2 = Rand.Next(1, 7);

                sum += dice1 + dice2;

                Console.WriteLine($"  {dice1}  and  {dice2} !");

                if (dice1 != dice2)
                {
                    this.Position += sum;
                    return sum;
                }

                Console.WriteLine("Wow they are the same number, how cool ?!\n");
            }

            Console.WriteLine("3 times in a row ?? You're going to prison my friend !");
            this.Prison = true;

            return sum;
        }

        public void Reset()
        {
            this.Position = 0;
            this.PrisonTurns = 0;
            this.Prison = false;
        }
    }
}
