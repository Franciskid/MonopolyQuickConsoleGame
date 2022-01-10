using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyQuickConsoleGame
{
    public class Monopoly : Observable
    {
        public const int JAIL_POSITION = 10;
        public const int GO_TO_JAIL_POSITION = 30;
        public const int MAX_DICE_DOUBLES_BEFORE_PRISON = 3;
        public const int MAX_TURNS_PRISON = 3;
        public const int GAMEBOARD_SIZE = 40;

        /// <summary>
        /// Initialized by the static constructor, even before the instance constructor
        /// </summary>
        private static readonly Monopoly instance = new Monopoly(2, -1);
        private int playerAmount = 2;
        private int playerTurn = 0;
        private int turn;
        private GameState state;
        private MonopolyView view = new MonopolyView();


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

        private void SetPlayersAmount(int amnt)
        {
            if (!this.IsInitialized)
            {
                this.Players = new List<Player>(amnt);

                for (int i = 0; i < amnt; i++)
                    this.Players.Add(new Player($"Player {i + 1}", i + 1));

                playerAmount = amnt;

                PlayerObserver obs = new PlayerObserver();
                this.Players.ForEach(x => x.AddSubscriber(obs));
            }
            else
            {
                throw new Exception("Cannot change the amount of player while the game is already initialized or running. Use ResetGame()");
            }
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        /// <param name="timeBetweenEachTurn">-1 if manual, else game is automatic and this value is the time between each player's turn</param>
        public void InitializeGame(int amountPlayers = 2)
        {
            this.Turn = -1;

            SetPlayersAmount(amountPlayers);

            this.State = GameState.GameStarting;

            this.Turn = 0; //Game is now initialized
        }

        /// <summary>
        /// Reset the game. Game needs to be initialized 
        /// </summary>
        public void ResetGame()
        {
            this.Players?.ForEach(x => x.Reset());
        }

        #endregion

        /// <summary>
        /// Starts the game or continues to where it was.
        /// </summary>
        /// <param name="breakpause">Number of turns before update</param>
        public void ContinueGame(int breakpause)
        {
            if (!IsInitialized)
                InitializeGame();
            int max = this.Turn + breakpause;
            do
            {
                var Player = Players[PlayerTurn];

                if (Player.ID == 1)
                {
                    this.Turn++;
                }

                PressKey("\n\nPress any key to continue to the next player turn !\n");
            }
            while (this.NextTurn() && (this.Turn < max || PlayerTurn == Players.Count -1) && this.Turn < this.MaxTurns);

            if (this.GameOver)
                State = GameState.GameOver;
        }

        public void UpdateView() => this.view.PrintGameDetails(this);


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

                    Player.Position += Player.LastDice.Total;

                    Player.NumberOfDicesSameValue = !Player.LastDice.IsSame || Player.Prison ? 0 : Player.NumberOfDicesSameValue + 1;

                    if (Player.NumberOfDicesSameValue == MAX_DICE_DOUBLES_BEFORE_PRISON)
                    {
                        Player.NumberOfDicesSameValue = 0;
                        Player.Prison = true;
                    }
                    else if (Player.NumberOfDicesSameValue == 0)
                    {
                        break;
                    }
                }
            }

            ++PlayerTurn;

            return true;
        }


        private void PressKey(string message)
        {
            Console.Write(message);
            Console.ReadKey(true);
            Console.WriteLine();
        }

    }
}
