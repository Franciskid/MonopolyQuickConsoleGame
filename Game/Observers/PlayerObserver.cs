using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyQuickConsoleGame
{
    public class PlayerObserver : Observer
    {

        public override void Update(IObservable player)
        {
            if (!(player is Player pl))
                return;

            switch (pl.State)
            {
                case PlayerState.Prison:
                    Console.WriteLine($"{pl.Name} is in prison for another {Monopoly.MAX_TURNS_PRISON + 1 - pl.PrisonTurns} turn(s).");
                    break;

                case PlayerState.OutOfPrison:
                    Console.WriteLine($"{pl.Name} is no more in prison ! Congrats");
                    break;

                case PlayerState.ThrowDices:
                    Console.WriteLine($"{pl.Name} has thrown the dices, he got {pl.LastDice.Dice1} and {pl.LastDice.Dice2}");
                    break;

                case PlayerState.DicesSameValues:
                    Console.WriteLine($"{pl.Name} has {Monopoly.MAX_DICE_DOUBLES_BEFORE_PRISON - pl.NumberOfDicesSameValue} dices throws before jail !");
                    break;

                case PlayerState.Moving:
                    Console.WriteLine($"{pl.Name} is now moving {pl.LastDice.Total} steps forward.");
                    break;

                case PlayerState.NewPosition:
                    Console.WriteLine($"{pl.Name} is now on position {pl.Position}.");
                    break;

                case PlayerState.GoToPrisonPosition:
                    Console.WriteLine($"Oh no {pl.Name}. This is the go to prison position, bye bye !");
                    break;

                case PlayerState.Reset:
                    Console.WriteLine($"{pl.Name} is now being reinitialized.");
                    break;
            }
        }
    }
}
