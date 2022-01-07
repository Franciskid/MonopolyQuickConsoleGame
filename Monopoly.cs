using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyQuickConsoleGame
{
    class Monopoly
    {
        public const int JAIL_POSITION = 10;
        public const int GO_TO_JAIL_POSITION = 30;

        public List<Player> Players { get; private set; }


        private int playerTurn = 0;
        public int PlayerTurn
        {
            get => playerTurn;
            set => playerTurn = value % Players.Count;
        }

        public int Turn { get; private set; }

        public int MaxTurns { get; set; }


        public bool GameOver => Turn > MaxTurns;


        public Monopoly(int amntPlayers, int maxTurns = -1)
        {
            this.Players = new List<Player>(amntPlayers);

            for (int i = 0; i < amntPlayers; i++)
                this.Players.Add(new Player($"Player {i + 1}", i + 1));

            this.MaxTurns = maxTurns <= 0 ? int.MaxValue : maxTurns;
            this.Turn = -1;
        }

        /// <summary>
        /// Starts the game. 
        /// </summary>
        /// <param name="timeBetweenEachTurn">-1 if manual, else game is automatic and this value is the time between each player's turn</param>
        public void StartGame(int timeBetweenEachTurn = -1)
        {
            this.Players.ForEach(x => x.Reset());
            this.Turn = 0;

            Console.WriteLine("************************ The game is going to start ... Get ready ! ***************************\n\n");

            Console.ReadKey();
        }

        /// <summary>
        /// Makes the game play the next player's turn. Stops when the game has reached the max amount of turns there is and returns false, else true
        /// </summary>
        /// <returns>Returns </returns>
        public bool PlayNextPlayerTurn()
        {
            if (Turn == -1)
                throw new Exception($"Game has not yet started... Use Monopoly.StartGame()");

            if (GameOver) //Game is over we have reached the max amount of turns
                return false;

            var Player = Players[PlayerTurn];

            if (Player.ID == 1)
            {
                this.Turn++;
                Console.WriteLine($"\n----------------------Turn {this.Turn}---------------------------------\n");
                QuickRecap();
            }

            Console.WriteLine($"{Player.Name}, it's your turn to play !");


            int sum = 0;
            if (Player.Prison)
            {
                var dices = Player.RollDices();
                if (Player.PrisonTurns == 2)
                {
                    Console.WriteLine("It's being 3 turns ! Next turn you will be out of prison !");
                    Player.Prison = false;
                    Player.PrisonTurns = 0;
                }
                else if (dices.Item1 == dices.Item2)
                {
                    Console.WriteLine("Same number ! Congrats, you're out of prison !");
                    Player.Prison = false;
                    Player.PrisonTurns = 0;
                    Player.Position += (sum = dices.Item1 + dices.Item2);
                }
                else
                {
                    Player.PrisonTurns++;
                    Console.WriteLine("Better luck next time !");
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i == 3)
                    {
                        Console.WriteLine("3 times in a row ?? You're going to prison my friend !");
                        Player.Prison = true;
                        break;
                    }
                    var dices = Player.RollDices();

                    sum += dices.Item1 + dices.Item2;

                    if (dices.Item1 != dices.Item2)
                    {
                        Player.Position += sum;

                        break;
                    }

                    Console.WriteLine("Wow they are the same number, how cool ?!\n");
                }
            }


            if (Player.Prison)
            {
                Console.WriteLine($"{Player.Name} is staying in prison for at least another {3 - Player.PrisonTurns} turn(s).");
            }
            else
            {
                Console.WriteLine($"{Player.Name} is moving {sum} steps forward !\n\nHe is now on position {Player.Position}");
            }


            if (Player.Position == GO_TO_JAIL_POSITION)
            {
                Console.WriteLine("Oh no, you are going to jail !");
                Player.Prison = true;
            }


            ++PlayerTurn;

            Console.WriteLine("\n");

            return true;
        }

        /// <summary>
        /// Makes the game play all player's turn, the amount of turn played is the amount of players there is. Stops when the game has reached the max amount of turns there is and returns false, else true
        /// </summary>
        /// <returns></returns>
        public bool PlayNextBoardTurn()
        {
            for (int i = PlayerTurn; i < PlayerTurn + Players.Count; i++)
            {
                if (!PlayNextPlayerTurn())
                    return false;
            }
            return true;
        }


        private void QuickRecap()
        {
            Console.WriteLine("--- GAME RECAP ---\n");
            Console.WriteLine("Players   ||   Position  ||   Prison?   ||   Prison turn count");
            Players.ForEach(x => Console.WriteLine($"{x.Name}  ||      {x.Position}      ||     {(x.Prison ? "Oui" : "Non")}     ||    {x.PrisonTurns}"));
            Console.WriteLine("--- END RECAP ---\n");
        }
    }
}
