using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyQuickConsoleGame
{
    enum PlayerState
    {
        Prison = 0,
        OutOfPrison = 1,
        ThrowDices = 2,
        DicesSameValues = 4,
        Moving = 8,
        NewPosition = 16,
        Reset = 32,
        GoToPrisonPosition = 64,
    }
}
