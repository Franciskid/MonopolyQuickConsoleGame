using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyQuickConsoleGame
{
    public abstract class Observable : IObservable
    {
        protected List<Observer> observers = new List<Observer>();

        public void AddSubscriber(IObserver obs)
        {
            Console.WriteLine($"{obs} has just subscribed to a {this.GetType().Name} instance");
            this.observers.Add(obs as Observer);
        }

        public void RemoveSubscriber(IObserver obs)
        {
            Console.WriteLine($"{obs} has just unsubscribed to a {this.GetType().Name} instance");
            this.observers.Remove(obs as Observer);
        }

        public void NotifySubscribers()
        {
            foreach (var observer in this.observers)
            {
                observer.Update(this);
            }
        }
    }
}
