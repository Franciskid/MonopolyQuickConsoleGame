using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyQuickConsoleGame
{
    class Monopoly : IObservable
    {
        public const int JAIL_POSITION = 10;
        public const int GO_TO_JAIL_POSITION = 30;

        /// <summary>
        /// Initialized by the static constructor, even before the instance constructor
        /// </summary>
        private static readonly Monopoly instance = new Monopoly(2, -1);
        private int playerAmount = 2;
        private int playerTurn = 0;
        private int turn;
        private GameState state;


        /// <summary>
        /// Which player it is to play.
        /// </summary>
        public int PlayerTurn
        {
            get => playerTurn;
            set => playerTurn = value % Players.Count;
        }

        /// <summary>
        /// Turn of the game
        /// </summary>
        public int Turn
        {
            get => turn;
            set
            {
                this.turn = value;

                if (value > 0)
                {
                    State = GameState.NextTurn;
                }
            }
        }

        /// <summary>
        /// State of the game.
        /// </summary>
        public GameState State
        {
            get => this.state;
            set
            {
                this.state = value;
                NotifySubscribers();
            }
        }

        /// <summary>
        /// Max amount of turns. This can be set while the game is live. Default value and maximum possible is int.max
        /// </summary>
        public int MaxTurns { get; set; }

        /// <summary>
        /// Players playing
        /// </summary>
        public List<Player> Players { get; private set; }


        /// <summary>
        /// If the game is done or not
        /// </summary>
        public bool GameOver => Turn > MaxTurns;

        /// <summary>
        /// If the game is initialized or not.
        /// </summary>
        private bool IsInitialized => this.Turn != -1;



        private Monopoly(int amntPlayers, int maxTurns = -1)
        {
            this.MaxTurns = maxTurns <= 0 ? int.MaxValue : maxTurns;
            this.Turn = -1;

            playerAmount = amntPlayers;
        }

        /// <summary>
        /// Creates or returns the monopoly game. Only 1 instance
        /// </summary>
        /// <returns></returns>
        public static Monopoly GetMonopolyGame() => instance;



        #region Initializing

        public void SetPlayersAmount(int amnt)
        {
            if (!this.IsInitialized)
            {
                this.Players = new List<Player>(amnt);

                for (int i = 0; i < amnt; i++)
                    this.Players.Add(new Player($"Player {i + 1}", i + 1));

                playerAmount = amnt;

                PlayerObserver obs = new PlayerObserver();
                this.Players.ForEach(x => x.Subscribe(obs));
            }
            else
            {
                Console.WriteLine("Cannot change the amount of player while the game is already initialized or running. Use ResetGame()");
            }
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        /// <param name="timeBetweenEachTurn">-1 if manual, else game is automatic and this value is the time between each player's turn</param>
        public void InitializeGame(int timeBetweenEachTurn = -1)
        {
            ResetGame();

            SetPlayersAmount(this.playerAmount);

            this.State = GameState.GameStarting;

            //PressKey("Press any key to start the game !");

            this.Turn = 0; //Game is now initialized
        }

        /// <summary>
        /// Reset the game. Game needs to be initialized 
        /// </summary>
        public void ResetGame()
        {
            this.Turn = -1;
            this.Players?.ForEach(x => x.Reset());
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void StartGame()
        {
            if (!IsInitialized)
                InitializeGame();

            do
            {
                var Player = Players[PlayerTurn];

                if (Player.ID == 1)
                {
                    this.Turn++;
                    //QuickRecap();
                }

                PressKey("\n\nPress any key to continue to the next player turn !\n");
            }
            while (this.NextTurn());
        }

        #endregion


        /// <summary>
        /// Makes the game play the next player's turn. Stops when the game has reached the max amount of turns there is and returns false, else true
        /// </summary>
        /// <returns>Returns </returns>
        private bool NextTurn()
        {
            if (!IsInitialized)
                throw new Exception($"Game has not yet being initialized... Use Monopoly.InitializeGame()");

            if (GameOver) //Game is over we have reached the max amount of turns
                return false;

            var Player = Players[PlayerTurn];

            State = GameState.PlayerNextTurn;


            int sum = 0;
            if (Player.Prison)
            {
                Player.RollDices();
                if (Player.LastDice.IsSame)
                {
                    Player.Prison = false;
                    Player.Position += Player.LastDice.Total;
                }
                else
                {
                    Player.PrisonTurns++;
                }
            }
            else
            {
                while (!Player.Prison)
                {
                    Player.RollDices();

                    sum += Player.LastDice.Total;

                    if (!Player.LastDice.IsSame)
                    {
                        Player.Position += sum;

                        Player.NumberOfDicesSameValue = 0;

                        break;
                    }
                    else
                    {
                        Player.NumberOfDicesSameValue++;
                    }
                }
            }

            if (Player.Position == GO_TO_JAIL_POSITION)
            {
                Player.State = PlayerState.GoToPrisonPosition;
                Player.Prison = true;
            }

            ++PlayerTurn;

            return true;
        }


        #region Observers

        private List<IObserver> observers = new List<IObserver>();

        public void Subscribe(IObserver obs)
        {
            Console.WriteLine($"{obs} has just subscribed to Monopoly instance");
            this.observers.Add(obs);
        }

        public void Unsubscribe(IObserver obs)
        {
            Console.WriteLine($"{obs} has just unsubscribed to Monopoly instance");
            this.observers.Add(obs);
        }

        public void NotifySubscribers()
        {
            foreach (var observer in this.observers)
            {
                observer.Update(this);
            }
        }

        #endregion


        private void PressKey(string message)
        {
            Console.Write(message);
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}
