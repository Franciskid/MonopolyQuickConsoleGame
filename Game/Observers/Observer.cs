using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyQuickConsoleGame
{
    public abstract class Observer : IObserver
    {
        public abstract void Update(IObservable game);
    }
}
