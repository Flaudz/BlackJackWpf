﻿using System;
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
    public class SharedViewModel : CommandViewModel
    {
        // Fields
        private Player winner = new("Unknown");
        private Deck deck = new();
        private Card card, dealerTempCard;
        private Player player = new("Unknown");
        private Player dealer = new("Dealer");
        private int beforeMoney = 0;
        private int count;

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

        public int Count { get => count;
            set
            {
                if (count != value)
                {
                    count = value;
                    RaisePropertyChanged("Count");
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
            this.SplitCommand = new SplitCommand(this);
            this.HandHitCommand = new HandHitCommand(this);
            this.SplitStayCommand = new SplitStayCommand(this);
        }

        // Early Game
        public void SetName(string name)
        {
            Player.Name = name;
            BetVisibility = ChangeVisibility();
            Deck.MakeDeck();
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

            Count = Deck.PlayDeck.Count;
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
                    CheckWinner();
                }
            }
            else if(choice == "no")
            {
                DoubbleDownVisibility = "Hidden";
            }
        }


        // Mid Game

        public void Split()
        {
            Player.Hand1.StartCard(Player);
            Player.Hand1.AddCardFromDeck(Deck);

            Player.Money -= Player.Bet;

            Player.Hand2.StartCard(Player);
            Player.Hand2.AddCardFromDeck(Deck);


            SplitVisibility = ChangeVisibility();
        }

        public void Hit()
        {
            Player.AddCard(Deck);

            Count = Deck.PlayDeck.Count;

            if(Player.Value > 21)
            {
                DealDealerTempCard();
                CheckWinner();
            }
        }

        public void Stay()
        {
            DealerTempCardVisibility = "Visible";
            DealersTurn();
            CheckWinner();
        }


        // Late Game
        private void DealersTurn()
        {
            while (Dealer.Value < 17)
            {
                DealDealerTempCard();
                if(Dealer.Cards.Count != 1 && Dealer.Value < 17)
                {
                    Dealer.AddCard(Deck);
                }
            }
            Count = Deck.PlayDeck.Count;
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
            Player.Value = 0;
            HaveAWinner = "false";
            ShowWinner = ChangeVisibility();
            if(Player.Money < 1)
            {
                NoMoney = "false";
            }
        }

        

        // MiniReset will reset the bet, value, cards and insurance so player can start a new round
        public void MiniReset()
        {
            HaveAWinner = "true";
            Winner.Name = "Unknown";

            Player.Bet = 0;
            Player.Value = 0;
            Player.Insurance = 0;
            Player.Hand1.Cards.Clear();
            player.Hand1.Bet = 0;
            Player.Hand2.Cards.Clear();
            Player.Hand2.Bet = 0;
            
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

        protected void DealDealerTempCard()
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
        }


        // Split Game
        public void SplitHit(string input)
        {
            if (input.Contains("Hand1"))
            {
                if(Player.Hand1.Value < 22)
                {
                    Player.Hand1.AddCardFromDeck(Deck);
                }
            }
            else if (input.Contains("Hand2"))
            {
                if(Player.Hand2.Value < 22)
                {
                    Player.Hand2.AddCardFromDeck(Deck);
                }
                else
                {
                    SplitStay(input);
                }
            }
        }

        public void SplitStay(string input)
        {
            if (input.Contains("Hand2"))
            {
                SplitResult();
            }
        }

        protected void SplitResult()
        {
            DealersTurn();
            List<Hand> hands = new();
            hands.Add(Player.Hand1);
            hands.Add(Player.Hand2);

            foreach (Hand hand in hands)
            {
                if (hand.Value > 21 || Dealer.Value > 21)
                {
                    if (hand.Value > 21)
                    {
                        // hand Bustede - Dealer vinder
                        Winner.Name = Dealer.Name;
                    }
                    if (Dealer.Value > 21)
                    {
                        // Dealer Bustede - hand vinder
                        Winner.Name = Player.Name;
                        Player.Money += hand.Bet * 2;
                    }
                }
                else
                {
                    if (hand.CheckBlackJack() || Dealer.CheckBlackJack())
                    {
                        if (Dealer.CheckBlackJack() && hand.CheckBlackJack())
                        {
                            Winner.Name = Dealer.Name;
                        }
                        else
                        {
                            if (hand.CheckBlackJack())
                            {
                                Winner.Name = Player.Name;
                                Player.Money += Convert.ToInt32(Math.Ceiling(hand.Bet * 2.5));
                            }

                            if (Dealer.CheckBlackJack())
                            {
                                Winner.Name = Dealer.Name;
                            }
                        }
                    }
                    else
                    {
                        if (Dealer.Value > hand.Value)
                        {
                            // Dealer Vinder
                            Winner.Name = Dealer.Name;
                        }
                        if (Dealer.Value < hand.Value)
                        {
                            // hand vinder
                            Winner.Name = Player.Name;
                            Player.Money += hand.Bet * 2;
                        }
                        else if (Dealer.Value == hand.Value)
                        {
                            Winner.Name = $"{Player.Name} + {Dealer.Name}";
                            Player.Money += hand.Bet;
                        }
                    }
                }
                hand.Value = 0;
                
                HaveAWinner = "false";
                SplitWinnerVisibility = ChangeVisibility();
            }
        }
    }
}
