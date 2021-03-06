using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyQuickConsoleGame
{
    public interface IObservable
    {
        void AddSubscriber(IObserver obs);

        void RemoveSubscriber(IObserver obs);

        void NotifySubscribers();
    }
}
