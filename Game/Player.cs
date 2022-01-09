using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyQuickConsoleGame
{
    public class Player : Observable
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
                this.position = value % Monopoly.GAMEBOARD_SIZE;

                this.State = PlayerState.NewPosition;

                if (Position == Monopoly.GO_TO_JAIL_POSITION)
                {
                    State = PlayerState.GoToPrisonPosition;
                    Prison = true;
                }
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
                if (value == 0)
                {
                    this.prisonTurns = value;
                }
                else if (value <= Monopoly.MAX_TURNS_PRISON)
                {
                    this.prisonTurns = value;
                    this.State = PlayerState.Prison;
                }
                else
                {
                    this.Prison = false;
                }
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

                    if (value == Monopoly.MAX_DICE_DOUBLES_BEFORE_PRISON)
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
            this.prisonTurns = 0;
            this.numberOfDicesSameValue = 0;
            this.LastDice.Reset();

            this.State = PlayerState.Reset;
        }

    }
}
