using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfPrac.Models;
using WpfPrac.ViewModels.Commands;

namespace WpfPrac.ViewModels
{
    public class SharedViewModel : INotifyPropertyChanged
    {

        // Fields
        private Player winner = new("Unknown");
        private Deck deck = new();
        private Card card, dealerTempCard;
        private Player player = new("Unknown");
        private Player dealer = new("Dealer");
        private int beforeMoney = 0;

        // Commands
        private LoginCommand loginCommand;
        private StartGameCommand startGameCommand;
        private HitCommand hitCommand;
        private StayCommand stayCommand;
        private GoAgainCommand goAgainCommand;
        private ResetCommand resetCommand;
        private DoubbleDownCommand doubbleDownCommand;

        // Visibility
        private string loginVisibility = "Visible";
        private string betVisibility = "Hidden";
        private string gameVisibility = "Hidden";
        private string dealerTempCardVisibility = "Visible";
        private string showWinner = "Hidden";
        private string doubbleDown = "Hidden";

        private string haveAWinner = "true";
        private string noMoney = "true";

        public int BeforeMoney { get => beforeMoney;
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


        // Visibility
        public string LoginVisibility { get => loginVisibility;
            set
            {
                if (loginVisibility != value)
                {
                    loginVisibility = value;
                    RaisePropertyChanged("LoginVisibility");
                }
            }
        }
        public string BetVisibility { get => betVisibility;
            set
            {
                if (betVisibility != value)
                {
                    betVisibility = value;
                    RaisePropertyChanged("BetVisibility");
                }
            }
        }
        public string GameVisibility { get => gameVisibility;
            set
            {
                if (gameVisibility != value)
                {
                    gameVisibility = value;
                    RaisePropertyChanged("GameVisibility");
                }
            }
        }
        public string ShowWinner { get => showWinner;
            set
            {
                if (showWinner != value)
                {
                    showWinner = value;
                    RaisePropertyChanged("ShowWinner");
                }
            }
        }
        public string DealerTempCardVisibility
        {
            get => dealerTempCardVisibility;
            set
            {
                if (dealerTempCardVisibility != value)
                {
                    dealerTempCardVisibility = value;
                    RaisePropertyChanged("DealerTempCardVisibility");
                }
            }
        }

        public string HaveAWinner { get => haveAWinner;
            set
            {
                if (haveAWinner != value)
                {
                    haveAWinner = value;
                    RaisePropertyChanged("HaveAWinner");
                }
            }
        }
        public string DoubbleDownVisibility { get => doubbleDown;
            set
            {
                if (doubbleDown != value)
                {
                    doubbleDown = value;
                    RaisePropertyChanged("DoubbleDownVisibility");
                }
            }
        }


        // Commands
        public LoginCommand LoginCommand { get => loginCommand; set => loginCommand = value; }
        public StartGameCommand StartGameCommand { get => startGameCommand; set => startGameCommand = value; }
        public HitCommand HitCommand { get => hitCommand; set => hitCommand = value; }
        public StayCommand StayCommand { get => stayCommand; set => stayCommand = value; }
        public GoAgainCommand GoAgainCommand { get => goAgainCommand; set => goAgainCommand = value; }
        public ResetCommand ResetCommand { get => resetCommand; set => resetCommand = value; }
        public DoubbleDownCommand DoubbleDownCommand { get => doubbleDownCommand; set => doubbleDownCommand = value; }

        public string NoMoney { get => noMoney;
            set
            {
                if (noMoney != value)
                {
                    noMoney = value;
                    RaisePropertyChanged("NoMoney");
                }
            }
        }






        // Constructor
        public SharedViewModel()
        {
            this.StartGameCommand = new StartGameCommand(this);
            this.LoginCommand = new LoginCommand(this);
            this.HitCommand = new HitCommand(this);
            this.StayCommand = new StayCommand(this);
            this.GoAgainCommand = new GoAgainCommand(this);
            this.ResetCommand = new ResetCommand(this);
            this.DoubbleDownCommand = new DoubbleDownCommand(this);
        }

        // Early Game
        public void SetName(string name)
        {
            Player.Name = name;
            BetVisibility = ChangeVisibility();
        }

        public void Start(int bet)
        {
            if(bet < Player.Money + 1)
            {
                Player.SetBet(bet);
                if (Player.Money >= Player.Bet)
                    DoubbleDownVisibility = "Visible";
                GameVisibility = ChangeVisibility();
                // Give starter card
                Deck.MakeDeck();

                // Start Deal
                StartDeal();
            }
        }

        private void StartDeal()
        {
            BeforeMoney = Player.Money + Player.Bet;

            Dealer.AddCard(Deck);

            Player.AddCard(Deck);

            DealerTempCard = Deck.PickCard();

            Player.AddCard(Deck);
        }

        public void DoubbleDown(string choice)
        {
            if(choice == "yes")
            {
                Player.Money -= Player.Bet;
                Player.Bet *= 2;
                Hit();
                if(Player.Value > 21)
                {
                    CheckWinner();
                }
                else
                {
                    DealersTurn();
                }
            }
            else if(choice == "no")
            {
                DoubbleDownVisibility = "Hidden";
            }
        }


        // Mid Game
        public void Hit()
        {
            Player.AddCard(Deck);

            if(Player.Value > 21)
            {
                CheckWinner();
            }
        }

        public void Stay()
        {
            DealerTempCardVisibility = "Visible";
            DealersTurn();
        }


        // Late Game
        private void DealersTurn()
        {
            while (Dealer.Value < 17)
            {
                if (Dealer.Cards.Count == 1)
                {
                    Dealer.Cards.Add(DealerTempCard);
                    Dealer.Value += DealerTempCard.Value;
                    if (Dealer.CheckBlackJack())
                    {
                        CheckWinner();
                    }
                }
                else
                {
                    Dealer.AddCard(Deck);
                }
            }
            CheckWinner();
        }

        private void CheckWinner()
        {
            if(Player.Value > 21 || Dealer.Value > 21)
            {
                if(Player.Value > 21)
                {
                    // Player Bustede - Dealer vinder
                    Winner.Name = Dealer.Name;
                }
                if(Dealer.Value > 21)
                {
                    // Dealer Bustede - Player vinder
                    Winner.Name = Player.Name;
                    Player.Money += Player.Bet * 2;
                }
            }
            else
            {
                if(Player.CheckBlackJack() || Dealer.CheckBlackJack())
                {
                    if (Dealer.CheckBlackJack() && Player.CheckBlackJack())
                    {
                        Winner.Name = Dealer.Name;
                    }
                    else
                    {
                        if (Player.CheckBlackJack())
                        {
                            Winner.Name = Player.Name;
                            Player.Money += Convert.ToInt32(Math.Ceiling(Player.Bet * 2.5));
                        }

                        if (Dealer.CheckBlackJack())
                        {
                            Winner.Name = Dealer.Name;
                        }
                    }
                }
                else
                {
                    if (Dealer.Value > Player.Value)
                    {
                        // Dealer Vinder
                        Winner.Name = Dealer.Name;
                    }
                    if(Dealer.Value < Player.Value)
                    {
                        // Player vinder
                        Winner.Name = Player.Name;
                        Player.Money += Player.Bet * 2;
                    }
                    else if(Dealer.Value == Player.Value)
                    {
                        Winner.Name = $"{Player.Name} + {Dealer.Name}";
                        Player.Money += Player.Bet;
                    }
                }
            }
            HaveAWinner = "false";
            ShowWinner = ChangeVisibility();
            if(Player.Money < 1)
            {
                NoMoney = "false";
            }
        }

        public string ChangeVisibility()
        {
            LoginVisibility = "Hidden";
            GameVisibility = "Hidden";
            BetVisibility = "Hidden";
            ShowWinner = "Hidden";

            return "Visible";

        }

        // MiniReset will reset the bet, value, cards and insurance so player can start a new round
        public void MiniReset()
        {
            HaveAWinner = "true";
            Winner.Name = "Unknown";

            Player.Bet = 0;
            Player.Value = 0;
            Player.Insurance = 0;
            Player.Hand1 = new Hand();
            Player.Hand2 = new Hand();
            Player.Cards.Clear();

            Dealer.Bet = 0;
            Dealer.Value = 0;
            Dealer.Insurance = 0;
            Dealer.Hand1 = new Hand();
            Dealer.Hand2 = new Hand();
            Dealer.Cards.Clear();

            BetVisibility = ChangeVisibility();
        }

        // This will reset the player completly so the player can start from a new start.
        public void FullReset()
        {
            NoMoney = "true";
            HaveAWinner = "true";
            Winner.Name = "Unknown";

            Player.Money = 250;
            Player.Name = "Unknown";
            Player.Bet = 0;
            Player.Value = 0;
            Player.Insurance = 0;
            Player.Hand1 = new Hand();
            Player.Hand2 = new Hand();
            Player.Cards.Clear();

            Dealer.Money = 250;
            Dealer.Name = "Dealer";
            Dealer.Bet = 0;
            Dealer.Value = 0;
            Dealer.Insurance = 0;
            Dealer.Hand1 = new Hand();
            Dealer.Hand2 = new Hand();
            Dealer.Cards.Clear();



            LoginVisibility = ChangeVisibility();
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
