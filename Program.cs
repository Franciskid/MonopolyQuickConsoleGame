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
            Monopoly game = new Monopoly(2);

            game.StartGame();

            while (game.PlayNextPlayerTurn())
            {
                Console.ReadKey();
            }

        }
    }
}
