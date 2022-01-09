using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyQuickConsoleGame
{
    public class GameObserver : Observer
    {
        public override void Update(IObservable game)
        {
            if (!(game is Monopoly monop))
                return;

            Player playing = monop.Players[monop.PlayerTurn];

            switch (monop.State)
            {
                case GameState.GameOver:
                    Console.WriteLine("\n\n***********************  THE GAME IS OVER *****************************");
                    break;

                case GameState.GameStarting:
                    Console.WriteLine("***************************************** MONOPOLY ***********************************\n\n");
                    Console.WriteLine("************************ The game is going to start ... Get ready ! ***************************\n\n");
                    break;

                case GameState.NextTurn:
                    Console.WriteLine($"\n----------------------Turn {monop.Turn}---------------------------------\n");
                    break;

                case GameState.PlayerNextTurn:
                    Console.WriteLine($"{playing.Name}, it's your turn to play !");
                    break;
            }
        }
    }
}
