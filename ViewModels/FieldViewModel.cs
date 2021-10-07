using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPrac.Models;

namespace WpfPrac.ViewModels
{
    public class FieldViewModel : INotifyPropertyChanged
    {
        // Fields
        private Player winner = new("Unknown");
        private Deck deck = new();
        private Card card, dealerTempCard;
        private Player player = new("Unknown");
        private Player dealer = new("Dealer");
        private int beforeMoney = 0;
        private int count;
        private float roundNubmer = 0;

        private string haveAWinner = "true";
        private string noMoney = "true";
        private string canSplit = "false";

        private bool enabledBot = false;

        public int BeforeMoney
        {
            get => beforeMoney;
            set
            {
                if (beforeMoney != value)
                {
                    beforeMoney = value;
                    RaisePropertyChanged("BeforeMoney");
                }
            }
        }
        public Deck Deck { get => deck; set => deck = value; }
        public Card Card { get => card; set => card = value; }
        public Card DealerTempCard { get => dealerTempCard; set => dealerTempCard = value; }
        public Player Player { get => player; set => player = value; }
        public Player Dealer { get => dealer; set => dealer = value; }
        public Player Winner { get => winner; set => winner = value; }

        public string HaveAWinner
        {
            get => haveAWinner;
            set
            {
                if (haveAWinner != value)
                {
                    haveAWinner = value;
                    RaisePropertyChanged("HaveAWinner");
                }
            }
        }

        public string NoMoney
        {
            get => noMoney;
            set
            {
                if (noMoney != value)
                {
                    noMoney = value;
                    RaisePropertyChanged("NoMoney");
                }
            }
        }
        public string CanSplit
        {
            get => canSplit;
            set
            {
                if (canSplit != value)
                {
                    canSplit = value;
                    RaisePropertyChanged("CanSplit");
                }
            }
        }

        public int Count
        {
            get => count;
            set
            {
                if (count != value)
                {
                    count = value;
                    RaisePropertyChanged("Count");
                }
            }
        }

        public bool EnabledBot
        {
            get => enabledBot;
            set
            {
                if (enabledBot != value)
                {
                    enabledBot = value;
                    RaisePropertyChanged("EnabledBot");
                }
            }
        }

        public float RoundNubmer { get => roundNubmer;
            set
            {
                if (roundNubmer != value)
                {
                    roundNubmer = value;
                    RaisePropertyChanged("RoundNubmer");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This fuction is what informors the thing that it is changed
        public void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
