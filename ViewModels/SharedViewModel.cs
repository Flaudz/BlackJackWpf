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
    public class SharedViewModel : CommandViewModel
    {
        private DataModel tempData = new();
        private int roundNumber = 0;

        private int realCount = 0;

        public int RealCount { get => realCount;
            set
            {
                if (realCount != value)
                {
                    realCount = value;
                    RaisePropertyChanged("RealCount");
                }
            }
        }

        public DataModel TempData { get => tempData; set => tempData = value; }
        public int RoundNumber { get => roundNumber; set => roundNumber = value; }

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
            this.EnableBotCommand = new EnableBotCommand(this);
            this.BotGoAgainCommand = new BotGoAgainCommand(this);
        }

        // Early Game
        public void SetName(string name)
        {
            MiniReset();
            if (EnabledBot)
            {
                Player.Name = name;
                Deck.MakeDeck();
                if (Deck.PlayDeck.Count == 52)
                    RealCount = 0;
                BotSetBet();
            }
            else
            {
                Player.Name = name;
                BetVisibility = ChangeVisibility();
                Deck.MakeDeck();
            }
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

            if (Player.Cards[0].Value == Player.Cards[1].Value)
                CanSplit = "True";

            Count = Deck.PlayDeck.Count;
            MakeCountFromRound();
            TempData.PlayerValue = Player.Value;
            TempData.DealerValue = Dealer.Value;
            TempData.RealCount = RealCount;

        }

        public async Task DoubbleDown(string choice)
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
                    await DealersTurn();
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
                TempData.ShouldHit = false;
                Data.AddToList(TempData);
                TempData = new();
                DealDealerTempCard();
                CheckWinner();
            }
            else
            {
                TempData.ShouldHit = true;
                Data.AddToList(TempData);
                TempData = new();
                TempData.DealerValue = Dealer.Value;
                TempData.PlayerValue = Player.Value;
                TempData.RealCount = RealCount;
            }
        }

        public async Task Stay()
        {
            DealerTempCardVisibility = "Visible";
            await DealersTurn();
            CheckWinner();
        }

        // Late Game
        private async Task DealersTurn()
        {
            while (Dealer.Value < 17)
            {
                await DealDealerTempCard();
                if (Dealer.Cards.Count != 1 && Dealer.Value < 17)
                {
                    Dealer.AddCard(Deck);
                }
                if(!EnabledBot)
                    await Task.Delay(1000);
            }
            MakeCountFromRound();
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
                    else if(Dealer.Value < Player.Value)
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
            RoundNumber++;
            if(Player.Value < 22)
            {
                if(Winner.Name == Player.Name)
                {
                    TempData.ShouldHit = false;
                }
            }

            Player.Value = 0;
            HaveAWinner = "false";
            ShowWinner = ChangeVisibility();

            if(Player.Money < 1)
            {
                NoMoney = "false";
            }
            
            if(EnabledBot && Player.Money > 1000)
            {
                SetName(Player.Name);
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
            Player.Hand1.Bet = 0;
            Player.Hand2.Cards.Clear();
            Player.Hand2.Bet = 0;
            
            Player.Cards.Clear();

            Dealer.Bet = 0;
            Dealer.Value = 0;
            Dealer.Insurance = 0;
            Dealer.Hand1 = new Hand();
            Dealer.Hand2 = new Hand();
            Dealer.Cards.Clear();

            if(!EnabledBot)
                BetVisibility = ChangeVisibility();
        }

        // This will reset the player completly so the player can start from a new start.
        public void FullReset()
        {
            EnabledBot = false;

            NoMoney = "true";
            HaveAWinner = "true";
            Winner.Name = "Unknown";

            Player.Money = 1250;
            Player.Name = "Unknown";
            Player.Bet = 0;
            Player.Value = 0;
            Player.Insurance = 0;
            Player.Hand1.Cards.Clear();
            Player.Hand1.Bet = 0;
            Player.Hand2.Cards.Clear();
            Player.Hand2.Bet = 0;
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

        protected async Task DealDealerTempCard()
        {
            if (Dealer.Cards.Count == 1)
            {
                Dealer.Cards.Add(DealerTempCard);
                Dealer.Value += DealerTempCard.Value;
                if (Dealer.CheckBlackJack())
                {
                    CheckWinner();
                }
                if(!EnabledBot)
                    await Task.Delay(500);
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

        protected async Task SplitResult()
        {
            await DealersTurn();
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

        // Card Counting Bot
        public void EnableBot()
        {
            if (EnabledBot)
                EnabledBot = false;
            else
                EnabledBot = true;
        }

        protected void MakeCountFromRound()
        {
            foreach (Card card in Player.Cards)
            {
                if(card.Value < 7 && card.Value != 1)
                {
                    RealCount++;
                }
                else if(card.Value > 9 || card.Value == 1)
                {
                    RealCount--;
                }
            }
            foreach (Card card in Dealer.Cards)
            {
                if (card.Value < 7 && card.Value != 1)
                {
                    RealCount++;
                }
                else if (card.Value > 9 || card.Value == 1)
                {
                    RealCount--;
                }
            }
        }

        public void BotSetBet()
        {
            Player.SetBet(1);
            StartDeal();
            BotStayOrHit();
        }

        public void BotStayOrHit()
        {
            while (Player.Value < 22)
            {

                List<DataModel> tempList = new();
                int shouldHitNum = 0;
                if(Data.Data.Count != 0)
                {
                    foreach (var list in Data.Data)
                {
                    if(list[0].DealerValue == Dealer.Value && list[0].PlayerValue == Player.Value && list[0].RealCount == RealCount)
                    {
                        tempList = list;
                    }
                }
                }

                if(tempList.Count != 0)
                {
                    foreach (DataModel data in tempList)
                    {
                        if (data.ShouldHit)
                            shouldHitNum++;
                        else
                            shouldHitNum--;
                    }
                }

                if(shouldHitNum >= 0)
                {
                    Hit();
                    Player.FixZeroError();
                    if (Player.Value > 21)
                        break;
                }
                else if(shouldHitNum < 0)
                {
                    Stay();
                    break;
                }
            }

        }
    }
}
