using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyQuickConsoleGame
{
    interface IObservable
    {
        void Subscribe(IObserver obs);

        void Unsubscribe(IObserver obs);

        void NotifySubscribers();
    }
}
