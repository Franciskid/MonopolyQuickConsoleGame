using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyQuickConsoleGame
{
    class Player : IObservable
    {
        private int position;
        private int prisonTurns;
        private int numberOfDicesSameValue;
        private PlayerState state;

        public int Position
        {
            get => this.position;
            set
            {
                this.position = value % 40;

                this.State = PlayerState.NewPosition;
            }
        }

        public string Name { get; private set; }

        /// <summary>
        /// Amount of turns since in prison.
        /// </summary>
        public int PrisonTurns
        {
            get => this.prisonTurns;
            set
            {
                if (value != 0)
                {
                    this.State = PlayerState.Prison;
                }
                this.prisonTurns = value;
            }
        }


        public bool Prison
        {
            get => this.prisonTurns != 0;
            set
            {
                if (value)
                {
                    this.position = Monopoly.JAIL_POSITION;
                    this.PrisonTurns = 1;
                }
                else
                {
                    this.prisonTurns = 0;
                    this.State = PlayerState.OutOfPrison;
                }
            }
        }
        public int NumberOfDicesSameValue
        {
            get => this.numberOfDicesSameValue;
            set
            {
                this.numberOfDicesSameValue = value;
                if (value != 0)
                {
                    this.State = PlayerState.DicesSameValues;

                    if (value == 3)
                    {
                        this.numberOfDicesSameValue = 0;
                        this.Prison = true;
                    }
                }
            }
        }

        public Dice LastDice { get; } = new Dice();


        public int ID { get; }

        public PlayerState State
        {
            get => this.state;
            set
            {
                this.state = value;
                NotifySubscribers();
            }
        }



        public Player(string name, int id)
        {
            this.Name = name;
            this.ID = id;
        }


        public void RollDices()
        {
            LastDice.Throw();
            this.State = PlayerState.ThrowDices;
        }

        public void Reset()
        {
            this.position = 0;
            this.Prison = false;
            this.LastDice.Reset();

            this.State = PlayerState.Reset;
        }


        private List<IObserver> observers = new List<IObserver>();


        public void Subscribe(IObserver obs)
        {
            Console.WriteLine($"New Observer has just subscribed to the {this.Name} updates");
            this.observers.Add(obs);
        }

        public void Unsubscribe(IObserver obs)
        {
            Console.WriteLine($"{(obs as Player).Name} has just unsubscribed to the player updates");
            this.observers.Add(obs);
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
