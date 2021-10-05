using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WpfPrac.Models;

namespace WpfPrac.Models
{
    public class Player : INotifyPropertyChanged
    {
        // Fields
        private string name;
        private ObservableCollection<Card> cards = new();
        private Hand hand1, hand2 = new();
        private int value, money, bet, insurance;

        // Properties
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        public ObservableCollection<Card> Cards { get => cards; set => cards = value; }
        public Hand Hand1 { get => hand1; set => hand1 = value; }
        public Hand Hand2 { get => hand2; set => hand2 = value; }
        
        public int Value { get => value; set => this.value = value; }
        public int Money { get => money;
            set
            {
                if (money != value)
                {
                    money = value;
                    RaisePropertyChanged("Money");
                }
            }
        }
        public int Bet { get => bet;
            set
            {
                if (bet != value)
                {
                    bet = value;
                    RaisePropertyChanged("Bet");
                }
            }
        }
        public int Insurance { get => insurance; set => insurance = value; }

        // Constructor
        public Player(string name)
        {
            Name = name;
            Value = 0;
            Money = 250;
            Bet = 0;
            Insurance = 0;

            Hand1 = new Hand();
            Hand2 = new Hand();
        }

        // Add Card
        public void AddCard(Deck deck)
        {
            Card card = deck.PickCard();
            Cards.Add(card);
            Value += card.Value;
            this.CheckAce();
        }

        // Check for ace in hand
        public void CheckAce()
        {
            if (this.Value > 21)
            {
                foreach (Card forCard in this.Cards)
                {
                    if (forCard.Value == 11)
                    {
                        forCard.Value = 1;
                        this.Value -= 10;
                        break;
                    }
                }
            }
        }

        public void SetBet(int bet)
        {
            this.Bet = bet;
            this.Money -= bet;
        }

        public bool CheckBlackJack()
        {
            if(this.Cards.Count == 2 && this.Value == 21)
            {
                return true;
            }
            return false;
        }

        // Property Changed
        public event PropertyChangedEventHandler PropertyChanged;

        // This fuction is what informors the thing that it is changed
        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class Hand : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<Card> cards = new();
        private int value, money, bet, insurance;

        // Properties
        public ObservableCollection<Card> Cards { get => cards; set => cards = value; }

        public int Value { get => value; set => this.value = value; }
        public int Money
        {
            get => money;
            set
            {
                if (money != value)
                {
                    money = value;
                    RaisePropertyChanged("Money");
                }
            }
        }
        public int Bet
        {
            get => bet;
            set
            {
                if (bet != value)
                {
                    bet = value;
                    RaisePropertyChanged("Bet");
                }
            }
        }
        public int Insurance { get => insurance; set => insurance = value; }

        // Constructor
        public Hand()
        {
            Value = 0;
            Money = 250;
            Bet = 0;
            Insurance = 0;
        }

        // Add Card's from Player
        public void StartCard(Player player)
        {
            this.Bet = player.Bet;

            this.Cards.Add(player.Cards[0]);
            this.Value = this.Cards[0].Value;

            player.Cards.Remove(player.Cards[0]);
        }

        // Add Card From Deck
        public void AddCardFromDeck(Deck deck)
        {
            Card card = deck.PickCard();
            Cards.Add(card);
            Value += card.Value;
            this.CheckAce();
        }

        // Check for ace in hand
        public void CheckAce()
        {
            if (this.Value > 21)
            {
                foreach (Card forCard in this.Cards)
                {
                    if (forCard.Value == 11)
                    {
                        forCard.Value = 1;
                        this.Value -= 10;
                        break;
                    }
                }
            }
        }

        public void SetBet(int bet)
        {
            this.Bet = bet;
            this.Money -= bet;
        }

        public bool CheckBlackJack()
        {
            if (this.Cards.Count == 2 && this.Value == 21)
            {
                return true;
            }
            return false;
        }

        // Property Changed
        public event PropertyChangedEventHandler PropertyChanged;

        // This fuction is what informors the thing that it is changed
        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
