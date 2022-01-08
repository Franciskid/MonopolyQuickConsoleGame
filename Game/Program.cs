using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyQuickConsoleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Monopoly game = Monopoly.GetMonopolyGame();

            GameObserver obs = new GameObserver();
            game.AddSubscriber(obs);

            game.SetPlayersAmount(2);
            game.InitializeGame();

            while (!game.GameOver)
            {
                game.ContinueGame(3);

                game.UpdateView();
            }
        }
    }
}
